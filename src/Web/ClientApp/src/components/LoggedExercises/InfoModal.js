import React from 'react';
import './InfoModal.css';

const InfoModal = ({ show, onClose }) => {
  if (!show) {
    return null;
  }

  return (
    <div className="info-modal">
      <div className="info-modal-content">
        <span className="close-button" onClick={onClose}>
          &times;
        </span>
        <p>This number shows how many times you have logged an exercise in our database.</p>
      </div>
    </div>
  );
};

export default InfoModal;
