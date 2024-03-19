import { useAppDispatch, useAppSelector } from "../../store";
import React, { useRef, useState } from "react";
import { Button, Space, Table, Input, Modal } from 'antd';
import { SearchOutlined } from "@ant-design/icons";
import Highlighter from 'react-highlight-words';
import { actionGetMyRequests, actionGetRequests } from "../../store/request/action";

import PageHeader from '../../components/PageHeader';
import { paymentLink } from "../../constants/constants";


const HistoryRequest = () => {
    const dispatch = useAppDispatch();
    const requests = useAppSelector(({ request }) => request?.requests);

    const [request, setRequest] = React.useState({
        PageIndex: 1,
        IsAsc: true,
        SearchValue: "",
    });

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

    const columns = [
        {
            title: 'Request Id',
            dataIndex: 'requestId',
            width: '5%',
            align: 'center',
        },
      
        {
            title: 'Created At',
            dataIndex: 'createdAt',
            width: '15%',
        },
        {
            title: 'Update At',
            dataIndex: 'updatedAt',
            width: '15%',
        },
        {
            title: 'Status',
            dataIndex: 'statusResponseOfStaff',
            width: '10%',
        },
        {
            title: 'Action',
            dataIndex: '',
            align: 'center',
            key: 'x',
            width: '20%',
            render: (text, record, index) => {
                return (<>
                    <Button onClick={() => handleShowRequest(record, index)} type="primary" className="blue">View</Button>
                </>)
            },
        },
    ];

    const handleShowRequest = (record, index) => {
        console.log("record", record);
        const listInterior = record.listInterior;
        Modal.info({
            title: 'Request Details',
            content: <>
                <span>
                    <span style={{ fontWeight: "bold", marginTop: "10px;", display: "inline-block" }}>
                        Request Id:
                    </span>
                    {record.requestId !== null && (<span>{" " + record.requestId}</span>)}
                </span>
                <br />
                <span><span style={{ fontWeight: "bold" }}>
                    Response of Staff:
                </span>
                    <span>{" " + record.responseOfStaff}</span>
                </span>
                <br />
                <span>
                    <span style={{ fontWeight: "bold", marginTop: "10px;", display: "inline-block" }}>
                        List Interior Of Request:
                    </span>
                    <span><ul>
                    {listInterior?.map((item) => (
                        <li key={item.interiorId}><a href={`/interior/${item.interiorId}`}>Item {listInterior.indexOf(item.interiorId) + 2}</a></li>
                    ))}
                    </ul>
                 </span>
                </span>
                <br/>
                <span><span style={{ fontWeight: "bold" }}>
                   Image : 
                </span>
                    <span>{" " + record.responseOfStaff}</span>
                </span>
                <br />
                               
             
            </>,
            footer: (_, { OkBtn, CancelBtn }) => (
                <>
                    <CancelBtn />
                    <OkBtn />
                </>
            ),
        });
    }
    React.useEffect(() => {
        dispatch(actionGetMyRequests(request));
    }, []);

    return (<>
        <div className="h-[90vh] w-[100vw] flex justify-center items-center" style={{
            background: '#343f4024'
        }}>
            <div className="h-[65vh] w-[70vw]">
                <PageHeader message={"History Request"} />

                <Table
                    columns={columns}
                    dataSource={requests}
                    pagination={{ pageSize: 5 }}
                />
            </div>
        </div >
    </>);
}

export default HistoryRequest;