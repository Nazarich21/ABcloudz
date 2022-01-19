import React from 'react';
import {List, Datagrid, TextField,EditButton,DeleteButton } from "react-admin";

const UserList = (props) => {
    return(
        <List {...props}
              bulkActionButtons={false}>
            <Datagrid>
                <TextField source='id'/>
                <TextField source='email'/>
                <TextField source='password'/>
                <TextField source='role'/>
                <TextField label="Blocked" source='blocked'/>
                <EditButton basepath='/user' />
                <DeleteButton basepath='/user' />
            </Datagrid>
        </List>
    );
}

export default UserList;