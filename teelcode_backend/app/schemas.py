from pydantic import BaseModel, EmailStr, Field, constr


# USER SCHEMAS

class UserCreate(BaseModel):
    username: constr(min_length=3, max_length=20)
    email: EmailStr
    password: constr(min_length=6, max_length=64)


class UserLogin(BaseModel):
    username_or_email: str = Field(..., min_length=3)
    password: str = Field(..., min_length=6)


class UserOut(BaseModel):
    user_id: int
    username: str
    email: EmailStr

    class Config:
        from_attributes = True  # Replaces orm_mode for Pydantic v2


# PLAYER STATS SCHEMAS

class PlayerStats(BaseModel):
    level: int
    xp: int
    proficiency: int

    class Config:
        from_attributes = True


class XPUpdate(BaseModel):
    xp_gain: int = Field(..., ge=0, description="Amount of XP to add")


class ProficiencyUpdate(BaseModel):
    change: int = Field(..., description="Change in proficiency (can be negative)")


#  LEADERBOARD SCHEMA

class LeaderboardEntry(BaseModel):
    username: str
    level: int
    xp: int
    proficiency: int

    class Config:
        from_attributes = True


#  QUEST RESULT SCHEMA

class QuestResult(BaseModel):
    user_id: int
    category: str
    difficulty: str = Field(..., pattern="^(easy|medium|hard)$")
    lives_left: int = Field(..., ge=0, le=3, description="Remaining lives after quest")
