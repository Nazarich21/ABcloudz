import './App.css';
import NavBar from "./Components/Navbar";
import React, {useEffect, useState} from "react";
import {BrowserRouter as Router, Route, Routes} from "react-router-dom";
import Home from "./Components/Home";
import SignUp from "./Components/SignUp";
import LogIn from "./Components/LogIn";
import AdminPage from "./Components/AdminPage";
import Manager from "./Components/Manager"
import User from "./Components/User";
import UserTickets from "./Components/UserTickets";
import Employees from "./Components/Employees";
import Statistics from "./Components/Statistics";
import {userGet} from "./API";

function App() {

    const [role, setRole] = useState(' ');
    const [id, setId] = useState(0);
    const [blocked, setBlocked] = useState(false);
    const [serverRunOut, setServerRunOut] = useState(true);

    useEffect(() => {
        async function fetchData() {
            const response = await userGet()

            if(response.status === 200 ) {
                setServerRunOut(false);
                setRole(response.data.role);
                setId(response.data.id);
                setBlocked(response.data.blocked);
            } else if(response.status === 401 )  {
                setServerRunOut(false);
            }
        };
        fetchData();
    }, [])

    if (!serverRunOut) {
        return (
            <Router>
                <NavBar role={role} setRole={setRole}/>
                <Routes>
                    <Route exact path="/" element={<Home/>}/>
                    <Route path="/admin" element={<AdminPage role={role} blocked={blocked}/>}/>
                    <Route path="/signup" element={<SignUp setRole={setRole}/>}/>
                    <Route path="/login" element={<LogIn setRole={setRole}/>}/>
                    <Route path="/manager" element={<Manager role={role} blocked={blocked}/>}/>
                    <Route path="/tickets" element={<User role={role} id={id} blocked={blocked}/>}/>
                    <Route path="/usertickets" element={<UserTickets role={role} id={id} blocked={blocked}/>}/>
                    <Route path="/employees" element={<Employees role={role} blocked={blocked}/>}/>
                    <Route path="/statistics" element={<Statistics role={role} blocked={blocked}/>}/>
                </Routes>
            </Router>);
    }
    return (
        <h1>Sorry, server unavailable now</h1>
    );

};

export default App;
