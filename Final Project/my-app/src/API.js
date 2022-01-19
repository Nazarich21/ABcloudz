import Cookies from "js-cookie";
import axios from "axios";

    const apiUrl = 'https://localhost:5001';

    export const userGet = async() => {
        try {
            const response = await axios.get(`${apiUrl}/userget`, {
                headers: {
                    'Content-type': 'application/json',
                    'Authorization': `Bearer ${Cookies.get('jwt')}`
                },
            });
            console.log(response.status);
            return response;
        } catch (e) {
            return (e.response);
        }
    };

    export const stationList = () => {
        return  axios.get(`${apiUrl}/lists/stationslist`,{
            headers: {
                'Content-type': 'application/json',
                'Authorization':`Bearer ${Cookies.get('jwt')}`},
        });
    };

    export const trainList = () => {
        return axios.get(`${apiUrl}/lists/trainslist`, {
            headers: {
                'Content-type': 'application/json',
                'Authorization': `Bearer ${Cookies.get('jwt')}`
            },
            credentials: 'include',
        });
    };

    export const findTrainTimetable = (id) => {
        return axios.get(`${apiUrl}/data/allStations?train=${id}`,{
            headers: {
                'Content-type': 'application/json',
                'Authorization':`Bearer ${Cookies.get('jwt')}`},
        })
    };
    export const findTrains = (id) => {
        return axios.get(`${apiUrl}/data/passingTrains?station=${id}`,{
            headers: {
                'Content-type': 'application/json',
                'Authorization':`Bearer ${Cookies.get('jwt')}`},
        });
    };

    export const stationsGet = () => {
        return  axios.get(`${apiUrl}/station`, {
            headers: {
                'Content-type': 'application/json',
                'Authorization':`Bearer ${Cookies.get('jwt')}`},
            credentials: 'include'
        });
    };

    export const getTrainStatistic  = (beginDate,endDate,train) => {
        axios.get(`${apiUrl}/data/reportTrain?beginDate=${beginDate}&endDate=${endDate}&train=${train}`, {
            headers: {
                'Content-type': 'application/json',
                'Authorization':`Bearer ${Cookies.get('jwt')}`},
            credentials: 'include'
        });
    }
    export const getStationStatistic = (beginDate, endDate, station) => {
        return axios.get(`${apiUrl}/data/reportStation?beginDate=${beginDate}&endDate=${endDate}&station=${station}`, {
            headers: {
                'Content-type': 'application/json',
                'Authorization':`Bearer ${Cookies.get('jwt')}`},
            credentials: 'include',
        });

    };
    export const findTrainsPurchasing = (firstStation, secondStation, departureDate) => {
        return axios.get(`${apiUrl}/data/findTrains?firstStation=${firstStation}&lastStation=${secondStation}&departure=${departureDate}`,{
            headers: {
                'Content-type': 'application/json',
                'Authorization':`Bearer ${Cookies.get('jwt')}`},
            credentials: 'include',
        });
    }
    export const findSeatsPurchasing = (trainId, firstStation, secondStation, departureDate) => {
        return axios.get(`${apiUrl}/data/findSeats?trainId=${trainId}&firstStation=${firstStation}&lastStation=${secondStation}&departureDate=${departureDate}`,{
            headers: {
                'Content-type': 'application/json',
                'Authorization':`Bearer ${Cookies.get('jwt')}`},
            credentials: 'include',
        });
    }
    export const addTicket = async(ticket) => {
        return await axios.post(`${apiUrl}/Ticket`,ticket,{
            headers: {
                'Content-type': 'application/json',
                'Authorization':`Bearer ${Cookies.get('jwt')}`},
            credentials: 'include',
        });
    };
    export const userTickets = (id) => {
        return axios.get(`${apiUrl}/data/usertickets?userId=${id}`,{
            headers: {
                'Content-type': 'application/json',
                'Authorization':`Bearer ${Cookies.get('jwt')}`},
            credentials: 'include',
        });
    };


    export const logIn = async(email, password) => {
        try {
            const res = await axios.post(`${apiUrl}/login`,{email, password});
            return res;
        } catch (e) {
            return e;
        }
    };

    export const signUp = async(email, password) => {
        try {
            const res = await axios.post(`${apiUrl}/register`,{email, password});
            return res;
        } catch (e) {
            return e;
        }
    };

    export const logOut = async() =>{
        await axios.post(`${apiUrl}/logout`);
    };


