import { useAppSelector } from '@/store';
import React, { useState } from 'react';
import { Button, Modal, Checkbox, Form, Input, Radio, Select } from 'antd';
import { useDispatch } from 'react-redux';
import { createRequestAction } from '../../store/request/action';
import { actionGetInteriors } from '../../store/interior/action';

import PageHeader from '../../components/PageHeader';
import { useNavigate } from 'react-router-dom';
const CreateRequest = () => {
    const auth = useAppSelector(({ authentication }) => authentication.authUser);
    const navigator = useNavigate();

    const interiors = useAppSelector(({ interior }) => interior.interiors);
    const dispatch = useDispatch();
    const [email, setEmail] = useState(auth.email);
    const [address, setAddress] = useState("");
    const [phone, setPhone] = useState("");
    const [content, setContent] = useState("");
    const [interiorId, setInteriorId] = useState("65f04adf953c9051e6f3152f");

    const handleOk = () => {
        dispatch(createRequestAction({ Email: email, Address: address, Phone: phone, Content: content, Interior: interiorId }));
        navigator("/request/history");
    };

    React.useEffect(() => {
        dispatch(actionGetInteriors({
            PageIndex: 1,
            IsAsc: true,
            SearchValue: "",
        }));
    }, [])

    const [selectedItems, setSelectedItems] = useState([]);
    let interiorNamesArray = interiors?.map(item => item.interiorName);

    const filteredOptions = interiorNamesArray?.filter((o) => !selectedItems.includes(o));


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
                        <Input placeholder='Enter email' value={email} onChange={(e) => setEmail(e.target.value)} />
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
                        <Input placeholder='Enter phone' maxLength={10} onChange={(e) => setPhone(e.target.value)} />
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
                        <Input placeholder='Enter address' onChange={(e) => setAddress(e.target.value)} />
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
                        <Input placeholder='Enter content' onChange={(e) => setContent(e.target.value)} />
                    </Form.Item>

                    <Form.Item label="Choose interior"
                        rules={[
                            {
                                required: true,
                                message: 'Please input your content!',
                            },
                        ]} >
                        <Select
                            mode="multiple"
                            placeholder="Choose interior"
                            value={selectedItems}
                            onChange={setSelectedItems}
                            style={{ width: '100%' }}
                            options={filteredOptions?.map((item) => ({
                                value: item,
                                label: item,
                            }))}
                        />
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