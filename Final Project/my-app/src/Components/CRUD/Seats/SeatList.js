import React from 'react';
import {List, Datagrid, TextField,EditButton,DeleteButton } from "react-admin";

const SeatList = (props) => {
    return(
        <List {...props}
              bulkActionButtons={false}>
            <Datagrid>
                {/*<TextField source='id'/>*/}
                <TextField source='carriage'/>
                <TextField source='seatNo'/>
                <TextField source='status'/>
                <EditButton basepath='/seat' />
                <DeleteButton basepath='/seat' />
            </Datagrid>
        </List>
    );
}

export default SeatList;