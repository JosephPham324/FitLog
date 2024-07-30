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
          <div className="program-detail-card">
            <img src={programDetail.programThumbnail} alt={programDetail.programName} className="program-detail-image" />
            <div className="program-detail-info">
              <h1 className="title mt-3"><strong>{programDetail.programName}</strong></h1>
              <p className="mt-2"><strong>Goal:</strong> {programDetail.goal}</p>
              <p><strong>Experience Level:</strong> {programDetail.experienceLevel}</p>
              <p><strong>Gym Type:</strong> {programDetail.gymType}</p>
              <p><strong>Muscles Priority:</strong> {programDetail.musclesPriority}</p>
              <p><strong>Age Group:</strong> {programDetail.ageGroup}</p>
              <p><strong>Public Program:</strong> {programDetail.publicProgram ? 'Yes' : 'No'}</p>
              <p className="mb-3"><strong>Created by:</strong> {programDetail.userName}</p>
            </div>
          </div>
          <div className="program-overview">
            <div>
              <h1><strong>Program Workouts</strong></h1>
              {programDetail.programWorkouts.map((workout) => (
                <div key={workout.programWorkoutId} className="workout-card">
                  <h3>Week {workout.weekNumber}, Day {workout.orderInWeek}</h3>
                  <p><strong>Workout:</strong> {workout.workoutTemplate.templateName}</p>
                  <p><strong>Duration:</strong> {workout.workoutTemplate.duration}</p>
                  <p><strong>Created by:</strong> {workout.workoutTemplate.creatorName}</p>
                  <div className="exercise-list">
                    <h4>Exercises</h4>
                    <table className="table">
                      <thead>
                        <tr>
                          <th>Note</th>
                          <th>Sets Recommendation</th>
                          <th>Intensity Percentage</th>
                          <th>RPE Recommendation</th>
                          <th>Weights Used</th>
                          <th>Numbers of Reps</th>
                        </tr>
                      </thead>
                      <tbody>
                        {workout.workoutTemplate.workoutTemplateExercises.map((exercise) => (
                          <tr key={exercise.exerciseTemlateId}>
                            <td>{exercise.note}</td>
                            <td>{exercise.setsRecommendation}</td>
                            <td>{exercise.intensityPercentage}%</td>
                            <td>{exercise.rpeRecommendation}</td>
                            <td>{exercise.weightsUsed}</td>
                            <td>{exercise.numbersOfReps}</td>
                          </tr>
                        ))}
                      </tbody>
                    </table>
                  </div>
                </div>
              ))}
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
