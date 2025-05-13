import React from "react";

import { ThemeProvider } from "@/components/core/theme-provider";
import DefaultLayout from "@/components/layouts/default-layout";
import { Toaster } from "@/components/ui/sonner";

function App(): React.JSX.Element {
  return (
    <ThemeProvider defaultTheme="dark" storageKey="vite-ui-theme">
      <DefaultLayout />
      <Toaster />
    </ThemeProvider>
  );
}

export default App;
