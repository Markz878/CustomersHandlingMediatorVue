<template>
    <button @click="createNewCustomer">CreateNewCustomer</button>
    <form>
        <input v-model="newCustomer.firstName" placeholder="First name"/>
        <input v-model="newCustomer.lastName" placeholder="Last name"/>
        <input v-model="newCustomer.phone" placeholder="Phone"/>
        <input v-model="newCustomer.email" placeholder="Email"/>
    </form>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue'
import { addCustomer } from '@/services/CustomerService'
import Customer from '@/types/Customer'

export default defineComponent({
    setup() {
        const createNewCustomer = async () => {
            await addCustomer(newCustomer.value);
            newCustomer.value = { id: 0, firstName: '', lastName: '', phone: '', email: ''}
        }

        const newCustomer = ref<Customer>({ id: 0, firstName: '', lastName: '', phone: '', email: ''});

        return { createNewCustomer, newCustomer }
    },
})
</script>
