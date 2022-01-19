import React, {useEffect, useState} from 'react';
import {Edit, SimpleForm, TextInput, SelectInput} from "react-admin";
import TimeInput from "../../TimeInput";

const RouteEdit = (props) =>{
    const [stations, setStations] = useState([]);
    useEffect(() => {
        async function fetchData() {
        await fetch('https://localhost:5001/station')
            .then(response => response.json())
            .then(data => setStations(data));
        ;}
        fetchData();
    }, []);

    return (
        <Edit title ='Edit Route' {...props}>
            <SimpleForm>
                <TextInput disabled source='id' />
                <SelectInput source="firstStationId" choices ={stations}  />
                <TimeInput source="departureTime" label="Departure time" options={{ format: 'HH:mm' }} />
                <TimeInput  source="arrivalTime" label="Arrival time" options={{ format: 'HH:mm' }} />
                <SelectInput source="lastStationId" choices ={stations} />
            </SimpleForm>
        </Edit>
    )
}

export default RouteEdit ;