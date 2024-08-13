import React, { useEffect, useState } from 'react';
import axiosInstance from '../../utils/axiosInstance'; // Adjust the import path according to your project structure
import { useParams, useNavigate } from 'react-router-dom'; // Use useNavigate instead of useHistory
import './WorkoutProgramsDetail.css';

export const WorkoutProgramsDetail = () => {
  const { id } = useParams(); // Using useParams to get the templateId from the URL
  const [programDetail, setProgramDetail] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const navigate = useNavigate(); // useNavigate for navigation

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

  const handleUseTemplateClick = (templateId) => {
    navigate(`/workout-log/create/${templateId}`);
  };

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
              <p><strong>Number of weeks:</strong> {programDetail.numberOfWeeks}</p>
              <p><strong>Number of training sessions per week:</strong> {programDetail.daysPerWeek}</p>
              <p><strong>Public Program:</strong> {programDetail.publicProgram ? 'Yes' : 'No'}</p>
              <p className="mb-3"><strong>Coach name:</strong> {programDetail.creatorFullName}</p>
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
                  <div className="exercise-list">
                    <h4>Exercises</h4>
                    <div className="font-bold mt-3 mb-3">
                      <button
                        onClick={() => handleUseTemplateClick(workout.workoutTemplate.id)}
                        className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
                      >
                        Use template
                      </button>
                    </div>
                    <table className="table">
                      <thead>
                        <tr>
                          <th>Exercise Name</th>
                          <th>Sets Recommendation</th>
                          <th>Intensity Percentage</th>
                          <th>RPE Recommendation</th>
                          <th>Weights Used</th>
                          <th>Numbers of Reps</th>
                          <th>Types</th>
                          <th>Note</th>
                        </tr>
                      </thead>
                      <tbody>
                        {workout.workoutTemplate.workoutTemplateExercises.map((exercise) => (
                          <tr key={exercise.exerciseTemlateId}>
                            <td>{exercise.exercise.exerciseName}</td>
                            <td>{exercise.setsRecommendation}</td>
                            <td>{exercise.intensityPercentage}%</td>
                            <td>{exercise.rpeRecommendation}</td>
                            <td>{exercise.weightsUsed}</td>
                            <td>{exercise.numbersOfReps}</td>
                            <td>{exercise.exercise.type}</td>
                            <td>{exercise.note}</td>
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
