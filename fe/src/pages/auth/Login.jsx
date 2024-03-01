import './Auth.scss'

import { useAppDispatch } from '../../store';
import { actionLogin } from '@/store/authentication/action';
import { getInfo } from '@/api/user'
const Login = () => {
    const dispatch = useAppDispatch();

    const handleLogin =async () => {
        try {
           await dispatch(actionLogin({
                email:'a',
                password:'a'
            }))
        } catch (error) {
            console.log('error::', error)
        }
    }
    const handleTestingUserInfo =async () => {
        try {
           const {data} = await getInfo();

           console.log('info::', data)
        } catch (error) {
            console.log('error::', error)
        }
    }
    return (
        <div className="login-container">
            <button onClick={handleLogin}>
                Login
            </button>
            <br/>

            <button onClick={handleTestingUserInfo}>
                get info user
            </button>
            
        </div>
    )
}

export default Login;