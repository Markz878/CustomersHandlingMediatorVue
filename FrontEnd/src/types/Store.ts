import Customer from "./Customer"
import OrderJobsProperty from "./OrderJobsProperty";

interface Store {
    state: {
        customers: Customer[],
        sortHeaderName : OrderJobsProperty,
        orderAscending : boolean,
        firstNameFilter : string,
        lastNameFilter : string,
        phoneFilter : string,
        emailFilter : string,
    },
    methods: {
        loadcustomers(): Promise<void>
    },
    computed: {
        sortedFilteredCustomers(): Customer[]
    }
}

export default Store