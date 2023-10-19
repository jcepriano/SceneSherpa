# 🎥 Scene Sherpa 

One Paragraph of project description goes here

![image](https://github.com/jcepriano/SceneSherpa/assets/130601077/3c14cbe0-0c95-4ae9-b1e7-0519276eff95)
<details>
  <summary>Additional Screenshots</summary>
  <img src="https://github.com/jeremy-kimball/MvcMessageLogger/assets/130601077/6026a762-443a-4d31-b21e-d18013e8733b" name="user-creation">
  <img src="https://github.com/jeremy-kimball/MvcMessageLogger/assets/130601077/7556ad18-90ed-4150-b81f-e8697504fbee" name="statistics">
  <img src="https://github.com/jeremy-kimball/MvcMessageLogger/assets/130601077/d085308b-3760-47a4-a7b3-037b050535ac" name="privacy-policy">
  <img src="https://github.com/jeremy-kimball/MvcMessageLogger/assets/130601077/0e005a64-609f-4fcd-86c9-1bd8c3fd6413" name="welcome">
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
4. Navigate to the (seed data txt file goes here) and copy the entire contents of this file.
5. Open pgAdmin and connect to the SceneSherpa Database
6. Run the Query Tool for SceneSherpa
7. Paste the contents from the seed_data.txt file.
8. Run the query
9. Done ✅ Run the project from Visual Studio!

## Built With
* [Bootstrap](https://getbootstrap.com/)
* [Markdig](https://github.com/xoofx/markdig)
* [ASP .NET Core](https://github.com/dotnet/aspnetcore)
* [Serilog](https://serilog.net/)
* [Entity Framework Core](https://github.com/dotnet/efcore)

## Authors
| <img src="https://github.com/jcepriano.png?">    | <img src="https://github.com/bradenasmith2.png?">|<img src="https://github.com/jeremy-kimball.png?"> |
|:---:|:--:|:----:|
| James Cepriano | Braden Smith  | Jeremy Kimball|
|  <a href="https://www.linkedin.com/in/jamescepriano/"><img src="https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white"></img></a><a href="https://github.com/jcepriano/"><img src="https://img.shields.io/badge/GitHub-100000?style=for-the-badge&logo=github&logoColor=white"></img></a>              |   <a href="https://www.linkedin.com/in/braden-smith2/"><img src="https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white"></img></a><a href="https://github.com/bradenasmith2"><img src="https://img.shields.io/badge/GitHub-100000?style=for-the-badge&logo=github&logoColor=white"></img></a>            |<a href="https://www.linkedin.com/in/jeremyckimball/"><img src="https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white"></img></a><a href="https://github.com/jeremy-kimball"><img src="https://img.shields.io/badge/GitHub-100000?style=for-the-badge&logo=github&logoColor=white"></img></a>|

## Acknowledgments

* Eli Paris [@Eli-J-Paris](https://github.com/Eli-J-Paris)
* Isiah Worsham [@iworsham](https://github.com/iworsham)
* Alexander Bukhmirov [@abukhmirov](https://github.com/abukhmirov)




















