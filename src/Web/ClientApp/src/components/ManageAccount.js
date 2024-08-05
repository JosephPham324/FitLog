import React, { useState, useEffect } from 'react';
import { Table, Button, Container, Input, Row, Col, Form, FormGroup, Label, Modal, ModalHeader, ModalBody, ModalFooter, Pagination, PaginationItem, PaginationLink } from 'reactstrap';
import axiosInstance from '../utils/axiosInstance'; // Import the configured Axios instance
import './ManageAccount.css'; // Import the CSS file

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
            <Button
              color="danger"
              className="mr-2 delete-btn"
              onClick={() => deleteUser(user.id)}
            >
              Delete
            </Button>
          </div>
        </td>
      </tr>
    ));
  };

  const renderPaginationItems = () => {
    const items = [];
    const maxPagesToShow = 3; // Maximum number of page links to display

    // First page
    items.push(
      <PaginationItem key={1} active={currentPage === 1}>
        <PaginationLink onClick={() => setCurrentPage(1)}>1</PaginationLink>
      </PaginationItem>
    );

    // Pages before truncation
    if (currentPage > maxPagesToShow) {
      items.push(
        <PaginationItem key="dots1" disabled>
          <PaginationLink>...</PaginationLink>
        </PaginationItem>
      );
    }

    // Current page, previous and next pages
    const startPage = Math.max(2, currentPage);
    const endPage = Math.min(totalPages - 1, currentPage + 1);
    for (let i = startPage; i <= endPage; i++) {
      items.push(
        <PaginationItem key={i} active={currentPage === i}>
          <PaginationLink onClick={() => setCurrentPage(i)}>{i}</PaginationLink>
        </PaginationItem>
      );
    }

    // Pages after truncation
    if (currentPage < totalPages - maxPagesToShow + 1) {
      items.push(
        <PaginationItem key="dots2" disabled>
          <PaginationLink>...</PaginationLink>
        </PaginationItem>
      );
    }

    // Last page
    if (totalPages > 1) {
      items.push(
        <PaginationItem key={totalPages} active={currentPage === totalPages}>
          <PaginationLink onClick={() => setCurrentPage(totalPages)}>{totalPages}</PaginationLink>
        </PaginationItem>
      );
    }

    return items;
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

      <Pagination className="justify-content-center mt-3">
        <PaginationItem disabled={currentPage === 1}>
          <PaginationLink first onClick={() => setCurrentPage(1)} />
        </PaginationItem>
        <PaginationItem disabled={currentPage === 1}>
          <PaginationLink previous onClick={() => setCurrentPage(currentPage - 1)} />
        </PaginationItem>
        {renderPaginationItems()}
        <PaginationItem disabled={currentPage === totalPages}>
          <PaginationLink next onClick={() => setCurrentPage(currentPage + 1)} />
        </PaginationItem>
        <PaginationItem disabled={currentPage === totalPages}>
          <PaginationLink last onClick={() => setCurrentPage(totalPages)} />
        </PaginationItem>
      </Pagination>
    </Container>
  );
}
