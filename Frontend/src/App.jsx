import { Navigate, Route, Routes } from "react-router-dom";
import Navbar from "./components/Navbar";
import ProtectedRoute from "./components/ProtectedRoute";
import Home from "./pages/Home";
import Login from "./pages/Login";
import Register from "./pages/Register";
import RecipeDetails from "./pages/RecipeDetails";
import RecipeForm from "./pages/RecipeForm";
import MyProfile from "./pages/MyProfile";
import UserProfile from "./pages/UserProfile";
import NotFound from "./pages/NotFound";

export default function App() {
  return (
    <>
      <Navbar />

      <main className="page-shell">
        <Routes>
          <Route path="/" element={<Home />} />

          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />

          <Route path="/recipe/:id" element={<RecipeDetails />} />

          <Route element={<ProtectedRoute />}>
            <Route path="/recipe/new" element={<RecipeForm />} />
            <Route path="/recipe/:id/edit" element={<RecipeForm />} />
            <Route path="/profile" element={<MyProfile />} />
          </Route>

          <Route path="/users/:userId" element={<UserProfile />} />

          <Route path="/home" element={<Navigate to="/" replace />} />
          <Route path="*" element={<NotFound />} />
        </Routes>
      </main>
    </>
  );
}
