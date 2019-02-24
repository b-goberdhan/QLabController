import React, { Component } from 'react';
import  {FontAwesomeIcon} from '@fortawesome/react-fontawesome';
import {faSync} from '@fortawesome/free-solid-svg-icons';
import QLabContainer from '../../containers/QLabContainer.js';

import './QLabLogin.css';
import './../../index.css';

const LOGGING_IN_PROGRESS = "Connecting to QLab...";
const LOGGING_ERROR = "Could not connect to QLab please try again."

export class QLabLogin extends Component {
  constructor(props) {
      super(props);
      this.state = {
        hasError: false,
        currentMessage: LOGGING_IN_PROGRESS
      };
      this.isConnected = false;
      this.ipAddress = '';
      this.connectToQLab(this.onSuccess, this.onError);
      
  }
  connectToQLab(onSuccess, onError) {
      QLabContainer.connect(this.ipAddress, onSuccess.bind(this), onError.bind(this));
  }
  onSuccess() {
    this.isConnected = true;
    this.setState({
      hasError: false
    });
  }
  onError() {
    this.isConnected = false;
    this.setState({
        hasError: true,
        currentMessage: LOGGING_ERROR
    });
  }

  onConnect() {
    this.connectToQLab(this.onSuccess, this.onError);
  }
  onInputUpdated(evt) {
    this.ipAddress = evt.target.value;
  }
  render() {

    var connecting = (
      <div className="LoginContainer LightGray BlueBorder CenterContents" >
        <div className="CenterContents StatusMessage">
            <div>
                {this.state.currentMessage}
            </div>
        </div>
        <FontAwesomeIcon icon={faSync} size="2x" spin/>
      </div>
    );
    var error = (
      <div className="LoginContainer LightGray BlueBorder CenterContents" >
        <div className="CenterContents StatusMessage">
            <div>
                {this.state.currentMessage}
            </div>
        </div>
        <div>Enter a new ip address:</div>
        <div className="CenterContents">
            <input className="IpInputBox IpInputBox:focus" onChange={this.onInputUpdated.bind(this)}></input>
        </div>
        <button className="SubmitButton Blue BlueBorder"  onClick={this.onConnect.bind(this)}>
          Connect
        </button>
      </div>
    );
    return this.state.hasError ? error: connecting;
  }
}

export default QLabLogin;
