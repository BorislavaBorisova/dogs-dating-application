import React, { Component } from 'react';
import { BrowserRouter as Router, Route, Switch } from 'react-router-dom';
import SignUpForm from './pages/SignUpForm';
import SignInForm from './pages/SignInForm';
import Explore from './pages/Explore';
import Profile from './pages/Profile';
import LikedDogs from './pages/LikedDogs';
import './App.css';
import PrivateRoute from './Components/PrivateRoute';



class App extends Component {
  render() {

    return (
      <Router>
        
          <Route exact path='/' component={SignUpForm} />
          <Route path='/sign-in' component={SignInForm} />
          <PrivateRoute path='/explore' component={Explore} />
          <PrivateRoute path='/your-profile' component={Profile} />
          <Route path='/liked-dogs' component={LikedDogs}/>
      </Router>
    );
  }
}

export default App;
