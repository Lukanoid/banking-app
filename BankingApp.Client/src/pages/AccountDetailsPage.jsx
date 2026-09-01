import { useParams, Link } from "react-router";
import { useState, useEffect } from "react";
import {
    deposit,
    withdraw,
    getAccount,
    getTransactions,
    updateOwnerName
} from "../api/accountsApi.js";


function AccountDetailsPage() {
    const { accountNumber } = useParams();

    const [account, setAccount] = useState(null);
    const [amount, setAmount] = useState("");
    const [error, setError] = useState("");
    const [transactions, setTransactions] = useState([]);
    const [newOwnerName, setNewOwnerName] = useState("");


    async function loadAccount() {
        try {
            const data = await getAccount(accountNumber)

            setAccount(data)
            setError("")
        } catch (error) {
            setError(error.message);
        }
    }

    async function loadTransactions() {
        try {
            const data = await getTransactions(accountNumber)

            setTransactions(data);
            setError("");
        } catch (error) {
            setError(error.message);
        }
    }

    async function handleDeposit() {
        try {
            await deposit(accountNumber, Number(amount));

            setAmount("");
            await loadAccount();
            await loadTransactions();
        } catch (error) {
            setError(error.message);
        }
    }

    async function handleWithdraw() {
        try {
            await withdraw(accountNumber, Number(amount));

            setAmount("")
            await loadAccount();
            await loadTransactions();
        } catch (error) {
            setError(error.message);
        }
    }

    async function handleUpdateOwnerName(event){
        event.preventDefault();

        try {
            const updatedAccount = await updateOwnerName(accountNumber, newOwnerName);

            setAccount(updatedAccount)
            setNewOwnerName("");
            setError("")
        } catch (error) {
            setError(error.message);
        }
    }

    useEffect(() => {
        let ignore = false;

        async function loadPageData() {
            try {
                const accountData = await getAccount(accountNumber);
                const transactionsData = await getTransactions(accountNumber);

                if (!ignore) {
                    setAccount(accountData);
                    setTransactions(transactionsData);
                    setError("");
                }
            } catch (error) {
                if (!ignore) {
                    setError(error.message);
                }
            }
        }

        loadPageData();

        return () => {
            ignore = true;
        };
    }, [accountNumber]);

    if (account === null) {
        return (
            <section>
                <Link to="/">Back to accounts</Link>
                <p>Loading accounts...</p>
                {error && <p>{error}</p>}
            </section>
        );
    }

    return (
        <section>
            <Link to="/">Back to accounts</Link>

            <h2>{account.ownerName}</h2>

            <p>Account Number: {account.accountNumber}</p>
            <p>Balance: {account.balance}</p>

            <h3>Update Owner Name</h3>

            <form onSubmit={handleUpdateOwnerName}>
                <input 
                value={newOwnerName}
                onChange={(event) => setNewOwnerName(event.target.value)}
                placeholder="New owner name"
                 />

                 <button type="submit">Update Owner</button>
            </form>

            <h3>Actions</h3>

            <input
                value={amount}
                onChange={(event) => setAmount(event.target.value)}
                placeholder="Amount"
                type="number"
            />
            <button type="button" onClick={handleDeposit}>
                Deposit
            </button>

            <button type="button" onClick={handleWithdraw}>
                Withdraw
            </button>

            <h3>Transactions</h3>

            {transactions.length === 0 ? (
                <p>No Transactions yet.</p>
            ) : (
                <ul>
                    {transactions.map((transaction, index) => (
                        <li key={index}>
                            {transaction.type} - {transaction.amount}  {transaction.descriptiion} - {" "}
                            {transaction.date}
                        </li>
                    ))}
                </ul>
            )}

            {error && <p>{error}</p>}
        </section>
    )

}

export default AccountDetailsPage;