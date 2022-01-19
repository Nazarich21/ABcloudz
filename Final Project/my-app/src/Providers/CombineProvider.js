import UserDataProvider from "./UserDataProvider";
import TrainDataProvider from "./TrainDataProvider";
import RouteStationDataProvider  from "./RouteStationDataProvider";
import EmployeeDataProvider from "./EmployeeDataProvider";
import SeatDataProvider from "./SeatDataProvider";
import RouteDataProvider from "./RouteDataProvider";
import CarriageDataProvider from "./CarriageDataProvider";
import StationDataProvider from "./StationDataProvider";

const providers = [
    { resource: 'user', provider: UserDataProvider },
    { resource: 'employee', provider: EmployeeDataProvider },
    { resource: 'station', provider: StationDataProvider },
    { resource: 'carriage', provider: CarriageDataProvider },
    { resource: 'train', provider: TrainDataProvider },
    { resource: 'route', provider: RouteDataProvider },
    { resource: 'routestation', provider: RouteStationDataProvider },
    { resource: 'seat', provider: SeatDataProvider },

];

function findProvider(resource) {
    return providers.find(provider => provider.resource.includes(resource)).provider;
}

const combineProviders = {

    getOne: (resource, params) => {
        return findProvider(resource).getOne(resource, params);
    },

    delete: (resource, params) => {
        return findProvider(resource).delete(resource, params);
    },

    getList: (resource, params) => {
        return findProvider(resource).getList(resource, params);
    },

    create: (resource, params) => {
        return findProvider(resource).create(resource, params);
    },

    update: (resource, params) => {
        return findProvider(resource).update(resource, params);
    },

};

export default combineProviders;
