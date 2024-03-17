import { PlusCircleOutlined } from "@ant-design/icons";
import PageHeader from "../../../components/PageHeader";
import { Button, Flex } from "antd";
import CreateInteriorModel from "../../../components/interior/CreateInteriorModal";
import ListInterior from "../../../components/interior/ListInterior";
import { useState } from "react";


const ManageInterior = () => {
    const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
    
    return (<>
        <div className="w-[100vw] flex justify-center items-center" style={{
          background: 'grey', paddingTop: '120px'
        }}>
          <div className="w-[90vw]">
            <PageHeader message={"Manage Interior"} />
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
                onClick={() => { setIsCreateModalOpen(true); }}
                icon={<PlusCircleOutlined />}
              >
                Create New Account
              </Button>
            </Flex>
  
            <ListInterior />
            <CreateInteriorModel isModalOpen={isCreateModalOpen} setIsModalOpen={setIsCreateModalOpen} />
            
          </div>
        </div>
    </>);
}
export default ManageInterior;