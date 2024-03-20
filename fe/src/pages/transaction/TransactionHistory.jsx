import { useAppDispatch, useAppSelector } from "../../store";
import React, { useRef, useState } from "react";
import { Button, Space, Table, Input, Modal } from "antd";
import { SearchOutlined } from "@ant-design/icons";
import Highlighter from "react-highlight-words";

import { actionGetMyTransactions } from "../../store/transaction/action";
import TransactionDetailModal from "../../components/transaction/TransactionDetailModal";

import PageHeader from "../../components/PageHeader";

const TransactionHistory = () => {
  const [selectedTransaction, setSelectedTransaction] = useState(null);
  const [isViewDetailTransactionModalOpen,
     setIsViewDetailTransactionModalOpen] = useState(false);

  const handleViewDetailTransaction = (transactionId) => {
    setSelectedTransaction(transactionId);
    setIsViewDetailTransactionModalOpen(true);
  }

  const dispatch = useAppDispatch();
  const transactions = useAppSelector(
    ({ transaction }) => transaction?.transactions
  );

  const [transaction, setTransaction] = React.useState({
    PageIndex: 1,
    IsAsc: true,
    SearchValue: "",
  });

  const [searchText, setSearchText] = useState("");
  const [searchedColumn, setSearchedColumn] = useState("");
  const searchInput = useRef(null);
  const handleSearch = (selectedKeys, confirm, dataIndex) => {
    confirm();
    setSearchText(selectedKeys[0]);
    setSearchedColumn(dataIndex);
  };
  const handleReset = (clearFilters) => {
    clearFilters();
    setSearchText("");
  };
  const getColumnSearchProps = (dataIndex) => ({
    filterDropdown: ({
      setSelectedKeys,
      selectedKeys,
      confirm,
      clearFilters,
      close,
    }) => (
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
          onChange={(e) =>
            setSelectedKeys(e.target.value ? [e.target?.value] : [])
          }
          onPressEnter={() => handleSearch(selectedKeys, confirm, dataIndex)}
          style={{
            marginBottom: 8,
            display: "block",
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
          color: filtered ? "#1677ff" : undefined,
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
            backgroundColor: "#ffc069",
            padding: 0,
          }}
          searchWords={[searchText]}
          autoEscape
          textToHighlight={text ? text.toString() : ""}
        />
      ) : (
        text
      ),
  });

  const columns = [
    {
      title: "Transaction Id",
      dataIndex: "transactionId",
      width: "5%",
      align: "center",
    },
    {
      title: "Status",
      dataIndex: "transactionStatus",
      width: "10%",
    },
    {
      title: "Action",
      dataIndex: "",
      align: "center",
      key: "x",
      width: "20%",
      render: (text, record, index) => {
        return (
          <>
            {/*
            onClick={() => handleShowTransaction(record, index)}
          */}
            <Button
              onClick={() => handleViewDetailTransaction(record.transactionId)}
              type="primary"
              className="blue"
            >
              View
            </Button>
          </>
        );
      },
    },
  ];

  const handleShowTransaction = (record) => {
    console.log("record", record);
    const listInterior = record.listInterior;
    Modal.info({
      title: "Transaction Details",
      content: (
        <>
          <span>
            <span
              style={{
                fontWeight: "bold",
                marginTop: "10px;",
                display: "inline-block",
              }}
            >
              Transaction Id:
            </span>
            {record.transactionId !== null && (
              <span>{" " + record.transactionId}</span>
            )}
          </span>
          <br />
          <span>
            <span style={{ fontWeight: "bold" }}>Response of Staff:</span>
            <span>{" " + record.responseOfStaff}</span>
          </span>
          <br />
          <span>
            <span
              style={{
                fontWeight: "bold",
                marginTop: "10px;",
                display: "inline-block",
              }}
            >
              List Interior Of Transaction:
            </span>
            <span>
              <ul>
                {listInterior?.map((item) => (
                  <li key={item.interiorId}>
                    <a href={`/interior/${item.interiorId}`}>
                      Item {listInterior.indexOf(item.interiorId) + 2}
                    </a>
                  </li>
                ))}
              </ul>
            </span>
          </span>
          <br />
          <span>
            <span style={{ fontWeight: "bold" }}>Image :</span>
            <span>{" " + record.responseOfStaff}</span>
          </span>
          <br />
        </>
      ),
      footer: (_, { OkBtn, CancelBtn }) => (
        <>
          <CancelBtn />
          <OkBtn />
        </>
      ),
    });
  };
  React.useEffect(() => {
    dispatch(actionGetMyTransactions(transaction));
  }, []);

  return (
    <>
      <div
        className="w-[100vw] flex justify-center items-center"
        style={{
          background: "#343f4024",
          paddingTop: "200px"
        }}
      >
        <div className="w-[70vw]">
          <PageHeader message={"Transaction History"} />

          <Table
            columns={columns}
            dataSource={transactions ? transactions : []}
            pagination={{ pageSize: 5 }}
          />
          <TransactionDetailModal selectedTransaction={selectedTransaction}
                isModalOpen={isViewDetailTransactionModalOpen}
                setIsModalOpen={setIsViewDetailTransactionModalOpen}/>
        </div>
      </div>
    </>
  );
};

export default TransactionHistory;
