

import { 
  setAuthUser,
} from "./slice";
import { 
  login, 
} from "@/api/auth";
export const actionLogin = (
  {email, password}
) => {
  return async (dispatch) => {
    try {
      console.log(
        'check request::', email
      )
      const { data } = await login(email, password);
      console.log('data::', data)
      dispatch(setAuthUser(data));
      localStorage.setItem('auth', JSON.stringify(data));
    } catch (error) {
      console.log(error)
      throw error;
    }
  };
};

