import { actionGetAccounts } from "../../store/user/action";
import { useAppDispatch, useAppSelector } from "../../store";
import React, { useRef, useState } from "react";
import { Button, Space, Table, Input } from 'antd';
import { SearchOutlined } from "@ant-design/icons";
import CreateAccountModel from "./CreateAccountModel";
import ViewAccountModel from "./ViewAccountModel";
import Highlighter from 'react-highlight-words';



const ListAccount = () => {
    const dispatch = useAppDispatch();
    const accounts = useAppSelector(({ user }) => user?.accounts)

    const [request, setRequest] = React.useState({
        PageIndex: 1,
        IsAsc: true,
        SearchValue: "",
    });

    const [account, setAccount] = React.useState(null);

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

    const handleConfirmDelete = () => {
        Modal.confirm({
            title: 'Confirm',
            content: 'Are you sure you want to delete this account?',
            footer: (_, { OkBtn, CancelBtn }) => (
                <>
                    <CancelBtn />
                    <OkBtn />
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
            ...getColumnSearchProps('email'),
            width: '35%',
        },
        {
            title: 'Phone Number',
            dataIndex: 'phone',
            width: '20%',
        },
        {
            title: 'Role',
            dataIndex: 'role',
            width: '10%',
        },
        {
            title: 'Action',
            dataIndex: '',
            align: 'center',
            key: 'x',
            width: '20%',
            render: () => <>
                <Button type="primary" className="blue">View</Button>
                <Button onClick={handleConfirmDelete} type="primary" danger className="red ms-2">Delete</Button>
            </>,
        },
    ];


    React.useEffect(() => {
        dispatch(actionGetAccounts(request));
    }, []);

    return (<>
        <Table
            columns={columns}
            dataSource={accounts}
        />
        <ViewAccountModel />
    </>);
}

export default ListAccount;