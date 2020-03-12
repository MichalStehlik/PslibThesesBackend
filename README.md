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

    GET /ideas[search][name][subject][userId][firstname][lastname][target][offered][order][page][pagesize]
    
Data námětu

    GET /ideas/{id}
 
Kompletní data námětu

    GET /ideas/{id}/full 
 
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
 
Seznam všech cílových skupin

    GET /ideas/{id}/allTargets 

Seznam nepřiřazených cílových skupin

    GET /ideas/{id}/unusedTargets

Přidání cílové skupiny pro námět

    POST /ideas/{id}/targets

Odebrání cílové skupiny pro námět

    DELETE /ideas/{id}/targets/{target}

Seznam cílů námětu

    GET /ideas/{id}/goals
    
Získání cíle námětu

    GET /ideas/{id}/goals/{position}

Přidání cíle námětu

    POST /ideas/{id}/goals

Náhrada cílů námětu

    PUT /ideas/{id}/goals

Změna textu cíle námětu

    PUT /ideas/{id}/goals/{position}

Změna pořadí cíle námětu

    PUT /ideas/{id}/goals/{oldPosition}/moveto/{newPosition}

Vymazání všech cílů námětu

    DELETE /ideas/{id}/goals

Smazání cíle námětu

    DELETE /ideas/{id}/goals/{position}

Seznam bodů osnovy námětu

    GET /ideas/{id}/outlines
    
Získání určitého bodu osnovy námětu

    GET /ideas/{id}/outlines/{position}

Přidání nového bodu osnovy námětu

    POST /ideas/{id}/outlines

Náhrada osnovy námětu

    PUT /ideas/{id}/outlines

Změna textu bodu osnovy námětu

    PUT /ideas/{id}/outlines/{position}

Změna pořadí bodů osnovy námětu

    PUT /ideas/{id}/outlines/{oldPosition}/moveto/{newPosition}

Vymazání celé osnovy námětu

    DELETE /ideas/{id}/outlines

Smazání bodu osnovy námětu

    DELETE /ideas/{id}/outlines/{position}
    
### Sady

Seznam

    GET /sets[search][name][year][active][order][page][pagesize]
    
Data sady

    GET /sets/{id}

Změna dat sady

    PUT /sets/{id}
    
Vytvoření nové sady

    POST /sets
    
Odstranění sady

    DELETE /sets/{id}
    
Seznam termínů sady

    GET /sets/{id}/terms
    
Data termínu sady

    GET /sets/{id}/terms/{termId}
    
Změna termínu sady

    PUT /sets/{id}/terms/{termId}
    
Přidání termínu sady

    POST /sets/{id}/terms
    
Odstranění termínu sady

    DELETE /sets/{id}/terms/{termId}
    
Seznam rolí sady

    GET /sets/{id}/roles
    
Data role sady

    GET /sets/{id}/roles/{roleId}
    
Změna role sady

    PUT /sets/{id}/roles/{roleId}
    
Přidání role sadě

    POST /sets/{id}/roles
    
Odstranění role sady

    DELETE /sets/{id}/roles/{roleId}
    
## Práce

Seznam

    GET /works[search][name][subject][authorId][userId][firstname][lastname][set][setName][state][order][page][pagesize]
    
Data práce

    GET /works/{id}
    
Kompletní data práce

    GET /works/{id}/full    
    
Vytvoření práce (není kompletní, nevytváří záznamy pro role podle sady)

    POST /works

Změna práce

    PUT /works/{id}
    
Smazání práce

    DELETE /works/{id}

Seznam cílů práce

    GET /works/{id}/goals
    
Získání cíle práce

    GET /works/{id}/goals/{position}

Přidání cíle práci

    POST /works/{id}/goals

Náhrada cílů práce

    PUT /works/{id}/goals

Změna textu cíle práce

    PUT /works/{id}/goals/{position}

Změna pořadí cíle práce

    PUT /works/{id}/goals/{oldPosition}/moveto/{newPosition}

Vymazání všech cílů práce

    DELETE /works/{id}/goals

Smazání cíle práce

    DELETE /works/{id}/goals/{position}

Seznam bodů osnovy práce

    GET /works/{id}/outlines
    
Získání určitého bodu osnovy práce

    GET /works/{id}/outlines/{position}

Přidání nového bodu osnovy práce

    POST /works/{id}/outlines

Náhrada osnovy práce

    PUT /works/{id}/outlines

Změna textu bodu osnovy práce

    PUT /works/{id}/outlines/{position}

Změna pořadí bodů osnovy práce

    PUT /works/{id}/outlines/{oldPosition}/moveto/{newPosition}

Vymazání celé osnovy práce

    DELETE works/{id}/outlines

Smazání bodu osnovy práce

    DELETE /works/{id}/outlines/{position}
    
Stav práce

    GET /works/{id}/state
    
Stavy práce, na které lze přejít

    GET /works/{id}/nextstates
    
Všechny stavy práce

    GET /works/allstates
    
Změna stavu práce

    PUT /works/{id}/state

Role

    GET /works/{id}/roles

Data role v práci

    GET /works/{id}/roles/{roleId}
    
Přidání role specifické pro tuto práci
 
    POST /works/{id}/roles

Odstranění role specifické pro tuto práci
 
    DELETE /works/{id}/roles/{roleId}
    
Uživatelé přiřazení této roli
 
    GET /works/{id}/roles/{roleId}/users
    
Stažení přihlášky

    GET /works/{id}/application
