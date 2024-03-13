import { useEffect, useState } from 'react';
import { useNavigate, useLocation, useParams } from 'react-router-dom';
import { Dropdown, Space, Menu, Avatar } from 'antd';
import { CaretDownOutlined, CaretUpOutlined } from '@ant-design/icons'
import { useAppSelector, useAppDispatch } from '@/store';
import Login from '@/pages/auth/Auth';
import { getLocalStorage, setLocalStorage } from '@/utils/common';
import { setAuthUser, setPicture } from '../../../store/auth/slice';
import { convertBase64Img } from '../../../utils/common';
import { actionGetUserInfo } from '../../../store/auth/action';

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

	const picture = useAppSelector(({authentication}) => {
		return authentication.picture
	}) 

	useEffect(() => {
		dispatch(actionGetUserInfo()).then((data) => {
			dispatch(setPicture(data?.picture))
		})
	}, [])


	const location = useLocation();
	// auth
	const [typeAuth, setTypeAuth] = useState('login')
	const [openAuth, setOpenAuth] = useState(false);
	const [changePass, setChangePass] = useState(false);
	const handleLogout = () => {
		setLocalStorage('auth', {});
		window.location.reload();
	}

	const handleRequest = () => {
		window.location.href = "/request";
	}
	useEffect(() => {
		console.log('check reset pass::', window.location.search.slice(16))
		if (openAuth) {
			setOpenAuth(false)
		}
		if (window.location.search.slice(0, 16)) {
			setOpenAuth(true);
			setTypeAuth('resetPass')
		}
	}, [location.pathname])

	const items = [
		{
			label: <Login setChangePass={setChangePass} type={typeAuth} setType={setTypeAuth} setOpenAuth={setOpenAuth} />,
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
					getItem(<a onClick={() => {
						navigator('/admin/reports')
					}}>Reports</a>, null, null),
				] :
				auth?.role === 'Staff' ?
					[
						//add for staff routes
						getItem(<a onClick={() => {
							navigator('/staff/blogs')
						}}>Blogs</a>, null, null),
						getItem(<a onClick={() => {
							navigator('/staff/requests')
						}}>Requests</a>, null, null),
					]
					:
					[]

		)

	]

	const itemsRequest = [
		getItem('Request', null, null,
			[
				getItem(<a onClick={() => {
					navigator('/request/history')
				}}>History</a>, null, null),
				getItem(<a onClick={() => {
					navigator('/request/create')
				}}>Create New</a>, null, null),
			]
		)
	];

	const itemsProfile = [
		{
			label: <a onClick={() => navigator('/profile')}>Profile</a>
		},
		auth?.role === 'Admin' || auth?.role === 'Staff' ? {
			label: <Menu style={{ width: 256 }} mode="vertical" items={itemsDashboard} />
		} : null,
		{
			label: <a onClick={() => {
				setOpenAuth(false);
				setTimeout(() => {
					setChangePass(true)
					setOpenAuth(true);
					setTypeAuth('changePass')
				}, 300)
			}}>Change Password</a>
		},
		auth?.role !== 'Admin' && auth?.role !== 'Staff' ? {
			label: <Menu style={{ width: 256 }} mode="vertical" items={itemsRequest} />
		} : null,
		{
			label: <a onClick={() => handleLogout()}>Logout</a>
		}
	]

	return (
		<nav className={`navbar navbar-expand-lg navbar-dark ftco_navbar bg-dark ftco-navbar-light`} id="ftco-navbar">
			<div className="container">
				<a className="navbar-brand" href="/">Interior Construction Quotation</a>
				<button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#ftco-nav" aria-controls="ftco-nav" aria-expanded="false" aria-label="Toggle navigation">
					<span className="oi oi-menu"></span> Menu
				</button>

				<div className="collapse navbar-collapse" style={{
					visibility: 'visible'
				}} id="ftco-nav">
					<ul className="navbar-nav ml-auto">
						<li className={`nav-item ${location.pathname === '/' ? 'active' : ''}`}><a href='/' className="nav-link">Home</a></li>
						<li className={`nav-item ${location.pathname === '/about' ? 'active' : ''} `}><a href="/about" className="nav-link">About</a></li>
						<li className={`nav-item ${location.pathname === '/blog' ? 'active' : ''} `}><a href="/blog" className="nav-link">Blog</a></li>

						<Dropdown
							menu={{
								items: auth?.token
									? changePass
										? items
										: itemsProfile
									: items
							}}
							open={openAuth}
							placement='topRight'
						>
							<a onClick={(e) => e.preventDefault()}>
								{auth?.token ? (<>
									<Space>
										<li onClick={() => {
											setOpenAuth(pre => !pre)
											setChangePass(false)
											setTypeAuth('login')
										}}  className={`nav-item hover:cursor-pointer flex`}><div className="nav-link !underline">
												{auth.email}
											</div>
										</li>
										<li>
										<Avatar onClick={() => {
											setOpenAuth(pre => !pre)
											setChangePass(false)
											setTypeAuth('login')
										}} size={36}>
											<img src={convertBase64Img(picture)} />
										</Avatar>
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