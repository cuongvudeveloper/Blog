import React from "react";
import { Route, Routes } from "react-router-dom";

import Fallback from "./common/Fallback";
import DefaultLayout from "./layouts/DefaultLayout";
import NotFoundPage from "./pages/_404";
import HomePage from "./pages/Home";

const pages = import.meta.glob("./pages/**/index.jsx");

const routes = Object.keys(pages).map((path) => {
  const name = path.match(/\.\/pages\/(.*)\/index\.jsx$/)[1];
  const page = React.lazy(pages[path]);

  return {
    name,
    path: name.toLowerCase(),
    page: page,
  };
});

function App() {
  return (
    <>
      <DefaultLayout />

      <div className="w-screen h-[calc(100vh-(var(--spacing)*15))] bg-gray-100">
        <React.Suspense fallback={<Fallback />}>
          <Routes>
            {routes.map(({ path, page }) => (
              <Route
                key={path}
                path={path}
                element={React.createElement(page)}
              />
            ))}
            <Route path="/" element={<HomePage />} />
            <Route path="*" element={<NotFoundPage />} />
          </Routes>
        </React.Suspense>
      </div>
    </>
  );
}

export default App;
