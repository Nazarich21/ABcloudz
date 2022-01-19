import { Layout } from 'react-admin';
import Toolbar from '@mui/material/Toolbar';



const LayoutForAdmin = props => <Layout
    {...props}
    appBar={Toolbar}
/>;

export default LayoutForAdmin;