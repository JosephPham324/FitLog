import React, { useState, useEffect } from 'react';
import { Table, Button, Container, Input, Row, Col, Form, FormGroup, Label, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import './MuscleGroup.css';

const apiUrl = 'https://localhost:44447/api/MuscleGroups';

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
  const rowsPerPage = 5;

  useEffect(() => {
    fetchMuscleGroups(currentPage, searchTerm);
  }, [currentPage, searchTerm]);

  const fetchMuscleGroups = async (page, searchTerm) => {
    try {
      const response = await fetch(`${apiUrl}/get-list?PageNumber=${page}&PageSize=${rowsPerPage}&searchTerm=${searchTerm}`);
      if (!response.ok) {
        throw new Error('Failed to fetch muscle groups');
      }
      const data = await response.json();
      setMuscleGroups(data.items);
      setTotalPages(data.totalPages);
    } catch (error) {
      console.error('Error fetching muscle groups:', error);
    }
  };

  const toggleCreateModal = () => {
    setCreateModal(!createModal);
    if (!createModal) {
      setNewGroupName('');
      setNewGroupImage('');
    }
  };

  const toggleUpdateModal = () => {
    setUpdateModal(!updateModal);
  };

  const createMuscleGroup = async () => {
    try {
      if (!newGroupName) {
        throw new Error('Muscle group name is required');
      }

      const response = await fetch(`${apiUrl}/create`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          MuscleGroupName: newGroupName,
          ImageUrl: newGroupImage
        })
      });

      if (!response.ok) {
        throw new Error('Failed to create muscle group');
      }

      fetchMuscleGroups(currentPage, searchTerm);
      toggleCreateModal();
    } catch (error) {
      console.error('Error creating muscle group:', error.message);
      alert('Failed to create muscle group. Please check your input and try again.');
    }
  };

  const updateMuscleGroup = async () => {
    try {
      if (!newGroupName || !editGroupId) {
        throw new Error('Muscle group name and ID are required');
      }

      const response = await fetch(`${apiUrl}/${editGroupId}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          id: editGroupId,
          MuscleGroupName: newGroupName,
          ImageUrl: newGroupImage
        })
      });

      if (!response.ok) {
        throw new Error('Failed to update muscle group');
      }

      fetchMuscleGroups(currentPage, searchTerm);
      toggleUpdateModal();
    } catch (error) {
      console.error('Error updating muscle group:', error.message);
      alert('Failed to update muscle group. Please check your input and try again.');
    }
  };

  const deleteMuscleGroup = async (id) => {
    try {
      const response = await fetch(`${apiUrl}/${id}`, {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          id: id,
        })
      });

      if (!response.ok) {
        throw new Error('Failed to delete muscle group');
      }

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
            </FormGroup>
            <FormGroup>
              <Label for="newGroupImage">Image URL</Label>
              <Input
                type="text"
                id="newGroupImage"
                value={newGroupImage}
                onChange={(e) => setNewGroupImage(e.target.value)}
              />
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
            </FormGroup>
            <FormGroup>
              <Label for="newGroupImage">Image URL</Label>
              <Input
                type="text"
                id="newGroupImage"
                value={newGroupImage}
                onChange={(e) => setNewGroupImage(e.target.value)}
              />
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
