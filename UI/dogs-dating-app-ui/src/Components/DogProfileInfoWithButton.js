import React, { Component } from 'react';
import DogProfileInfo from './DogProfileInfo';
import axios from 'axios';
import Button from 'react-bootstrap/Button';
import UserContactInfo from './UserContactInfo';
import Modal from 'react-awesome-modal';
import {FaRegWindowClose} from 'react-icons/fa';


class DogProfileInfoWithButton extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            token: localStorage.getItem("api-key"),
            id: props.id,
            name: props.name,
            age: props.age,
            gender: props.gender,
            breed: props.breed,
            owner: props.owner,
            profilePicturePath: props.profilePicturePath,
            specifics: props.specifics,

            username: '',
            fullName: '',
            phoneNumber: '',
            email: '',
            socialMediaLink: '',
            city: '',

            showContactInfo: false,
            visible: false
        }

        this.handleClick = this.handleClick.bind(this);
        this.openModal = this.openModal.bind(this);
        this.closeModal = this.closeModal.bind(this);
    }

    openModal() {
        this.setState({
            visible: true
        });

    }

    closeModal() {
        this.setState({
            visible: false
        });
    }


    handleClick(e) {
        e.preventDefault();

        const AuthStr = 'Bearer '.concat(this.state.token);

        axios({
            method: 'get',
            url: 'http://localhost:5000/api/usermanagement/user/' + this.state.owner,
            headers: { Authorization: AuthStr },
        })
            .then((response) => {

                if (response.status == 200) {
                    this.setState({
                        username: response.data.username,
                        fullName: response.data.fullName,
                        phoneNumber: response.data.phoneNumber,
                        email: response.data.email,
                        socialMediaLink: response.data.socialMediaLink,
                        city: response.data.city,
                        showContactInfo: true
                    });

                    this.openModal();
                }

            })
            .catch((error) => {
                if (error.response) {
                    alert(error.response.data.error);
                } else if (error.request) {
                    alert('Unable to find contact info. Please try again later.');
                } else {

                    alert('Error' + error.message);
                }

            });       
    }


    render() {
        return (
            <div>
                <div className="Card">
                    <DogProfileInfo id={this.state.id} name={this.state.name} age={this.state.age} gender={this.state.gender} breed={this.state.breed} specifics={this.state.specifics} profilePicturePath={this.state.profilePicturePath}></DogProfileInfo>
                    <Button variant="outline-light" className="ContactButton" onClick={this.handleClick}>Contact Info</Button>
                </div>

                <Modal
                    visible={this.state.visible}
                    width="400"
                    height="200"
                    effect="fadeInUp"
                    onClickAway={() => this.closeModal()}
                >
                    <div className="Modal">
                        {this.state.showContactInfo && <UserContactInfo username={this.state.username} fullName={this.state.fullName} phoneNumber={this.state.phoneNumber} email={this.state.email}
                            socialMediaLink={this.state.socialMediaLink} city={this.state.city}></UserContactInfo>}
                        <Button variant="outline-light" onClick={() => this.closeModal()} className="CloseModalButton"><FaRegWindowClose></FaRegWindowClose></Button>
                    </div>
                </Modal>
            </div>
        )
    }
}

export default DogProfileInfoWithButton;