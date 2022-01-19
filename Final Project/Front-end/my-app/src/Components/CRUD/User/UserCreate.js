import React from 'react';
import {Create, SimpleForm, TextInput, SelectInput, BooleanInput,email} from "react-admin";

const UserCreate = (props) =>{

    let validateEmail = email();

    return (
       <Create title ='Create User' {...props}>
           <SimpleForm>
               <TextInput label="Email" source="email" validate={validateEmail}/>
               <TextInput label ="Password" source="password"/>
               <BooleanInput label="Blocked" source="status" />
               <SelectInput source="Role" choices={[
                   { id: 'admin', name: 'Admin' },
                   { id: 'manager', name: 'Manager' },
                   { id: 'chief', name: 'Chief' },
                   { id: 'user', name: 'User' },
               ]} />
           </SimpleForm>
       </Create>
    );
};

export default UserCreate;