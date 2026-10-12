import { useState } from "react";
import {
  Link,
  Navigate,
  useLocation,
  useNavigate,
} from "react-router-dom";
import { api } from "../services/api";
import { useAuth } from "../context/AuthContext";
import Alert from "../components/Alert";

export default function Login() {
  const { login, isAuthenticated } = useAuth();

  const navigate = useNavigate();
  const location = useLocation();

  const [form, setForm] = useState({
    username: "",
    password: "",
  });

  const [error, setError] = useState("");
  const [submitting, setSubmitting] = useState(false);

  if (isAuthenticated) {
    return <Navigate to="/" replace />;
  }

  const updateField = (event) => {
    setForm((current) => ({
      ...current,
      [event.target.name]: event.target.value,
    }));
  };

  const handleSubmit = async (event) => {
    event.preventDefault();

    try {
      setSubmitting(true);
      setError("");

      const response = await api.login(form);
      login(response);

      navigate(location.state?.from || "/", {
        replace: true,
      });
    } catch (err) {
      setError(err.message);
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div className="auth-layout">
      <section className="auth-card">
        <div className="auth-heading">
          <span className="eyebrow">Dobrodošao natrag</span>
          <h1>Prijava</h1>
          <p>
            Prijavi se kako bi mogao objavljivati,
            komentirati i lajkovati recepte.
          </p>
        </div>

        <Alert>{error}</Alert>

        <form onSubmit={handleSubmit} className="form">
          <label>
            Korisničko ime
            <input
              name="username"
              value={form.username}
              onChange={updateField}
              autoComplete="username"
              required
            />
          </label>

          <label>
            Lozinka
            <input
              name="password"
              type="password"
              value={form.password}
              onChange={updateField}
              autoComplete="current-password"
              required
            />
          </label>

          <button
            className="button button-primary button-full"
            disabled={submitting}
          >
            {submitting
              ? "Prijavljujem..."
              : "Prijavi se"}
          </button>
        </form>

        <p className="auth-switch">
          Nemaš račun?{" "}
          <Link to="/register">Registriraj se</Link>
        </p>
      </section>
    </div>
  );
}
