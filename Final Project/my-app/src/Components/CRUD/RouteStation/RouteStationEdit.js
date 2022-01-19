import React, {useEffect, useState} from 'react';
import {Edit, SimpleForm, NumberInput, SelectInput } from "react-admin";
import axios from "axios";
import TimeInput from "../../TimeInput";

const RouteStationEdit = (props) => {
    const [stations, setStations] = useState([]);
    const [routes,setRoutes] = useState([])
    useEffect(() => {
        async function fetchMyAPI() {
            await axios.get('https://localhost:5001/lists/stationslist')
                .then(function (response) {
                    setStations( response.data);
                });
            await axios.get('https://localhost:5001/lists/routeslist')
                .then(function (response) {
                    setRoutes( response.data);
                });
        }
        fetchMyAPI();
    }, []);

    return (
        <Edit title ='Edit RouteStation' {...props}>
            <SimpleForm>
                <SelectInput source="route" choices ={routes}  />
                <SelectInput source="stationId" choices ={stations}  />
                <NumberInput source="order"/>
                <TimeInput source='arrivalTime'/>
                <TimeInput source='departureTime'/>

            </SimpleForm>
        </Edit>
    )
}

export default RouteStationEdit ;