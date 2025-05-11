import React from "react";
import { Link, Route, Routes } from "react-router-dom";

import logo from "@/assets/logo.png";
import Fallback from "@/components/core/Fallback";
import { Button } from "@/components/ui/button";
import {
  NavigationMenu,
  NavigationMenuItem,
  NavigationMenuLink,
  NavigationMenuList,
  navigationMenuTriggerStyle,
} from "@/components/ui/navigation-menu";
import menu from "@/configs/menu";
import NotFoundPage from "@/pages/_404";
import HomePage from "@/pages/Home";

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
  return (
    <>
      <header className="flex justify-between p-3 shadow-sm">
        <Link to="/">
          <img src={logo} alt="Hanover" className="h-9" />
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
        <div>
          <Link to="/register">
            <Button className="mr-2 cursor-pointer">Sign up</Button>
          </Link>
          <Link to="/login">
            <Button className="cursor-pointer">Login</Button>
          </Link>
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
