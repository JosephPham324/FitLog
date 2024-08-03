//import React, { useState } from 'react';
//import './ExerciseListScreen.css';

//const exercises = [
//  { name: 'Inverted Row', muscles: 'Upper Back, Lats', times: 2 },
//  { name: 'Y Raise', muscles: 'Rear Delts, Upper Back', times: 17 },
//  { name: 'Bodyweight BSS', muscles: 'Glutes, Quadriceps, Abductors', times: 1 },
//  { name: 'BTN Press', muscles: 'Middle Delts, Front Delts, Upper Back', times: 1 },
//  { name: 'Hammer Preacher Curl', muscles: 'Biceps, Forearms', times: 11 },
//  { name: 'Cable Skier', muscles: 'Rear Delts, Middle Delts', times: 1 },
//  { name: 'Chest Supported Row (Machine)', muscles: 'Upper Back, Lats, Biceps', times: 26 },
//];

//const ExerciseListScreen = () => {
//  const [searchTerm, setSearchTerm] = useState('');

//  const handleSearchChange = (event) => {
//    setSearchTerm(event.target.value);
//  };

//  const filteredExercises = exercises.filter((exercise) =>
//    exercise.name.toLowerCase().includes(searchTerm.toLowerCase())
//  );

//  return (
//    <div className="exercise-list-screen">
//      <header className="header">
//        <h1>Exercises</h1>
//        <button className="create-new-button">Create New</button>
//      </header>
//      <div className="search-bar">
//        <input
//          type="text"
//          placeholder="Search exercise name"
//          value={searchTerm}
//          onChange={handleSearchChange}
//        />
//        <div className="filter-buttons">
//          <select>
//            <option>All Muscle Groups</option>
//            <option>Upper Back</option>
//            <option>Rear Delts</option>
//            <option>Glutes</option>
//            <option>Middle Delts</option>
//          </select>
//          <select>
//            <option>All Equipment</option>
//            <option>Dumbbells</option>
//            <option>Barbell</option>
//            <option>Machine</option>
//          </select>
//        </div>
//      </div>
//      <h2>Recent Exercises</h2>
//      <ul className="exercise-list">
//        {filteredExercises.map((exercise, index) => (
//          <li key={index} className="exercise-item">
//            <div className="exercise-info">
//              <span className="exercise-name">{exercise.name}</span>
//              <span className="exercise-muscles">{exercise.muscles}</span>
//            </div>
//            <span className="exercise-times">{exercise.times} times</span>
//            <button className="info-button">i</button>
//          </li>
//        ))}
//      </ul>
//      <div className="action-buttons">
//        <button className="add-superset-button">Add as Superset</button>
//        <button className="add-exercises-button">Add Exercises</button>
//      </div>
//    </div>
//  );
//};

//export default ExerciseListScreen;

import React, { useState, useEffect } from 'react';
import './ExerciseListScreen.css';

const exercises = [
  { name: 'Inverted Row', muscles: 'Upper Back, Lats', times: 2 },
  { name: 'Y Raise', muscles: 'Rear Delts, Upper Back', times: 17 },
  { name: 'Bodyweight BSS', muscles: 'Glutes, Quadriceps, Abductors', times: 1 },
  { name: 'BTN Press', muscles: 'Middle Delts, Front Delts, Upper Back', times: 1 },
  { name: 'Hammer Preacher Curl', muscles: 'Biceps, Forearms', times: 11 },
  { name: 'Cable Skier', muscles: 'Rear Delts, Middle Delts', times: 1 },
  { name: 'Chest Supported Row (Machine)', muscles: 'Upper Back, Lats, Biceps', times: 26 },
];

const ExerciseListScreen = () => {
  const [searchTerm, setSearchTerm] = useState('');
  const [showPopup, setShowPopup] = useState(false);
  const [muscleGroups, setMuscleGroups] = useState([]);
  const [equipment, setEquipment] = useState([]);
  const [selectedTargetType, setSelectedTargetType] = useState('');
  const [selectedMuscleGroup, setSelectedMuscleGroup] = useState('');
  const [selectedEquipment, setSelectedEquipment] = useState('');

  useEffect(() => {
    // Fetch muscle groups from the API
    fetch('https://localhost:44447/api/MuscleGroups')
      .then((response) => response.json())
      .then((data) => setMuscleGroups(data))
      .catch((error) => console.error('Error fetching muscle groups:', error));

    // Fetch equipment from the API
    fetch('https://localhost:44447/api/Equipment')
      .then((response) => response.json())
      .then((data) => setEquipment(data))
      .catch((error) => console.error('Error fetching equipment:', error));
  }, []);

  const handleSearchChange = (event) => {
    setSearchTerm(event.target.value);
  };

  const filteredExercises = exercises.filter((exercise) =>
    exercise.name.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const handleCreateNewClick = () => {
    setShowPopup(true);
  };

  const handleClosePopup = () => {
    setShowPopup(false);
  };

  const handleTargetTypeClick = (type) => {
    setSelectedTargetType(type);
  };

  const handleMuscleGroupChange = (event) => {
    setSelectedMuscleGroup(event.target.value);
  };

  const handleEquipmentChange = (event) => {
    setSelectedEquipment(event.target.value);
  };

  return (
    <div className="exercise-list-screen">
      <header className="header">
        <h1>Exercises</h1>
        <button className="create-new-button" onClick={handleCreateNewClick}>Create New</button>
      </header>
      <div className="search-bar">
        <input
          type="text"
          placeholder="Search exercise name"
          value={searchTerm}
          onChange={handleSearchChange}
        />
        <div className="filter-buttons">
          <select>
            <option>All Muscle Groups</option>
            <option>Upper Back</option>
            <option>Rear Delts</option>
            <option>Glutes</option>
            <option>Middle Delts</option>
          </select>
          <select>
            <option>All Equipment</option>
            <option>Dumbbells</option>
            <option>Barbell</option>
            <option>Machine</option>
          </select>
        </div>
      </div>
      <h2>Recent Exercises</h2>
      <ul className="exercise-list">
        {filteredExercises.map((exercise, index) => (
          <li key={index} className="exercise-item">
            <div className="exercise-info">
              <span className="exercise-name">{exercise.name}</span>
              <span className="exercise-muscles">{exercise.muscles}</span>
            </div>
            <span className="exercise-times">{exercise.times} times</span>
            <button className="info-button">i</button>
          </li>
        ))}
      </ul>
      <div className="action-buttons">
        <button className="add-superset-button">Add as Superset</button>
        <button className="add-exercises-button">Add Exercises</button>
      </div>

      {showPopup && (
        <div className="create-popup">
          <div className="create-popup-content">
            <div className="popup-header">
              <h2>Create Exercise</h2>
              <button className="close-button" onClick={handleClosePopup}>×</button>
            </div>
            <div className="form-group">
              <label>Name <span className="required">*</span></label>
              <input type="text" placeholder="Enter exercise name" />
            </div>
            <div className="form-group">
              <label>Target Type <span className="required">*</span></label>
              <div className="button-group">
                <button
                  className={`target-button ${selectedTargetType === 'Reps' ? 'active' : ''}`}
                  onClick={() => handleTargetTypeClick('Reps')}
                >
                  Reps
                </button>
                <button
                  className={`target-button ${selectedTargetType === 'Time' ? 'active' : ''}`}
                  onClick={() => handleTargetTypeClick('Time')}
                >
                  Time
                </button>
              </div>
            </div>
            <div className="form-group">
              <label>Equipment <span className="required">*</span></label>
              <select value={selectedEquipment} onChange={handleEquipmentChange}>
                {equipment.map((equip) => (
                  <option key={equip.id} value={equip.name}>
                    {equip.name}
                  </option>
                ))}
              </select>
            </div>
            <div className="form-group">
              <label>Muscle Group <span className="required">*</span></label>
              <select value={selectedMuscleGroup} onChange={handleMuscleGroupChange}>
                {muscleGroups.map((group) => (
                  <option key={group.id} value={group.name}>
                    {group.name}
                  </option>
                ))}
              </select>
            </div>
            <div className="popup-buttons">
              <button onClick={handleClosePopup}>Cancel</button>
              <button>Save</button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default ExerciseListScreen;
