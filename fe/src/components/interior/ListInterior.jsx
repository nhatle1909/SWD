import React, {useState, useRef, useEffect} from "react";
import { SearchOutlined } from "@ant-design/icons";
import { Form, Button, Space, Table, Input, Modal, message } from 'antd';
import Highlighter from 'react-highlight-words';
import { useAppDispatch, useAppSelector } from "../../store";
import { actionGetInteriors, actionRemoveInterior, actionUpdateInterior } from "../../store/interior/action";

const ListInterior = () => {
    const dispatch = useAppDispatch();
    const [request] = React.useState({
        PageIndex: 1,
        IsAsc: true,
        SearchValue: "",
    });
    React.useEffect(() => {
        dispatch(actionGetInteriors(request))
    },[]);
    const interiors = useAppSelector(({interiors}) => interiors?.interiors);

    const [interiorId, setInteriorId] = useState("");
    const [interiorName, setInteriorName] = useState("");
    const [interiorType, setInteriorType] = useState("");
    const [description, setDescription] = useState("");
    const [quantity, setQuantity] = useState("");
    const [price, setPrice] = useState("");
    const [image, setImage] = useState("");

    const [searchText, setSearchText] = useState('');
    const [searchedColumn, setSearchedColumn] = useState('');
    const searchInput = useRef(null);
    const handleSearch = (selectedKeys, confirm, dataIndex) => {
        confirm();
        setSearchText(selectedKeys[0]);
        setSearchedColumn(dataIndex);
    };
    const handleReset = (clearFilters) => {
        clearFilters();
        setSearchText('');
    };
    const getColumnSearchProps = (dataIndex) => ({
        filterDropdown: ({ setSelectedKeys, selectedKeys, confirm, clearFilters, close }) => (
            <div
                style={{
                    padding: 8,
                }}
                onKeyDown={(e) => e.stopPropagation()}
            >
                <Input
                    ref={searchInput}
                    placeholder={`Search ${dataIndex}`}
                    value={selectedKeys[0]}
                    onChange={(e) => setSelectedKeys(e.target.value ? [e.target.value] : [])}
                    onPressEnter={() => handleSearch(selectedKeys, confirm, dataIndex)}
                    style={{
                        marginBottom: 8,
                        display: 'block',
                    }}
                />
                <Space>
                    <Button
                        type="primary"
                        className="blue"
                        onClick={() => handleSearch(selectedKeys, confirm, dataIndex)}
                        icon={<SearchOutlined />}
                        size="small"
                        style={{
                            width: 90,
                        }}
                    >
                        Search
                    </Button>
                    <Button
                        onClick={() => clearFilters && handleReset(clearFilters)}
                        size="small"
                        style={{
                            width: 90,
                        }}
                    >
                        Reset
                    </Button>
                    <Button
                        type="link"
                        size="small"
                        onClick={() => {
                            confirm({
                                closeDropdown: false,
                            });
                            setSearchText(selectedKeys[0]);
                            setSearchedColumn(dataIndex);
                        }}
                    >
                        Filter
                    </Button>
                    <Button
                        type="link"
                        size="small"
                        onClick={() => {
                            close();
                        }}
                    >
                        close
                    </Button>
                </Space>
            </div>
        ),
        filterIcon: (filtered) => (
            <SearchOutlined
                style={{
                    color: filtered ? '#1677ff' : undefined,
                }}
            />
        ),
        onFilter: (value, record) =>
            record[dataIndex].toString().toLowerCase().includes(value.toLowerCase()),
        onFilterDropdownOpenChange: (visible) => {
            if (visible) {
                setTimeout(() => searchInput.current?.select(), 100);
            }
        },
        render: (text) =>
            searchedColumn === dataIndex ? (
                <Highlighter
                    highlightStyle={{
                        backgroundColor: '#ffc069',
                        padding: 0,
                    }}
                    searchWords={[searchText]}
                    autoEscape
                    textToHighlight={text ? text.toString() : ''}
                />
            ) : (
                text
            ),
    });

    const handleDelete = (record) => {
        dispatch(actionRemoveInterior({ interiorId: record.interiorId }));
    }

    const handleConfirmDelete = (record) => {
        console.log("record", record);
        Modal.confirm({
            onOk: () => handleDelete(record),
            title: 'Confirm',
            content: 'Are you sure you want to delete this Interior?',
            footer: (_, { OkBtn, CancelBtn }) => (
                <>
                    <CancelBtn />
                    <OkBtn type="primary" className="blue" />
                </>
            ),
        });
    }

    const handleEditOk = () => {
        if (!interiorName) {
            message.error('Please input the interior name!');
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
        if (!quantity){
            message.error('Please input price number!');
            return;
        }
        if (quantity<0){
            message.error('Invalid price!');
            return;
        }
        if (!image){
            message.error('Please select image for interior!');
            return;
        }
        dispatch(actionUpdateInterior({
                    interiorId: interiorId,interiorName: interiorName, price: price, image: image
                }));
    };
    const handleImageChange = (event) => {
        console.log("event",event);
        const data = new FileReader();
        data.addEventListener('load',() => {
            file = data.result;
            console.log("file",file);
            setImage(data.result);
        })
        data.readAsDataURL(event.target.files[0]);
    }
    var file;
    const handleEditInterior = (record) =>{
        console.log("record", record);
        setInteriorId(record.interiorId);
        setInteriorName(record.interiorName);
        setPrice(record.price);
        setQuantity(record.quantity);
        setImage(record.image);
        file = record.image;
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
                        <Input onChange={(e) => setInteriorName(e.target.value)} />
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
                        <Input onChange={(e) => setPrice(e.target.value)}/>
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
                        <Input onChange={(e) => setQuantity(e.target.value)}/>
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
                footer: (_, {OkBtn, CancelBtn}) => (<>
                    <CancelBtn />
                    <OkBtn />
                </>),
        });
    }

    const handleShowInterior = (record) => {
        console.log("record", record);
        Modal.info({
            title: 'Interior Details',
            content: <>
                <span><span style={{ fontWeight: "bold" }}>
                    Id:
                </span>
                    <span>{" " + record.interiorId}</span>
                </span>
                <br />
                <span><span style={{ fontWeight: "bold" }}>
                    Name:
                </span>
                    <span>{" " + record.interiorName}</span>
                </span>
                <br />
                <span><span style={{ fontWeight: "bold" }}>
                    Price:
                </span>
                    <span>{" " + record.price}</span>
                </span>
                <br />
                <span>
                    <span style={{ fontWeight: "bold", marginTop: "10px", display: "inline-block" }}>
                        Image:
                    </span>
                    <span><a className="block-20"
                            style={{
                                backgroundImage: `url(data:image/jpeg;base64,${record.image})`
                            }}>
                    </a></span>
                </span>
            </>,
            footer: (_, { OkBtn}) => ( <OkBtn />),
        });
    }

    const columns = [
        {
            title: 'Name',
            dataIndex: 'interiorName',
            ...getColumnSearchProps('interiorName'),
            width: '15%',
        },
        {
            title: 'Image',
            dataIndex: 'image',
            width: '45%',
            render: (text,record) => (
                <a className="block-20"
                    style={{
                        backgroundImage: `url(data:image/jpeg;base64,${record.image})`
                    }}>
                </a>
            )
        },
        {
            title: 'Price',
            dataIndex: 'price',
            width: '10%',
        },
        {
            title: 'Quantity',
            dataIndex: 'quantity',
            width: '10%',
        },
        {
            title: 'Created At',
            dataIndex: 'createdAt',
            width: '10%',
            render: (text, record) => {
                return text.split('T')[0];
            }
        },
        {
            title: 'Action',
            align: 'center',
            width: '20%',
            render: (record) => {
                return (<div style={{ display: 'flex', flexDirection: 'column', gap: '10px' }}>
                    <Button onClick={() => handleShowInterior(record)} type="primary" className="blue">View</Button>
                    <Button onClick={() => handleEditInterior(record)} type="primary">Edit</Button>
                    <Button onClick={() => handleConfirmDelete(record)} type="primary" danger className="red">Delete</Button>
                </div>)
            },
        },
    ];
    return(<>
        <Table
            rowKey= "interiorId"
            columns={columns}
            dataSource={interiors}
            pagination= {{ pageSize: 5 }}>
        </Table>
    </>);
}

export default ListInterior;