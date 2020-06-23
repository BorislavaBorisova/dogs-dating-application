import React, { Component } from 'react';
import CustomNavbar from '../Components/CustomNavbar';
import ExploreContent from '../Components/ExploreContent';
import Container from 'react-bootstrap/Container';


class Explore extends Component {


    render() {

        return (
            <div>
                <CustomNavbar></CustomNavbar>

                <Container>
                    <ExploreContent></ExploreContent>
                </Container>

            </div>
        );

    }

}

export default Explore;