from sqlalchemy.orm import Session
from app import models, schemas
from passlib.context import CryptContext
from fastapi import HTTPException, status

pwd_context = CryptContext(schemes=["bcrypt"], deprecated="auto")



# AUTH / USER CREATION

def get_password_hash(password: str):
    return pwd_context.hash(password)


def create_user(db: Session, user: schemas.UserCreate):
    # Hash password
    hashed_pw = get_password_hash(user.password)

    # Create user entry
    db_user = models.User(
        username=user.username,
        email=user.email,
        password_hash=hashed_pw
    )

    # Save user to DB
    db.add(db_user)
    db.commit()
    db.refresh(db_user)

    # ✅ Automatically create player stats for this user
    initial_stats = models.PlayerStats(
        user_id=db_user.user_id,
        level=1,
        xp=0,
        proficiency=0
    )
    db.add(initial_stats)
    db.commit()

    return db_user


def verify_password(plain_password: str, hashed_password: str):
    return pwd_context.verify(plain_password, hashed_password)


def authenticate_user(db: Session, username_or_email: str, password: str):
    # Try username first, then email
    user = db.query(models.User).filter(
        (models.User.username == username_or_email) |
        (models.User.email == username_or_email)
    ).first()

    if not user:
        raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail="User not found")

    if not verify_password(password, user.password_hash):
        raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="Incorrect password")

    return user



# PLAYER PROGRESSION FUNCTIONS

def get_player_stats(db: Session, user_id: int):
    stats = db.query(models.PlayerStats).filter(models.PlayerStats.user_id == user_id).first()
    if not stats:
        raise HTTPException(status_code=404, detail="Player stats not found")
    return stats


def add_xp(db: Session, user_id: int, xp_gain: int):
    stats = get_player_stats(db, user_id)
    stats.xp += xp_gain

    # Level up every 100 XP (adjust as you like)
    while stats.xp >= stats.level * 100:
        stats.xp -= stats.level * 100
        stats.level += 1

    db.commit()
    db.refresh(stats)
    return stats


def update_proficiency(db: Session, user_id: int, change: int):
    stats = get_player_stats(db, user_id)
    stats.proficiency = max(0, min(100, stats.proficiency + change))  # Clamp 0–100
    db.commit()
    db.refresh(stats)
    return stats



# LEADERBOARD FUNCTIONS

def get_leaderboard(db: Session, limit: int = 10):
    """Return the top players sorted by level, xp, then proficiency."""
    results = (
        db.query(models.User.username,
                 models.PlayerStats.level,
                 models.PlayerStats.xp,
                 models.PlayerStats.proficiency)
        .join(models.PlayerStats, models.User.user_id == models.PlayerStats.user_id)
        .order_by(models.PlayerStats.level.desc(),
                  models.PlayerStats.xp.desc(),
                  models.PlayerStats.proficiency.desc())
        .limit(limit)
        .all()
    )
    return results


# QUEST RESULT HANDLER (Updated)

def record_quest_result(
    db: Session,
    user_id: int,
    difficulty: str,
    lives_left: int
):
    """
    Updates XP and proficiency automatically when a quest or quiz is completed.
    XP is based on quest difficulty.
    Proficiency change is based on remaining lives.
    """

    stats = get_player_stats(db, user_id)

    # --- XP Gain Based on Difficulty
    if difficulty == "easy":
        xp_gain = 10
    elif difficulty == "medium":
        xp_gain = 25
    elif difficulty == "hard":
        xp_gain = 50
    else:
        xp_gain = 0  # fallback safety

    stats.xp += xp_gain

    # --- Level Up Every 100 * Current Level
    while stats.xp >= stats.level * 100:
        stats.xp -= stats.level * 100
        stats.level += 1

    # --- Proficiency Change Based on Lives Left
    if lives_left == 3:
        prof_change = 3
    elif lives_left == 2:
        prof_change = 2
    elif lives_left == 1:
        prof_change = 1
    else:  # died / 0 lives
        prof_change = -3

    stats.proficiency = max(0, min(100, stats.proficiency + prof_change))

    db.commit()
    db.refresh(stats)
    return {
        "user_id": stats.user_id,
        "level": stats.level,
        "xp_gain": xp_gain,
        "xp_total": stats.xp,
        "proficiency_change": prof_change,
        "proficiency_total": stats.proficiency
    }

