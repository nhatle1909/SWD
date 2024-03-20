import React, { useEffect, useState } from 'react';
import { Modal, message, Form, Input, Select, Typography } from 'antd';
import { useDispatch } from 'react-redux';
import { actionUpdateBlog } from '../../store/blog/action';
const EditBlogModel = ({ isModalOpen, setIsModalOpen, selectedBlog}) => {
    const dispatch = useDispatch();
    const [blogId, setBlogId] = useState(null);
    const [blogTitle, setBlogTitle] = useState(null);
    const [blogContent, setBlogContent] = useState(null);
    const [pictures, setPictures] = useState(null);
    const [preview, setPreview] = useState(null);
    useEffect(() => {
        if (isModalOpen && selectedBlog){
            setBlogId(selectedBlog.blogId);
            setBlogTitle(selectedBlog.title);
            setBlogContent(selectedBlog.content);
            setPictures(selectedBlog.pictures);
            setPreview(`data:image/jpeg;base64,${selectedBlog.pictures[0]}`);
        }
    },[selectedBlog]);
    const handleOk = () => {
        if (!blogTitle) {
            message.error('Please input the blog title!');
            return;
        }
        if (!blogContent){
            message.error('Please input the blog content!');
            return;
        }
        if (!pictures){
            message.error('Please select pictures for interior!');
            return;
        }
        dispatch(actionUpdateBlog({
            blogId: blogId, title: blogTitle, content: blogContent,
            pictures: pictures
                }));
        setIsModalOpen(false);
    };
    const handleCancel = () => {
        setIsModalOpen(false);
    };

    const onFinish = (values) => {
        console.log('Success:', values);
    };
    
    const onFinishFailed = (errorInfo) => {
        console.log('Failed:', errorInfo);
    };

    const handleImageChange = (e) => {
        setPictures(e.target.files[0]);
        setPreview(URL.createObjectURL(e.target.files[0]));
    }
    return (
        <>
            {isModalOpen && console.log("Selected Blog", selectedBlog)}
            <Modal title="Edit Blog" open={isModalOpen} onOk={handleOk} onCancel={handleCancel}
                    width={900}>
                <Form
                    name="edit"
                    labelCol={{ span: 3, }}
                    wrapperCol={{ span: 21, }}
                    key={ selectedBlog? selectedBlog.blogId : 'empty'}
                    initialValues={{
                        blogId: selectedBlog?.blogId,
                        blogTitle: selectedBlog?.title,
                        blogContent: selectedBlog?.content,
                    }}
                    onFinish={onFinish}
                    onFinishFailed={onFinishFailed}
                    autoComplete="off"
                >
                    <Form.Item
                        label="Id"
                        name="blogId"
                    >
                        <Typography.Text>{blogId}</Typography.Text>
                    </Form.Item>
                    <Form.Item
                        label="Title"
                        name="blogTitle"
                        rules={[
                            {
                                required: true,
                                message: 'Please input the blog title!',
                            },
                        ]}
                    >
                        <Input onChange={(e) => setBlogTitle(e.target.value)} />
                    </Form.Item>
                    <Form.Item
                        label="Content"
                        name="blogContent"
                        rules={[
                            {
                                required: true,
                                message: 'Please input the blog content!',
                            },
                        ]}
                    >
                        <Input.TextArea onChange={(e) => setBlogContent(e.target.value)}
                                    rows={10}/>
                    </Form.Item>
                    <Form.Item
                        label="Pictures"
                        name="pictures"
                        rules={[
                            {
                                required: true,
                                message: 'Please select picture for blog!',
                            },
                        ]}
                    >
                        <>
                            {preview && <img src={preview} alt="Preview" />}
                            <input type="file" onChange={handleImageChange} />
                        </>
                    </Form.Item>
                </Form>
            </Modal>
        </>
    );
}

export default EditBlogModel;