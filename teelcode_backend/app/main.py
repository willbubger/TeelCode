from fastapi import FastAPI, Depends, HTTPException
from sqlalchemy.orm import Session
from app.database import get_db, engine
from app import models, schemas, crud

# Initialize tables if not already created
models.init_models()

# Create the FastAPI app
app = FastAPI(title="TeelCode Backend")


# Health check endpoint

@app.get("/health")
def health_check():
    try:
        with engine.connect() as conn:
            return {"status": "ok", "message": "Connected to database!"}
    except Exception as e:
        return {"status": "error", "message": str(e)}

# Register a new user
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


# Login user (verify credentials)
@app.post("/users/login", response_model=schemas.UserOut)
def login_user(credentials: schemas.UserLogin, db: Session = Depends(get_db)):
    user = crud.authenticate_user(
        db,
        username_or_email=credentials.username_or_email,
        password=credentials.password
    )
    return user

# PLAYER PROGRESSION ROUTES

@app.get("/player/{user_id}", response_model=schemas.PlayerStats)
def get_stats(user_id: int, db: Session = Depends(get_db)):
    return crud.get_player_stats(db, user_id)


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

@app.post("/player/quest_result")
def quest_result(data: schemas.QuestResult, db: Session = Depends(get_db)):
    """
    Called by Unity (or Java) when a player finishes a quest.
    Automatically updates XP, Level, and Proficiency based on difficulty and lives.
    """
    return crud.record_quest_result(
        db,
        user_id=data.user_id,
        difficulty=data.difficulty,
        lives_left=data.lives_left
    )
