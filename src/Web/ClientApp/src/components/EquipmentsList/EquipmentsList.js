////import React, { useState, useEffect } from 'react';
////import {
////  Button,
////  Container,
////  Input,
////  Row,
////  Col,
////  Form,
////  FormGroup,
////  Label,
////  Modal,
////  ModalHeader,
////  ModalBody,
////  ModalFooter,
////  Alert,
////  Table,
////} from 'reactstrap';
////import cloudinary from './cloudinaryConfig'; // Import Cloudinary configuration
////import './EquipmentsList.css';
////import axiosInstance from '../../utils/axiosInstance'; // Import axiosInstance
////import axios from 'axios';

////const EquipmentsList = () => {
////  const [equipments, setEquipments] = useState([]);
////  const [allEquipments, setAllEquipments] = useState([]);
////  const [openUpdate, setOpenUpdate] = useState(false);
////  const [openCreate, setOpenCreate] = useState(false);
////  const [openDeleteConfirm, setOpenDeleteConfirm] = useState(false);
////  const [openUpdateConfirm, setOpenUpdateConfirm] = useState(false);
////  const [selectedEquipment, setSelectedEquipment] = useState(null);
////  const [newEquipmentName, setNewEquipmentName] = useState('');
////  const [newEquipmentImageUrl, setNewEquipmentImageUrl] = useState('');
////  const [newFileName, setNewFileName] = useState('');
////  const [newFileType, setNewFileType] = useState('');
////  const [updateFileName, setUpdateFileName] = useState('');
////  const [updateFileType, setUpdateFileType] = useState('');
////  const [searchQuery, setSearchQuery] = useState('');
////  const [currentPage, setCurrentPage] = useState(1);
////  const itemsPerPage = 5;
////  const [totalPages, setTotalPages] = useState(1);
////  const [nameErrorMessage, setNameErrorMessage] = useState('');
////  const [imageErrorMessage, setImageErrorMessage] = useState('');
////  const [error, setError] = useState('');

////  useEffect(() => {
////    fetchAllEquipments();
////  }, []);

////  useEffect(() => {
////    fetchEquipments(currentPage, searchQuery);
////  }, [currentPage, searchQuery, allEquipments]);

////  const fetchAllEquipments = async () => {
////    try {
////      const response = await axiosInstance.get('/Equipments/get-all', {
////        params: {
////          PageNumber: 1,
////          PageSize: 1000, // Fetch all items
////        }
////      });
////      const data = response.data.items.map(item => ({
////        id: item.equipmentId,
////        name: item.equipmentName,
////        imageUrl: item.imageUrl || '',
////      }));
////      setAllEquipments(data);
////    } catch (error) {
////      console.error('Error fetching all equipments:', error);
////    }
////  };

////  const fetchEquipments = (page, searchTerm) => {
////    const filteredData = allEquipments.filter((equipment) =>
////      equipment.name.toLowerCase().includes(searchTerm.toLowerCase())
////    );
////    filteredData.sort((a, b) => a.name.localeCompare(b.name)); // Sắp xếp theo tên
////    setTotalPages(Math.ceil(filteredData.length / itemsPerPage));
////    setEquipments(filteredData.slice((page - 1) * itemsPerPage, page * itemsPerPage));
////  };

////  const handleOpenCreate = () => {
////    setNewEquipmentName('');
////    setNewEquipmentImageUrl('');
////    setNewFileName('');
////    setNewFileType('');
////    setError('');
////    setNameErrorMessage('');
////    setImageErrorMessage('');
////    setOpenCreate(true);
////  };

////  const handleOpenUpdate = (equipment) => {
////    setSelectedEquipment(equipment);
////    setUpdateFileName('');
////    setUpdateFileType('');
////    setError('');
////    setNameErrorMessage('');
////    setImageErrorMessage('');
////    setOpenUpdate(true);
////  };

////  const handleCloseCreate = () => {
////    setOpenCreate(false);
////  };

////  const handleCloseUpdate = () => {
////    setOpenUpdate(false);
////  };

////  const handleCloseDeleteConfirm = () => {
////    setOpenDeleteConfirm(false);
////  };

////  const handleCloseUpdateConfirm = () => {
////    setOpenUpdateConfirm(false);
////  };

////  const validateCreateInput = (name, imageUrl) => {
////    const nameRegex = /^[a-zA-Z0-9 ]+$/;
////    let isValid = true;

////    if (name.trim() === '' || !nameRegex.test(name)) {
////      setNameErrorMessage('Equipment Name cannot be empty!');
////      isValid = false;
////    } else if (allEquipments.some(equipment => equipment.name.toLowerCase() === name.toLowerCase())) {
////      setNameErrorMessage(`${name} already exists!`);
////      isValid = false;
////    } else {
////      setNameErrorMessage('');
////    }

////    if (!imageUrl) {
////      setImageErrorMessage('Equipment Image cannot be empty!');
////      isValid = false;
////    } else {
////      setImageErrorMessage('');
////    }

////    return isValid;
////  };

////  const validateUpdateInput = (name, imageUrl) => {
////    const nameRegex = /^[a-zA-Z0-9 ]+$/;
////    let isValid = true;

////    if (name.trim() === '' || !nameRegex.test(name)) {
////      setNameErrorMessage('Equipment Name cannot be empty!');
////      isValid = false;
////    } else {
////      setNameErrorMessage('');
////    }

////    if (!imageUrl) {
////      setImageErrorMessage('Equipment Image cannot be empty!');
////      isValid = false;
////    } else {
////      setImageErrorMessage('');
////    }

////    return isValid;
////  };

////  const handleCreate = async () => {
////    if (!validateCreateInput(newEquipmentName, newEquipmentImageUrl)) {
////      return;
////    }

////    const formData = new FormData();
////    formData.append('file', newEquipmentImageUrl);
////    formData.append('upload_preset', 'xabhblft'); // Sử dụng đúng tên preset

////    try {
////      const uploadResponse = await axios.post(
////        `https://api.cloudinary.com/v1_1/${cloudinary.config().cloud_name}/image/upload`,
////        formData,
////        {
////          withCredentials: false, // Cloudinary không yêu cầu thông tin đăng nhập
////        }
////      );

////      const newEquipment = {
////        equipmentName: newEquipmentName,
////        imageUrl: uploadResponse.data.secure_url,
////      };

////      await axiosInstance.post("/Equipments", newEquipment, {
////        headers: {
////          'Content-Type': 'application/json',
////        },
////      });

////      fetchAllEquipments();
////      handleCloseCreate();
////    } catch (error) {
////      console.error('Error uploading image:', error);
////      const errorMessage = error.response && error.response.data && error.response.data.error && error.response.data.error.message
////        ? error.response.data.error.message
////        : 'Unknown error occurred';
////      if (error.response && error.response.data && error.response.data.errors) {
////        const errorMessages = error.response.data.errors;
////        if (errorMessages.EquipmentName && errorMessages.EquipmentName.includes("Unique")) {
////          setNameErrorMessage('Equipment Name already exists!');
////        }
////      } else {
////        setError('Error uploading image: ' + errorMessage);
////      }
////    }
////  };

////  const handleUpdate = async () => {
////    if (!validateUpdateInput(selectedEquipment.name, selectedEquipment.imageUrl)) {
////      return;
////    }

////    try {
////      const updateData = {
////        equipmentId: selectedEquipment.id,
////        equipmentName: selectedEquipment.name,
////        imageUrl: selectedEquipment.imageUrl,
////      };

////      await axiosInstance.put(`/Equipments/${selectedEquipment.id}`, updateData, {
////        headers: {
////          'Content-Type': 'application/json',
////        },
////      });

////      fetchAllEquipments();
////      handleCloseUpdate();
////      handleCloseUpdateConfirm();
////    } catch (error) {
////      console.error('Error updating equipment:', error);
////      setError('Error updating equipment');
////    }
////  };

////  const handleDelete = async () => {
////    try {
////      const response = await axiosInstance.delete(`/Equipments/${selectedEquipment.id}`);
////      if (response.data.success === false) {
////        alert(response.data.errors[0]);
////      }
////      fetchAllEquipments();
////      handleCloseDeleteConfirm();
////    } catch (error) {
////      console.error('Error deleting equipment:', error);
////      setError('Error deleting equipment');
////    }
////  };

////  const handleSearch = (event) => {
////    setSearchQuery(event.target.value);
////    setCurrentPage(1); // Reset to the first page on search
////    fetchEquipments(1, event.target.value);
////  };

////  const handleChangePage = (pageNumber) => {
////    console.log('Changing to page', pageNumber);
////    setCurrentPage(pageNumber);
////    fetchEquipments(pageNumber, searchQuery);
////  };

////  const handleUpdateConfirmation = () => {
////    setOpenUpdateConfirm(true);
////  };

////  const handleDeleteConfirmation = (equipment) => {
////    setSelectedEquipment(equipment);
////    setOpenDeleteConfirm(true);
////  };

////  const handleImageUpload = (event) => {
////    const file = event.target.files[0];
////    if (file) {
////      const reader = new FileReader();
////      reader.onloadend = () => {
////        setNewEquipmentImageUrl(reader.result);
////        setNewFileName(file.name);
////        setNewFileType(file.type);
////      };
////      reader.readAsDataURL(file);
////    }
////  };

////  const handleUpdateImageUpload = async (event) => {
////    const file = event.target.files[0];
////    if (file) {
////      const formData = new FormData();
////      formData.append('file', file);
////      formData.append('upload_preset', 'xabhblft');
////      try {
////        const uploadResponse = await axios.post(
////          `https://api.cloudinary.com/v1_1/${cloudinary.config().cloud_name}/image/upload`,
////          formData,
////          {
////            withCredentials: false, // Cloudinary không yêu cầu thông tin đăng nhập
////          }
////        );
////        setSelectedEquipment({ ...selectedEquipment, imageUrl: uploadResponse.data.secure_url });
////      } catch (error) {
////        console.error('Error uploading image:', error);
////        setImageErrorMessage('Error uploading image');
////      }
////    }
////  };

////  return (
////    <Container>
////      <h2 className="my-4">Equipments</h2>
////      <Row className="align-items-center mb-3">
////        <Col xs="12" md="10" className="mb-3 mb-md-0">
////          <Input
////            type="text"
////            placeholder="Search Equipments..."
////            value={searchQuery}
////            onChange={handleSearch}
////            className="btn-search"
////          />
////        </Col>
////        <Col xs="12" md="2" className="text-md-right">
////          <Button color="primary" className="create-buttone" onClick={handleOpenCreate}>Create Equipment</Button>
////        </Col>
////      </Row>

////      <Table striped hover responsive>
////        <thead>
////          <tr>
////            <th>#</th>
////            <th>Equipment Name</th>
////            <th>Image URL</th>
////            <th>Actions</th>
////          </tr>
////        </thead>
////        <tbody>
////          {equipments.map((equipment, index) => (
////            <tr key={equipment.id}>
////              <td>{(currentPage - 1) * itemsPerPage + index + 1}</td>
////              <td>{equipment.name}</td>
////              <td>
////                {equipment.imageUrl ? (
////                  <img src={equipment.imageUrl} alt={equipment.name} className="img" />
////                ) : (
////                  'No Image'
////                )}
////              </td>
////              <td>
////                <Button color="success" className="mr-2 edit-buttone" onClick={() => handleOpenUpdate(equipment)}>Update</Button>
////                <Button color="danger" onClick={() => handleDeleteConfirmation(equipment)}>Delete</Button>
////              </td>
////            </tr>
////          ))}
////        </tbody>
////      </Table>

////      <div className="pagination">
////        <Button
////          className="pre"
////          color="primary"
////          size="sm"
////          disabled={currentPage === 1}
////          onClick={() => handleChangePage(currentPage - 1)}
////        >
////          Previous
////        </Button>
////        <span className="mx-2">
////          Page {currentPage} of {totalPages}
////        </span>
////        <Button
////          className="next"
////          color="primary"
////          size="sm"
////          disabled={currentPage === totalPages}
////          onClick={() => handleChangePage(currentPage + 1)}
////        >
////          Next
////        </Button>
////      </div>

////      {/* Create Modal */}
////      <Modal isOpen={openCreate} toggle={handleCloseCreate}>
////        <ModalHeader toggle={handleCloseCreate}>Create Equipment</ModalHeader>
////        <ModalBody>
////          <Form>
////            <FormGroup>
////              <Label for="newEquipmentName">
////                Equipment Name <span style={{ color: 'red' }}>*</span>
////              </Label>
////              <Input
////                type="text"
////                id="newEquipmentName"
////                value={newEquipmentName}
////                onChange={(e) => setNewEquipmentName(e.target.value)}
////              />
////              {nameErrorMessage && <Alert color="danger">{nameErrorMessage}</Alert>}
////            </FormGroup>
////            <FormGroup>
////              <Label for="newEquipmentImageUrl">
////                Equipment Image <span style={{ color: 'red' }}>*</span>
////              </Label>
////              <Input
////                type="file"
////                id="newEquipmentImageUrl"
////                accept="image/*"
////                onChange={handleImageUpload}
////              />
////              {imageErrorMessage && <Alert color="danger">{imageErrorMessage}</Alert>}
////            </FormGroup>
////          </Form>
////        </ModalBody>
////        <ModalFooter>
////          <Button color="primary" onClick={handleCreate}>Create</Button>
////          <Button color="danger" onClick={handleCloseCreate}>Cancel</Button>
////        </ModalFooter>
////      </Modal>

////      {/* Update Modal */}
////      <Modal isOpen={openUpdate} toggle={handleCloseUpdate}>
////        <ModalHeader toggle={handleCloseUpdate}>Update Equipment</ModalHeader>
////        <ModalBody>
////          <Form>
////            <FormGroup>
////              <Label for="selectedEquipmentName">
////                Equipment Name <span style={{ color: 'red' }}>*</span>
////              </Label>
////              <Input
////                type="text"
////                id="selectedEquipmentName"
////                value={selectedEquipment ? selectedEquipment.name : ''}
////                onChange={(e) => setSelectedEquipment({ ...selectedEquipment, name: e.target.value })}
////              />
////              {nameErrorMessage && <Alert color="danger">{nameErrorMessage}</Alert>}
////            </FormGroup>
////            <FormGroup>
////              <Label for="updateEquipmentImageUrl">
////                Equipment Image <span style={{ color: 'red' }}>*</span>
////              </Label>
////              <Input
////                type="file"
////                id="updateEquipmentImageUrl"
////                accept="image/*"
////                onChange={handleUpdateImageUpload}
////              />
////              {selectedEquipment && selectedEquipment.imageUrl && (
////                <img src={selectedEquipment.imageUrl} alt={selectedEquipment.name} className="img mt-3" />
////              )}
////              {imageErrorMessage && <Alert color="danger">{imageErrorMessage}</Alert>}
////            </FormGroup>
////          </Form>
////        </ModalBody>
////        <ModalFooter>
////          <Button color="primary" onClick={handleUpdateConfirmation}>Update</Button>
////          <Button color="danger" onClick={handleCloseUpdate}>Cancel</Button>
////        </ModalFooter>
////      </Modal>

////      {/* Delete Confirm Modal */}
////      <Modal isOpen={openDeleteConfirm} toggle={handleCloseDeleteConfirm}>
////        <ModalHeader toggle={handleCloseDeleteConfirm}>Confirmation Delete</ModalHeader>
////        <ModalBody>
////          <p>Are you sure you want to delete this equipment?</p>
////        </ModalBody>
////        <ModalFooter>
////          <Button color="primary" onClick={handleDelete}>Yes</Button>
////          <Button color="danger" onClick={handleCloseDeleteConfirm}>No</Button>
////        </ModalFooter>
////      </Modal>

////      {/* Update Confirm Modal */}
////      <Modal isOpen={openUpdateConfirm} toggle={handleCloseUpdateConfirm}>
////        <ModalHeader toggle={handleCloseUpdateConfirm}>Confirmation Update</ModalHeader>
////        <ModalBody>
////          <p>Are you sure you want to update this equipment?</p>
////        </ModalBody>
////        <ModalFooter>
////          <Button color="primary" onClick={handleUpdate}>Yes</Button>
////          <Button color="danger" onClick={handleCloseUpdateConfirm}>No</Button>
////        </ModalFooter>
////      </Modal>
////    </Container>
////  );
////};

////export default EquipmentsList;


//  import React, { useState, useEffect } from 'react';
//  import {
//    Button,
//    Container,
//    Input,
//    Row,
//    Col,
//    Form,
//    FormGroup,
//    Label,
//    Modal,
//    ModalHeader,
//    ModalBody,
//    ModalFooter,
//    Alert,
//    Table,
//  } from 'reactstrap';
//  import cloudinary from './cloudinaryConfig'; // Import Cloudinary configuration
//  import './EquipmentsList.css';
//  import axiosInstance from '../../utils/axiosInstance'; // Import axiosInstance
//  import axios from 'axios';

//  const EquipmentsList = () => {
//    const [equipments, setEquipments] = useState([]);
//    const [allEquipments, setAllEquipments] = useState([]);
//    const [openUpdate, setOpenUpdate] = useState(false);
//    const [openCreate, setOpenCreate] = useState(false);
//    const [openDeleteConfirm, setOpenDeleteConfirm] = useState(false);
//    const [openUpdateConfirm, setOpenUpdateConfirm] = useState(false);
//    const [selectedEquipment, setSelectedEquipment] = useState(null);
//    const [isReferenced, setIsReferenced] = useState(false);
//    const [searchQuery, setSearchQuery] = useState('');
//    const [currentPage, setCurrentPage] = useState(1);
//    const itemsPerPage = 5;
//    const [totalPages, setTotalPages] = useState(1);
//    const [nameErrorMessage, setNameErrorMessage] = useState('');
//    const [imageErrorMessage, setImageErrorMessage] = useState('');
//    const [error, setError] = useState('');
//    const [newEquipmentName, setNewEquipmentName] = useState('');
//    const [newEquipmentImageUrl, setNewEquipmentImageUrl] = useState('');
//    const [newFileName, setNewFileName] = useState('');
//    const [newFileType, setNewFileType] = useState('');
//    const [updateFileName, setUpdateFileName] = useState('');
//    const [updateFileType, setUpdateFileType] = useState('');

//    useEffect(() => {
//      fetchAllEquipments();
//    }, []);

//    useEffect(() => {
//      fetchEquipments(currentPage, searchQuery);
//    }, [currentPage, searchQuery, allEquipments]);

//    const fetchAllEquipments = async () => {
//      try {
//        const response = await axiosInstance.get('/Equipments/get-all', {
//          params: {
//            PageNumber: 1,
//            PageSize: 1000, // Fetch all items
//          }
//        });
//        const data = response.data.items.map(item => ({
//          id: item.equipmentId,
//          name: item.equipmentName,
//          imageUrl: item.imageUrl || '',
//        }));
//        setAllEquipments(data);
//      } catch (error) {
//        console.error('Error fetching all equipments:', error);
//      }
//    };

//    const fetchEquipments = (page, searchTerm) => {
//      const filteredData = allEquipments.filter((equipment) =>
//        equipment.name.toLowerCase().includes(searchTerm.toLowerCase())
//      );
//      filteredData.sort((a, b) => a.name.localeCompare(b.name)); // Sort by name
//      setTotalPages(Math.ceil(filteredData.length / itemsPerPage));
//      setEquipments(filteredData.slice((page - 1) * itemsPerPage, page * itemsPerPage));
//    };

//    const handleOpenCreate = () => {
//      setNewEquipmentName('');
//      setNewEquipmentImageUrl('');
//      setNewFileName('');
//      setNewFileType('');
//      setError('');
//      setNameErrorMessage('');
//      setImageErrorMessage('');
//      setOpenCreate(true);
//    };

//    const handleOpenUpdate = (equipment) => {
//      setSelectedEquipment(equipment);
//      setUpdateFileName('');
//      setUpdateFileType('');
//      setError('');
//      setNameErrorMessage('');
//      setImageErrorMessage('');
//      setOpenUpdate(true);
//    };

//    const handleCloseCreate = () => {
//      setOpenCreate(false);
//    };

//    const handleCloseUpdate = () => {
//      setOpenUpdate(false);
//    };

//    const handleCloseDeleteConfirm = () => {
//      setOpenDeleteConfirm(false);
//    };

//    const handleCloseUpdateConfirm = () => {
//      setOpenUpdateConfirm(false);
//    };

//    const validateCreateInput = (name, imageUrl) => {
//      const nameRegex = /^[a-zA-Z0-9 ]+$/;
//      let isValid = true;

//      if (name.trim() === '' || !nameRegex.test(name)) {
//        setNameErrorMessage('Equipment Name cannot be empty!');
//        isValid = false;
//      } else if (allEquipments.some(equipment => equipment.name.toLowerCase() === name.toLowerCase())) {
//        setNameErrorMessage(`${name} already exists!`);
//        isValid = false;
//      } else {
//        setNameErrorMessage('');
//      }

//      if (!imageUrl) {
//        setImageErrorMessage('Equipment Image cannot be empty!');
//        isValid = false;
//      } else {
//        setImageErrorMessage('');
//      }

//      return isValid;
//    };

//    const validateUpdateInput = (name, imageUrl) => {
//      const nameRegex = /^[a-zA-Z0-9 ]+$/;
//      let isValid = true;

//      if (name.trim() === '' || !nameRegex.test(name)) {
//        setNameErrorMessage('Equipment Name cannot be empty!');
//        isValid = false;
//      } else if (allEquipments.some(equipment => equipment.name.toLowerCase() === name.toLowerCase() && equipment.id !== selectedEquipment.id)) {
//        setNameErrorMessage(`${name} already exists!`);
//        isValid = false;
//      } else {
//        setNameErrorMessage('');
//      }

//      if (!imageUrl) {
//        setImageErrorMessage('Equipment Image cannot be empty!');
//        isValid = false;
//      } else {
//        setImageErrorMessage('');
//      }

//      return isValid;
//    };

//    const handleCreate = async () => {
//      if (!validateCreateInput(newEquipmentName, newEquipmentImageUrl)) {
//        return;
//      }

//      const formData = new FormData();
//      formData.append('file', newEquipmentImageUrl);
//      formData.append('upload_preset', 'xabhblft'); // Correct preset name

//      try {
//        const uploadResponse = await axios.post(
//          `https://api.cloudinary.com/v1_1/${cloudinary.config().cloud_name}/image/upload`,
//          formData,
//          {
//            withCredentials: false, // Cloudinary does not require login info
//          }
//        );

//        const newEquipment = {
//          equipmentName: newEquipmentName,
//          imageUrl: uploadResponse.data.secure_url,
//        };

//        await axiosInstance.post("/Equipments", newEquipment, {
//          headers: {
//            'Content-Type': 'application/json',
//          },
//        });

//        fetchAllEquipments();
//        handleCloseCreate();
//      } catch (error) {
//        console.error('Error uploading image:', error);
//        const errorMessage = error.response && error.response.data && error.response.data.error && error.response.data.error.message
//          ? error.response.data.error.message
//          : 'Unknown error occurred';
//        if (error.response && error.response.data && error.response.data.errors) {
//          const errorMessages = error.response.data.errors;
//          if (errorMessages.EquipmentName && errorMessages.EquipmentName.includes("Unique")) {
//            setNameErrorMessage('Equipment Name already exists!');
//          }
//        } else {
//          setError('Error uploading image: ' + errorMessage);
//        }
//      }
//    };

//    const handleUpdate = async () => {
//      if (!validateUpdateInput(selectedEquipment.name, selectedEquipment.imageUrl)) {
//        return;
//      }

//      try {
//        const updateData = {
//          equipmentId: selectedEquipment.id,
//          equipmentName: selectedEquipment.name,
//          imageUrl: selectedEquipment.imageUrl,
//        };

//        await axiosInstance.put(`/Equipments/${selectedEquipment.id}`, updateData, {
//          headers: {
//            'Content-Type': 'application/json',
//          },
//        });

//        fetchAllEquipments();
//        handleCloseUpdate();
//        handleCloseUpdateConfirm();
//      } catch (error) {
//        console.error('Error updating equipment:', error);
//        setError('Error updating equipment');
//      }
//    };

//    const handleDelete = async () => {
//      try {
//        const response = await axiosInstance.delete(`/Equipments/${selectedEquipment.id}`);
//        if (response.data.success === false) {
//          alert(response.data.errors[0]);
//        }
//        fetchAllEquipments();
//        handleCloseDeleteConfirm();
//      } catch (error) {
//        console.error('Error deleting equipment:', error);
//        setError('Error deleting equipment');
//      }
//    };

//    const handleSearch = (event) => {
//      setSearchQuery(event.target.value);
//      setCurrentPage(1); // Reset to the first page on search
//      fetchEquipments(1, event.target.value);
//    };

//    const handleChangePage = (pageNumber) => {
//      console.log('Changing to page', pageNumber);
//      setCurrentPage(pageNumber);
//      fetchEquipments(pageNumber, searchQuery);
//    };

//    const handleDeleteConfirmation = async (equipment) => {
//      setSelectedEquipment(equipment);
//      try {
//        const response = await axiosInstance.get(`/Equipments/check-referenced/${equipment.id}`);
//        setIsReferenced(response.data.isReferenced);
//      } catch (error) {
//        console.error('Error checking if equipment is referenced:', error);
//        setIsReferenced(false);
//      }
//      setOpenDeleteConfirm(true);
//    };

//    const handleUpdateConfirmation = () => {
//      if (!validateUpdateInput(selectedEquipment.name, selectedEquipment.imageUrl)) {
//        return;
//      }
//      setOpenUpdateConfirm(true);
//    };

//    const handleImageUpload = (event) => {
//      const file = event.target.files[0];
//      if (file) {
//        const reader = new FileReader();
//        reader.onloadend = () => {
//          setNewEquipmentImageUrl(reader.result);
//          setNewFileName(file.name);
//          setNewFileType(file.type);
//        };
//        reader.readAsDataURL(file);
//      }
//    };

//    const handleUpdateImageUpload = async (event) => {
//      const file = event.target.files[0];
//      if (file) {
//        const formData = new FormData();
//        formData.append('file', file);
//        formData.append('upload_preset', 'xabhblft');
//        try {
//          const uploadResponse = await axios.post(
//            `https://api.cloudinary.com/v1_1/${cloudinary.config().cloud_name}/image/upload`,
//            formData,
//            {
//              withCredentials: false, // Cloudinary does not require login info
//            }
//          );
//          setSelectedEquipment({ ...selectedEquipment, imageUrl: uploadResponse.data.secure_url });
//        } catch (error) {
//          console.error('Error uploading image:', error);
//          setImageErrorMessage('Error uploading image');
//        }
//      }
//    };

//    return (
//      <Container>
//        <h2 className="my-4">Equipments</h2>
//        <Row className="align-items-center mb-3">
//          <Col xs="12" md="10" className="mb-3 mb-md-0">
//            <Input
//              type="text"
//              placeholder="Search Equipments..."
//              value={searchQuery}
//              onChange={handleSearch}
//              className="btn-search"
//            />
//          </Col>
//          <Col xs="12" md="2" className="text-md-right">
//            <Button color="primary" className="create-button" onClick={handleOpenCreate}>Create Equipment</Button>
//          </Col>
//        </Row>

//        <Table striped hover responsive>
//          <thead>
//            <tr>
//              <th>#</th>
//              <th>Equipment Name</th>
//              <th>Image URL</th>
//              <th>Actions</th>
//            </tr>
//          </thead>
//          <tbody>
//            {equipments.map((equipment, index) => (
//              <tr key={equipment.id}>
//                <td>{(currentPage - 1) * itemsPerPage + index + 1}</td>
//                <td>{equipment.name}</td>
//                <td>
//                  {equipment.imageUrl ? (
//                    <img src={equipment.imageUrl} alt={equipment.name} className="img" />
//                  ) : (
//                    'No Image'
//                  )}
//                </td>
//                <td>
//                  <Button color="success" className="mr-2 edit-buttone" onClick={() => handleOpenUpdate(equipment)}>Update</Button>
//                  <Button color="danger" onClick={() => handleDeleteConfirmation(equipment)}>Delete</Button>
//                </td>
//              </tr>
//            ))}
//          </tbody>
//        </Table>

//        <div className="pagination">
//          <Button
//            className="pre"
//            color="primary"
//            size="sm"
//            disabled={currentPage === 1}
//            onClick={() => handleChangePage(currentPage - 1)}
//          >
//            Previous
//          </Button>
//          <span className="mx-2">
//            Page {currentPage} of {totalPages}
//          </span>
//          <Button
//            className="next"
//            color="primary"
//            size="sm"
//            disabled={currentPage === totalPages}
//            onClick={() => handleChangePage(currentPage + 1)}
//          >
//            Next
//          </Button>
//        </div>

//        {/* Create Modal */}
//        <Modal isOpen={openCreate} toggle={handleCloseCreate}>
//          <ModalHeader toggle={handleCloseCreate}>Create Equipment</ModalHeader>
//          <ModalBody>
//            <Form>
//              <FormGroup>
//                <Label for="newEquipmentName">
//                  Equipment Name <span style={{ color: 'red' }}>*</span>
//                </Label>
//                <Input
//                  type="text"
//                  id="newEquipmentName"
//                  value={newEquipmentName}
//                  onChange={(e) => setNewEquipmentName(e.target.value)}
//                />
//                {nameErrorMessage && <Alert color="danger">{nameErrorMessage}</Alert>}
//              </FormGroup>
//              <FormGroup>
//                <Label for="newEquipmentImageUrl">
//                  Equipment Image <span style={{ color: 'red' }}>*</span>
//                </Label>
//                <Input
//                  type="file"
//                  id="newEquipmentImageUrl"
//                  accept="image/*"
//                  onChange={handleImageUpload}
//                />
//                {imageErrorMessage && <Alert color="danger">{imageErrorMessage}</Alert>}
//              </FormGroup>
//            </Form>
//          </ModalBody>
//          <ModalFooter>
//            <Button color="primary" onClick={handleCreate}>Create</Button>
//            <Button color="danger" onClick={handleCloseCreate}>Cancel</Button>
//          </ModalFooter>
//        </Modal>

//        {/* Update Modal */}
//        <Modal isOpen={openUpdate} toggle={handleCloseUpdate}>
//          <ModalHeader toggle={handleCloseUpdate}>Update Equipment</ModalHeader>
//          <ModalBody>
//            <Form>
//              <FormGroup>
//                <Label for="selectedEquipmentName">
//                  Equipment Name <span style={{ color: 'red' }}>*</span>
//                </Label>
//                <Input
//                  type="text"
//                  id="selectedEquipmentName"
//                  value={selectedEquipment ? selectedEquipment.name : ''}
//                  onChange={(e) => setSelectedEquipment({ ...selectedEquipment, name: e.target.value })}
//                />
//                {nameErrorMessage && <Alert color="danger">{nameErrorMessage}</Alert>}
//              </FormGroup>
//              <FormGroup>
//                <Label for="updateEquipmentImageUrl">
//                  Equipment Image <span style={{ color: 'red' }}>*</span>
//                </Label>
//                <Input
//                  type="file"
//                  id="updateEquipmentImageUrl"
//                  accept="image/*"
//                  onChange={handleUpdateImageUpload}
//                />
//                {selectedEquipment && selectedEquipment.imageUrl && (
//                  <img src={selectedEquipment.imageUrl} alt={selectedEquipment.name} className="img mt-3" />
//                )}
//                {imageErrorMessage && <Alert color="danger">{imageErrorMessage}</Alert>}
//              </FormGroup>
//            </Form>
//          </ModalBody>
//          <ModalFooter>
//            <Button color="primary" onClick={handleUpdateConfirmation}>Update</Button>
//            <Button color="danger" onClick={handleCloseUpdate}>Cancel</Button>
//          </ModalFooter>
//        </Modal>

//        {/* Delete Confirm Modal */}
//        <Modal isOpen={openDeleteConfirm} toggle={handleCloseDeleteConfirm}>
//          <ModalHeader toggle={handleCloseDeleteConfirm}>Confirmation</ModalHeader>
//          <ModalBody>
//            {isReferenced ? (
//              <p>Equipment is referenced by 1 or more exercises.</p>
//            ) : (
//              <p>Are you sure you want to delete this equipment?</p>
//            )}
//          </ModalBody>
//          <ModalFooter>
//            {isReferenced ? (
//              <Button color="primary" onClick={handleCloseDeleteConfirm}>OK</Button>
//            ) : (
//              <>
//                <Button color="primary" onClick={handleDelete}>Yes</Button>
//                <Button color="danger" onClick={handleCloseDeleteConfirm}>No</Button>
//              </>
//            )}
//          </ModalFooter>
//        </Modal>

//        {/* Update Confirm Modal */}
//        <Modal isOpen={openUpdateConfirm} toggle={handleCloseUpdateConfirm}>
//          <ModalHeader toggle={handleCloseUpdateConfirm}>Confirmation</ModalHeader>
//          <ModalBody>
//            <p>Are you sure you want to update this equipment?</p>
//          </ModalBody>
//          <ModalFooter>
//            <Button color="primary" onClick={handleUpdate}>Yes</Button>
//            <Button color="danger" onClick={handleCloseUpdateConfirm}>No</Button>
//          </ModalFooter>
//        </Modal>
//      </Container>
//    );
//  };

//  export default EquipmentsList;


import React, { useState, useEffect } from 'react';
import {
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
  Table,
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
  const [isReferenced, setIsReferenced] = useState(false);
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
      setAllEquipments(data);
    } catch (error) {
      console.error('Error fetching all equipments:', error);
    }
  };

  const fetchEquipments = (page, searchTerm) => {
    const filteredData = allEquipments.filter((equipment) =>
      equipment.name.toLowerCase().includes(searchTerm.toLowerCase())
    );
    filteredData.sort((a, b) => a.name.localeCompare(b.name)); // Sort by name
    setTotalPages(Math.ceil(filteredData.length / itemsPerPage));
    setEquipments(filteredData.slice((page - 1) * itemsPerPage, page * itemsPerPage));
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
    } catch (error) {
      console.error('Error updating equipment:', error);
      setError('Error updating equipment');
    }
  };

  const handleDelete = async () => {
    try {
      const response = await axiosInstance.delete(`/Equipments/${selectedEquipment.id}`);
      if (response.data.success === false) {
        alert(response.data.errors[0]);
      }
      fetchAllEquipments();
      handleCloseDeleteConfirm();
    } catch (error) {
      console.error('Error deleting equipment:', error);
      setError('Error deleting equipment');
    }
  };

  const handleSearch = (event) => {
    setSearchQuery(event.target.value);
    setCurrentPage(1); // Reset to the first page on search
    fetchEquipments(1, event.target.value);
  };

  const handleChangePage = (pageNumber) => {
    setCurrentPage(pageNumber);
    fetchEquipments(pageNumber, searchQuery);
  };

  const handleDeleteConfirmation = async (equipment) => {
    setSelectedEquipment(equipment);
    try {
      const response = await axiosInstance.get(`/Equipments/check-referenced/${equipment.id}`);
      setIsReferenced(response.data.isReferenced);
    } catch (error) {
      console.error('Error checking if equipment is referenced:', error);
      setIsReferenced(false);
    }
    setOpenDeleteConfirm(true);
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
    const pages = [];
    const startPage = Math.max(1, currentPage - 2);
    const endPage = Math.min(totalPages, currentPage + 2);

    for (let i = startPage; i <= endPage; i++) {
      pages.push(
        <Button
          key={i}
          color={currentPage === i ? 'success' : 'secondary'}
          className={`page-button ${currentPage === i ? 'active' : ''}`}
          onClick={() => handleChangePage(i)}
        >
          {i}
        </Button>
      );
    }

    return (
      <div className="pagination-container">
        <Button
          color="primary"
          className="first-page-button"
          onClick={() => handleChangePage(1)}
          disabled={currentPage === 1}
        >
          &laquo;
        </Button>
        <Button
          color="primary"
          className="prev-page-button"
          onClick={() => handleChangePage(currentPage - 1)}
          disabled={currentPage === 1}
        >
          &lt;
        </Button>
        {pages}
        <Button
          color="primary"
          className="next-page-button"
          onClick={() => handleChangePage(currentPage + 1)}
          disabled={currentPage === totalPages}
        >
          &gt;
        </Button>
        <Button
          color="primary"
          className="last-page-button"
          onClick={() => handleChangePage(totalPages)}
          disabled={currentPage === totalPages}
        >
          &raquo;
        </Button>
      </div>
    );
  };

  return (
    <Container>
      <h2 className="my-4">Equipments</h2>
      <Row className="align-items-center mb-3">
        <Col xs="12" md="10" className="mb-3 mb-md-0">
          <Input
            type="text"
            placeholder="Search Equipments..."
            value={searchQuery}
            onChange={handleSearch}
            className="btn-search"
          />
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
                <Button color="danger" onClick={() => handleDeleteConfirmation(equipment)}>Delete</Button>
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
                Equipment Image <span style={{ color: 'red' }}>*</span>
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
        <ModalHeader toggle={handleCloseDeleteConfirm}>Confirmation</ModalHeader>
        <ModalBody>
          {isReferenced ? (
            <p>Equipment is referenced by 1 or more exercises.</p>
          ) : (
            <p>Are you sure you want to delete this equipment?</p>
          )}
        </ModalBody>
        <ModalFooter>
          {isReferenced ? (
            <Button color="primary" onClick={handleCloseDeleteConfirm}>OK</Button>
          ) : (
            <>
              <Button color="primary" onClick={handleDelete}>Yes</Button>
              <Button color="danger" onClick={handleCloseDeleteConfirm}>No</Button>
            </>
          )}
        </ModalFooter>
      </Modal>

      {/* Update Confirm Modal */}
      <Modal isOpen={openUpdateConfirm} toggle={handleCloseUpdateConfirm}>
        <ModalHeader toggle={handleCloseUpdateConfirm}>Confirmation</ModalHeader>
        <ModalBody>
          <p>Are you sure you want to update this equipment?</p>
        </ModalBody>
        <ModalFooter>
          <Button color="primary" onClick={handleUpdate}>Yes</Button>
          <Button color="danger" onClick={handleCloseUpdateConfirm}>No</Button>
        </ModalFooter>
      </Modal>
    </Container>
  );
};

export default EquipmentsList;
