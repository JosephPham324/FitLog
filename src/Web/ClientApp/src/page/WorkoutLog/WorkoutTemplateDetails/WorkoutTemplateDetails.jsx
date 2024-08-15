import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import axiosInstance from '../../../utils/axiosInstance';
import 'bootstrap/dist/css/bootstrap.min.css';
//import './WorkoutTemplateDetailsPage.css';
import { getUserId, getUserRole } from '../../../utils/tokenOperations';

const WorkoutTemplateDetailsPage = () => {
  const { templateId } = useParams(); // Get the workout template ID from URL parameters
  const [workoutName, setWorkoutName] = useState('');
  const [duration, setDuration] = useState(''); // Duration in minutes
  const [rows, setRows] = useState([]); // Centralized data for rows
  const [ownerId, setOwnerId] = useState(''); // Owner ID of the template
    const navigate = useNavigate(); // Initialize useNavigate for redirection
  const userRole = getUserRole();
  function parseDuration(duration) {
        const parts = duration.split(':');
        const hours = parseInt(parts[0], 10);
        const minutes = parseInt(parts[1], 10);
        const seconds = parseInt(parts[2], 10);

      return hours * 60 + minutes + seconds / 60;
    }
  useEffect(() => {
    const fetchWorkoutTemplate = async () => {
      try {
        const response = await axiosInstance.get(`/WorkoutTemplates/get-workout-template-details/${templateId}`);
        const templateData = response.data;
        setWorkoutName(templateData.templateName);

        const durationRegex = /^\d{1,2}:\d{2}:\d{2}$/;

        if (durationRegex.test(templateData.duration)) {
              setDuration(parseDuration(templateData.duration));
        } else {
              setDuration(parseInt(templateData.duration));
        }
          setOwnerId(templateData.createdBy); // Assuming ownerId is part of the response data
          templateData.workoutTemplateExercises.forEach(e => console.log(JSON.parse(e.numbersOfReps)))
        setRows(templateData.workoutTemplateExercises.map(exercise => ({
          exercise: { exerciseId: exercise.exercise.exerciseId, exerciseName: exercise.exercise.exerciseName },
          sets: exercise.setsRecommendation,
          intensity: exercise.intensityPercentage, // Assuming intensity is a single value
          weights: exercise.weightsUsed.replace(/^\[|\]$/g, ''),
          reps: exercise.numbersOfReps.replace(/^\[|\]$/g, ''),
          note: exercise.note
        })));
      } catch (error) {
        console.error('Error fetching workout template:', error);
      }
    };

    fetchWorkoutTemplate();
  }, [templateId]);

  const handleUpdateButtonClick = () => {
    navigate(`/workout-templates/${templateId}/update`);
    };
    const handleReturnButtonClick = () => {
        navigate(-1); // Navigate back to the previous page
    };

    const userId = getUserId();
    console.log(userId);
    console.log(ownerId)

  return (
      <div className="container mt-5">
          <button className="btn btn-secondary mb-4" onClick={handleReturnButtonClick}>
              &#8592;
          </button>
      <h1 className="text-center mb-4">Workout Template Details</h1>
       
      <div className="mb-3 row">
        <label className="col-sm-2 col-form-label">Workout Template Name</label>
        <div className="col-sm-10">
          <input
            type="text"
            className="form-control"
            value={workoutName}
            readOnly
          />
        </div>
      </div>
      <div className="mb-3 row">
        <label className="col-sm-2 col-form-label">Expected Duration (minutes)</label>
        <div className="col-sm-10">
          <input
            type="number"
            className="form-control"
            value={duration}
            readOnly
          />
        </div>
      </div>
      <div className="table-responsive">
        <table className="table table-striped workout-table">
          <thead>
            <tr>
              <th>#</th>
              <th>Exercise</th>
              <th>Sets</th>
              <th>Weight (kg)</th>
              <th>Reps</th>
              <th>Intensity</th>
              <th>Note</th>
            </tr>
          </thead>
          <tbody>
            {rows.map((row, rowIndex) => (
              <tr key={rowIndex}>
                <td>{rowIndex + 1}</td>
                <td>{row.exercise.exerciseName}</td>
                <td>{row.sets}</td>
                <td key={`weight-${rowIndex}`}>
                        {row.weightS}
                </td>
                <td key={`reps-${rowIndex}`}>
                        {row.reps}
                </td>
                <td>{row.intensity}</td>
                <td>{row.note}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
          {(userId === ownerId || userRole?.includes("Administrator")) && (
        <button className="btn btn-primary mt-3" onClick={handleUpdateButtonClick}>
          Update Template
        </button>
      )}
    </div>
  );
};

export default WorkoutTemplateDetailsPage;
