const API_URL =
  import.meta.env.VITE_API_URL || "http://localhost:5019";

function buildHeaders(token, hasBody = false) {
  const headers = {};

  if (hasBody) {
    headers["Content-Type"] = "application/json";
  }

  if (token) {
    headers.Authorization = `Bearer ${token}`;
  }

  return headers;
}

async function parseResponse(response) {
  if (response.status === 204) {
    return null;
  }

  const contentType = response.headers.get("content-type") || "";

  if (contentType.includes("application/json")) {
    return await response.json();
  }

  const text = await response.text();
  return text || null;
}

async function request(path, options = {}) {
  const response = await fetch(`${API_URL}${path}`, options);
  const data = await parseResponse(response);

  if (!response.ok) {
    let message = "Dogodila se greška.";

    if (typeof data === "string" && data.trim()) {
      message = data;
    } else if (data?.title) {
      message = data.title;
    } else if (data?.message) {
      message = data.message;
    } else if (data?.errors) {
      if (Array.isArray(data.errors)) {
        message = data.errors
          .map((error) => error.description || error.code)
          .join(" ");
      } else {
        message = Object.values(data.errors)
          .flat()
          .join(" ");
      }
    }

    const error = new Error(message);
    error.status = response.status;
    error.data = data;
    throw error;
  }

  return data;
}

export const api = {
  login(credentials) {
    return request("/api/account/login", {
      method: "POST",
      headers: buildHeaders(null, true),
      body: JSON.stringify(credentials),
    });
  },

  register(payload) {
    return request("/api/account/register", {
      method: "POST",
      headers: buildHeaders(null, true),
      body: JSON.stringify(payload),
    });
  },

  getRecipes(userId) {
    const query = userId
      ? `?UserId=${encodeURIComponent(userId)}`
      : "";

    return request(`/api/recipe${query}`);
  },

  getRecipe(id) {
    return request(`/api/recipe/${id}`);
  },

  createRecipe(payload, token) {
    return request("/api/recipe", {
      method: "POST",
      headers: buildHeaders(token, true),
      body: JSON.stringify(payload),
    });
  },

  updateRecipe(id, payload, token) {
    return request(`/api/recipe/${id}`, {
      method: "PUT",
      headers: buildHeaders(token, true),
      body: JSON.stringify(payload),
    });
  },

  deleteRecipe(id, token) {
    return request(`/api/recipe/${id}`, {
      method: "DELETE",
      headers: buildHeaders(token),
    });
  },

  createComment(payload, token) {
    return request("/api/comment", {
      method: "POST",
      headers: buildHeaders(token, true),
      body: JSON.stringify(payload),
    });
  },

  updateComment(id, payload, token) {
    return request(`/api/comment/${id}`, {
      method: "PUT",
      headers: buildHeaders(token, true),
      body: JSON.stringify(payload),
    });
  },

  deleteComment(id, token) {
    return request(`/api/comment/${id}`, {
      method: "DELETE",
      headers: buildHeaders(token),
    });
  },

  createLike(recipeId, token) {
    return request("/api/like", {
      method: "POST",
      headers: buildHeaders(token, true),
      body: JSON.stringify({ recipeId }),
    });
  },

  deleteLike(id, token) {
    return request(`/api/like/${id}`, {
      method: "DELETE",
      headers: buildHeaders(token),
    });
  },
};

export { API_URL };
