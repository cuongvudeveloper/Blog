import React from "react";
import { Link, Route, Routes } from "react-router-dom";

import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { Button } from "@/components/ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import Fallback from "@/components/ui/fallback";
import Logo from "@/components/ui/logo";
import { ModeToggle } from "@/components/ui/mode-toggle";
import {
  NavigationMenu,
  NavigationMenuItem,
  NavigationMenuLink,
  NavigationMenuList,
  navigationMenuTriggerStyle,
} from "@/components/ui/navigation-menu";
import menu from "@/configs/menu";
import NotFoundPage from "@/pages/_404";
import HomePage from "@/pages/home";

import { useAuth } from "../hooks/auth-context";

const pages = import.meta.glob("@/pages/**/index.tsx") as Record<
  string,
  () => Promise<{ default: React.ComponentType }>
>;

const routes = Object.keys(pages).map((path) => {
  const match = path.match(/\/pages\/(.*)\/index\.tsx$/);
  if (!match) throw new Error(`Invalid path format: ${path}`);

  const name = match[1];
  const PageComponent = React.lazy(pages[path]);

  return {
    name,
    path: name.toLowerCase(),
    component: PageComponent,
  };
});

function DefaultLayout(): React.JSX.Element {
  const authContext = useAuth();

  function logout() {
    authContext.setIsAuthenticated(false);
  }

  function avatarFallback(userName?: string) {
    if (!userName) return "";

    const matches = userName.match(/[A-Z]/g) || [];
    return (matches[0] || "") + (matches[1] || "");
  }

  return (
    <>
      <header className="flex justify-between p-3 shadow-md">
        <Link to="/">
          <Logo />
        </Link>
        <NavigationMenu>
          <NavigationMenuList>
            {menu.map((item, index) => (
              <NavigationMenuItem key={index}>
                <NavigationMenuLink asChild>
                  <Link to={item.to} className={navigationMenuTriggerStyle()}>
                    {item.lable}
                  </Link>
                </NavigationMenuLink>
              </NavigationMenuItem>
            ))}
          </NavigationMenuList>
        </NavigationMenu>
        <div className="flex gap-2 items-center">
          <ModeToggle />
          {authContext.isAuthenticated ? (
            <DropdownMenu>
              <DropdownMenuTrigger>
                <Avatar className="cursor-pointer">
                  <AvatarImage src="" />
                  <AvatarFallback>
                    {avatarFallback(authContext.user?.userName)}
                  </AvatarFallback>
                </Avatar>
              </DropdownMenuTrigger>
              <DropdownMenuContent side="bottom" align="end">
                <DropdownMenuLabel>
                  Hi {authContext.user?.userName}!
                </DropdownMenuLabel>
                <DropdownMenuSeparator />
                <Link to="/profile">
                  <DropdownMenuItem className="cursor-pointer">
                    Profile
                  </DropdownMenuItem>
                </Link>
                <DropdownMenuItem onClick={logout} className="cursor-pointer">
                  Logout
                </DropdownMenuItem>
              </DropdownMenuContent>
            </DropdownMenu>
          ) : (
            <Link to="/login">
              <Button>Login</Button>
            </Link>
          )}
        </div>
      </header>
      <div className="w-screen h-[calc(100vh-(var(--spacing)*15))]">
        <React.Suspense fallback={<Fallback />}>
          <Routes>
            {routes.map(({ path, component: Component }) => (
              <Route key={path} path={path} element={<Component />} />
            ))}
            <Route path="/" element={<HomePage />} />
            <Route path="*" element={<NotFoundPage />} />
          </Routes>
        </React.Suspense>
      </div>
    </>
  );
}

export default DefaultLayout;
