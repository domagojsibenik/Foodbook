import { Link, NavLink, useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

export default function Navbar() {
  const { user, isAuthenticated, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate("/");
  };

  return (
    <header className="topbar">
      <nav className="navbar">
        <Link to="/" className="brand">
          <span className="brand-mark">F</span>
          Foodbook
        </Link>

        <div className="nav-links">
          <NavLink to="/">Recepti</NavLink>

          {isAuthenticated && (
            <NavLink to="/recipe/new">Novi recept</NavLink>
          )}
        </div>

        <div className="nav-user">
          {isAuthenticated ? (
            <>
              <Link to="/profile" className="user-chip">
                <span className="avatar avatar-small">
                  {user?.userName?.[0]?.toUpperCase() || "U"}
                </span>
                <span>{user?.userName}</span>
              </Link>

              <button
                type="button"
                className="button button-secondary button-small"
                onClick={handleLogout}
              >
                Odjava
              </button>
            </>
          ) : (
            <>
              <Link
                to="/login"
                className="button button-ghost button-small"
              >
                Prijava
              </Link>

              <Link
                to="/register"
                className="button button-primary button-small"
              >
                Registracija
              </Link>
            </>
          )}
        </div>
      </nav>
    </header>
  );
}
