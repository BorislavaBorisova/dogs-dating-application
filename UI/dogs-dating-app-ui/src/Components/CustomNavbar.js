import React, { Component } from 'react';
import Navbar from 'react-bootstrap/Navbar';
import Nav from 'react-bootstrap/Nav';
import logoImg from '../img/paw-love-transparent.png';

class CustomNavbar extends Component {

    signOut(){
        
        localStorage.removeItem("api-key");
    }

    render() {
        return (
            <Navbar className="Navbar" collapseOnSelect expand="lg" >
                <Nav.Link href="/explore"><img src={logoImg} style={{width:40, marginTop: -7, marginRight: -20}} /></Nav.Link>
                <Nav.Link href="/explore" className="NavBarText">Doge Love</Nav.Link>
                <Navbar.Toggle aria-controls="responsive-navbar-nav" />
                <Navbar.Collapse id="responsive-navbar-nav">
                    <Nav className="mr-auto">
                        <Nav.Link href="/explore" className="NavBarText">Explore</Nav.Link>
                    </Nav>
                    <Nav>
                    <Nav.Link href="/liked-dogs" className="NavBarText">
                            Liked Dogs
                        </Nav.Link>
                        <Nav.Link href="/your-profile" className="NavBarText">
                            Your profile
                        </Nav.Link>
                        <Nav.Link eventKey={2} href="/sign-in" className="NavBarText" onClick={this.signOut}>
                            Sign out
                        </Nav.Link>
                    </Nav>
                </Navbar.Collapse>
            </Navbar>
        );
    }
}

export default CustomNavbar;