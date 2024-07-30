import React, { useState, useEffect } from 'react';
import {
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  TextField,
  Button,
} from '@mui/material';
import cloudinary from './cloudinaryConfig'; // Import Cloudinary configuration
import './EquipmentsList.css';
import axiosInstance from '../../utils/axiosInstance'; // Import axiosInstance
import axios from 'axios';

const EquipmentsList = () => {
  const [equipments, setEquipments] = useState([]);
  const [openUpdate, setOpenUpdate] = useState(false);
  const [openCreate, setOpenCreate] = useState(false);
  const [selectedEquipment, setSelectedEquipment] = useState(null);
  const [newEquipmentName, setNewEquipmentName] = useState('');
  const [newEquipmentImageUrl, setNewEquipmentImageUrl] = useState('');
  const [newFileName, setNewFileName] = useState('');
  const [newFileType, setNewFileType] = useState('');
  const [updateFileName, setUpdateFileName] = useState('');
  const [updateFileType, setUpdateFileType] = useState('');
  const [searchQuery, setSearchQuery] = useState('');
  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 10;
  const [error, setError] = useState('');

  useEffect(() => {
    fetchEquipments();
  }, []);

  const fetchEquipments = async () => {
    try {
      const response = await axiosInstance.get('/Equipments/get-all?PageNumber=1&PageSize=50');
      const data = response.data.items.map(item => ({
        id: item.equipmentId,
        name: item.equipmentName,
        imageUrl: item.imageUrl || '' // Sử dụng trường imageUrl từ API nếu có
      }));
      setEquipments(data);
    } catch (error) {
      console.error('Error fetching equipments:', error);
    }
  };

  const handleOpenCreate = () => {
    setNewEquipmentName('');
    setNewEquipmentImageUrl('');
    setNewFileName('');
    setNewFileType('');
    setError('');
    setOpenCreate(true);
  };

  const handleOpenUpdate = (equipment) => {
    setSelectedEquipment(equipment);
    setUpdateFileName('');
    setUpdateFileType('');
    setError('');
    setOpenUpdate(true);
  };

  const handleCloseCreate = () => {
    setOpenCreate(false);
  };

  const handleCloseUpdate = () => {
    setOpenUpdate(false);
  };

  const validateInput = (name) => {
    const nameRegex = /^[a-zA-Z0-9 ]+$/;
    if (name.trim() === '' || !nameRegex.test(name)) {
      setError('Create Equipment cannot be left blank or contain special characters');
      return false;
    }
    return true;
  };

  const handleCreate = async () => {
    if (!validateInput(newEquipmentName)) {
      return;
    }

    const formData = new FormData();
    formData.append('file', newEquipmentImageUrl);
    formData.append('upload_preset', 'xabhblft'); // Sử dụng đúng tên preset

    try {
      const uploadResponse = await axios.post(
        `https://api.cloudinary.com/v1_1/${cloudinary.config().cloud_name}/image/upload`,
        formData,
        {
          withCredentials: false, // Cloudinary không yêu cầu thông tin đăng nhập
        }
      );

      const newEquipment = {
        equipmentName: newEquipmentName,
        imageUrl: uploadResponse.data.secure_url,
      };

      const response = await axiosInstance.post("/Equipments", newEquipment, {
        headers: {
          'Content-Type': 'application/json',
        },
      });

      fetchEquipments();

      handleCloseCreate();
    } catch (error) {
      console.error('Error uploading image:', error);
      console.log('Error response:', error.response ? error.response.data : error.message);
      const errorMessage = error.response && error.response.data && error.response.data.error && error.response.data.error.message
        ? error.response.data.error.message
        : 'Unknown error occurred';
      setError('Error uploading image: ' + errorMessage);
    }
  };

  const handleUpdate = async () => {
    if (!validateInput(selectedEquipment.name)) {
      setError('Update Equipment cannot be left blank or contain special characters');
      return;
    }

    try {
      const updateData = {
        equipmentId: selectedEquipment.id, // Đảm bảo ID được gửi đi khớp với ID trong URL
        equipmentName: selectedEquipment.name,
        imageUrl: selectedEquipment.imageUrl,
      };

      const response = await axiosInstance.put(`/Equipments/${selectedEquipment.id}`, updateData, {
        headers: {
          'Content-Type': 'application/json',
        },
      });

      console.log('Update response:', response.data);

      setEquipments(
        equipments.map((equip) =>
          equip.id === selectedEquipment.id
            ? { ...equip, name: selectedEquipment.name, imageUrl: selectedEquipment.imageUrl }
            : equip
        )
      );

      handleCloseUpdate();
    } catch (error) {
      console.error('Error updating equipment:', error);
      setError('Error updating equipment');
    }
  };

  const handleSearch = (event) => {
    setSearchQuery(event.target.value);
  };

  const handleChangePage = (pageNumber) => {
    setCurrentPage(pageNumber);
  };

  const handleUpdateConfirmation = () => {
    if (window.confirm('Are you sure to edit?')) {
      handleUpdate();
    }
  };

  const handleImageUpload = (event) => {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onloadend = () => {
        console.log('File content:', reader.result); // Log dữ liệu file
        setNewEquipmentImageUrl(reader.result);
        setNewFileName(file.name);
        setNewFileType(file.type);
      };
      reader.readAsDataURL(file);
    }
  };

  const handleUpdateImageUpload = (event) => {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onloadend = () => {
        setSelectedEquipment({ ...selectedEquipment, imageUrl: reader.result });
        setUpdateFileName(file.name);
        setUpdateFileType(file.type);
      };
      reader.readAsDataURL(file);
    }
  };

  const filteredEquipments = equipments
    .filter((equipment) =>
      equipment.name.toLowerCase().includes(searchQuery.toLowerCase())
    )
    .sort((a, b) => a.id - b.id);

  const indexOfLastItem = currentPage * itemsPerPage;
  const indexOfFirstItem = indexOfLastItem - itemsPerPage;
  const currentItems = filteredEquipments.slice(indexOfFirstItem, indexOfLastItem);
  const totalPages = Math.ceil(filteredEquipments.length / itemsPerPage);

  return (
    <div className="equipments-container">
      <h2 className="equipments-title">
        <span className="gradient-text">Equipments</span>
      </h2>
      <div className="equipments-actions">
        <input
          type="text"
          placeholder="Search Equipments..."
          value={searchQuery}
          onChange={handleSearch}
          style={{ border: '1px solid #000', padding: '5px' }}
        />
        <button className="create-buttone" onClick={handleOpenCreate}>
          Create Equipment
        </button>
      </div>
      <table className="equipments-table">
        <thead>
          <tr>
            <th>Equipment ID</th>
            <th>Equipment Name</th>
            <th>Image URL</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {currentItems.map((equipment) => (
            <tr key={equipment.id}>
              <td>{equipment.id}</td>
              <td>{equipment.name}</td>
              <td>
                {equipment.imageUrl ? (
                  <img src={equipment.imageUrl} alt={equipment.name} style={{ width: '100px', height: '100px' }} />
                ) : (
                  'No Image'
                )}
              </td>
              <td>
                <button className="edit-buttone" onClick={() => handleOpenUpdate(equipment)}>Update</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      <div className="pagination">
        {Array.from({ length: totalPages }, (_, index) => (
          <button
            key={index + 1}
            className={`page-button ${currentPage === index + 1 ? 'active' : ''}`}
            onClick={() => handleChangePage(index + 1)}
          >
            {index + 1}
          </button>
        ))}
        {currentPage < totalPages && (
          <button className="page-button" onClick={() => handleChangePage(currentPage + 1)}>Next</button>
        )}
      </div>

      {/* Create Dialog */}
      <Dialog open={openCreate} onClose={handleCloseCreate}>
        <DialogTitle>Create Equipment</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Please enter the details of the new equipment.
          </DialogContentText>
          {error && <p className="error-text">{error}</p>}
          <TextField
            autoFocus
            margin="dense"
            label="Equipment Name"
            type="text"
            fullWidth
            value={newEquipmentName}
            onChange={(e) => setNewEquipmentName(e.target.value)}
          />
          <input
            id="upload-button-create"
            type="file"
            accept="image/*"
            onChange={handleImageUpload}
            style={{ display: 'none' }}
          />
          <label htmlFor="upload-button-create" className="upload-button">
            Select File
          </label>
          {newFileName && newFileType && (
            <div>
              <p><strong>File Name:</strong> {newFileName}</p>
              <p><strong>File Type:</strong> {newFileType}</p>
            </div>
          )}
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseCreate} color="primary">
            Cancel
          </Button>
          <Button onClick={handleCreate} color="primary">
            Create
          </Button>
        </DialogActions>
      </Dialog>

      {/* Update Dialog */}
      <Dialog open={openUpdate} onClose={handleCloseUpdate}>
        <DialogTitle>Update Equipment</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Update the equipment information.
          </DialogContentText>
          {error && <p className="error-text">{error}</p>}
          <TextField
            autoFocus
            margin="dense"
            label="Equipment ID"
            type="text"
            fullWidth
            value={selectedEquipment ? selectedEquipment.id : ''}
            disabled // Disable the ID field as it is not editable
          />
          <TextField
            margin="dense"
            label="Equipment Name"
            type="text"
            fullWidth
            value={selectedEquipment ? selectedEquipment.name : ''}
            onChange={(e) => setSelectedEquipment({ ...selectedEquipment, name: e.target.value })}
          />
          <input
            id="upload-button-update"
            type="file"
            accept="image/*"
            onChange={handleUpdateImageUpload}
            style={{ display: 'none' }}
          />
          <label htmlFor="upload-button-update" className="upload-button">
            Select File
          </label>
          {updateFileName && updateFileType && (
            <div>
              <p><strong>File Name:</strong> {updateFileName}</p>
              <p><strong>File Type:</strong> {updateFileType}</p>
            </div>
          )}
          {selectedEquipment && selectedEquipment.imageUrl && (
            <img src={selectedEquipment.imageUrl} alt={selectedEquipment.name} style={{ width: '100px', height: '100px', marginTop: '10px' }} />
          )}
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseUpdate} color="primary">
            Cancel
          </Button>
          <Button onClick={handleUpdateConfirmation} color="primary">
            Update
          </Button>
        </DialogActions>
      </Dialog>
    </div>
  );
};

export default EquipmentsList;



//import React, { useState, useEffect } from 'react';
//import { Table, Button, Container, Input, Row, Col, Form, FormGroup, Label, Modal, ModalHeader, ModalBody, ModalFooter, Alert } from 'reactstrap';
//import cloudinary from './cloudinaryConfig'; // Import Cloudinary configuration
//import axiosInstance from '../../utils/axiosInstance'; // Import axiosInstance
//import axios from 'axios';
//import './EquipmentsList.css';

//const apiUrl = '/Equipments';

//const EquipmentsList = () => {
//  const [equipments, setEquipments] = useState([]);
//  const [currentPage, setCurrentPage] = useState(1);
//  const [newEquipmentName, setNewEquipmentName] = useState('');
//  const [newEquipmentImageUrl, setNewEquipmentImageUrl] = useState('');
//  const [createModal, setCreateModal] = useState(false);
//  const [updateModal, setUpdateModal] = useState(false);
//  const [editEquipmentId, setEditEquipmentId] = useState(null);
//  const [totalPages, setTotalPages] = useState(1);
//  const [searchTerm, setSearchTerm] = useState('');
//  const [nameErrorMessage, setNameErrorMessage] = useState('');
//  const [imageErrorMessage, setImageErrorMessage] = useState('');
//  const rowsPerPage = 5;

//  useEffect(() => {
//    fetchEquipments(currentPage, searchTerm);
//  }, [currentPage, searchTerm]);

//  const fetchEquipments = async (page, searchTerm) => {
//    try {
//      const response = await axiosInstance.get(`${apiUrl}/get-all`, {
//        params: {
//          PageNumber: page,
//          PageSize: rowsPerPage,
//          searchTerm: searchTerm
//        }
//      });
//      setEquipments(response.data.items);
//      setTotalPages(response.data.totalPages);
//    } catch (error) {
//      console.error('Error fetching equipments:', error);
//    }
//  };

//  const toggleCreateModal = () => {
//    setCreateModal(!createModal);
//    if (!createModal) {
//      setNewEquipmentName('');
//      setNewEquipmentImageUrl('');
//      setNameErrorMessage('');
//      setImageErrorMessage('');
//    }
//  };

//  const toggleUpdateModal = () => {
//    setUpdateModal(!updateModal);
//    if (!updateModal) {
//      setNameErrorMessage('');
//      setImageErrorMessage('');
//    }
//  };

//  const validateInput = () => {
//    let valid = true;
//    if (!newEquipmentName) {
//      setNameErrorMessage('Equipment name is required');
//      valid = false;
//    } else {
//      setNameErrorMessage('');
//    }
//    if (!newEquipmentImageUrl) {
//      setImageErrorMessage('Equipment image URL is required');
//      valid = false;
//    } else {
//      setImageErrorMessage('');
//    }
//    return valid;
//  };

//  const createEquipment = async () => {
//    if (!validateInput()) {
//      return;
//    }

//    const formData = new FormData();
//    formData.append('file', newEquipmentImageUrl);
//    formData.append('upload_preset', 'xabhblft'); // Use the correct preset name

//    try {
//      const uploadResponse = await axios.post(
//        `https://api.cloudinary.com/v1_1/${cloudinary.config().cloud_name}/image/upload`,
//        formData,
//        {
//          withCredentials: false, // Cloudinary does not require login information
//        }
//      );

//      const newEquipment = {
//        equipmentName: newEquipmentName,
//        imageUrl: uploadResponse.data.secure_url,
//      };

//      await axiosInstance.post("/Equipments", newEquipment, {
//        headers: {
//          'Content-Type': 'application/json',
//        },
//      });

//      fetchEquipments(currentPage, searchTerm);
//      toggleCreateModal();
//    } catch (error) {
//      console.error('Error uploading image:', error);
//      alert('Failed to create equipment. Please check your input and try again.');
//    }
//  };

//  const updateEquipment = async () => {
//    if (!window.confirm('Are you sure you want to update this equipment?')) {
//      return;
//    }

//    if (!validateInput() || !editEquipmentId) {
//      return;
//    }

//    try {
//      await axiosInstance.put(`${apiUrl}/${editEquipmentId}`, {
//        id: editEquipmentId,
//        equipmentName: newEquipmentName,
//        imageUrl: newEquipmentImageUrl
//      });

//      fetchEquipments(currentPage, searchTerm);
//      toggleUpdateModal();
//    } catch (error) {
//      console.error('Error updating equipment:', error.message);
//      alert('Failed to update equipment. Please check your input and try again.');
//    }
//  };

//  const handleEdit = (equipment) => {
//    setNewEquipmentName(equipment.name);
//    setNewEquipmentImageUrl(equipment.imageUrl);
//    setEditEquipmentId(equipment.id);
//    toggleUpdateModal();
//  };

//  const handleSearch = (e) => {
//    setSearchTerm(e.target.value);
//    setCurrentPage(1);
//  };

//  const renderTableRows = () => {
//    return equipments.map(equipment => (
//      <tr key={equipment.id}>
//        <td>{equipment.id}</td>
//        <td>{equipment.name}</td>
//        <td>{equipment.imageUrl && <img src={equipment.imageUrl} alt={equipment.name} className="table-image" />}</td>
//        <td>
//          <div className="button-group">
//            <Button color="success" className="mr-2 update-btn" onClick={() => handleEdit(equipment)}>Update</Button>
//            {/* <Button color="danger" className="mr-2 delete-btn" onClick={() => deleteEquipment(equipment.id)}>Delete</Button>*/}
//          </div>
//        </td>
//      </tr>
//    ));
//  };

//  const handleImageUpload = (event) => {
//  const file = event.target.files[0];
//  if (file) {
//    const reader = new FileReader();
//    reader.onloadend = () => {
//      setNewEquipmentImageUrl(reader.result);
//    };
//    reader.readAsDataURL(file);
//  }
//};

//const handleUpdateImageUpload = (event) => {
//  const file = event.target.files[0];
//  if (file) {
//    const reader = new FileReader();
//    reader.onloadend = () => {
//      setNewEquipmentImageUrl(reader.result);
//    };
//    reader.readAsDataURL(file);
//  }
//};

//  return (
//    <Container>
//      <h1 className="my-4">Equipments</h1>
//      <Row className="align-items-center mb-3">
//        <Col xs="12" md="10" className="mb-3 mb-md-0">
//          <Input
//            type="text"
//            placeholder="Search equipment..."
//            value={searchTerm}
//            onChange={handleSearch}
//            className="btn-search"
//          />
//        </Col>
//        <Col xs="12" md="2" className="text-md-right">
//          <Button color="primary" onClick={toggleCreateModal} className="btn-create">Create Equipment</Button>
//        </Col>
//      </Row>

//      {/* Create Equipment Modal */}
//      <Modal isOpen={createModal} toggle={toggleCreateModal}>
//        <ModalHeader toggle={toggleCreateModal}>Create Equipment</ModalHeader>
//        <ModalBody>
//          <Form>
//            <FormGroup>
//              <Label for="newEquipmentName">Name</Label>
//              <Input
//                type="text"
//                id="newEquipmentName"
//                value={newEquipmentName}
//                onChange={(e) => setNewEquipmentName(e.target.value)}
//              />
//              {nameErrorMessage && <Alert color="danger">{nameErrorMessage}</Alert>}
//            </FormGroup>
//            <FormGroup>
//              <Label for="newEquipmentImageUrl">Image URL</Label>
//              <Input
//                type="file"
//                id="newEquipmentImageUrl"
//                accept="image/*"
//                onChange={handleImageUpload}
//              />
//              {imageErrorMessage && <Alert color="danger">{imageErrorMessage}</Alert>}
//            </FormGroup>
//          </Form>
//        </ModalBody>
//        <ModalFooter>
//          <Button color="primary" onClick={createEquipment}>Create</Button>
//          <Button color="danger" onClick={toggleCreateModal}>Cancel</Button>
//        </ModalFooter>
//      </Modal>

//      {/* Update Equipment Modal */}
//      <Modal isOpen={updateModal} toggle={toggleUpdateModal}>
//        <ModalHeader toggle={toggleUpdateModal}>Update Equipment</ModalHeader>
//        <ModalBody>
//          <Form>
//            <FormGroup>
//              <Label for="newEquipmentName">Name</Label>
//              <Input
//                type="text"
//                id="newEquipmentName"
//                value={newEquipmentName}
//                onChange={(e) => setNewEquipmentName(e.target.value)}
//              />
//              {nameErrorMessage && <Alert color="danger">{nameErrorMessage}</Alert>}
//            </FormGroup>
//            <FormGroup>
//              <Label for="newEquipmentImageUrl">Image URL</Label>
//              <Input
//                type="file"
//                id="newEquipmentImageUrl"
//                accept="image/*"
//                onChange={handleUpdateImageUpload}
//              />
//              {imageErrorMessage && <Alert color="danger">{imageErrorMessage}</Alert>}
//            </FormGroup>
//          </Form>
//        </ModalBody>
//        <ModalFooter>
//          <Button color="primary" onClick={updateEquipment}>Update</Button>
//          <Button color="danger" onClick={toggleUpdateModal}>Cancel</Button>
//        </ModalFooter>
//      </Modal>

//      <Table striped hover responsive>
//        <thead>
//          <tr>
//            <th>Equipment ID</th>
//            <th>Equipment Name</th>
//            <th>Image</th>
//            <th>Actions</th>
//          </tr>
//        </thead>
//        <tbody>
//          {renderTableRows()}
//        </tbody>
//      </Table>
//      <div className="pagination">
//        <Button
//          className="pre"
//          color="primary"
//          size="sm"
//          disabled={currentPage === 1}
//          onClick={() => setCurrentPage(currentPage - 1)}
//        >
//          Previous
//        </Button>
//        <span className="mx-2">
//          Page {currentPage} of {totalPages}
//        </span>
//        <Button
//          className="next"
//          color="primary"
//          size="sm"
//          disabled={currentPage === totalPages}
//          onClick={() => setCurrentPage(currentPage + 1)}
//        >
//          Next
//        </Button>
//      </div>
//    </Container>
//  );
//};

//export default EquipmentsList;
