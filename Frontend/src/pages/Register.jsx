import { useState } from "react";
import {
  Link,
  Navigate,
  useNavigate,
} from "react-router-dom";
import { api } from "../services/api";
import { useAuth } from "../context/AuthContext";
import Alert from "../components/Alert";

export default function Register() {
  const { login, isAuthenticated } = useAuth();
  const navigate = useNavigate();

  const [form, setForm] = useState({
    username: "",
    email: "",
    password: "",
    confirmPassword: "",
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

    if (form.password !== form.confirmPassword) {
      setError("Lozinke se ne podudaraju.");
      return;
    }

    try {
      setSubmitting(true);
      setError("");

      const response = await api.register({
        username: form.username,
        email: form.email,
        password: form.password,
      });

      login(response);
      navigate("/", { replace: true });
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
          <span className="eyebrow">Novi račun</span>
          <h1>Registracija</h1>
          <p>
            Napravi Foodbook račun i počni dijeliti svoje
            recepte.
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
              minLength={3}
              maxLength={30}
              required
            />
          </label>

          <label>
            Email
            <input
              name="email"
              type="email"
              value={form.email}
              onChange={updateField}
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
              minLength={12}
              required
            />
          </label>

          <label>
            Ponovi lozinku
            <input
              name="confirmPassword"
              type="password"
              value={form.confirmPassword}
              onChange={updateField}
              minLength={12}
              required
            />
          </label>

          <button
            className="button button-primary button-full"
            disabled={submitting}
          >
            {submitting
              ? "Kreiram račun..."
              : "Registriraj se"}
          </button>
        </form>

        <p className="auth-switch">
          Već imaš račun?{" "}
          <Link to="/login">Prijavi se</Link>
        </p>
      </section>
    </div>
  );
}
