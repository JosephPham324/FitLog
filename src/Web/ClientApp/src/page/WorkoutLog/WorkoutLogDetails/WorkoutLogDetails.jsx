import React, { useState, useEffect } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import axiosInstance from '../../../utils/axiosInstance';
import { useParams, useNavigate } from 'react-router-dom';

const ReadOnlyWorkoutLogPage = () => {
    const { workoutLogId } = useParams(); // Get workoutLogId from URL parameters
    const [workoutName, setWorkoutName] = useState('');
    const [workoutNote, setWorkoutNote] = useState('');
    const [duration, setDuration] = useState(0); // Duration in seconds
    const [rows, setRows] = useState([]); // State for active rows

    const history = useNavigate();

    useEffect(() => {
        // Fetch data when the component mounts
        const fetchWorkoutLog = async () => {
            try {
                const response = await axiosInstance.get(`/WorkoutLog/${workoutLogId}`);
                const logData = response.data;
                setWorkoutName(logData.workoutLogName);
                setWorkoutNote(logData.note);
                setDuration(parseDuration(logData.duration));
                setRows(logData.exerciseLogs.map(log => ({
                    exercise: { exerciseId: log.exerciseId, exerciseName: log.exerciseName }, // Assuming exerciseName is provided
                    exerciseLogId: log.exerciseLogId, // Use appropriate ID if available
                    sets: log.numberOfSets,
                    data: JSON.parse(log.weightsUsed).map((weight, index) => ({
                        weight,
                        reps: JSON.parse(log.numberOfReps)[index]
                    })),
                    note: log.note,
                    isDeleted: log.isDeleted // Retain deletion state
                })));
            } catch (error) {
                console.error('Error fetching workout log:', error);
            }
        };

        fetchWorkoutLog();
    }, [workoutLogId]);

    const formatDuration = (duration) => {
        const hours = Math.floor(duration / 3600);
        const minutes = Math.floor((duration % 3600) / 60);
        const seconds = duration % 60;
        return `${hours.toString().padStart(2, '0')}:${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`;
    };

    const parseDuration = (duration) => {
        const [hours, minutes, seconds] = duration.split(':').map(Number);
        return (hours * 3600) + (minutes * 60) + seconds;
    };

    const navigateToUpdatePage = () => {
        history(`/workout-log/${workoutLogId}/update`);
    };

    return (
        <div className="container mt-5">
            <h1 className="text-center mb-4">Workout Log Details</h1>
            <div className="mb-3 row">
                <label className="col-sm-2 col-form-label">Workout Log Name</label>
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
                <label className="col-sm-2 col-form-label">Workout Duration</label>
                <div className="col-sm-10">
                    <input
                        type="text"
                        className="form-control"
                        value={formatDuration(duration)}
                        readOnly
                    />
                </div>
            </div>
            <div className="mb-3 row">
                <label className="col-sm-2 col-form-label">Workout Note</label>
                <div className="col-sm-10">
                    <textarea
                        className="form-control"
                        value={workoutNote}
                        readOnly
                        style={{ height: '150px' }}
                    />
                </div>
            </div>
            <div className="mb-3">
                <h4>Exercises</h4>
                <table className="table table-striped workout-table">
                    <thead>
                        <tr>
                            <th>#</th>
                            <th>Exercise</th>
                            <th>Sets</th>
                            <th>Weight (kg)</th>
                            <th>Reps</th>
                            <th>Note</th>
                        </tr>
                    </thead>
                    <tbody>
                        {rows.map((row, rowIndex) => (
                            <tr key={rowIndex} className={row.isDeleted ? 'table-danger' : ''}>
                                <td>{rowIndex + 1}</td>
                                <td>{row.exercise.exerciseName}</td>
                                <td>{row.sets}</td>
                                <td>
                                    {row.data.map((set, setIndex) => (
                                        <div key={`weight-${rowIndex}-${setIndex}`}>
                                            {set.weight}
                                        </div>
                                    ))}
                                </td>
                                <td>
                                    {row.data.map((set, setIndex) => (
                                        <div key={`reps-${rowIndex}-${setIndex}`}>
                                            {set.reps}
                                        </div>
                                    ))}
                                </td>
                                <td>{row.note}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
            <button className="btn btn-primary mt-3" onClick={navigateToUpdatePage}>
                Edit Log
            </button>
        </div>
    );
};

export default ReadOnlyWorkoutLogPage;
