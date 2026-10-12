# Foodbook frontend

React/Vite frontend za Foodbook ASP.NET Core backend.

## Pokretanje

```bash
npm install
cp .env.example .env
npm run dev
```

Na Windows PowerShellu umjesto `cp` možeš:

```powershell
Copy-Item .env.example .env
```

Backend je po defaultu očekivan na:

```text
http://localhost:5019
```

Ako ti radi na drugoj adresi, promijeni `VITE_API_URL` u `.env`.

## Glavne rute

- `/` - svi recepti
- `/login` - prijava
- `/register` - registracija
- `/recipe/new` - novi recept
- `/recipe/:id` - detalji recepta
- `/recipe/:id/edit` - uređivanje vlastitog recepta
- `/profile` - profil prijavljenog korisnika
- `/users/:userId` - javni profil korisnika

## Napomena za backend

Frontend očekuje da `RecipeResponseDTO` sadrži:

```json
{
  "id": 1,
  "name": "Sarma",
  "description": "...",
  "cookingTimeInMinutes": 120,
  "imageUrl": "https://...",
  "user": {
    "id": "...",
    "userName": "dominik"
  },
  "comments": [
    {
      "id": 1,
      "text": "...",
      "createdAt": "...",
      "recipeId": 1,
      "user": {
        "id": "...",
        "userName": "dominik"
      }
    }
  ],
  "likes": [
    {
      "id": 1,
      "recipeId": 1,
      "createdAt": "...",
      "user": {
        "id": "...",
        "userName": "dominik"
      }
    }
  ]
}
```

Za pravi javni profil korisnika bez recepata backend bi idealno trebao imati
`GET /api/user/{id}`. Trenutna stranica javnog profila pokušava izvući podatke
korisnika iz njegovih recepata.
