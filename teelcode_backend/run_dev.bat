@echo off
echo Starting TeelCode backend...
call .venv\Scripts\activate
uvicorn app.main:app --reload
pause
