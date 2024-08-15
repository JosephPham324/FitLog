import React, { useState, useEffect } from 'react';
import UpdateWorkoutTemplateTable from '../../../components/UpdateWorkoutTemplate/WorkoutLogTable';
import 'bootstrap/dist/css/bootstrap.min.css';
import axiosInstance from '../../../utils/axiosInstance';
import './UpdateWorkoutTemplate.css';
import { useParams, useNavigate } from 'react-router-dom';
import { getUserRole } from '../../../utils/tokenOperations'; 

const UpdateWorkoutTemplatePage = () => {
    const { templateId } = useParams(); // Get the workout template ID from URL parameters
    const [workoutName, setWorkoutName] = useState('');
    const [workoutNote, setWorkoutNote] = useState('');
    const [isPublic, setIsPublic] = useState(false);
    const [isNotePopupOpen, setIsNotePopupOpen] = useState(false);
    const [duration, setDuration] = useState(''); // Duration in minutes
    const [rows, setRows] = useState([]); // Centralized data for rows
    const [deletedRows, setDeletedRows] = useState([]); // State for deleted rows
    const [isPopupOpen, setIsPopupOpen] = useState(false);
    const [popupMessage, setPopupMessage] = useState('');
    const navigate = useNavigate(); // Initialize useNavigate for redirection
    const userRole = getUserRole();
    function parseDuration(duration) {
        const parts = duration.split(':');
        const hours = parseInt(parts[0], 10);
        const minutes = parseInt(parts[1], 10);
        const seconds = parseInt(parts[2], 10);

        return hours * 60 + minutes + seconds / 60;
    }
    function minutesToHHMMSS(totalMinutes) {
        const hours = Math.floor(totalMinutes / 60);
        const minutes = Math.floor(totalMinutes % 60);
        const seconds = Math.floor((totalMinutes * 60) % 60);

        // Pad the minutes and seconds with leading zeros if necessary
        const paddedHours = String(hours).padStart(2, '0');
        const paddedMinutes = String(minutes).padStart(2, '0');
        const paddedSeconds = String(seconds).padStart(2, '0');

        return `${paddedHours}:${paddedMinutes}:${paddedSeconds}`;
    }

    useEffect(() => {
        const fetchWorkoutTemplate = async () => {
            try {
                const response = await axiosInstance.get(`/WorkoutTemplates/get-workout-template-details/${templateId}`);
                const templateData = response.data;
                console.log(templateData)
                setWorkoutName(templateData.templateName);
                setIsPublic(templateData.isPublic);
                const durationRegex = /^\d{1,2}:\d{2}:\d{2}$/;

                if (durationRegex.test(templateData.duration)) {
                    setDuration(parseDuration(templateData.duration));
                } else {
                    setDuration(parseInt(templateData.duration));
                }
                const templateExercises = templateData.workoutTemplateExercises;
                console.log(templateExercises);
                const newRows = [];


                templateExercises.forEach(exercise => {
                    let weights = [];
                    let reps = [];
                    let data = [];
                    const sets = exercise.setsRecommendation || 0;

                    try {
                        weights = JSON.parse(exercise.weightsUsed);
                    } catch (error) {
                        console.error('Error parsing weightsUsed for exercise:', exercise, error);
                        weights = [];
                    }

                    try {
                        reps = JSON.parse(exercise.numbersOfReps);
                    } catch (error) {
                        console.error('Error parsing numbersOfReps for exercise:', exercise, error);
                        reps = [];
                    }

                    // Iterate based on the number of sets
                    for (let i = 0; i < sets; i++) {
                        data.push({
                            weight: weights[i] !== undefined ? weights[i] : 0, // Default to 0 if weight is missing
                            reps: reps[i] !== undefined ? reps[i] : ''         // Default to empty string if reps is missing
                        });
                    }

                    newRows.push({
                        exercise: { exerciseId: exercise.exercise.exerciseId, exerciseName: exercise.exercise.exerciseName },
                        sets: sets,
                        intensity: exercise.intensityPercentage,
                        weights: weights,
                        data: data,
                        reps: reps,
                        note: exercise.note,
                        isDeleted: false // Initial state is not deleted
                    });
                });

                setRows(newRows);
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
            duration: `${minutesToHHMMSS(duration)}`,
            isPublic: isPublic, // Assuming template is public, update if necessary
            workoutTemplateExercises
        };

        try {
            const response = await axiosInstance.put(`/WorkoutTemplates/update-workout-template/${templateId}`, templateData);
            if (response.data.success) {
                setPopupMessage('Template updated successfully!');
                setIsPopupOpen(true);
                setTimeout(() => {
                    setIsPopupOpen(false);
                    
                    navigate
                        ('/'); // Redirect to root URL after 2 seconds
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
    const handleReturnButtonClick = () => {
        navigate(-1); // Navigate back to the previous page
    };

    return (
        <div className="container mt-5">
            <button className="btn btn-secondary mb-4" onClick={handleReturnButtonClick}>
                &#8592;
            </button>
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
