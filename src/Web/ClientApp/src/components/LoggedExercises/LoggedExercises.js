import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom'; // Import useNavigate from React Router
import axiosInstance from '../../utils/axiosInstance';
import InfoModal from './InfoModal'; // Adjust the import path according to your project structure
import './LoggedExercises.css';
import { Pagination } from 'react-bootstrap';

const LoggedExercises = () => {
  const [exercises, setExercises] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 10;

  const navigate = useNavigate(); // Initialize useNavigate for redirection

  useEffect(() => {
    const fetchLoggedExercises = async () => {
      try {
        const response = await axiosInstance.get('https://localhost:44447/api/Statistics/exercise/logged-exercises');
        setExercises(response.data);
        setLoading(false);
      } catch (error) {
        console.error('Error fetching logged exercises:', error);
        setError('Failed to fetch logged exercises');
        setLoading(false);
      }
    };

    fetchLoggedExercises();
  }, []);

  const openModal = () => {
    setIsModalOpen(true);
  };

  const closeModal = () => {
    setIsModalOpen(false);
  };

  const handleRowClick = (id) => {
    navigate(`/statistics/exercises/${id}`);
  };

  const totalPages = Math.ceil(exercises.length / itemsPerPage);
  const currentExercises = exercises.slice((currentPage - 1) * itemsPerPage, currentPage * itemsPerPage);

  const handlePageChange = (page) => {
    setCurrentPage(page);
  };

  const handleNextPage = () => {
    if (currentPage < totalPages) {
      setCurrentPage(currentPage + 1);
    }
  };

  const handlePrevPage = () => {
    if (currentPage > 1) {
      setCurrentPage(currentPage - 1);
    }
  };

  return (
    <div className="logged-exercises-screen">
      <header className="header">
        <h1>Logged Exercises</h1>
      </header>
      {loading ? (
        <p>Loading...</p>
      ) : error ? (
        <p>{error}</p>
      ) : (
        <ul className="exercise-list">
          {currentExercises.map((exercise, index) => (
            <li
              key={index}
              className="exercise-item"
              onClick={() => handleRowClick(exercise.exerciseKey.exerciseId)} // Make the row clickable
              style={{ cursor: 'pointer' }} // Add pointer cursor for better UX
            >
              <div className="exercise-info">
                <span className="exercise-name">{exercise.exerciseKey.exerciseName}</span>
                <span className="exercise-times">{exercise.logCount} times</span>
              </div>
              <button
                className="info-button"
                onClick={(e) => { e.stopPropagation(); openModal(); }} // Prevent row click when clicking info button
              >
                i
              </button>
            </li>
          ))}
        </ul>
      )}
      <Pagination className="pagination-controls">
        <Pagination.Prev onClick={handlePrevPage} disabled={currentPage === 1} />
        {[...Array(totalPages)].map((_, index) => (
          <Pagination.Item
            key={index + 1}
            active={currentPage === index + 1}
            onClick={() => handlePageChange(index + 1)}
          >
            {index + 1}
          </Pagination.Item>
        ))}
        <Pagination.Next onClick={handleNextPage} disabled={currentPage === totalPages} />
      </Pagination>
      <InfoModal show={isModalOpen} onClose={closeModal} />
    </div>
  );
};

export default LoggedExercises;
