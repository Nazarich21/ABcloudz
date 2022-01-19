import React, {SyntheticEvent, useEffect, useState} from "react";
import 'bootstrap/dist/css/bootstrap.min.css';
import "../Styles/LogIn.css"
import {Link, Navigate}  from "react-router-dom";
import {logIn} from "../API";

const LogIn = ( props:{setRole:(role:string) => void}) => {
    const[email,setEmail] = useState('');
    const[password,setPassword] = useState('');
    const[emailDirty,setEmailDirty] = useState(false);
    const[passwordDirty,setPasswordDirty] = useState(false);
    const[emailError,setEmailError] = useState('Email can`t be empty');
    const[passwordError,setPasswordError] = useState('Password can`t be empty');
    const[formValid,setFormValid] = useState(false);
    const [redirect,setRedirect] = useState(false);

    useEffect(() =>{
        if (emailError|| passwordError) {
            setFormValid(false)
        } else {
            setFormValid(true)
        }
    },[emailError,passwordError])

    const emailHandler = (e) => {
        setEmail(e.target.value)
        const re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        if (!re.test(String(email).toLowerCase())) {
            setEmailError('wrong email format');
            if (!e.target.value) {
                setPasswordError("Email can`t be empty");
            }
        } else {
            setEmailError("");
        }
    }

    const passwordHandler = (e) => {
        setPassword(e.target.value);

        if (e.target.value.length <8) {
            setPasswordError("password length can`t be less then 8 ");
            if (!e.target.value) {
                setPasswordError("Password can`t be empty");
            }
        } else {
            setPasswordError("");
        }
    }

    const blurHandler = (e) => {
        switch (e.target.name) {
            case 'email':
                setEmailDirty(true);
                break
            case 'password':
                setPasswordDirty(true);
                break
            default:
        }
    }

    const submit = async (e: SyntheticEvent) => {
        e.preventDefault();
        const response =  await logIn(email,password);
        if (response.status !== 200) {
                alert("Wrong email or password");
        } else {
            props.setRole(response.data.role);
            setRedirect(true);
        }
    }

    if (redirect) {
       return <Navigate to="/"/>
    }

    return (
        <div className={"container mt-5 pt-5"}>
            <form onSubmit={submit} >

                <div className="form-outline mb-4">
                    <label className="form-label" htmlFor="form3Example3">Email address</label>
                    {(emailError && emailDirty) && <div className="text-danger">{emailError}</div>}
                    <input name="email" id="form3Example3" className="form-control form-control-lg"
                           placeholder="Enter a valid email address" value={email}
                           onBlur={e=>blurHandler(e)} onChange={e => emailHandler(e)} />
                </div>

                <div className="form-outline mb-3">
                    <label className="form-label" htmlFor="form3Example4">Password</label>
                    {(passwordError && passwordDirty) && <div className="link-danger">{passwordError}</div>}
                    <input  name="password" type="password" id="form3Example4" className="form-control form-control-lg"
                            placeholder="Enter password" value={password}
                            onBlur={e=>blurHandler(e)} onChange={e => passwordHandler(e)}/>
                </div>

                <div className="d-flex justify-content-between flex-row-reverse text-center text-lg-start mt-4 pt-2 me-auto">
                    <button type="submit" disabled={!formValid} className="btn btn-primary btn-lg"
                    >Login</button>
                    <p className="small fw-bold mt-2 pt-1 mb-0">Don't have an account?

                        <Link to="/signup" className="link-danger"> Register</Link></p>
                </div>

            </form>
        </div>
    );
};

export default LogIn;


