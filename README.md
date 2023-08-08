<h1 align="center">
CasseroleX
</h1>

<div align="center">

CasseroleX is a backend development framework based on permission management.
 
</div>

[![](https://img.alicdn.com/tfs/TB1t6QPylr0gK0jSZFnXXbRRXXa-4000-1378.png)](http://ng.ant.design)

English | [简体中文](README-zh_CN.md)


## ✨ Features

- Support unlimited administrator permission inheritance,
- Parent level administrators can add, delete, or modify child level administrators and permission settings at will
- Support multiple roles for a single administrator
- Support attachment upload classification management
- Supports regular user groups, user permissions, and menu management
- It uses Clean Architecture and CQRS, with excellent performance and concise code.
 


## 🎉 Technologies

- [ASP.NET Core 7](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core)
- [Entity Framework Core 7](https://docs.microsoft.com/en-us/ef/core/)
- [MediatR](https://github.com/jbogard/MediatR)
- [AutoMapper](https://automapper.org/)
- [FluentValidation](https://fluentvalidation.net/)
- [Serilog](https://github.com/serilog/serilog-aspnetcore)
- [FastAdmin](https://github.com/karsonzhang/fastadmin)
- [Pomelo EntityFrameworkCore MySql](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql) 
 

## 🎨 UI Specification

The front-end interface uses `Fastadmin`, which integrates `AdminLTE`, `Bootstrap`, `jQuery`, `Bootstrap table`, `Layer` and other front-end frameworks. I would like to express my special gratitude to them for saving me a lot of time on the UI


## 📦 Installation

After opening the project using `Visual Studio 2022` , open the `appsettings.json` file to configure the database connection. This project uses a `Mysql` database. If using another database, please change the database connection related statements in the `ConfigureServices.cs` file under the `Infrastructure` project

```bash
services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString)));
```

add a new migration from the root folder:


```bash
 dotnet ef migrations add "InitMigration" --project src\Infrastructure --startup-project src\WebUI --output-dir Migrations
```

If Redis is not installed, please set `UseRedisCache` to false

```bash
"RedisOptions": {
    "UseRedisCache": false, 
    "RedisDataProtectionKey": "",
    "CacheTime": 1440, //min
    "RedisConnectionString": "192.168.0.1:6379",
    "RedisDatabaseId": 2
  }
```

Launch the app:
```bash
cd src/Web
dotnet run
```
  
## ❓ Help from the Community

For questions on how to use CasseroleX, please post questions to [issues](https://github.com/Harold-Jiang/CasseroleX/issues)  
 

## ☀️ License

This project is licensed with the [Apache2](LICENSE).



**If you find this project useful, please give it a star. Thanks! ⭐**