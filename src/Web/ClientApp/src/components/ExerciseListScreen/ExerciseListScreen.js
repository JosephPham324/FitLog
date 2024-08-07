import React, { useState, useEffect } from 'react';
import axiosInstance from '../../utils/axiosInstance';
import './ExerciseListScreen.css';

const ExerciseListScreen = () => {
  const [searchTerm, setSearchTerm] = useState('');
  const [muscleGroup, setMuscleGroup] = useState('');
  const [equipment, setEquipment] = useState('');
  const [exercises, setExercises] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchExercises = async () => {
      try {
        const response = await axiosInstance.get('/Exercises/paginated-all', {
          params: {
            PageNumber: 1,
            PageSize: 10,
          },
        });
        setExercises(response.data.items);
        setLoading(false);
      } catch (error) {
        console.error('Error fetching exercises:', error);
        setError('Failed to fetch exercises');
        setLoading(false);
      }
    };

    fetchExercises();
  }, []);

  const handleSearchChange = async (event) => {
    setSearchTerm(event.target.value);
    try {
      const response = await axiosInstance.get('/Exercises/search', {
        params: {
          exerciseName: event.target.value,
          equipmentId: equipment,
          muscleGroupIds: muscleGroup,
        },
      });
      setExercises(response.data);
    } catch (error) {
      console.error('Error searching exercises:', error);
      setError('Failed to search exercises');
    }
  };

  const handleMuscleGroupChange = async (event) => {
    setMuscleGroup(event.target.value);
    try {
      const response = await axiosInstance.get('/Exercises/search', {
        params: {
          exerciseName: searchTerm,
          equipmentId: equipment,
          muscleGroupIds: event.target.value,
        },
      });
      setExercises(response.data);
    } catch (error) {
      console.error('Error filtering by muscle group:', error);
      setError('Failed to filter exercises');
    }
  };

  const handleEquipmentChange = async (event) => {
    setEquipment(event.target.value);
    try {
      const response = await axiosInstance.get('/Exercises/search', {
        params: {
          exerciseName: searchTerm,
          equipmentId: event.target.value,
          muscleGroupIds: muscleGroup,
        },
      });
      setExercises(response.data);
    } catch (error) {
      console.error('Error filtering by equipment:', error);
      setError('Failed to filter exercises');
    }
  };

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
          <select value={muscleGroup} onChange={handleMuscleGroupChange}>
            <option value="">All Muscle Groups</option>
            <option value="upper-back">Upper Back</option>
            <option value="rear-delts">Rear Delts</option>
            <option value="glutes">Glutes</option>
            <option value="middle-delts">Middle Delts</option>
          </select>
          <select value={equipment} onChange={handleEquipmentChange}>
            <option value="">All Equipment</option>
            <option value="dumbbells">Dumbbells</option>
            <option value="barbell">Barbell</option>
            <option value="machine">Machine</option>
          </select>
        </div>
      </div>
      <h2>Recent Exercises</h2>
      {loading ? (
        <p>Loading...</p>
      ) : error ? (
        <p>{error}</p>
      ) : (
        <ul className="exercise-list">
          {exercises.map((exercise) => (
            <li key={exercise.exerciseId} className="exercise-item">
              <div className="exercise-info">
                <span className="exercise-name">{exercise.exerciseName}</span>
                <span className="exercise-muscles">{exercise.type}</span>
              </div>
              <span className="exercise-times">{exercise.times} times</span>
              <button className="info-button">i</button>
            </li>
          ))}
        </ul>
      )}
      <div className="action-buttons">
        <button className="add-superset-button">Add as Superset</button>
        <button className="add-exercises-button">Add Exercises</button>
      </div>
    </div>
  );
};

export default ExerciseListScreen;
