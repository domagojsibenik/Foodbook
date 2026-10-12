import { Link } from "react-router-dom";

export default function NotFound() {
  return (
    <div className="empty-state not-found">
      <span className="eyebrow">404</span>
      <h1>Stranica nije pronađena.</h1>
      <p>
        Adresa koju si otvorio ne postoji.
      </p>

      <Link to="/" className="button button-primary">
        Na naslovnicu
      </Link>
    </div>
  );
}
