
import { getAccounts, createAccountCustomer, createAccountStaff, removeAccount, viewAccount } from "../../api/user";
import {
  setAccounts,
} from "./slice";

export const actionGetAccounts = (request) => {
  return async (dispatch) => {
    try {
      const { data } = await getAccounts(request);

      console.log(data.message);
      dispatch(setAccounts(data.message));

    } catch (error) {
      console.log(error)
      throw error;
    }
  };
}
