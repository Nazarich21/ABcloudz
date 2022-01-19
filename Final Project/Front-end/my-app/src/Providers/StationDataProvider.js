import axios from 'axios';
import Cookies from "js-cookie";

const apiUrl = 'https://localhost:5001';

export default {
    getList:async (resource, params) => {
        const url = `${apiUrl}/${resource}`;

        const response = await axios.get(url,{ headers: {'Content-type': 'application/json',
                'Authorization':`Bearer ${Cookies.get('jwt')}`},});

        return{
            data: response.data,
            total: parseInt(response.headers['content-range'].split('/').pop(), 10),
        }
    },

    getOne:async (resource, params) =>{
        const url = `${apiUrl}/${resource}/${params.id}`;
        const response = await axios.get(url,{ headers: {'Content-type': 'application/json',
                'Authorization':`Bearer ${Cookies.get('jwt')}`},});
        return({
            data:response.data
        });
    },

    update: async (resource, params) =>{
        const url = `${apiUrl}/${resource}/${params.id}`
        const response = await axios.put(url, params.data,{ headers: {'Content-type': 'application/json',
                'Authorization':`Bearer ${Cookies.get('jwt')}`},});
        return({
            data:response.data
        });
    },

    create: async (resource, params) =>{
        const url = `${apiUrl}/${resource}`
        const response = await axios.post(url, params.data,{ headers: {'Content-type': 'application/json',
                'Authorization':`Bearer ${Cookies.get('jwt')}`},})
        return({
            data:{...params.data, id:response.data.id   }
        });
    },

    delete: async (resource, params) =>{
        const url = `${apiUrl}/${resource}/${params.id}`
        const response = await axios.delete(url,{ headers: {'Content-type': 'application/json',
                'Authorization':`Bearer ${Cookies.get('jwt')}`},})

        return({data:response.data})
    },
};