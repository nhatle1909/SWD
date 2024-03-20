import { PlusCircleOutlined } from "@ant-design/icons";
import { Button, Flex } from "antd";
import { useState } from "react";
import PageHeader from "../../../components/PageHeader";
import ListBlog from "../../../components/blog/ListBlog";
import CreateBlogModel from "../../../components/blog/CreateBlogModal";
import EditBlogModel from "../../../components/blog/EditBlogModal";

export const ManageBlog = () => {
  const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
  const [isEditModalOpen, setIsEditModalOpen] = useState(false);
  const [selectedBlog, setSelectedBlog] = useState(null);

  const handleEditBlog = (record) => {
    setSelectedBlog(record);
    setIsEditModalOpen(true);
  }

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
              onClick={() => { setIsCreateModalOpen(true); }}
              icon={<PlusCircleOutlined />}
            >
              Create New Blog
            </Button>
          </Flex>
          <ListBlog handleEditBlog={handleEditBlog}/>
          <CreateBlogModel isModalOpen={isCreateModalOpen} setIsModalOpen={setIsCreateModalOpen} />
          <EditBlogModel isModalOpen={isEditModalOpen} setIsModalOpen={setIsEditModalOpen}
                selectedBlog={selectedBlog}/>
        </div>
      </div >
    </>
  )
}

export default ManageBlog;