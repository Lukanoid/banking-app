import { BrowserRouter, Route, Routes } from "react-router";
import "./App.css";
import AccountDetailsPage from "./pages/AccountDetailsPage";
import AccountsPage from "./pages/AccountsPage";
import { TransferPage } from "./pages/TransferPage";

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
          <Route 
          path="/accounts/:accountNumber/transfer"
          element={<TransferPage />}
          />
        </Routes>
      </main>
    </BrowserRouter>
  );
}

export default App;