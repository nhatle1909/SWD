import { useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { Dropdown, Space } from 'antd';
import {CaretDownOutlined, CaretUpOutlined} from '@ant-design/icons'
import Login from '@/pages/auth/Auth';

const Header = () => {
		const navigator = useNavigate();
		const location = useLocation();

		// auth
		const [typeAuth, setTypeAuth] = useState('login')
		const [openAuth, setOpenAuth] = useState(false);
		const items = [
			{
				label: <Login type={typeAuth} setType={setTypeAuth}/>,
			}
		];
		console.log(location.pathname)
    return (
        <nav className="navbar navbar-expand-lg navbar-dark ftco_navbar bg-dark ftco-navbar-light" id="ftco-navbar">
	    <div className="container">
	      <a className="navbar-brand" href="index.html">Klift</a>
	      <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#ftco-nav" aria-controls="ftco-nav" aria-expanded="false" aria-label="Toggle navigation">
	        <span className="oi oi-menu"></span> Menu
	      </button>

	      <div className="collapse navbar-collapse" id="ftco-nav">
	        <ul className="navbar-nav ml-auto">
	        	<li className={`nav-item ${location.pathname === '/' ? 'active' : ''}`}><a href='/' className="nav-link">Home</a></li>
	        	<li className={`nav-item ${location.pathname === '/about' ? 'active' : ''} `}><a onClick={() => navigator('/about')} className="nav-link">About</a></li>

				
						<Dropdown
    menu={{
      items,
    }}
		open={openAuth}
		placement='topRight'
  >
    <a onClick={(e) => e.preventDefault()}>
      <Space>
			<li onClick={() => setOpenAuth(pre => !pre)}  className={`nav-item hover:cursor-pointer flex`}><div className="nav-link !underline">
					Login		
      </div>{openAuth ? (<CaretDownOutlined />): (<CaretUpOutlined />)}</li>
      </Space>
    </a>
  </Dropdown>
							

	        	{/* <li className="nav-item"><a href="project.html" className="nav-link">Project</a></li>
	        	<li className="nav-item"><a href="services.html" className="nav-link">Services</a></li>
	        	<li className="nav-item"><a href="blog.html" className="nav-link">Blog</a></li>
	          <li className="nav-item"><a href="contact.html" className="nav-link">Contact</a></li> */}
	        </ul>
	      </div>
	    </div>
	  </nav>

    )
}

export default Header;