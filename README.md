![helo-logo](https://github.com/user-attachments/assets/a3837815-9d8b-49b4-aa33-478be6c94e3a)

# Kodtest - .NET
Välkommen till Helos backend kodtest. Detta test är utformat för att ge dig möjlighet att visa upp dina kunskaper inom .NET och hur du arbetar med API:er.
Tanken är inte att du ska lägga flera dagar på att färdigställa API:et, och om du inte hinner med alla delar är det helt okej. Det viktigaste är att du kan visa dina kunskaper, resonera kring din tankegång och förklara de beslut du har tagit. De delar du eventuellt inte hinner med kommer vi att diskutera under intervjun, där du får möjlighet att förklara hur du skulle ha löst dem.


---
# Förberedelser 


### 1. Klona projektet

För att komma igång, klona projektets repository till din lokala miljö med länken vi har skickat till dig. 


```bash
git clone https://github.com/HAWCO/be-code-test.git
```

Innan du börjar, säkerställ att du har följande installerat:

1. **.NET**  - för att kunna utveckla och köra ditt API.
2. **Docker** - för att hantera databasens körning via `docker-compose`.

Projektet innehåller redan en controller med de endpoints du kommer behöva, du är fri att utgå ifrån detta eller om du hellre vill göra på annat sätt är det fritt fram att göra. 

Vi har förberett projektet så att du enkelt kan starta databasen genom att köra `docker-compose.yaml`-filen. 
```bash
docker compose up --build
```

En PostgreSQL-databas är förberedd och redo att användas, men du är fri att använda vilken databas du vill om du föredrar något annat. 

En länk till ett externt API finns också förkonfigurerad i projektets `appsettings`-fil.

---
# Problembeskrivning
Vi behöver ett API för att hantera våra favorit-Pokémons. Via API:et ska användarna kunna fånga, lista och släppa sina Pokémons. De huvudsakliga funktionerna är följande:

1. **Fånga Pokémons**: Användaren skickar in ett namn eller ID för en Pokémon. API:et ska söka efter Pokémonen med hjälp av [Pokémon API](https://pokeapi.co/), och om en träff hittas ska Pokémonen sparas i databasen. 

2. **Lista fångade Pokémons**: Användaren ska kunna lista sina fångade Pokémons via API:et. Det ska också vara möjligt att filtrera listan baserat på Pokémon-typ, till exempel för att visa endast gräs-Pokémons.

3. **Frigöra sina fångade Pokémons**: Användaren ska kunna släppa sina fångade Pokémons, vilket innebär att de tas bort från databasen.

---

## Databasstruktur och lagring

API:et ska spara vissa uppgifter om fångade Pokémons i databasen. Här är de minimala fält som vi vill att databasen ska innehålla för varje fångad Pokémon:

- **ID**: Ett unikt identifierare för posten i databasen.
- **Namn**: Namnet på Pokémonen (t.ex., "Pikachu").
- **Pokémon-ID**: Det unika ID som används av [Pokémon API](https://pokeapi.co/) för att identifiera Pokémonen.
- **Datum och tid för fångst**: Tidsstämpel för när Pokémonen fångades.
- **Typ av Pokémon**: Pokémonens typ (t.ex., "gräs", "eld").

Eftersom filtrering på Pokémon-typ är ett krav för listningsfunktionen, behöver typen sparas i databasen. Fundera över hur du bäst kan representera Pokémon-typen i din databasdesign – en Pokémon kan ha flera typer (t.ex. både "vatten" och "is") och det är viktigt att strukturen kan hantera detta. 


Detta är en grundstruktur för att spara de fångade Pokémons. Om du vill lägga till fler fält (t.ex., typ av Pokémon, förmågor eller andra attribut) är det fritt fram att göra så, men det är inte nödvändigt.

---

## Bedömningskriterier
Vi kommer att utvärdera din lösning utifrån följande faktorer:

* **Felhantering**: Hur väl hanteras olika typer av fel.

* **API-design**: Hur strukturerat och användarvänligt är API:et.

* **Databashantering**: Hur hanteras datalagring och databasanrop.  

* **Kodstruktur**: Hur strukturerad och lättläst är koden.

---

## Vad vi inte har krav på

För att hålla testet fokuserat och genomförbart på begränsad tid finns det vissa aspekter vi inte har krav på att inkludera i lösningen:

- **Autentisering och auktorisering** 
  Det behövs ingen inloggningsfunktion eller hantering av användarbehörigheter.

- **Användare**  
  Du behöver inte designa databasen eller API:et för att särskilja sparade Pokémons mellan olika användare. Alla som använder API:et delar på samma lista över fångade Pokémons, så det krävs ingen användarspecifik lagring.


Vidare är du fri att implementera vad du än vill för att skapa ett så bra API som möjligt. Om du vill lägga till funktionalitet eller extra detaljer som förbättrar API:et är det välkommet, men inte ett krav.

---
## Inlämning av din lösning
När du är klar med din lösning, vänligen följ ett av de två alternativen nedan för att skicka in ditt arbete:

**Alternativ 1**: Ladda upp till din egen Git-profil
Skapa ett nytt repository på din egen Git-profil (GitHub, GitLab, etc.).
Skicka länken till ditt repository till oss för granskning via mejl.

**Alternativ 2**: Skicka en zip-fil
Zippa projektmappen med alla dina ändringar.
Skicka zip-filen till oss via mejl, enligt instruktioner du har fått från oss.