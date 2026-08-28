const API_BASE_URL = "https://localhost:7031";

export async function getAccounts(){
    const response = await fetch(`${API_BASE_URL}/accounts`);
    if(!response.ok){
        throw new Error("Failed to load accounts.")
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