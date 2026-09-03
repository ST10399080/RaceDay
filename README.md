<p align="center">
<img width="1536" height="936" alt="RaceDay Logo" src="https://github.com/user-attachments/assets/bc32db66-1a15-4beb-bf04-0bb8ec7a7d89" />
</p>

<div align="center">

# 🏃 RaceDay 🚶🚴

### **Every Race. Every Athlete. One Platform.**

*A modern event management platform built for organising and participating in running, walking, and cycling events.*

![Status](https://img.shields.io/badge/Status-In%20Development-orange)
![Language](https://img.shields.io/badge/C%23-.NET-blue)
![Framework](https://img.shields.io/badge/ASP.NET-Core-purple)
![Database](https://img.shields.io/badge/SQL%20Server-Database-red)
![License](https://img.shields.io/badge/License-MIT-green)

</div>

---

# 📖 About RaceDay

**RaceDay** is a comprehensive sporting event management platform that enables organisers to create and manage sporting events while allowing participants to discover, register, and track their race performance—all from one integrated platform.

Whether it's a **5km Fun Run**, **Half Marathon**, **Cycling Challenge**, or **Community Walk**, RaceDay simplifies the entire event management process from registration to race results.

---

# 🎯 Project Objectives

RaceDay aims to provide a complete digital solution for sporting event management by allowing:

✅ Event Creation

✅ Participant Registration

✅ Category Management

✅ Event Enrolment

✅ Race Result Management

✅ Secure Authentication

✅ Cloud Image Storage

✅ Role-Based Access Control

---

# 🌟 Key Features

## 👨‍💼 Organiser

* 🏁 Create sporting events
* ✏️ Edit existing events
* ❌ Delete events
* 🗂️ Manage event categories
* 📋 View participant enrolments
* 🏆 Record race results
* 🖼️ Upload event banner images
* 📊 View organiser dashboard

---

## 🏃 Participant

* 👤 Register an account
* 🔐 Secure login
* 🔎 Browse upcoming events
* 📝 Enter races
* 🎯 Choose event categories
* 📅 View enrolled events
* 🏅 Track race history
* 📈 View personal results
* 🖼️ Upload profile picture
* ⚙️ Manage profile

---

# 🚴 Supported Sporting Events

* 🏃 Running
* 🚶 Walking
* 🚴 Cycling

---

# 🛠️ Technology Stack

## 💻 Backend

* ASP.NET Core Web API
* C#
* Entity Framework Core
* SQL Server
* Session Authentication
* REST API
* Swagger

---

## 🌐 Frontend

* ASP.NET Core MVC
* HTML5
* CSS3
* Bootstrap 5
* JavaScript

---

## ☁️ Cloud

* Azure Blob Storage

---

## 🐳 DevOps

* Docker
* GitHub
* GitHub Actions CI/CD

---

# 🗃️ Database

The application uses **Microsoft SQL Server** with **Entity Framework Core (Code First)**.

Main entities include:

* Users
* Events
* Categories
* Event Enrolments
* Results

---

# 🔐 User Roles

## 👨‍💼 Organiser

Organisers are responsible for creating and managing sporting events.

They can:

* Manage Events
* Manage Categories
* View Enrolments
* Publish Results
* Upload Event Images

---

## 🏃 Participant

Participants are athletes who take part in sporting events.

They can:

* Register
* Login
* Browse Events
* Enrol in Events
* View Results
* Manage Their Profile

---

# 🔒 Security Features

* 🔑 Password Hashing
* 🔐 Session Authentication
* 🛡️ Role-Based Authorization
* ✅ Input Validation
* 🚫 Protected API Endpoints

---

# 📂 Project Structure

```text
RaceDay/
│
├── docs/
│   ├── ERD.png
│   ├── API_Endpoint_Plan.pdf
│   └── RaceDay.sql
│
├── RaceDay.API/
│
├── RaceDay.MVC/
│
├── RaceDay.Tests/
│
├── .github/
│   └── workflows/
│
├── README.md
│
└── Dockerfile
```

---

# 🚀 Getting Started

## Prerequisites

* Visual Studio 2022
* .NET 8 SDK
* SQL Server
* SQL Server Management Studio (SSMS)
* Docker Desktop
* Git

---

## Installation

### Clone the repository

```bash
git clone https://github.com/YOUR_USERNAME/RaceDay.git
```

---

### Open the solution

```text
RaceDay.sln
```

---

### Restore packages

```bash
dotnet restore
```

---

### Update the database

```bash
Update-Database
```

---

### Run the application

```bash
dotnet run
```

---

# 📚 API Documentation

Swagger is enabled during development.

```text
https://localhost:xxxx/swagger
```

All API endpoints can be tested directly from the Swagger UI.

---

# 🐳 Docker

Build the container

```bash
docker build -t raceday .
```

Run the application

```bash
docker run -p 8080:80 raceday
```

---

# 🔄 CI/CD

GitHub Actions automatically:

* ✅ Builds the solution
* ✅ Runs unit tests
* ✅ Verifies project integrity

## 🟢 Latest Build

> 📸 **Insert Green Build Screenshot Here**

---

# 🧪 Unit Testing

Tests include:

* Authentication
* Registration
* Event Management
* Role Authorization
* Event Enrolments
* Results

---

# 🎥 Demonstration Video

📺 YouTube:

> 

---

# 📄 Documentation

Inside the **/docs** folder:

* 📑 Entity Relationship Diagram (ERD)
* 📑 API Endpoint Plan
* 📑 SQL Script

---

# 👨‍💻 Developer

**Fezile Jam-Jam (ST10399080)**

Programming 2B (PROG6212)

The Independent Institute of Education (IIE)

---

# 🎯 Future Improvements

* 📱 Mobile Application
* 📍 GPS Route Tracking
* 📊 Live Analytics Dashboard
* ⌚ Smartwatch Integration
* 📲 Push Notifications
* 💳 Online Payments
* 🏅 Digital Certificates
* 📈 Leaderboards
* 🗺️ Interactive Maps

---

# 📜 License

This project is licensed under the MIT License.

See [LICENSE](LICENSE) for full details.

<div align="center">

# 🏁 RaceDay

### **Every Race. Every Athlete. One Platform.**

**Built with ❤️, determination, and a passion for sport.**

⭐ **If you like this project, don't forget to star the repository!** ⭐

</div>

You can further personalize this once you have your repository URL, CI/CD build screenshot, and unlisted YouTube links. I also recommend adding your final RaceDay logo to the top of the README for an even more professional presentation.
