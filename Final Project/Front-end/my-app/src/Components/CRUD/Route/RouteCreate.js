import React, {useEffect, useState} from 'react';
import {Create, SimpleForm, SelectInput} from "react-admin";
import TimeInput from "../../TimeInput";

const RouteCreate = (props) =>{
    const [stations, setStations] = useState([]);

    useEffect(() => {
        (
            async() => {
                fetch('https://localhost:5001/station')
                    .then(response => response.json())
                    .then(data => setStations(data));
            }
        )();

    },[]);

    return (
        <Create title ='Create Route' {...props}>
            <SimpleForm>
                <SelectInput source="firstStationId" choices ={stations}  />
                <TimeInput source="departureTime" label="Departure time" options={{ format: 'HH:mm' }} />
                <TimeInput source="arrivalTime" label="Arrival time" options={{ format: 'HH:mm' }} />
                <SelectInput source="lastStationId" choices ={stations} />
            </SimpleForm>
        </Create>
    )
}

export default RouteCreate;