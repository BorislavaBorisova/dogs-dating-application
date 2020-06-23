import React, { Component } from 'react';
import {Route, Redirect} from 'react-router-dom';

class PrivateRoute extends Component {
    constructor(props) {
        super(props);

        this.state = {
            path: props.path,
            component: props.component,
            
        }

    }

    render() {


        return (
            <div>
                {localStorage.getItem("api-key") != null &&
                    <Route path ={this.state.path} component={this.state.component}>

                    </Route>
                }
                {localStorage.getItem("api-key") == null &&
                    <Redirect to='/sign-in' />
                }
            </div>
        );
    }

}


export default PrivateRoute;