<template>
  <div class="home">
    <h1>Customers management</h1>
    <AddCustomerView/>
    <CustomersView/>
  </div>
</template>

<script lang="ts">
import { defineComponent, inject } from 'vue';
import CustomersView from '@/components/CustomersView.vue'; // @ is an alias to /src
import AddCustomerView from '@/components/AddCustomerView.vue'; // @ is an alias to /src
import Store from '@/types/Store'

export default defineComponent({
  name: 'Home',
  components: {
    CustomersView,
    AddCustomerView
  },
  setup() {
    const store = inject('store') as Store
    const loadCustomers = async () => {
      await store.methods.loadcustomers();
    };
    loadCustomers();
  }
});
</script>
