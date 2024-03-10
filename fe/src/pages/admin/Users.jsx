import { PlusCircleOutlined } from "@ant-design/icons";
import PageHeader from "../../components/PageHeader";
import ListAccount from "../../components/account/ListAccount";
import { Button, Flex } from "antd";
import CreateAccountModel from "../../components/account/CreateAccountModel";

const Users = () => {
  return (
    <>
      <div className="h-[90vh] w-[100vw] flex justify-center items-center" style={{
        background: '#343f4024'
      }}>
        <div className="h-[65vh] w-[70vw]">
          <PageHeader message={"Manage User Account"} />
          <Flex
            gap="small"
            style={{
              width: '100%',
            }}
            justify="flex-end"
          >
            <Button
              type="primary"
              danger
              className="blue mb-2"
              onClick={() => { }}
              icon={<PlusCircleOutlined />}
            >
              Create New Account
            </Button>
          </Flex>

          <ListAccount />
          <CreateAccountModel />
        </div>
      </div >
    </>
  )
}

export default Users;