import './Auth.scss'
import { LockOutlined, UserOutlined } from '@ant-design/icons';
import { Button, Checkbox, Form, Input } from 'antd';
import { useAppDispatch } from '../../store';
import { actionSignUpUser, actionLogin, actionSendMailToResetPassword } from '../../store/auth/action'
const actionSubmit = new Map([
  ['login', 'Log in'], ['register', 'Sign Up'], ['forgot', 'Send']
])
const Auth = ({type, setType, setOpenAuth}) => {
    const dispatch = useAppDispatch();
 
    const onFinish = async (values) => {
        const action = type === 'login' 
        ? dispatch(actionLogin({email:values.email, password:values.password}))
          : type === 'register' 
         
          ?  dispatch(actionSignUpUser({email:values.email, password:values.password}))
        : dispatch(actionSendMailToResetPassword(values.email))

        try {
          const payload = await action
          setOpenAuth(pre => !pre)
          console.log('chek data response action:::', payload)
        } catch (error) {
          console.log('check error::', error)
        }
        
      };
    return (
        <div className="login-container">
             <Form
      name="normal_login"
      className="login-form"
      initialValues={{ remember: true }}
      onFinish={onFinish}
    >
      <Form.Item
        name="email"
        rules={[{ required: true, message: 'Please input your Email!' }]}
      >
        <Input prefix={<UserOutlined className="site-form-item-icon" />} placeholder="Email" />
      </Form.Item>

      {type === 'forgot' ? (
        <></>
      ): (
        <>
        <Form.Item
          name="password"
          rules={[{ required: true, message: 'Please input your Password!' }]}
        >
          <Input
            prefix={<LockOutlined className="site-form-item-icon" />}
            type="password"
            placeholder="Password"
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
              return Promise.reject(new Error('The new password that you entered do not match!'));
            },
          })
        ]}
        >
          <Input
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

      <Form.Item>
        <Button type="primary" htmlType="submit" className="login-form-button">
          {actionSubmit.get(type)}
        </Button>
        {type === 'login' ? (
          <>
          &nbsp; Or &nbsp; <a onClick={() => setType('register')}>register now!</a>
          </>
        ):  (
          <>
          &nbsp; Or  &nbsp; <a onClick={() => setType('login')}>Login</a>
          </>
        )}
        
      </Form.Item>
    </Form>
            
        </div>
    )
}

export default Auth;