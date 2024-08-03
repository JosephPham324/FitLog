import React, { useState, useEffect } from 'react';
import {
  Table,
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
  Alert,
  Pagination,
  PaginationItem,
  PaginationLink
} from 'reactstrap';
import axiosInstance from '../utils/axiosInstance';
import cloudinary from './cloudinaryConfig';
import axios from 'axios';
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
  const [existingImage, setExistingImage] = useState('');
  const [totalPages, setTotalPages] = useState(1);
  const [searchTerm, setSearchTerm] = useState('');
  const rowsPerPage = 5;

  useEffect(() => {
    fetchMuscleGroups(currentPage);
  }, [currentPage]);

  const fetchMuscleGroups = async (page) => {
    try {
      const response = await axiosInstance.get(`${apiUrl}/get-list`, {
        params: {
          PageNumber: page,
          PageSize: rowsPerPage,
        },
      });
      const data = response.data.items.map(item => ({
        id: item.muscleGroupId,
        name: item.muscleGroupName,
        imageUrl: item.imageUrl || '',
      }));
      setMuscleGroups(data);
      setTotalPages(Math.ceil(response.data.totalCount / rowsPerPage));
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
    if (!newGroupImage && !existingImage) {
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
    formData.append('upload_preset', 'xabhblft');

    try {
      const response = await axios.post(
        `https://api.cloudinary.com/v1_1/${cloudinary.config().cloud_name}/image/upload`,
        formData,
        {
          withCredentials: false,
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

    let imageUrl = existingImage;
    if (newGroupImage) {
      imageUrl = await uploadImageToCloudinary(newGroupImage);
      if (!imageUrl) {
        return;
      }
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
      console.error('Error updating muscle group:', error.message);
      alert('Failed to update muscle group. Please check your input and try again.');
    }
  };

  const deleteMuscleGroup = async (id) => {
    if (!window.confirm('Are you sure you want to delete this muscle group?')) {
      return;
    }

    try {
      await axiosInstance.delete(`${apiUrl}/${id}`);
      fetchMuscleGroups(currentPage);
    } catch (error) {
      console.error('Error deleting muscle group:', error.message);
      alert('Failed to delete muscle group. Please try again.');
    }
  };

  const handleEdit = (group) => {
    setNewGroupName(group.name);
    setNewGroupImage(null);
    setExistingImage(group.imageUrl);
    setEditGroupId(group.id);
    toggleUpdateModal();
  };

  const handleSearch = (e) => {
    setSearchTerm(e.target.value);
    setCurrentPage(1);
    fetchMuscleGroups(1);
  };

  const handleImageChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setNewGroupImage(file);
    }
  };

  const renderTableRows = () => {
    return muscleGroups.map((group, index) => (
      <tr key={group.id}>
        <td>{(currentPage - 1) * rowsPerPage + index + 1}</td>
        <td>{group.name}</td>
        <td>{group.imageUrl && <img src={group.imageUrl} alt={group.name} className="table-image" />}</td>
        <td>
          <div className="button-group">
            <Button color="success" className="mr-2 update-btn" onClick={() => handleEdit(group)}>Update</Button>
          </div>
        </td>
      </tr>
    ));
  };

  const renderPagination = () => {
    const maxPagesToShow = 3;
    const pages = [];

    let startPage = Math.max(1, currentPage - Math.floor(maxPagesToShow / 2));
    let endPage = Math.min(totalPages, startPage + maxPagesToShow - 1);

    if (endPage - startPage < maxPagesToShow - 1) {
      startPage = Math.max(1, endPage - maxPagesToShow + 1);
    }

    if (startPage > 1) {
      pages.push(
        <PaginationItem key="start-ellipsis" disabled>
          <PaginationLink>...</PaginationLink>
        </PaginationItem>
      );
    }

    for (let i = startPage; i <= endPage; i++) {
      pages.push(
        <PaginationItem key={i} active={i === currentPage}>
          <PaginationLink onClick={() => setCurrentPage(i)}>
            {i}
          </PaginationLink>
        </PaginationItem>
      );
    }

    if (endPage < totalPages) {
      pages.push(
        <PaginationItem key="end-ellipsis" disabled>
          <PaginationLink>...</PaginationLink>
        </PaginationItem>
      );
    }

    return (
      <Pagination aria-label="Page navigation example">
        <PaginationItem disabled={currentPage === 1}>
          <PaginationLink first onClick={() => setCurrentPage(1)} />
        </PaginationItem>
        <PaginationItem disabled={currentPage === 1}>
          <PaginationLink previous onClick={() => setCurrentPage(currentPage - 1)} />
        </PaginationItem>
        {pages}
        <PaginationItem disabled={currentPage === totalPages}>
          <PaginationLink next onClick={() => setCurrentPage(currentPage + 1)} />
        </PaginationItem>
        <PaginationItem disabled={currentPage === totalPages}>
          <PaginationLink last onClick={() => setCurrentPage(totalPages)} />
        </PaginationItem>
      </Pagination>
    );
  };

  return (
    <Container>
      <h1 className="my-4"><strong>Muscle Groups</strong></h1>
      <Row className="align-items-center mb-3">
        <Col xs="12" md="10" className="mb-3 mb-md-0">
          <Input
            type="text"
            placeholder="Search muscle group..."
            value={searchTerm}
            onChange={handleSearch}
            className="btn-search"
          />
        </Col>
        <Col xs="12" md="2" className="text-md-right">
          <Button color="primary" onClick={toggleCreateModal} className="btn-create">Create Muscle Group</Button>
        </Col>
      </Row>

      {/* Create Muscle Group Modal */}
      <Modal isOpen={createModal} toggle={toggleCreateModal}>
        <ModalHeader toggle={toggleCreateModal}>Create Muscle Group</ModalHeader>
        <ModalBody>
          <Form>
            <FormGroup>
              <Label for="newGroupName">Muscle Name <span style={{ color: 'red' }}>*</span></Label>
              <Input
                type="text"
                id="newGroupName"
                value={newGroupName}
                onChange={(e) => setNewGroupName(e.target.value)}
              />
              {nameErrorMessage && <Alert color="danger">{nameErrorMessage}</Alert>}
            </FormGroup>
            <FormGroup>
              <Label for="newGroupImage">Muscle Image <span style={{ color: 'red' }}>*</span></Label>
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

          <Button color="danger" class="btn-cannle" onClick={toggleCreateModal}>Cancel</Button>

          <Button color="danger" onClick={toggleCreateModal}>Cancel</Button>

        </ModalFooter>
      </Modal>

      {/* Update Muscle Group Modal */}
      <Modal isOpen={updateModal} toggle={toggleUpdateModal}>
        <ModalHeader toggle={toggleUpdateModal}>Update Muscle Group</ModalHeader>
        <ModalBody>
          <Form>
            <FormGroup>
              <Label for="newGroupName">Muscle Name <span style={{ color: 'red' }}>*</span></Label>
              <Input
                type="text"
                id="newGroupName"
                value={newGroupName}
                onChange={(e) => setNewGroupName(e.target.value)}
              />
              {nameErrorMessage && <Alert color="danger">{nameErrorMessage}</Alert>}
            </FormGroup>
            <FormGroup>
              <Label for="newGroupImage">Muscle Image</Label>
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
          <Button color="danger" onClick={toggleUpdateModal}>Cancel</Button>
        </ModalFooter>
      </Modal>

      <Table striped hover responsive>
        <thead>
          <tr>
            <th>Number</th>
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
        {renderPagination()}
      </div>
    </Container>
  );
}

export default MuscleGroup;
