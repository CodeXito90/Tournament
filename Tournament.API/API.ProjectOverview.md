# Turnering API Projektkrav och Implementering

## Projekt�versikt
Turnering API �r ett backend-system som erbjuder funktionalitet f�r att hantera sportturneringar och deras tillh�rande matcher. 
Systemet till�ter anv�ndare att utf�ra CRUD-operationer p� turneringar och deras matcher, samt implementera olika aff�rsregler 
och funktioner f�r att f�rb�ttra hanteringen av turneringar.

## Krav

### Grundl�ggande Funktionalitet

#### Hantering av Turneringar:
- Skapa, l�sa, uppdatera och radera turneringar
- H�mta en lista �ver alla turneringar med st�d f�r paginering
- H�mta detaljer om en specifik turnering, inklusive dess tillh�rande matcher

#### Hantering av Matcher:
- Skapa, l�sa, uppdatera och radera matcher som �r kopplade till en turnering
- H�mta en lista �ver alla matcher f�r en specifik turnering med st�d f�r paginering
- H�mta detaljer om en specifik match

### Aff�rsregler

#### Maxgr�ns f�r Matcher per Turnering:
- Begr�nsa antalet matcher per turnering till 10
- Kasta ett undantag om gr�nsen �verskrids n�r man f�rs�ker skapa en ny match

#### Validering av Turneringar och Matcher:
- Validera att titlar f�r turneringar och matcher inte �r tomma och inte �verskrider 100 tecken
- Kasta l�mpliga undantag vid valideringsfel

### API-design

#### Paginering:
- Tillhandah�lla st�d f�r paginering vid h�mtning av listor �ver turneringar och matcher
- Till�ta klienter att ange sidstorlek och aktuell sidnummer som query-parametrar
- Begr�nsa den maximala sidstorleken till 100 objekt
- Inkludera metadata i svaret, som totalt antal sidor, aktuell sida och totalt antal objekt

#### Felhantering:
- Implementera anpassade undantag f�r specifika felscenarier (t.ex. turnering inte hittad, maxgr�ns f�r matcher �verskriden)
- Returnera l�mpliga HTTP-statuskoder f�r olika typer av fel
- Tillhandah�lla ett konsekvent format f�r felmeddelanden, till exempel ProblemDetails-formatet

### Teknikstack
- .NET 8 Core Web API
- Entity Framework Core f�r data�tkomst
- AutoMapper f�r att mappa mellan entiteter och DTO:er
- Repository-pattern och Unit of Work f�r abstraktion av data�tkomst
- Dependency Injection f�r att hantera tj�nstberoenden

## Implementeringsdetaljer

### Projektstruktur
L�sningen �r organiserad i f�ljande projekt:

- **Tournament.API**: Ansvarar f�r att hantera HTTP-f�rfr�gningar, konfigurera applikationen och registrera tj�nster.
- **Tournament.Core**: Inneh�ller dom�nmodeller, DTO:er, repositories och tj�nstegr�nssnitt.
- **Tournament.Data**: Implementerar repositories, Unit of Work och databascontext.
- **Tournament.Services**: Implementerar logik f�r tj�nstelagret, inklusive mappning mellan entiteter och DTO:er.

### Architectural Patterns

- **Repository Pattern**: Gr�nssnitten `ITournamentRepository` och `IGameRepository` definierar data�tkomstoperationer, medan de konkreta repository-implementationerna hanterar databasinteraktionerna.
- **Unit of Work**: Gr�nssnittet `IUoW` och dess implementation `UoW` samordnar repositories och hanterar databastransaktioner.
- **Service Layer**: Klasserna `TournamentService` och `GameService` kapslar in aff�rslogiken och samordnar repository-operationerna.
- **Dependency Injection**: Applikationen anv�nder konstruktionsinjektion f�r att hantera beroenden mellan de olika lagren och komponenterna.

### Paginering Implementation
- **RequestParameters** och dess underklasser (`TournamentParameters` och `GameParameters`) hanterar query-parametrar relaterade till paginering, s�som sidnummer och sidstorlek.
- **PagedList<T>** ansvarar f�r att paginera data och tillhandah�lla n�dv�ndig metadata (totalt antal sidor, aktuell sida, totalt antal objekt) i svaret.
- Tj�nstemetoderna (`GetAllTournamentsAsync` och `GetAllGamesAsync`) anv�nder `PagedList<T>.ToPagedList`-metoden f�r att till�mpa paginering p� data som returneras fr�n repositories.

### Felhantering
- Anpassade undantag, s�som `TournamentNotFoundException` och `GameNotFoundException`, kastas av tj�nstelagret n�r entiteter inte hittas.

```csharp
// Exempel p� hur ett undantag kastas
if (tournament == null)
{
    throw new TournamentNotFoundException($"Turnering med ID {id} hittades inte.");
}

// Exempel p� hur undantaget hanteras i controllern
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

- `MaxGamesExceededException` kastas n�r maxgr�nsen f�r matcher per turnering �verskrids.
- Controllers hanterar undantagen och returnerar l�mpliga HTTP-statuskoder (t.ex. 404 f�r inte hittad, 400 f�r valideringsfel).
- Felsvaren f�ljer ProblemDetails-formatet f�r att tillhandah�lla ett konsekvent och informativt felmeddelande.

### Mappning mellan Entiteter och DTO:er
- AutoMapper anv�nds f�r att mappa mellan dom�nentiteter (`TournamentDetails` och `Game`) och motsvarande DTO:er (`TournamentDto`, `TournamentForCreationDto`, `TournamentForUpdateDto`, `GameDto`, `GameForCreationDto`, `GameForUpdateDto`).
- Mappningskonfigurationen definieras i klassen `TournamentMappings`, som �rver fr�n AutoMappers `Profile`-klass.

### Implementering av Aff�rsregler
- Klassen `GameService` uppr�tth�ller aff�rsregeln om att begr�nsa antalet matcher per turnering till 10.
- Vid skapande av en ny match kontrollerar metoden `GameService.CreateGameAsync` det aktuella antalet matcher f�r turneringen och kastar `MaxGamesExceededException` om gr�nsen �verskrids.

## Framtida F�rb�ttringar och �verv�ganden

- **Filtrering och Sortering**: Implementera ytterligare query-parametrar f�r att till�ta klienter att filtrera och sortera listorna �ver turneringar och matcher.
- **Caching**: Implementera cachemekanismer f�r att f�rb�ttra prestandan f�r ofta �tkomna data, s�som turnerings- och matchdetaljer.
- **Autentisering och Auktorisering**: Integrera autentisering och auktorisering f�r att s�kra API:et och begr�nsa �tkomst till vissa operationer baserat p� anv�ndarroller.
- **Loggning och �vervakning**: Implementera detaljerad loggning och �vervakning f�r att underl�tta fels�kning och prestandaanalys.
- **Integrationstester**: Utveckla en omfattande upps�ttning integrationstester f�r att s�kerst�lla API:ets �vergripande funktionalitet och tillf�rlitlighet.
