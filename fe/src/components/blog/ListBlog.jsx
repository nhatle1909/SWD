import { useAppDispatch, useAppSelector } from "../../store";
import React, { useRef, useState } from "react";
import { Button, Space, Table, Input, Modal } from 'antd';
import { SearchOutlined } from "@ant-design/icons";
import Highlighter from 'react-highlight-words';
import { actionGetBlogList, actionGetBlogs, actionRemoveBlog } from "../../store/blog/action";

const ListBlog = () => {
    const dispatch = useAppDispatch();
    const blogs = useAppSelector(({ blogs }) => blogs.blogs);

    const [request, setRequest] = React.useState({
        PageIndex: 1,
        IsAsc: true,
        SearchValue: "",
    });

    const [blog, setBlog] = React.useState(null);

    const handleShowBlog = (record, index) => {
        console.log("record", record);
        setBlog(record);
        Modal.info({
            title: 'Blog Details',
            content: <>
                <span><span style={{ fontWeight: "bold" }}>
                    Title:
                </span>
                    <span>{" " + record.title}</span>
                </span>
                <br />
                <span>
                    <span style={{ fontWeight: "bold", marginTop: "10px;", display: "inline-block" }}>
                        Content:
                    </span>
                    <span>{" " + record.content}</span>
                </span>
            </>,
            footer: (_, { OkBtn, CancelBtn }) => (
                <>
                    <CancelBtn />
                    <OkBtn />
                </>
            ),
        });
    }

    const handleDelete = (record) => {
        dispatch(actionRemoveBlog({ interiorId: record.interiorId }));
    }

    const handleConfirmDelete = (record) => {
        console.log("record", record);
        setBlog(record);
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
            width: '45%',
        },
        {
            title: 'Created At',
            dataIndex: 'createdAt',
            width: '15%',
            render: (text, record) => {
                return text.split('T')[0];
            }
        },
        {
            title: 'Action',
            dataIndex: '',
            align: 'center',
            key: 'x',
            width: '20%',
            render: (text, record, index) => {
                return(<>
                    <Button onClick={() => handleShowBlog(record, index)} type="primary" className="blue">View</Button>
                    <Button onClick={() => handleConfirmDelete(record)} type="primary" danger className="red ms-2">Delete</Button>
                </>)
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
            columns={columns}
            dataSource={blogs}
            pagination = {{ pageSize: 5 }}
        />
    </>);
}
export default ListBlog;