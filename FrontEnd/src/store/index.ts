import Customer from "@/types/Customer";
import { reactive } from "@vue/runtime-dom";
import { getCustomers } from "@/services/CustomerService";
import OrderJobsProperty from '@/types/OrderJobsProperty'

const state = reactive({
    customers: [] as Array<Customer>,
    sortHeaderName : 'lastName' as OrderJobsProperty,
    orderAscending : true,
    firstNameFilter : '',
    lastNameFilter : '',
    phoneFilter : '',
    emailFilter : '',
})

const methods = {
    async loadcustomers() {
        state.customers = await getCustomers();
    }
}

const computed = {
    sortedFilteredCustomers(): Customer[] {
        const y = [...state.customers]
            .filter(x=> x.firstName.toLowerCase().includes(state.firstNameFilter.toLowerCase()))
            .filter(x=> x.lastName.toLowerCase().includes(state.lastNameFilter.toLowerCase()))
            .filter(x=> x.phone.toLowerCase().includes(state.phoneFilter.toLowerCase()))
            .filter(x=> x.email.toLowerCase().includes(state.emailFilter.toLowerCase()))
            .sort((a: Customer,b: Customer) => {
            if(state.orderAscending) {
                return a[state.sortHeaderName] > b[state.sortHeaderName] ? 1 : -1;
            }
            else {
                return a[state.sortHeaderName] > b[state.sortHeaderName] ? -1 : 1;
            }
        })
        return y;
    }
}

export default {
    state,
    methods,
    computed
}