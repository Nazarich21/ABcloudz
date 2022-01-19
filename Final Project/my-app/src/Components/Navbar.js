import React  from "react";
import 'bootstrap/dist/css/bootstrap.min.css';
import {Button, Navbar, Nav, Container} from "react-bootstrap";
import {logOut} from "../API";


const NavBar = (props) => {
    let menuLogin;
    let mainMenu;

    const logout = () => {
        logOut();
        props.setRole(" ");
    }
    if (props.role === " ") {
        mainMenu =(
            <Nav className="me-auto">
                <Nav.Link className="nav-link" href="/">Home</Nav.Link>
            </Nav>
        );
        menuLogin = (
                <Nav>
                    <Nav.Link href="/login"><Button variant={"primary"} className="me-3">LogIn</Button></Nav.Link>
                    <Nav.Link href="/signup"><Button variant={"primary"} >SignUp</Button></Nav.Link>
                </Nav>
            );
        } else {
        menuLogin =(
                <Nav>
                    <Nav.Link href="#"><Button variant={"primary"} onClick={logout} className="me-3">Logout</Button></Nav.Link>
                </Nav>
            );
        };


        if ( props.role === 'admin') {
            mainMenu =(
                <Nav className="me-auto">
                    <Nav.Link className="nav-link" href="/">Home</Nav.Link>
                    <Nav.Link className="nav-link" href="/admin">Admin</Nav.Link>
                </Nav>);
        } else  if ( props.role === 'chief') {
            mainMenu =(
                <Nav className="me-auto">
                    <Nav.Link className="nav-link" href="/">Home</Nav.Link>
                    <Nav.Link className="nav-link" href="/statistics">Statistics</Nav.Link>
                    <Nav.Link className="nav-link" href="/employees">Employees</Nav.Link>
                </Nav>);
        } else  if ( props.role === 'manager') {
            mainMenu = (
                <Nav className="me-auto">
                    <Nav.Link className="nav-link" href="/">Home</Nav.Link>
                    <Nav.Link className="nav-link" href="/manager">Manager</Nav.Link>
                </Nav>);
        }
        else  if ( props.role === 'user') {
            mainMenu = (
                <Nav className="me-auto">
                    <Nav.Link className="nav-link" href="/">Home</Nav.Link>
                    <Nav.Link className="nav-link" href="/tickets">Tickets Purchse</Nav.Link>
                    <Nav.Link className="nav-link" href="/usertickets">My Tickets</Nav.Link>
                </Nav>);
        }else{
            mainMenu =(
                <Nav className="me-auto">
                    <Nav.Link className="nav-link" href="/">Home</Nav.Link>
                </Nav>
            );
        }


    return (
        <Navbar collapseOnSelect expand="lg" bg="dark" variant="dark">
            <Container fluid>
                <Navbar.Brand>Railway</Navbar.Brand>
                <Navbar.Toggle aria-controls="responsive-navbar-nav"/>
                <Navbar.Collapse id="responsive-navbar-nav">
                    {mainMenu}
                    {menuLogin}
                </Navbar.Collapse>
            </Container>
        </Navbar>
        );
    };
export default  NavBar;
