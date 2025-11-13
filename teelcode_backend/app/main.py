from fastapi import FastAPI, Depends, HTTPException
from sqlalchemy.orm import Session
from app.database import get_db, engine
from app import models, schemas, crud

# INITIAL SETUP

# Initialize tables if not already created
models.init_models()

# Create the FastAPI app
app = FastAPI(title="TeelCode Backend")


# HEALTH CHECK

@app.get("/health")
def health_check():
    try:
        with engine.connect() as conn:
            return {"status": "ok", "message": "Connected to database!"}
    except Exception as e:
        return {"status": "error", "message": str(e)}


# AUTH / USER REGISTRATION

@app.post("/users/register", response_model=schemas.UserOut)
def register_user(user: schemas.UserCreate, db: Session = Depends(get_db)):
    # Check if username or email already exists
    existing = db.query(models.User).filter(
        (models.User.username == user.username) |
        (models.User.email == user.email)
    ).first()
    if existing:
        raise HTTPException(status_code=400, detail="Username or email already taken")

    return crud.create_user(db, user)


@app.post("/users/login", response_model=schemas.UserOut)
def login_user(credentials: schemas.UserLogin, db: Session = Depends(get_db)):
    user = crud.authenticate_user(
        db,
        username_or_email=credentials.username_or_email,
        password=credentials.password
    )
    return user


# PLAYER PROGRESSION ROUTES

@app.get("/player/{user_id}")
def get_stats(user_id: int, db: Session = Depends(get_db)):
    """
    Returns player stats for HUD display:
    XP, Level, Proficiency, and Login Streak.
    """
    stats = crud.get_player_stats(db, user_id)
    user = db.query(models.User).filter(models.User.user_id == user_id).first()

    if not user:
        raise HTTPException(status_code=404, detail="User not found")

    return {
        "user_id": user.user_id,
        "level": stats.level,
        "xp": stats.xp,
        "proficiency": stats.proficiency,
        "login_streak": user.login_streak,
        "last_login": user.last_login
    }


@app.post("/player/{user_id}/update_xp", response_model=schemas.PlayerStats)
def update_xp(user_id: int, data: schemas.XPUpdate, db: Session = Depends(get_db)):
    return crud.add_xp(db, user_id, data.xp_gain)


@app.post("/player/{user_id}/update_proficiency", response_model=schemas.PlayerStats)
def update_proficiency(user_id: int, data: schemas.ProficiencyUpdate, db: Session = Depends(get_db)):
    return crud.update_proficiency(db, user_id, data.change)


# LEADERBOARD ROUTE

@app.get("/leaderboard", response_model=list[schemas.LeaderboardEntry])
def leaderboard(limit: int = 10, db: Session = Depends(get_db)):
    """Return the top players (default top 10)."""
    return crud.get_leaderboard(db, limit)


# QUEST RESULT ROUTE

@app.post("/player/quest_result")
def quest_result(data: schemas.QuestResult, db: Session = Depends(get_db)):
    """
    Called by Unity when a player finishes a quest.
    Automatically updates XP, Level, and Proficiency based on difficulty and lives.
    """
    return crud.record_quest_result(
        db,
        user_id=data.user_id,
        category=data.category,
        difficulty=data.difficulty,
        lives_left=data.lives_left
    )


# PROGRESS TRACKING ROUTE 

@app.get("/users/{user_id}/progress")
def get_progress(user_id: int, db: Session = Depends(get_db)):
    """
    Returns a dictionary of category progress.
    Example:
    {
        "DataStructures": {"completed": 2, "total": 3},
        "Algorithms": {"completed": 1, "total": 3}
    }
    """
    return crud.get_user_progress(db, user_id)


# FRIEND SYSTEM ROUTES

@app.post("/users/{user_id}/add_friend/{friend_username}")
def add_friend(user_id: int, friend_username: str, db: Session = Depends(get_db)):
    """
    Add a friend by username (auto-accept, no request system yet).
    """
    return crud.add_friend_by_username(db, user_id, friend_username)


@app.get("/users/{user_id}/friends")
def get_friends(user_id: int, db: Session = Depends(get_db)):
    """
    View all friends for a given user.
    """
    return crud.get_friends_list(db, user_id)


# ENTRY POINT (Cloud Run / Local)

if __name__ == "__main__":
    import uvicorn
    import os

    # Cloud Run automatically provides a PORT environment variable
    port = int(os.environ.get("PORT", 8080))
    uvicorn.run(app, host="0.0.0.0", port=port)
