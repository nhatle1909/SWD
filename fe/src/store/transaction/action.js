import { toast } from "react-toastify";
import { getTransaction, getTransactions } from "../../api/transaction";

import {
  setTransactions, setTransaction
} from "./slice";

export const actionGetMyTransactions = (request) => {
  return async (dispatch) => {
    try {
      const { data } = await getTransactions(request);

      console.log("data", data);

      if (Array.isArray(data)) {
        dispatch(setTransactions(data));
      }

  
    } catch (error) {
      toast(error.response.data, {
        type: 'error'
      });
      console.log(error)
      throw error;
    }
  };
}

export const actionGetTransaction = (request) => {
  return async (dispatch) => {
    try {
      const { data } = await getTransaction(request);
      console.log("data", data);
      dispatch(setTransaction(data));

    } catch (error) {
      toast(error.response.data, {
        type: 'error'
      });
      console.log(error)
      throw error;
    }
  };
}