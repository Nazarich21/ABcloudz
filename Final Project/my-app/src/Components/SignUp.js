import React, {useEffect, useState} from "react";
import {Link, Navigate} from "react-router-dom";
import {signUp} from "../API";

export default  function SignUp() {
    const [email,setEmail] = useState('');
    const [password,setPassword] = useState('');
    const [password2,setPassword2] = useState('');
    const [emailDirty,setEmailDirty] = useState(false);
    const [passwordDirty,setPasswordDirty] = useState(false);
    const [passwordDirty2,setPasswordDirty2] = useState(false);
    const [emailError,setEmailError] = useState('Email can`t be empty');
    const [passwordError,setPasswordError] = useState('Password can`t be empty');
    const [passwordError2,setPasswordError2] = useState('Password can`t be empty');
    const [formValid,setFormValid] = useState(false);
    const [redirect,setRedirect] = useState(false);

    useEffect(() =>{
        if (emailError|| passwordError||passwordError2) {
            setFormValid(false)
        } else {
            setFormValid(true)
        }
        },[emailError,passwordError,passwordError2])

    const emailHandler = (e) => {
        setEmail(e.target.value)
        const re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

        if(!re.test(String(email).toLowerCase())){
            setEmailError('Wrong email format')
            if(!e.target.value){
                setEmailError("Email can`t be empty")
            }
        } else {
            setEmailError("")
        }
    }
    const passwordHandler = (e) => {
        setPassword(e.target.value)

        if (e.target.value.length <8) {
            setPasswordError("Password length can`t be less then 8 ")
            if(!e.target.value){
                setPasswordError("Password can`t be empty")
            }
        } else {
            setPasswordError("")
        }
    }

    const password2Handler = (e) => {
        setPassword2(e.target.value)

      if (e.target.value !== password) {
          setPasswordError2("Wrong 2 password")
      } else {
          setPasswordError2("")
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
            case 'passwordconfirm':
                setPasswordDirty2(true);
                break
            default:
        }
    }

    const submit = async (e) => {
        e.preventDefault();
        const response = await signUp(email,password);

        if (response.status === 409) {
           alert("This email is busy");
        } else {
            setRedirect(true);
        }
    }

    if (redirect) {
        return <Navigate to="/"/>
    }

    return (
        <div className={"container mt-5 pt-5"}>
            <form onSubmit={submit}>
                <div className="form-outline mb-4">
                    <label className="form-label" htmlFor="form1">Email address</label>
                    {(emailError && emailDirty) && <div className="text-danger">{emailError}</div>}
                    <input name="email" id="form1" className="form-control form-control-lg"
                           placeholder="Enter a valid email address" value={email}
                           onBlur={e=>blurHandler(e)} onChange={e => emailHandler(e)} />
                </div>

                <div className="form-outline mb-3">
                    <label className="form-label" htmlFor="form2">Password</label>
                    {(passwordError && passwordDirty) && <div className="link-danger">{passwordError}</div>}
                    <input  name="password" type="password" id="form2" className="form-control form-control-lg"
                            placeholder="Enter password" value={password}
                            onBlur={e=>blurHandler(e)} onChange={e => passwordHandler(e)}/>
                </div>
                <div className="form-outline mb-3">
                    <label className="form-label" htmlFor="form3">Confirm password </label>
                    {(passwordError2 && passwordDirty2) && <div className="link-danger">{passwordError2}</div>}
                    <input  name="passwordconfirm" type="password" id="form3" className="form-control form-control-lg"
                            placeholder="Enter password" value={password2}
                            onBlur={e=>blurHandler(e)} onChange={e => password2Handler(e)}/>
                </div>

                <div className="d-flex justify-content-between flex-row-reverse text-center text-lg-start mt-4 pt-2 me-auto">
                    <button type="submit" disabled={!formValid} className="btn btn-primary btn-lg"
                    >Register</button>
                    <p className="small fw-bold mt-2 pt-1 mb-0">You already have account?
                        <Link to="/login" className="link-danger"> LogIn</Link></p>
                </div>
            </form>
        </div>
    );
}
