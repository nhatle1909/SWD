import { PlusCircleOutlined } from "@ant-design/icons";
import PageHeader from "../../../components/PageHeader";
import { Button, Flex } from "antd";
import CreateInteriorModel from "../../../components/interior/CreateInteriorModal";
import ListInterior from "../../../components/interior/ListInterior";
import { useState } from "react";
import EditInteriorModel from "../../../components/interior/EditInteriorModal";


const ManageInterior = () => {
    const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
    const [isEditModalOpen, setIsEditModalOpen] = useState(false);
    const [selectedInterior, setSelectedInterior] = useState(null);

    const handleEditInterior = (record) => {
      setSelectedInterior(record);
      setIsEditModalOpen(true);
    }

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
                Create New Interior
              </Button>
            </Flex>
  
            <ListInterior handleEditInterior = {handleEditInterior} />
            <CreateInteriorModel isModalOpen={isCreateModalOpen} setIsModalOpen={setIsCreateModalOpen} />
            <EditInteriorModel isModalOpen={isEditModalOpen} setIsModalOpen={setIsEditModalOpen}
                                selectedInterior={selectedInterior}/>
          </div>
        </div>
    </>);
}
export default ManageInterior;