TeelCode Backend Build Instructions

1. Prerequisites
- Install Python 
- Install Git
- Install pip
- Install MySQL Server or use provided Cloud SQL connection

2. Clone Repository
git clone https://github.com/willbubger/TeelCode.git
cd TeelCode/backend

3. Create Virtual Environment
python -m venv .venv
source .venv/bin/activate (Linux/Mac)
.venv\Scripts\activate (Windows)

4. Install Dependencies
pip install -r requirements.txt

5. Configure Environment Variables
Create .env file with:
DATABASE_URL=mysql+pymysql://USER:PASSWORD@HOST:3306/teelcode_app

6. Initialize Database
Run MySQL:
mysql -u USER -p
CREATE DATABASE teelcode_app;

7. Run Backend Locally
uvicorn app.main:app --reload --host 0.0.0.0 --port 8080

8. Test API
Open browser:
http://localhost:8080/docs

9. Deploy to Cloud Run
gcloud run deploy teelcode-backend --source . --region us-east1 --platform managed
--allow-unauthenticated --set-env-vars "DATABASE_URL=YOUR_URL"

10. CSV Quest Import
Place CSVs in quests_csv/ directory.
Run:
python import_quests.py

11. Notes
- Make sure MySQL accepts external connections
- Ensure MySQL user has correct permissions
