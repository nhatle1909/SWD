import { useAppSelector } from '@/store';
import React, { useState } from 'react';
import { Button, Modal, Checkbox, Form, Input, Radio, Select } from 'antd';
import { useDispatch } from 'react-redux';
import { createRequestAction } from '../../store/request/action';
import { actionGetInteriors } from '../../store/interior/action';

import PageHeader from '../../components/PageHeader';
const CreateRequest = () => {
    const auth = useAppSelector(({ authentication }) => authentication.authUser);

    const interiors = useAppSelector(({ interior }) => interior.interiors);
    const dispatch = useDispatch();
    const [email, setEmail] = useState(auth.email);
    const [address, setAddress] = useState("");
    const [phone, setPhone] = useState("");
    const [content, setContent] = useState("");
    const [interiorId, setInteriorId] = useState("");

    const handleOk = () => {
        dispatch(createRequestAction({ Email: email, Address: address, Phone: phone, Content: content, Interior: interiorId }));
    };

    React.useEffect(() => {
        dispatch(actionGetInteriors({
            PageIndex: 1,
            IsAsc: true,
            SearchValue: "",
        }));
    }, [])


    console.log("interiors", interiors);
    return (<>
        <div className="h-[90vh] w-[100vw] flex justify-center items-center" style={{
            background: '#343f4024'
        }}>
            <div className="h-[50vh] w-[60vw]">
                <PageHeader message={"Create New Request"} />
                <Form

                    name="basic"
                    labelCol={{
                        span: 6,
                    }}
                    wrapperCol={{
                        span: 12,
                    }}


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
                        <Input value={email} onChange={(e) => setEmail(e.target.value)} />
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
                        <Input maxLength={10} onChange={(e) => setPhone(e.target.value)} />
                    </Form.Item>
                    <Form.Item
                        label="Address"
                        name="address"
                        rules={[
                            {
                                required: true,
                                message: 'Please input your address!',
                            },
                        ]}
                    >
                        <Input onChange={(e) => setAddress(e.target.value)} />
                    </Form.Item>
                    <Form.Item
                        label="Content"
                        name="content"
                        rules={[
                            {
                                required: true,
                                message: 'Please input your content!',
                            },
                        ]}
                    >
                        <Input onChange={(e) => setContent(e.target.value)} />
                    </Form.Item>

                    <Form.Item label="Choose interior"
                        rules={[
                            {
                                required: true,
                                message: 'Please input your content!',
                            },
                        ]} >
                        <Select onChange={(value) => {
                            setInteriorId(value);
                        }}>
                            {interiors.map((interior) => <Select.Option value={interior.interiorId}>{interior.interiorName}</Select.Option>)}                        </Select>
                    </Form.Item>

                    <Form.Item wrapperCol={{ offset: 6, span: 12 }}>
                        <Button onClick={() => handleOk()} type="primary" htmlType="button">
                            Submit
                        </Button>
                    </Form.Item>
                </Form>


            </div>

        </div >
    </>)
}

export default CreateRequest;