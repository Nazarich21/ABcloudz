import {HttpError} from 'react-admin';
import axios from "axios";
import Cookies from "js-cookie";

const apiUrl = 'https://localhost:5001';

function SeatCompKey({id,seatNo, ...item}) {

    return {
        ...item,
        id: id + seatNo,
        seatNo:seatNo,
        carriage:id,
    };
};
export default {
    getList: async (resource, params) => {
        const url = `${apiUrl}/${resource}`;
        const response = await axios.get(url,{ headers: {'Content-type': 'application/json',
                'Authorization':`Bearer ${Cookies.get('jwt')}`},});
        let resultData =response.data.map(seat => SeatCompKey(seat))

        return{
            data: resultData,
            total: parseInt(response.headers['content-range'].split('/').pop(), 10),
        }
    },

    getOne: async (resource, params) =>{
       const carriage = params.id.substring(0,1);
       const seat = params.id.substring(1,params.id.length);
       const url = `${apiUrl}/${resource}/${carriage},${seat}`;
       const response = await axios.get(url,{ headers: {'Content-type': 'application/json',
               'Authorization':`Bearer ${Cookies.get('jwt')}`},});
         response.data.carriage = response.data.id;
         response.data.id = response.data.id + response.data.seatNo ;

       return({
           data:response.data
       });
    },

    update: async (resource, params) =>{
        const carriage = params.id.substring(0,1);
        const seat = params.id.substring(1,params.id.length);
        params.data.id = params.data.carriage;
        delete  params.data.carriage;
        params.data.status = params.data.statusId;
        delete  params.data.statusId;
        const url = `${apiUrl}/${resource}/${carriage},${seat}`;
        const response = await axios.put(url, params.data,{ headers: {'Content-type': 'application/json',
                'Authorization':`Bearer ${Cookies.get('jwt')}`},});
        response.data.carriage = response.data.id;
        response.data.id = response.data.id + response.data.seatNo ;

        return{
            data:response.data
        };
    },

    create: async (resource, params) =>{
        params.data.id = parseInt(params.data.carriage,10);
        delete  params.data.carriage;
        params.data.status = params.data.statusId;
        delete  params.data.statusId;
        const url = `${apiUrl}/${resource}`;

        const response = await axios.post(url, params.data,{ headers: {'Content-type': 'application/json',
                'Authorization':`Bearer ${Cookies.get('jwt')}`},})
        response.data.carriage = response.data.id;
        response.data.id = response.data.id + response.data.seatNo;

        return({
            data:{...params.data, id:response.data.id   }
        });
    },

    delete: async(resource, params) =>{
        try {
            const carriage = params.id.substring(0, 1);
            const seat = params.id.substring(1, params.id.length);
            const url = `${apiUrl}/${resource}/${carriage},${seat}`;
            const response = await axios.delete(url, {
                headers: {
                    'Content-type': 'application/json',
                    'Authorization': `Bearer ${Cookies.get('jwt')}`
                },
            })
            return ({data: response.data})
        } catch (error) {
            return (Promise.reject(new HttpError('You can not delete this seat, there is ticket with this seat', 409, error)));
        }
    },
};