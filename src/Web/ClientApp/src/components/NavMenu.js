import React, { useContext, useEffect, useState, useRef } from 'react';
import {
  Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink,
  UncontrolledDropdown, DropdownToggle, DropdownMenu, DropdownItem, Badge
} from 'reactstrap';
import { Link } from 'react-router-dom';
import { HubConnectionBuilder, LogLevel, HttpTransportType } from '@microsoft/signalr';
import './NavMenu.css';
import logoImage from '../assets/Logo.png';
import LogoutButton from './LogoutButton';
import { AuthContext } from '../context/AuthContext';
import { getCookie } from '../utils/cookiesOperations';

export const NavMenu = () => {
  const { isAuthenticated, logout } = useContext(AuthContext);
  const [collapsed, setCollapsed] = useState(true);
  const [notifications, setNotifications] = useState([]);
  const connectionRef = useRef(null);

  useEffect(() => {
    if (isAuthenticated) {
      const jwtHeaderPayload = getCookie('jwtHeaderPayload');
      const jwtSignature = getCookie('jwtSignature');
      const token = jwtHeaderPayload && jwtSignature ? `${jwtHeaderPayload}.${jwtSignature}` : null;

      if (!token) {
        console.error('Token not found or invalid');
        return;
      }

      const newNotifConnection = new HubConnectionBuilder()
        .withUrl('https://localhost:44447/api/notificationHub', {
          accessTokenFactory: () => token,
          transport: HttpTransportType.LongPolling
        })
        .configureLogging(LogLevel.Debug)
        .withAutomaticReconnect()
        .build();

      newNotifConnection.on('ReceiveMessage', (user, message) => {
        setNotifications(prevState => [...prevState, { user, message }]);
      });

      newNotifConnection.start()
        .then(() => {
          console.log('Connected to SignalR hub!');
          connectionRef.current = newNotifConnection;
        })
        .catch(err => {
          console.error('Error connecting to SignalR hub:', err);
        });

      return () => {
        if (connectionRef.current) {
          connectionRef.current.stop().then(() => console.log('Disconnected from SignalR hub.'));
        }
      };
    }
  }, [isAuthenticated]);

  const toggleNavbar = () => {
    setCollapsed(!collapsed);
  };

  return (
    <header>
      <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
        <NavbarBrand tag={Link} to="/">
          <img src={logoImage} alt="FitLog Logo" className="logo-nav" /> {/* Logo image */}
          FitLog.Web
        </NavbarBrand>

        <NavbarToggler onClick={toggleNavbar} className="mr-2" />
        <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar>
          <ul className="navbar-nav flex-grow">
            <NavItem>
              <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
            </NavItem>
            {/* <NavItem>
              <NavLink tag={Link} className="text-dark" to="/counter">Counter</NavLink>
            </NavItem>
            <NavItem>
              <NavLink tag={Link} className="text-dark" to="/fetch-data">Fetch data</NavLink>
            </NavItem> */}
            {isAuthenticated && (
              <>
                {/* <NavItem>
                  <a className="nav-link text-dark" href="/Identity/Account/Manage">Account</a>
                </NavItem> */}
                <UncontrolledDropdown nav inNavbar>
                  <DropdownToggle nav caret className="text-dark">
                    Training
                  </DropdownToggle>
                  <DropdownMenu right>
                    <DropdownItem tag={Link} to="/WorkoutHistory">
                      Workout History
                    </DropdownItem>
                    <DropdownItem tag={Link} to="/ExerciseLog">
                      Workout Log
                    </DropdownItem>
                    <DropdownItem tag={Link} to="/profile">
                      Profile
                    </DropdownItem>
                    <DropdownItem tag={Link} to="/changepassword">
                      Change Password
                    </DropdownItem>
                    <DropdownItem tag={Link} to="/trainingBoard">
                      Training Board
                    </DropdownItem>
                  </DropdownMenu>
                </UncontrolledDropdown>
                <UncontrolledDropdown nav inNavbar>
                  <DropdownToggle nav caret className="text-dark">
                    Admin
                  </DropdownToggle>
                  <DropdownMenu right>
                    <DropdownItem tag={Link} to="/MuscleGroup">
                      Muscle Group
                    </DropdownItem>
                    <DropdownItem tag={Link} to="/admin/management/equipments">
                      Equipments List
                    </DropdownItem>
                    <DropdownItem tag={Link} to="/users-list">
                      User List
                    </DropdownItem>
                  </DropdownMenu>
                </UncontrolledDropdown>
                <NavItem>
                  <NavLink className="text-dark" href="#">
                    Notifications <Badge color="secondary">{notifications.length}</Badge>
                  </NavLink>
                </NavItem>
                <NavItem>
                  <button className="btn btn-link nav-link text-dark" onClick={logout}>Logout</button>
                </NavItem>
              </>
            )}
            {!isAuthenticated && (
              <>
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/login">Login</NavLink>
                </NavItem>
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/register">Register</NavLink>
                </NavItem>
              </>
            )}
          </ul>
        </Collapse>
      </Navbar>
    </header>
  );
};
