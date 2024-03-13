import './Auth.scss'
import { useState, useRef } from 'react';
import { LockOutlined, UserOutlined, PhoneOutlined } from '@ant-design/icons';
import { Button, Checkbox, Form, Input, InputNumber } from 'antd';
import { useAppDispatch } from '../../store';
import Cookies from 'js-cookie';
import { actionSignUpUser, actionLogin, actionSendMailToResetPassword, actionChangePassword, actionResetPassword } from '../../store/auth/action'
const actionSubmit = new Map([
  ['login', 'Log in'], ['register', 'Sign Up'], ['forgot', 'Send'], ['changePass', 'Submit'], ['resetPass', 'Submit']
])
const Auth = ({type, setType, setOpenAuth, setChangePass}) => {
    const dispatch = useAppDispatch();
    const cookies = JSON.parse(Cookies.get('login') ? Cookies.get('login') : false);
    const [loading, setLoading] = useState(false);
    const remember = JSON.parse(Cookies.get('remember')? Cookies.get('remember') : false);
    const [email, setEmail] = useState(remember ? cookies.email:'');
    const [password, setPassword] = useState(remember?cookies.password:'')

    const emailEle = useRef(null);
    const passEle = useRef(null);

    const onFinish = async (values) => {
      setLoading(true);
        const action = type === 'login' 
        ? dispatch(actionLogin({email:values.email, password:values.password}))
          : type === 'register'
          ?  dispatch(actionSignUpUser({email:values.email, password:values.password, phoneNumber: values.phoneNumber}))
        : type === 'forgot' 
          ? dispatch(actionSendMailToResetPassword(values.email))
        : type === 'resetPass'
          ? dispatch(actionResetPassword(window.location.search.slice(16), values.newPass))
          : dispatch(actionChangePassword(values.oldPass, values.newPass))

        try {
          const payload = await action
          if(payload === 'reset-password') {
            setTimeout(() => {
              window.location.href = '/'
            }, 1000)
          }
          else{
            setChangePass(false)
            setType('login')
            if(values.remember){
              Cookies.set('login', JSON.stringify({
                email: values.email,
                password: values.password,
                
              }), { expires: 30 }); // Expires in 30 days
              Cookies.set('remember', JSON.stringify(true),{
                expires:30})
            }else{
              Cookies.set('login', JSON.stringify({
                email: '',
                password: '',
              
              }));
              Cookies.set('remember', JSON.stringify(false),{
                expires:30})
            }
            setLoading(false);
            setOpenAuth(pre => !pre)
         
          }
        
          
        } catch (error) {
          setLoading(false);
          console.log('check error::', error)
        }
        
      };
    return (
        <div className="login-container">
             <Form
      name="normal_login"
      className="login-form"
      //check for undefined
      initialValues={{ remember: remember ? true : false, email: remember ? email: '', password:remember? password: '' }}
      onFinish={onFinish}
    >
      {type === 'changePass' || type === 'resetPass' ? (
        <>
        {type === 'changePass' ? (
            <Form.Item
         
            name="oldPass"
            rules={[{ required: true, message: 'Please input your old password!' }]}
            
          >
            <Input.Password
              prefix={<LockOutlined className="site-form-item-icon" />}
              placeholder="Old Password"
              type="password"
            />
          </Form.Item>
        ): (<></>)}
         
          <Form.Item
         
         name="newPass"
    
         rules={[{ required: true, message: 'Please input your new password!' }]}
         
       >
         <Input.Password

           
   
           prefix={<LockOutlined className="site-form-item-icon" />}
           type="password"
           placeholder="New Password"
         
         />
       </Form.Item>
       <Form.Item
          name="confirmNewPassword"
          dependencies={['newPass']}
          rules={[{ required: true, message: 'Please input your ConfirmPassword!' }, 
          ({ getFieldValue }) => ({
            validator(_, value) {
              if (!value || getFieldValue('newPass') === value) {
                return Promise.resolve();
              }
              return Promise.reject(new Error('The password not matched!'));
            },
          })
        ]}
        >
          <Input.Password
            prefix={<LockOutlined className="site-form-item-icon" />}
            type="password"
            placeholder="Confirm Password"
            
          />
        </Form.Item>
        </>
      ): (
        <>
         <Form.Item
        name="email"
      
        rules={[{ required: true, message: 'Please input your Email!' },{
          type: "email",
          message: "The input is not valid E-mail!",
        }]}
        
      >
        <Input   ref={emailEle} value={email} onChange={(e) => setEmail(e.target.value)}  prefix={<UserOutlined className="site-form-item-icon" />} placeholder="Email" />
      </Form.Item>
        {type === 'register' ? (
          <>
            <Form.Item
           
        name="phoneNumber"
      >
        <Input  style={{
              width:'100%'
            }}   prefix={<PhoneOutlined className="site-form-item-icon" />} placeholder="Phone number" />
      </Form.Item>
          </>
        ): (
          <></>
        )}
    

        {type === 'forgot' ? (
          <></>
        ): (
          <>
          <Form.Item
          
            name="password"
            initialValue={password}
            rules={[{ required: true, message: 'Please input your Password!' }]}
            
          >
            <Input.Password
            ref={passEle}
              
              onChange={(e) => setPassword(e.target.value)}
              prefix={<LockOutlined className="site-form-item-icon" />}
              type="password"
              placeholder="Password"
              value={password}
            />
          </Form.Item>
          {type === 'register' ? (
            <>
            <Form.Item
            name="confirmPassword"
            dependencies={['password']}
            rules={[{ required: true, message: 'Please input your ConfirmPassword!' }, 
            ({ getFieldValue }) => ({
              validator(_, value) {
                if (!value || getFieldValue('password') === value) {
                  return Promise.resolve();
                }
                return Promise.reject(new Error('The password not matched!'));
              },
            })
          ]}
          >
            <Input.Password
              prefix={<LockOutlined className="site-form-item-icon" />}
              type="password"
              placeholder="Confirm Password"
              
            />
          </Form.Item>
            </>
          ): (
            <></>
          )}
            {type === 'login' ? (<>
          <Form.Item>
            <Form.Item name="remember" valuePropName="checked" noStyle>
              <Checkbox>Remember me</Checkbox>
            </Form.Item>
            <a className="login-form-forgot" onClick={()=> setType('forgot')}>
              Forgot password
            </a>
          </Form.Item>
            </>): (
              <></>
            )}
          
          </>
        )}
        </>
      )}
     

      <Form.Item>
        <Button loading={loading} type="primary" htmlType="submit" className="login-form-button">
          {actionSubmit.get(type)}
        </Button>
        {type === 'changePass' ? (
          <>
          </>
        ): (
          <>
          {type === 'login' ? (
          <>
          &nbsp; Or &nbsp; <a onClick={() => {
            setType('register')
            setEmail('')
            setPassword('')
            emailEle.current.input.value = ''
            passEle.current.input.value = ''
          }}>register now!</a>
          </>
        ):  (
          <>
          &nbsp; Or  &nbsp; <a onClick={() => setType('login')}>Login</a>
          </>
        )}
          </>
        )}
        
        
      </Form.Item>
    </Form>
            
        </div>
    )
}

export default Auth;