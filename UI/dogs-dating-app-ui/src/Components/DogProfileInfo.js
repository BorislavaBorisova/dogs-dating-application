import React, { Component } from 'react';
import Popup from "reactjs-popup";
import { FaRegHeart } from 'react-icons/fa';
import { FaHeart } from 'react-icons/fa';
import axios from 'axios';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';

class DogProfileInfo extends React.Component {

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
            specifics: props.specifics,
            isLiked: false
        }

        this.likeDog = this.likeDog.bind(this);
        this.unlikeDog = this.unlikeDog.bind(this);
    }

    insertDogIntoDatabase() {
        const AuthStr = 'Bearer '.concat(localStorage.getItem("api-key"));

        axios({
            method: 'post',
            url: 'http://localhost:5000/api/usermanagement/likedog/' + this.state.id,
            headers: { Authorization: AuthStr }
        })
            .then((response) => {

            })
            .catch((error) => {
                if (error.response) {

                    alert(error.response.data.error);

                } else if (error.request) {

                    alert('Unable to add to liked dogs. Please try again later.');

                } else {

                    alert('Error' + error.message);
                }
            });
    }

    likeDog(e) {
        e.preventDefault();

        this.setState({ isLiked: true });
        this.insertDogIntoDatabase();
    }

    unlikeDog(e) {
        e.preventDefault();
        //TODO: delete from database
        this.setState({ isLiked: false });

    }

    render() {
        return (
            <div >
                {this.state.profilePicturePath != null &&
                    <div id="dogImageId">
                        <span className="helper"></span><img src={this.state.profilePicturePath} alt={this.state.name} />
                    </div>}
                {<hr style={{ 'margin-top': 0 }}></hr>}
                {<h2 className="Heading2_DogName"> {this.state.name} </h2>}

                {!this.state.isLiked && <a className="LikeButton" onClick={this.likeDog}><FaRegHeart></FaRegHeart></a>}
                {this.state.isLiked && <a className="LikeButton" onClick={this.unlikeDog}><FaHeart></FaHeart></a>}

                {<p className="DogProfileBreed">{this.state.breed}</p>}
                {<p className="ProfileText">{this.state.gender}</p>}
                {<p className="ProfileText"> {this.state.age} years old</p>}

                <Popup on="hover" trigger={<a className="GetToKnowMe"> Get to know me </a>} position="right bottom">
                    {this.state.specifics && <div className="PopupContent">{this.state.specifics}</div>}
                    {!this.state.specifics && <div className="PopupContent">Sorry. No information. ;(</div>}
                </Popup>
            </div>
        )
    }
}
export default DogProfileInfo;