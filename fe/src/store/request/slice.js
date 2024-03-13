import { createSlice } from "@reduxjs/toolkit";

const initialState = {
  requests: [],
};

export const requestSlice = createSlice({
  name: "request",
  initialState,
  reducers: {
    setRequests(state, action) {
      state.requests = action.payload;
    },
  },
});

export const {
  setRequests
} = requestSlice.actions;

export default requestSlice.reducer;