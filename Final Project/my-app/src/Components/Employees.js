import React from 'react';
import {Admin,Resource} from 'react-admin';
import "../Styles/Admin.css"
import LayoutForAdmin from "../Providers/LayoutForAdmin";
import EmployeeList from "./CRUD/Employee/EmployeeList";
import EmployeeCreate from "./CRUD/Employee/EmployeeCreate";
import EmployeeEdit from "./CRUD/Employee/EmployeeEdit";
import CombineProvider from "../Providers/CombineProvider";


const Employees = (props) => {

    if (props.role === "chief" && !props.blocked) {
        return (
            <Admin layout={LayoutForAdmin} dataProvider={CombineProvider}>
                <Resource name='employee' list={EmployeeList} create={EmployeeCreate} edit={EmployeeEdit} />
            </Admin>
        );
    } else if (props.blocked) {
        return (
            <h1>You are blocked</h1>
        );
    }

    return (
            <h1>You do not have permission</h1>
    );
};

export default Employees;