import { toast } from "react-toastify";
import { getInteriors, removeInterior } from "../../api/interior";
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