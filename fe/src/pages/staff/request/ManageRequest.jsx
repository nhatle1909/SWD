import { linkImg } from '@/utils/common'
import React, { useRef, useState } from "react";
import { Button, Space, Table, Input, Modal, Form,Select } from 'antd';
import { SearchOutlined } from "@ant-design/icons";
import Highlighter from 'react-highlight-words';
import { actionGetRequests } from "../../../store/request/action";

import PageHeader from '../../../components/PageHeader';
import { useAppDispatch, useAppSelector } from "../../../store";

import { actionResponseRequest } from '../../../store/request/action';

import TextArea from "antd/es/input/TextArea";
import { toast } from "react-toastify";
import { responseRequest } from '../../../api/request';

import { useNavigate } from 'react-router-dom';


const ManageRequest = () => {
    React.useEffect(() => {
        dispatch(actionGetRequests(request));
        
    },[] );
    const dispatch = useAppDispatch();
    const requests = useAppSelector(({ request }) => request?.requests)
    const navigator = useNavigate();
    const interiors = useAppSelector(({ interior }) => interior.interiors);
    const [request, setRequest] = React.useState({
        PageIndex: 1,
        IsAsc: true,
        SearchValue: "",
    });
    let _id = "";
    let resReply = "";
    const [DataFile,setDataFile] = useState(null);
    
    const [searchText, setSearchText] = useState('');
    const [searchedColumn, setSearchedColumn] = useState('');
    const searchInput = useRef(null);
    const handleSearch = (selectedKeys, confirm, dataIndex) => {
        confirm();
        setSearchText(selectedKeys[0]);
        setSearchedColumn(dataIndex);
    };

    const handleDownloadPdf = (base64Pdf) => {
        const pdfData = `data:application/pdf;base64,${base64Pdf}`;
        const downloadLink = document.createElement('a');
        downloadLink.href = pdfData;
        downloadLink.download = 'download.pdf'; // Set the desired filename
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
    let modal;
    const handleReply = (action) => {
        if (action === "accept") {
            toast.success("Accept request successful");
            console.log(DataFile);
            modal.destroy();
            dispatch(responseRequest({id:_id, response:resReply, status:"Consulting", file:DataFile}));
            navigator("/staff/requests");
        } else {
            toast.success("Reject request successful");
            modal.destroy();
            dispatch(responseRequest({id:_id, response:resReply, status:"Completed", file:DataFile}));
        }
        if(action == "close")
        {
            modal.destroy();
        }
        modal.destroy();
    }
    const handleOpenReply = (record) => {
        console.log("record", record);
        _id = record.requestId;
        modal = Modal.info({
            title: 'Reply',
            content: (
                <>  <Form
                    name="basic"


                    autoComplete="off"
                >
                     <span>
                    <span style={{ fontWeight: "bold", marginTop: "10px;", display: "inline-block" }}>
                        Message:
                    </span>
                    <span> {record.content}</span>
                </span>
                <br/>
                <br/>
                    <Form.Item
                        label="Enter your message: "
                        name="reply"
                        required="true"
                    >
                        <TextArea  onChange={(e) =>resReply = e.target.value} style={{ width: "400xp" }} />
                        
                    </Form.Item>
                    <Form.Item label="Choose Image to send ( Optional )">
                   
                        <input type="file" onChange={(e) => setDataFile(e.target.files[0])}/>
                    </Form.Item>
                  
                  
                  
                </Form>
                </>
            ),
            footer: (_, { OkBtn, CancelBtn, close }) => (
                <>
                    <CancelBtn />
                    <Button onClick={() => handleReply("close", close)} type="primary">Close</Button>
                    <Button onClick={() => handleReply("reject", close)} type="primary" >Reject</Button>
                    <Button onClick={() => handleReply("accept", close)} type="primary">Accept</Button>
                </>
            ),
        });
    }

 

    const columns = [
        {
            title: 'Request Id',
            dataIndex: 'requestId',
            width: '10%',
            align: 'center',
        },
        {
            title: 'Email',
            dataIndex: 'email',
            ...getColumnSearchProps('email'),
            width: '20%',
        },
        {
            title: 'Created At',
            dataIndex: 'createdAt',
            width: '20%',
        },
        {
            title: 'Status',
            dataIndex: 'statusResponseOfStaff',
            width: '15%',
        },
        {
            title: 'Action',
            dataIndex: '',
            align: 'center',
            key: 'x',
            width: '35%',
            render: (text, record, index) => {
                return (<>
                    <Button onClick={() => handleShowRequest(record, index)} type="primary" className="blue me-2">View</Button>
                    {record.statusResponseOfStaff !== 'Completed' && record.statusResponseOfStaff !== 'Consulting' &&( // Check for not equal
        <Button onClick={() => handleOpenReply(record)} type="primary" className="red">
          Reply
        </Button>
      )}
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
                    {record.contactId !== null && (<span>{" " + record.requestId}</span>)}
                </span>
                <br />
                <span><span style={{ fontWeight: "bold" }}>
                    Email:
                </span>
                    <span>{" " + record.email}</span>
                </span>
                <br />
                <span>
                    <span style={{ fontWeight: "bold", marginTop: "10px;", display: "inline-block" }}>
                        Address:
                    </span>
                    <span> {record.address}</span>
                </span>
                <br />
                <span>
                    <span style={{ fontWeight: "bold", marginTop: "10px;", display: "inline-block" }}>
                        Message:
                    </span>
                    <span> {record.content}</span>
                </span>
                <br/>
               
                <span>
                    <span style={{ fontWeight: "bold", marginTop: "10px;", display: "inline-block" }}>
                        List Interior Of Request:
                    </span>
                    <span><ul>
                    {listInterior?.map((item) => (
                        <li key={item.interiorId}><a href={`/interior/${item.interiorId}`}>Interior</a></li>
                      
                    ))}
                    </ul>
                 </span>
                </span>
                <br/>
              
            </>,
            footer: (_, { OkBtn, CancelBtn }) => (
                <>
                    <CancelBtn />
                    <OkBtn />
                </>
            ),
        });
    }


    return (<>
        <div className="h-[90vh] w-[100vw] flex justify-center items-center" style={{
            background: '#343f4024'
        }}>
            <div className="h-[65vh] w-[70vw]">
                <PageHeader message={"List Request"} />

                <Table
                    columns={columns}
                    dataSource={requests}
                    pagination={{ pageSize: 5 }}
                />
            </div>
        </div >
    </>);
}

export default ManageRequest;