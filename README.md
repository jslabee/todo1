# Glavne komponente projekta

## ToDoApp.API

- **Vsebina**: Vsebuje ASP.NET Core API, ki omogoča interakcijo z aplikacijo preko HTTP zahtevkov.
- **Glavne funkcionalnosti**: 
  - Definira kontrolerje za operacije CRUD (ustvarjanje, branje, posodabljanje, brisanje) nad opravili.

## ToDoApp.Application

- **Vsebina**: Vsebuje poslovno logiko in storitve.
- **Glavne funkcionalnosti**:
  - Nadzira obdelavo podatkov in implementira poslovna pravila.

## ToDoApp.Domain

- **Vsebina**: Vsebuje entitete in poslovne modele.
- **Glavne funkcionalnosti**:
  - Definira podatkovne modele in logiko, ki je neodvisna od shranjevanja podatkov.

## ToDoApp.Infrastructure

- **Vsebina**: Vsebuje implementacije dostopa do podatkov, kot so repozitoriji.
- **Glavne funkcionalnosti**:
  - Skrbi za interakcijo z bazo podatkov, vključuje konfiguracijo Entity Framework Core.

## ToDoApp.Tests

- **Vsebina**: Vsebuje enote in integracijske teste.
- **Glavne funkcionalnosti**:
  - Preverja pravilnost delovanja aplikacije, vključno z API-jem, storitvami in repozitoriji.
