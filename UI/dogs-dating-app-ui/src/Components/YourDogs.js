import React, { Component } from 'react';
import YourDogProfile from './YourDogProfile';
import axios from 'axios';
import '../css/DogProfile.css';

class YourDogs extends Component {

    constructor() {
        super();

        this.state = {
            token: localStorage.getItem("api-key"),
            yourDogs: [],
            displayDogs: false
        }
    }

    componentDidMount() {

        const AuthStr = 'Bearer '.concat(this.state.token);

        axios({
            method: 'get',
            url: 'http://localhost:5000/api/usermanagement/dogs',
            headers: { Authorization: AuthStr }
        })
            .then((response) => {
                if (response.status == 200) {
                    response.data.dogs.map((dog, i) => {
                        this.state.yourDogs.push(<YourDogProfile id={dog.id} name={dog.name} age={dog.age} gender={dog.gender} breed={dog.breed} specifics={dog.specifics} profilePicturePath={dog.profilePicturePath}></YourDogProfile>)
                    })
                    this.setState({ displayDogs: true });
                }
            });
    }

    render() {
        return (

            <div>
                {this.state.displayDogs && this.state.yourDogs.map(item => (

                    <div>
                        {item}
                    </div>
                ))}
                <br />
            </div>

        );
    }
}

export default YourDogs;