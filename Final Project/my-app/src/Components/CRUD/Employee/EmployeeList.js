import React from 'react';
import {
    List,
    Datagrid,
    TextField,
    DateField,
    ImageField,
    EditButton, DeleteButton
} from "react-admin";

const EmployeeList = (props) => {
    return(
        <List {...props}
              bulkActionButtons={false}>
            <Datagrid>
                <TextField source='id'/>
                <TextField source='firstName'/>
                <TextField source='lastName'/>
                <TextField source='patronymic'/>
                <DateField source='birthday'/>
                <TextField source='phones'/>
                <TextField source='homeAdress'/>
                <TextField source='position'/>
                <ImageField source="photo"/>
                <EditButton basepath='/employee' />
                <DeleteButton basepath='/employee' />
            </Datagrid>
        </List>
    );
}

export default EmployeeList;