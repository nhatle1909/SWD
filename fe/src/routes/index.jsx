import React from 'react'
import { Route, Routes as ReactRoutes } from "react-router-dom";
import MainLayout from '../layout/authLayout/MainLayout';
import Login from '../pages/auth/Auth';
import NotFound from '../pages/common/NotFound';

import UnauthRoute from './guard/UnauthRoute';
import AdminRoute from './guard/AdminRoute';
import PermissionRoute from './guard/PermissionRoute';
import Home from '@/pages/home/Home';
import About from '@/pages/about/About';
import Blog from '@/pages/blog/Blog';
import Users from '@/pages/admin/Users'
import Profile from '../pages/profile/Profile';
import StaffRoute from './guard/StaffRoute';
import ManageBlog from '../pages/staff/blog/ManageBlog';
import Reports from '../pages/admin/Reports';
import ManageRequest from '../pages/staff/request/ManageRequest';
import CreateRequest from '../pages/request/CreateRequest';
import HistoryRequest from '../pages/request/HistoryRequest';

import InteriorDetails from '../pages/interior/InteriorDetails'

const unauthRoutes = {
  path: '/',
  element: <MainLayout />,
  guard: <UnauthRoute />,
  children: [
    {
      path: '',
      element: <Home />
    },
    {
      path: 'about',
      element: <About />,
    },
    {
      path: 'blog',
      element: <Blog />,
    },
    {
      path: 'profile',
      element: <Profile />,
      permissions: ['user']
    },
    {
      path: 'request/create',
      element: <CreateRequest />,
      permissions: ['user']
    },
    {
      path: 'request/history',
      element: <HistoryRequest />,
      permissions: ['user']
    },
      path: 'interior/:interiorId',
      element:<InteriorDetails/>
    }
  ]
};

const adminRoutes = {
  path: 'admin',
  guard: <AdminRoute />,
  element: <MainLayout />,
  children: [
    {
      path: 'users',
      element: <Users />,
    },
    {
      path: 'reports',
      element: <Reports />,
    },
  ],
};

const staffRoutes = {
  path: 'staff',
  guard: <StaffRoute />,
  element: <MainLayout />,
  children: [
    {
      path: 'blogs',
      element: <ManageBlog />,
    },
    {
      path: 'requests',
      element: <ManageRequest />,
    },
  ],
};

const notfoundRoute = {
  path: "*",
  element: <NotFound />,
};

const routes = [unauthRoutes, adminRoutes, staffRoutes, notfoundRoute];

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