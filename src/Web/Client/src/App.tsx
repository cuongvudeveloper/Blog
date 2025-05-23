import React from "react";

import { ThemeProvider } from "@/components/hooks/theme-provider";
import DefaultLayout from "@/components/layouts/default-layout";
import { Toaster } from "@/components/ui/sonner";

function App(): React.JSX.Element {
  return (
    <ThemeProvider defaultTheme="dark" storageKey={import.meta.env.VITE_UI_THEME_KEY_NAME}>
      <DefaultLayout />
      <Toaster />
    </ThemeProvider>
  );
}

export default App;
