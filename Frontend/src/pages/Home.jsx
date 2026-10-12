import { useEffect, useMemo, useState } from "react";
import { Link } from "react-router-dom";
import { api } from "../services/api";
import RecipeCard from "../components/RecipeCard";
import Loading from "../components/Loading";
import Alert from "../components/Alert";
import { useAuth } from "../context/AuthContext";

export default function Home() {
  const { isAuthenticated } = useAuth();

  const [recipes, setRecipes] = useState([]);
  const [search, setSearch] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    async function load() {
      try {
        setLoading(true);
        setError("");

        const data = await api.getRecipes();
        setRecipes(data || []);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    }

    load();
  }, []);

  const filteredRecipes = useMemo(() => {
    const term = search.trim().toLowerCase();

    if (!term) {
      return recipes;
    }

    return recipes.filter((recipe) => {
      return (
        recipe.name?.toLowerCase().includes(term) ||
        recipe.description?.toLowerCase().includes(term) ||
        recipe.user?.userName?.toLowerCase().includes(term)
      );
    });
  }, [recipes, search]);

  return (
    <>
      <section className="hero">
        <div>
          <span className="eyebrow">
            Kuhaj · Dijeli · Otkrij
          </span>

          <h1>
            Recepti koje vrijedi
            <span> sačuvati.</span>
          </h1>

          <p>
            Otkrij recepte Foodbook zajednice i podijeli
            svoja omiljena jela.
          </p>
        </div>

        {isAuthenticated ? (
          <Link
            to="/recipe/new"
            className="button button-primary"
          >
            + Dodaj recept
          </Link>
        ) : (
          <Link
            to="/register"
            className="button button-primary"
          >
            Pridruži se
          </Link>
        )}
      </section>

      <section className="section">
        <div className="section-heading">
          <div>
            <h2>Svi recepti</h2>
            <p className="muted">
              {recipes.length} objavljenih recepata
            </p>
          </div>

          <input
            className="search-input"
            type="search"
            placeholder="Pretraži recepte..."
            value={search}
            onChange={(event) =>
              setSearch(event.target.value)
            }
          />
        </div>

        <Alert>{error}</Alert>

        {loading ? (
          <Loading text="Dohvaćam recepte..." />
        ) : filteredRecipes.length === 0 ? (
          <div className="empty-state">
            <h3>Nema recepata.</h3>
            <p>
              {search
                ? "Pokušaj s drugim pojmom pretrage."
                : "Budi prvi koji će objaviti recept."}
            </p>
          </div>
        ) : (
          <div className="recipe-grid">
            {filteredRecipes.map((recipe) => (
              <RecipeCard
                key={recipe.id}
                recipe={recipe}
              />
            ))}
          </div>
        )}
      </section>
    </>
  );
}
