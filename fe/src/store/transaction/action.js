
import { toast } from "react-toastify";
import { getTransactions } from "../../api/transaction";

import {
  setTransactions,
} from "./slice";

export const actionGetMyTransactions = (request) => {
  return async (dispatch) => {
    try {
      const { data } = await getTransactions(request);

      console.log("data", data);

      dispatch(setTransactions(data));

    } catch (error) {
      toast(error.response.data, {
        type: 'error'
      });
      console.log(error)
      throw error;
    }
  };
}
