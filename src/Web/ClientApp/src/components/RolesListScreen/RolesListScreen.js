import React, { useState, useEffect } from 'react';
import { Table, Button, Container, Row, Col, Form, FormGroup, Label, Input, Modal, ModalHeader, ModalBody, ModalFooter, Alert } from 'reactstrap';
import axiosInstance from '../../utils/axiosInstance';
import './RolesListScreen.css';

const RolesList = () => {
  const [roles, setRoles] = useState([]);
  const [newRole, setNewRole] = useState({ roleName: '', roleDesc: '' });
  const [updateRole, setUpdateRole] = useState({ roleId: '', roleDescription: '' });
  const [createModal, setCreateModal] = useState(false);
  const [updateModal, setUpdateModal] = useState(false);
  const [confirmUpdateModal, setConfirmUpdateModal] = useState(false);
  const [currentRoleId, setCurrentRoleId] = useState(null);
  const [createErrors, setCreateErrors] = useState({ roleName: '', roleDesc: '' });
  const [updateErrors, setUpdateErrors] = useState({ roleDesc: '' });
  const [successModal, setSuccessModal] = useState(false);
  const [successMessage, setSuccessMessage] = useState('');

  useEffect(() => {
    const fetchRoles = async () => {
      try {
        const response = await axiosInstance.get('/Roles');
        setRoles(response.data.sort((a, b) => a.name.localeCompare(b.name)));
      } catch (error) {
        console.error('Error fetching roles:', error);
      }
    };

    fetchRoles();
  }, []);

  const toggleCreateModal = () => {
    if (createModal) {
      setNewRole({ roleName: '', roleDesc: '' });
    }
    setCreateErrors({ roleName: '', roleDesc: '' });
    setCreateModal(!createModal);
  };

  const toggleUpdateModal = () => {
    setUpdateErrors({ roleDesc: '' });
    setUpdateModal(!updateModal);
  };

  const toggleSuccessModal = () => {
    setSuccessModal(!successModal);
  };

  const toggleConfirmUpdateModal = () => {
    setConfirmUpdateModal(!confirmUpdateModal);
  };

  const handleInputChange = (e, setRole) => {
    const { name, value } = e.target;
    setRole(prevState => ({ ...prevState, [name]: value }));
  };

  const validateCreateForm = () => {
    let errors = { roleName: '', roleDesc: '' };
    let isValid = true;
    if (!newRole.roleName) {
      errors.roleName = 'Role Name cannot be empty!';
      isValid = false;
    } else if (roles.some(role => role.name.toLowerCase() === newRole.roleName.toLowerCase())) {
      errors.roleName = 'Role Name already exists!';
      isValid = false;
    }
    if (!newRole.roleDesc) {
      errors.roleDesc = 'Role Description cannot be empty!';
      isValid = false;
    }
    setCreateErrors(errors);
    return isValid;
  };

  const validateUpdateForm = () => {
    let errors = { roleDesc: '' };
    let isValid = true;
    if (!updateRole.roleDescription) {
      errors.roleDesc = 'Role Description cannot be empty!';
      isValid = false;
    }
    setUpdateErrors(errors);
    return isValid;
  };

  const handleCreateRole = async () => {
    if (validateCreateForm()) {
      try {
        await axiosInstance.post('/Roles', newRole);
        const response = await axiosInstance.get('/Roles');
        setRoles(response.data.sort((a, b) => a.name.localeCompare(b.name)));
        toggleCreateModal();
        setSuccessMessage('Role Name Created successfully');
        toggleSuccessModal();
        setNewRole({ roleName: '', roleDesc: '' });
      } catch (error) {
        console.error('Error creating role:', error);
      }
    }
  };

  const handleUpdateRole = async () => {
    if (validateUpdateForm()) {
      try {
        const updateData = {
          roleId: currentRoleId,
          roleDescription: updateRole.roleDescription
        };
        await axiosInstance.put(`/Roles/${currentRoleId}`, updateData);
        const response = await axiosInstance.get('/Roles');
        setRoles(response.data.sort((a, b) => a.name.localeCompare(b.name)));
        toggleUpdateModal();
        setSuccessMessage('Role Description Updated successfully');
        toggleSuccessModal();
        setUpdateRole({ roleId: '', roleDescription: '' });
        setCurrentRoleId(null);
      } catch (error) {
        console.error('Error updating role:', error);
      }
    }
  };

  const confirmUpdateRole = () => {
    toggleConfirmUpdateModal();
    handleUpdateRole();
  };

  const openUpdateModal = (role) => {
    setUpdateRole({ roleId: role.id, roleDescription: role.roleDesc });
    setCurrentRoleId(role.id);
    toggleUpdateModal();
  };

  return (
    <Container className="roles-list-container">
      <h2 className="my-title"><strong>Roles List</strong></h2>
      <Row className="roles-list-header">
        <Col className="text-right">
          <Button color="primary" className="create-role-button" onClick={toggleCreateModal}>Create Role</Button>
        </Col>
      </Row>
      <Table className="roles-table" bordered>
        <thead>
          <tr>
            <th>Role Name</th>
            <th>Description</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {roles.map((role) => (
            <tr key={role.id}>
              <td>{role.name}</td>
              <td>{role.roleDesc}</td>
              <td>
                <Button color="success" className="update-role-button" onClick={() => openUpdateModal(role)}>Update</Button>
                {/*{' '}*/}
                {/*<Button color="danger">Delete</Button>*/}
              </td>
            </tr>
          ))}
        </tbody>
      </Table>

      {/* Create Role Modal */}
      <Modal isOpen={createModal} toggle={toggleCreateModal}>
        <ModalHeader toggle={toggleCreateModal}>Create Role</ModalHeader>
        <ModalBody>
          <Form>
            <FormGroup>
              <Label for="roleName">Role Name <span style={{ color: 'red' }}>*</span></Label>
              <Input
                type="text"
                name="roleName"
                id="roleName"
                value={newRole.roleName}
                onChange={(e) => handleInputChange(e, setNewRole)}
              />
              {createErrors.roleName && <Alert color="danger">{createErrors.roleName}</Alert>}
            </FormGroup>
            <FormGroup>
              <Label for="roleDesc">Role Description <span style={{ color: 'red' }}>*</span></Label>
              <Input
                type="text"
                name="roleDesc"
                id="roleDesc"
                value={newRole.roleDesc}
                onChange={(e) => handleInputChange(e, setNewRole)}
              />
              {createErrors.roleDesc && <Alert color="danger">{createErrors.roleDesc}</Alert>}
            </FormGroup>
          </Form>
        </ModalBody>
        <ModalFooter>
          <Button color="primary" onClick={handleCreateRole}>Create</Button>{' '}
          <Button color="danger" className="cancel-role-button" onClick={toggleCreateModal}>Cancel</Button>
        </ModalFooter>
      </Modal>

      {/* Update Role Modal */}
      <Modal isOpen={updateModal} toggle={toggleUpdateModal}>
        <ModalHeader toggle={toggleUpdateModal}>Update Role</ModalHeader>
        <ModalBody>
          <Form>
            <FormGroup>
              <Label for="roleId">Role ID</Label>
              <Input
                type="text"
                name="roleId"
                id="roleId"
                value={updateRole.roleId}
                disabled
              />
            </FormGroup>
            <FormGroup>
              <Label for="roleDescription">Role Description <span style={{ color: 'red' }}>*</span></Label>
              <Input
                type="text"
                name="roleDescription"
                id="roleDescription"
                value={updateRole.roleDescription}
                onChange={(e) => handleInputChange(e, setUpdateRole)}
              />
              {updateErrors.roleDesc && <Alert color="danger">{updateErrors.roleDesc}</Alert>}
            </FormGroup>
          </Form>
        </ModalBody>
        <ModalFooter>
          <Button color="primary" onClick={toggleConfirmUpdateModal}>Update</Button>{' '}
          <Button color="danger" className="cancel-role-button" onClick={toggleUpdateModal}>Cancel</Button>
        </ModalFooter>
      </Modal>

      {/* Confirm Update Modal */}
      <Modal isOpen={confirmUpdateModal} toggle={toggleConfirmUpdateModal}>
        <ModalHeader toggle={toggleConfirmUpdateModal}>Confirmation</ModalHeader>
        <ModalBody>
          Are you sure you want to update this role?
        </ModalBody>
        <ModalFooter>
          <Button color="primary" onClick={confirmUpdateRole}>Yes</Button>{' '}
          <Button color="danger" onClick={toggleConfirmUpdateModal}>No</Button>
        </ModalFooter>
      </Modal>

      {/* Success Modal */}
      <Modal isOpen={successModal} toggle={toggleSuccessModal}>
        <ModalHeader toggle={toggleSuccessModal}>Success</ModalHeader>
        <ModalBody>
          {successMessage}
        </ModalBody>
        <ModalFooter>
          <Button color="success" onClick={toggleSuccessModal}>OK</Button>
        </ModalFooter>
      </Modal>
    </Container>
  );
};

export default RolesList;
