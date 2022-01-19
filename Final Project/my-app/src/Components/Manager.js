import React from 'react';
import {Admin,Resource} from 'react-admin';
import UserList from "./CRUD/User/UserList";
import UserCreate from "./CRUD/User/UserCreate";
import UserEdit from "./CRUD/User/UserEdit";
import "../Styles/Admin.css"
import LayoutForAdmin from "../Providers/LayoutForAdmin";
import EmployeeList from "./CRUD/Employee/EmployeeList";
import EmployeeCreate from "./CRUD/Employee/EmployeeCreate";
import EmployeeEdit from "./CRUD/Employee/EmployeeEdit";
import StationList from "./CRUD/Station/StationList";
import StationCreate from "./CRUD/Station/StationCreat";
import StationEdit from "./CRUD/Station/StationEdit";
import CombineProvider from "../Providers/CombineProvider";
import CarriageList from "./CRUD/Carriage/CarriageList";
import CarriageCreate from "./CRUD/Carriage/CarriageCreate";
import CarriageEdit from "./CRUD/Carriage/CarriageEdit";
import TrainEdit from "./CRUD/Train/TrainEdit";
import TrainCreate from "./CRUD/Train/TrainCreate";
import TrainList from "./CRUD/Train/TrainList";
import RouteList from "./CRUD/Route/RouteList";
import RouteEdit from "./CRUD/Route/RouteEdit";
import RouteCreate from "./CRUD/Route/RouteCreate";
import RouteStationList from "./CRUD/RouteStation/RouteStationList";
import RouteStationEdit from "./CRUD/RouteStation/RouteStationEdit";
import RouteStationCreate from "./CRUD/RouteStation/RouteStationCreate";
import SeatList from "./CRUD/Seats/SeatList";
import SeatCreate from "./CRUD/Seats/SeatCreate";
import SeatEdit from "./CRUD/Seats/SeatEdit";

const Manager = (props) => {

    if (props.role === "manager" && !props.blocked) {
        return (
            <Admin layout={LayoutForAdmin} dataProvider={CombineProvider}>
                <Resource name='user' list={UserList} create={UserCreate} edit={UserEdit}/>
                <Resource name='employee' list={EmployeeList} create={EmployeeCreate} edit={EmployeeEdit} />
                <Resource name='station' list={StationList} create={StationCreate} edit={StationEdit}/>
                <Resource name='carriage' list={CarriageList} create={CarriageCreate} edit={CarriageEdit}/>
                <Resource name='train' list={TrainList} create={TrainCreate} edit={TrainEdit}/>
                <Resource name='route' list={RouteList} create={RouteCreate} edit={RouteEdit}/>
                <Resource name='routestation' list={RouteStationList} create={RouteStationCreate} edit={RouteStationEdit}/>
                <Resource name='seat' list={SeatList} create={SeatCreate} edit={SeatEdit}/>
            </Admin>
        );
    } else if (props.blocked) {
       return (
            <h1>You are blocked</h1>
        );
    }

    return (
        <h1>You do not have permission</h1>
    );
};

export default Manager;