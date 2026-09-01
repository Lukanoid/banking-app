const API_BASE_URL = "https://localhost:7031";

export async function getAccounts(){
    const response = await fetch(`${API_BASE_URL}/accounts`);

    if(!response.ok){
        throw new Error("Failed to load accounts.")
    }

    return await response.json();
}

export async function getAccount(accountNumber){
    const response = await fetch(`${API_BASE_URL}/accounts/${accountNumber}`);

    if(!response.ok){
        throw new Error("Failed to load accounts.")
    }

    return await response.json();
}

export async function deposit(accountNumber, amount) {
    const response = await fetch(`${API_BASE_URL}/accounts/${accountNumber}/deposit`,{
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({ amount }),
    });

    if(!response.ok){
        throw new Error("Failed to deposit money.")
    }

    return await response.json();
}

export async function withdraw(accountNumber, amount) {
    const response = await fetch(`${API_BASE_URL}/accounts/${accountNumber}/withdraw`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({ amount }),
    });

    if(!response.ok){
        throw new Error("Failed to withdraw money.")
    }

    return await response.json();
}

export async function createAccount(ownerName){
    const response = await fetch(`${API_BASE_URL}/accounts`,{
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({ownerName}),
    });

    if(!response.ok){
        throw new Error("Failed to create account.");
    }

    return await response.json();
}

export async function getTransactions(accountNumber) {
    const response = await fetch(`${API_BASE_URL}/accounts/${accountNumber}/transactions`)

    if(!response.ok){
        throw new Error("Failed to laod transactions.")
    }

    return await response.json();
}

export async function updateOwnerName(accountNumber, ownerName){
    const response = await fetch(`${API_BASE_URL}/accounts/${accountNumber}/owner`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({ ownerName })
    });

    if(!response.ok){
        throw new Error("Failed to update owner name.")
    }

    return await response.json();
}