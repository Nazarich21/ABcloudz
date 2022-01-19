import React from 'react';
import {List, Datagrid, TextField,EditButton,DeleteButton} from "react-admin";

const RouteList = (props) => {
    return(
        <List {...props}
              bulkActionButtons={false}>
            <Datagrid>
                <TextField source='station'/>
                <TextField source='route'/>
                <TextField source='order'/>
                <TextField order='lastStationId'/>
                <TextField source="departureTime" />
                <TextField source="arrivalTime" />
                <EditButton basepath='/train' />
                <DeleteButton basepath='/train' />
            </Datagrid>
        </List>
    );
}

export default RouteList;