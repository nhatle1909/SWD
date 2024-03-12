
import React, { useState } from 'react';
import { Button, Modal, Checkbox, Form, Input, Radio } from 'antd';
import { useDispatch } from 'react-redux';
import { actionAddAccount, actionRemoveAccount } from '../../store/user/action';
const CreateAccountModel = ({ isModalOpen, setIsModalOpen }) => {
    const dispatch = useDispatch();
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [phone, setPhone] = useState("");
    const [accountType, setAccountType] = useState("");
    const showModal = () => {
        setIsModalOpen(true);
    };
    const handleOk = () => {
        dispatch(actionAddAccount({ email: email, password: password, phoneNumber: phone, accountType: accountType }));
        setIsModalOpen(false);
    };
    const handleCancel = () => {
        setIsModalOpen(false);
    };

    const onFinish = (values) => {
        console.log('Success:', values);
    };
    const handleRadioChange = (e) => {
        console.log("e", e.target);
        setAccountType(e.target.value);
    };
    const onFinishFailed = (errorInfo) => {
        console.log('Failed:', errorInfo);
    };
    return (
        <>
            <Modal title="Add New Account" open={isModalOpen} onOk={handleOk} onCancel={handleCancel}>
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
                        label="Email"
                        name="email"
                        rules={[
                            {
                                required: true,
                                message: 'Please input your email!',
                            },
                        ]}
                    >
                        <Input onChange={(e) => setEmail(e.target.value)} />
                    </Form.Item>
                    <Form.Item
                        label="Phone Number"
                        name="phoneNumber"
                        rules={[
                            {
                                required: true,
                                message: 'Please input your phone number!',
                            },
                        ]}
                    >
                        <Input onChange={(e) => setPhone(e.target.value)} />
                    </Form.Item>
                    <Form.Item
                        label="Password"
                        name="password"
                        rules={[
                            {
                                required: true,
                                message: 'Please input your password!',
                            },
                        ]}
                    >
                        <Input.Password onChange={(e) => setPassword(e.target.value)} />
                    </Form.Item>
                    <Form.Item initialValue="customer" name={"type"} label="Account Type:  ">
                        <Radio.Group>
                            <Radio onClick={() => setAccountType("customer")} checked={"checked"} value="customer"> Customer </Radio>
                            <Radio onClick={() => setAccountType("staff")} value="staff"> Staff </Radio>
                        </Radio.Group>
                    </Form.Item>
                </Form>
            </Modal>
        </>
    );
}

export default CreateAccountModel;

