# PslibThesesBackend
Backend API aplikace pro správu dlouhodobých prací.

## Požadavky
* ASP.NET Core 2.2
* Entity Framework Core 2.2

## Instalace
### Balíčky (NuGet)
* Install-Package Microsoft.EntityFrameworkCore.SqlServer
* Install-Package Microsoft.EntityFrameworkCore.Tools
### Databáze
* Update-Database

## Použití
### Uživatelé

Seznam uživatelů

    GET /users[search][firstname][lastname][email][order][page][pagesize]

Data uživatele

    GET /users/{id}

Vytvoření uživatele

    POST /users

Změna uživatele

    PUT /users/{id}

Smazání uživatele

    DELETE /users/{id}
