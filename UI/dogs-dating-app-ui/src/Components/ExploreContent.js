import React, { Component } from 'react';
import Cities from './Cities';
import Breeds from './Breeds';
import axios from 'axios';
import Form from 'react-bootstrap/Form';
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';
import Container from 'react-bootstrap/Container';
import Button from 'react-bootstrap/Button';
import DogProfileInfoWithButton from './DogProfileInfoWithButton';
import AllDogs from './AllDogs';
import SadDog from '../img/sadDog-transparent.png';


class ExploreContent extends Component {
    constructor() {
        super();

        this.state = {
            token: localStorage.getItem("api-key"),
            breed: '',
            minAge: 0,
            maxAge: 30,
            gender: '',
            city: '',
            fireRedirect: false,

            lettersForCitiesList: '',
            lengthOfCitiesLetters: 0,
            displayCitiesList: false,

            lettersForBreedsList: '',
            lengthOfBreedsLetters: 0,
            displayBreedsList: false,

            areFiltered: false,

            dogs: [],
            displayDogs: false,
            currentPage: 1,
            lastPage: 0,

            noResults: true
        };

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleKeyForCities = this.handleKeyForCities.bind(this);
        this.handleKeyForBreeds = this.handleKeyForBreeds.bind(this);
        this.decrementCurrentPage = this.decrementCurrentPage.bind(this);
        this.incrementCurrentPage = this.incrementCurrentPage.bind(this);
        this.getFilteredDogsCount = this.getFilteredDogsCount.bind(this);
    }


    handleChange(e) {
        const target = e.target;
        const value = target.value;
        const name = target.name;

        this.setState({
            [name]: value
        });
    }

    handleKeyForCities(e) {
        const target = e.target;
        const value = target.value;
        const length = value.length;

        this.setState({
            city: value,
            lengthOfCitiesLetters: length,
            displayCitiesList: false
        });

        if (this.state.lengthOfCitiesLetters >= 2) {
            this.setState(
                {
                    lettersForCitiesList: value,
                    displayCitiesList: true
                }
            )

        }
        if (this.state.value === "") {
            this.setState({
                lengthOfCitiesLetters: 0,
                displayCitiesList: true
            });
        }
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

    getFilteredDogsCount() {
        const AuthStr = 'Bearer '.concat(this.state.token);

        const filters = {
            "breed": this.state.breed,
            "minAge": this.state.minAge,
            "maxAge": this.state.maxAge,
            "gender": this.state.gender,
            "city": this.state.city
        };

        axios({
            method: 'post',
            url: 'http://localhost:5000/api/usermanagement/filtereddogscount',
            headers: { Authorization: AuthStr },
            data: filters
        })
            .then((response) => {
                if (response.status == 200) {
                    this.setState({ lastPage: Math.ceil(response.data.count / 4) });
                }
            })
            .catch((error) => {
                if (error.response) {
                    alert(error.response.data.error);
                } else if (error.request) {
                    alert('Unable to execute filters. Please try again later.');
                } else {
                    alert('Error' + error.message);
                }
            });
    }

    handleSubmit(e) {
        e.preventDefault();

        this.setState({ areFiltered: true, currentPage: 1 });

        this.getFilteredDogsCount();
        this.sendQuery();
    }

    sendQuery() {

        const AuthStr = 'Bearer '.concat(this.state.token);

        const filters = {
            "breed": this.state.breed,
            "minAge": this.state.minAge,
            "maxAge": this.state.maxAge,
            "gender": this.state.gender,
            "city": this.state.city
        };

        axios({
            method: 'post',
            url: 'http://localhost:5000/api/usermanagement/filter/' + this.state.currentPage,
            headers: { Authorization: AuthStr },
            data: filters
        })
            .then((response) => {

                this.setState({ displayDogs: true });
                this.state.dogs = [];
                this.setState({ displayDogs: false });
                if (response.status == 200) {

                    response.data.filteredDogs.map((dog, i) => {
                        this.state.dogs.push(<DogProfileInfoWithButton id={dog.id} name={dog.name} age={dog.age} gender={dog.gender} breed={dog.breed} owner={dog.owner} specifics={dog.specifics} profilePicturePath={dog.profilePicturePath}></DogProfileInfoWithButton>)
                    })

                    this.setState({ displayDogs: true, noResults: false });
                }
                if (response.status == 204) {
                    this.setState({
                        noResults: true
                    });
                }

            })
            .catch((error) => {
                if (error.response) {
                    alert(error.response.data.error);
                } else if (error.request) {
                    alert('Unable to execute filters. Please try again later.');
                } else {
                    alert('Error' + error.message);
                }
            });
    }

    goToFirstPage() {
        this.state.currentPage = 1;
        this.setState({ hideAllDogs: true });
        this.getCurrentPageDogs();
    }

    goToLastPage() {
        this.state.currentPage = this.state.lastPage;
        this.setState({ hideAllDogs: true });
        this.getCurrentPageDogs();
    }

    decrementCurrentPage() {
        const previousPage = --this.state.currentPage;
        this.setState({
            currentPage: previousPage
        });

        this.sendQuery();
    }

    incrementCurrentPage() {
        const nextPage = ++this.state.currentPage;
        this.setState({
            currentPage: nextPage
        });

        this.sendQuery();
    }

    render() {
        return (
            <div className="FormCenter">
                <Form onSubmit={this.handleSubmit} className="FormFields FilterForm" >
                    <Form.Row id="topRow">

                        <Form.Group className="Box" as={Col}>
                            <Form.Label className="FormField__LabelCustom" htmlFor="bread">Breed</Form.Label>
                            <Form.Control className="FormField__InputCustom" type="text" list="breeds" id="bread" placeholder="Enter breed" name="breed" value={this.state.breed} onChange={this.handleKeyForBreeds} />
                            {this.state.displayBreedsList && <Breeds letters={this.state.lettersForBreedsList}></Breeds>}
                        </Form.Group>

                        <Form.Group className="Box" as={Col} >
                            <Form.Label htmlFor="city" className="FormField__LabelCustom">City</Form.Label>
                            <Form.Control className="FormField__InputCustom" type="text" list="cities" id="city" placeholder="Enter city" name="city" value={this.state.city} onChange={this.handleKeyForCities} />
                            {this.state.displayCitiesList && <Cities letters={this.state.lettersForCitiesList}></Cities>}
                        </Form.Group>
                    </Form.Row>

                    <Form.Row>
                        <Form.Group as={Col} >
                            <Form.Label htmlFor="minAge" className="FormField__LabelCustom">Minimum Age</Form.Label>
                            <Form.Control type="text" className="FormField__InputCustom" id="minAge" placeholder="Enter minimum age" name="minAge" value={this.state.minAge} onChange={this.handleChange} />
                        </Form.Group>

                        <Form.Group as={Col}>
                            <Form.Label htmlFor="maxAge" className="FormField__LabelCustom">Maximum Age</Form.Label>
                            <Form.Control type="text" className="FormField__InputCustom" id="maxAge" placeholder="Enter maximum age" name="maxAge" value={this.state.maxAge} onChange={this.handleChange} />
                        </Form.Group>
                    </Form.Row>


                    <Form.Row>
                        <Form.Group as={Col} >
                            <Form.Label as="legend" className="FormField__LabelCustomRadioButton">
                                Gender
                            </Form.Label>

                            <Form.Check
                                className="RadioButtonsLabels"
                                type="radio"
                                label="Boy"
                                id="boy"
                                name="gender"
                                value="Boy"
                                checked={this.state.gender === "Boy"}
                                onChange={this.handleChange}
                                inline
                            />
                            <Form.Check
                                className="RadioButtonsLabels"
                                type="radio"
                                label="Girl"
                                id="girl"
                                name="gender"
                                value="Girl"
                                checked={this.state.gender === "Girl"}
                                onChange={this.handleChange}
                                inline
                            />
                        </Form.Group>

                        <Form.Group as={Col} style={{ display: "flex" }}>
                            <Button variant="outline-light" className="FormField__ButtonCustom" type="submit" style={{ marginLeft: "auto" }}>Filter</Button>
                        </Form.Group>

                    </Form.Row>
                </Form>

                {!this.state.areFiltered && <AllDogs> </AllDogs>}
                {this.state.areFiltered &&
                    <div>
                        <div className="Dogs">
                            <Row>
                                {this.state.displayDogs && this.state.dogs.map(item => (

                                    <Col className="bla-bla-class">
                                        {item}
                                    </Col>
                                ))}


                                <br />
                            </Row>
                        </div>

                        {this.state.noResults &&
                            <div className="NoContent">
                                <div className="Centered">
                                    <h1>No Results</h1>
                                    <img src={SadDog}></img>
                                </div>
                            </div>
                        }

                        <Row className="justify-content-md-center">

                            {
                                this.state.currentPage != 1 &&
                                <a onClick={this.goToFirstPage} className="PageButton_Text">
                                    First
                                </a>
                            }
                            {
                                this.state.currentPage != 1 &&
                                <Button variant="outline-light" className="PageButton" onClick={this.decrementCurrentPage}>
                                    &#8249;
                                </Button>
                            }

                            {
                                this.state.currentPage < this.state.lastPage &&
                                <Button variant="outline-light" className="PageButton" onClick={this.incrementCurrentPage}>
                                    &#8250;
                                </Button>
                            }
                            {
                                this.state.currentPage < this.state.lastPage &&
                                <a onClick={this.goToLastPage} className="PageButton_Text">
                                    Last
                                </a>
                            }
                        </Row>
                    </div>
                }

            </div>
        );
    }
}

export default ExploreContent;