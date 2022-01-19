import React from 'react';
import {Admin,Resource} from 'react-admin';
import UserList from "./CRUD/User/UserList";
import UserCreate from "./CRUD/User/UserCreate";
import UserEdit from "./CRUD/User/UserEdit";
import UserDataProvider from "../Providers/UserDataProvider";
import "../Styles/Admin.css"
import LayoutForAdmin from "../Providers/LayoutForAdmin";

const AdminPage = (props) => {

    if (props.role === "admin" && !props.blocked) {
        return(
            <Admin layout={LayoutForAdmin} dataProvider={UserDataProvider}>
                <Resource name='user' list={UserList} create={UserCreate} edit={UserEdit}/>
            </Admin>
        );
    } else if (props.blocked) {
        return(
            <h1>You are blocked</h1>
        );
    }

    return(
        <h1>You do not have permission</h1>
    );

};

export default AdminPage;