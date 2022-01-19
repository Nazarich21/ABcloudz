import React, {useEffect, useState} from "react";
import Form from 'react-bootstrap/Form'
import BootstrapTable from 'react-bootstrap-table-next';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogTitle from '@mui/material/DialogTitle';
import {addTicket, findSeatsPurchasing, findTrainsPurchasing, stationList} from "../API";

const User = (props) => {
    const [stations, setStations] = useState([]);
    const [firstStation, setFirstStation] = useState('KH');
    const [secondStation, setSecondStation] = useState('KV');
    const [departureDate, setDepartureDate] = useState('');
    const [trains, setTrains] = useState([]);
    const [seats, setSeats] = useState([]);
    const [open, setOpen] = React.useState(false);
    const [selectedTickets, setSelectedTickets] = useState([]);
    const [price,setPrice] = useState(0);
    const[documentType,setDocumentType] = useState('');
    const[documentNumber, setDocumentNumber] = useState('');
    const[firstName, setFirstName] = useState('');
    const[lastName, setLastName] = useState('');
    const[ticketStatus,setTicketStatus] = useState('')

    const columnsTrains = [{
        dataField: 'trainId',
        text: 'Train '
    }, {
        dataField: 'departureTime',
        text: 'Departure Time'
    },{
        dataField: 'arrivalTime',
        text: 'Arrival Time'
    }, {
        dataField: 'routeTime',
        text: 'Route Time'
    },];

    const columnsSeats = [{
        dataField: 'train',
        text: 'Train '
    }, {
        dataField: 'carriage',
        text: 'Carriage'
    }, {
        dataField: 'seat',
        text: 'Seat'
    },{
        dataField: 'carriageType',
        text: 'Carriage Type'
    }, {
        dataField: 'price',
        text: 'Price'
    },];

    useEffect(() => {
        stationList().then(res => {
            setStations(res.data);
        });

    }, []);

    function SeatCompKey({trainId,carriageId,seat, ...item}) {
        return {
            ...item,
            id: `${trainId}` + carriageId + seat,
            train: trainId,
            carriage: carriageId,
            seat: seat
        };
    };

    const rowEvents = {
        onClick: (e, row, rowIndex) => {
            findSeatsPurchasing(row.trainId, firstStation,secondStation, departureDate).then(res => {
                let resultData =res.data.map(seat => SeatCompKey(seat))
                setSeats( resultData);
            });
        }
    };

    const selectRowSeats = {
        mode: 'checkbox',
        onSelect: (row, isSelect, rowIndex, e) => {
            if (isSelect === true) {
                selectedTickets.push(row);
            } else if (isSelect === false){
                for(let i=0; i<selectedTickets.length; i++){
                    let temp = selectedTickets[i];
                    if(temp.id === row.id){
                        selectedTickets.splice(i, 1);
                        break;
                    }
                }
            }
            let sum = 0;
            for(let i = 0; i<selectedTickets.length; i++){
               sum = sum  + selectedTickets[i].price
            }
            setPrice(sum);
        }
    };

    const handleClose = () => {
        setOpen(false);
    };

    function findTrains() {
        findTrainsPurchasing(firstStation, secondStation, departureDate).then(res => {
                setTrains( res.data);
            });
        setPrice(0);
        setSelectedTickets([]);
        setSeats([]);
    }

    function bookClick() {
        let currentdate =  Date.now();
        let elapsed = new Date(departureDate) - currentdate;
        if (Math.ceil(elapsed / (1000 * 60 * 60 * 24)) < 3 ){
            alert("Сan be booked in 3 days before train departure")
        } else {
            setTicketStatus('booked');
            setOpen(true);
        }
    }

    function buyClick() {
        setTicketStatus('bought')
        setOpen(true);
    }

    async function addTickets() {
        setOpen(false);
        for(let a = 0; a < selectedTickets.length; a++)
        {
            console.log(selectedTickets[a]);
            selectedTickets[a].trainId =  selectedTickets[a].train;
            delete selectedTickets[a].train;
            selectedTickets[a].carriageId = selectedTickets[a].carriage;
            delete selectedTickets[a].carriage;
            console.log(selectedTickets[a]);
            selectedTickets[a].departureDate = departureDate;
            selectedTickets[a].departureStationId = firstStation;
            selectedTickets[a].arrivalStationId = secondStation;
            let date1 = new Date();
            selectedTickets[a].operationDate = date1.toJSON();
            selectedTickets[a].userId = props.id;
            selectedTickets[a].custom = props.id;
            selectedTickets[a].documentType = documentType;
            selectedTickets[a].documentNumber = documentNumber;
            selectedTickets[a].firstName = firstName;
            selectedTickets[a].lastName = lastName;
            selectedTickets[a].status = ticketStatus;
            console.log(selectedTickets[a]);
            addTicket(selectedTickets[a]);
        }
    }

    if (props.role === "user" && !props.blocked) {
        return (
            <div className={"p-3"}>
                <div className={"d-flex mb-3 justify-content-around"}>
                    <Form.Select className={"w-20"} aria-label="Select departure station"  onChange={e => {
                        setFirstStation(e.target.value);
                    }}>
                        {stations.map((e, key) => {
                            return <option key={key} value={e.id}>{e.name}</option>;
                        })};
                    </Form.Select>

                    <Form.Select className={"w-20"} aria-label="Select departure station"  onChange={e => {
                        setSecondStation(e.target.value);
                    }}>
                        {stations.map((e, key) => {
                            return <option key={key} value={e.id}>{e.name}</option>;
                        })};
                    </Form.Select>

                    <Form.Control className={"w-20"} type="date" onChange={e => {
                        console.log("e.target.value", e.target.value);
                        setDepartureDate(e.target.value);
                    }} />

                    <button onClick={findTrains}  className="btn btn-primary btn-lg w-20"
                    >Find</button>
                </div>
                <BootstrapTable keyField='trainId' data={ trains } columns={ columnsTrains } rowEvents={ rowEvents } noDataIndication={ 'no results found' }/>
                <BootstrapTable keyField='id' data={ seats } columns={ columnsSeats } selectRow={ selectRowSeats } noDataIndication={ 'no results found' } />
                <div className={"d-flex mb-3 justify-content-between"}>

                    <h2>Price: {price}</h2>
                    <div>
                        <button onClick={bookClick}  className="btn btn-primary btn-lg"
                        >Book</button>
                        <button onClick={buyClick}  className="btn btn-primary btn-lg ms-5"
                        >Buy</button>
                    </div>
                </div>

                <Dialog
                    open={open}
                    aria-labelledby="responsive-dialog-title"
                >
                    <DialogTitle id="responsive-dialog-title" className={"w-100"}>
                        {"Enter personal data for creating tickets"}
                    </DialogTitle>
                    <DialogContent>
                        <Form className={"container mt-5 pt-5 d-flex flex-column"}>
                            <Form.Group className="mb-3 ">
                                <Form.Label>Email address</Form.Label>
                                <Form.Control  placeholder="Enter first name"
                                               value={firstName}
                                               onChange={e => setFirstName(e.target.value)} />
                            </Form.Group>

                            <Form.Group className="mb-3 " >
                                <Form.Label>Last Name</Form.Label>
                                <Form.Control placeholder="Enter second name"
                                              value={lastName}
                                              onChange={e => setLastName(e.target.value)}/>
                            </Form.Group>

                            <Form.Group className="mb-3" >
                                <Form.Label>Document Type</Form.Label>
                                <Form.Control placeholder="Enter document type"
                                              value={documentType}
                                              onChange={e => setDocumentType(e.target.value)}/>
                            </Form.Group>

                            <Form.Group className="mb-3" >
                                <Form.Label>Document Number</Form.Label>
                                <Form.Control placeholder="Enter document number"
                                              value={documentNumber}
                                              onChange={e => setDocumentNumber(e.target.value)}/>
                            </Form.Group>
                        </Form>

                    </DialogContent>
                    <DialogActions>
                        <Button  onClick={handleClose}>
                            Cancel
                        </Button>
                        <Button onClick={addTickets} >
                            Commit
                        </Button>
                    </DialogActions>
                </Dialog>
            </div>
        );
    } else if (props.blocked) {
        return(
            <h1>You are blocked</h1>
        );
    }
    return(
        <h1>You do not have permission</h1>
    );
}

export default User;


