import React from 'react';
import './CoachApplicationNotification.css'; // Import the CSS file

const Notification = () => {
  return (
    <div className="container">
      <div className="headerw">Your check-in has a status update</div>
       
      <div className="title">Ambition Check-In</div>
      <div className="date">Completed: Tuesday, August 30th, 2022</div>
  
      <div className="message">Great news! This Check-In has been completed! You can view the results by clicking the link below.</div>
      <button className="button">Review Check-In</button>
    </div>
  );
};

export default Notification;
