import React, { useRef, useState } from "react";
import { PlusCircleOutlined } from "@ant-design/icons";
import { Button, Space, Table, Input, Modal, Form } from 'antd';
import { Flex } from "antd";
import { SearchOutlined } from "@ant-design/icons";
import Highlighter from 'react-highlight-words';
import { actionGetRequests } from "../../../store/request/action";

import PageHeader from '../../../components/PageHeader';
import { useAppDispatch, useAppSelector } from "../../../store";
import TextArea from "antd/es/input/TextArea";
import { toast } from "react-toastify";
import { actionRemoveInterior } from "../../../store/interior/action";
import ListInterior from "../../../components/interior/ListInterior";
const ManageInterior = () => {
    const dispatch = useAppDispatch();
    const interiors = useAppSelector(({ interiors }) => interiors.interior);

    const handleDelete = (record) => {
        dispatch(actionRemoveInterior({ interiorId : record.interiorId }));
    }

    const handleConfirmDelete = (record) => {
        console.log("record", record);
        setAccount(record);
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
            width: '35%',
            align: 'center',
        },
        {
            title: 'Quantity',
            dataIndex: 'quantity',
            width: '10%',
        },
        {
            title: 'Price',
            dataIndex: 'price',
            width: '10%',
        },
        {
            title: 'Created At',
            dataIndex: 'createdAt',
            width: '15%',
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
    return (<>
        <div className="h-[90vh] w-[100vw] flex justify-center items-center"
            style={{ background: '#343f4024' }}>
            <div className="h-[65vh] w-[70vw]">
                <PageHeader message={"List Interior"} />
                <Flex
                    gap="small"
                    style={{ width: '100%' }}
                    justify="flex-end"
                >
                    <Button
                        type="primary"
                        danger
                        className="blue mb-2"
                        onClick={() => { setIsModalOpen(true); }}
                        icon={<PlusCircleOutlined />}
                    >
                    Create New Interior
                    </Button>
                </Flex>
                <ListInterior />
            </div>
        </div >
    </>);
}
export default ManageInterior;