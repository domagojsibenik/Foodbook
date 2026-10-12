import { Link } from "react-router-dom";

export default function RecipeCard({ recipe }) {
  const likesCount =
    recipe.likesCount ?? recipe.likes?.length ?? 0;

  const commentsCount =
    recipe.commentsCount ?? recipe.comments?.length ?? 0;

  return (
    <article className="recipe-card">
      <Link
        to={`/recipe/${recipe.id}`}
        className="recipe-image-wrap"
      >
        <img
          src={recipe.imageUrl}
          alt={recipe.name}
          className="recipe-image"
          onError={(event) => {
            event.currentTarget.src =
              "https://placehold.co/800x500?text=Foodbook";
          }}
        />
      </Link>

      <div className="recipe-card-body">
        <div className="recipe-meta-row">
          <span className="pill">
            {recipe.cookingTimeInMinutes} min
          </span>

          <span className="muted">
            ♥ {likesCount} · 💬 {commentsCount}
          </span>
        </div>

        <Link to={`/recipe/${recipe.id}`}>
          <h2 className="recipe-card-title">
            {recipe.name}
          </h2>
        </Link>

        <p className="recipe-card-description">
          {recipe.description}
        </p>

        {recipe.user && (
          <Link
            to={`/users/${recipe.user.id}`}
            state={{ userName: recipe.user.userName }}
            className="author-link"
          >
            <span className="avatar avatar-small">
              {recipe.user.userName?.[0]?.toUpperCase() ||
                "U"}
            </span>
            {recipe.user.userName}
          </Link>
        )}
      </div>
    </article>
  );
}
