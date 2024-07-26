import React, { useContext, useEffect, useState, useRef } from 'react';
import {
  Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink,
  UncontrolledDropdown, DropdownToggle, DropdownMenu, DropdownItem, Badge
} from 'reactstrap';
import { Link } from 'react-router-dom';
import { HubConnectionBuilder, LogLevel, HttpTransportType } from '@microsoft/signalr';
import './NavMenu.css';
import logoImage from '../assets/Logo.png';
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
      <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow" light>
        <NavbarBrand tag={Link} to="/" className="text-white">
          <img src={logoImage} alt="FitLog Logo" className="logo-nav" /> {/* Logo image */}
          FitLog.Web
        </NavbarBrand>

        <NavbarToggler onClick={toggleNavbar} className="mr-2" />
        <Collapse className="navbar-collapse" isOpen={!collapsed} navbar>
          <ul className="navbar-nav">
            <NavItem>
              <NavLink tag={Link} className="text-white" to="/"><b>Home</b></NavLink>
            </NavItem>
            {isAuthenticated && (
              <>
                <UncontrolledDropdown nav inNavbar>
                  <DropdownToggle nav caret className="text-white">
                    <b>Training</b>
                  </DropdownToggle>
                  <DropdownMenu right>
                    <DropdownItem tag={Link} to="/WorkoutHistory">
                      <b>Workout History</b>
                    </DropdownItem>
                    <DropdownItem tag={Link} to="/ExerciseLog">
                      <b> Workout Log</b>
                    </DropdownItem>
                    <DropdownItem tag={Link} to="/profile">
                      <b> Profile</b>
                    </DropdownItem>
                    <DropdownItem tag={Link} to="/changepassword">
                      <b>Change Password</b>
                    </DropdownItem>
                    <DropdownItem tag={Link} to="/trainingBoard">
                      <b> Training Board</b>
                    </DropdownItem>
                    <DropdownItem tag={Link} to="/CoachServiceBooking">
                      <b> Coach Service Booking</b>
                    </DropdownItem>
                  </DropdownMenu>
                </UncontrolledDropdown>
                <UncontrolledDropdown nav inNavbar>
                  <DropdownToggle nav caret className="text-white">
                    <b> Admin</b>
                  </DropdownToggle>
                  <DropdownMenu right>
                    <DropdownItem tag={Link} to="/MuscleGroup">
                      <b> Muscle Group</b>
                    </DropdownItem>
                    <DropdownItem tag={Link} to="/admin/management/equipments">
                      <b> Equipments List</b>
                    </DropdownItem>
                    <DropdownItem tag={Link} to="/users-list">
                      <b>  User List</b>
                    </DropdownItem>
                    <DropdownItem tag={Link} to="/ManageAccount">
                      <b>  Manage Account</b>
                    </DropdownItem>
                  </DropdownMenu>
                </UncontrolledDropdown>
                <NavItem>
                  <NavLink className="text-white-noti" href="#">
                    <b> Notifications</b> <Badge color="secondary">{notifications.length}</Badge>
                  </NavLink>
                </NavItem>
                <NavItem>
                  <button className="btn btn-link nav-link text-white" onClick={logout}><b>Logout</b></button>
                </NavItem>
              </>
            )}
            {!isAuthenticated && (
              <>
                <NavItem>
                  <NavLink tag={Link} className="text-white" to="/login"><b>Login</b></NavLink>
                </NavItem>
                <NavItem>
                  <NavLink tag={Link} className="text-white" to="/register"><b>Register</b></NavLink>
                </NavItem>
              </>
            )}
          </ul>
        </Collapse>
      </Navbar>
    </header>
  );
};
