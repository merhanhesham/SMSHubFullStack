import React from 'react'
import { Outlet } from 'react-router-dom'
import Footer from '../Common/Footer'

import Header from '../Common/Header'

export default function Layout() {
  return <>
    <Header />
    <div className="container">
      <Outlet></Outlet>
    </div>
    <Footer />
  </>
}
