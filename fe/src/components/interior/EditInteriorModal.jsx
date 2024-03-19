import React, { useEffect, useState } from 'react';
import { Modal, message, Form, Input, Select } from 'antd';
import { useDispatch } from 'react-redux';
import { actionAddInterior, actionUpdateInterior } from '../../store/interior/action';
const EditInteriorModel = ({ isModalOpen, setIsModalOpen, selectedInterior}) => {
    const dispatch = useDispatch();
    const [interiorName, setInteriorName] = useState("");
    const [interiorType, setInteriorType] = useState("");
    const [description, setDescription] = useState("");
    const [quantity, setQuantity] = useState("");
    const [price, setPrice] = useState("");
    const [image, setImage] = useState("");

    useEffect(() => {
        if (isModalOpen && selectedInterior){
            setInteriorName(selectedInterior.interiorName);
            setInteriorType(selectedInterior.interiorType);
            setDescription(selectedInterior.description);
            setQuantity(selectedInterior.quantity);
            setPrice(selectedInterior.price);
            setImage(selectedInterior.image);
        }
    },[selectedInterior]);
    const handleOk = () => {
        if (!interiorName) {
            message.error('Please input the interior name!');
            return;
        }
        if (!interiorType){
            message.error('Please select interior type!');
            return;
        }
        if (!description) {
            message.error('Please input the interior description!');
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
        dispatch(actionUpdateInterior({ 
                    interiorId: selectedInterior.interiorId, interiorName: interiorName,
                    interiorType: interiorType, description: description,
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
    {/**
 Modal.confirm({
            title: 'Edit Interior',
            onOk: () => handleEditOk(),
            content:
                <Form
                    name="basic"
                    labelCol={{ span: 6 }}
                    wrapperCol={{ span: 18 }}
                    autoComplete="off"
                    initialValues={{
                        interiorId: record.interiorId,
                        interiorName: record.interiorName,
                        interiorType: record.interiorType,
                        description: record.description,
                        price: record.price,
                        quantity: record.quantity       
                    }}
                >
                    <Form.Item
                        label="ID"
                        name = "interiorId"
                    >
                        <span>{ record.interiorId }</span>
                    </Form.Item>
                    <Form.Item
                        label="Name"
                        name = "interiorName"
                        rules={[
                            {
                                required: true,
                                message: 'Please input the interior name!',
                            },
                        ]}
                    >
                        <Input onChange={(e) => name = e.target.value} />
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
                        <Select onChange={(value) => type = value}>
                            <Option value="Chair">Chair</Option>
                            <Option value="Desk">Desk</Option>
                            <Option value="Cabinet">Cabinet</Option>
                            <Option value="Clock">Clock</Option>
                        </Select>
                    </Form.Item>
                    <Form.Item
                        label="Description"
                        name = "description"
                        rules={[
                            {
                                required: true,
                                message: 'Please input the interior name!',
                            },
                        ]}
                    >
                        <Input.TextArea onChange={(e) => des = e.target.value}
                                        rows={8}/>
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
                        <Input onChange={(e) => pric = e.target.value}/>
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
                        <Input onChange={(e) => quan = e.target.value}/>
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
                        <div>
                            <a className="block-20" style={{backgroundImage: `url(data:image/jpeg;base64,${file})`}}></a>
                            <Input type="file" onChange={handleImageChange}/>
                        </div>
                    </Form.Item>
                </Form>,
                width: 900,
                footer: (_, {OkBtn, CancelBtn}) => (<>
                    <CancelBtn />
                    <OkBtn />
                </>),
        });
    */}
    return (
        <>
        {console.log("selectedInterior", selectedInterior)}
            <Modal title="Edit Interior" open={isModalOpen} onOk={handleOk} onCancel={handleCancel}>
                <Form
                    name="basic"
                    labelCol={{
                        span: 6,
                    }}
                    wrapperCol={{
                        span: 18,
                    }}
                    key={ selectedInterior? selectedInterior.interiorId : 'empty'}
                    initialValues={{
                        interiorId: selectedInterior?.interiorId,
                        interiorName: selectedInterior?.interiorName,
                        interiorType: selectedInterior?.interiorType,
                        description: selectedInterior?.description,
                        price: selectedInterior?.price,
                        quantity: selectedInterior?.quantity       
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
                        <Input onChange={(e) => setInteriorName(e.target.value)} />
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