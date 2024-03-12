import {useParams} from 'react-router-dom'
import {useAppDispatch, useAppSelector} from '@/store'
const InteriorDetails = () => {
    const dispatch = useAppDispatch();
    const interior = useAppSelector(({interiors}) => interiors.interior)
    const {interiorId} = useParams();

    console.log('check interior::', interior)
    useEffect(() => {
        dispatch(actionGetDetailInterio(interiorId))
    }, [])
    return (
        <>
        this is detail interior
        </>
    )
}

export default InteriorDetails;