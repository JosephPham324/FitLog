import React, { useState, useEffect } from 'react';
import {
  Button,
  Container,
  Input,
  Row,
  Col,
  Table,
  Modal,
  ModalHeader,
  ModalBody,
  ModalFooter,
  Form,
  FormGroup,
  Label,
  Alert,
  Pagination,
  PaginationItem,
  PaginationLink,
} from 'reactstrap';
import cloudinary from './cloudinaryConfig'; // Import Cloudinary configuration
import './EquipmentsList.css';
import axiosInstance from '../../utils/axiosInstance'; // Import axiosInstance
import axios from 'axios';

const EquipmentsList = () => {
  const [equipments, setEquipments] = useState([]);
  const [allEquipments, setAllEquipments] = useState([]);
  const [openUpdate, setOpenUpdate] = useState(false);
  const [openCreate, setOpenCreate] = useState(false);
  const [openDeleteConfirm, setOpenDeleteConfirm] = useState(false);
  const [openUpdateConfirm, setOpenUpdateConfirm] = useState(false);
  const [selectedEquipment, setSelectedEquipment] = useState(null);
  const [searchQuery, setSearchQuery] = useState('');
  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 5;
  const [totalPages, setTotalPages] = useState(1);
  const [nameErrorMessage, setNameErrorMessage] = useState('');
  const [imageErrorMessage, setImageErrorMessage] = useState('');
  const [error, setError] = useState('');
  const [newEquipmentName, setNewEquipmentName] = useState('');
  const [newEquipmentImageUrl, setNewEquipmentImageUrl] = useState('');
  const [newFileName, setNewFileName] = useState('');
  const [newFileType, setNewFileType] = useState('');
  const [updateFileName, setUpdateFileName] = useState('');
  const [updateFileType, setUpdateFileType] = useState('');
  const [openSuccess, setOpenSuccess] = useState(false);
  const [successMessage, setSuccessMessage] = useState('');
  const [openError, setOpenError] = useState(false);
  const [popupErrorMessage, setPopupErrorMessage] = useState('');
  const [searchErrorMessage, setSearchErrorMessage] = useState(''); // Add this state for search error message

  useEffect(() => {
    fetchAllEquipments();
  }, []);

  useEffect(() => {
    fetchEquipments(currentPage, searchQuery);
  }, [currentPage, searchQuery, allEquipments]);

  const fetchAllEquipments = async () => {
    try {
      const response = await axiosInstance.get('/Equipments/get-all', {
        params: {
          PageNumber: 1,
          PageSize: 1000, // Fetch all items
        }
      });
      const data = response.data.items.map(item => ({
        id: item.equipmentId,
        name: item.equipmentName,
        imageUrl: item.imageUrl || '',
      }));
      data.sort((a, b) => a.name.localeCompare(b.name));
      setAllEquipments(data);
    } catch (error) {
      console.error('Error fetching all equipments:', error);
    }
  };

  const fetchEquipments = async (page, searchTerm) => {
    if (searchTerm) {
      try {
        const response = await axiosInstance.get(`https://localhost:44447/api/Equipments/search`, {
          params: {
            EquipmentName: searchTerm,
            PageNumber: page,
            PageSize: itemsPerPage,
          }
        });
        const data = response.data.items.map(item => ({
          id: item.equipmentId,
          name: item.equipmentName,
          imageUrl: item.imageUrl || '',
        }));
        data.sort((a, b) => a.name.localeCompare(b.name));
        setTotalPages(Math.ceil(response.data.totalCount / itemsPerPage));
        setEquipments(data);
        if (data.length === 0) {
          setSearchErrorMessage('Equipment name has not exist');
        } else {
          setSearchErrorMessage('');
        }
      } catch (error) {
        console.error('Error searching equipments:', error);
      }
    } else {
      const filteredData = allEquipments.slice((page - 1) * itemsPerPage, page * itemsPerPage);
      setTotalPages(Math.ceil(allEquipments.length / itemsPerPage));
      setEquipments(filteredData);
      setSearchErrorMessage('');
    }
  };

  const handleOpenCreate = () => {
    setNewEquipmentName('');
    setNewEquipmentImageUrl('');
    setNewFileName('');
    setNewFileType('');
    setError('');
    setNameErrorMessage('');
    setImageErrorMessage('');
    setOpenCreate(true);
  };

  const handleOpenUpdate = (equipment) => {
    setSelectedEquipment(equipment);
    setUpdateFileName('');
    setUpdateFileType('');
    setError('');
    setNameErrorMessage('');
    setImageErrorMessage('');
    setOpenUpdate(true);
  };

  const handleCloseCreate = () => {
    setOpenCreate(false);
  };

  const handleCloseUpdate = () => {
    setOpenUpdate(false);
  };

  const handleCloseDeleteConfirm = () => {
    setOpenDeleteConfirm(false);
  };

  const handleCloseUpdateConfirm = () => {
    setOpenUpdateConfirm(false);
  };

  const handleCloseSuccess = () => {
    setOpenSuccess(false);
  };

  const handleCloseError = () => {
    setOpenError(false);
    setPopupErrorMessage('');
  };

  const validateCreateInput = (name, imageUrl) => {
    const nameRegex = /^[a-zA-Z0-9 ]+$/;
    let isValid = true;

    if (name.trim() === '' || !nameRegex.test(name)) {
      setNameErrorMessage('Equipment Name cannot be empty!');
      isValid = false;
    } else if (allEquipments.some(equipment => equipment.name.toLowerCase() === name.toLowerCase())) {
      setNameErrorMessage(`${name} already exists!`);
      isValid = false;
    } else {
      setNameErrorMessage('');
    }

    if (!imageUrl) {
      setImageErrorMessage('Equipment Image cannot be empty!');
      isValid = false;
    } else {
      setImageErrorMessage('');
    }

    return isValid;
  };

  const validateUpdateInput = (name, imageUrl) => {
    const nameRegex = /^[a-zA-Z0-9 ]+$/;
    let isValid = true;

    if (name.trim() === '' || !nameRegex.test(name)) {
      setNameErrorMessage('Equipment Name cannot be empty!');
      isValid = false;
    } else if (allEquipments.some(equipment => equipment.name.toLowerCase() === name.toLowerCase() && equipment.id !== selectedEquipment.id)) {
      setNameErrorMessage(`${name} already exists!`);
      isValid = false;
    } else {
      setNameErrorMessage('');
    }

    if (!imageUrl) {
      setImageErrorMessage('Equipment Image cannot be empty!');
      isValid = false;
    } else {
      setImageErrorMessage('');
    }

    return isValid;
  };

  const handleCreate = async () => {
    if (!validateCreateInput(newEquipmentName, newEquipmentImageUrl)) {
      return;
    }

    const formData = new FormData();
    formData.append('file', newEquipmentImageUrl);
    formData.append('upload_preset', 'xabhblft'); // Correct preset name

    try {
      const uploadResponse = await axios.post(
        `https://api.cloudinary.com/v1_1/${cloudinary.config().cloud_name}/image/upload`,
        formData,
        {
          withCredentials: false, // Cloudinary does not require login info
        }
      );

      const newEquipment = {
        equipmentName: newEquipmentName,
        imageUrl: uploadResponse.data.secure_url,
      };

      await axiosInstance.post("/Equipments", newEquipment, {
        headers: {
          'Content-Type': 'application/json',
        },
      });

      fetchAllEquipments();
      handleCloseCreate();
      setSuccessMessage("Equipment Created successfully");
      setOpenSuccess(true);
    } catch (error) {
      console.error('Error uploading image:', error);
      const errorMessage = error.response && error.response.data && error.response.data.error && error.response.data.error.message
        ? error.response.data.error.message
        : 'Unknown error occurred';
      if (error.response && error.response.data && error.response.data.errors) {
        const errorMessages = error.response.data.errors;
        if (errorMessages.EquipmentName && errorMessages.EquipmentName.includes("Unique")) {
          setNameErrorMessage('Equipment Name already exists!');
        }
      } else {
        setError('Error uploading image: ' + errorMessage);
      }
    }
  };

  const handleUpdate = async () => {
    if (!validateUpdateInput(selectedEquipment.name, selectedEquipment.imageUrl)) {
      return;
    }

    try {
      const updateData = {
        equipmentId: selectedEquipment.id,
        equipmentName: selectedEquipment.name,
        imageUrl: selectedEquipment.imageUrl,
      };

      await axiosInstance.put(`/Equipments/${selectedEquipment.id}`, updateData, {
        headers: {
          'Content-Type': 'application/json',
        },
      });

      fetchAllEquipments();
      handleCloseUpdate();
      handleCloseUpdateConfirm();
      setSuccessMessage("Equipment Updated successfully");
      setOpenSuccess(true);
    } catch (error) {
      console.error('Error updating equipment:', error);
      setError('Error updating equipment');
    }
  };

  const handleDelete = async () => {
    if (!selectedEquipment) {
      return;
    }
    try {
      const response = await axiosInstance.delete(`/Equipments/${selectedEquipment.id}`, {
        data: {
          equipmentId: selectedEquipment.id
        }
      });
      if (response.status === 200 && response.data.success) {
        setSuccessMessage('Equipment deleted successfully!');
        setOpenSuccess(true);
        fetchAllEquipments();
      } else {
        setPopupErrorMessage(response.data.errors ? response.data.errors[0] : 'Error deleting equipment. Please try again.');
        setOpenError(true);
      }
      handleCloseDeleteConfirm();
    } catch (error) {
      console.error('Error deleting equipment:', error);
      setPopupErrorMessage('Failed to delete equipment. Please try again.');
      setOpenError(true);
    }
  };

  const handleSearch = (event) => {
    setSearchQuery(event.target.value);
    setCurrentPage(1); // Reset to the first page on search
  };

  const handleUpdateConfirmation = () => {
    if (!validateUpdateInput(selectedEquipment.name, selectedEquipment.imageUrl)) {
      return;
    }
    setOpenUpdateConfirm(true);
  };

  const handleImageUpload = (event) => {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onloadend = () => {
        setNewEquipmentImageUrl(reader.result);
        setNewFileName(file.name);
        setNewFileType(file.type);
      };
      reader.readAsDataURL(file);
    }
  };

  const handleUpdateImageUpload = async (event) => {
    const file = event.target.files[0];
    if (file) {
      const formData = new FormData();
      formData.append('file', file);
      formData.append('upload_preset', 'xabhblft');
      try {
        const uploadResponse = await axios.post(
          `https://api.cloudinary.com/v1_1/${cloudinary.config().cloud_name}/image/upload`,
          formData,
          {
            withCredentials: false, // Cloudinary does not require login info
          }
        );
        setSelectedEquipment({ ...selectedEquipment, imageUrl: uploadResponse.data.secure_url });
      } catch (error) {
        console.error('Error uploading image:', error);
        setImageErrorMessage('Error uploading image');
      }
    }
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
      <h2 className="my-4"><strong>Equipments</strong></h2>
      <Row className="align-items-center mb-3">
        <Col xs="12" md="10" className="mb-3 mb-md-0">
          <Input
            type="text"
            placeholder="Search Equipments..."
            value={searchQuery}
            onChange={handleSearch}
            className="btn-search"
          />
          {searchErrorMessage && <Alert color="danger">{searchErrorMessage}</Alert>}
        </Col>
        <Col xs="12" md="2" className="text-md-right">
          <Button color="primary" className="create-button" onClick={handleOpenCreate}>Create Equipment</Button>
        </Col>
      </Row>

      <Table striped hover responsive>
        <thead>
          <tr>
            <th>#</th>
            <th>Equipment Name</th>
            <th>Image URL</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {equipments.map((equipment, index) => (
            <tr key={equipment.id}>
              <td>{(currentPage - 1) * itemsPerPage + index + 1}</td>
              <td>{equipment.name}</td>
              <td>
                {equipment.imageUrl ? (
                  <img src={equipment.imageUrl} alt={equipment.name} className="img" />
                ) : (
                  'No Image'
                )}
              </td>
              <td>
                <Button color="success" className="mr-2 edit-buttone" onClick={() => handleOpenUpdate(equipment)}>Update</Button>
                <Button color="danger" onClick={() => { setSelectedEquipment(equipment); setOpenDeleteConfirm(true); }}>Delete</Button>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>

      {renderPagination()}

      {/* Create Modal */}
      <Modal isOpen={openCreate} toggle={handleCloseCreate}>
        <ModalHeader toggle={handleCloseCreate}>Create Equipment</ModalHeader>
        <ModalBody>
          <Form>
            <FormGroup>
              <Label for="newEquipmentName">
                Equipment Name <span style={{ color: 'red' }}>*</span>
              </Label>
              <Input
                type="text"
                id="newEquipmentName"
                value={newEquipmentName}
                onChange={(e) => setNewEquipmentName(e.target.value)}
              />
              {nameErrorMessage && <Alert color="danger">{nameErrorMessage}</Alert>}
            </FormGroup>
            <FormGroup>
              <Label for="newEquipmentImageUrl">
                Equipment Image <span style={{ color: 'red' }}>*</span>
              </Label>
              <Input
                type="file"
                id="newEquipmentImageUrl"
                accept="image/*"
                onChange={handleImageUpload}
              />
              {imageErrorMessage && <Alert color="danger">{imageErrorMessage}</Alert>}
            </FormGroup>
          </Form>
        </ModalBody>
        <ModalFooter>
          <Button color="primary" onClick={handleCreate}>Create</Button>
          <Button color="danger" onClick={handleCloseCreate}>Cancel</Button>
        </ModalFooter>
      </Modal>

      {/* Update Modal */}
      <Modal isOpen={openUpdate} toggle={handleCloseUpdate}>
        <ModalHeader toggle={handleCloseUpdate}>Update Equipment</ModalHeader>
        <ModalBody>
          <Form>
            <FormGroup>
              <Label for="selectedEquipmentName">
                Equipment Name <span style={{ color: 'red' }}>*</span>
              </Label>
              <Input
                type="text"
                id="selectedEquipmentName"
                value={selectedEquipment ? selectedEquipment.name : ''}
                onChange={(e) => setSelectedEquipment({ ...selectedEquipment, name: e.target.value })}
              />
              {nameErrorMessage && <Alert color="danger">{nameErrorMessage}</Alert>}
            </FormGroup>
            <FormGroup>
              <Label for="updateEquipmentImageUrl">
                Equipment Image
              </Label>
              <Input
                type="file"
                id="updateEquipmentImageUrl"
                accept="image/*"
                onChange={handleUpdateImageUpload}
              />
              {selectedEquipment && selectedEquipment.imageUrl && (
                <img src={selectedEquipment.imageUrl} alt={selectedEquipment.name} className="img mt-3" />
              )}
              {imageErrorMessage && <Alert color="danger">{imageErrorMessage}</Alert>}
            </FormGroup>
          </Form>
        </ModalBody>
        <ModalFooter>
          <Button color="primary" onClick={handleUpdateConfirmation}>Update</Button>
          <Button color="danger" onClick={handleCloseUpdate}>Cancel</Button>
        </ModalFooter>
      </Modal>

      {/* Delete Confirm Modal */}
      <Modal isOpen={openDeleteConfirm} toggle={handleCloseDeleteConfirm}>
        <ModalHeader toggle={handleCloseDeleteConfirm}>Confirmation Delete</ModalHeader>
        <ModalBody>
          <p>Are you sure you want to delete this equipment?</p>
        </ModalBody>
        <ModalFooter>
          <Button color="danger" onClick={handleDelete}>Yes</Button>
          <Button color="primary" onClick={handleCloseDeleteConfirm}>No</Button>
        </ModalFooter>
      </Modal>

      {/* Update Confirm Modal */}
      <Modal isOpen={openUpdateConfirm} toggle={handleCloseUpdateConfirm}>
        <ModalHeader toggle={handleCloseUpdateConfirm}>Confirmation Update</ModalHeader>
        <ModalBody>
          <p>Are you sure you want to update this equipment?</p>
        </ModalBody>
        <ModalFooter>
          <Button color="primary" onClick={handleUpdate}>Yes</Button>
          <Button color="danger" onClick={handleCloseUpdateConfirm}>No</Button>
        </ModalFooter>
      </Modal>

      {/* Success Modal */}
      <Modal isOpen={openSuccess} toggle={handleCloseSuccess}>
        <ModalHeader toggle={handleCloseSuccess}>Success</ModalHeader>
        <ModalBody>
          <p>{successMessage}</p>
        </ModalBody>
        <ModalFooter>
          <Button color="success" onClick={handleCloseSuccess}>OK</Button>
        </ModalFooter>
      </Modal>

      {/* Error Modal */}
      <Modal isOpen={openError} toggle={handleCloseError}>
        <ModalHeader toggle={handleCloseError}>Error</ModalHeader>
        <ModalBody>
          <p>{popupErrorMessage}</p>
        </ModalBody>
        <ModalFooter>
          <Button color="danger" onClick={handleCloseError}>OK</Button>
        </ModalFooter>
      </Modal>
    </Container>
  );
};

export default EquipmentsList;
