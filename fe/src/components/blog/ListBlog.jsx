import { useAppDispatch, useAppSelector } from "../../store";
import React, {useState, useRef} from "react";
import { SearchOutlined } from "@ant-design/icons";
import Highlighter from 'react-highlight-words';
import { Button, Table,  Modal, Input, Space } from 'antd';
import { actionGetBlogList, actionRemoveBlog } from "../../store/blog/action";

const ListBlog = ({handleEditBlog}) => {
    const dispatch = useAppDispatch();
    const blogs = useAppSelector(({ blogs }) => blogs.blogs);

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

    const handleShowBlog = (record) => {
        console.log("record", record);
        Modal.info({
            title: 'Blog Details',
            content: (
                <div style={{ lineHeight: '1.5', fontSize: '16px' }}>
                    <div style={{ marginBottom: '10px' }}>
                        <span style={{ fontWeight: "bold" }}>Title:</span>
                        <br/>
                        <span>{" " + record.title}</span>
                    </div>
                    <div>
                        <span style={{ fontWeight: "bold" }}>Content:</span>
                        <br/>
                        <span style={{ whiteSpace: "pre-line"}}>{" " + record.content}</span>
                    </div>
                    <br/>
                    <span>
                        <span style={{ fontWeight: "bold", marginTop: "10px", display: "inline-block" }}>
                            Image:
                        </span>
                        <br/>
                        <span><a className="block-20"
                                style={{
                                    backgroundSize: 'cover',
                                    backgroundImage: `url(data:image/jpeg;base64,${record.pictures})`
                                }}>
                        </a></span>
                    </span>
                </div>
            ),
            footer: (_, { OkBtn, CancelBtn }) => (
                <>
                    <CancelBtn />
                    <OkBtn />
                </>
            ),
            width: 1200,
            
        });
    }

    const handleDelete = (record) => {
        dispatch(actionRemoveBlog({ blogId: record.blogId }));
    }

    const handleConfirmDelete = (record) => {
        console.log("record", record);
        Modal.confirm({
            onOk: () => handleDelete(record),
            title: 'Confirm',
            content: 'Are you sure you want to delete this blog?',
            footer: (_, { OkBtn, CancelBtn }) => (
                <>
                    <CancelBtn />
                    <OkBtn type="primary" className="blue" />
                </>
            ),
        });
    }

    const columns = [
        {
            title: 'Index',
            dataIndex: 'index',
            width: '5%',
            align: 'center',
            render: (text, record, index) => <span>{index + 1}</span>,
        },
        {
            title: 'Email',
            dataIndex: 'email',
            width: '15%',
        },
        {
            title: 'Tittle',
            dataIndex: 'title',
            ...getColumnSearchProps('title'),
            width: '45%',
        },
        {
            title: 'Created At',
            dataIndex: 'createdAt',
            width: '15%',
            render: (text) => {
                return text.split('T')[0];
            }
        },
        {
            title: 'Action',
            align: 'center',
            key: 'x',
            width: '10%',
            render: (record) => {
                return(<div style={{ display: 'flex', flexDirection: 'column', gap: '10px' }}>
                    <Button onClick={() => handleShowBlog(record)} type="primary" className="blue">View</Button>
                    <Button onClick={() => handleEditBlog(record)} type="primary">Edit</Button>
                    <Button onClick={() => handleConfirmDelete(record)} type="primary" danger className="red">Delete</Button>
                </div>)
            }
        },
    ];

    React.useEffect(() => {
        dispatch(actionGetBlogList({
            pageIndex: 1,
            isAsc: true,
            searchValue: ''
        }));
    },[]);
     
    return (<>
        <Table
            rowKey="blogId"
            columns={columns}
            dataSource={blogs}
            pagination = {{ pageSize: 5 }}
        />
    </>);
}
export default ListBlog;