<template>
    <table class="customer-table">
      <thead>
        <th @click="sortHeader('firstName')">First name</th>
        <th @click="sortHeader('lastName')">Last name</th>
        <th @click="sortHeader('phone')">Phone number</th>
        <th @click="sortHeader('email')">Email</th>
      </thead>
      <thead>
        <th><input v-model="store.state.firstNameFilter" placeholder="Search by first name"/></th>
        <th><input v-model="store.state.lastNameFilter" placeholder="Search by last name"/></th>
        <th><input v-model="store.state.phoneFilter" placeholder="Search by phone number"/></th>
        <th><input v-model="store.state.emailFilter" placeholder="Search by email"/></th>
      </thead>
      <tbody>
        <tr class="customer-list" v-for="customer in store.computed.sortedFilteredCustomers()" :key="customer.id">
          <td>{{customer.firstName}}</td>
          <td>{{customer.lastName}}</td>
          <td>{{customer.phone}}</td>
          <td>{{customer.email}}</td>
        </tr>
      </tbody>
    </table>
</template>

<script lang="ts">
import { defineComponent, inject } from 'vue'
import Store from '@/types/Store'
import OrderJobsProperty from '@/types/OrderJobsProperty'
export default defineComponent({
    setup() {
        const store = inject('store') as Store;
        const sortHeader = (header: OrderJobsProperty) => {
            store.state.sortHeaderName = header;
            store.state.orderAscending = !store.state.orderAscending;
        }
        return { store, sortHeader }
    }
})
</script>

<style scoped>
    table{
        margin: auto;
        border: 1px solid black;
    }

    td {
        padding: 5px;
    }

    tr:nth-child(odd) {
        background: lightgray;
    }

</style>
