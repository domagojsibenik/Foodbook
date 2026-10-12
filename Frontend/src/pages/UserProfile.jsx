import { useEffect, useMemo, useState } from "react";
import { useLocation, useParams } from "react-router-dom";
import { api } from "../services/api";
import RecipeCard from "../components/RecipeCard";
import Loading from "../components/Loading";
import Alert from "../components/Alert";

export default function UserProfile() {
  const { userId } = useParams();
  const location = useLocation();

  const [recipes, setRecipes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    async function load() {
      try {
        setError("");

        const data = await api.getRecipes(userId);
        setRecipes(data || []);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    }

    load();
  }, [userId]);

  const profileUser = useMemo(() => {
    return (
      recipes[0]?.user || {
        id: userId,
        userName:
          location.state?.userName || "Korisnik",
      }
    );
  }, [recipes, userId, location.state]);

  return (
    <>
      <section className="profile-hero card">
        <span className="avatar avatar-xl">
          {profileUser.userName?.[0]?.toUpperCase() ||
            "U"}
        </span>

        <div className="profile-main">
          <span className="eyebrow">
            Foodbook profil
          </span>
          <h1>{profileUser.userName}</h1>

          <div className="profile-stats">
            <strong>{recipes.length}</strong>
            <span>
              {recipes.length === 1
                ? "recept"
                : "recepata"}
            </span>
          </div>
        </div>
      </section>

      <section className="section">
        <div className="section-heading">
          <div>
            <h2>Recepti korisnika</h2>
            <p className="muted">
              Objavljeni recepti na Foodbooku.
            </p>
          </div>
        </div>

        <Alert>{error}</Alert>

        {loading ? (
          <Loading />
        ) : recipes.length === 0 ? (
          <div className="empty-state">
            <h3>Nema objavljenih recepata.</h3>
            <p>
              Ovaj korisnik još nije objavio recept.
            </p>
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
