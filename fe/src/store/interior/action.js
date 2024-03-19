import { toast } from "react-toastify";
import { createInterior, getInteriors, removeInterior, updateInterior } from "../../api/interior";
import {
  setInteriors,
} from "./slice";

import { getInteriorList } from '@/api/interior';
import { setDetailInterior } from './slice';
import { getDetailInterior } from '../../api/interior';


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

export const actionGetInteriosList = ({pageIndex, isAsc, searchValue}) => {
    return async (dispatch) => {
        try {
            const {data} = await getInteriorList({ pageIndex, isAsc, searchValue });
            dispatch(setInteriors(data));
        } catch (error) {
            toast('Something went wrong, pls contact admin!', {
                type: 'error'
            })
            console.error('Error fetching blogs:', error);
        }
    };
};

export const actionGetDetailInterior = (interiorId) => {
  return async (dispatch) => {
      try {
          const {data} = await getDetailInterior(interiorId);
          dispatch(setDetailInterior(data));
      } catch (error) {
          toast('Something went wrong, pls contact admin!', {
              type: 'error'
          })
          console.error('Error fetching blogs:', error);
      }
  };
};

export const actionRemoveInterior = (request) => {
  return async (dispatch) => {
    try {

      await removeInterior(request);

      const { data } = await getInteriors({
        PageIndex: 1,
        IsAsc: true,
        SearchValue: "",
      });

      dispatch(setInteriors(data));
      toast('Remove interior successful', {
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
export const actionAddInterior = (request) => {
  return async (dispatch) => {
    try {
      console.log("going to add: ", request)
      const response = await createInterior(request, request.image);
      console.log("response", response);
      const { data } = await getInteriors({
        PageIndex: 1,
        IsAsc: true,
        SearchValue: "",
      });

      dispatch(setInteriors(data));
      toast('Add new interior successful', {
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
export const actionUpdateInterior = (request) => {
  return async (dispatch) => {
    try {
      console.log("going to update: ", request)
      const response = await updateInterior(request);
      console.log("response", response);
      const { data } = await getInteriors({
        PageIndex: 1,
        IsAsc: true,
        SearchValue: "",
      });

      dispatch(setInteriors(data));
      toast('Update interior successful', {
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