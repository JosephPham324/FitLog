import React, { useState, useEffect } from 'react';
import UpdateWorkoutTemplateTable from '../../../components/UpdateWorkoutTemplate/WorkoutLogTable';
import 'bootstrap/dist/css/bootstrap.min.css';
import axiosInstance from '../../../utils/axiosInstance';
import './UpdateWorkoutTemplate.css';
import { useParams, useNavigate } from 'react-router-dom';

const UpdateWorkoutTemplatePage = () => {
    const { templateId } = useParams(); // Get the workout template ID from URL parameters
    const [workoutName, setWorkoutName] = useState('');
    const [workoutNote, setWorkoutNote] = useState('');
    const [isNotePopupOpen, setIsNotePopupOpen] = useState(false);
    const [duration, setDuration] = useState(''); // Duration in minutes
    const [rows, setRows] = useState([]); // Centralized data for rows
    const [deletedRows, setDeletedRows] = useState([]); // State for deleted rows
    const [isPopupOpen, setIsPopupOpen] = useState(false);
    const [popupMessage, setPopupMessage] = useState('');
    const navigate = useNavigate(); // Initialize useNavigate for redirection

    useEffect(() => {
        const fetchWorkoutTemplate = async () => {
            try {
                const response = await axiosInstance.get(`/WorkoutTemplates/get-workout-template-details/${templateId}`);
                const templateData = response.data;
                console.log(templateData)
                setWorkoutName(templateData.templateName);
                setDuration(parseInt(templateData.duration));
                setRows(templateData.workoutTemplateExercises.map(exercise => ({
                    exercise: { exerciseId: exercise.exercise.exerciseId, exerciseName: exercise.exercise.exerciseName },
                    sets: exercise.setsRecommendation,
                    intensity: exercise.intensityPercentage, // Assuming intensity is a single value
                    data: JSON.parse(exercise.weightsUsed).map((weight, index) => ({
                        weight,
                        reps: JSON.parse(exercise.numbersOfReps)[index]
                    })),
                    note: exercise.note,
                    isDeleted: false // Initial state is not deleted
                })));
            } catch (error) {
                console.error('Error fetching workout template:', error);
            }
        };

        fetchWorkoutTemplate();
    }, [templateId]);

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

    const saveTemplate = async () => {
        const workoutTemplateExercises = [...rows, ...deletedRows].map((row, rowIndex) => ({
            exerciseId: row.exercise.exerciseId,
            orderInSession: rowIndex + 1,
            orderInSuperset: 0, // Assuming no supersets, update if necessary
            note: row.note,
            setsRecommendation: row.sets,
            intensityPercentage: row.intensity, // Single value for intensity
            rpeRecommendation: 0, // Assuming no RPE recommendation, update if necessary
            weightsUsed: `[${row.data.map(set => set.weight).join(', ')}]`,
            numbersOfReps: `[${row.data.map(set => set.reps).join(', ')}]`,
            isDeleted: row.isDeleted
        }));

        const templateData = {
            id: templateId,
            templateName: workoutName,
            duration: `${duration}`,
            isPublic: true, // Assuming template is public, update if necessary
            workoutTemplateExercises
        };

        try {
            const response = await axiosInstance.put(`/WorkoutTemplates/update-workout-template/${templateId}`, templateData);
            if (response.data.success) {
                setPopupMessage('Template updated successfully!');
                setIsPopupOpen(true);
                setTimeout(() => {
                    setIsPopupOpen(false);
                    navigate('/'); // Redirect to root URL after 2 seconds
                }, 2000);
            } else {
                setPopupMessage('Error updating template: ' + response.data.errors.join(', '));
                setIsPopupOpen(true);
                setTimeout(() => {
                    setIsPopupOpen(false);
                }, 2000);
            }
        } catch (error) {
            setPopupMessage('Error updating template. Please try again.');
            setIsPopupOpen(true);
            setTimeout(() => {
                setIsPopupOpen(false);
            }, 2000);
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
            <h1 className="text-center mb-4">Update Workout Template</h1>
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
            <div className="mb-3 row">
                <label className="col-sm-2 col-form-label">Workout Note</label>
                <div className="col-sm-10">
                    <button className="btn btn-secondary" onClick={openNotePopup}>Add Note</button>
                </div>
            </div>
            <UpdateWorkoutTemplateTable rows={rows} setRows={setRows} onDeleteRow={handleDeleteRow} /> {/* Updated component */}
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
                                {popupMessage === 'Template updated successfully!' && (
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

export default UpdateWorkoutTemplatePage;
