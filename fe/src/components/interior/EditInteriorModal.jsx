import React, { useEffect, useState } from 'react';
import { Modal, message, Form, Input, Select } from 'antd';
import { useDispatch } from 'react-redux';
import { actionAddInterior } from '../../store/interior/action';
const EditInteriorModel = ({ isModalOpen, setIsModalOpen, interior}) => {
    const dispatch = useDispatch();
    const [interiorName, setInteriorName] = useState("");
    const [interiorType, setInteriorType] = useState("");
    const [description, setDescription] = useState("");
    const [quantity, setQuantity] = useState("");
    const [price, setPrice] = useState("");
    const [image, setImage] = useState("");

    useEffect(() => {
        if (isModalOpen && interior){
            setInteriorName(interior.interiorName);
            setInteriorType(interior.interiorType);
            setDescription(interior.description);
            setQuantity(interior.quantity);
            setPrice(interior.price);
            setImage(interior.image);
        }
    },[isModalOpen,interior]);
    const handleOk = () => {
        if (!interiorName) {
            message.error('Please input the interior name!');
            return;
        }
        if (!interiorType){
            message.error('Please select interior type!');
            return;
        }
        if (!quantity){
            message.error('Please input quantity number!');
            return;
        }
        if (quantity<0){
            message.error('Invalid quantity!');
            return;
        }
        if (!price){
            message.error('Please input price number!');
            return;
        }
        if (price<=0){
            message.error('Invalid price!');
            return;
        }
        if (!image){
            message.error('Please select image for interior!');
            return;
        }
        dispatch(actionAddInterior({ 
                    interiorName: interiorName, interiorType: interiorType, description: description,
                    quantity: quantity, price: price, image: image
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
            <Modal title="Edit Interior" open={isModalOpen} onOk={handleOk} onCancel={handleCancel}>
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
                        label="Name"
                        name="interiorName"
                        rules={[
                            {
                                required: true,
                                message: 'Please input the interior name!',
                            },
                        ]}
                    >
                        <Input value={interiorName} onChange={(e) => setInteriorName(e.target.value)} />
                    </Form.Item>
                    <Form.Item
                        label="Interior Type"
                        name="interiorType"
                        rules={[
                            {
                                required: true,
                                message: 'Please select interior type!',
                            },
                        ]}
                    >
                        <Select onChange={(value) => setInteriorType(value)}>
                            <Option value="Chair">Chair</Option>
                            <Option value="Desk">Desk</Option>
                            <Option value="Cabinet">Cabinet</Option>
                            <Option value="Clock">Clock</Option>
                        </Select>
                    </Form.Item>
                    <Form.Item
                        label="Description"
                        name="description"
                    >
                        <Input onChange={(e) => setDescription(e.target.value)} />
                    </Form.Item>
                    <Form.Item
                        label="Quantity"
                        name="quantity"
                        rules={[
                            {
                                required: true,
                                message: 'Please input quantity number!',
                            },
                        ]}
                    >
                        <Input onChange={(e) => setQuantity(e.target.value)} />
                    </Form.Item>
                    <Form.Item
                        label="Price"
                        name="price"
                        rules={[
                            {
                                required: true,
                                message: 'Please input price number!',
                            },
                        ]}
                    >
                        <Input onChange={(e) => setPrice(e.target.value)} />
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

export default EditInteriorModel;