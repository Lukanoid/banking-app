import { useParams, Link } from "react-router";
import { useState, useEffect } from "react";
import { deposit, withdraw, getAccount } from "../api/accountsApi.js";


function AccountDetailsPage() {
    const { accountNumber } = useParams();

    const [account, setAccount] = useState(null);
    const [amount, setAmount] = useState("");
    const [error, setError] = useState("");


    async function loadAccount() {
        try {
            const data = await getAccount(accountNumber)

            setAccount(data)
            setError("")
        } catch (error) {
            setError(error.message);
        }
    }

    async function handleDeposit() {
        try {
            await deposit(accountNumber, Number(amount));

            setAmount("");
            await loadAccount();
        } catch (error) {
            setError(error.message);
        }
    }

    async function handleWithdraw() {
        try {
            await withdraw(accountNumber, Number(amount));

            setAmount("")
            await loadAccount();
        } catch (error) {
            setError(error.message);
        }
    }

    useEffect(() => {
        let ignore = false;

        getAccount(accountNumber)
            .then((data) => {
                if (!ignore) {
                    setAccount(data);
                    setError("");
                }
            })
            .catch((error) => {
                if (!ignore) {
                    setError(error.message);
                }
            });

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

            {error && <p>{error}</p>}
        </section>
    )

}

export default AccountDetailsPage;