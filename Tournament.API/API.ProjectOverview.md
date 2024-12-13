# Turnering API Projektkrav och Implementering

## Projektöversikt
Turnering API är ett backend-system som erbjuder funktionalitet för att hantera sportturneringar och deras tillhörande matcher. 
Systemet tillåter användare att utföra CRUD-operationer på turneringar och deras matcher, samt implementera olika affärsregler 
och funktioner för att förbättra hanteringen av turneringar.

## Krav

### Grundläggande Funktionalitet

#### Hantering av Turneringar:
- Skapa, läsa, uppdatera och radera turneringar
- Hämta en lista över alla turneringar med stöd för paginering
- Hämta detaljer om en specifik turnering, inklusive dess tillhörande matcher

#### Hantering av Matcher:
- Skapa, läsa, uppdatera och radera matcher som är kopplade till en turnering
- Hämta en lista över alla matcher för en specifik turnering med stöd för paginering
- Hämta detaljer om en specifik match

### Affärsregler

#### Maxgräns för Matcher per Turnering:
- Begränsa antalet matcher per turnering till 10
- Kasta ett undantag om gränsen överskrids när man försöker skapa en ny match

#### Validering av Turneringar och Matcher:
- Validera att titlar för turneringar och matcher inte är tomma och inte överskrider 100 tecken
- Kasta lämpliga undantag vid valideringsfel

### API-design

#### Paginering:
- Tillhandahålla stöd för paginering vid hämtning av listor över turneringar och matcher
- Tillåta klienter att ange sidstorlek och aktuell sidnummer som query-parametrar
- Begränsa den maximala sidstorleken till 100 objekt
- Inkludera metadata i svaret, som totalt antal sidor, aktuell sida och totalt antal objekt

#### Felhantering:
- Implementera anpassade undantag för specifika felscenarier (t.ex. turnering inte hittad, maxgräns för matcher överskriden)
- Returnera lämpliga HTTP-statuskoder för olika typer av fel
- Tillhandahålla ett konsekvent format för felmeddelanden, till exempel ProblemDetails-formatet

### Teknikstack
- .NET 8 Core Web API
- Entity Framework Core för dataåtkomst
- AutoMapper för att mappa mellan entiteter och DTO:er
- Repository-pattern och Unit of Work för abstraktion av dataåtkomst
- Dependency Injection för att hantera tjänstberoenden

## Implementeringsdetaljer

### Projektstruktur
Lösningen är organiserad i följande projekt:

- **Tournament.API**: Ansvarar för att hantera HTTP-förfrågningar, konfigurera applikationen och registrera tjänster.
- **Tournament.Core**: Innehåller domänmodeller, DTO:er, repositories och tjänstegränssnitt.
- **Tournament.Data**: Implementerar repositories, Unit of Work och databascontext.
- **Tournament.Services**: Implementerar logik för tjänstelagret, inklusive mappning mellan entiteter och DTO:er.

### Architectural Patterns

- **Repository Pattern**: Gränssnitten `ITournamentRepository` och `IGameRepository` definierar dataåtkomstoperationer, medan de konkreta repository-implementationerna hanterar databasinteraktionerna.
- **Unit of Work**: Gränssnittet `IUoW` och dess implementation `UoW` samordnar repositories och hanterar databastransaktioner.
- **Service Layer**: Klasserna `TournamentService` och `GameService` kapslar in affärslogiken och samordnar repository-operationerna.
- **Dependency Injection**: Applikationen använder konstruktionsinjektion för att hantera beroenden mellan de olika lagren och komponenterna.

### Paginering Implementation
- **RequestParameters** och dess underklasser (`TournamentParameters` och `GameParameters`) hanterar query-parametrar relaterade till paginering, såsom sidnummer och sidstorlek.
- **PagedList<T>** ansvarar för att paginera data och tillhandahålla nödvändig metadata (totalt antal sidor, aktuell sida, totalt antal objekt) i svaret.
- Tjänstemetoderna (`GetAllTournamentsAsync` och `GetAllGamesAsync`) använder `PagedList<T>.ToPagedList`-metoden för att tillämpa paginering på data som returneras från repositories.

### Felhantering
- Anpassade undantag, såsom `TournamentNotFoundException` och `GameNotFoundException`, kastas av tjänstelagret när entiteter inte hittas.

```csharp
// Exempel på hur ett undantag kastas
if (tournament == null)
{
    throw new TournamentNotFoundException($"Turnering med ID {id} hittades inte.");
}

// Exempel på hur undantaget hanteras i controllern
catch (TournamentNotFoundException ex)
{
    return NotFound(new ProblemDetails
    {
        Title = "Turnering hittades inte",
        Detail = ex.Message,
        Status = StatusCodes.Status404NotFound
    });
}
```

- `MaxGamesExceededException` kastas när maxgränsen för matcher per turnering överskrids.
- Controllers hanterar undantagen och returnerar lämpliga HTTP-statuskoder (t.ex. 404 för inte hittad, 400 för valideringsfel).
- Felsvaren följer ProblemDetails-formatet för att tillhandahålla ett konsekvent och informativt felmeddelande.

### Mappning mellan Entiteter och DTO:er
- AutoMapper används för att mappa mellan domänentiteter (`TournamentDetails` och `Game`) och motsvarande DTO:er (`TournamentDto`, `TournamentForCreationDto`, `TournamentForUpdateDto`, `GameDto`, `GameForCreationDto`, `GameForUpdateDto`).
- Mappningskonfigurationen definieras i klassen `TournamentMappings`, som ärver från AutoMappers `Profile`-klass.

### Implementering av Affärsregler
- Klassen `GameService` upprätthåller affärsregeln om att begränsa antalet matcher per turnering till 10.
- Vid skapande av en ny match kontrollerar metoden `GameService.CreateGameAsync` det aktuella antalet matcher för turneringen och kastar `MaxGamesExceededException` om gränsen överskrids.

## Framtida Förbättringar och Överväganden

- **Filtrering och Sortering**: Implementera ytterligare query-parametrar för att tillåta klienter att filtrera och sortera listorna över turneringar och matcher.
- **Caching**: Implementera cachemekanismer för att förbättra prestandan för ofta åtkomna data, såsom turnerings- och matchdetaljer.
- **Autentisering och Auktorisering**: Integrera autentisering och auktorisering för att säkra API:et och begränsa åtkomst till vissa operationer baserat på användarroller.
- **Loggning och Övervakning**: Implementera detaljerad loggning och övervakning för att underlätta felsökning och prestandaanalys.
- **Integrationstester**: Utveckla en omfattande uppsättning integrationstester för att säkerställa API:ets övergripande funktionalitet och tillförlitlighet.
