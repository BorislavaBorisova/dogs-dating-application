import React, { Component } from 'react';
import {FaPhone} from 'react-icons/fa';
import {FaRegEnvelope} from 'react-icons/fa';
import '../css/DogProfile.css';

class UserContactInfo extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            fullName: props.fullName,
            phoneNumber: props.phoneNumber,
            email: props.email,
            socialMediaLink: props.socialMediaLink,
            city: props.city,
        };

    }

    render() {
        return (
            <div className="UserContactCard">
                {<p> Owner: {this.state.fullName} </p>}
                {this.state.phoneNumber != null && <p> <FaPhone></FaPhone> {this.state.phoneNumber}</p>}
                {this.state.email != null && <p> <FaRegEnvelope></FaRegEnvelope> {this.state.email}</p>}
                {this.state.socialMediaLink != null && <a href={this.state.socialMediaLink}>Connect on social media</a>}
            </div>
        )
    }
}

export default UserContactInfo;

