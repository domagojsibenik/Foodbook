import { useEffect, useMemo, useState } from "react";
import {
  Link,
  useNavigate,
  useParams,
} from "react-router-dom";
import { api } from "../services/api";
import { useAuth } from "../context/AuthContext";
import Loading from "../components/Loading";
import Alert from "../components/Alert";

function getUserId(item) {
  return item?.user?.id ?? item?.userId ?? null;
}

export default function RecipeDetails() {
  const { id } = useParams();
  const navigate = useNavigate();

  const {
    user,
    token,
    isAuthenticated,
  } = useAuth();

  const [recipe, setRecipe] = useState(null);
  const [commentText, setCommentText] = useState("");
  const [editingCommentId, setEditingCommentId] =
    useState(null);
  const [editingText, setEditingText] = useState("");

  const [loading, setLoading] = useState(true);
  const [actionLoading, setActionLoading] =
    useState(false);
  const [error, setError] = useState("");

  const loadRecipe = async () => {
    try {
      setError("");
      const data = await api.getRecipe(id);
      setRecipe(data);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    setLoading(true);
    loadRecipe();
  }, [id]);

  const myLike = useMemo(() => {
    return recipe?.likes?.find(
      (like) => getUserId(like) === user?.id
    );
  }, [recipe, user]);

  const isOwner = recipe?.user?.id === user?.id;

  const handleLike = async () => {
    if (!isAuthenticated) {
      navigate("/login", {
        state: { from: `/recipe/${id}` },
      });
      return;
    }

    try {
      setActionLoading(true);
      setError("");

      if (myLike) {
        await api.deleteLike(myLike.id, token);
      } else {
        await api.createLike(Number(id), token);
      }

      await loadRecipe();
    } catch (err) {
      setError(err.message);
    } finally {
      setActionLoading(false);
    }
  };

  const handleCommentSubmit = async (event) => {
    event.preventDefault();

    if (!isAuthenticated) {
      navigate("/login", {
        state: { from: `/recipe/${id}` },
      });
      return;
    }

    const text = commentText.trim();

    if (!text) {
      return;
    }

    try {
      setActionLoading(true);
      setError("");

      await api.createComment(
        {
          text,
          recipeId: Number(id),
        },
        token
      );

      setCommentText("");
      await loadRecipe();
    } catch (err) {
      setError(err.message);
    } finally {
      setActionLoading(false);
    }
  };

  const beginEditComment = (comment) => {
    setEditingCommentId(comment.id);
    setEditingText(comment.text);
  };

  const saveComment = async (commentId) => {
    const text = editingText.trim();

    if (!text) {
      return;
    }

    try {
      setActionLoading(true);
      setError("");

      await api.updateComment(
        commentId,
        { text },
        token
      );

      setEditingCommentId(null);
      setEditingText("");
      await loadRecipe();
    } catch (err) {
      setError(err.message);
    } finally {
      setActionLoading(false);
    }
  };

  const deleteComment = async (commentId) => {
    if (!window.confirm("Obrisati komentar?")) {
      return;
    }

    try {
      setActionLoading(true);
      setError("");

      await api.deleteComment(commentId, token);
      await loadRecipe();
    } catch (err) {
      setError(err.message);
    } finally {
      setActionLoading(false);
    }
  };

  const deleteRecipe = async () => {
    if (
      !window.confirm(
        "Jesi li siguran da želiš obrisati recept?"
      )
    ) {
      return;
    }

    try {
      setActionLoading(true);
      setError("");

      await api.deleteRecipe(recipe.id, token);
      navigate("/");
    } catch (err) {
      setError(err.message);
      setActionLoading(false);
    }
  };

  if (loading) {
    return <Loading text="Dohvaćam recept..." />;
  }

  if (!recipe) {
    return (
      <div className="empty-state">
        <Alert>{error || "Recept nije pronađen."}</Alert>
        <Link to="/" className="button button-secondary">
          Natrag na recepte
        </Link>
      </div>
    );
  }

  return (
    <div className="details-layout">
      <Alert>{error}</Alert>

      <article className="recipe-details-card">
        <img
          src={recipe.imageUrl}
          alt={recipe.name}
          className="details-image"
          onError={(event) => {
            event.currentTarget.src =
              "https://placehold.co/1200x700?text=Foodbook";
          }}
        />

        <div className="details-content">
          <div className="details-topline">
            <span className="pill">
              {recipe.cookingTimeInMinutes} min
            </span>

            {isOwner && (
              <div className="owner-actions">
                <Link
                  to={`/recipe/${recipe.id}/edit`}
                  className="button button-secondary button-small"
                >
                  Uredi
                </Link>

                <button
                  className="button button-danger button-small"
                  onClick={deleteRecipe}
                  disabled={actionLoading}
                >
                  Obriši
                </button>
              </div>
            )}
          </div>

          <h1>{recipe.name}</h1>

          {recipe.user && (
            <Link
              to={`/users/${recipe.user.id}`}
              state={{
                userName: recipe.user.userName,
              }}
              className="author-link author-large"
            >
              <span className="avatar">
                {recipe.user.userName?.[0]?.toUpperCase() ||
                  "U"}
              </span>

              <div>
                <span className="muted mini-label">
                  Autor
                </span>
                <strong>{recipe.user.userName}</strong>
              </div>
            </Link>
          )}

          <p className="details-description">
            {recipe.description}
          </p>

          <div className="recipe-actions-bar">
            <button
              className={`like-button ${
                myLike ? "liked" : ""
              }`}
              onClick={handleLike}
              disabled={actionLoading}
            >
              {myLike ? "♥" : "♡"}{" "}
              {recipe.likes?.length ?? 0}
            </button>

            <span className="muted">
              💬 {recipe.comments?.length ?? 0}
            </span>
          </div>
        </div>
      </article>

      <section className="comments-panel">
        <div className="section-heading">
          <div>
            <h2>Komentari</h2>
            <p className="muted">
              {recipe.comments?.length ?? 0} komentara
            </p>
          </div>
        </div>

        {isAuthenticated ? (
          <form
            className="comment-form"
            onSubmit={handleCommentSubmit}
          >
            <textarea
              placeholder="Napiši komentar..."
              value={commentText}
              onChange={(event) =>
                setCommentText(event.target.value)
              }
              maxLength={1000}
              required
            />

            <div className="comment-form-footer">
              <span className="muted">
                {commentText.length}/1000
              </span>

              <button
                className="button button-primary"
                disabled={actionLoading}
              >
                Objavi komentar
              </button>
            </div>
          </form>
        ) : (
          <div className="login-callout">
            <span>
              Prijavi se kako bi mogao komentirati.
            </span>

            <Link
              to="/login"
              state={{ from: `/recipe/${id}` }}
              className="button button-primary button-small"
            >
              Prijava
            </Link>
          </div>
        )}

        <div className="comment-list">
          {!recipe.comments?.length ? (
            <div className="empty-state compact">
              <p>Još nema komentara.</p>
            </div>
          ) : (
            recipe.comments.map((comment) => {
              const commentUserId = getUserId(comment);
              const ownsComment =
                commentUserId === user?.id;

              return (
                <article
                  key={comment.id}
                  className="comment-card"
                >
                  <div className="comment-card-header">
                    {comment.user ? (
                      <Link
                        to={`/users/${comment.user.id}`}
                        state={{
                          userName:
                            comment.user.userName,
                        }}
                        className="author-link"
                      >
                        <span className="avatar avatar-small">
                          {comment.user.userName?.[0]?.toUpperCase() ||
                            "U"}
                        </span>
                        {comment.user.userName}
                      </Link>
                    ) : (
                      <span className="muted">
                        Korisnik
                      </span>
                    )}

                    <span className="muted">
                      {comment.createdAt
                        ? new Date(
                            comment.createdAt
                          ).toLocaleString("hr-HR")
                        : ""}
                    </span>
                  </div>

                  {editingCommentId === comment.id ? (
                    <div className="comment-edit">
                      <textarea
                        value={editingText}
                        onChange={(event) =>
                          setEditingText(
                            event.target.value
                          )
                        }
                        maxLength={1000}
                      />

                      <div className="inline-actions">
                        <button
                          className="button button-primary button-small"
                          onClick={() =>
                            saveComment(comment.id)
                          }
                          disabled={actionLoading}
                        >
                          Spremi
                        </button>

                        <button
                          className="button button-ghost button-small"
                          onClick={() =>
                            setEditingCommentId(null)
                          }
                          type="button"
                        >
                          Odustani
                        </button>
                      </div>
                    </div>
                  ) : (
                    <p>{comment.text}</p>
                  )}

                  {ownsComment &&
                    editingCommentId !== comment.id && (
                      <div className="inline-actions">
                        <button
                          className="text-button"
                          onClick={() =>
                            beginEditComment(comment)
                          }
                          type="button"
                        >
                          Uredi
                        </button>

                        <button
                          className="text-button danger-text"
                          onClick={() =>
                            deleteComment(comment.id)
                          }
                          type="button"
                        >
                          Obriši
                        </button>
                      </div>
                    )}
                </article>
              );
            })
          )}
        </div>
      </section>
    </div>
  );
}
