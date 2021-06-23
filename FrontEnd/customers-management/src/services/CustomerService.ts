import Customer from "../types/Customer"

export async function getCustomers(): Promise<Customer[]> {
    const response = await fetch("/customers")
    if(!response.ok) {  
        console.log(response.statusText);
        throw new Error(response.statusText)
    }
    const customers = await response.json();
    return customers;
}

export async function addCustomer(customer: Customer): Promise<void> {
    const response = await fetch("/customers", {
        method: 'POST',
        headers: { 'Content-Type' : 'application/json' },
        body: JSON.stringify(customer)
    })
    if(!response.ok) {  
        console.log(response.statusText);
        throw new Error(response.statusText)
    }
}