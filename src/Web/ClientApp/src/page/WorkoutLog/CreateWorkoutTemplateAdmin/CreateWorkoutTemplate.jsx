import React, { useState } from 'react';
import CreateWorkoutTemplateTable from '../../../components/WorkoutTemplate/WorkoutTemplate';
import 'bootstrap/dist/css/bootstrap.min.css';
import axiosInstance from '../../../utils/axiosInstance';
import './CreateWorkoutTemplate.css';
import { useNavigate } from 'react-router-dom';

const CreateWorkoutTemplatePage = () => {
    const [workoutName, setWorkoutName] = useState('');
    const [workoutNote, setWorkoutNote] = useState('');
    const [isNotePopupOpen, setIsNotePopupOpen] = useState(false);
    const [duration, setDuration] = useState(''); // Duration in minutes
    const [rows, setRows] = useState([]); // Centralized data for rows
    const [isPopupOpen, setIsPopupOpen] = useState(false);
    const [popupMessage, setPopupMessage] = useState('');
    const [isPublic, setIsPublic] = useState(true); // State for IsPublic checkbox
    const navigate = useNavigate(); // Initialize useNavigate for redirection

    const openNotePopup = () => {
        setIsNotePopupOpen(true);
    };

    const closeNotePopup = () => {
        setIsNotePopupOpen(false);
    };

    const saveNote = () => {
        setIsNotePopupOpen(false);
    };

    const handleDurationChange = (e) => {
        setDuration(e.target.value);
    };

    const handleIsPublicChange = (e) => {
        setIsPublic(e.target.checked);
    };

    const saveTemplate = async () => {
        console.log(rows);
        const workoutTemplateExercises = rows.map((row, rowIndex) => ({
            exerciseId: row.exercise.exerciseId,
            orderInSession: rowIndex + 1,
            orderInSuperset: 0, // Assuming no supersets, update if necessary
            note: row.note,
            setsRecommendation: row.sets,
            intensityPercentage: row.sets > 0 ? row.intensityPercentage : 0, // Assuming no intensity recommendation, update if necessary
            rpeRecommendation: 0, // Assuming no RPE recommendation, update if necessary
            weightsUsed: `[${row.data.map(set => set.weight !== null ? set.weight : 0).join(', ')}]`,
            numbersOfReps: `[${row.data.map(set => set.reps !== null? set.reps : 0).join(', ')}]`,
        }));
        console.log(workoutTemplateExercises)
        const templateData = {
            templateName: workoutName,
            duration: `${minutesToTime(duration)}`,
            isPublic, // Include the IsPublic field in the request body
            workoutTemplateExercises
        };

        function minutesToTime(minutes) {
            let hours = Math.floor(minutes / 60);
            let mins = minutes % 60;
            let secs = 0; // Since we are converting from minutes, seconds will always be 0

            // Adding leading zeros if needed
            hours = String(hours).padStart(2, '0');
            mins = String(mins).padStart(2, '0');
            secs = String(secs).padStart(2, '0');

            return `${hours}:${mins}:${secs}`;
        }

        try {

            const response = await axiosInstance.post('/WorkoutTemplates/create-workout-template', templateData);
            if (response.data.success) {
                setPopupMessage('Template saved successfully!');
                setIsPopupOpen(true);
                setTimeout(() => {
                    setIsPopupOpen(false);
                    //navigate('/workout-templates-admin'); // Redirect to root URL after 2 seconds
                }, 2000);
            } else {
                setPopupMessage('Error saving template: ' + response.data.errors.join(', '));
                setIsPopupOpen(true);
                setTimeout(() => {
                    setIsPopupOpen(false);
                }, 2000);
            }
        } catch (error) {
            setPopupMessage('Error saving template. Please try again.');
            setIsPopupOpen(true);
            setTimeout(() => {
                setIsPopupOpen(false);
            }, 2000);
        }
    };

    return (
        <div className="container mt-5">
            <h1 className="text-center mb-4">Create Workout Template</h1>
            <div className="mb-3 row">
                <label className="col-sm-2 col-form-label">Workout Template Name</label>
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
                <label className="col-sm-2 col-form-label">Expected Duration (minutes)</label>
                <div className="col-sm-10">
                    <input
                        type="number"
                        className="form-control"
                        value={duration}
                        onChange={handleDurationChange}
                    />
                </div>
            </div>
            {/*<div className="mb-3 row">*/}
            {/*    <label className="col-sm-2 col-form-label">Make Template Public</label>*/}
            {/*    <div className="col-sm-10">*/}
            {/*        <input*/}
            {/*            type="checkbox"*/}
            {/*            className="form-check-input"*/}
            {/*            checked={isPublic}*/}
            {/*            onChange={handleIsPublicChange}*/}
            {/*        />*/}
            {/*    </div>*/}
            {/*</div>*/}
            <div className="mb-3 row">
                <label className="col-sm-2 col-form-label">Workout Note</label>
                <div className="col-sm-10">
                    <button className="btn btn-secondary" onClick={openNotePopup}>Add Note</button>
                </div>
            </div>
            <CreateWorkoutTemplateTable rows={rows} setRows={setRows} /> {/* Updated component */}
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
            {isPopupOpen && (
                <div className="modal show d-block" role="dialog">
                    <div className="modal-dialog" role="document">
                        <div className="modal-content">
                            <div className="modal-body text-center">
                                <p>{popupMessage}</p>
                                {popupMessage === 'Template saved successfully!' && (
                                    <div>
                                        <p>Redirecting to the home page...</p>
                                    </div>
                                )}
                            </div>
                        </div>
                    </div>
                </div>
            )}
            <button className="btn btn-success mt-3" onClick={saveTemplate}>
                Save Template
            </button>
        </div>
    );
};

export default CreateWorkoutTemplatePage;
