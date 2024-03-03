
import { sendMailResetPassword, signUpUser } from "../../api/auth";
import { 
  setAuthUser,
} from "./slice";
import { 
  login, 
} from "@/api/auth";
import { setLocalStorage } from "../../utils/common";
export const actionLogin = (
  {email, password}
) => {
  return async (dispatch) => {
    try {
      const { data } = await login(email, password);  
      dispatch(setAuthUser({
        token: data,
        email: email
      }));
      setLocalStorage('auth', {token: data, email: email});

    } catch (error) {
      console.log(error)
      throw error;
    }
  };
};

export const actionSignUpUser = ({email, password}) => {
  return async (dispatch) => {
    try {
      await signUpUser(email, password);
      const { data } = await login(email, password);
      dispatch(setAuthUser({token:data, email: email}));
      setLocalStorage('auth', {token: data, email: email});

    } catch (error) {
      console.log(error)
      throw error;
    }
  };
}

export const actionSendMailToResetPassword = (email) => {
  return async (dispatch) => {
    try {
      await sendMailResetPassword(email);
      
    } catch (error) {
      console.log(error)
      throw error;
    }
  };
}
