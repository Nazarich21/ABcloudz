import {HttpError} from 'react-admin';
import axios from "axios";
import Cookies from "js-cookie";

const apiUrl = 'https://localhost:5001';

export default {
    getList: async (resource, params) => {
        const url = `${apiUrl}/${resource}`;
        const response = await axios.get(url,{ headers: {'Content-type': 'application/json',
                'Authorization':`Bearer ${Cookies.get('jwt')}`},});

        return{
            data: response.data,
            total: parseInt(response.headers['content-range'].split('/').pop(), 10),
        }
    },

    getOne: async (resource, params) =>{
        const url = `${apiUrl}/${resource}/${params.id}`
        const response = await axios.get(url,{ headers: {'Content-type': 'application/json',
                'Authorization':`Bearer ${Cookies.get('jwt')}`},});
        const arrivalTime  =  response.data.arrivalTime;
        const departureTime = response.data.departureTime;
        delete response.data.arrivalTime;
        delete response.data.departureTime;
        response.data.arrivalTime = new Date(0,0,0,arrivalTime.slice(0,2), arrivalTime.slice(-2));
        response.data.departureTime = new Date(0,0,0,departureTime.slice(0,2), departureTime.slice(-2));

        return{
            data:response.data
        }

    },

    update:async (resource, params) =>{
        const response = await axios.put(`${apiUrl}/${resource}/${params.id}`, params.data,{ headers: {'Content-type': 'application/json',
                'Authorization':`Bearer ${Cookies.get('jwt')}`},});
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


    delete: async (resource, params) =>{
        try {
            const response = await axios.delete(`${apiUrl}/${resource}/${params.id}`, {
                headers: {
                    'Content-type': 'application/json',
                    'Authorization': `Bearer ${Cookies.get('jwt')}`
                },
            });

            return{
                data:response.data
            };

        } catch (error) {
            return (Promise.reject(new HttpError('You can not delete this route, there is train on this route', 409, error)));
        }

    },

};