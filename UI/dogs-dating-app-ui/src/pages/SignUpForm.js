import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Redirect } from 'react-router';
import axios from 'axios';
import Cities from '../Components/Cities';
import { NavLink } from 'react-router-dom';
import dogsImg from '../img/dogs-kissing.png';

class SignUpForm extends Component {

  constructor() {
    super();

    this.state = {
      fullName: null,
      username: null,
      password: null,
      repeatedPassword: null,
      phoneNumber: null,
      email: null,
      socialMediaLink: null,
      city: '',
      fireRedirect: false,
      lettersForList: '',
      lengthOfLetters: 0,
      displayList: false
    };

    this.handleChange = this.handleChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.handleKey = this.handleKey.bind(this);
  }

  handleChange(e) {
    const target = e.target;
    const value = target.value;
    const name = target.name;

    this.setState({
      [name]: value
    });
  }

  handleKey(e) {
    const target = e.target;
    const value = target.value;
    const length = value.length;

    this.setState({
      city: value,
      lengthOfLetters: length,
      displayList: false
    });

    if (this.state.lengthOfLetters >= 2) {
      this.setState(
        {
          lettersForList: value,
          displayList: true
        }
      )

    }
    if (this.state.value === "") {
      this.setState({
        lengthOfLetters: 0,
        displayList: true
      });
    }
  }


  handleSubmit(e) {
    e.preventDefault();

    const userForRegister = {
      "fullName": this.state.fullName,
      "username": this.state.username,
      "password": this.state.password,
      "repeatedPassword": this.state.repeatedPassword,
      "phoneNumber": this.state.phoneNumber,
      "email": this.state.email,
      "socialMediaLink": this.state.socialMediaLink,
      "city": this.state.city
    };

    axios({
      method: 'post',
      url: 'http://localhost:5000/api/authentication/register',
      data: userForRegister
    })
      .then((response) => {
        if (response.data.success) {
          this.setState({
            fireRedirect: true
          });
        }

      })
      .catch((error) => {
        if (error.response) {
          
          alert(error.response.data.error);
        } else if (error.request) {
          
          alert('Unable to register user. Please try again later.');
        } else {
          
          alert('Error' + error.message);
        }
        
      });
  }

  render() {

    if (this.state.fireRedirect) {

      return (<Redirect to='/sign-in' />);
    }

    return (
      <div className="App">

        <div className="App__Aside">
          <img className="App__Aside__Img" src={dogsImg} alt="Dogs kissing" />
        </div>

        <div className="App__Form">

          <div className="PageSwitcher">
            <NavLink to="/sign-in" activeClassName="PageSwitcher__Item--Active" className="PageSwitcher__Item">Sign In</NavLink>
            <NavLink exact to="/" activeClassName="PageSwitcher__Item--Active" className="PageSwitcher__Item">Sign Up</NavLink>
          </div>

          <div className="FormTitle">
            <NavLink to="/sign-in" activeClassName="FormTitle__Link--Active" className="FormTitle__Link">Sign In</NavLink>
            or
              <NavLink exact to="/" activeClassName="FormTitle__Link--Active" className="FormTitle__Link">Sign Up</NavLink>
          </div>

          <div className="FormCenter">
            <form className="FormFields" onSubmit={this.handleSubmit}>

              <div className="FormField Required">
                <label className="FormField__Label" htmlFor="fullName">Full Name</label>
                <input type="text" id="fullName" className="FormField__Input" placeholder="Enter your full name" name="fullName" value={this.state.fullName} onChange={this.handleChange} required />
              </div>
              <div className="FormField Required">
                <label className="FormField__Label" htmlFor="username">Username</label>
                <input type="text" id="username" className="FormField__Input" placeholder="Enter your username" name="username" value={this.state.username} onChange={this.handleChange} required />
              </div>
              <div className="FormField Required">
                <label className="FormField__Label" htmlFor="password">Password</label>
                <input type="password" id="password" className="FormField__Input" placeholder="Enter your password" name="password" value={this.state.password} onChange={this.handleChange} required />
              </div>
              <div className="FormField Required">
                <label className="FormField__Label" htmlFor="repeatedPassword">Repeat Password</label>
                <input type="password" id="repeatedPassword" className="FormField__Input" placeholder="Repeat your password" name="repeatedPassword" value={this.state.repeatedPassword} onChange={this.handleChange} required />
              </div>
              <div className="FormField">
                <label className="FormField__Label" htmlFor="phoneNumber">Phone Number</label>
                <input type="tel" id="phoneNumber" className="FormField__Input" placeholder="Enter your number" name="phoneNumber" value={this.state.phoneNumber} onChange={this.handleChange} />
              </div>
              <div className="FormField">
                <label className="FormField__Label" htmlFor="email">E-mail Address</label>
                <input type="email" id="email" className="FormField__Input" placeholder="Enter your email" name="email" value={this.state.email} onChange={this.handleChange} />
              </div>
              <div className="FormField">
                <label className="FormField__Label" htmlFor="socialMediaLink">Social Media Link</label>
                <input type="url" id="socialMediaLink" className="FormField__Input" placeholder="Enter your social media link" name="socialMediaLink" value={this.state.socialMediaLink} onChange={this.handleChange} />
              </div>
              <div className="FormField Required">
                <label className="FormField__Label" htmlFor="city">City</label>
                <input type="text" list="cities" id="city" className="FormField__Input" placeholder="Enter your city" name="city" value={this.state.city} onChange={this.handleKey} required />

                {this.state.displayList && <Cities letters={this.state.lettersForList}></Cities>}
              </div>

              <div className="FormField">
                <button className="FormField__Button mr-20">Sign Up</button>
                <Link to="/sign-in" className="FormField__Link">I'm already member</Link>
              </div>
            </form>
          </div>

        </div>
      </div>
    );
  }

}

export default SignUpForm;