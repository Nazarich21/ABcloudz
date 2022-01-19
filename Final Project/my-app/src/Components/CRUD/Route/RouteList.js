import React from 'react';
import {List, Datagrid, TextField,EditButton,DeleteButton} from "react-admin";

const RouteList = (props) => {
    return(
        <List {...props}
              bulkActionButtons={false}>
            <Datagrid>
                <TextField source='id'/>
                <TextField source='firstStationId'/>
                <TextField source="departureTime" showTime />
                <TextField source="arrivalTime" showTime />
                <TextField source='lastStationId'/>
                <EditButton basepath='/route' />
                <DeleteButton basepath='/route' />
            </Datagrid>
        </List>
    );
}

export default RouteList;