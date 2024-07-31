import React, { useState, useEffect } from 'react';
import { Table, Button, Container, Input, Row, Col, Form, FormGroup, Label, Modal, ModalHeader, ModalBody, ModalFooter, Alert } from 'reactstrap';
import axiosInstance from '../utils/axiosInstance'; // Import the configured Axios instance
import cloudinary from './cloudinaryConfig'; // Import Cloudinary config
import axios from 'axios'; // Import Axios
import './MuscleGroup.css';

const apiUrl = '/MuscleGroups';

export function MuscleGroup() {
  const [muscleGroups, setMuscleGroups] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [newGroupName, setNewGroupName] = useState('');
  const [newGroupImage, setNewGroupImage] = useState(null);
  const [createModal, setCreateModal] = useState(false);
  const [updateModal, setUpdateModal] = useState(false);
  const [editGroupId, setEditGroupId] = useState(null);
  const [nameErrorMessage, setNameErrorMessage] = useState('');
  const [imageErrorMessage, setImageErrorMessage] = useState('');

  useEffect(() => {
    fetchMuscleGroups(currentPage);
  }, [currentPage]);

  const fetchMuscleGroups = async (page) => {
    try {
      const response = await axiosInstance.get(`${apiUrl}?page=${page}`);
      setMuscleGroups(response.data);
    } catch (error) {
      console.error('Error fetching muscle groups:', error);
    }
  };

  const toggleCreateModal = () => {
    setCreateModal(!createModal);
    if (!createModal) {
      setNewGroupName('');
      setNewGroupImage(null);
      setNameErrorMessage('');
      setImageErrorMessage('');
    }
  };

  const toggleUpdateModal = () => {
    setUpdateModal(!updateModal);
    if (!updateModal) {
      setNewGroupName('');
      setNewGroupImage(null);
      setNameErrorMessage('');
      setImageErrorMessage('');
    }
  };

  const validateInput = () => {
    let valid = true;
    if (!newGroupName.trim()) {
      setNameErrorMessage('Muscle group name is required');
      valid = false;
    } else {
      setNameErrorMessage('');
    }
    if (!newGroupImage) {
      setImageErrorMessage('Muscle group image is required');
      valid = false;
    } else {
      setImageErrorMessage('');
    }
    return valid;
  };

  const uploadImageToCloudinary = async (file) => {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('upload_preset', 'xabhblft'); // Use the correct preset name

    try {
      const response = await axios.post(
        `https://api.cloudinary.com/v1_1/${cloudinary.config().cloud_name}/image/upload`,
        formData,
        {
          withCredentials: false, // Cloudinary does not require credentials
        }
      );
      return response.data.secure_url;
    } catch (error) {
      console.error('Error uploading image to Cloudinary:', error);
      alert('Failed to upload image. Please try again.');
      return null;
    }
  };

  const createMuscleGroup = async () => {
    if (!validateInput()) {
      return;
    }

    const imageUrl = await uploadImageToCloudinary(newGroupImage);
    if (!imageUrl) {
      return;
    }

    try {
      await axiosInstance.post(`${apiUrl}/create`, {
        MuscleGroupName: newGroupName,
        ImageUrl: imageUrl
      });

      fetchMuscleGroups(currentPage);
      toggleCreateModal();
    } catch (error) {
      console.error('Error creating muscle group:', error);
      alert('Failed to create muscle group. Please try again.');
    }
  };

  const updateMuscleGroup = async () => {
    if (!validateInput()) {
      return;
    }

    const imageUrl = await uploadImageToCloudinary(newGroupImage);
    if (!imageUrl) {
      return;
    }

    try {
      await axiosInstance.put(`${apiUrl}/${editGroupId}`, {
        id: editGroupId,
        MuscleGroupName: newGroupName,
        ImageUrl: imageUrl
      });

      fetchMuscleGroups(currentPage);
      toggleUpdateModal();
    } catch (error) {
      console.error('Error updating muscle group:', error);
      alert('Failed to update muscle group. Please try again.');
    }
  };

  const deleteMuscleGroup = async (id) => {
    try {
      await axiosInstance.delete(`${apiUrl}/${id}`, {
        data: {
          id: id,
        },
      });

      fetchMuscleGroups(currentPage);
    } catch (error) {
      console.error('Error deleting muscle group:', error);
      alert('Failed to delete muscle group. Please try again.');
    }
  };

  const handleImageChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setNewGroupImage(file);
    }
  };

  const renderTableRows = () => {
    return muscleGroups.map(group => (
      <tr key={group.muscleGroupId}>
        <td>{group.muscleGroupId}</td>
        <td>{group.muscleGroupName}</td>
        <td>
          <img src={group.imageUrl} alt={group.muscleGroupName} style={{ width: '50px', height: '50px' }} />
        </td>
        <td>
          <div className="button-group">
            <Button color="success" className="mr-2 update-btn" onClick={() => handleEdit(group)}>Update</Button>
            <Button color="danger" className="mr-2 delete-btn" onClick={() => deleteMuscleGroup(group.muscleGroupId)}>Delete</Button>
          </div>
        </td>
      </tr>
    ));
  };

  const handleEdit = (group) => {
    setEditGroupId(group.muscleGroupId);
    setNewGroupName(group.muscleGroupName);
    toggleUpdateModal();
  };

  return (
    <Container>
      <Row>
        <Col>
          <Button color="primary" onClick={toggleCreateModal}>Create New Muscle Group</Button>
          <Table>
            <thead>
              <tr>
                <th>ID</th>
                <th>Name</th>
                <th>Image</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {renderTableRows()}
            </tbody>
          </Table>
        </Col>
      </Row>

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
              <Label for="newGroupImage">Image</Label>
              <Input
                type="file"
                id="newGroupImage"
                accept="image/*"
                onChange={handleImageChange}
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
              <Label for="newGroupImage">Image</Label>
              <Input
                type="file"
                id="newGroupImage"
                accept="image/*"
                onChange={handleImageChange}
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
    </Container>
  );
}
