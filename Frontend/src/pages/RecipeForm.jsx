import { useEffect, useState } from "react";
import {
  Link,
  useNavigate,
  useParams,
} from "react-router-dom";
import { api } from "../services/api";
import { useAuth } from "../context/AuthContext";
import Alert from "../components/Alert";
import Loading from "../components/Loading";

const emptyForm = {
  name: "",
  description: "",
  cookingTimeInMinutes: "",
  imageUrl: "",
};

export default function RecipeForm() {
  const { id } = useParams();
  const navigate = useNavigate();
  const { user, token } = useAuth();

  const isEditing = Boolean(id);

  const [form, setForm] = useState(emptyForm);
  const [loading, setLoading] = useState(isEditing);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    if (!isEditing) {
      return;
    }

    async function loadRecipe() {
      try {
        setError("");

        const recipe = await api.getRecipe(id);

        if (recipe.user?.id !== user?.id) {
          setError(
            "Nemaš dopuštenje za uređivanje ovog recepta."
          );
          return;
        }

        setForm({
          name: recipe.name || "",
          description: recipe.description || "",
          cookingTimeInMinutes:
            recipe.cookingTimeInMinutes || "",
          imageUrl: recipe.imageUrl || "",
        });
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    }

    loadRecipe();
  }, [id, isEditing, user]);

  const updateField = (event) => {
    setForm((current) => ({
      ...current,
      [event.target.name]: event.target.value,
    }));
  };

  const handleSubmit = async (event) => {
    event.preventDefault();

    const payload = {
      name: form.name.trim(),
      description: form.description.trim(),
      cookingTimeInMinutes: Number(
        form.cookingTimeInMinutes
      ),
      imageUrl: form.imageUrl.trim(),
    };

    try {
      setSubmitting(true);
      setError("");

      if (isEditing) {
        await api.updateRecipe(id, payload, token);
        navigate(`/recipe/${id}`);
      } else {
        const created = await api.createRecipe(
          payload,
          token
        );

        if (created?.id) {
          navigate(`/recipe/${created.id}`);
        } else {
          navigate("/profile");
        }
      }
    } catch (err) {
      setError(err.message);
    } finally {
      setSubmitting(false);
    }
  };

  if (loading) {
    return <Loading text="Dohvaćam recept..." />;
  }

  return (
    <div className="form-page">
      <div className="form-page-header">
        <div>
          <span className="eyebrow">
            {isEditing
              ? "Uredi svoj recept"
              : "Podijeli nešto ukusno"}
          </span>

          <h1>
            {isEditing
              ? "Uredi recept"
              : "Novi recept"}
          </h1>
        </div>

        <Link
          to={isEditing ? `/recipe/${id}` : "/"}
          className="button button-ghost"
        >
          Odustani
        </Link>
      </div>

      <Alert>{error}</Alert>

      <form
        className="recipe-form card"
        onSubmit={handleSubmit}
      >
        <label>
          Naziv recepta
          <input
            name="name"
            value={form.name}
            onChange={updateField}
            maxLength={100}
            required
            placeholder="npr. Slavonski čobanac"
          />
        </label>

        <label>
          Opis
          <textarea
            name="description"
            value={form.description}
            onChange={updateField}
            maxLength={2000}
            required
            placeholder="Opiši recept, sastojke ili postupak..."
          />
        </label>

        <div className="form-row">
          <label>
            Vrijeme pripreme (min)
            <input
              name="cookingTimeInMinutes"
              type="number"
              min="1"
              max="1440"
              value={form.cookingTimeInMinutes}
              onChange={updateField}
              required
            />
          </label>

          <label>
            URL slike
            <input
              name="imageUrl"
              type="url"
              value={form.imageUrl}
              onChange={updateField}
              maxLength={2048}
              required
              placeholder="https://..."
            />
          </label>
        </div>

        {form.imageUrl && (
          <div className="image-preview">
            <span className="muted mini-label">
              Pregled slike
            </span>
            <img
              src={form.imageUrl}
              alt="Pregled recepta"
              onError={(event) => {
                event.currentTarget.style.display =
                  "none";
              }}
            />
          </div>
        )}

        <button
          className="button button-primary"
          disabled={submitting}
        >
          {submitting
            ? "Spremam..."
            : isEditing
              ? "Spremi promjene"
              : "Objavi recept"}
        </button>
      </form>
    </div>
  );
}
