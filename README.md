# Agile Project Management System 

## :pencil2: Overview

**AgileMS** is a website that resembles Jira, it can hold projects with differant work items like user stories, bugs, sub-tasks etc. The site has burndown charts that show the workflow of the sprints. The main idea is to help developers manage the work of a project. In order for user to be added to a project he/she needs to make his/hers profile public from the profile settings.

## Usage
Once logged in you can go to the projects page where you can see all projects that you participate in or create new ones.

![Projects page](https://github.com/IvanDichev/AgileProjectManagementSystem/blob/main/images/Screenshot%202021-06-08%20at%2022-45-19%20List%20all%20projects%20-%20Web.png)
![Craete project](https://github.com/IvanDichev/AgileProjectManagementSystem/blob/main/images/Screenshot%202021-06-08%20at%2022-45-28%20List%20all%20projects%20-%20Web.png)

After you log into a project you can see who participats in that project, add new members or remove them. There is also a left nav-bar that shows current pages available to the project

![Project] (https://github.com/IvanDichev/AgileProjectManagementSystem/blob/main/images/Screenshot%202021-06-08%20at%2022-45-38%20Project%20-%20Web.png)

From the workitem page you can see all the current workitems that have been assigned to that project. You can sort them, add new ones or siply edit them. After workitems has been created you can leave a comment about it for your team. Also after work item has been created it can be assigned to a sprint that is currently active.

![Workitems table](https://github.com/IvanDichev/AgileProjectManagementSystem/blob/main/images/Screenshot%202021-06-08%20at%2022-47-27%20Wrok%20items%20overview%20-%20Web.png)
![Creating workitem](https://github.com/IvanDichev/AgileProjectManagementSystem/blob/main/images/Screenshot%202021-06-08%20at%2022-47-38%20Create%20-%20Web.png)

Once workitem has been assigned to a sprint you can open the board page to see the workflow of the sprint. Here you can see brief overview of the workitem and you can drag it between the columns to set it's status from backlog to in progress for example.

 ![Board page](https://github.com/IvanDichev/AgileProjectManagementSystem/blob/main/images/Screenshot%202021-06-08%20at%2022-52-35%20Board%20-%20Web.png)

In the sprint page you can see all the sprints for the current project and create new ones.

- sprint page

In order for your profile to be visible for others to invite you for a given project you must check visability status from you profile settings 

![Profile settings](https://github.com/IvanDichev/AgileProjectManagementSystem/blob/main/images/Screenshot%202021-06-09%20at%2012-45-21%20Profile%20-%20Web.png)

After you have been invited to a project, removed from one or simply assigned to a task you get a notification that is always visible at the top right corner of the page. You also reacive an email for that notification.

![notifications](https://github.com/IvanDichev/AgileProjectManagementSystem/blob/main/images/Screenshot%202021-06-08%20at%2022-34-17%20Home%20Page%20-%20Web.png)

## Databse diagram

![Db diagram](https://github.com/IvanDichev/AgileProjectManagementSystem/blob/main/images/AgileMSDbDiagram.png)


## **Technologies Used**

This website is designed and runs using the **main** technologies below:

   1) **[C#](https://en.wikipedia.org/wiki/C_Sharp_(programming_language))**
   2) **[ASP.NET Core 3.1](https://en.wikipedia.org/wiki/ASP.NET_Core)**
   3) **[Entity Framework Core 3.1](https://en.wikipedia.org/wiki/Entity_Framework?wprov=srpw1_0)**
   4) **[MS SQL Server](https://en.wikipedia.org/wiki/Microsoft_SQL_Server)**
   5) **[Bootstrap 4](https://getbootstrap.com/docs/4.0/getting-started/introduction/)**
   6) **[JavaScript](https://en.wikipedia.org/wiki/JavaScript)**
   7) **[HTML5](https://en.wikipedia.org/wiki/HTML)**
   8) **[CSS](https://www.w3schools.com/css/css_intro.asp)**
   9) **[Hangfire API](https://api.hangfire.io/html/R_Project_Hangfire_Api.htm)**
   10) **[Cloudinary API](https://cloudinary.com/documentation/image_upload_api_reference)**
   11) **[MailKit API](https://github.com/jstedfast/MailKit)**
   10) **[Chart.js](https://www.chartjs.org/samples/latest/)**
   10) **[Ajax](https://en.wikipedia.org/wiki/Ajax_(programming))**
   10) **[Jquery](https://jquery.com/)**
