# 🎥 Scene Sherpa 

SceneSherpa is an MVC web application that allows users to CRUD an account and track their media consumption through the use of three lists: media they want to watch, media they have watched, and media that they are watching. In addition to being able to edit their lists, users are able to add reviews to media that they have seen, allowing other users to view their rating and review directly from the media's page.

### Project Context
* Number of primary contributors: **3**
* Number of other contributors: **4**
* Total work-time: **70 Hours**

![image](https://github.com/jcepriano/SceneSherpa/assets/130601077/3c14cbe0-0c95-4ae9-b1e7-0519276eff95)
<details>
  <summary>Additional Screenshots</summary>
  <img src="https://github.com/jcepriano/SceneSherpa/assets/130601077/333eb95d-dce4-4aa6-93b4-57bd5aa3d848" name="media-show">
  <img src="https://github.com/jcepriano/SceneSherpa/assets/130601077/e64b766c-cabe-4923-8ce7-f61c5f15b857" name="user-show">
  <img src="https://github.com/jcepriano/SceneSherpa/assets/130601077/885420a5-13df-48ca-81c2-b501e1ac03ec" name="about-page">
</details>

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

* [pgAdmin](https://www.pgadmin.org/)
* [Visual Studio](https://visualstudio.microsoft.com/)

### Installing

1. Fork and clone the repository
2. In the Package Manager Console run the command `update-database`
3. Right click the SceneSherpa Project file in Visual studio and select user secrets paste the code below into your secrets file, replacing the `YOURUSERNAMEHERE` and `YOURPASSWORDHERE` with your username and password for pgAdmin.
```
{
  "SCENESHERPA_DBCONNECTIONSTRING": "Server=localhost;Database=SceneSherpa;Port=5432;Username=YOURUSERNAMEHERE;Password=YOURPASSWORDHERE"
}
```
4. Navigate to the [seed data file](SceneSherpa/wwwroot/Resources/seed_data.txt) and copy the entire contents of this file.
5. Open pgAdmin and connect to the SceneSherpa Database
6. Run the Query Tool for the SceneSherpa Database
7. Paste the contents from the seed_data.txt file.
8. Run the query
9. Done ✅ Run the project from Visual Studio!

## Built With
* [Bootstrap](https://getbootstrap.com/)
* [Markdig](https://github.com/xoofx/markdig)
* [ASP .NET Core](https://github.com/dotnet/aspnetcore)
* [Entity Framework Core](https://github.com/dotnet/efcore)
* [PostgreSQL](https://www.postgresql.org/)

## Maintained With
* [Serilog](https://serilog.net/)

## Authors
| <img src="https://github.com/jcepriano.png?">    | <img src="https://github.com/bradenasmith2.png?">|<img src="https://github.com/jeremy-kimball.png?"> |
|:---:|:--:|:----:|
| James Cepriano | Braden Smith  | Jeremy Kimball|
|  <a href="https://www.linkedin.com/in/jamescepriano/"><img src="https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white"></img></a><a href="https://github.com/jcepriano/"><img src="https://img.shields.io/badge/GitHub-100000?style=for-the-badge&logo=github&logoColor=white"></img></a>              |   <a href="https://www.linkedin.com/in/braden-smith2/"><img src="https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white"></img></a><a href="https://github.com/bradenasmith2"><img src="https://img.shields.io/badge/GitHub-100000?style=for-the-badge&logo=github&logoColor=white"></img></a>            |<a href="https://www.linkedin.com/in/jeremyckimball/"><img src="https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white"></img></a><a href="https://github.com/jeremy-kimball"><img src="https://img.shields.io/badge/GitHub-100000?style=for-the-badge&logo=github&logoColor=white"></img></a>|

## Acknowledgments

* Eli Paris [@Eli-J-Paris](https://github.com/Eli-J-Paris)
* Isiah Worsham [@iworsham](https://github.com/iworsham)
* Alexander Bukhmirov [@abukhmirov](https://github.com/abukhmirov)




















