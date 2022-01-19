import React, {useEffect, useState} from "react";
import Form from "react-bootstrap/Form";
import 'bootstrap/dist/css/bootstrap.min.css';
import BootstrapTable from "react-bootstrap-table-next";
import {findTrains, findTrainTimetable, stationList, trainList} from "../API";

const Home = () => {
    const [trains, setTrains] = useState([]);
    const [stations, setStations] = useState([]);
    const [trainsListTimetable, setTrainsListTimetable] = useState([]);
    const [stationsListPassing, setStationsListPassing] = useState([]);
    const columnsTrainsPassing = [{
        dataField: 'id',
        text: 'Train '
    },{
        dataField: 'type',
        text: 'Train Type'
    },{
        dataField: 'arrivalTime',
        text: 'Arrival Time'
    },{
        dataField: 'departureTime',
        text: 'Departure Time'
    }];
    const columnsTrainTimetable = [{
        dataField: 'name',
        text: 'Station '
    },{
        dataField: 'arrivalTime',
        text: 'Arrival Time'
    },{
        dataField: 'departureTime',
        text: 'Departure Time'
    }];

    useEffect(() => {
       stationList().then(res => {
            setStations(res.data);
        });

       trainList().then(res => {
            setTrains(res.data);
        });
    }, []);

    const FindTrainTimetable = (e) => {
        findTrainTimetable(e).then(res => {
            setTrainsListTimetable(res.data);
        });
    }

    const FindTrains = (e) => {
       findTrains(e).then(res => {
           setStationsListPassing(res.data);
       });
    }

    return (
        <div className={"p-3"}>
                <h2 className={"align-self-start"}>Train Timetable</h2>
                <Form.Select className={"w-25 mb-3"} aria-label="Select station"
                             onChange={e => {FindTrainTimetable(e.target.value);
                }} >
                    {trains.map((e, key) => {
                        return <option key={key} value={e.id}>{e.name}</option>;
                    })};
                </Form.Select>
            <BootstrapTable keyField='name' data={ trainsListTimetable } columns={ columnsTrainTimetable } className={"w-100 p-3"}  noDataIndication={ 'no results found' }/>

            <h2 className={"align-self-start"}>Passing Trains</h2>
            <Form.Select className={"w-25 mb-3"} aria-label="Select station"
                         onChange={e => {
                             FindTrains(e.target.value);}}>
                {stations.map((e, key) => {
                    return <option key={key} value={e.id}>{e.name}</option>;
                })};
            </Form.Select>
            <BootstrapTable keyField='id' data={ stationsListPassing } columns={ columnsTrainsPassing }  noDataIndication={ 'no results found' }/>
        </div>
    );
}

export default Home;


