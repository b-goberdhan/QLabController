import React, { Component } from 'react';
import './SideBar.css';
import './../../index.css';
export class SideBar extends Component {
  render() {
    return (
      <div className="SideBarContainer LightGray">
        <div className="SideBarItemContainer Gray">
          All
        </div>
        <div className="SideBarItemContainer Gray">
          Recently Used
        </div>
        <div className="SideBarItemContainer Gray">
          Settings
        </div>
      </div>
    );
  }
}

export default SideBar;
