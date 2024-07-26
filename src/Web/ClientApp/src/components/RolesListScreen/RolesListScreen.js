import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './RolesListScreen.css';

const RolesListScreen = () => {
  const [roles, setRoles] = useState([]);
  const [showPopup, setShowPopup] = useState(false);
  const [editingRole, setEditingRole] = useState(null);
  const [newRole, setNewRole] = useState({ id: '', name: '', des: '' });
  const [error, setError] = useState('');

  useEffect(() => {
    fetchRoles();
  }, []);

  const fetchRoles = async () => {
    try {
      const response = await axios.get('https://localhost:44447/api/Roles');
      setRoles(response.data);
    } catch (error) {
      console.error('Error fetching roles:', error);
    }
  };

  const handleAddRole = () => {
    setShowPopup(true);
  };

  const handleSaveRole = async () => {
    if (!newRole.name && !newRole.des) {
      setError('Data fields cannot be left blank');
      return;
    } else if (!newRole.name) {
      setError('The Name field cannot be left blank');
      return;
    } else if (!newRole.des) {
      setError('Des field cannot be left blank');
      return;
    }

    try {
      if (editingRole) {
        await axios.put(`https://localhost:44447/api/Roles/${newRole.id}`, newRole);
        setRoles(roles.map(role => (role.id === editingRole.id ? newRole : role)));
      } else {
        const response = await axios.post('https://localhost:44447/api/Roles', newRole);
        setRoles([...roles, response.data]);
      }
      setShowPopup(false);
      setNewRole({ id: '', name: '', des: '' });
      setEditingRole(null);
      setError('');
    } catch (error) {
      console.error('Error saving role:', error);
      setError('An error occurred while saving the role');
    }
  };

  const handleEditRole = (role) => {
    setEditingRole(role);
    setNewRole(role);
    setShowPopup(true);
  };

  const handleClosePopup = () => {
    setShowPopup(false);
    setNewRole({ id: '', name: '', des: '' });
    setEditingRole(null);
    setError('');
  };

  return (
    <div className="container">
      <div className="header">
        <h1>Roles List</h1>
        <button className="add-role-button" onClick={handleAddRole}>Add Role</button>
      </div>
      <table className="table">
        <thead>
          <tr>
            {/*<th>Id</th>*/}
            <th>Name</th>
            <th>Des</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {roles.map(role => (
            <tr key={role.id}>
              {/*<td>{role.id}</td>*/}
              <td>{role.name}</td>
              <td>{role.des}</td>
              <td>
                <button className="action-button" onClick={() => handleEditRole(role)}>Edit</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      {showPopup && (
        <div className="popup">
          <div className="popup-inner">
            <div className="popup-header">
              <h2>{editingRole ? 'Edit Role' : 'Add Role'}</h2>
              <button className="close-button" onClick={handleClosePopup}>X</button>
            </div>
            <form>
              {editingRole && <input type="text" value={newRole.id} readOnly />}
              <input
                type="text"
                placeholder="Name"
                value={newRole.name}
                onChange={(e) => setNewRole({ ...newRole, name: e.target.value })}
              />
              <input
                type="text"
                placeholder="Des"
                value={newRole.des}
                onChange={(e) => setNewRole({ ...newRole, des: e.target.value })}
              />
              {error && <p className="error-message">{error}</p>}
              <button type="button" onClick={handleSaveRole}>{editingRole ? 'Save' : 'Add'}</button>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};

export default RolesListScreen;
