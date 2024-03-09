import PageHeader from "../../components/PageHeader";
import { useState } from "react";
import ListContract from "../../components/contract/ListContract";

const Contracts = () => {
    const [isModalOpen, setIsModalOpen] = useState(false);


    return (
        <>
            <div className="h-[90vh] w-[100vw] flex justify-center items-center" style={{
                background: '#343f4024'
            }}>
                <div className="h-[65vh] w-[70vw]">
                    <PageHeader message={"Manage Contract"} />
                    <ListContract />
                </div>
            </div >
        </>
    )
}

export default Contracts;