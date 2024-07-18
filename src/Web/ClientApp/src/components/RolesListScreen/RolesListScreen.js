//import React, { useState } from 'react';
//import './RolesListScreen.css';

//const RolesListScreen = () => {
//  const [roles, setRoles] = useState([
//    { id: 1, name: 'Admin', des: 'Administrator' },
//    // Add more roles as needed
//  ]);
//  const [showPopup, setShowPopup] = useState(false);
//  const [editingRole, setEditingRole] = useState(null);
//  const [newRole, setNewRole] = useState({ id: '', name: '', des: '' });

//  const handleAddRole = () => {
//    const nextId = roles.length ? roles[roles.length - 1].id + 1 : 1;
//    setNewRole({ ...newRole, id: nextId });
//    setShowPopup(true);
//  };

//  const handleSaveRole = () => {
//    if (editingRole) {
//      setRoles(roles.map(role => (role.id === editingRole.id ? newRole : role)));
//    } else {
//      setRoles([...roles, newRole]);
//    }
//    setShowPopup(false);
//    setNewRole({ id: '', name: '', des: '' });
//    setEditingRole(null);
//  };

//  const handleEditRole = (role) => {
//    setEditingRole(role);
//    setNewRole(role);
//    setShowPopup(true);
//  };

//  return (
//    <div className="container">
//      <div className="header">
//        <h1>Roles List</h1>
//        <button className="add-role-button" onClick={handleAddRole}>Add Role</button>
//      </div>
//      <table className="table">
//        <thead>
//          <tr>
//            <th>Id</th>
//            <th>Name</th>
//            <th>Des</th>
//            <th>Action</th>
//          </tr>
//        </thead>
//        <tbody>
//          {roles.map(role => (
//            <tr key={role.id}>
//              <td>{role.id}</td>
//              <td>{role.name}</td>
//              <td>{role.des}</td>
//              <td>
//                <button className="action-button" onClick={() => handleEditRole(role)}>Edit</button>
//              </td>
//            </tr>
//          ))}
//        </tbody>
//      </table>
//      {showPopup && (
//        <div className="popup">
//          <div className="popup-inner">
//            <h2>{editingRole ? 'Edit Role' : 'Add Role'}</h2>
//            <form>
//              <input type="text" value={newRole.id} readOnly />
//              <input
//                type="text"
//                placeholder="Name"
//                value={newRole.name}
//                onChange={(e) => setNewRole({ ...newRole, name: e.target.value })}
//              />
//              <input
//                type="text"
//                placeholder="Des"
//                value={newRole.des}
//                onChange={(e) => setNewRole({ ...newRole, des: e.target.value })}
//              />
//              <button type="button" onClick={handleSaveRole}>{editingRole ? 'Save' : 'Add'}</button>
//            </form>
//          </div>
//        </div>
//      )}
//    </div>
//  );
//};

//export default RolesListScreen;

import React, { useState } from 'react';
import './RolesListScreen.css';

const RolesListScreen = () => {
  const [roles, setRoles] = useState([
    { id: 1, name: 'Admin', des: 'Administrator' },
    // Add more roles as needed
  ]);
  const [showPopup, setShowPopup] = useState(false);
  const [editingRole, setEditingRole] = useState(null);
  const [newRole, setNewRole] = useState({ id: '', name: '', des: '' });
  const [error, setError] = useState('');

  const handleAddRole = () => {
    const nextId = roles.length ? roles[roles.length - 1].id + 1 : 1;
    setNewRole({ ...newRole, id: nextId });
    setShowPopup(true);
  };

  const handleSaveRole = () => {
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

    if (editingRole) {
      setRoles(roles.map(role => (role.id === editingRole.id ? newRole : role)));
    } else {
      setRoles([...roles, newRole]);
    }
    setShowPopup(false);
    setNewRole({ id: '', name: '', des: '' });
    setEditingRole(null);
    setError('');
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
            <th>Id</th>
            <th>Name</th>
            <th>Des</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {roles.map(role => (
            <tr key={role.id}>
              <td>{role.id}</td>
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
              <input type="text" value={newRole.id} readOnly />
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

