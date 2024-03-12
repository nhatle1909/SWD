
import { toast } from "react-toastify";
import { getInteriors } from "../../api/interior";
import {
  setInteriors,
} from "./slice";

export const actionGetInteriors = (request) => {
  return async (dispatch) => {
    try {
      const { data } = await getInteriors(request);

      console.log("data", data);

      dispatch(setInteriors(data));

    } catch (error) {
      toast(error.response.data, {
        type: 'error'
      });
      console.log(error)
      throw error;
    }
  };
}



// export const actionAddAccount = (request) => {
//   return async (dispatch) => {
//     try {
//       console.log("going to add: ", request)
//       const response = await createAccountCustomer({ email: request.email, phoneNumber: request.phoneNumber, password: request.password });

//       console.log("response", response);
//       const { data } = await getAccounts({
//         PageIndex: 1,
//         IsAsc: true,
//         SearchValue: "",
//       });

//       dispatch(setAccounts(data));
//       toast('Add new account successful', {
//         type: 'success'
//       });
//     } catch (error) {
//       toast(error.response.data, {
//         type: 'error'
//       });
//       throw error;
//     }
//   };
// }

