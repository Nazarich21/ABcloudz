import React, {useEffect, useState} from 'react';
import Form from "react-bootstrap/Form";
import {getStationStatistic, getTrainStatistic, stationsGet, trainList} from "../API";

const Statistics = (props) => {
    const [beginDate, setBeginDate] = useState('');
    const [endDate, setEndDate] = useState('');
    const [train, setTrain] = useState(0);
    const [trains,setTrains] = useState([]);
    const [station, setStation] = useState('');
    const [stations,setStations] = useState([]);
    const [revenue, setRevenue] = useState(0);
    const [passengersAmount,setPassengersAmount] = useState(0);
    const [arrivalPassengers,setArrivalPassengers] = useState(0);
    const [departuedPassengers,setDepartuedPassengers] = useState(0);

    useEffect(() => {
       stationsGet().then(res => {
            setStations(res.data);
        });

        trainList().then(res => {
            setTrains(res.data);
        });

    }, []);

    const getTrainStatistics = () => {
        getTrainStatistic(beginDate, endDate,train).then(res =>{
            setRevenue(res.data.revenue);
            setPassengersAmount(res.data.amountOfPassengers);
        });
    };


    const getStationStatistics = () => {
        getStationStatistic(beginDate, endDate, station).then(res =>{
            setArrivalPassengers(res.data.amountArrivalPassengers);
            setDepartuedPassengers(res.data.amountDepartPassengers);
        });
    };

    if (props.role === "chief" && !props.blocked) {
        return (
            <div className="m-3">
                <h2 className="mt-3">Train Statistic</h2>
            <div className={"d-flex mb-3 justify-content-around w-100"}>
                <Form.Control className={"w-10"} type="date" onChange={e => {
                    console.log("e.target.value", e.target.value);
                    setBeginDate(e.target.value);
                }} />
                <Form.Control className={"w-20"} type="date" onChange={e => {
                    console.log("e.target.value", e.target.value);
                    setEndDate(e.target.value);
                }} />
                <Form.Select className={"w-20"} aria-label="Select station"
                             onChange={e => {setTrain(e.target.value);
                             }} >
                    {trains.map((e, key) => {
                        return <option key={key} value={e.id}>{e.name}</option>;
                    })};
                </Form.Select>

                <button onClick={ getTrainStatistics}  className="btn btn-primary btn-lg w-20">Find</button>
            </div>

            <div className="d-flex flex-column">
                <h3>Amount of passengers: {passengersAmount}</h3>
                <h3>Revenue: {revenue}</h3>
            </div>
                <div>
                    <Form.Select className="w-20" aria-label="Select departure station"  onChange={e => {
                        setStation(e.target.value);
                    }}>
                        {stations.map((e, key) => {
                            return <option key={key} value={e.id}>{e.name}</option>;
                        })};
                    </Form.Select>
                    <button onClick={getStationStatistics}  className="btn btn-primary btn-lg w-20">Find</button>
                </div>
                <div className="d-flex flex-column">
                    <h3>Amount of arrival passengers: {arrivalPassengers}</h3>
                    <h3>Amount of departured passengers: {departuedPassengers}</h3>
                </div>
            </div>
        );
    }  else if (props.blocked) {
        return (
            <h1>You are blocked</h1>
        );
    }
    return (
        <h1>You do not have permission</h1>
    );
};

export default Statistics;