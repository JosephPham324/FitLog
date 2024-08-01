import React, { useState, useEffect } from 'react';
import WorkoutTable from '../components/WorkoutLog/WorkoutLogTable';
import 'bootstrap/dist/css/bootstrap.min.css';
//import '../styles/createWorkoutLog.css'; // Add this line to include the custom CSS file

const WorkoutLog = () => {
    const [workoutName, setWorkoutName] = useState('');
    const [workoutNote, setWorkoutNote] = useState('');
    const [isNotePopupOpen, setIsNotePopupOpen] = useState(false);
    const [duration, setDuration] = useState(0); // Duration in seconds

    useEffect(() => {
        const timer = setInterval(() => {
            setDuration(prevDuration => prevDuration + 1);
        }, 1000);

        return () => clearInterval(timer);
    }, []);

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

    return (
        <div className="container mt-5">
            <h1 className="text-center mb-4">Create Workout Log</h1>
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
            <WorkoutTable />
            {isNotePopupOpen && (
                <div className="modal show d-block" role="dialog">
                    <div className="modal-dialog modal-lg modal-dialog-centered" role="document"> {/* Added modal-lg and modal-dialog-centered */}
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
        </div>
    );
};

export default WorkoutLog;
