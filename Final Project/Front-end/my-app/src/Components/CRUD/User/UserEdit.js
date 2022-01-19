import React from 'react';
import {Edit, SimpleForm, TextInput, SelectInput,BooleanInput } from "react-admin";

const UserEdit = (props) => {

    return (
        <Edit title ='Edit User' {...props}>
            <SimpleForm>
                <TextInput disabled source='id' />
                <TextInput label="Email" source="email"/>
                <TextInput label="Password" source='password'/>
                <BooleanInput label="Blocked" source="status" />
                <SelectInput source="role" choices={[
                    { id: 'admin', name: 'Admin' },
                    { id: 'manager', name: 'Manager' },
                    { id: 'chief', name: 'Chief' },
                    { id: 'user', name: 'User' },
                ]} />
            </SimpleForm>
        </Edit>
    )
}

export default UserEdit;