import React, { useState } from 'react';
import { Modal, message, Form, Input, Select } from 'antd';
import { useDispatch } from 'react-redux';
import { actionAddInterior } from '../../store/interior/action';
import { actionAddBlog } from '../../store/blog/action';
const CreateBlogModel = ({ isModalOpen, setIsModalOpen }) => {
    const dispatch = useDispatch();
    const [title, setTitle] = useState("");
    const [content, setContent] = useState("");
    const [image, setImage] = useState("");
    const showModal = () => {
        setIsModalOpen(true);
    };
    const handleOk = () => {
        console.log(image);
        if (!title) {
            message.error('Please input the blog name!');
            return;
        }
        if (!content){
            message.error('Please input the blog content!');
            return;
        }
        if (!image){
            message.error('Please select image for blog!');
            return;
        }
        dispatch(actionAddBlog({ 
                    title: title, content: content, image: image
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
    
    return (
        <>
            <Modal title="Add New Blog" open={isModalOpen} onOk={handleOk} onCancel={handleCancel}>
                <Form
                    name="basic"
                    labelCol={{
                        span: 6,
                    }}
                    wrapperCol={{
                        span: 18,
                    }}

                    onFinish={onFinish}
                    onFinishFailed={onFinishFailed}
                    autoComplete="off"
                >
                    <Form.Item
                        label="Title"
                        name="title"
                        rules={[
                            {
                                required: true,
                                message: 'Please input the blog title!',
                            },
                        ]}
                    >
                        <Input onChange={(event) => setTitle(event.target.value)} />
                    </Form.Item>
                    <Form.Item
                        label="content"
                        name="content"
                        rules={[
                            {
                                required: true,
                                message: 'Please input the blog content!',
                            },
                        ]}
                    >
                        <Input onChange={(e) => setContent(e.target.value)} />
                    </Form.Item>
                    <Form.Item
                        label="Image"
                        name="image"
                        rules={[
                            {
                                required: true,
                                message: 'Please select image for interior!',
                            },
                        ]}
                    >
                        <input type="file" onChange={(e) => setImage(e.target.files[0])} />
                    </Form.Item>
                </Form>
            </Modal>
        </>
    );
}

export default CreateBlogModel;