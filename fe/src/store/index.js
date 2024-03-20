import { configureStore } from "@reduxjs/toolkit";
import { useDispatch, useSelector } from "react-redux";

import authenticationSlice from "./auth/slice"; 
import userSlice from "./user/slice";
import requestSlice from "./request/slice";
import interiorSlice from "./interior/slice";
import interiorsSlice from "./interior/slice";

import blogsSlice  from "./blog/slice";
import transactionSlice from "./transaction/slice";


export const store = configureStore({
  reducer: {
    authentication: authenticationSlice,
    user: userSlice,
    request: requestSlice,
    interior: interiorSlice,
    blogs: blogsSlice,
    interiors: interiorsSlice,
    transaction: transactionSlice
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware({
      serializableCheck: false,
    }),
});

export const useAppDispatch = () => useDispatch();
export const useAppSelector = useSelector;
