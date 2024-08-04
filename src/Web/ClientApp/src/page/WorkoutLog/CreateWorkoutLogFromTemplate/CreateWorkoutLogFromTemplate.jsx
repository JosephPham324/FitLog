import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import WorkoutTable from '../../../components/WorkoutLog/WorkoutLogTable';
import 'bootstrap/dist/css/bootstrap.min.css';
import axiosInstance from '../../../utils/axiosInstance';
import './CreateWorkoutLog.css';

const CreateWorkoutLogFromTemplate = () => {
    const { templateId } = useParams();
    const navigate = useNavigate();
    const [workoutName, setWorkoutName] = useState('');
    const [workoutNote, setWorkoutNote] = useState('');
    const [isNotePopupOpen, setIsNotePopupOpen] = useState(false);
    const [duration, setDuration] = useState(0); // Duration in seconds
    const [rows, setRows] = useState([]); // Centralized data for rows
    const [isPopupOpen, setIsPopupOpen] = useState(false);
    const [popupMessage, setPopupMessage] = useState('');
    const [isIntensityPopupOpen, setIsIntensityPopupOpen] = useState(false);
    const [oneRepMax, setOneRepMax] = useState({});
    const [exercisesWithIntensity, setExercisesWithIntensity] = useState([]);

    useEffect(() => {
        const fetchTemplate = async () => {
            try {
                const response = await axiosInstance.get(`https://localhost:44447/api/WorkoutTemplates/get-workout-template-details/${templateId}`);
                const template = response.data;
                console.log(response.data);

                setWorkoutName(template.templateName);
                const expectedDuration = parseDuration(template.duration);
                setWorkoutNote(`Expected duration: ${formatDuration(expectedDuration)}`);
                setDuration(0); // Start the duration from 0

                const prefilledRows = template.workoutTemplateExercises.map(ex => ({
                    exercise: ex.exercise,
                    sets: ex.setsRecommendation,
                    data: Array.from({ length: ex.setsRecommendation }, (_, index) => ({
                        reps: JSON.parse(ex.numbersOfReps)[index] || '',
                        weight: JSON.parse(ex.weightsUsed)[index] || '',
                        intensity: ex.intensityPercentage,
                    })),
                    note: ex.note
                }));
                setRows(prefilledRows);

                const exercises = template.workoutTemplateExercises.filter(ex => ex.intensityPercentage > 0).map(ex => ex.exercise);
                setExercisesWithIntensity(exercises);
                if (exercises.length > 0) setIsIntensityPopupOpen(true);

            } catch (error) {
                console.error('Error fetching template:', error);
            }
        };

        fetchTemplate();
    }, [templateId]);

    useEffect(() => {
        const timer = setInterval(() => {
            setDuration(prevDuration => prevDuration + 1);
        }, 1000);

        return () => clearInterval(timer);
    }, []);

    const parseDuration = (duration) => {
        const parts = duration.split(':');
        return parseInt(parts[0]) * 3600 + parseInt(parts[1]) * 60 + parseInt(parts[2]);
    };

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

    const handleOneRepMaxChange = (e, exerciseId) => {
        setOneRepMax({
            ...oneRepMax,
            [exerciseId]: e.target.value
        });
    };

    const fillWeights = () => {
        const newRows = rows.map((row) => {
            if (row.data.some(set => set.intensity > 0)) {
                const updatedData = row.data.map(set => ({
                    ...set,
                    weight: oneRepMax[row.exercise.exerciseId] * (set.intensity / 100)
                }));
                return { ...row, data: updatedData };
            }
            return row;
        });

        setRows(newRows);
        setIsIntensityPopupOpen(false);
    };

    const saveLog = async () => {
        const exerciseLogs = rows.map((row, rowIndex) => ({
            exerciseId: row.exercise.exerciseId,
            orderInSession: rowIndex + 1,
            note: row.note,
            numberOfSets: row.sets,
            weightsUsed: `[${row.data.map(set => set.weight).join(', ')}]`,
            numberOfReps: `[${row.data.map(set => set.reps).join(', ')}]`,
            footageUrls: "[]"
        }));

        const logData = {
            workoutLogName: workoutName,
            duration: formatDuration(duration),
            note: workoutNote,
            exerciseLogs
        };

        try {
            const response = await axiosInstance.post('https://localhost:44447/api/WorkoutLog', logData);
            if (response.data.success) {
                setPopupMessage('Log saved successfully!');
                setIsPopupOpen(true);
                setTimeout(() => {
                    setIsPopupOpen(false);
                    navigate('/'); // Redirect to root URL after 2 seconds
                }, 2000);
            } else {
                setPopupMessage('Error saving log: ' + response.data.errors.join(', '));
                setIsPopupOpen(true);
                setTimeout(() => {
                    setIsPopupOpen(false);
                }, 2000);
            }
        } catch (error) {
            setPopupMessage('Error saving log. Please try again.');
            setIsPopupOpen(true);
            setTimeout(() => {
                setIsPopupOpen(false);
            }, 2000);
        }
    };

    return (
        <div className="container mt-5">
            <h1 className="text-center mb-4">Create Workout Log from Template</h1>
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
            <WorkoutTable rows={rows} setRows={setRows} />
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
                                {popupMessage === 'Log saved successfully!' && (
                                    <div>
                                        <p>Redirecting to the home page...</p>
                                    </div>
                                )}
                            </div>
                        </div>
                    </div>
                </div>
            )}
            {isIntensityPopupOpen && (
                <div className="modal show d-block" role="dialog">
                    <div className="modal-dialog modal-dialog-centered" role="document">
                        <div className="modal-content">
                            <div className="modal-header">
                                <h5 className="modal-title">Enter 1 Rep Max for Exercises</h5>
                                <button type="button" className="close" onClick={() => setIsIntensityPopupOpen(false)}>
                                    <span>&times;</span>
                                </button>
                            </div>
                            <div className="modal-body">
                                {exercisesWithIntensity.map(exercise => (
                                    <div key={exercise.exerciseId} className="mb-3">
                                        <label>{exercise.exerciseName}</label>
                                        <input
                                            type="number"
                                            className="form-control"
                                            value={oneRepMax[exercise.exerciseId] || ''}
                                            onChange={(e) => handleOneRepMaxChange(e, exercise.exerciseId)}
                                        />
                                    </div>
                                ))}
                            </div>
                            <div className="modal-footer">
                                <button className="btn btn-secondary" onClick={() => setIsIntensityPopupOpen(false)}>Cancel</button>
                                <button className="btn btn-primary" onClick={fillWeights}>Save</button>
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

export default CreateWorkoutLogFromTemplate;
