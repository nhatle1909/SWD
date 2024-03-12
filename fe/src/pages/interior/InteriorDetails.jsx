import { useParams } from 'react-router-dom'
import { useAppDispatch, useAppSelector } from '@/store'
import { useEffect } from 'react'
import { linkImg } from '@/utils/common';
import { actionGetDetailInterior } from '../../store/interior/action';
const InteriorDetails = () => {
    const dispatch = useAppDispatch();
    const interior = useAppSelector(({ interiors }) => interiors.interior)
    const { interiorId } = useParams();

    console.log('check interior::', interior)
    useEffect(() => {
        dispatch(actionGetDetailInterior(interiorId))
    }, [])
    return (
        <>
            <section className="home-slider js-fullheight owl-carousel">
                <div className="slider-item js-fullheight" style={{ backgroundImage: `url(${linkImg('interior.jpg')})` }}>
                    <div className="overlay"></div>
                    <div className="container">
                        <div className="row no-gutters slider-text js-fullheight align-items-center justify-content-end" data-scrollax-parent="true">
                            <div className="col-md-7 text ftco-animate" data-scrollax=" properties: { translateY: '70%' }">
                                <h1 className="mb-3 mt-5 bread">Interior Detail</h1>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
            <br />
            <div className="container">
                <div className="row">
                    <div className="col-md-12 ftco-animate">
                        <div className="blog-entry">
                            <img src={`data:image/jpeg;base64,${interior?.image}`} alt={interior?.interiorName} className="img-fluid" />
                            <div className="text py-4">
                                <h2 className="heading mb-3"><strong>{interior?.interiorName}</strong></h2>
                                <p>{interior?.description}</p>
                                <div style={{ display: "flex", justifyContent: "center", alignItems: "center"}}>
                                    <div className="mt-4 p-3 w-50 " style={{ backgroundColor: "#f8f9fa", borderRadius: "10px", boxShadow: "0 4px 6px rgba(0, 0, 0, 0.1)" }}>
                                        <h5 className="mb-3">Details</h5>
                                        <div className="d-flex justify-content-between">
                                            <div>Created at:</div>
                                            <div><strong>{new Date(interior?.createdAt).toLocaleDateString()}</strong></div>
                                        </div>
                                        <div className="d-flex justify-content-between">
                                            <div>Price:</div>
                                            <div><strong>{interior?.price} VND</strong></div>
                                        </div>
                                        <div className="d-flex justify-content-between">
                                            <div>Quantity:</div>
                                            <div><strong>{interior?.quantity}</strong></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </>
    )
}

export default InteriorDetails;