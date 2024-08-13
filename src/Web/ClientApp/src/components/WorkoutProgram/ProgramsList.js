import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import axiosInstance from '../../utils/axiosInstance';

const ProgramsList = ({ filters = null, search = null, maxItemsPerPage = 4, showPagination = true }) => {
  const [programs, setPrograms] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [currentPage, setCurrentPage] = useState(1);

  useEffect(() => {
    const fetchPrograms = async () => {
      try {
        const response = await axiosInstance.get('/WorkoutPrograms', {
          headers: {
            accept: 'application/json',
          },
        });
        setPrograms(response.data);
      } catch (error) {
        setError('Error fetching workout programs');
        console.error('Error fetching workout programs:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchPrograms();
  }, []);

  let filteredPrograms = programs;
  if (filters != null || search !== null) {
    filteredPrograms = programs.filter(program => {
      return (
        (program.programName.toLowerCase().includes(search.toLowerCase())) &&
        (filters.goal === '' || program.goal === filters.goal) &&
        (filters.experienceLevel === '' || program.experienceLevel === filters.experienceLevel) &&
        (filters.gymType === '' || program.gymType === filters.gymType) &&
        (filters.musclesPriority === '' || program.musclesPriority.split(', ').some(muscle => filters.musclesPriority.includes(muscle)))
      );
    }).sort((a, b) => {
      const priorityOrder = ['experienceLevel', 'gymType', 'fitnessGoal', 'musclesPriority', 'daysPerWeek'];
      const aPriority = priorityOrder.findIndex(item => a[item] === filters[item]);
      const bPriority = priorityOrder.findIndex(item => b[item] === filters[item]);
      return aPriority - bPriority;
    });
  }

  const totalPages = Math.ceil(filteredPrograms.length / maxItemsPerPage);
  const startIndex = (currentPage - 1) * maxItemsPerPage;
  const currentPrograms = filteredPrograms.slice(startIndex, startIndex + maxItemsPerPage);

  const handlePageChange = (newPage) => {
    if (newPage > 0 && newPage <= totalPages) {
      setCurrentPage(newPage);
    }
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div>
      <div className="row">
        {currentPrograms.map((program) => (
          <div className="col-md-3 mb-4" key={program.programId}>
            <Link to={`/workout-programs/${program.programId}`} className="program-card">
              <img src={program.programThumbnail} alt={program.programName} className="program-image img-fluid" />
              <div className="program-details">
                <h2 className="program-name">{program.programName}</h2>
                <p className="program-goal"><strong>Goal:</strong> {program.goal}</p>
                <p className="program-level"><strong>Experience Level:</strong> {program.experienceLevel}</p>
                <p className="program-type"><strong>Gym Type:</strong> {program.gymType}</p>
                <p className="program-priority"><strong>Muscles Priority:</strong> {program.musclesPriority}</p>
                <p className="program-group"><strong>Age Group:</strong> {program.ageGroup}</p>
                <p className="program-creatorfullname"><strong>Coach name:</strong> {program.creatorFullName}</p>
              </div>
            </Link>
          </div>
        ))}
      </div>

      {showPagination && (
        <div className="pagination-controls d-flex justify-content-center mb-5">
          <button
            className="btn btn-primary mx-2"
            onClick={() => handlePageChange(currentPage - 1)}
            disabled={currentPage === 1}
          >
            Previous
          </button>
          <span className="mx-2 align-self-center">Page {currentPage} of {totalPages}</span>
          <button
            className="btn btn-primary mx-2"
            onClick={() => handlePageChange(currentPage + 1)}
            disabled={currentPage === totalPages}
          >
            Next
          </button>
        </div>
      )}
    </div>
  );
};

export default ProgramsList;
