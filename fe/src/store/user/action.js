
import { toast } from "react-toastify";
import { getAccounts, createAccountCustomer, createAccountStaff, removeAccount, viewAccount,changeAvatarProfile, uploadInfoProfile } from "../../api/user";
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
      const response = await createAccountCustomer(request.accountType, { email: request.email, phoneNumber: request.phoneNumber, password: request.password });
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


// profile
export const actionChangeAvatarProfile = (file) => {
  return async (dispatch) => {
    try {
      const { data } = await changeAvatarProfile(file);
      toast('The avatar updated!', {
        type:'success'
      })
      return data
    } catch (error) {
      toast('Something went wrong, pls contact admin!', {
        type:'error'
      })
      console.log(error)
      throw error;
    }
  };
}
export const actionUpdateProfile = (phoneNumber, homeAddress) => {
  return async (dispatch) => {
    try {
      const { data } = await uploadInfoProfile(phoneNumber, homeAddress);
      toast('The profile updated!', {type: 'success'})
      return data
    } catch (error) {
      toast('The phone number is invalid!', {type: 'error'})
    }
  }
}