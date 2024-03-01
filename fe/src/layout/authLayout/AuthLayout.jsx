import Header from './header/Header'
import Footer from './footer/Footer'
import { Outlet } from 'react-router-dom'
const AuthLayout = () => {
    return (
        <>
        <Header/>
        <br/>
            <Outlet/> 
            <br/>
        <Footer/>
        </>
    )
}

export default AuthLayout;