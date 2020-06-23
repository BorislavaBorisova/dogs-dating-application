import React, { Component } from 'react';
import DogProfileInfoWithButton from './DogProfileInfoWithButton';
import axios from 'axios';
import Button from 'react-bootstrap/Button';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import Container from 'react-bootstrap/Container';

class AllDogs extends Component {

    constructor() {
        super();

        this.state = {
            token: localStorage.getItem("api-key"),
            allDogs: [],
            displayDogs: false,
            currentPage: 1,
            lastPage: 0
        }

        this.decrementCurrentPage = this.decrementCurrentPage.bind(this);
        this.incrementCurrentPage = this.incrementCurrentPage.bind(this);
        this.getDogEntriesCount = this.getDogEntriesCount.bind(this);
        this.getCurrentPageDogs = this.getCurrentPageDogs.bind(this);
        this.goToFirstPage = this.goToFirstPage.bind(this);
        this.goToLastPage = this.goToLastPage.bind(this);
    }

    getDogEntriesCount() {
        const AuthStr = 'Bearer '.concat(this.state.token);

        axios({
            method: 'get',
            url: 'http://localhost:5000/api/usermanagement/entriescount',
            headers: { Authorization: AuthStr }
        })
            .then((response) => {

                if (response.status == 200) {
                    this.setState({ lastPage: Math.ceil(response.data.count / 4) });
                }
            });
    }

    decrementCurrentPage() {
        --this.state.currentPage;

        this.setState({ hideAllDogs: true });
        this.getCurrentPageDogs();
    }

    incrementCurrentPage() {
        ++this.state.currentPage;

        this.setState({ hideAllDogs: true });
        this.getCurrentPageDogs();
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

    componentDidMount() {

        this.getDogEntriesCount();
        this.getCurrentPageDogs();
    }

    getCurrentPageDogs() {
        const AuthStr = 'Bearer '.concat(this.state.token);

        axios({
            method: 'get',
            url: 'http://localhost:5000/api/usermanagement/alldogs/' + this.state.currentPage,
            headers: { Authorization: AuthStr }
        })
            .then((response) => {

                if (response.status == 200) {

                    this.setState({ allDogs: [] });
                    response.data.allDogs.map((dog, i) => {
                        this.state.allDogs.push(<DogProfileInfoWithButton id={dog.id} name={dog.name} age={dog.age} gender={dog.gender} owner={dog.owner} breed={dog.breed} specifics={dog.specifics} profilePicturePath={dog.profilePicturePath}></DogProfileInfoWithButton>)
                    });

                    this.setState({ displayDogs: true });
                }
            });
    }

    render() {
        return (

            <div>
                <div className="Dogs">
                    <Row>
                        {this.state.displayDogs && this.state.allDogs.map(item => (

                            <Col className="bla-bla-class">
                                {item}
                            </Col>
                        ))}


                        <br />
                    </Row>

                </div>

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

        );
    }
}

export default AllDogs;