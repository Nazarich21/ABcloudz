import React from 'react';
import {
    Edit,
    SimpleForm,
    TextInput,
    SelectInput,
} from "react-admin";

const SeatEdit = (props) => {
    return (
        <Edit title ='Edit Route' {...props}>
            <SimpleForm>
                <TextInput disabled source='id'/>
                <TextInput disabled source='carriage'/>
                <TextInput disabled source='seatNo'/>
                <SelectInput source='statusId'  choices={[
                    { id: 'standart', name: 'Standart' },
                    { id: 'business', name: 'Business' },
                ]}/>
            </SimpleForm>
        </Edit>
    )
}

export default SeatEdit;