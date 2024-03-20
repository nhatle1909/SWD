import { useEffect}  from "react";
import { Form, Modal, Typography } from 'antd';
import { useAppDispatch, useAppSelector } from "../../store";
import { actionGetTransaction } from "../../store/transaction/action";

const TransactionDetailModal = ({ isModalOpen, setIsModalOpen, selectedTransaction }) => {
    const dispatch = useAppDispatch();
    useEffect(() => {
        if (isModalOpen && selectedTransaction)
            dispatch(actionGetTransaction({
            transactionId: selectedTransaction
            }));
    },[isModalOpen, selectedTransaction]);
    const transaction = useAppSelector(({transaction}) => transaction?.transaction);

    const handleOk = () => {
        setIsModalOpen(false);
    }
    const handleCancel = () => {
        setIsModalOpen(false);
        
    }

    return(<>
        {isModalOpen && console.log("Transaction", transaction)}
        <Modal title="Transaction Detail" open={isModalOpen} onOk={handleOk}
            onCancel={handleCancel} width={800}>
            <Form name="ViewDetailTransaction"
                labelCol={{ span: 4, }}
                wrapperCol={{ span: 21, }}
                key={transaction? transaction.transactionId : 'empty'}>
                <Form.Item label={<span style={{ fontWeight: 'bold' }}>Transaction Id</span>}
                    name="transactionId">
                        <Typography.Text>{transaction?.transactionId}</Typography.Text>
                </Form.Item>
                <Form.Item label={<span style={{ fontWeight: 'bold' }}>Request Id</span>}
                    name="requestId">
                        <Typography.Text>{transaction?.requestId}</Typography.Text>
                </Form.Item>
                <Form.Item label={<span style={{ fontWeight: 'bold' }}>Status</span>}
                    name="transactionStatus">
                        <Typography.Text>{transaction?.transactionStatus}</Typography.Text>
                </Form.Item>
                <Form.Item label={<span style={{ fontWeight: 'bold' }}>Total Price</span>}
                    name="totalPrice">
                        <Typography.Text>{transaction?.totalPrice.toLocaleString('en-US') + " VND"}</Typography.Text>
                </Form.Item>
                <Form.Item label={<span style={{ fontWeight: 'bold' }}>Remain Price</span>}
                    name="remainPrice">
                        <Typography.Text>{transaction?.remainPrice.toLocaleString('en-US') + " VND"}</Typography.Text>
                </Form.Item>
                <Form.Item label={<span style={{ fontWeight: 'bold' }}>Expired Date</span>}
                    name="expiredDate">
                        <Typography.Text>{transaction?.expiredDate.split('T')[0]}</Typography.Text>
                </Form.Item>
                <Form.Item label={<span style={{ fontWeight: 'bold' }}>Created At</span>}
                    name="createdAt">
                        <Typography.Text>{transaction?.createdAt.split('T')[0]}</Typography.Text>
                </Form.Item>
                <Form.Item label={<span style={{ fontWeight: 'bold' }}>updatedAt</span>}
                    name="updatedAt">
                        <Typography.Text>{transaction?.updatedAt.split('T')[0]}</Typography.Text>
                </Form.Item>
            </Form>
        </Modal>
    </>);
}

export default TransactionDetailModal;