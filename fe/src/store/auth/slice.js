import { createSlice } from "@reduxjs/toolkit";

const initialState = {
  authUser: null

};

export const authenticationSlice = createSlice({
  name: "authentication",
  initialState,
  reducers: {
    setAuthUser(state, action) {
      state.authUser = action.payload;
    },
  },
});

export const { 
  setAuthUser,
} = authenticationSlice.actions;

export default authenticationSlice.reducer;