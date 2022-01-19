import React, {useEffect, useState} from 'react';
import {Create,
    SimpleForm,
    SelectInput,
    NumberInput
} from "react-admin";
import axios from "axios";
import TimeInput from "../../TimeInput";

const RouteStationCreate = (props) => {
    const [stations, setStations] = useState([]);
    const [routes,setRoutes] = useState([])

    useEffect(() => {
        async function fetchData() {
            await axios.get('https://localhost:5001/data/stationslist')
                .then(function (response) {
                    setStations( response.data);
                });
            await axios.get('https://localhost:5001/data/routeslist')
                .then(function (response) {
                    setRoutes( response.data);
                });}
        fetchData();
    }, []);

    return (
        <Create title ='Edit RouteStation' {...props}>
            <SimpleForm>
                <SelectInput source="route" choices ={routes}  />
                <SelectInput source="station" choices ={stations}  />
                <NumberInput source="order"/>
                <TimeInput source='departureTime'/>
                <TimeInput source='arrivalTime'/>
            </SimpleForm>
        </Create>
    )
}

export default RouteStationCreate;