
import { toast } from "react-toastify";
import { getAccounts, createAccountCustomer, createAccountStaff, removeAccount, viewAccount } from "../../api/user";
import {
  setAccounts,
} from "./slice";

export const actionGetAccounts = (request) => {
  return async (dispatch) => {
    try {
      const { data } = await getAccounts(request);

      console.log("data", data);

      dispatch(setAccounts(data));

    } catch (error) {
      toast(error.response.data, {
        type: 'error'
      });
      console.log(error)
      throw error;
    }
  };
}

export const actionRemoveAccount = (request) => {
  return async (dispatch) => {
    try {

      await removeAccount(request);

      const { data } = await getAccounts({
        PageIndex: 1,
        IsAsc: true,
        SearchValue: "",
      });

      dispatch(setAccounts(data));
      toast('Remove account successful', {
        type: 'success'
      });

    } catch (error) {
      toast(error.response.data, {
        type: 'error'
      });
      console.log(error)
      throw error;
    }
  };
}

export const actionAddAccount = (request) => {
  return async (dispatch) => {
    try {
      console.log("going to add: ", request)
      const response = await createAccountCustomer({ email: request.email, phoneNumber: request.phoneNumber, password: request.password });

      console.log("response", response);
      const { data } = await getAccounts({
        PageIndex: 1,
        IsAsc: true,
        SearchValue: "",
      });

      dispatch(setAccounts(data));
      toast('Add new account successful', {
        type: 'success'
      });
    } catch (error) {
      toast(error.response.data, {
        type: 'error'
      });
      throw error;
    }
  };
}

