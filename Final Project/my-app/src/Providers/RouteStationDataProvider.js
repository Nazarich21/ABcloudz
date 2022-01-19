import axios from "axios";
import Cookies from "js-cookie";

const apiUrl = 'https://localhost:5001';
function RSCompKey({id,stationId,order, ...item}) {

    return {
        ...item,
        id: id +stationId+order,
        route:id,
        station:stationId,
        order: order,
    };

};
export default {
    getList: async(resource, params) => {
        const url = `${apiUrl}/${resource}`;
        const response = await axios.get(url,{ headers: {'Content-type': 'application/json',
                'Authorization':`Bearer ${Cookies.get('jwt')}`},});

        let resultData =response.data.map(seat => RSCompKey(seat))
        return ({
            data: resultData,
            total: parseInt(response.headers['content-range'].split('/').pop(), 10),
        });
    },
     getOne: async(resource, params) =>{
         const route = params.id.substring(0,1);
         const station = params.id.substring(1,3);
         const order = params.id.substring(params.id.length-1,params.id.length+1);
         const url = `${apiUrl}/${resource}/${station},${route},${order}`;
         const response = await axios.get(url,{ headers: {'Content-type': 'application/json',
                 'Authorization':`Bearer ${Cookies.get('jwt')}`},});
         const arrivalTime  =  response.data.arrivalTime;
         const departureTime = response.data.departureTime;
         delete response.data.arrivalTime;
         delete response.data.departureTime;
         response.data.arrivalTime = new Date(0,0,0,arrivalTime.slice(0,2), arrivalTime.slice(-2));
         response.data.departureTime = new Date(0,0,0,departureTime.slice(0,2), departureTime.slice(-2))
         response.data.route = response.data.id;
         response.data.id = response.data.id + response.data.stationId + response.data.order ;
         return({
                     data:response.data
                 });
     },

    update: async (resource, params) =>{
        const route = params.id.substring(0,1);
        const station = params.id.substring(1,3);
        const order = params.id.substring(params.id.length-1,params.id.length+1);
        console.log(params)
        params.data.id = route;
        delete  params.data.station;

        const url = `${apiUrl}/${resource}/${station},${route},${order}`;
        const response = await axios.put(url, params.data,{ headers: {'Content-type': 'application/json',
                'Authorization':`Bearer ${Cookies.get('jwt')}`},});

        response.data.station = response.data.stationId;
        response.data.route = response.data.id;
        response.data.id = response.data.route + response.data.stationId + response.data.order;
        delete  response.data.stationId;

        return({
            data:response.data
        });
    },

    create: async (resource, params) =>{
        const response = await axios.post(`${apiUrl}/${resource}`, params.data,{ headers: {'Content-type': 'application/json',
                'Authorization':`Bearer ${Cookies.get('jwt')}`}});
        return({
            data:{...params.data, id:response.data.id}
        });
    },

    delete: async(resource, params) =>{
        console.log(params);
        const url = `${apiUrl}/${resource}?id=${params.previousData.route}&stationId=${params.previousData.station}&order=${params.previousData.order}`;
        const response = await axios.delete(url,{ headers: {'Content-type': 'application/json',
                'Authorization':`Bearer ${Cookies.get('jwt')}`},})
        return({data:response.data})
    },

};