import { createContext, useContext, useMemo, useState } from "react";
import { jwtDecode } from "jwt-decode";

const AuthContext = createContext(null);

const TOKEN_KEY = "foodbook_token";
const USER_KEY = "foodbook_user";

function readStoredUser() {
  try {
    const value = localStorage.getItem(USER_KEY);
    return value ? JSON.parse(value) : null;
  } catch {
    return null;
  }
}

function getIdFromToken(token) {
  try {
    const decoded = jwtDecode(token);

    return (
      decoded.nameid ||
      decoded.sub ||
      decoded[
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
      ] ||
      null
    );
  } catch {
    return null;
  }
}

function isTokenExpired(token) {
  try {
    const decoded = jwtDecode(token);

    if (!decoded.exp) {
      return false;
    }

    return decoded.exp * 1000 <= Date.now();
  } catch {
    return true;
  }
}

export function AuthProvider({ children }) {
  const storedToken = localStorage.getItem(TOKEN_KEY);

  const [token, setToken] = useState(
    storedToken && !isTokenExpired(storedToken)
      ? storedToken
      : null
  );

  const [user, setUser] = useState(() => {
    if (!storedToken || isTokenExpired(storedToken)) {
      localStorage.removeItem(TOKEN_KEY);
      localStorage.removeItem(USER_KEY);
      return null;
    }

    return readStoredUser();
  });

  const login = (response) => {
    const nextToken = response.token;

    const nextUser = {
      id: getIdFromToken(nextToken),
      userName: response.userName,
      email: response.email,
    };

    localStorage.setItem(TOKEN_KEY, nextToken);
    localStorage.setItem(USER_KEY, JSON.stringify(nextUser));

    setToken(nextToken);
    setUser(nextUser);
  };

  const logout = () => {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(USER_KEY);

    setToken(null);
    setUser(null);
  };

  const value = useMemo(
    () => ({
      token,
      user,
      isAuthenticated: Boolean(token && user),
      login,
      logout,
    }),
    [token, user]
  );

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);

  if (!context) {
    throw new Error(
      "useAuth mora biti korišten unutar AuthProvidera."
    );
  }

  return context;
}
