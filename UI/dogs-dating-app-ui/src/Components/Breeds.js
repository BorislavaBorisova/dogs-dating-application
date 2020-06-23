import React from 'react';
import axios from 'axios';
import uuid from "uuid";

class Breeds extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      letters: this.props.letters,
      items: []
    };
  }

  componentDidMount() {
    const url = 'http://localhost:5000/api/searchfilter/breeds/' + this.state.letters;
    axios.get(url)
      .then(

        res => {
          this.setState({
            items: res.data.breeds
          });
        }
      )
  }


  render() {
    return (
        <datalist id="breeds">
          {
            this.state.items.map(item => (
            <option key={uuid()}
              value = {item.Name}/>
          ))}
        </datalist>
    );
  }
}

export default Breeds;