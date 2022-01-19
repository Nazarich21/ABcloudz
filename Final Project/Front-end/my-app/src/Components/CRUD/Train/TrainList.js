import React from 'react';
import {List, Datagrid, TextField,EditButton,DeleteButton ,BooleanField} from "react-admin";

const TrainList = (props) => {

    return(
        <List {...props}
              bulkActionButtons={false}>
            <Datagrid>
                <TextField source='id'/>
                <TextField source='routeId'/>
                <TextField source='driverId'/>
                <TextField source='driverAssistId'/>
                <TextField source='trainmasterId'/>
                <TextField source='type'/>
                <BooleanField source='everyDay'/>
                <BooleanField source='evenDays'/>
                <TextField source='days'/>
                <EditButton basepath='/route' />
                <DeleteButton basepath='/route' />
            </Datagrid>
        </List>
    );
}

export default TrainList;