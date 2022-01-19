import React from 'react';
import {
    Create,
    SimpleForm,
    TextInput,
    DateInput,
} from "react-admin";

const EmployeeCreate = (props) => {

    return (
        <Create title ='Create User' {...props}>
            <SimpleForm>
                <TextInput source='firstName'/>
                <TextInput source='lastName'/>
                <TextInput source='patronymic'/>
                <DateInput source='birthday'/>
                <TextInput source='homeAdress'/>
                <TextInput source='phones'/>
                <TextInput source='position'/>
                <TextInput source="photo"/>

            </SimpleForm>
        </Create>
    );
};

export default EmployeeCreate