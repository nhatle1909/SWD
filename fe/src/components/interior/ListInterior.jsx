import React, {useState, useRef, useEffect} from "react";
import { SearchOutlined } from "@ant-design/icons";
import { Form, Button, Space, Table, Input, Modal, message } from 'antd';
import Highlighter from 'react-highlight-words';
import { useAppDispatch, useAppSelector } from "../../store";
import React, { useRef, useState } from "react";
import { Button, Space, Table, Input, Modal } from 'antd';
import { actionGetInteriors, actionGetInteriosList } from "../../store/interior/action";

const ListInterior = () => {
    const dispatch = useAppDispatch();
    const interiors = useAppSelector(({ interiors }) => interiors.interior)

    const [request, setRequest] = React.useState({
        PageIndex: 1,
        IsAsc: true,
        SearchValue: "",
    });

    const [interior, setInterior] = React.useState(null);
    
    const handleDelete = (record) => {
        dispatch(actionRemoveAccount({ Email: record.email, Comments: "delete account " + record.email }));
    }

    const handleConfirmDelete = (record) => {
        console.log("record", record);
        setInterior(record);
        Modal.confirm({
            onOk: () => handleDelete(record),
            title: 'Confirm',
            content: 'Are you sure you want to delete this interior?',
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
            title: 'Name',
            dataIndex: 'interiorName',
            width: '30%',
            align: 'center'
        },
        {
            title: 'Price',
            dataIndex: 'price',
            width: '20%',
        },
        {
            title: 'Action',
            dataIndex: '',
            align: 'center',
            key: 'x',
            width: '20%',
            render: (text, record, index) => {
                return (<>
                    <Button onClick={() => handleShowInterior(record, index)} type="primary" className="blue">View</Button>
                    <Button onClick={() => handleConfirmDelete(record)} type="primary" danger className="red ms-2">Delete</Button>
                </>)
            },
        },
    ];
    
    React.useEffect(() => {
        dispatch(actionGetInteriosList({
            pageIndex: 1,
            isAsc: true,
            searchValue: ''
        }));
    }, []);

    return (<>
        <Table
            columns={columns}
            dataSource={interiors}
            pagination={{ pageSize: 5 }}
        />
    </>);
}

export default ListInterior;