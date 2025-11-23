# 🏋️ Andrea360 Fullstack Izazov: Upravljanje Fitnes Centrom

## 1\. Opis Funkcionalnosti Sistema

Ovaj sistem implementira mini rešenje za upravljanje fitnes centrom sa ciljem da omogući kreiranje termina, prodaju usluga i zakazivanje treninga.

### Ključne Funkcionalnosti

  * **Upravljanje Domenom:** CRUD operacije za Lokacije, Usluge (treninzi) i Termine.
  * **Autentifikacija & Autorizacija (Keycloak):** Implementirana je na osnovu uloga (`Admin`, `Zaposleni`, `Član`).
  * **Finansije (Stripe):** Omogućena kupovina usluga isključivo putem Stripe-a (u test modu).
  * **Rezervacije:** Članovi mogu rezervisati samo one usluge za koje imaju preostale kredite.
  * **Ažuriranje u Realnom Vremenu:** Koristeći Real-Time tehnologiju (SignalR / WebSockets), broj preostalih mesta na terminu se automatski ažurira kod svih aktivnih korisnika.

### Definisane Uloge

| Uloga | Email | Lozinka | Opis Funkcionalnosti |
| :--- | :--- | :--- | :--- |
| **Admin** | `admin@andrea360.com` | `Andrea360!` | Ima neograničen pristup; kreiranje Lokacija i Zaposlenih. |
| **Zaposleni** | `marko@andrea360.com` | `Andrea360!` | Kreiranje Usluga i Termina za svoju Lokaciju; upravljanje rezervacijama. |
| **Član** | `petar@gmail.com` | `Andrea360!` | Kupovina usluga i rezervisanje termina. |

-----

## 2\. Uputstvo za Pokretanje Aplikacije

Sistem se sastoji od Backend servisa, baza podataka i Keycloaka (sve u Docker-u) i Frontend aplikacije (koja se pokreće lokalno).

### Preduslovi

  * Instaliran **Docker** i **Docker Compose**.
  * Instaliran **Node.js** i **npm** (za pokretanje Frontend-a).
  * Instaliran **Git**.

### Koraci za Pokretanje

#### 2.1. Podešavanje Okruženja (`.env` fajl)

Kreirajte fajl pod nazivom `.env` u root direktorijumu projekta i popunite ga tajnim ključevima.

```env
# Keycloak tajni ključ za klijenta 'andrea360-backend'
KEYCLOAK_CLIENT_SECRET=zSYcv7f4TXTCtLKxPJ8CXww5atnwHjHX

# Stripe testni tajni ključ
STRIPE_SECRET_KEY=sk_test_...VAŠ_STRIPE_KLJUČ...
```

#### 2.2. Inicijalizacija Baza i Pokretanje Backend Servisa

Zbog automatske sinhronizacije podataka (Keycloak Realm Import i Postgres Seeding), preporučuje se resetovanje volumena pri prvom pokretanju:

1.  **Očistite i resetujte volumene** (briše sve podatke iz PostgreSQL i Keycloak baza):

    ```bash
    docker-compose down -v
    ```

2.  **Pokrenite Backend, Keycloak i Baze podataka:**
    *(Napomena: Ako je frontend servis definisan u docker-compose.yml, uklonite ga ili pokrenite samo potrebne servise).*

    ```bash
    docker-compose up --build -d
    ```

    Ovo će pokrenuti servise: `restapi` (Backend), `postgres` (Aplikaciona baza), `keycloak` i `keycloak_postgres`.

#### 2.3. Pokretanje Frontenda (Lokalno)

Da bi Angular aplikacija mogla pouzdano da komunicira sa Docker servisima, mora se pokrenuti na host mašini (lokalno).

1.  **Navigirajte do Frontend foldera:**

    ```bash
    cd frontend/andrea360-web
    ```

2.  **Instalirajte zavisnosti (ako već niste):**

    ```bash
    npm install
    ```

3.  **Pokrenite aplikaciju:**

    ```bash
    npm start
    ```

Aplikacija će biti dostupna na adresi: **`http://localhost:4200`**.

### Adrese Pokrenutih Servisa

| Servis | Adresa | Opis |
| :--- | :--- | :--- |
| **Frontend (Angular)** | `http://localhost:4200` | Glavna aplikacija za korisnike i administratore. |
| **Backend (REST API)** | `http://localhost:5000` | Glavna API adresa. |
| **Keycloak Admin** | `http://localhost:8080` | Administracija (User: `admin`, Pass: `admin`). |

-----

## 3\. Podaci za Testiranje

Sistem je inicijalno popunjen sinhronizovanim podacima. Svi korisnici koriste lozinku: **`Andrea360!`**

| Uloga | Ime | Email | Lokacija | Napomena |
| :--- | :--- | :--- | :--- | :--- |
| **Admin** | Admin Adminovic | `admin@andrea360.com` | Vračar | Glavni nalog |
| **Zaposleni** | Marko Marković | `marko@andrea360.com` | Vračar | Može kreirati termine i usluge. |
| **Član** | Petar Petrović | `petar@gmail.com` | Vračar | Ima preostale kredite za CrossFit i jednu rezervaciju. |
| **Član** | Ana Anić | `ana@gmail.com` | Novi Sad | Nema kupljene usluge (ne može rezervisati dok ne kupi). |

### Inicijalno Stanje Baze

  * **Termini:** Postoje kreirani termini u budućnosti (sa jednom rezervacijom).
  * **Krediti (Petar):** Petar ima kupljenih 5 CrossFit treninga (4 preostala nakon rezervacije).

-----

## 4\. Stripe Test Kartice

Koristite sledeće standardne test kartice za simulaciju kupovine:

| Ishod | Broj Kartice | CVC | Datum Isteka |
| :--- | :--- | :--- | :--- |
| **Uspešna Transakcija** | `4242 4242 4242 4242` | `123` | Bilo koji datum u budućnosti |
| **Neuspešna (Odbijena)** | `4000 0000 0000 0002` | `123` | Bilo koji datum u budućnosti |
| **Nedovoljno Sredstava** | `4242 4242 4242 4244` | `123` | Bilo koji datum u budućnosti |
