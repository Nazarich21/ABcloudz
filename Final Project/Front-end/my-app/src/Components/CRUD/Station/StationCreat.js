import React from 'react';
import {Create, SimpleForm, TextInput, useNotify} from "react-admin";

const StationCreat = (props) => {

    const notify = useNotify();
    const onFailure = (error) => {

        if (error.status === 409) {
            notify('Such station already exists');
        }
    };

    return (
        <Create title ='Create Station' {...props} onFailure = {onFailure}>
            <SimpleForm>
                <TextInput source='id'/>
                <TextInput source='name'/>
            </SimpleForm>
        </Create>
    );
};

export default StationCreat;