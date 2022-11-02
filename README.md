## 💻 Developer
This project was developed by: Vitor Oliveira

## 📝 Project
Storage
  A simple document storage api.
  
## Description ✈️
* This project is using Domain Driven Design for it strucutre.  
* For database we are using the Postresql.  
* This API is written with ASP.NET Core language.
  
## ▶️ Start application 💻 

### ❗ First of all:
* You need to have docker installed in your computer.

### Start ▶️
To start the applciation, you need just to run the command `docker-compose up`. When you see this message `your server stated on [::]:8002` so your app is running correctly!

Migration files are located at the findox.Data/Migrations directory, using the Flyway application to apply and track the migrations runinng in Docker. For more information such as the file naming patterns used by [Flyway](https://flywaydb.org), see their SQL-based migrations documentation page.

### Tests
* With the command `make test` you can run all tests in this project.
