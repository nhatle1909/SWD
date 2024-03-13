import {toast} from 'react-toastify';
import { getAccounts, createAccountCustomer, createAccountStaff, removeAccount, viewAccount, changeAvatarProfile, uploadInfoProfile } from "../../api/user";
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
      console.log(error)
      throw error;
    }
  };
}


