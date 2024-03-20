import React, { useEffect, useState } from 'react';
import { Modal, message, Form, Input, Select } from 'antd';
import { useDispatch } from 'react-redux';
import { actionUpdateInterior } from '../../store/interior/action';
const EditInteriorModel = ({ isModalOpen, setIsModalOpen, selectedInterior}) => {
    const dispatch = useDispatch();
    const [interiorName, setInteriorName] = useState("");
    const [interiorType, setInteriorType] = useState("");
    const [description, setDescription] = useState("");
    const [quantity, setQuantity] = useState("");
    const [price, setPrice] = useState("");
    const [image, setImage] = useState(null);
    const [preview, setPreview] = useState(null);

    useEffect(() => {
        if (isModalOpen && selectedInterior){
            setInteriorName(selectedInterior.interiorName);
            setInteriorType(selectedInterior.interiorType);
            setDescription(selectedInterior.description);
            setQuantity(selectedInterior.quantity);
            setPrice(selectedInterior.price);
            setImage(selectedInterior.image);
            setPreview(`data:image/jpeg;base64,${selectedInterior.image}`);
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

    const handleImageChange = (e) => {
        setImage(e.target.files[0]);
        setPreview(URL.createObjectURL(e.target.files[0]));
    }
    return (
        <>
            {console.log("selectedInterior", selectedInterior)}
            <Modal title="Edit Interior" open={isModalOpen} onOk={handleOk} onCancel={handleCancel}
                    width={900}>
                <Form
                    name="edit"
                    labelCol={{ span: 3, }}
                    wrapperCol={{ span: 21, }}
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
                        <Input.TextArea onChange={(e) => setDescription(e.target.value)}
                                    rows={10}/>
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
                        {preview && <img src={preview} alt="Preview" />}
                        <input type="file" onChange={handleImageChange} />
                    </Form.Item>
                </Form>
            </Modal>
        </>
    );
}

export default EditInteriorModel;