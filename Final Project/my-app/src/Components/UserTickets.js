import React, {useEffect, useState} from "react";
import BootstrapTable from "react-bootstrap-table-next";
import {userTickets} from "../API";


const UserTickets = (props) => {
    const[tickets,setTickets]= useState([]);
    const columnsTickets = [{
        dataField: 'ticketid',
        text: 'Ticket'
    },{
        dataField: 'trainId',
        text: 'Train'
    },{
        dataField: 'carriageId',
        text: 'Carriage'
    },{
        dataField: 'seat',
        text: 'Seat'
    },{
        dataField: 'date',
        text: 'Departure Date'
    },{
        dataField: 'status',
        text: 'Status'
    }];

    useEffect(() => {
        userTickets(props.id).then(res => {
            setTickets(res.data);
        });

    }, [props.id]);

    if (props.role === "user" && !props.blocked) {
        return (
            <BootstrapTable keyField='ticketid' data={ tickets } columns={ columnsTickets }  noDataIndication={ 'You do not have tickets' }/>
        );
    } else if (props.blocked) {
        return (
            <h1>You are blocked</h1>
        );
    }
    return (
        <h1>You do not have permission</h1>
    );
}
export default UserTickets;