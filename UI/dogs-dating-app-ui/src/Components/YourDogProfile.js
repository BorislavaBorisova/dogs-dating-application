import React, { Component } from 'react';
import EditDogProfileForm from '../Components/EditDogProfileForm';
import Container from 'react-bootstrap/Container';
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';

class YourDogProfile extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            id: props.id,
            name: props.name,
            age: props.age,
            gender: props.gender,
            owner: props.owner,
            breed: props.breed,
            profilePicturePath: props.profilePicturePath,
            specifics: props.specifics
        }
    }

    render() {
        return (
            <Container className='DogProfile'>
                <Row>
                    <Col className='MiniDogProfile'>

                        {this.state.profilePicturePath != null && 
                                   
                            <img width="300" height="auto" className="DogProfilePicture" src={this.state.profilePicturePath} alt={this.state.name} />
                        }
                        {<h2 className="Heading2_DogName"> {this.state.name} </h2>}
                        <hr className="DogNameHr"></hr>
                        {<p className="DogProfileBreed">{this.state.breed}</p>}
                        {<p className="ProfileTextBottomMarginRemoved">{this.state.gender}</p>}
                    </Col>
                    <Col xs={8}>
                        <EditDogProfileForm id={this.state.id} name={this.state.name} age={this.state.age} specifics={this.state.specifics} profilePicture={this.state.profilePicturePath}></EditDogProfileForm>
                    </Col>
                </Row>
            </Container>
        )
    }
}

export default YourDogProfile;