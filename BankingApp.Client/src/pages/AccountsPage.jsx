import { useEffect, useState } from "react";
import { createAccount, getAccounts } from "../api/accountsApi.js";
import { Link } from "react-router";

function AccountsPage() {
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
                            <Link to={`/accounts/${account.accountNumber}`}
                            style={{ cursor: "pointer", textDecoration: "underline" }}
                            >
                            {account.ownerName} - {account.accountNumber} - Balance: {" "}
                            {account.balance}
                            </Link>
                        </li>
                    ))}
                </ul>
            )}
        </main>
    );
}

export default AccountsPage;