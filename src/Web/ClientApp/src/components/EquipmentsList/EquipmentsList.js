import React, { useState } from 'react';
import {
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  TextField,
  Button,
} from '@mui/material';
import './EquipmentsList.css';

const EquipmentsList = () => {
  const [equipments, setEquipments] = useState([
    { id: 1, name: 'Equipment 1', details: 'Detail for Equipment 1', imageUrl: 'https://antimatter.vn/wp-content/uploads/2022/11/hinh-anh-gai-xinh-trung-quoc.jpg' },
    { id: 2, name: 'Equipment 2', details: 'Detail for Equipment 2', imageUrl: 'https://example.com/image2.jpg' },
    { id: 3, name: 'Equipment 3', details: 'Detail for Equipment 3', imageUrl: 'https://example.com/image3.jpg' },
    // Add more equipment here
  ]);

  const [openUpdate, setOpenUpdate] = useState(false);
  const [openDelete, setOpenDelete] = useState(false);
  const [openCreate, setOpenCreate] = useState(false);
  const [openDetail, setOpenDetail] = useState(false);
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

  const handleOpenDelete = (equipment) => {
    setSelectedEquipment(equipment);
    setOpenDelete(true);
  };

  const handleOpenDetail = (equipment) => {
    setSelectedEquipment(equipment);
    setOpenDetail(true);
  };

  const handleCloseCreate = () => {
    setOpenCreate(false);
  };

  const handleCloseUpdate = () => {
    setOpenUpdate(false);
  };

  const handleCloseDelete = () => {
    setOpenDelete(false);
  };

  const handleCloseDetail = () => {
    setOpenDetail(false);
  };

  const validateInput = (name) => {
    const nameRegex = /^[a-zA-Z0-9 ]+$/;
    if (name.trim() === '' || !nameRegex.test(name)) {
      setError('Create Equipment cannot be left blank or contain special characters');
      return false;
    }
    return true;
  };

  const handleCreate = () => {
    if (!validateInput(newEquipmentName)) {
      return;
    }
    const newEquipment = {
      id: equipments.length ? equipments[equipments.length - 1].id + 1 : 1,
      name: newEquipmentName,
      details: '',
      imageUrl: newEquipmentImageUrl,
    };
    setEquipments([...equipments, newEquipment]);
    handleCloseCreate();
  };

  const handleUpdate = () => {
    if (!validateInput(selectedEquipment.name)) {
      setError('Update Equipment cannot be left blank or contain special characters');
      return;
    }
    setEquipments(
      equipments.map((equip) =>
        equip.id === selectedEquipment.id
          ? { ...equip, name: selectedEquipment.name, imageUrl: selectedEquipment.imageUrl }
          : equip
      )
    );
    handleCloseUpdate();
  };

  const handleDelete = () => {
    setEquipments(equipments.filter((equip) => equip.id !== selectedEquipment.id));
    handleCloseDelete();
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

  const handleDeleteConfirmation = () => {
    if (window.confirm('Are you sure you want to delete?')) {
      handleDelete();
    }
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

  const filteredEquipments = equipments.filter((equipment) =>
    equipment.name.toLowerCase().includes(searchQuery.toLowerCase())
  );
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
        <button className="create-button" onClick={handleOpenCreate}>
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
                <img src={equipment.imageUrl} alt={equipment.name} style={{ width: '100px', height: '100px' }} />
              </td>
              <td>
                <button className="detail-button" onClick={() => handleOpenDetail(equipment)}>Detail</button>
                <button className="edit-button" onClick={() => handleOpenUpdate(equipment)}>Edit</button>
                <button className="delete-button" onClick={() => handleOpenDelete(equipment)}>Delete</button>
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

      {/* Delete Dialog */}
      <Dialog open={openDelete} onClose={handleCloseDelete}>
        <DialogTitle>Delete Equipment</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Are you sure you want to delete this equipment?
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseDelete} color="primary">
            Cancel
          </Button>
          <Button onClick={handleDeleteConfirmation} color="primary">
            Delete
          </Button>
        </DialogActions>
      </Dialog>

      {/* Detail Dialog */}
      <Dialog open={openDetail} onClose={handleCloseDetail}>
        <DialogTitle>Equipment Detail</DialogTitle>
        <DialogContent>
          <DialogContentText>
            <div>
              <p><strong>ID:</strong> {selectedEquipment ? selectedEquipment.id : ''}</p>
              <p><strong>Equipment Name:</strong> {selectedEquipment ? selectedEquipment.name : ''}</p>
              <p><strong>Image:</strong></p>
              {selectedEquipment && selectedEquipment.imageUrl && (
                <img src={selectedEquipment.imageUrl} alt={selectedEquipment.name} style={{ width: '100px', height: '100px' }} />
              )}
            </div>
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseDetail} color="primary">
            Close
          </Button>
        </DialogActions>
      </Dialog>
    </div >
  );
};

export default EquipmentsList;
