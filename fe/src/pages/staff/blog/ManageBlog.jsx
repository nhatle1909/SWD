import { PlusCircleOutlined } from "@ant-design/icons";
import { Button, Flex } from "antd";
import { useState } from "react";
import PageHeader from "../../../components/PageHeader";
import ListBlog from "../../../components/blog/ListBlog";

export const ManageBlog = () => {
  const [isModalOpen, setIsModalOpen] = useState(false);
  return (
    <>
      <div className="h-[90vh] w-[100vw] flex justify-center items-center"
          style={{ background: '#343f4024' }}>
        <div className="h-[65vh] w-[70vw]">
          <PageHeader message={"Manage Blog"} />
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
              onClick={() => { setIsModalOpen(true); }}
              icon={<PlusCircleOutlined />}
            >
              Create New Blog
            </Button>
          </Flex>
          <ListBlog />
            {/*
          <CreateAccountModel isModalOpen={isModalOpen} setIsModalOpen={setIsModalOpen} />
            */}
        </div>
      </div >
    </>
  )
}

export default ManageBlog;