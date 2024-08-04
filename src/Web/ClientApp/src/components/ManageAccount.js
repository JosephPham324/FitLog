import React, { useState, useEffect } from 'react';
import { Table, Button, Container, Input, Row, Col, Form, FormGroup, Label, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import axiosInstance from '../utils/axiosInstance'; // Import the configured Axios instance

const apiUrl = process.env.REACT_APP_BACKEND_URL + '/Users';

export function ManageAccount() {
  const [users, setUsers] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [searchTerm, setSearchTerm] = useState('');
  const [newUsername, setNewUsername] = useState('');
  const [newEmail, setNewEmail] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [newRole, setNewRole] = useState('');
  const [createModal, setCreateModal] = useState(false);
  const [updateModal, setUpdateModal] = useState(false);
  const [editUserId, setEditUserId] = useState(null);
  const rowsPerPage = 5;

  useEffect(() => {
    fetchUsers(currentPage, searchTerm);
  }, [currentPage, searchTerm]);

  const fetchUsers = async (page, searchTerm) => {
    try {
      const response = await axiosInstance.get(`${apiUrl}/all`, {
        params: {
          PageNumber: page,
          PageSize: rowsPerPage,
          searchTerm: searchTerm,
        },
      });
      setUsers(response.data.items);
      setTotalPages(response.data.totalPages);
    } catch (error) {
      console.error('Error fetching users:', error);
    }
  };

  const toggleCreateModal = () => {
    setCreateModal(!createModal);
    if (!createModal) {
      setNewUsername('');
      setNewEmail('');
      setNewPassword('');
      setNewRole('');
    }
  };

  const toggleUpdateModal = () => {
    setUpdateModal(!updateModal);
  };

  const createUser = async () => {
    try {
      if (!newUsername || !newEmail || !newPassword || !newRole) {
        throw new Error('All fields are required');
      }

      await axiosInstance.post(`${apiUrl}/create-account`, {
        email: newEmail,
        password: newPassword,
        userName: newUsername,
        role: newRole,
      });

      fetchUsers(currentPage, searchTerm);
      toggleCreateModal();
    } catch (error) {
      console.error('Error creating user:', error.message);
      alert('Failed to create user. Please check your input and try again.');
    }
  };

  const updateUser = async () => {
    try {
      if (!newUsername || !editUserId) {
        throw new Error('Username and User ID are required');
      }

      await axiosInstance.put(`${apiUrl}/${editUserId}`, {
        id: editUserId,
        username: newUsername,
        email: newEmail,
        role: newRole,
      });

      fetchUsers(currentPage, searchTerm);
      toggleUpdateModal();
    } catch (error) {
      console.error('Error updating user:', error.message);
      alert('Failed to update user. Please check your input and try again.');
    }
  };

  const deleteUser = async (id) => {
    try {
      await axiosInstance.delete(`${apiUrl}/delete-account/${id}`, {
        data: {
          id: id,
        },
      });

      fetchUsers(currentPage, searchTerm);
    } catch (error) {
      console.error('Error deleting user:', error.message);
      alert('Failed to delete user. Please try again.');
    }
  };

  const handleEdit = (user) => {
    setNewUsername(user.username);
    setNewEmail(user.email);
    setNewRole(user.role);
    setEditUserId(user.id);
    toggleUpdateModal();
  };

  const handleSearch = (e) => {
    setSearchTerm(e.target.value);
    setCurrentPage(1);
  };

  const renderTableRows = () => {
    return users.map((user) => (
      <tr key={user.id}>
        <td>{user.id}</td>
        <td>{user.userName}</td>
        <td>{user.email}</td>
        <td>{user.role}</td>
        <td>
          <div className="button-group">
            <Button
              color="success"
              className="mr-2 update-btn"
              onClick={() => handleEdit(user)}
            >
              Update
            </Button>
            {/*            <Button
              color="danger"
              className="mr-2 delete-btn"
              onClick={() => deleteUser(user.id)}
            >
              Delete
            </Button>*/}
          </div>
        </td>
      </tr>
    ));
  };

  return (
    <Container>
      <h1 className="my-4"><strong>Manage Account</strong></h1>
      <Row className="align-items-center mb-3">
        <Col xs="12" md="10" className="mb-3 mb-md-0">
          <Input
            type="text"
            placeholder="Search by username..."
            value={searchTerm}
            onChange={handleSearch}
            className="btn-search"
          />
        </Col>
        <Col xs="12" md="2" className="text-md-right">
          <Button color="primary" onClick={toggleCreateModal} className="btn-create">
            Create User
          </Button>
        </Col>
      </Row>

      {/* Create User Modal */}
      <Modal isOpen={createModal} toggle={toggleCreateModal}>
        <ModalHeader toggle={toggleCreateModal}>Create User</ModalHeader>
        <ModalBody>
          <Form>
            <FormGroup>
              <Label for="newEmail">Email</Label>
              <Input
                type="email"
                id="newEmail"
                value={newEmail}
                onChange={(e) => setNewEmail(e.target.value)}
              />
            </FormGroup>
            <FormGroup>
              <Label for="newPassword">Password</Label>
              <Input
                type="password"
                id="newPassword"
                value={newPassword}
                onChange={(e) => setNewPassword(e.target.value)}
              />
            </FormGroup>
            <FormGroup>
              <Label for="newUsername">Username</Label>
              <Input
                type="text"
                id="newUsername"
                value={newUsername}
                onChange={(e) => setNewUsername(e.target.value)}
              />
            </FormGroup>
            <FormGroup>
              <Label for="newRole">Role</Label>
              <Input
                type="text"
                id="newRole"
                value={newRole}
                onChange={(e) => setNewRole(e.target.value)}
              />
            </FormGroup>
          </Form>
        </ModalBody>
        <ModalFooter>
          <Button color="primary" onClick={createUser}>
            Create
          </Button>
          <Button color="danger" onClick={toggleCreateModal}>
            Cancel
          </Button>
        </ModalFooter>
      </Modal>

      {/* Update User Modal */}
      <Modal isOpen={updateModal} toggle={toggleUpdateModal}>
        <ModalHeader toggle={toggleUpdateModal}>Update User</ModalHeader>
        <ModalBody>
          <Form>
            <FormGroup>
              <Label for="editUsername">Username</Label>
              <Input
                type="text"
                id="editUsername"
                value={newUsername}
                onChange={(e) => setNewUsername(e.target.value)}
              />
            </FormGroup>
            <FormGroup>
              <Label for="editEmail">Email</Label>
              <Input
                type="email"
                id="editEmail"
                value={newEmail}
                onChange={(e) => setNewEmail(e.target.value)}
              />
            </FormGroup>
            <FormGroup>
              <Label for="editRole">Role</Label>
              <Input
                type="text"
                id="editRole"
                value={newRole}
                onChange={(e) => setNewRole(e.target.value)}
              />
            </FormGroup>
          </Form>
        </ModalBody>
        <ModalFooter>
          <Button color="primary" onClick={updateUser}>
            Update
          </Button>
          <Button color="danger" onClick={toggleUpdateModal}>
            Cancel
          </Button>
        </ModalFooter>
      </Modal>

      <Table striped hover responsive>
        <thead>
          <tr>
            <th>User ID</th>
            <th>Username</th>
            <th>Email</th>
            <th>Role</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>{renderTableRows()}</tbody>
      </Table>

      <div className="pagination">
        <Button
          className="pre"
          color="primary"
          size="sm"
          disabled={currentPage === 1}
          onClick={() => setCurrentPage(currentPage - 1)}
        >
          Previous
        </Button>
        <span className="mx-2">
          Page {currentPage} of {totalPages}
        </span>
        <Button
          className="next"
          color="primary"
          size="sm"
          disabled={currentPage === totalPages}
          onClick={() => setCurrentPage(currentPage + 1)}
        >
          Next
        </Button>
      </div>
    </Container>
  );
}
