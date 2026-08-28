import { useEffect, useState } from "react";
import { createAccount, getAccounts } from "./api/accountsApi";
import "./App.css";

function App() {
  const [accounts, setAccounts] = useState([]);
  const [ownerName, setOwnerName] = useState("");
  const [error, setError] = useState("");

  async function loadAccounts() {
    try {
      const data = await getAccounts();

      setAccounts(data);
      setError("");
    } catch (error) {
      setError(error.message);
    }
  }

  async function handleCreateAccount(event) {
    event.preventDefault();

    try {
      await createAccount(ownerName);

      setOwnerName("");
      await loadAccounts();
    } catch (error) {
      setError(error.message);
    }
  }

  useEffect(() => {
  async function fetchAccounts() {
    await loadAccounts();
  }

  fetchAccounts();
}, []);

  return (
    <main>
      <h1>Banking App</h1>

      <form onSubmit={handleCreateAccount}>
        <input
          value={ownerName}
          onChange={(event) => setOwnerName(event.target.value)}
          placeholder="Owner name"
        />

        <button type="submit">Create Account</button>
      </form>

      {error && <p>{error}</p>}

      <h2>Accounts</h2>

      {accounts.length === 0 ? (
        <p>No accounts yet.</p>
      ) : (
        <ul>
          {accounts.map((account) => (
            <li key={account.accountNumber}>
              {account.ownerName} — {account.accountNumber} — Balance:{" "}
              {account.balance}
            </li>
          ))}
        </ul>
      )}
    </main>
  );
}

export default App;