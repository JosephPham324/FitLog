import React, { useEffect, useState } from 'react';
import axiosInstance from '../utils/axiosInstance';
import { useParams } from 'react-router-dom';
import './WorkoutProgramsDetail.css';

export const WorkoutProgramsDetail = () => {
  const { id } = useParams(); // Using useParams to get the id from the URL
  const [programDetail, setProgramDetail] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchProgramDetail = async () => {
      try {
        const response = await axiosInstance.get(`/WorkoutPrograms/details/${id}`, {
          headers: {
            accept: 'application/json',
          },
        });
        setProgramDetail(response.data);
      } catch (error) {
        setError('Error fetching workout program details');
        console.error('Error fetching workout program details:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchProgramDetail();
  }, [id]);

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div className="program-detail-container">
      {programDetail ? (
        <>
          <h1 className="title"><strong>{programDetail.programName}</strong></h1>
          <div className="program-detail-card">
            <img src={programDetail.programThumbnail} alt={programDetail.programName} className="program-detail-image" />
            <div className="program-detail-info">
              <p><strong>Goal:</strong> {programDetail.goal}</p>
              <p><strong>Experience Level:</strong> {programDetail.experienceLevel}</p>
              <p><strong>Gym Type:</strong> {programDetail.gymType}</p>
              <p><strong>Muscles Priority:</strong> {programDetail.musclesPriority}</p>
              <p><strong>Age Group:</strong> {programDetail.ageGroup}</p>
              <p><strong>Public Program:</strong> {programDetail.publicProgram ? 'Yes' : 'No'}</p>
              <p><strong>Created by:</strong> {programDetail.userName}</p>
            </div>
          </div>
        </>
      ) : (
        <div>No program details found</div>
      )}
    </div>
  );
};

export default WorkoutProgramsDetail;
