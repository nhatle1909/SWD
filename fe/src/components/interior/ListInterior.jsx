import React, {useState, useRef, useEffect} from "react";
import { SearchOutlined } from "@ant-design/icons";
import { Button, Space, Table, Input, Modal, Typography, Image, Row } from 'antd';
import Highlighter from 'react-highlight-words';
import { useAppDispatch, useAppSelector } from "../../store";
import { actionGetInteriors, actionRemoveInterior, actionUpdateInterior } from "../../store/interior/action";
const {Title, Text} = Typography;

const ListInterior = ({ handleEditInterior }) => {
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
                <span>
                    <span style={{ fontWeight: "bold" }}>Interior Type:</span>
                    <span>{" " + record.interiorType}</span>
                </span>
                <br />
                <span><span style={{ fontWeight: "bold" }}>
                    Description:
                </span>
                    <br/>
                    <pre style={{ whiteSpace: "pre-wrap", wordWrap:"break-word"}}>{record.description}</pre>
                </span>
                <br />
                <span><span style={{ fontWeight: "bold" }}>
                    Price:
                </span>
                    <span>{" " + record.price.toLocaleString('en-US') + " VND"}</span>
                </span>
                <br />
                <span><span style={{ fontWeight: "bold" }}>
                    Quantity:
                </span>
                    <span>{" " + record.quantity}</span>
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
            width: 1200,
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
            render: (text, record) => `${record.price.toLocaleString('en-US')} VND`
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
            render: (text) => {
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