import React, {useEffect, useState} from 'react';
import {Create, SimpleForm, TextInput, SelectInput} from "react-admin";
import axios from "axios";


function CarriageCreate(props){
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
        <Create title ='Create Carriage' {...props}>
            <SimpleForm>
                <TextInput disabled source='id' />
                <SelectInput  source='trainId' choices={trains} />
                <SelectInput  source='guide1Id' choices={employees}/>
                <SelectInput  source='guide2Id' choices={employees}/>
                <SelectInput source="type" choices={[
                    {id: 'econom', name: 'Econom' },
                    { id: 'business', name: 'Business' },
                ]} />
            </SimpleForm>
        </Create>
    )
}

export default  CarriageCreate;