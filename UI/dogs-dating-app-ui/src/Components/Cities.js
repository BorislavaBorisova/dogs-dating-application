import React from 'react';
import axios from 'axios';
import uuid from "uuid";

class Cities extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      letters: this.props.letters,
      items: []
    };
  }

  componentDidMount() {
    const url = 'http://localhost:5000/api/searchfilter/cities/' + this.state.letters;
    axios.get(url)
      .then(

        res => {
          this.setState({
            items: res.data.cities
          });
        }
      )
  }


  render() {
    return (
        <datalist id="cities">
          {
            this.state.items.map(item => (
            <option key={uuid()}
              value = {item.Name}/>
          ))}
        </datalist>
    );
  }
}

export default Cities;