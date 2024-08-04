import React, { useState, useEffect } from 'react';
import axios from 'axios';
import 'bootstrap/dist/css/bootstrap.min.css';
import './ExerciseSearchBox.css';

const ExerciseSearchBox = ({ closePopup, setSelectedExercise }) => {
  const [muscleGroups, setMuscleGroups] = useState([]);
  const [equipments, setEquipments] = useState([]);
  const [selectedEquipment, setSelectedEquipment] = useState('');
  const [selectedMuscleGroups, setSelectedMuscleGroups] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [exercises, setExercises] = useState([]);
  const [error, setError] = useState(null);
  const [selectedExerciseId, setSelectedExerciseId] = useState(null);
  const [currentPage, setCurrentPage] = useState(1);

  const ITEMS_PER_PAGE = 5;
  //const baseAPI = {env.}

  useEffect(() => {
    const fetchMuscleGroups = async () => {
      try {
        const response = await axios.get('https://localhost:44447/api/MuscleGroups/get-list?PageNumber=1&PageSize=10');
        setMuscleGroups(response.data.items);
      } catch (err) {
        setError('Failed to fetch muscle groups. Please try again later.');
      }
    };

    const fetchEquipments = async () => {
      try {
        const response = await axios.get('https://localhost:44447/api/Equipments/get-all?PageNumber=1&PageSize=10');
        setEquipments(response.data.items);
      } catch (err) {
        setError('Failed to fetch equipments. Please try again later.');
      }
    };

    fetchMuscleGroups();
    fetchEquipments();
  }, []);

  const handleEquipmentChange = (event) => {
    setSelectedEquipment(event.target.value);
  };

  const handleMuscleGroupChange = (event) => {
    const { value, checked } = event.target;
    if (checked) {
      setSelectedMuscleGroups([...selectedMuscleGroups, value]);
    } else {
      setSelectedMuscleGroups(selectedMuscleGroups.filter((id) => id !== value));
    }
  };

  const handleSearchTermChange = (event) => {
    setSearchTerm(event.target.value);
  };

  const handleSearch = async () => {
    try {
      const response = await axios.get(
        `https://localhost:44447/api/Exercises/search?exerciseName=${searchTerm}&equipmentId=${selectedEquipment}&muscleGroupIds=${selectedMuscleGroups.join(',')}`
      );
      setExercises(response.data);
      setError(null);
      setCurrentPage(1); // Reset to first page on new search
    } catch (err) {
      setError('Failed to fetch exercises. Please try again later.');
    }
  };

  const handleExerciseClick = (exerciseId) => {
    setSelectedExerciseId(exerciseId);
  };

  const handleOkClick = () => {
    const selectedExercise = exercises.find(exercise => exercise.exerciseId === selectedExerciseId);
    setSelectedExercise(selectedExercise);
    closePopup();
  };

  const handlePageChange = (page) => {
    setCurrentPage(page);
  };

  // Calculate pagination
  const startIndex = (currentPage - 1) * ITEMS_PER_PAGE;
  const endIndex = startIndex + ITEMS_PER_PAGE;
  const paginatedExercises = exercises.slice(startIndex, endIndex);
  const totalPages = Math.ceil(exercises.length / ITEMS_PER_PAGE);

  return (
    <div className="modal show d-block" role="dialog">
      <div className="modal-dialog modal-lg" role="document">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">Search Exercises</h5>
            <button type="button" className="close" onClick={closePopup}>
              <span>&times;</span>
            </button>
          </div>
          <div className="modal-body">
            {error && <div className="alert alert-danger">{error}</div>}
            <input
              type="text"
              className="form-control mb-3"
              value={searchTerm}
              onChange={handleSearchTermChange}
              placeholder="Search term"
            />
            <div className="mb-3">
              <h6>Select Equipment</h6>
              <select className="form-control" value={selectedEquipment} onChange={handleEquipmentChange}>
                <option value="">Select Equipment</option>
                {equipments.map((equipment) => (
                  <option key={equipment.equipmentId} value={equipment.equipmentId}>
                    {equipment.equipmentName}
                  </option>
                ))}
              </select>
            </div>
            <div className="mb-3">
              <h6>Select Muscle Groups</h6>
              <div className="form-check form-check-inline">
                {muscleGroups.map((muscleGroup) => (
                  <div key={muscleGroup.muscleGroupId} className="form-check form-check-inline">
                    <input
                      className="form-check-input"
                      type="checkbox"
                      value={muscleGroup.muscleGroupId}
                      onChange={handleMuscleGroupChange}
                      id={`muscleGroup-${muscleGroup.muscleGroupId}`}
                    />
                    <label className="form-check-label" htmlFor={`muscleGroup-${muscleGroup.muscleGroupId}`}>
                      {muscleGroup.muscleGroupName}
                    </label>
                  </div>
                ))}
              </div>
            </div>
            <button className="btn btn-primary" onClick={handleSearch}>Search</button>
            <div className="mt-3">
              <h6>Exercises</h6>
              {paginatedExercises.length > 0 ? (
                <div className="exercise-list-container">
                  <ul className="list-group">
                    {paginatedExercises.map((exercise) => (
                      <li
                        key={exercise.exerciseId}
                        className={`list-group-item ${exercise.exerciseId === selectedExerciseId ? 'active' : ''}`}
                        onClick={() => handleExerciseClick(exercise.exerciseId)}
                        style={{ cursor: 'pointer' }}
                      >
                        {exercise.exerciseName} - {exercise.type}
                      </li>
                    ))}
                  </ul>
                </div>
              ) : (
                <p>No exercises found.</p>
              )}
            </div>
            <div className="pagination-container mt-3">
              {Array.from({ length: totalPages }, (_, i) => (
                <button
                  key={i + 1}
                  className={`btn ${i + 1 === currentPage ? 'btn-primary' : 'btn-secondary'} ml-1`}
                  onClick={() => handlePageChange(i + 1)}
                >
                  {i + 1}
                </button>
              ))}
            </div>
            <button className="btn btn-success mt-3" onClick={handleOkClick} disabled={!selectedExerciseId}>
              OK
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ExerciseSearchBox;
