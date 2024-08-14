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
import { getUserRole } from '../utils/tokenOperations';

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

  let userRole = getUserRole();
  console.log(userRole);

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
                {(userRole.includes('Member') || userRole.includes('Coach')) && (
                  <>
                <NavItem>
                  <NavLink tag={Link} className="text-white" to="/workout-programs"><b>Programs</b></NavLink>
                </NavItem>
                <NavItem>
                  <NavLink tag={Link} className="text-white" to="/survey"><b>Survey</b></NavLink>
                </NavItem>

                <UncontrolledDropdown nav inNavbar>
                  <DropdownToggle nav caret className="text-white">
                    <b>Training</b>
                  </DropdownToggle>
                  <DropdownMenu right>
                    <DropdownItem tag={Link} to="/workout-history">
                      <b>Workout History</b>
                    </DropdownItem>
                    <DropdownItem tag={Link} to="/workout-log/create">
                      <b>Log Workout</b>
                    </DropdownItem>
                    <UncontrolledDropdown nav inNavbar className="nested-dropdown">
                      <DropdownToggle nav caret className="text-dark">
                        <b>Templates</b>
                      </DropdownToggle>
                      <DropdownMenu right className="centered-dropdown-menu">
                        <DropdownItem tag={Link} to="/workout-templates/">
                          <b>Public</b>
                        </DropdownItem>
                        <DropdownItem tag={Link} to="/workout-templates/private">
                          <b>Private</b>
                        </DropdownItem>
                      </DropdownMenu>
                    </UncontrolledDropdown>
                    <DropdownItem tag={Link} to="/training-board">
                      <b>Training Board</b>
                    </DropdownItem>
                  </DropdownMenu>
                </UncontrolledDropdown>

                <UncontrolledDropdown nav inNavbar>
                  <DropdownToggle nav caret className="text-white">
                    <b>Statistics</b>
                  </DropdownToggle>
                  <DropdownMenu right>
                    <DropdownItem tag={Link} to="/logged-exercises">
                      <b>Logged Exercises</b>
                    </DropdownItem>
                    <DropdownItem tag={Link} to="/workout-log-graphs">
                      <b>Workout Log Graphs</b>
                    </DropdownItem>
                  </DropdownMenu>
                    </UncontrolledDropdown>
                </>
                )}
                {userRole.includes('Administrator') && (
                  <UncontrolledDropdown nav inNavbar>
                    <DropdownToggle nav caret className="text-white">
                      <b>Manage</b>
                    </DropdownToggle>
                    <DropdownMenu right>
                      <DropdownItem tag={Link} to="admin/manage/muscle-groups">
                        <b>Muscle Groups</b>
                      </DropdownItem>
                      <DropdownItem tag={Link} to="/admin/manage/equipments">
                        <b>Equipments</b>
                      </DropdownItem>
                      <DropdownItem tag={Link} to="/admin/manage/exercises">
                        <b>Exercises</b>
                      </DropdownItem>
                      <DropdownItem tag={Link} to="/admin/manage/accounts">
                        <b>Users</b>
                      </DropdownItem>
                    </DropdownMenu>
                  </UncontrolledDropdown>
                )}
               
                <NavItem>
                    <NavLink tag={Link} className="text-white" to="/profile"><b>Profile</b></NavLink>
                </NavItem>
                <NavItem>
                  <NavLink className="text-white-noti" href="#">
                    <b>Notifications</b> <Badge color="secondary">{notifications.length}</Badge>
                  </NavLink>
                </NavItem>
                {(userRole.includes('Member') || userRole.includes('Coach')) && (
                  <UncontrolledDropdown nav inNavbar>
                    <DropdownToggle nav caret className="text-white">
                      <b>Service</b>
                    </DropdownToggle>
                    <DropdownMenu right>
                      <DropdownItem tag={Link} to="/coach-service-booking">
                        <b>Coach Service Booking</b>
                      </DropdownItem>
                    </DropdownMenu>
                  </UncontrolledDropdown>
                )}
                <NavItem>
                  <button
                    className="btn btn-link nav-link text-white"
                    style={{ backgroundColor: '#3971a1' }}
                    onClick={logout}
                  >
                    <b>Logout</b>
                  </button>
                </NavItem>
              </>
            )}
           
            {!isAuthenticated && (
              <>
                <NavItem>
                  <NavLink tag={Link} className="text-white" to="/workout-programs"><b>Programs</b></NavLink>
                </NavItem>
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
