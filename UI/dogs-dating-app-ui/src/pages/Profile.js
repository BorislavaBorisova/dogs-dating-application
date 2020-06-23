import React, { Component } from 'react';
import CustomNavbar from '../Components/CustomNavbar';
import Container from 'react-bootstrap/Container';
import EditProfileForm from '../Components/EditProfileForm';
import AddDogForm from '../Components/AddDogForm';
import YourDogs from '../Components/YourDogs';
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';


class Profile extends Component {
    constructor(props) {
        super(props);

    }

    render() {

        return (
            <div>
                <CustomNavbar></CustomNavbar>

                <Container>

                    <EditProfileForm></EditProfileForm>

                    <AddDogForm></AddDogForm>

                    <h2 className="Heading2_Titles TopMargin">Your Dogs</h2>
                    <hr className="Hr"></hr>

                    <YourDogs></YourDogs>

                </Container>
            </div>
        );
    }
}

export default Profile;