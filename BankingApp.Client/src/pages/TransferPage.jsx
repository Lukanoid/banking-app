import { useState, useEffect } from "react";
import { Link, useParams } from "react-router";
import { transfer, getAccount } from "../api/accountsApi.js";

export function TransferPage() {
    const { accountNumber } = useParams();

    const [error, setError] = useState("");
    const [receiverAccountNumber, setReceiverAccountNumber] = useState("");
    const [amount, setAmount] = useState("");
    const [successMessage, setSuccessMessage] = useState("");
    const [account, setAccount] = useState("");

    async function handleTransfer(event) {
        event.preventDefault();

        try {
            await transfer(
                accountNumber,
                receiverAccountNumber,
                Number(amount)
            );

            setReceiverAccountNumber("");
            setAmount("");
            setError("");
            setSuccessMessage("Transfer successful.")

        } catch (error) {
            setSuccessMessage("");
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

    return (
        <section>
            <Link to={`/accounts/${accountNumber}`}>Back to account details</Link>

            <h2>Transfer Money</h2>

            <p>Sender account: {accountNumber}</p>

            {account && <p>Account owner: {account.ownerName}</p>}

            <form onSubmit={handleTransfer}>
                <input
                    value={receiverAccountNumber}
                    onChange={(event) => setReceiverAccountNumber(event.target.value)}
                    placeholder="Receiver account number"
                />

                <input
                    value={amount}
                    onChange={(event) => setAmount(event.target.value)}
                    placeholder="Amount"
                    type="number"
                />
                <button type="submit">Transfer</button>
            </form>

            {successMessage && <p>{successMessage}</p>}
            {error && <p>{error}</p>}
        </section>
    )
}