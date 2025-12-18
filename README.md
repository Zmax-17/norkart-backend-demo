# Norkart Kontor API – .NET 10 Minimal API Demo

Live API: https://norkart-backend-demo.onrender.com/api/offices/

Dette er backend-delen av et full-stack demo jeg har laget spesielt som del av åpen søknad til Norkart.

### Teknologier
- .NET 10 (LTS)
- Minimal APIs
- In-memory lagring (for enkelhet i demoen)
- CORS konfigurert for frontend på Netlify
- Validering av unikhetskontroll på kontornavn og koordinater

### Deploy
- For rask og enkel demonstrasjon er API-et deployet på Render.com (gratis tier).
- Prosjektet er fullt kompatibelt med Azure App Service - klar for produksjon på plattform.

### Endepunkter

- **GET** `/api/offices` - hent alle kontorer
- **GET** `/api/offices/{id}` - hent kontor etter ID
- **POST** `/api/offices` - legg til nytt kontor (med sjekk på duplikater og gyldige koordinater)
- **DELETE** `/api/offices/{id}` - slett kontor
- **POST** `/api/reset` - tilbakestill data til opprinnelige 4 kontorer (praktisk i demo)
- **GET** `/api/hello` - hilsen "Klar for Norkart!"

Kristiansand-kontoret mangler med vilje - legg det til via frontend!

### Frontend
Live demo: https://norkart-frontend-demo.netlify.app
Kildekode: https://github.com/Zmax-17/norkart-frontend-demo

Laget på en uke fra scratch spesielt for Norkart.  
Gleder meg til å utvikle ekte GIS-løsninger på .NET + Azure sammen med dere!

Desember 2025  
Zmaxim17
