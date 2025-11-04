from sqlalchemy import Column, Integer, String, Text, Boolean, Date, ForeignKey, Enum, JSON, TIMESTAMP
from sqlalchemy.orm import relationship
from app.database import Base
import enum



# TABLE MODELS

class User(Base):
    __tablename__ = "users"

    user_id = Column(Integer, primary_key=True, index=True)
    username = Column(String(50), unique=True, nullable=False)
    email = Column(String(100), unique=True, nullable=False)
    password_hash = Column(String(255), nullable=False)
    created_at = Column(TIMESTAMP)

    stats = relationship("PlayerStats", back_populates="user", uselist=False)
    inventory = relationship("Inventory", back_populates="user")


class PlayerStats(Base):
    __tablename__ = "player_stats"

    stat_id = Column(Integer, primary_key=True, index=True)
    user_id = Column(Integer, ForeignKey("users.user_id", ondelete="CASCADE"))
    level = Column(Integer, nullable=False, default=1)
    xp = Column(Integer, nullable=False, default=0)
    proficiency = Column(Integer, nullable=False, default=0)
    last_played = Column(Date)

    user = relationship("User", back_populates="stats")


class Quest(Base):
    __tablename__ = "quests"

    quest_id = Column(Integer, primary_key=True, index=True)
    title = Column(String(100), nullable=False)
    difficulty = Column(Enum("easy", "medium", "hard", name="difficulty_enum"), default="easy")
    xp_reward = Column(Integer, nullable=False, default=10)
    proficiency_gain = Column(Integer, nullable=False, default=0)
    description = Column(Text)


class Question(Base):
    __tablename__ = "questions"

    question_id = Column(Integer, primary_key=True, index=True)
    quest_id = Column(Integer, ForeignKey("quests.quest_id", ondelete="CASCADE"))
    question_text = Column(Text, nullable=False)
    question_type = Column(Enum("multiple_choice", "true_false", "coding", name="question_type_enum"), nullable=False)
    correct_answer = Column(Text, nullable=False)
    options = Column(JSON)


class QuestAttempt(Base):
    __tablename__ = "quest_attempts"

    attempt_id = Column(Integer, primary_key=True, index=True)
    user_id = Column(Integer, ForeignKey("users.user_id", ondelete="CASCADE"))
    quest_id = Column(Integer, ForeignKey("quests.quest_id", ondelete="CASCADE"))
    score = Column(Integer, default=0)
    completed = Column(Boolean, default=False)
    xp_earned = Column(Integer, default=0)
    prof_change = Column(Integer, default=0)
    attempt_started = Column(TIMESTAMP)
    attempt_ended = Column(TIMESTAMP)


class Cosmetic(Base):
    __tablename__ = "cosmetics"

    cosmetic_id = Column(Integer, primary_key=True, index=True)
    name = Column(String(50), unique=True, nullable=False)
    level_required = Column(Integer, nullable=False, default=1)
    description = Column(Text)


class Inventory(Base):
    __tablename__ = "inventory"

    inv_id = Column(Integer, primary_key=True, index=True)
    user_id = Column(Integer, ForeignKey("users.user_id", ondelete="CASCADE"))
    cosmetic_id = Column(Integer, ForeignKey("cosmetics.cosmetic_id", ondelete="CASCADE"))
    unlocked = Column(Boolean, default=False)
    equipped = Column(Boolean, default=False)
    unlocked_at = Column(TIMESTAMP)

    user = relationship("User", back_populates="inventory")



# INITIALIZER

def init_models():
    from app.database import engine
    Base.metadata.create_all(bind=engine)
