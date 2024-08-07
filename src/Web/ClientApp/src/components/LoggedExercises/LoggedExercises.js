import React, { useState, useEffect } from 'react';
import axiosInstance from '../../utils/axiosInstance'; // Adjust the import path according to your project structure
import './LoggedExercises.css';

const LoggedExercises = () => {
  const [exercises, setExercises] = useState([]);

  const fetchLoggedExercises = async () => {
    try {
      const response = await axiosInstance.get('https://localhost:44447/api/Statistics/exercise/logged-exercises');
      setExercises(response.data);
    } catch (error) {
      console.error('Error fetching logged exercises:', error);
    }
  };

  useEffect(() => {
    fetchLoggedExercises();
  }, []);

  return (
    <div className="logged-exercises">
      <h1>Logged Exercises</h1>
      <table>
        <thead>
          <tr>
            <th>Exercise Name</th>
            <th>Log Count</th>
          </tr>
        </thead>
        <tbody>
          {exercises.map((exercise, index) => (
            <tr key={index}>
              <td>{exercise.exerciseKey.exerciseName}</td>
              <td>{exercise.logCount}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default LoggedExercises;
