import React, { Component } from 'react';
import CustomNavbar from '../Components/CustomNavbar';
import Container from 'react-bootstrap/Container';
import EditProfileForm from '../Components/EditProfileForm';
import AddDogForm from '../Components/AddDogForm';
import YourDogs from '../Components/YourDogs';
import DogProfileInfoWithButton from '../Components/DogProfileInfoWithButton';
import axios from 'axios';
import InfiniteScroll from 'react-infinite-scroll-component';
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';


class LikedDogs extends Component {
    constructor() {
        super();

        this.state = {
            token: localStorage.getItem("api-key"),
            likedDogs: [],
            displayDogs: false,
            currentPage: 1,
            numberOfLikedDogs: 0
        }

        this.getLikedDogsCount = this.getLikedDogsCount.bind(this);
        this.getCurrentPageDogs = this.getCurrentPageDogs.bind(this);
        this.fetchData = this.fetchData.bind(this);

    }

    getLikedDogsCount() {
        const AuthStr = 'Bearer '.concat(this.state.token);

        axios({
            method: 'get',
            url: 'http://localhost:5000/api/usermanagement/likeddogscount',
            headers: { Authorization: AuthStr }
        })
            .then((response) => {

                if (response.status == 200) {
                    this.setState({ numberOfLikedDogs: Math.ceil(response.data.count) });
                }

            });

    }


    getCurrentPageDogs() {
        const AuthStr = 'Bearer '.concat(this.state.token);

        axios({
            method: 'get',
            url: 'http://localhost:5000/api/usermanagement/likeddogs/' + this.state.currentPage,
            headers: { Authorization: AuthStr }
        })
            .then((response) => {

                if (response.status == 200) {

                    response.data.likedDogs.map((dog, i) => {
                        this.state.likedDogs.push(<DogProfileInfoWithButton name={dog.name} age={dog.age} gender={dog.gender} owner={dog.owner} breed={dog.breed} specifics={dog.specifics} profilePicturePath={dog.profilePicturePath}></DogProfileInfoWithButton>)
                    });

                    this.setState({ displayDogs: true });
                }
            });

    }

    fetchData() {
        ++this.state.currentPage;
        this.setState({ displayDogs: false });
        this.getCurrentPageDogs();
        this.setState({ displayDogs: true });
    }

    componentDidMount() {
        this.getLikedDogsCount();
        this.getCurrentPageDogs();
    }

    render() {

        return (
            <div>
                <CustomNavbar></CustomNavbar>

                <Container>

                    {this.state.displayDogs && <InfiniteScroll
                        dataLength={this.state.likedDogs.length} //This is important field to render the next data
                        next={this.fetchData}
                        hasMore={this.state.likedDogs.length <= this.state.numberOfLikedDogs}
                        loader={<h4>Loading...</h4>}
                        endMessage={
                            <p style={{ textAlign: 'center' }}>
                                <b>Yay! You have seen it all</b>
                            </p>

                        }
                    >

                    
                        {this.state.likedDogs}



                    </InfiniteScroll>
                    }
                </Container>
            </div>
        );
    }
}

export default LikedDogs;