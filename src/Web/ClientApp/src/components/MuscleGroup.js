import React, { useState, useEffect } from 'react';
import { Table, Button, Container, Input, Row, Col, Form, FormGroup, Label, Modal, ModalHeader, ModalBody, ModalFooter, Alert } from 'reactstrap';
import axiosInstance from '../utils/axiosInstance'; // Import the configured Axios instance
import './MuscleGroup.css';

const apiUrl = '/MuscleGroups';

export function MuscleGroup() {
  const [muscleGroups, setMuscleGroups] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [newGroupName, setNewGroupName] = useState('');
  const [newGroupImage, setNewGroupImage] = useState('');
  const [createModal, setCreateModal] = useState(false);
  const [updateModal, setUpdateModal] = useState(false);
  const [editGroupId, setEditGroupId] = useState(null);
  const [totalPages, setTotalPages] = useState(1);
  const [searchTerm, setSearchTerm] = useState('');
  const [nameErrorMessage, setNameErrorMessage] = useState('');
  const [imageErrorMessage, setImageErrorMessage] = useState('');
  const rowsPerPage = 5;

  useEffect(() => {
    fetchMuscleGroups(currentPage, searchTerm);
  }, [currentPage, searchTerm]);

  const fetchMuscleGroups = async (page, searchTerm) => {
    try {
      const response = await axiosInstance.get(`${apiUrl}/get-list`, {
        params: {
          PageNumber: page,
          PageSize: rowsPerPage,
          searchTerm: searchTerm
        }
      });
      setMuscleGroups(response.data.items);
      setTotalPages(response.data.totalPages);
    } catch (error) {
      console.error('Error fetching muscle groups:', error);
    }
  };

  const toggleCreateModal = () => {
    setCreateModal(!createModal);
    if (!createModal) {
      setNewGroupName('');
      setNewGroupImage('');
      setNameErrorMessage('');
      setImageErrorMessage('');
    }
  };

  const toggleUpdateModal = () => {
    setUpdateModal(!updateModal);
    if (!updateModal) {
      setNameErrorMessage('');
      setImageErrorMessage('');
    }
  };

  const validateInput = () => {
    let valid = true;
    if (!newGroupName) {
      setNameErrorMessage('Muscle group name is required');
      valid = false;
    } else {
      setNameErrorMessage('');
    }
    if (!newGroupImage) {
      setImageErrorMessage('Muscle group image URL is required');
      valid = false;
    } else {
      setImageErrorMessage('');
    }
    return valid;
  };

  const createMuscleGroup = async () => {
    if (!validateInput()) {
      return;
    }

    try {
      await axiosInstance.post(`${apiUrl}/create`, {
        MuscleGroupName: newGroupName,
        ImageUrl: newGroupImage
      });

      fetchMuscleGroups(currentPage, searchTerm);
      toggleCreateModal();
    } catch (error) {
      console.error('Error creating muscle group:', error.message);
      alert('Failed to create muscle group. Please check your input and try again.');
    }
  };

  const updateMuscleGroup = async () => {
    if (!window.confirm('Are you sure you want to update this muscle group?')) {
      return;
    }

    if (!validateInput() || !editGroupId) {
      return;
    }

    try {
      await axiosInstance.put(`${apiUrl}/${editGroupId}`, {
        id: editGroupId,
        MuscleGroupName: newGroupName,
        ImageUrl: newGroupImage
      });

      fetchMuscleGroups(currentPage, searchTerm);
      toggleUpdateModal();
    } catch (error) {
      console.error('Error updating muscle group:', error.message);
      alert('Failed to update muscle group. Please check your input and try again.');
    }
  };

  const deleteMuscleGroup = async (id) => {
    if (!window.confirm('Are you sure you want to delete this muscle group?')) {
      return;
    }

    try {
      await axiosInstance.delete(`${apiUrl}/${id}`, {
        data: {
          "id": id,
        },
      });

      fetchMuscleGroups(currentPage, searchTerm);
    } catch (error) {
      console.error('Error deleting muscle group:', error.message);
      alert('Failed to delete muscle group. Please try again.');
    }
  };

  const handleEdit = (group) => {
    setNewGroupName(group.muscleGroupName);
    setNewGroupImage(group.imageUrl);
    setEditGroupId(group.muscleGroupId);
    toggleUpdateModal();
  };

  const handleSearch = (e) => {
    setSearchTerm(e.target.value);
    setCurrentPage(1);
  };

  const renderTableRows = () => {
    return muscleGroups.map(group => (
      <tr key={group.muscleGroupId}>
        <td>{group.muscleGroupId}</td>
        <td>{group.muscleGroupName}</td>
        <td>{group.imageUrl && <img src={group.imageUrl} alt={group.muscleGroupName} className="table-image" />}</td>
        <td>
          <div className="button-group">
            <Button color="primary" className="mr-2 update-btn" onClick={() => handleEdit(group)}>Update</Button>
            <Button color="danger" className="mr-2 delete-btn" onClick={() => deleteMuscleGroup(group.muscleGroupId)}>Delete</Button>
          </div>
        </td>
      </tr>
    ));
  };

  return (
    <Container>
      <h1 className="my-4">Muscle Groups</h1>
      <Row>
        <Col md="6">
          <Input
            type="text"
            placeholder="Search muscle group..."
            value={searchTerm}
            onChange={handleSearch}
            className="btn-search"
          />
        </Col>
        <Col md="6" className="text-right">
          <Button color="primary" onClick={toggleCreateModal} className="btn-create">Create Muscle Group</Button>
        </Col>
      </Row>

      {/* Create Muscle Group Modal */}
      <Modal isOpen={createModal} toggle={toggleCreateModal}>
        <ModalHeader toggle={toggleCreateModal}>Create Muscle Group</ModalHeader>
        <ModalBody>
          <Form>
            <FormGroup>
              <Label for="newGroupName">Name</Label>
              <Input
                type="text"
                id="newGroupName"
                value={newGroupName}
                onChange={(e) => setNewGroupName(e.target.value)}
              />
              {nameErrorMessage && <Alert color="danger">{nameErrorMessage}</Alert>}
            </FormGroup>
            <FormGroup>
              <Label for="newGroupImage">Image URL</Label>
              <Input
                type="text"
                id="newGroupImage"
                value={newGroupImage}
                onChange={(e) => setNewGroupImage(e.target.value)}
              />
              {imageErrorMessage && <Alert color="danger">{imageErrorMessage}</Alert>}
            </FormGroup>
          </Form>
        </ModalBody>
        <ModalFooter>
          <Button color="primary" onClick={createMuscleGroup}>Create</Button>
          <Button color="secondary" onClick={toggleCreateModal}>Cancel</Button>
        </ModalFooter>
      </Modal>

      {/* Update Muscle Group Modal */}
      <Modal isOpen={updateModal} toggle={toggleUpdateModal}>
        <ModalHeader toggle={toggleUpdateModal}>Update Muscle Group</ModalHeader>
        <ModalBody>
          <Form>
            <FormGroup>
              <Label for="newGroupName">Name</Label>
              <Input
                type="text"
                id="newGroupName"
                value={newGroupName}
                onChange={(e) => setNewGroupName(e.target.value)}
              />
              {nameErrorMessage && <Alert color="danger">{nameErrorMessage}</Alert>}
            </FormGroup>
            <FormGroup>
              <Label for="newGroupImage">Image URL</Label>
              <Input
                type="text"
                id="newGroupImage"
                value={newGroupImage}
                onChange={(e) => setNewGroupImage(e.target.value)}
              />
              {imageErrorMessage && <Alert color="danger">{imageErrorMessage}</Alert>}
            </FormGroup>
          </Form>
        </ModalBody>
        <ModalFooter>
          <Button color="primary" onClick={updateMuscleGroup}>Update</Button>
          <Button color="secondary" onClick={toggleUpdateModal}>Cancel</Button>
        </ModalFooter>
      </Modal>

      <Table striped hover>
        <thead>
          <tr>
            <th>Muscle Group ID</th>
            <th>Muscle Group Name</th>
            <th>Image</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {renderTableRows()}
        </tbody>
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
