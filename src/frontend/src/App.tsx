import React from "react";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import EmployeeDashboard from "./containers/EmployeeDashboard";

import "./App.scss";

const queryClient = new QueryClient();

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <div className="app-container">
        <h1>Employee Project Collaborations</h1>
        <EmployeeDashboard />
      </div>
    </QueryClientProvider>
  );
}

export default App;
