import React from "react";
import { useNavigate } from "react-router-dom";

import { info, type InfoResponse } from "@/apis/oauth";

type AuthProviderProps = {
  children: React.ReactNode;
};

type AuthContextType = {
  isAuthenticated: boolean;
  user?: InfoResponse;
  // eslint-disable-next-line no-unused-vars
  setIsAuthenticated: (isAuthenticated: boolean) => void;
};

const AuthContext = React.createContext<AuthContextType>({
  isAuthenticated: false,
  user: undefined,
  setIsAuthenticated: () => null,
});

// eslint-disable-next-line react-refresh/only-export-components
export const useAuth = () => React.useContext(AuthContext);

export function AuthProvider({ children }: AuthProviderProps) {
  const [isAuthenticated, setIsAuthenticated] = React.useState<boolean>(
    !!localStorage.getItem(import.meta.env.VITE_ACCESS_TOKEN_KEY_NAME)
  );
  const [user, setUser] = React.useState<InfoResponse>();
  const navigate = useNavigate();

  React.useEffect(() => {
    if (!isAuthenticated) {
      localStorage.removeItem(import.meta.env.VITE_ACCESS_TOKEN_KEY_NAME);
      navigate("/login");
      return;
    }

    info()
      .then((response) => {
        if (response?.succeeded) {
          setUser(response?.data);
        }
      })
      .catch(() => {
        localStorage.removeItem(import.meta.env.VITE_ACCESS_TOKEN_KEY_NAME);
        navigate("/login");
        setIsAuthenticated(false);
      });
  }, [isAuthenticated, navigate]);

  return (
    <AuthContext.Provider value={{ isAuthenticated, user, setIsAuthenticated }}>
      {children}
    </AuthContext.Provider>
  );
}
