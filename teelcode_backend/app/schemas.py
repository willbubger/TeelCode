from pydantic import BaseModel, EmailStr

class UserCreate(BaseModel):
    username: str
    email: EmailStr
    password: str

class UserOut(BaseModel):
    user_id: int
    username: str
    email: EmailStr

    class Config:
        orm_mode = True
class UserLogin(BaseModel):
    username_or_email: str
    password: str
class PlayerStats(BaseModel):
    level: int
    xp: int
    proficiency: int

    class Config:
        from_attributes = True


class XPUpdate(BaseModel):
    xp_gain: int


class ProficiencyUpdate(BaseModel):
    change: int

class LeaderboardEntry(BaseModel):
    username: str
    level: int
    xp: int
    proficiency: int

    class Config:
        from_attributes = True


# QUEST RESULT SCHEMA

class QuestResult(BaseModel):
    user_id: int
    difficulty: str
    lives_left: int
