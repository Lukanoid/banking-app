import { BrowserRouter, Route, Routes } from "react-router";
import "./App.css";
import AccountDetailsPage from "./pages/AccountDetailsPage";
import AccountsPage from "./pages/AccountsPage";

function App() {
  return (
    <BrowserRouter>
      <main>
        <h1>Banking App</h1>

        <Routes>
          <Route path="/" element={<AccountsPage />} />
          <Route
            path="/accounts/:accountNumber"
            element={<AccountDetailsPage />}
          />
        </Routes>
      </main>
    </BrowserRouter>
  );
}

export default App;