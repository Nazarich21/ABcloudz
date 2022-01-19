import React, {useEffect, useState} from 'react';
import {Create, SimpleForm, TextInput, SelectInput, useNotify} from "react-admin";
import axios from "axios";


const SeatCreate = (props) => {

    const [carriage, setCarriage] = useState([]);
    const notify = useNotify();
    const onFailure = (error) => {

        if (error.status === 409) {
            notify('Such seat already exists');
        }
    };
    useEffect(() => {
        async function fetchMyAPI() {
            await axios.get('https://localhost:5001/data/carriageslist')
                .then(function (response) {
                    setCarriage( response.data);
                });
        }
        fetchMyAPI()
    }, []);

    return (
        <Create title ='Create Seat' {...props}  onFailure = {onFailure}>
            <SimpleForm>
                <SelectInput source='carriage' choices={carriage}/>
                <TextInput source='seatNo'/>
                <SelectInput source='statusId'  choices={[
                    { id: 'standart', name: 'Standart' },
                    { id: 'business', name: 'Business' },
                ]}/>
            </SimpleForm>
        </Create>
    );
};

export default SeatCreate;