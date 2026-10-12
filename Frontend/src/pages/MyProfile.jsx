import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { api } from "../services/api";
import { useAuth } from "../context/AuthContext";
import RecipeCard from "../components/RecipeCard";
import Loading from "../components/Loading";
import Alert from "../components/Alert";

export default function MyProfile() {
  const { user } = useAuth();

  const [recipes, setRecipes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    async function load() {
      try {
        setError("");
        const data = await api.getRecipes(user.id);
        setRecipes(data || []);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    }

    if (user?.id) {
      load();
    }
  }, [user]);

  return (
    <>
      <section className="profile-hero card">
        <span className="avatar avatar-xl">
          {user?.userName?.[0]?.toUpperCase() || "U"}
        </span>

        <div className="profile-main">
          <span className="eyebrow">Moj profil</span>
          <h1>{user?.userName}</h1>
          <p>{user?.email}</p>

          <div className="profile-stats">
            <strong>{recipes.length}</strong>
            <span>
              {recipes.length === 1
                ? "recept"
                : "recepata"}
            </span>
          </div>
        </div>

        <Link
          to="/recipe/new"
          className="button button-primary"
        >
          + Novi recept
        </Link>
      </section>

      <section className="section">
        <div className="section-heading">
          <div>
            <h2>Moji recepti</h2>
            <p className="muted">
              Sve što si objavio na Foodbooku.
            </p>
          </div>
        </div>

        <Alert>{error}</Alert>

        {loading ? (
          <Loading />
        ) : recipes.length === 0 ? (
          <div className="empty-state">
            <h3>Još nemaš recept.</h3>
            <p>
              Objavi svoj prvi recept i pojavit će se
              ovdje.
            </p>

            <Link
              to="/recipe/new"
              className="button button-primary"
            >
              Dodaj recept
            </Link>
          </div>
        ) : (
          <div className="recipe-grid">
            {recipes.map((recipe) => (
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
