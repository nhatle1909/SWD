import { useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { Dropdown, Space } from 'antd';
import { CaretDownOutlined, CaretUpOutlined } from '@ant-design/icons'
import { useAppSelector, useAppDispatch } from '@/store';
import Login from '@/pages/auth/Auth';
import { getLocalStorage, setLocalStorage } from '@/utils/common';
import { setAuthUser } from '../../../store/auth/slice';
import { Menu } from 'antd';
function getItem(label, key, icon, children, type) {
	return {
		key,
		icon,
		children,
		label,
		type,
	};
}

const Header = () => {
	const navigator = useNavigate();
	const dispatch = useAppDispatch();
	const auth = useAppSelector(({ authentication }) => {
		const authLocal = getLocalStorage('auth');
		const authStore = authentication?.authUser
		if (authStore?.token) {
			return authStore
		}
		if (authLocal?.token) {
			dispatch(setAuthUser(authLocal))
			return authLocal
		}

		return null;
	})

	const location = useLocation();
	// auth
	const [typeAuth, setTypeAuth] = useState('login')
	const [openAuth, setOpenAuth] = useState(false);
	const handleLogout = () => {
		setLocalStorage('auth', {});
		window.location.reload();
	}
	const items = [
		{
			label: <Login type={typeAuth} setType={setTypeAuth} setOpenAuth={setOpenAuth} />,
		}
	];

	const itemsDashboard = [
		getItem('Dashboard', null, null,
			auth?.role === 'Admin' ?
				[
					//add for admin routes
					getItem(<a onClick={() => {
						navigator('/admin/users')
					}}>Users</a>, null, null),

				] :
				auth?.role === 'Staff' ?
					[
						//add for staff routes
						getItem(<a onClick={() => {
							navigator('/staff/blogs')
						}}>Blogs</a>, null, null),
					]
					:
					[]

		)

	]

	const itemsProfile = [
		{
			label: <a onClick={() => navigator('/profile')}>Profile</a>
		},
		auth?.role === 'Admin' || auth?.role === 'Staff' ? {
			label: <Menu style={{ width: 256 }} mode="vertical" items={itemsDashboard} />
		} : null,
		{
			label: <a>Change Password</a>
		},
		{
			label: <a onClick={() => handleLogout()}>Logout</a>
		}
	]

	return (
		<nav className={`navbar navbar-expand-lg navbar-dark ftco_navbar bg-dark ftco-navbar-light`} id="ftco-navbar">
			<div className="container">
				<a className="navbar-brand" href="/">SWD</a>
				<button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#ftco-nav" aria-controls="ftco-nav" aria-expanded="false" aria-label="Toggle navigation">
					<span className="oi oi-menu"></span> Menu
				</button>

				<div className="collapse navbar-collapse" style={{
					visibility: 'visible'
				}} id="ftco-nav">
					<ul className="navbar-nav ml-auto">
						<li className={`nav-item ${location.pathname === '/' ? 'active' : ''}`}><a href='/' className="nav-link">Home</a></li>
						<li className={`nav-item ${location.pathname === '/about' ? 'active' : ''} `}><a onClick={() => navigator('/about')} className="nav-link">About</a></li>


						<Dropdown
							menu={{
								items: auth?.token ? itemsProfile : items
							}}
							open={openAuth}
							placement='topRight'
						>
							<a onClick={(e) => e.preventDefault()}>
								{auth?.token ? (<>
									<Space>
										<li onClick={() => {
											setOpenAuth(pre => !pre)
										}} className={`nav-item hover:cursor-pointer flex`}><div className="nav-link !underline">
												{auth.email}
											</div>
										</li>
									</Space>
								</>) : (
									<>
										<Space>
											<li onClick={() => {
												setOpenAuth(pre => !pre)
												setTypeAuth('login')
											}} className={`nav-item hover:cursor-pointer flex`}><div className="nav-link !underline">
													Login
												</div>{openAuth ? (<CaretDownOutlined />) : (<CaretUpOutlined />)}</li>
										</Space>
									</>
								)}

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