import React, {useEffect, useState} from 'react';
import {Edit, SimpleForm, TextInput, SelectInput} from "react-admin";
import axios from "axios";


const CarriageEdit = (props) => {

    const [trains, setTrains] = useState([]);
    const [employees, setEmployees] = useState([]);

     useEffect(() => {
        async function fetchMyAPI() {
            await axios.get('https://localhost:5001/lists/employeeslist')
                .then(function (response) {
                    setEmployees( response.data);
                });
            await axios.get('https://localhost:5001/lists/trainslist')
                .then(function (response) {
                    setTrains( response.data);
                });
        }

        fetchMyAPI()
    }, []);


    return (
        <Edit title ='Edit User' {...props}>
            <SimpleForm>
                <TextInput disabled source='id' />
                <SelectInput  source='trainId' choices={trains}/>
                <SelectInput  source='guide1Id' choices={employees}/>
                <SelectInput  source='guide2Id' choices={employees}/>
                <SelectInput source="type" choices={[
                    { id: 'econom', name: 'Econom' },
                    { id: 'business', name: 'Business' },
                ]} />
            </SimpleForm>
        </Edit>
    )
}

export default CarriageEdit;