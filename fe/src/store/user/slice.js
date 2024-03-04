import { createSlice } from "@reduxjs/toolkit";

const initialState = {
  accounts: [],
};

export const userSlice = createSlice({
  name: "user",
  initialState,
  reducers: {
    setAccounts(state, action) {
      state.accounts = action.payload;
    },
  },
});

export const {
  setAccounts
} = userSlice.actions;

export default userSlice.reducer;