# ⚙️ TeelCode Backend Setup Instructions

This backend powers the TeelCode project and connects directly to a Google Cloud SQL (MySQL) database.  
Follow these steps to run the backend locally on your machine.


---


## 1️⃣ Prerequisites

Before starting, make sure you have:

- Python 3.10+ installed  
- pip (Python package manager)  
- Access to the TeelCode Cloud SQL instance 
- Optionally, MySQL Workbench if you want to view or edit the database manually

---

## 2️⃣ Clone the Repository

Open terminal or PowerShell and run:

```bash
git clone https://github.com/<your_username>/TeelCode.git
cd TeelCode/teelcode_backend

---

##3⃣ Create virtual envioment

this creates isolated python environment for project.

python -m venv .venv

Activate it:

source .venv/bin/activate

---


##4️⃣ Install dependencies

with the environment activated, install the required packages:

pip install -r requirements.txt

---

##5️⃣ Create a .env file

in teelcode_backend folder, and it should look like this:


DB_USER=teelcode_native
DB_PASSWORD=Eric2010*
DB_HOST=35.237.7.170
DB_PORT=3306
DB_NAME=teelcode_app


---

##6️⃣ Test the database connection

run this to verify your backend can connect to cloud sql:

python app/test_connection.py

if successful you'll see:

✅ Connection successful!

if it failed either .env file is incorrect or your IP  isn't allowed in cloud sql authorized networks (ask Lj to add you)

---

##7️⃣ Run the backend server

python app/main.py


then open your browser to:
http://127.0.0.1:8000

you should now see the docs page for Teelcode backend