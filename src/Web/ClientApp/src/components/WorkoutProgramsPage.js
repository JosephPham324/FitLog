import React, { useEffect, useState } from 'react';
import axiosInstance from '../utils/axiosInstance'; // Import the configured Axios instance
import { Link } from 'react-router-dom'; // Import Link from react-router-dom
import './WorkoutProgramsPage.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';

export const WorkoutProgramsPage = () => {
  const [programs, setPrograms] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

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

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div className="container">
      <h1 className="title text-center"><strong>Workout Programs</strong></h1>
      <div className="row">
        {programs.map((program) => (
          <div className="col-md-4   mb-4" key={program.programId}>
            <Link to={`/program-details/${program.programId}`} className="program-card">
              <img src={program.programThumbnail} alt={program.programName} className="program-image img-fluid" />
              <div className="program-details">
                <h2 className="program-name">{program.programName}</h2>
                <p className="program-goal"><strong>Goal:</strong> {program.goal}</p>
                <p className="program-level"><strong>Experience Level:</strong> {program.experienceLevel}</p>
                <p className="program-type"><strong>Gym Type:</strong> {program.gymType}</p>
                <p className="program-priority"><strong>Muscles Priority:</strong> {program.musclesPriority}</p>
                <p className="program-group"><strong>Age Group:</strong> {program.ageGroup}</p>
                <p className="program-public"><strong>Public Program:</strong> {program.publicProgram ? 'Yes' : 'No'}</p>
                <p className="program-creator"><strong>Created by:</strong> {program.userName}</p>
              </div>
            </Link>
          </div>
        ))}
      </div>
    </div>
  );
};

export default WorkoutProgramsPage;
