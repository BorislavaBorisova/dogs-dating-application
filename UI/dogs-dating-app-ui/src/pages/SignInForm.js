import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Redirect } from 'react-router';
import { NavLink } from 'react-router-dom';
import dogsImg from '../img/dogs-kissing.png';
import axios from 'axios';
import {withRouter} from 'react-router-dom';

class SignInForm extends Component {
  constructor() {
    super();

    this.state = {
      username: null,
      password: null,
      fireRedirect: false
    };

    this.handleChange = this.handleChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  handleChange(e) {
    const target = e.target;
    const value = target.value;
    const name = target.name;

    this.setState({
      [name]: value
    });
  }

  handleSubmit(e) {
    e.preventDefault();

    const userForLogIn = {
      "username": this.state.username,
      "password": this.state.password
    };

    axios({
      method: 'post',
      url: 'http://localhost:5000/api/authentication/login',
      data: userForLogIn
    })
      .then((response) => {

        if (response.data.success) {
          this.setState({
            fireRedirect: true
          });
          localStorage.setItem("api-key", response.data.token);
        }
      })
      .catch((error) => {
        if (error.response) {
          alert(error.response.data.error);
        } else if (error.request) {
          alert('Unable to log in user. Please try again later.');
        } else {
          alert('Error' + error.message);
        }
      });
      
  }
  
  render() {
    if (this.state.fireRedirect) {
      this.props.history.push('/explore');
      window.location.reload();
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
                <label className="FormField__Label" htmlFor="username">Username</label>
                <input type="text" id="username" className="FormField__Input" placeholder="Enter your username" name="username" value={this.state.username} onChange={this.handleChange} required />
              </div>
              <div className="FormField Required">
                <label className="FormField__Label" htmlFor="password">Password</label>
                <input type="password" id="password" className="FormField__Input" placeholder="Enter your password" name="password" value={this.state.password} onChange={this.handleChange} required />
              </div>

              <div className="FormField">
                <button className="FormField__Button mr-20" >Sign In</button>
                <Link exact to="/" className="FormField__Link">Create an account</Link>
              </div>
            </form>
          </div>
        </div>

      </div>
    );
  }
}

export default withRouter(SignInForm);