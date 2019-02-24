import React, { Component } from 'react';
import SideBar from './Sidebar/SideBar.js';
import QLabLogin from './QLabLogin/QLabLogin.js';
import './Home.css'
export class Home extends Component {
  displayName = Home.name

  render() {
    return (
      <div className="HomeContainer">
         <QLabLogin/>
      </div>
    );
  }
}
