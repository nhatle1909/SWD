import { useAppDispatch } from '@/store';
import { useEffect, useState, useRef } from 'react';
import { actionGetUserInfo } from '../../store/auth/action';
import { actionChangeAvatarProfile, actionUpdateProfile } from '../../store/user/action';
import { convertBase64Img } from '../../utils/common';
import {Spin, Upload, Button, Input, Space} from 'antd';
import {UploadOutlined } from '@ant-design/icons'
import ImgCrop from 'antd-img-crop';
import { setPicture } from '../../store/auth/slice';
const Profile = () => {
  const uploadRef = useRef(null);
  const dispatch = useAppDispatch();
  const [profile, setProfile] = useState(null);
  const [loading, setLoading] = useState(false)
  const [phoneNumber, setPhoneNumber] = useState();
  const [address, setAddress] = useState();
  const [disabled, setDisabled] = useState(true);
  const customRequestUpload = async (option) => {
    const { file } = option;
    try {

      const linkImg = await dispatch(actionChangeAvatarProfile(file))
      await dispatch(actionGetUserInfo()).then((data) => {
        setProfile(data);
        dispatch(setPicture(data?.picture))
      })
      option.onSuccess(linkImg);
    } catch (error) {
      console.log('chekc error::', error)
    }
  };
  useEffect(() => {
    dispatch(actionGetUserInfo()).then((data) => {
      setProfile(data);
    })
  }, [])

  const handleSaveInfo = () => {
    setLoading(true)
    dispatch(actionUpdateProfile(phoneNumber, address)).then(() => {
      setLoading(false)
    }).catch(() => {
      setLoading(false)
    })
  }

  const handleResetField = () => {

  }

  return (
    <div className="h-[90vh] w-[100vw] flex justify-center items-center" style={{
      background: '#343f4024'
    }}>
      <div className="h-[50vh] w-[60vw]">

        <div className="border-b-2 block md:flex">

          <div className="w-full md:w-2/5 p-4 sm:p-6 lg:p-8 bg-white shadow-md">
            <div className="flex justify-between">
              <span className="text-xl font-semibold block">The Profile</span>
              <a href="#" className="-mt-2 text-md font-bold text-white bg-gray-700 rounded-full px-5 py-2 hover:bg-gray-800">Edit</a>
            </div>

            <span className="text-gray-600">This information is secret so be careful</span>
            <div className="w-full p-8 mx-2 flex justify-center flex-col items-center">
              <img id="showImage" className="max-w-xs w-32 items-center border rounded-full" src={convertBase64Img(profile?.picture)} alt="" />

              <Button style={{
                background:'#1677ff',
                marginTop:'50px'
              }} onClick={() => {
                document.querySelector('#upload-profile-picture').click()
               
              }} 
              type="primary" 
              icon={<UploadOutlined />}>
                Upload
              </Button>
             
              <ImgCrop rotationSlider cropShape="round" >
              <Upload
                id="upload-profile-picture"
                className='w-[0.5px] h-[0.5px] overflow-hidden'
                ref={uploadRef}
                listType="picture-card"
                customRequest={customRequestUpload}
              >
                '+ Upload'
              </Upload>
              </ImgCrop>
            </div>
          </div>

          <div className={`w-full md:w-3/5 p-8 bg-white lg:ml-4 shadow-md ${!profile ? 'flex items-center justify-center' : ''}`}>
            {profile ? (
              <div className="rounded  shadow p-6">
              <div className="pb-4">
                <label for="about" className="font-semibold text-gray-700 block pb-1">Email</label>
                <input disabled id="email" className="border-1  rounded-r px-4 py-2 w-full" type="email" defaultValue={profile.email} />
                {/* <span className="text-gray-600 pt-4 block opacity-70">Personal login information of your account</span> */}
              </div>
              <div className="pb-6">
                <label for="phone" className="font-semibold text-gray-700 block pb-1">Phone Number</label>
                <div className="flex">
                  <Input placeholder='Enter phone number'  defaultValue={profile.phoneNumber} onChange={(e) => {
                  setPhoneNumber(e.target.value)
                  setDisabled(false);
                }} value={phoneNumber}/>;
                  
                </div>
              </div>
              <div className="pb-6">
                <label for="address" className="font-semibold text-gray-700 block pb-1">Address</label>
                <div className="flex">
                <Input placeholder='Enter address' onChange={(e) => {
                  setAddress(e.target.value)
                  setDisabled(false);
                }}  defaultValue={profile.address} value={address}/>;

                </div>
              </div>
                {disabled ? (
                  <></>
                ): (
                  <div className="pb-6">
                  <Space>
    
                   <Button loading={loading} onClick={handleSaveInfo}>Save</Button>
                   <Button onClick={handleResetField}>Reset</Button>
                  </Space>
                  </div>
                )}
            

            </div>
            ): (
                <Spin tip="Loading" size="large">
              
              </Spin>
            )}
           
          </div>

        </div>

      </div>

    </div>
  )
}

export default Profile;