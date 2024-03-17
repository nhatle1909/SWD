import { PlusCircleOutlined } from "@ant-design/icons";
import { Button, Flex } from "antd";
import { useState } from "react";
import PageHeader from "../../../components/PageHeader";
import ListBlog from "../../../components/blog/ListBlog";
import CreateBlogModel from "../../../components/blog/CreateBlogModal";

export const ManageBlog = () => {
  const [isModalOpen, setIsModalOpen] = useState(false);
  return (
    <>
      <div className="w-[100vw] flex justify-center items-center"
          style={{ background: 'grey', paddingTop: '120px' }}>
        <div className=" w-[70vw]">
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
          <CreateBlogModel isModalOpen={isModalOpen} setIsModalOpen={setIsModalOpen} />
        </div>
      </div >
    </>
  )
}

export default ManageBlog;