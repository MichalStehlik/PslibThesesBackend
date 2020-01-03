# PslibThesesBackend
Backend API aplikace pro správu dlouhodobých prací.

## Požadavky
* ASP.NET Core 3.1
* Entity Framework Core 3.1

## Instalace
### Balíčky (NuGet)
* Install-Package Microsoft.EntityFrameworkCore
* Install-Package Microsoft.EntityFrameworkCore.SqlServer
* Install-Package Microsoft.EntityFrameworkCore.Tools
* Install-Package Microsoft.EntityFrameworkCore.Design
* Install-Package MailKit
* Install-Package IdentityServer4
* Install-Package IdentityServer4.AspNetIdentity
* Install-Package IdentityServer4.EntityFramework
* Install-Package Microsoft.AspNetCore.Authentication.JwtBearer
* Install-Package Microsoft.AspNetCore.Authentication.MicrosoftAccount
* Install-Package Utf8Json
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

### Cílové skupiny

Seznam

    GET /targets[search][text][order][page][pagesize]
    
Data cíle

    GET /targets/{id}
    
Vytvoření cíle

    POST /targets

Změna cíle

    PUT /targets/{id}
    
Smazání cíle

    DELETE /targets/{id}
    
### Náměty

Seznam

    GET /ideas[search][name][subject][iserId][firstname][lastname][target][offered][order][page][pagesize]
    
Data námětu

    GET /ideas/{id}
    
Vytvoření námětu

    POST /ideas

Změna námětu

    PUT /ideas/{id}
    
Nabídnutí námětu studentům

    PUT /ideas/{id}/offered
    
Smazání námětu

    DELETE /ideas/{id}

Seznam cílových skupin pro námět

    GET /ideas/{id}/targets
    
Přidání cílové skupiny pro námět

    POST /ideas/{id}/targets

Odebrání cílové skupiny pro námět

    DELETE /ideas/{id}/targets/{target}

Seznam cílů námětu

    GET /ideas/{id}/goals
    
Získání cíle námětu

    GET /ideas/{id}/goals/{order}

Přidání cíle námětu

    POST /ideas/{id}/goals

Změna textu cíle námětu

    PUT /ideas/{id}/goals/{order}
