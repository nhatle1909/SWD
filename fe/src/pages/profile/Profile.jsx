import { useAppSelector } from '@/store';
const Profile = () => {
  const auth = useAppSelector(({ authentication }) => authentication.authUser)
  return (<>
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
            <div className="w-full p-8 mx-2 flex justify-center">
              <img id="showImage" className="max-w-xs w-32 items-center border" src="https://images.unsplash.com/photo-1477118476589-bff2c5c4cfbb?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=200&q=200" alt="" />
            </div>
          </div>

          <div className="w-full md:w-3/5 p-8 bg-white lg:ml-4 shadow-md">
            <div className="rounded  shadow p-6">
              <div className="pb-4">
                <label for="about" className="font-semibold text-gray-700 block pb-1">Email</label>
                <input disabled id="email" className="border-1  rounded-r px-4 py-2 w-full" type="email" value={auth.email} />
                {/* <span className="text-gray-600 pt-4 block opacity-70">Personal login information of your account</span> */}
              </div>
              <div className="pb-6">
                <label for="name" className="font-semibold text-gray-700 block pb-1">Name</label>
                <div className="flex">
                  <input id="username" className="border-1  rounded-r px-4 py-2 w-full" type="text" value="Jane Name" />
                </div>
              </div>
              <div className="pb-6">
                <label for="phone" className="font-semibold text-gray-700 block pb-1">Phone Number</label>
                <div className="flex">
                  <input id="phone" className="border-1  rounded-r px-4 py-2 w-full" type="text" value="098.789.211" />
                </div>
              </div>
              <div className="pb-6">
                <label for="address" className="font-semibold text-gray-700 block pb-1">Address</label>
                <div className="flex">
                  <input id="address" className="border-1  rounded-r px-4 py-2 w-full" type="text" value="143.dt xxx" />
                </div>
              </div>

            </div>
          </div>

        </div>

      </div>

    </div>
  </>)
}

export default Profile;