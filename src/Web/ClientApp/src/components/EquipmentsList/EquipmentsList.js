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
    { id: 1, name: 'Equipment 1' },
    { id: 2, name: 'Equipment 2' },
    { id: 3, name: 'Equipment 3' },
    // Thêm các thiết bị khác ở đây
  ]);

  const [openUpdate, setOpenUpdate] = useState(false);
  const [openDelete, setOpenDelete] = useState(false);
  const [openCreate, setOpenCreate] = useState(false);
  const [selectedEquipment, setSelectedEquipment] = useState(null);
  const [newEquipmentName, setNewEquipmentName] = useState('');
  const [searchQuery, setSearchQuery] = useState('');
  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 10;

  const handleOpenCreate = () => {
    setNewEquipmentName('');
    setOpenCreate(true);
  };

  const handleOpenUpdate = (equipment) => {
    setSelectedEquipment(equipment);
    setOpenUpdate(true);
  };

  const handleOpenDelete = (equipment) => {
    setSelectedEquipment(equipment);
    setOpenDelete(true);
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

  const handleCreate = () => {
    const newEquipment = {
      id: equipments.length ? equipments[equipments.length - 1].id + 1 : 1,
      name: newEquipmentName,
    };
    setEquipments([...equipments, newEquipment]);
    handleCloseCreate();
  };

  const handleUpdate = () => {
    setEquipments(
      equipments.map((equip) =>
        equip.id === selectedEquipment.id
          ? { ...equip, name: selectedEquipment.name }
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

  // Tìm kiếm và phân trang
  const filteredEquipments = equipments.filter((equipment) =>
    equipment.name.toLowerCase().includes(searchQuery.toLowerCase())
  );
  const indexOfLastItem = currentPage * itemsPerPage;
  const indexOfFirstItem = indexOfLastItem - itemsPerPage;
  const currentItems = filteredEquipments.slice(indexOfFirstItem, indexOfLastItem);
  const totalPages = Math.ceil(filteredEquipments.length / itemsPerPage);

  return (
    <div className="equipments-container">
      <h2>Equipments</h2>
      <div className="equipments-actions">
        <input
          type="text"
          placeholder="Search Equipments..."
          value={searchQuery}
          onChange={handleSearch}
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
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {currentItems.map((equipment) => (
            <tr key={equipment.id}>
              <td>{equipment.id}</td>
              <td>{equipment.name}</td>
              <td>
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
            Please enter the name of the new equipment.
          </DialogContentText>
          <TextField
            autoFocus
            margin="dense"
            label="Equipment Name"
            type="text"
            fullWidth
            value={newEquipmentName}
            onChange={(e) => setNewEquipmentName(e.target.value)}
          />
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
          <TextField
            autoFocus
            margin="dense"
            label="Equipment Name"
            type="text"
            fullWidth
            value={selectedEquipment ? selectedEquipment.name : ''}
            onChange={(e) => setSelectedEquipment({ ...selectedEquipment, name: e.target.value })}
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseUpdate} color="primary">
            Cancel
          </Button>
          <Button onClick={handleUpdate} color="primary">
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
          <Button onClick={handleDelete} color="primary">
            Delete
          </Button>
        </DialogActions>
      </Dialog>
    </div>
  );
};

export default EquipmentsList;
