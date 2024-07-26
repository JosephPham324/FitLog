//import React from 'react';
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
//  return (
//    <div className="exercise-list-screen">
//      <header className="header">
//           <h1>Exercises</h1>
//        <button className="create-new-button">Create New</button>
//      </header>
//      <div className="search-bar">
//        <input type="text" placeholder="Search exercise name" />
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
//        {exercises.map((exercise, index) => (
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


import React, { useState } from 'react';
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

  const handleSearchChange = (event) => {
    setSearchTerm(event.target.value);
  };

  const filteredExercises = exercises.filter((exercise) =>
    exercise.name.toLowerCase().includes(searchTerm.toLowerCase())
  );

  return (
    <div className="exercise-list-screen">
      <header className="header">
        <h1>Exercises</h1>
        <button className="create-new-button">Create New</button>
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
    </div>
  );
};

export default ExerciseListScreen;
