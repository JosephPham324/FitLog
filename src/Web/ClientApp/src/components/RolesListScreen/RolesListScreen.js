//import React, { useState, useEffect } from 'react';
//import {
//  Button,
//  Container,
//  Input,
//  Row,
//  Col,
//  Form,
//  FormGroup,
//  Label,
//  Modal,
//  ModalHeader,
//  ModalBody,
//  ModalFooter,
//  Table,
//  Alert,
//} from 'reactstrap';
//import axiosInstance from '../../utils/axiosInstance'; // Import axiosInstance from config
//import './RolesListScreen.css';

//const RolesListScreen = () => {
//  const [roles, setRoles] = useState([]);
//  const [showPopup, setShowPopup] = useState(false);
//  const [editingRole, setEditingRole] = useState(null);
//  const [newRole, setNewRole] = useState({ id: '', name: '', des: '' });
//  const [error, setError] = useState('');
//  const [hoveredRole, setHoveredRole] = useState(null);

//  useEffect(() => {
//    fetchRoles();
//  }, []);

//  const fetchRoles = async () => {
//    try {
//      const response = await axiosInstance.get('/Roles');
//      setRoles(response.data);
//    } catch (error) {
//      console.error('Error fetching roles:', error);
//    }
//  };

//  const handleAddRole = () => {
//    setShowPopup(true);
//  };

//  const handleSaveRole = async () => {
//    if (!newRole.name && !newRole.des) {
//      setError('Data fields cannot be left blank');
//      return;
//    } else if (!newRole.name) {
//      setError('The Name field cannot be left blank');
//      return;
//    } else if (!newRole.des) {
//      setError('Des field cannot be left blank');
//      return;
//    }

//    try {
//      if (editingRole) {
//        await axiosInstance.put(`/Roles/${newRole.id}`, newRole);
//        setRoles(roles.map(role => (role.id === editingRole.id ? newRole : role)));
//      } else {
//        const response = await axiosInstance.post('/Roles', newRole);
//        setRoles([...roles, response.data]);
//      }
//      setShowPopup(false);
//      setNewRole({ id: '', name: '', des: '' });
//      setEditingRole(null);
//      setError('');
//    } catch (error) {
//      console.error('Error saving role:', error);
//      setError('An error occurred while saving the role');
//    }
//  };

//  const handleEditRole = (role) => {
//    setEditingRole(role);
//    setNewRole(role);
//    setShowPopup(true);
//  };

//  const handleClosePopup = () => {
//    setShowPopup(false);
//    setNewRole({ id: '', name: '', des: '' });
//    setEditingRole(null);
//    setError('');
//  };

//  const handleMouseEnter = (role) => {
//    setHoveredRole(role);
//  };

//  const handleMouseLeave = () => {
//    setHoveredRole(null);
//  };

//  return (
//    <Container>
//      <div className="header">
//        <h1>Roles List</h1>
//        <Button color="primary" onClick={handleAddRole}>Create Role</Button>
//      </div>
//      <Table striped hover responsive>
//        <thead>
//          <tr>
//            <th>Name</th>
//            <th>Des</th>
//            <th>Action</th>
//          </tr>
//        </thead>
//        <tbody>
//          {roles.map(role => (
//            <tr key={role.id}>
//              <td>{role.name}</td>
//              <td>
//                {role.des}
//                <span
//                  className="info-icon"
//                  onMouseEnter={() => handleMouseEnter(role)}
//                  onMouseLeave={handleMouseLeave}
//                >
//                  &#x26A0;
//                </span>
//                {hoveredRole === role && (
//                  <div className="tooltip">
//                    <p>{role.des}</p>
//                  </div>
//                )}
//              </td>
//              <td>
//                <Button color="success" className="action-buttone" onClick={() => handleEditRole(role)}>Update</Button>
//              </td>
//            </tr>
//          ))}
//        </tbody>
//      </Table>

//      <Modal isOpen={showPopup} toggle={handleClosePopup}>
//        <ModalHeader toggle={handleClosePopup}>{editingRole ? 'Update Role' : 'Create Role'}</ModalHeader>
//        <ModalBody>
//          <Form>
//            {editingRole && (
//              <FormGroup>
//                <Label for="roleId">ID</Label>
//                <Input type="text" id="roleId" value={newRole.id} readOnly />
//              </FormGroup>
//            )}
//            <FormGroup>
//              <Label for="roleName"> Role Name <span style={{ color: 'red' }}>*</span></Label>
//              <Input
//                type="text"
//                id="roleName"
//                placeholder="Name"
//                value={newRole.name}
//                onChange={(e) => setNewRole({ ...newRole, name: e.target.value })}
//              />
//            </FormGroup>
//            <FormGroup>
//              <Label for="roleDes">Des <span style={{ color: 'red' }}>*</span></Label>
//              <Input
//                type="text"
//                id="roleDes"
//                placeholder="Des"
//                value={newRole.des}
//                onChange={(e) => setNewRole({ ...newRole, des: e.target.value })}
//              />
//            </FormGroup>
//            {error && <Alert color="danger">{error}</Alert>}
//          </Form>
//        </ModalBody>
//        <ModalFooter>
//          <Button color="primary" onClick={handleSaveRole}>{editingRole ? 'Update' : 'Create'}</Button>
//          <Button color="danger" onClick={handleClosePopup}>Cancel</Button>
//        </ModalFooter>
//      </Modal>
//    </Container>
//  );
//};

//export default RolesListScreen;



import React, { useState, useEffect } from 'react';
import {
  Button,
  Container,
  Input,
  Row,
  Col,
  Form,
  FormGroup,
  Label,
  Modal,
  ModalHeader,
  ModalBody,
  ModalFooter,
  Table,
  Alert,
} from 'reactstrap';
import axiosInstance from '../../utils/axiosInstance'; // Import axiosInstance from config
import './RolesListScreen.css';

const RolesListScreen = () => {
  const [roles, setRoles] = useState([]);
  const [showCreatePopup, setShowCreatePopup] = useState(false);
  const [showUpdatePopup, setShowUpdatePopup] = useState(false);
  const [editingRole, setEditingRole] = useState(null);
  const [newRole, setNewRole] = useState({ roleName: '', roleDesc: '' });
  const [error, setError] = useState('');
  const [hoveredRole, setHoveredRole] = useState(null);

  useEffect(() => {
    fetchRoles();
  }, []);

  const fetchRoles = async () => {
    try {
      const response = await axiosInstance.get('/Roles');
      setRoles(response.data);
    } catch (error) {
      console.error('Error fetching roles:', error);
    }
  };

  const handleAddRole = () => {
    setShowCreatePopup(true);
  };

  const handleCreateRole = async () => {
    if (!newRole.roleName && !newRole.roleDesc) {
      setError('Data fields cannot be left blank');
      return;
    } else if (!newRole.roleName) {
      setError('The Name field cannot be left blank');
      return;
    } else if (!newRole.roleDesc) {
      setError('Des field cannot be left blank');
      return;
    }

    try {
      const response = await axiosInstance.post('/Roles', newRole);
      setRoles([...roles, response.data]);
      setShowCreatePopup(false);
      setNewRole({ roleName: '', roleDesc: '' });
      setError('');
    } catch (error) {
      console.error('Error saving role:', error);
      setError('An error occurred while saving the role');
    }
  };

  const handleEditRole = (role) => {
    setEditingRole(role);
    setNewRole({ roleName: role.name, roleDesc: role.des });
    setShowUpdatePopup(true);
  };

  const handleUpdateRole = async () => {
    if (!newRole.roleName && !newRole.roleDesc) {
      setError('Data fields cannot be left blank');
      return;
    } else if (!newRole.roleName) {
      setError('The Name field cannot be left blank');
      return;
    } else if (!newRole.roleDesc) {
      setError('Des field cannot be left blank');
      return;
    }

    try {
      await axiosInstance.put(`/Roles/${editingRole.id}`, newRole);
      setRoles(roles.map(role => (role.id === editingRole.id ? { ...editingRole, name: newRole.roleName, des: newRole.roleDesc } : role)));
      setShowUpdatePopup(false);
      setNewRole({ roleName: '', roleDesc: '' });
      setEditingRole(null);
      setError('');
    } catch (error) {
      console.error('Error updating role:', error);
      setError('An error occurred while updating the role');
    }
  };

  const handleCloseCreatePopup = () => {
    setShowCreatePopup(false);
    setNewRole({ roleName: '', roleDesc: '' });
    setError('');
  };

  const handleCloseUpdatePopup = () => {
    setShowUpdatePopup(false);
    setNewRole({ roleName: '', roleDesc: '' });
    setEditingRole(null);
    setError('');
  };

  const handleMouseEnter = (role) => {
    setHoveredRole(role);
  };

  const handleMouseLeave = () => {
    setHoveredRole(null);
  };

  return (
    <Container>
      <div className="header">
        <h1>Roles List</h1>
        <Button color="primary" onClick={handleAddRole}>Create Role</Button>
      </div>
      <Table striped hover responsive>
        <thead>
          <tr>
            <th>Name</th>
            <th>Des</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {roles.map(role => (
            <tr key={role.id}>
              <td>{role.name}</td>
              <td>
                {role.des}
                <span
                  className="info-icon"
                  onMouseEnter={() => handleMouseEnter(role)}
                  onMouseLeave={handleMouseLeave}
                >
                  &#x26A0;
                </span>
                {hoveredRole === role && (
                  <div className="tooltip">
                    <p>{role.des}</p>
                  </div>
                )}
              </td>
              <td>
                <Button color="success" className="action-buttone" onClick={() => handleEditRole(role)}>Update</Button>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>

      {/* Create Role Modal */}
      <Modal isOpen={showCreatePopup} toggle={handleCloseCreatePopup}>
        <ModalHeader toggle={handleCloseCreatePopup}>Create Role</ModalHeader>
        <ModalBody>
          <Form>
            <FormGroup>
              <Label for="roleName">Role Name <span style={{ color: 'red' }}>*</span></Label>
              <Input
                type="text"
                id="roleName"
                placeholder="Name"
                value={newRole.roleName}
                onChange={(e) => setNewRole({ ...newRole, roleName: e.target.value })}
              />
            </FormGroup>
            <FormGroup>
              <Label for="roleDesc">Des <span style={{ color: 'red' }}>*</span></Label>
              <Input
                type="text"
                id="roleDesc"
                placeholder="Des"
                value={newRole.roleDesc}
                onChange={(e) => setNewRole({ ...newRole, roleDesc: e.target.value })}
              />
            </FormGroup>
            {error && <Alert color="danger">{error}</Alert>}
          </Form>
        </ModalBody>
        <ModalFooter>
          <Button color="primary" onClick={handleCreateRole}>Create</Button>
          <Button color="danger" onClick={handleCloseCreatePopup}>Cancel</Button>
        </ModalFooter>
      </Modal>

      {/* Update Role Modal */}
      <Modal isOpen={showUpdatePopup} toggle={handleCloseUpdatePopup}>
        <ModalHeader toggle={handleCloseUpdatePopup}>Update Role</ModalHeader>
        <ModalBody>
          <Form>
            <FormGroup>
              <Label for="roleName">Role Name <span style={{ color: 'red' }}>*</span></Label>
              <Input
                type="text"
                id="roleName"
                placeholder="Name"
                value={newRole.roleName}
                onChange={(e) => setNewRole({ ...newRole, roleName: e.target.value })}
              />
            </FormGroup>
            <FormGroup>
              <Label for="roleDesc">Des <span style={{ color: 'red' }}>*</span></Label>
              <Input
                type="text"
                id="roleDesc"
                placeholder="Des"
                value={newRole.roleDesc}
                onChange={(e) => setNewRole({ ...newRole, roleDesc: e.target.value })}
              />
            </FormGroup>
            {error && <Alert color="danger">{error}</Alert>}
          </Form>
        </ModalBody>
        <ModalFooter>
          <Button color="primary" onClick={handleUpdateRole}>Update</Button>
          <Button color="danger" onClick={handleCloseUpdatePopup}>Cancel</Button>
        </ModalFooter>
      </Modal>
    </Container>
  );
};

export default RolesListScreen;
