import React, { useState, useEffect } from 'react';
import WorkoutTable from '../../../components/UpdateWorkoutLog/WorkoutLogTable';
import 'bootstrap/dist/css/bootstrap.min.css';
import axiosInstance from '../../../utils/axiosInstance';
import './CreateWorkoutLog.css'; // Add this line to include the custom CSS file
import { useParams } from 'react-router-dom';

const UpdateWorkoutLogPage = () => {
    const { workoutLogId } = useParams(); // Get workoutLogId from URL parameters
    const [workoutName, setWorkoutName] = useState('');
    const [workoutNote, setWorkoutNote] = useState('');
    const [isNotePopupOpen, setIsNotePopupOpen] = useState(false);
    const [duration, setDuration] = useState(0); // Duration in seconds
    const [rows, setRows] = useState([]); // State for active rows
    const [deletedRows, setDeletedRows] = useState([]); // State for deleted rows

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
                    isDeleted: false // Initial state is not deleted
                })));
            } catch (error) {
                console.error('Error fetching workout log:', error);
            }
        };

        fetchWorkoutLog();

        const timer = setInterval(() => {
            setDuration(prevDuration => prevDuration + 1);
        }, 1000);

        return () => clearInterval(timer);
    }, [workoutLogId]);

    const openNotePopup = () => {
        setIsNotePopupOpen(true);
    };

    const closeNotePopup = () => {
        setIsNotePopupOpen(false);
    };

    const saveNote = () => {
        setIsNotePopupOpen(false);
    };

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

    const saveLog = async () => {
        const exerciseLogs = [...rows, ...deletedRows].map((row, rowIndex) => ({
            exerciseLogId: row.exerciseLogId, // Use appropriate ID if available
            exerciseId: row.exercise.exerciseId,
            orderInSession: rowIndex + 1,
            note: row.note,
            numberOfSets: row.sets,
            weightsUsed: `[${row.data.map(set => set.weight).join(', ')}]`,
            numberOfReps: `[${row.data.map(set => set.reps).join(', ')}]`,
            footageUrls: "[]", // Placeholder, replace with actual footage URLs if available
            isDeleted: row.isDeleted // Include the isDeleted property
        }));

        const logData = {
            workoutLogId,
            workoutLogName: workoutName,
            duration: formatDuration(duration),
            note: workoutNote,
            exerciseLogs
        };

        try {
            const response = await axiosInstance.put(`/WorkoutLog/${workoutLogId}`, logData);
            alert(response.data.success); // Log the data to be saved)
            // Handle response if necessary
        } catch (error) {
            console.error('Error saving log:', error);
        }
    };

    const handleDeleteRow = (rowIndex) => {
        setRows(prevRows => {
            const newRows = [...prevRows];
            newRows[rowIndex].isDeleted = true;
            setDeletedRows(prevDeletedRows => [...prevDeletedRows, newRows[rowIndex]]);
            return prevRows.filter((_, index) => index !== rowIndex);
        });
    };

    return (
        <div className="container mt-5">
            <h1 className="text-center mb-4">Update Workout Log</h1>
            <div className="mb-3 row">
                <label className="col-sm-2 col-form-label">Workout Log Name</label>
                <div className="col-sm-10">
                    <input
                        type="text"
                        className="form-control"
                        value={workoutName}
                        onChange={(e) => setWorkoutName(e.target.value)}
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
                    <button className="btn btn-secondary" onClick={openNotePopup}>Add Note</button>
                </div>
            </div>
            <WorkoutTable rows={rows} setRows={setRows} onDeleteRow={handleDeleteRow} />
            {isNotePopupOpen && (
                <div className="modal show d-block" role="dialog">
                    <div className="modal-dialog modal-lg modal-dialog-centered" role="document">
                        <div className="modal-content">
                            <div className="modal-header">
                                <h5 className="modal-title">Workout Note</h5>
                                <button type="button" className="close" onClick={closeNotePopup}>
                                    <span>&times;</span>
                                </button>
                            </div>
                            <div className="modal-body">
                                <textarea
                                    className="form-control"
                                    value={workoutNote}
                                    onChange={(e) => setWorkoutNote(e.target.value)}
                                    style={{ height: '300px' }} // Adjust height of the textarea
                                />
                            </div>
                            <div className="modal-footer">
                                <button className="btn btn-secondary" onClick={closeNotePopup}>Cancel</button>
                                <button className="btn btn-primary" onClick={saveNote}>Save</button>
                            </div>
                        </div>
                    </div>
                </div>
            )}
            <button className="btn btn-success mt-3" onClick={saveLog}>
                Save Log
            </button>
        </div>
    );
};

export default UpdateWorkoutLogPage;
