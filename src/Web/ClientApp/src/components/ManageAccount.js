import React, { useState, useEffect } from 'react';
import { Table, Button, Container, Input, Row, Col, Form, FormGroup, Label, Modal, ModalHeader, ModalBody, ModalFooter, Pagination, PaginationItem, PaginationLink } from 'reactstrap';
import { toast, ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
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
  const [newFirstName, setNewFirstName] = useState('');
  const [newLastName, setNewLastName] = useState('');
  const [newDateOfBirth, setNewDateOfBirth] = useState('');
  const [newGender, setNewGender] = useState('');
  const [newPhoneNumber, setNewPhoneNumber] = useState('');
  const [createModal, setCreateModal] = useState(false);
  const [updateModal, setUpdateModal] = useState(false);
  const [deleteModal, setDeleteModal] = useState(false);
  const [deleteUserId, setDeleteUserId] = useState(null);
  const [editUserId, setEditUserId] = useState(null);
  const [createErrors, setCreateErrors] = useState({});
  const [updateErrors, setUpdateErrors] = useState({});
  const rowsPerPage = 10;

  useEffect(() => {
    fetchUsers(currentPage, searchTerm);
  }, [currentPage, searchTerm]);

  const fetchUsers = async (page, searchTerm) => {
    try {
      const response = searchTerm
        ? await axiosInstance.get(`${apiUrl}/search-by-username`, {
          params: {
            Username: searchTerm,
          },
        })
        : await axiosInstance.get(`${apiUrl}/all`, {
          params: {
            PageNumber: page,
            PageSize: rowsPerPage,
          },
        });

      setUsers(response.data.items || response.data);
      setTotalPages(response.data.totalPages || 1);
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
      setCreateErrors({});
    }
  };

  const toggleUpdateModal = () => {
    setUpdateModal(!updateModal);
    setUpdateErrors({});
  };

  const toggleDeleteModal = () => {
    setDeleteModal(!deleteModal);
  };

  const validateCreateInputs = () => {
    const errors = {};

    if (!newEmail) {
      errors.newEmail = 'Email is required';
    } else {
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      if (!emailRegex.test(newEmail)) {
        errors.newEmail = 'Invalid email format';
      }
    }

    if (!newPassword) {
      errors.newPassword = 'Password is required';
    } else if (newPassword.length < 8 || !/[!@#$%^&*]/.test(newPassword)) {
      errors.newPassword = 'Password must be at least 8 characters long and include a special character';
    }

    if (!newRole) {
      errors.newRole = 'Role is required';
    } else if (!['Administrator', 'Member', 'Coach'].includes(newRole)) {
    errors.newRole = 'Role must be Administrator, Member, or Coach';
  }

  if (!newUsername) {
    errors.newUsername = 'Username is required';
  } else if (newUsername.length < 3 || newUsername.length > 20) {
    errors.newUsername = 'Username must be between 3 and 20 characters long';
  } else {
    const usernameRegex = /^[a-zA-Z0-9_]+$/;
    if (!usernameRegex.test(newUsername)) {
      errors.newUsername = 'Username can only contain letters, numbers, and underscores';
    }
  }

  setCreateErrors(errors);
  return Object.keys(errors).length === 0;
};
  const validateUpdateInputs = () => {
    const errors = {};

    if (!newFirstName) {
      errors.newFirstName = 'First Name is required';
    } else {
      const nameRegex = /^[a-zA-Z]+$/;
      if (!nameRegex.test(newFirstName)) {
        errors.newFirstName = 'Invalid First Name format';
      }
    }

    if (!newLastName) {
      errors.newLastName = 'Last Name is required';
    } else {
      const nameRegex = /^[a-zA-Z]+$/;
      if (!nameRegex.test(newLastName)) {
        errors.newLastName = 'Invalid Last Name format';
      }
    }

    if (!newPhoneNumber) {
      errors.newPhoneNumber = 'Phone number is required';
    } else if (!/^\d{10}$/.test(newPhoneNumber)) {
      errors.newPhoneNumber = 'Phone number must be 10 digits';
    }

    const today = new Date().toISOString().split('T')[0];
    if (newDateOfBirth && newDateOfBirth > today) {
      errors.newDateOfBirth = 'Date of birth cannot be in the future';
    }

    setUpdateErrors(errors);
    return Object.keys(errors).length === 0;
  };

  const createUser = async () => {
    if (!validateCreateInputs()) return;

    try {
      await axiosInstance.post(`${apiUrl}/create-account`, {
        email: newEmail,
        password: newPassword,
        userName: newUsername,
        role: newRole,
      });

      fetchUsers(currentPage, searchTerm);
      toggleCreateModal();
      toast.success('User created successfully!');
    } catch (error) {
      console.error('Error creating user:', error.message);
      alert('Failed to create user. Please check your input and try again.');
    }
  };

  const updateUser = async () => {
    if (!validateUpdateInputs()) return;

    try {
      await axiosInstance.put(`${apiUrl}/update-profile`, {
        userId: editUserId,
        userName: newUsername,
        email: newEmail,
        firstName: newFirstName,
        lastName: newLastName,
        dateOfBirth: newDateOfBirth,
        gender: newGender,
        phoneNumber: newPhoneNumber,
      });

      fetchUsers(currentPage, searchTerm);
      toggleUpdateModal();
      toast.success('User updated successfully!');
    } catch (error) {
      console.error('Error updating user:', error.message);
      alert('Failed to update user. Please check your input and try again.');
    }
  };

  const deleteUser = async (id) => {
    try {
      await axiosInstance.delete(`${apiUrl}/delete-account/${id}`);

      fetchUsers(currentPage, searchTerm);
      toggleDeleteModal();
      toast.success('User deleted successfully!');
    } catch (error) {
      console.error('Error deleting user:', error.message);
      alert('Failed to delete user. Please try again.');
    }
  };

  const handleEdit = (user) => {
    setNewUsername(user.userName);
    setNewEmail(user.email);
    setNewFirstName(user.firstName);
    setNewLastName(user.lastName);
    setNewDateOfBirth(user.dateOfBirth);
    setNewGender(user.gender);
    setNewPhoneNumber(user.phoneNumber);
    setEditUserId(user.id);
    toggleUpdateModal();
  };

  const handleSearch = (e) => {
    setSearchTerm(e.target.value);
    setCurrentPage(1);
  };

  const handleDeleteClick = (id) => {
    setDeleteUserId(id);
    toggleDeleteModal();
  };

  const renderTableRows = () => {
    return users.map((user, index) => (
      <tr key={user.id}>
        <td>{index + 1}</td> {/* Sequential number starting from 1 */}
        <td>{user.userName}</td>
        <td>{user.email}</td>
        <td>{user.roles.join(', ')}</td> {/* Join roles array into a string */}
        <td>{user.phoneNumber}</td>
        <td>{user.IsRestricted ? 'Restricted' : 'Active'}</td>
        <td>
          <div className="button-group">
            <Button
              color="success"
              style={{ height: '38px' }}
              className="mr-2 update-btn"
              onClick={() => handleEdit(user)}
            >
              Update
            </Button>
            <Button
              color="danger"
              className="mr-2 delete-btn"

            

              onClick={() => deleteUser(user.templateId)}

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
      <ToastContainer />
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
              {createErrors.newEmail && <small className="text-danger">{createErrors.newEmail}</small>}
            </FormGroup>
            <FormGroup>
              <Label for="newPassword">Password</Label>
              <Input
                type="password"
                id="newPassword"
                value={newPassword}
                onChange={(e) => setNewPassword(e.target.value)}
              />
              {createErrors.newPassword && <small className="text-danger">{createErrors.newPassword}</small>}
            </FormGroup>
            <FormGroup>
              <Label for="newUsername">Username</Label>
              <Input
                type="text"
                id="newUsername"
                value={newUsername}
                onChange={(e) => setNewUsername(e.target.value)}
              />
              {createErrors.newUsername && <small className="text-danger">{createErrors.newUsername}</small>}
            </FormGroup>
            <FormGroup>
              <Label for="newRole">Role</Label>
              <Input
                type="text"
                id="newRole"
                value={newRole}
                onChange={(e) => setNewRole(e.target.value)}
              />
              {createErrors.newRole && <small className="text-danger">{createErrors.newRole}</small>}
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
                readOnly // Make this field read-only
              />
            </FormGroup>
            <FormGroup>
              <Label for="editEmail">Email</Label>
              <Input
                type="email"
                id="editEmail"
                value={newEmail}
                onChange={(e) => setNewEmail(e.target.value)}
                readOnly // Make this field read-only
              />
            </FormGroup>
            <FormGroup>
              <Label for="editFirstName">First Name</Label>
              <Input
                type="text"
                id="editFirstName"
                value={newFirstName}
                onChange={(e) => setNewFirstName(e.target.value)}
              />
              {updateErrors.newFirstName && <small className="text-danger">{updateErrors.newFirstName}</small>}
            </FormGroup>

            <FormGroup>
              <Label for="editLastName">Last Name</Label>
              <Input
                type="text"
                id="editLastName"
                value={newLastName}
                onChange={(e) => setNewLastName(e.target.value)}
              />
              {updateErrors.newLastName && <small className="text-danger">{updateErrors.newLastName}</small>}
            </FormGroup>

            <FormGroup>
              <Label for="editDateOfBirth">Date of Birth</Label>
              <Input
                type="date"
                id="editDateOfBirth"
                value={newDateOfBirth}
                onChange={(e) => setNewDateOfBirth(e.target.value)}
              />
              {updateErrors.newDateOfBirth && <small className="text-danger">{updateErrors.newDateOfBirth}</small>}
            </FormGroup>
            
            <FormGroup>
              <Label for="editGender">Gender</Label>
              <Input
                type="select"
                id="editGender"
                value={newGender}
                onChange={(e) => setNewGender(e.target.value)}
              >
                <option value="">Select Gender</option>
                <option value="Other">Other</option>
                <option value="Male">Male</option>
                <option value="Female">Female</option>
              </Input>
            </FormGroup>
            <FormGroup>
              <Label for="editPhoneNumber">Phone Number</Label>
              <Input
                type="text"
                id="editPhoneNumber"
                value={newPhoneNumber}
                onChange={(e) => setNewPhoneNumber(e.target.value)}
              />
              {updateErrors.newPhoneNumber && <small className="text-danger">{updateErrors.newPhoneNumber}</small>}
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

      {/* Delete Confirmation Modal */}
      <Modal isOpen={deleteModal} toggle={toggleDeleteModal}>
        <ModalHeader toggle={toggleDeleteModal}>Confirm Delete</ModalHeader>
        <ModalBody>
          Are you sure you want to delete this user?
        </ModalBody>
        <ModalFooter>
          <Button color="danger" onClick={() => deleteUser(deleteUserId)}>
            Delete
          </Button>
          <Button color="secondary" onClick={toggleDeleteModal}>
            Cancel
          </Button>
        </ModalFooter>
      </Modal>

      <Table striped hover responsive>
        <thead>
          <tr>
            <th>Number</th>
            <th>Username</th>
            <th>Email</th>
            <th>Role</th>
            <th>Phone Number</th>
            <th>Restricted</th>
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
