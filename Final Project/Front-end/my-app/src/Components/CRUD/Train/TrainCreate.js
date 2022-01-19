import React, {useEffect, useState} from "react";
import { Create, SelectInput, SimpleForm, TextInput,BooleanInput, NullableBooleanInput} from "react-admin";
import axios from "axios";

function TrainCreate(props){
    const [route, setRoute] = useState([]);
    const [employees, setEmployees] = useState([]);

    useEffect(() => {
        async function fetchMyAPI() {
            await axios.get('https://localhost:5001/data/employeeslist')
                .then(function (response) {
                    setEmployees( response.data);
                });
            await axios.get('https://localhost:5001/data/routeslist')
                .then(function (response) {
                    setRoute( response.data);
                });
        }
        fetchMyAPI()
    }, []);

    return (
        <Create title ='Create Train' {...props}>
            <SimpleForm>
                <TextInput disabled source='id'/>
                <SelectInput source='routeId' choices={route} />
                <SelectInput source='driverId' choices={employees}/>
                <SelectInput source='driverAssistId' choices={employees}/>
                <SelectInput source='trainmasterId' choices={employees}/>
                <SelectInput source='type'  choices={[
                    { id: 'standart', name: 'Standart' },
                    { id: 'express', name: 'Express' },
                ]}/>
                <BooleanInput source='everyDay'/>
                <NullableBooleanInput source='evenDays'/>
                <TextInput source='days'/>
            </SimpleForm>
        </Create>
    )
}

export default  TrainCreate;