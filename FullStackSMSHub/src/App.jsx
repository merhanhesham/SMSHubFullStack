import React from 'react'; // Ensure React is imported
import './App.css';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import Layout from './Components/Layout/Layout';
import Home from './Components/Home/Home';
import SenderID from './Components/Dashboard/SenderID';
import SenderIDManagement from './Components/SenderIdManagement/SenderIDManagement';
import SendMessage from './Components/SendMessage';
import Reports from './Components/Reports';
import Login from './Components/Login/Login';
import Register from './Components/Register/Register';
import Logout from './Components/Logout';
import Notfound from './Components/NotFound/Notfound';

let routers = createBrowserRouter([
  {
    path: "", element: <Layout />, children: [
      { path: "", element: <Home /> },
      { path: "Home", element: <Home /> },
      { path: "SenderID", element: <SenderID /> },
      { path: "senderIDManagement", element: <SenderIDManagement /> },
      { path: "messages", element: <SendMessage /> },
      { path: "reports", element: <Reports /> },
      { path: "login", element: <Login /> },
      { path: "register", element: <Register /> },
      { path: "logout", element: <Logout /> },
      { path: "*", element: <Notfound /> },
    ]
  }
]);

function App() {
  return <RouterProvider router={routers}></RouterProvider>;
}

export default App;
