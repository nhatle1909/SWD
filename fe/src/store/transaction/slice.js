import { createSlice } from "@reduxjs/toolkit";

const initialState = {
  transactions: [],
  transaction: null,
};

export const transactionSlice = createSlice({
  name: "transaction",
  initialState,
  reducers: {
    setTransactions(state, action) {
      state.transactions = action.payload;
    },
    setTransaction(state, action){
      state.transaction = action.payload;
    }
  },
});

export const {
  setTransactions,
  setTransaction
} = transactionSlice.actions;

export default transactionSlice.reducer;