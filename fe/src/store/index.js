import { configureStore } from "@reduxjs/toolkit";
import { useDispatch, useSelector } from "react-redux";

import authenticationSlice from "./auth/slice";
import userSlice from "./user/slice";
import contractSlice from "./contract/slice";

export const store = configureStore({
  reducer: {
    authentication: authenticationSlice,
    user: userSlice,
    contract: contractSlice,
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware({
      serializableCheck: false,
    }),
});

export const useAppDispatch = () => useDispatch();
export const useAppSelector = useSelector;
