import React from 'react';
import {Edit, SimpleForm, TextInput} from "react-admin";

const StationEdit = (props) => {
    return (
        <Edit title ='Edit User' {...props}>
            <SimpleForm>
                <TextInput disabled source='id' />
                <TextInput source='name' />
            </SimpleForm>
        </Edit>
    )
}

export default StationEdit;