import React, { Component } from 'react';
import axios from 'axios';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';
import Modal from 'react-awesome-modal';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';

class EditDogProfileForm extends Component {
    constructor(props) {
        super(props);

        this.state = {
            token: localStorage.getItem("api-key"),
            id: props.id,
            name: props.name,
            age: props.age,
            specifics: props.specifics,
            profilePicture: props.profilePicture,

            visible: false
        };

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.encodeImageFileAsURL = this.encodeImageFileAsURL.bind(this);
        this.deleteDog = this.deleteDog.bind(this);
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

        const AuthStr = 'Bearer '.concat(this.state.token);

        const dogForUpdate = {
            "name": this.state.name,
            "age": this.state.age,
            "specifics": this.state.specifics,
            "profilePicturePath": this.state.profilePicture
        };

        axios({
            method: 'put',
            url: 'http://localhost:5000/api/usermanagement/update/' + this.props.id,
            headers: { Authorization: AuthStr },
            data: dogForUpdate
        })
            .then((response) => {

                if (response.data.success) {
                    console.log(response);
                }

                window.location.reload();

            })
            .catch((error) => {
                if (error.response) {

                    alert(error.response.data.error);

                } else if (error.request) {

                    alert('Unable to update dog. Please try again later.');

                } else {

                    alert('Error' + error.message);
                }
            });
    }

    deleteDog() {
        const AuthStr = 'Bearer '.concat(this.state.token);

        axios({
            method: 'delete',
            url: 'http://localhost:5000/api/usermanagement/delete/' + this.props.id,
            headers: { Authorization: AuthStr }
        })
            .then((response) => {

                if (response.data.success) {
                    console.log(response);
                }

                window.location.reload();
            })
            .catch((error) => {
                if (error.response) {

                    alert(error.response.data.error);

                } else if (error.request) {

                    alert('Unable to delete dog. Please try again later.');

                } else {

                    alert('Error' + error.message);
                }
            });
        this.closeModal();
    }

    render() {

        return (
            <div>
                <h2 class="Heading2">Edit Profile</h2>

                <hr class="Hr"></hr>

                <Form onSubmit={this.handleSubmit}>
                    <Form.Group>
                        <Form.Label className="FormField__LabelCustom" htmlFor="name">Name</Form.Label>
                        <Form.Control type="text" className="FormField__InputCustom" id="name" placeholder={this.state.name} name="name" value={this.state.name} onChange={this.handleChange} />
                    </Form.Group>

                    <Form.Group>
                        <Form.Label className="FormField__LabelCustom" htmlFor="age">Age</Form.Label>
                        <Form.Control type="age" className="FormField__InputCustom" id="age" placeholder={this.state.age} name="age" value={this.state.age} onChange={this.handleChange} />
                    </Form.Group>

                    <Form.Group>
                        <Form.Label className="FormField__LabelCustom" htmlFor="specifics">Specifics</Form.Label>
                        <Form.Control className="FormField__InputCustom" id="specifics" placeholder={this.state.specifics} name="specifics" value={this.state.specifics} onChange={this.handleChange} />
                    </Form.Group>

                    <Form.Group>
                        <Form.Label className="FormField__LabelCustom" htmlFor="profilePicture">Profile Picture</Form.Label>
                        <Form.Control type="file" id="profilePicture" name="profilePicture" accept="image/*" onChange={this.encodeImageFileAsURL} />
                    </Form.Group>

                    <img id="img" height="100"></img>

                    <Button className="FormField__ButtonCustom FloatRightButton" type="submit" variant="outline-light">
                        Update
                    </Button>

                    <Button className="FormField__ButtonCustom FloatLeftButton" onClick={this.openModal} variant="outline-light">
                        Delete
                    </Button>

                    <Modal
                        visible={this.state.visible}
                        width="400"
                        height="200"
                        effect="fadeInUp"
                        onClickAway={() => this.closeModal()}
                    >

                        <div className="ConfirmationModal">
                            <Col className="ConfirmationModalText" >
                                <Row className= "justify-content-center ">
                                    <p className="Font">Are you sure you want to delete selected dog?</p>
                                </Row>
                                <Row className= "justify-content-center">
                                    <Button  variant="outline-light" className="YesButton" onClick={() => this.deleteDog()} >Yes</Button>
                                    <Button  variant="outline-light" className="NoButton" onClick={() => this.closeModal()} >No</Button>
                                </Row>
                            </Col>


                        </div>
                    </Modal>
                </Form>
            </div>
        );
    }
}

export default EditDogProfileForm;