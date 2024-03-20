import React, { useState, useRef } from 'react';
import { Modal, message, Form, Input, Select, Row } from 'antd';
import { useDispatch } from 'react-redux';
import { actionAddInterior } from '../../store/interior/action';
import { actionAddBlog } from '../../store/blog/action';
const CreateBlogModel = ({ isModalOpen, setIsModalOpen }) => {
    const dispatch = useDispatch();
    const [title, setTitle] = useState("");
    const [content, setContent] = useState("");
    const [image, setImage] = useState("");
    const formRef = useRef();

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
        formRef.current.resetFields();
        setIsModalOpen(false);
    };
    const handleCancel = () => {
        formRef.current.resetFields();
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
            <Modal title="Add New Blog" open={isModalOpen} onOk={handleOk} onCancel={handleCancel}
                    width={900}>
                <Form
                    name="basic"
                    labelCol={{
                        span: 6,
                    }}
                    wrapperCol={{
                        span: 18,
                    }}
                    ref={formRef}
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
                        <Input.TextArea onChange={(e) => setContent(e.target.value)} 
                                        rows={10}/>
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