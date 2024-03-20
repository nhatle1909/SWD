import { useAppSelector } from "@/store";
import { Button, Form, Input, Select, Spin, Table } from "antd";
import React, { useContext, useEffect, useRef, useState } from "react";
import { useDispatch } from "react-redux";
import { useNavigate } from "react-router-dom";
import PageHeader from "../../components/PageHeader";
import { actionGetInteriors } from "../../store/interior/action";
import { createRequestAction } from "../../store/request/action";
const CreateRequest = () => {
  const auth = useAppSelector(({ authentication }) => authentication.authUser);
  const navigator = useNavigate();

  const interiors = useAppSelector(({ interior }) => interior.interiors);
  const dispatch = useDispatch();
  const [email, setEmail] = useState(auth?.email);
  const [address, setAddress] = useState("");
  const [phone, setPhone] = useState("");
  const [content, setContent] = useState("");
  const [dataSource, setDataSource] = useState([]);
  const [count, setCount] = useState(0);

  const handleDelete = (key) => {
    const newData = dataSource.filter((item) => item.key !== key);
    setDataSource(newData);
  };

  const onSubmit = () => {
    let selectedInteriors = [];
    for (let i = 0; i < count; i++) {
      let select = document.getElementById("interior-" + i).value;
      console.log("select", select);

      let quantity = document.getElementById("quantity-" + i).value;
      console.log("quantity", quantity);

      selectedInteriors.push({
        interiorId: select,
        quantity: quantity,
      });

     
      
    }
    dispatch(
      createRequestAction({
        Email: email,
        Address: address,
        Phone: phone,
        Content: content,
        ListInterior: selectedInteriors,
      })
    );
    navigator("/request/history");
  };

  const defaultColumns = [
    {
      title: "name",
      dataIndex: "name",
      width: "50%",
    },
    {
      title: "quantity",
      dataIndex: "quantity",
    },
    {
      title: "action",
      dataIndex: "action",
    },
  ];
  const handleAdd = () => {
    const newData = {
      key: count,
      name: (
        <select id={`interior-${count}`}>
          {interiors?.map((item) => (
            <option value={item.interiorId}>{item.interiorName}</option>
          ))}
        </select>
      ),
      quantity: (
        <>
          <Input
            id={`quantity-${count}`}
            type="number"
            min={0}
            max={1000}
            defaultValue={1}
          />
        </>
      ),
      action: (
        <>
          <Button onClick={() => handleDelete(count)} type="primary">
            Delete
          </Button>
        </>
      ),
    };
    setDataSource([...dataSource, newData]);
    setCount(count + 1);
  };
  const handleSave = (row) => {
    const newData = [...dataSource];
    const index = newData.findIndex((item) => row.key === item.key);
    const item = newData[index];
    newData.splice(index, 1, {
      ...item,
      ...row,
    });
    setDataSource(newData);
  };
  const components = {
    body: {
      row: EditableRow,
      cell: EditableCell,
    },
  };
  const columns = defaultColumns.map((col) => {
    if (!col.editable) {
      return col;
    }
    return {
      ...col,
      onCell: (record) => ({
        record,
        editable: col.editable,
        dataIndex: col.dataIndex,
        title: col.title,
        handleSave,
      }),
    };
  });

  const handleOk = () => {
    // dispatch(
    //   createRequestAction({
    //     Email: email,
    //     Address: address,
    //     Phone: phone,
    //     Content: content,
    //     Interior: interiorId,
    //   })
    // );
    // navigator("/request/history");
  };

  React.useEffect(() => {
    dispatch(
      actionGetInteriors({
        PageIndex: 1,
        IsAsc: true,
        SearchValue: "",
      })
    );
  }, []);

  // console.log("interiors", interiors);
  return (
    <>
      <div
        className=" w-[100vw] pt-5 flex justify-center items-center"
        style={{
          background: "#343f4024",
        }}
      >
        <div className="container min-h-screen w-100 py-5">
          {interiors ? (
            <>
              <PageHeader message={"Create New Request"} />
              <Form layout="vertical" name="basic" autoComplete="off">
                <Form.Item
                  label="Email"
                  name="email"
                  rules={[
                    {
                      required: true,
                      message: "Please input your email!",
                    },
                  ]}
                >
                  <span>{auth?.email}</span>
                </Form.Item>
                <Form.Item
                  label="Phone Number"
                  name="phoneNumber"
                  rules={[
                    {
                      required: true,

                      message: "Please input your phone number!",
                    },
                  ]}
                >
                  <Input
                    placeholder="Enter phone"
                    maxLength={10}
                    onChange={(e) => setPhone(e.target.value)}
                  />
                </Form.Item>
                <Form.Item
                  label="Address"
                  name="address"
                  rules={[
                    {
                      required: true,
                      message: "Please input your address!",
                    },
                  ]}
                >
                  <Input
                    placeholder="Enter address"
                    onChange={(e) => setAddress(e.target.value)}
                  />
                </Form.Item>
                <Form.Item
                  label="Content"
                  name="content"
                  rules={[
                    {
                      required: true,
                      message: "Please input your content!",
                    },
                  ]}
                >
                  <Input
                    placeholder="Enter content"
                    onChange={(e) => setContent(e.target.value)}
                  />
                </Form.Item>

                <div>
                  <Button
                    onClick={handleAdd}
                    type="primary"
                    style={{
                      marginBottom: 16,
                    }}
                  >
                    Add a interior
                  </Button>
                  <Table
                    components={components}
                    rowClassName={() => "editable-row"}
                    bordered
                    dataSource={dataSource}
                    columns={columns}
                    pagination={false}
                  />
                </div>

                <Form.Item style={{ marginTop: 20, width: 200 }}>
                  <Button
                    style={{ width: 200 }}
                    onClick={() => onSubmit()}
                    type="primary"
                    htmlType="button"
                  >
                    Submit
                  </Button>
                </Form.Item>
              </Form>
            </>
          ) : (
            <>
              <span className="d-flex align-items-center">
                <Spin />{" "}
                <span className="fw-bold d-inline-block">
                  Loading interiors...
                </span>
              </span>
            </>
          )}
        </div>
      </div>
    </>
  );
};

export default CreateRequest;

const EditableContext = React.createContext(null);
const EditableRow = ({ index, ...props }) => {
  const [form] = Form.useForm();
  return (
    <Form form={form} component={false}>
      <EditableContext.Provider value={form}>
        <tr {...props} />
      </EditableContext.Provider>
    </Form>
  );
};
const EditableCell = ({
  title,
  editable,
  children,
  dataIndex,
  record,
  handleSave,
  ...restProps
}) => {
  const [editing, setEditing] = useState(false);
  const inputRef = useRef(null);
  const form = useContext(EditableContext);
  useEffect(() => {
    if (editing) {
      inputRef.current.focus();
    }
  }, [editing]);
  const toggleEdit = () => {
    setEditing(!editing);
    form.setFieldsValue({
      [dataIndex]: record[dataIndex],
    });
  };
  const save = async () => {
    try {
      const values = await form.validateFields();
      toggleEdit();
      handleSave({
        ...record,
        ...values,
      });
    } catch (errInfo) {
      console.log("Save failed:", errInfo);
    }
  };
  let childNode = children;
  if (editable) {
    childNode = editing ? (
      <Form.Item
        style={{
          margin: 0,
        }}
        name={dataIndex}
        rules={[
          {
            required: true,
            message: `${title} is required.`,
          },
        ]}
      >
        <Input ref={inputRef} onPressEnter={save} onBlur={save} />
      </Form.Item>
    ) : (
      <div
        className="editable-cell-value-wrap"
        style={{
          paddingRight: 24,
        }}
        onClick={toggleEdit}
      >
        {children}
      </div>
    );
  }
  return <td {...restProps}>{childNode}</td>;
};
