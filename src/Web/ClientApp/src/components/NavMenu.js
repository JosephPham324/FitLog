import React, { Component } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import logoImage from '../assets/Logo.png';
export class NavMenu extends Component {
  static displayName = NavMenu.name;

  constructor(props) {
    super(props);

    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true
    };
  }

  toggleNavbar() {
    this.setState({
      collapsed: !this.state.collapsed
    });
  }

  render() {
    return (
      <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>

          <NavbarBrand tag={Link} to="/">
            <img src={logoImage} alt="FitLog Logo" className="logo-nav" /> {/* Thêm hình ?nh logo */}
            FitLog.Web
          </NavbarBrand>

          <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
          <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
            <ul className="navbar-nav flex-grow">
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
              </NavItem>
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/counter">Counter</NavLink>
              </NavItem>
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/fetch-data">Fetch data</NavLink>
              </NavItem>
              <NavItem>
                <a className="nav-link text-dark" href="/Identity/Account/Manage">Account</a>
              </NavItem>
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/ExerciseLog">ExerciseLog</NavLink>
              </NavItem>
                <NavLink tag={Link} className="text-dark" to="/Login">Login</NavLink>
              </NavItem>
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/Register">Register</NavLink>
              </NavItem>
              {/*<NavItem>*/}
              {/*  <NavLink tag={Link} className="text-dark" to="/EquipmentsList">EquipmentsList</NavLink>*/}
              {/*</NavItem>*/}
            </ul>
          </Collapse>
        </Navbar>
      </header>
    );
  }
}