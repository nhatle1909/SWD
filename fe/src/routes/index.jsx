import React from 'react'
import { Route, Routes as ReactRoutes } from "react-router-dom";
import MainLayout from '../layout/authLayout/MainLayout';
import Login from '../pages/auth/Login';
import NotFound from '../pages/common/NotFound';

import UnauthRoute from './guard/UnauthRoute';
import PermissionRoute from './guard/PermissionRoute';
import Home from '../pages/home/Home';
const unauthRoutes = {
  path: '/',
  element: <MainLayout />,
  guard: <UnauthRoute />,
  children :[
    {
      path:'',
      element:<Home/>
    },
    {
      path:'auth/login',
      element: <Login/>
    }
  ]
};

// const adminRoutes = {
//   path: 'admin',
//   guard: <AdminRoute />,
//   element: <Layout page="admin"/>,
//   children: [
//     {
//       path: 'categories',
//       element: <ManageCategory />,
//     },
//     {
//       path: 'users',
//       element: <ManageUser />
//     },
//   ],
// };


const notfoundRoute= {
  path: "*",
  element: <NotFound />,
};

const routes = [unauthRoutes, notfoundRoute];

const Routes = () => {
  return (
    <ReactRoutes>
      {routes.map((route) => (
        <Route key={route.path} element={route.guard}>
          <Route element={<PermissionRoute permissions={route?.permissions} />}>
            <Route path={route.path} element={route?.element}>
              {route.children
                ? route.children.map(({ element, path, permissions }) => (
                    <Route key={path} element={route?.guard}>
                      <Route
                        element={<PermissionRoute permissions={permissions} />}
                      >
                        <Route path={path} element={element} />
                      </Route>
                    </Route>
                  ))
                : null}
            </Route>
          </Route>
        </Route>
      ))}
    </ReactRoutes>
  );
};

export default Routes;