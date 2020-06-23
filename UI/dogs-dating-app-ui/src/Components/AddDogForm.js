import React, { Component } from 'react';
import CustomNavbar from '../Components/CustomNavbar';
import Cities from '../Components/Cities';
import axios from 'axios';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import Breeds from './Breeds';


class AddDogsForm extends Component {
    constructor(props) {
        super(props);

        this.state = {
            token: localStorage.getItem("api-key"),
            showForm: false,
            name: '',
            age: '',
            gender: '',
            breed: '',
            specifics: '',
            profilePicture: '',

            lettersForBreedsList: '',
            lengthOfBreedsLetters: 0,
            displayBreedsList: false,
        };
        this.handleClickShow = this.handleClickShow.bind(this);
        this.handleClickHide = this.handleClickHide.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.encodeImageFileAsURL = this.encodeImageFileAsURL.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleKeyForBreeds = this.handleKeyForBreeds.bind(this);
    }

    handleKeyForBreeds(e) {
        const target = e.target;
        const value = target.value;
        const length = value.length;

        this.setState({
            breed: value,
            lengthOfBreedsLetters: length,
            displayBreedsList: false
        });

        if (this.state.lengthOfBreedsLetters >= 2) {
            this.setState(
                {
                    lettersForBreedsList: value,
                    displayBreedsList: true
                }
            )

        }
        if (this.state.value === "") {
            this.setState({
                lengthOfBreedsLetters: 0,
                displayBreedsList: true
            });
        }
    }

    handleClickShow() {
        this.setState({
            showForm: true
        })
    }

    handleClickHide() {
        this.setState({
            showForm: false
        })
    }

    handleChange(e) {
        const target = e.target;
        const value = target.value;
        const name = target.name;

        this.setState({
            [name]: value
        });
    }

    encodeImageFileAsURL(e) {
        const self = this;
        const file = e.target.files[0];
        const reader = new FileReader();
        reader.onloadend = function () {
            document.getElementById("img").src = reader.result;
            self.setState({ profilePicture: reader.result });
        }
        reader.readAsDataURL(file);
    }

    handleSubmit(e) {
        e.preventDefault();

        const dogToBeAdded = {
            "name": this.state.name,
            "age": this.state.age,
            "gender": this.state.gender,
            "breed": this.state.breed,
            "specifics": this.state.specifics,
            "profilePicturePath": this.state.profilePicture
        };

        const AuthStr = 'Bearer '.concat(localStorage.getItem("api-key"));

        axios({
            method: 'post',
            url: 'http://localhost:5000/api/usermanagement',
            headers: { Authorization: AuthStr },
            data: dogToBeAdded
        })
            .then((response) => {

                if (response.data.success) {
                    this.setState({ showForm: false });
                }
                window.location.reload();
            })
            .catch((error) => {
                if (error.response) {

                    alert(error.response.data.error);

                } else if (error.request) {

                    alert('Unable to add dog. Please try again later.');

                } else {

                    alert('Error' + error.message);
                }
            });
    }


    render() {

        return (

            <div>
                {!this.state.showForm && <span>   <Button className="FormField__ButtonCustom FloatRightButton" onClick={this.handleClickShow} variant="outline-light">Add dog</Button> </span>}

                {this.state.showForm &&


                    <Form onSubmit={this.handleSubmit}>
                        <h2 class="Heading2">Dog Details</h2>

                        <hr class="Hr"></hr>
                        <Form.Group>
                            <Form.Label className="FormField__LabelCustom" htmlFor="name">Name</Form.Label>
                            <Form.Control type="text" className="FormField__InputCustom" id="name" placeholder="Enter your dog's name" name="name" value={this.state.name} onChange={this.handleChange} required />
                        </Form.Group>

                        <Form.Group>
                            <Form.Label className="FormField__LabelCustom" htmlFor="age">Age</Form.Label>
                            <Form.Control type="text" className="FormField__InputCustom" id="age" placeholder="Enter your dog's age" name="age" value={this.state.age} onChange={this.handleChange} required />
                        </Form.Group>

                        <Form.Group >
                            <Form.Label as="legend" className="FormField__LabelCustom">
                                Gender
                                </Form.Label>

                            <Form.Check
                                type="radio"
                                label="Boy"
                                id="boy"
                                name="gender"
                                value="Boy"
                                checked={this.state.gender === "Boy"}
                                onChange={this.handleChange}
                            />
                            <Form.Check
                                type="radio"
                                label="Girl"
                                id="girl"
                                name="gender"
                                value="Girl"
                                checked={this.state.gender === "Girl"}
                                onChange={this.handleChange}
                            />

                        </Form.Group>

                        <Form.Group>
                            <Form.Label className="FormField__LabelCustom" htmlFor="bread">Breed</Form.Label>
                            <Form.Control className="FormField__InputCustom" type="text" list="breeds" id="bread" placeholder="Enter breed" name="breed" value={this.state.breed} onChange={this.handleKeyForBreeds} required/>
                            {this.state.displayBreedsList && <Breeds letters={this.state.lettersForBreedsList}></Breeds>}
                        </Form.Group>

                        <Form.Group>
                            <Form.Label className="FormField__LabelCustom" htmlFor="specifics">Specifics</Form.Label>
                            <Form.Control type="text" className="FormField__InputCustom" id="specifics" placeholder="About your dog" name="specifics" value={this.state.specifics} onChange={this.handleChange} />
                        </Form.Group>

                        <Form.Group>
                            <Form.Label className="FormField__LabelCustom" htmlFor="profilePicture">Profile Picture</Form.Label>
                            <Form.Control type="file" id="profilePicture" name="profilePicture" accept="image/*" onChange={this.encodeImageFileAsURL} />
                        </Form.Group>

                        <img id="img" height="100"></img>

                        <Form.Group style={{ display: "flex" }}>
                            <Button variant="outline-light" className="FormField__ButtonCustom" type="submit" onSubmit={this.handleSubmit}>
                                Add
                        </Button>

                            <Button variant="outline-light" className="FormField__ButtonCustom" type="submit" onClick={this.handleClickHide} style={{ marginLeft: "auto" }}>
                                Cancel
                        </Button>
                        </Form.Group>
                    </Form>
                }
            </div>
        );

    }
}

export default AddDogsForm;