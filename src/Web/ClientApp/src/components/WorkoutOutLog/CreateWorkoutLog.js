import React, { useState } from 'react';
import {
  Box,
  Button,
  TextField,
  Typography,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Autocomplete,
  MenuItem,
  Select,
  FormControl,
  InputLabel,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle
} from '@mui/material';
import './WorkoutOutLog.css';

const ExerciseRow = ({ exercise, index, onClick }) => (
  <TableRow key={index} onClick={onClick}>
    <TableCell>{index + 1}</TableCell>
    <TableCell>{exercise.name}</TableCell>
    <TableCell>{exercise.sets.length}</TableCell>
    <TableCell>
      {exercise.sets.map((set, setIndex) => (
        <div key={setIndex}>{set.weight}</div>
      ))}
    </TableCell>
    <TableCell>
      {exercise.sets.map((set, setIndex) => (
        <div key={setIndex}>
          {set.repsType === 'Reps' ? set.reps : `${set.repsMin}-${set.repsMax}`}
        </div>
      ))}
    </TableCell>
    <TableCell>
      {exercise.sets.map((set, setIndex) => (
        <div key={setIndex}>
          {set.rpeType === 'RPE'
            ? set.rpe
            : set.rpeType === 'RPE Range'
              ? `${set.rpeMin}-${set.rpeMax}`
              : set.rpeValue}
        </div>
      ))}
    </TableCell>
    <TableCell>
      {exercise.sets.map((set, setIndex) => (
        <div key={setIndex}>{set.intensity}</div>
      ))}
    </TableCell>
  </TableRow>
);

const CreateWorkoutLogPopup = ({
  newLog,
  setNewLog,
  exerciseNames,
  showNewExerciseFields,
  setShowNewExerciseFields,
  newExercise,
  setNewExercise,
  handleInputChange,
  handleAddExercise,
  handleNumericChange,
  handleAddSet,
  errors,
  handleCreateConfirm,
  closePopup
}) => {
  return (
    <div className="popup">
      <div className="popup-content">
        <h3>Create Workout Log</h3>
        <label>
          Note:
          <input type="text" name="note" value={newLog.note} onChange={handleInputChange} />
          {errors.note && <span className="error">{errors.note}</span>}
        </label>
        <label>
          Duration:
          <input type="text" name="duration" value={newLog.duration} onChange={handleInputChange} />
          {errors.duration && <span className="error">{errors.duration}</span>}
        </label>
        <label>
          ExerciseLogs:
          <Button
            variant="contained"
            fullWidth
            sx={{ my: 2 }}
            onClick={() => setShowNewExerciseFields(true)}
          >
            Add Exercise
          </Button>
          <TableContainer component={Paper}>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell>#</TableCell>
                  <TableCell>Exercise</TableCell>
                  <TableCell>Sets</TableCell>
                  <TableCell>Weight</TableCell>
                  <TableCell>Reps</TableCell>
                  <TableCell>RPE</TableCell>
                  <TableCell>Intensity</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {newLog.exerciseLogs.map((exercise, index) => (
                  <ExerciseRow
                    key={index}
                    exercise={exercise}
                    index={index}
                    onClick={() => setShowNewExerciseFields(true)}
                  />
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        </label>

        {showNewExerciseFields && (
          <div>
            <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
              <Autocomplete
                freeSolo
                options={exerciseNames}
                value={newExercise.name}
                onChange={(event, newValue) => setNewExercise({ ...newExercise, name: newValue })}
                onInputChange={(event, newInputValue) => setNewExercise({ ...newExercise, name: newInputValue })}
                renderInput={(params) => <TextField {...params} label="Exercise" fullWidth />}
              />
              {newExercise.sets.map((set, setIndex) => (
                <Box key={setIndex} sx={{ display: 'flex', justifyContent: 'space-between', gap: 2 }}>
                  <Box sx={{ display: 'flex', flexDirection: 'column', flex: 1 }}>
                    <Typography variant="caption">Sets</Typography>
                    <TextField
                      value={setIndex + 1}
                      disabled
                      sx={{ mt: 1 }}
                      fullWidth
                    />
                  </Box>
                  <Box sx={{ display: 'flex', flexDirection: 'column', flex: 1 }}>
                    <Typography variant="caption">Weight</Typography>
                    <TextField
                      value={set.weight}
                      onChange={(e) => handleNumericChange('weight', e.target.value, setIndex)}
                      sx={{ mt: 1 }}
                      fullWidth
                    />
                  </Box>
                  <Box sx={{ display: 'flex', flexDirection: 'column', flex: 1 }}>
                    <Typography variant="caption">Reps</Typography>
                    <FormControl sx={{ mt: 1, minWidth: 120 }}>
                      <InputLabel>Reps</InputLabel>
                      <Select
                        value={set.repsType}
                        onChange={(e) => {
                          const updatedSets = newExercise.sets.map((s, i) =>
                            i === setIndex ? { ...s, repsType: e.target.value } : s
                          );
                          setNewExercise({ ...newExercise, sets: updatedSets });
                        }}
                      >
                        <MenuItem value="Reps">Reps</MenuItem>
                        <MenuItem value="Rep Range">Rep Range</MenuItem>
                      </Select>
                    </FormControl>
                    {set.repsType === 'Reps' ? (
                      <TextField
                        value={set.reps}
                        onChange={(e) => handleNumericChange('reps', e.target.value, setIndex)}
                        sx={{ mt: 1 }}
                        fullWidth
                      />
                    ) : (
                      <Box sx={{ display: 'flex', gap: 2, mt: 1 }}>
                        <TextField
                          value={set.repsMin}
                          onChange={(e) => handleNumericChange('repsMin', e.target.value, setIndex)}
                          fullWidth
                          label="Min"
                        />
                        <TextField
                          value={set.repsMax}
                          onChange={(e) => handleNumericChange('repsMax', e.target.value, setIndex)}
                          fullWidth
                          label="Max"
                        />
                      </Box>
                    )}
                  </Box>
                  <Box sx={{ display: 'flex', flexDirection: 'column', flex: 1 }}>
                    <Typography variant="caption">RPE</Typography>
                    <FormControl sx={{ mt: 1, minWidth: 120 }}>
                      <InputLabel>RPE</InputLabel>
                      <Select
                        value={set.rpeType}
                        onChange={(e) => {
                          const updatedSets = newExercise.sets.map((s, i) =>
                            i === setIndex ? { ...s, rpeType: e.target.value } : s
                          );
                          setNewExercise({ ...newExercise, sets: updatedSets });
                        }}
                      >
                        <MenuItem value="RPE">RPE</MenuItem>
                        <MenuItem value="RPE Range">RPE Range</MenuItem>
                        <MenuItem value="RIR">RIR</MenuItem>
                        <MenuItem value="% Rep Max">% Rep Max</MenuItem>
                      </Select>
                    </FormControl>
                    {set.rpeType === 'RPE' ? (
                      <TextField
                        value={set.rpe}
                        onChange={(e) => handleNumericChange('rpe', e.target.value, setIndex)}
                        sx={{ mt: 1 }}
                        fullWidth
                      />
                    ) : set.rpeType === 'RPE Range' ? (
                      <Box sx={{ display: 'flex', gap: 2, mt: 1 }}>
                        <TextField
                          value={set.rpeMin}
                          onChange={(e) => handleNumericChange('rpeMin', e.target.value, setIndex)}
                          fullWidth
                          label="Min"
                        />
                        <TextField
                          value={set.rpeMax}
                          onChange={(e) => handleNumericChange('rpeMax', e.target.value, setIndex)}
                          fullWidth
                          label="Max"
                        />
                      </Box>
                    ) : (
                      <TextField
                        value={set.rpeValue}
                        onChange={(e) => handleNumericChange('rpeValue', e.target.value, setIndex)}
                        sx={{ mt: 1 }}
                        fullWidth
                        label={set.rpeType}
                      />
                    )}
                  </Box>
                  <Box sx={{ display: 'flex', flexDirection: 'column', flex: 1 }}>
                    <Typography variant="caption">Intensity</Typography>
                    <TextField
                      value={set.intensity}
                      onChange={(e) => handleNumericChange('intensity', e.target.value, setIndex)}
                      sx={{ mt: 1 }}
                      fullWidth
                    />
                  </Box>
                </Box>
              ))}
              <Box sx={{ display: 'flex', justifyContent: 'space-between', gap: 2, mt: 2 }}>
                <Button
                  variant="outlined"
                  onClick={handleAddSet}
                  sx={{ flex: 1 }}
                >
                  + Add Set
                </Button>
                <Button
                  variant="outlined"
                  onClick={() => setShowNewExerciseFields(false)}
                  sx={{ flex: 1 }}
                >
                  Cancel
                </Button>
                <Button
                  variant="contained"
                  onClick={handleAddExercise}
                  sx={{ flex: 1 }}
                >
                  OK
                </Button>
              </Box>
            </Box>
          </div>
        )}

        <button className="create-popup-button" onClick={handleCreateConfirm}>
          Create
        </button>
        <button className="cancel-popup-button" onClick={closePopup}>Cancel</button>
      </div>
    </div>
  );
};

const EditWorkoutLogPopup = ({
  newLog,
  setNewLog,
  exerciseNames,
  showNewExerciseFields,
  setShowNewExerciseFields,
  newExercise,
  setNewExercise,
  handleInputChange,
  handleAddExercise,
  handleNumericChange,
  handleAddSet,
  errors,
  handleEditConfirm,
  closePopup,
  handleEditExercise,
  handleDeleteExercise,
  editingExerciseIndex,
  setEditingExerciseIndex
}) => {
  return (
    <div className="popup">
      <div className="popup-content">
        <h3>Edit Workout Log</h3>
        <label>
          Note:
          <input type="text" name="note" value={newLog.note} onChange={handleInputChange} />
          {errors.note && <span className="error">{errors.note}</span>}
        </label>
        <label>
          Duration:
          <input type="text" name="duration" value={newLog.duration} onChange={handleInputChange} />
          {errors.duration && <span className="error">{errors.duration}</span>}
        </label>
        <label>
          ExerciseLogs:
          <Button
            variant="contained"
            fullWidth
            sx={{ my: 2 }}
            onClick={() => setShowNewExerciseFields(true)}
          >
            Add Exercise
          </Button>
          <TableContainer component={Paper}>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell>#</TableCell>
                  <TableCell>Exercise</TableCell>
                  <TableCell>Sets</TableCell>
                  <TableCell>Weight</TableCell>
                  <TableCell>Reps</TableCell>
                  <TableCell>RPE</TableCell>
                  <TableCell>Intensity</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {newLog.exerciseLogs.map((exercise, index) => (
                  <ExerciseRow
                    key={index}
                    exercise={exercise}
                    index={index}
                    onClick={() => {
                      setEditingExerciseIndex(index);
                      setNewExercise(exercise);
                      setShowNewExerciseFields(true);
                    }}
                  />
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        </label>

        {showNewExerciseFields && (
          <div>
            <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
              <Autocomplete
                freeSolo
                options={exerciseNames}
                value={newExercise.name}
                onChange={(event, newValue) => setNewExercise({ ...newExercise, name: newValue })}
                onInputChange={(event, newInputValue) => setNewExercise({ ...newExercise, name: newInputValue })}
                renderInput={(params) => <TextField {...params} label="Exercise" fullWidth />}
              />
              {newExercise.sets.map((set, setIndex) => (
                <Box key={setIndex} sx={{ display: 'flex', justifyContent: 'space-between', gap: 2 }}>
                  <Box sx={{ display: 'flex', flexDirection: 'column', flex: 1 }}>
                    <Typography variant="caption">Sets</Typography>
                    <TextField
                      value={setIndex + 1}
                      disabled
                      sx={{ mt: 1 }}
                      fullWidth
                    />
                  </Box>
                  <Box sx={{ display: 'flex', flexDirection: 'column', flex: 1 }}>
                    <Typography variant="caption">Weight</Typography>
                    <TextField
                      value={set.weight}
                      onChange={(e) => handleNumericChange('weight', e.target.value, setIndex)}
                      sx={{ mt: 1 }}
                      fullWidth
                    />
                  </Box>
                  <Box sx={{ display: 'flex', flexDirection: 'column', flex: 1 }}>
                    <Typography variant="caption">Reps</Typography>
                    <FormControl sx={{ mt: 1, minWidth: 120 }}>
                      <InputLabel>Reps</InputLabel>
                      <Select
                        value={set.repsType}
                        onChange={(e) => {
                          const updatedSets = newExercise.sets.map((s, i) =>
                            i === setIndex ? { ...s, repsType: e.target.value } : s
                          );
                          setNewExercise({ ...newExercise, sets: updatedSets });
                        }}
                      >
                        <MenuItem value="Reps">Reps</MenuItem>
                        <MenuItem value="Rep Range">Rep Range</MenuItem>
                      </Select>
                    </FormControl>
                    {set.repsType === 'Reps' ? (
                      <TextField
                        value={set.reps}
                        onChange={(e) => handleNumericChange('reps', e.target.value, setIndex)}
                        sx={{ mt: 1 }}
                        fullWidth
                      />
                    ) : (
                      <Box sx={{ display: 'flex', gap: 2, mt: 1 }}>
                        <TextField
                          value={set.repsMin}
                          onChange={(e) => handleNumericChange('repsMin', e.target.value, setIndex)}
                          fullWidth
                          label="Min"
                        />
                        <TextField
                          value={set.repsMax}
                          onChange={(e) => handleNumericChange('repsMax', e.target.value, setIndex)}
                          fullWidth
                          label="Max"
                        />
                      </Box>
                    )}
                  </Box>
                  <Box sx={{ display: 'flex', flexDirection: 'column', flex: 1 }}>
                    <Typography variant="caption">RPE</Typography>
                    <FormControl sx={{ mt: 1, minWidth: 120 }}>
                      <InputLabel>RPE</InputLabel>
                      <Select
                        value={set.rpeType}
                        onChange={(e) => {
                          const updatedSets = newExercise.sets.map((s, i) =>
                            i === setIndex ? { ...s, rpeType: e.target.value } : s
                          );
                          setNewExercise({ ...newExercise, sets: updatedSets });
                        }}
                      >
                        <MenuItem value="RPE">RPE</MenuItem>
                        <MenuItem value="RPE Range">RPE Range</MenuItem>
                        <MenuItem value="RIR">RIR</MenuItem>
                        <MenuItem value="% Rep Max">% Rep Max</MenuItem>
                      </Select>
                    </FormControl>
                    {set.rpeType === 'RPE' ? (
                      <TextField
                        value={set.rpe}
                        onChange={(e) => handleNumericChange('rpe', e.target.value, setIndex)}
                        sx={{ mt: 1 }}
                        fullWidth
                      />
                    ) : set.rpeType === 'RPE Range' ? (
                      <Box sx={{ display: 'flex', gap: 2, mt: 1 }}>
                        <TextField
                          value={set.rpeMin}
                          onChange={(e) => handleNumericChange('rpeMin', e.target.value, setIndex)}
                          fullWidth
                          label="Min"
                        />
                        <TextField
                          value={set.rpeMax}
                          onChange={(e) => handleNumericChange('rpeMax', e.target.value, setIndex)}
                          fullWidth
                          label="Max"
                        />
                      </Box>
                    ) : (
                      <TextField
                        value={set.rpeValue}
                        onChange={(e) => handleNumericChange('rpeValue', e.target.value, setIndex)}
                        sx={{ mt: 1 }}
                        fullWidth
                        label={set.rpeType}
                      />
                    )}
                  </Box>
                  <Box sx={{ display: 'flex', flexDirection: 'column', flex: 1 }}>
                    <Typography variant="caption">Intensity</Typography>
                    <TextField
                      value={set.intensity}
                      onChange={(e) => handleNumericChange('intensity', e.target.value, setIndex)}
                      sx={{ mt: 1 }}
                      fullWidth
                    />
                  </Box>
                </Box>
              ))}
              <Box sx={{ display: 'flex', justifyContent: 'space-between', gap: 2, mt: 2 }}>
                <Button
                  variant="outlined"
                  onClick={handleAddSet}
                  sx={{ flex: 1 }}
                >
                  + Add Set
                </Button>
                <Button
                  variant="outlined"
                  onClick={() => setShowNewExerciseFields(false)}
                  sx={{ flex: 1 }}
                >
                  Cancel
                </Button>
                <Button
                  variant="contained"
                  onClick={handleAddExercise}
                  sx={{ flex: 1 }}
                >
                  OK
                </Button>
                <Button
                  variant="contained"
                  onClick={handleEditExercise}
                  sx={{ flex: 1 }}
                >
                  Edit Exercise
                </Button>
                <Button
                  variant="contained"
                  onClick={handleDeleteExercise}
                  sx={{ flex: 1 }}
                >
                  Delete Exercise
                </Button>
              </Box>
            </Box>
          </div>
        )}

        <button className="create-popup-button" onClick={handleEditConfirm}>
          Edit
        </button>
        <button className="cancel-popup-button" onClick={closePopup}>Cancel</button>
      </div>
    </div>
  );
};

const WorkoutLog = () => {
  const [logs, setLogs] = useState([]);
  const [showPopup, setShowPopup] = useState(false);
  const [showConfirmPopup, setShowConfirmPopup] = useState(false);
  const [showDeleteConfirmPopup, setShowDeleteConfirmPopup] = useState(false);
  const [newLog, setNewLog] = useState({ note: '', duration: '', exerciseLogs: [] });
  const [errors, setErrors] = useState({});
  const [isEdit, setIsEdit] = useState(false);
  const [editIndex, setEditIndex] = useState(null);
  const [editingExerciseIndex, setEditingExerciseIndex] = useState(null);
  const [newExercise, setNewExercise] = useState({
    name: '',
    sets: [
      {
        weight: '',
        reps: '',
        rpe: '',
        intensity: '',
        repsType: 'Reps',
        rpeType: 'RPE',
        repsMin: '',
        repsMax: '',
        rpeMin: '',
        rpeMax: '',
        rpeValue: ''
      }
    ]
  });
  const [showNewExerciseFields, setShowNewExerciseFields] = useState(false);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setNewLog({ ...newLog, [name]: value });
    if (errors[name]) {
      setErrors({ ...errors, [name]: '' });
    }
  };

  const validate = () => {
    const newErrors = {};
    if (!newLog.note) newErrors.note = 'Note cannot be empty';
    if (!newLog.duration) newErrors.duration = 'Duration cannot be empty';
    if (newLog.exerciseLogs.length === 0) newErrors.exerciseLogs = 'ExerciseLogs cannot be empty';
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleCreateConfirm = () => {
    if (!validate()) return;
    setShowConfirmPopup(true);
  };

  const confirmCreate = () => {
    setLogs([...logs, newLog]);
    setNewLog({ note: '', duration: '', exerciseLogs: [] });
    setShowPopup(false);
    setShowConfirmPopup(false);
    setErrors({}); // Clear errors when closing the popup
  };

  const handleEditConfirm = () => {
    if (!validate()) return;
    setShowConfirmPopup(true);
  };

  const confirmEdit = () => {
    const updatedLogs = logs.map((log, index) =>
      index === editIndex ? newLog : log
    );
    setLogs(updatedLogs);
    setNewLog({ note: '', duration: '', exerciseLogs: [] });
    setShowPopup(false);
    setIsEdit(false);
    setEditIndex(null);
    setShowConfirmPopup(false);
    setErrors({}); // Clear errors when closing the popup
  };

  const handleDelete = (index) => {
    setEditIndex(index);
    setShowDeleteConfirmPopup(true);
  };

  const confirmDelete = () => {
    const updatedLogs = logs.filter((_, i) => i !== editIndex);
    setLogs(updatedLogs);
    setShowDeleteConfirmPopup(false);
  };

  const openCreatePopup = () => {
    setNewLog({ note: '', duration: '', exerciseLogs: [] });
    setShowPopup(true);
    setIsEdit(false);
    setEditIndex(null);
    setErrors({});
  };

  const openEditPopup = (index) => {
    setNewLog(logs[index]);
    setIsEdit(true);
    setEditIndex(index);
    setShowPopup(true);
    setErrors({});
  };

  const closePopup = () => {
    setShowPopup(false);
    setIsEdit(false);
    setEditIndex(null);
    setNewLog({ note: '', duration: '', exerciseLogs: [] });
    setErrors({});
  };

  const handleNumericChange = (field, value, setIndex) => {
    if (/^\d*$/.test(value)) {
      const updatedSets = newExercise.sets.map((set, index) =>
        index === setIndex ? { ...set, [field]: value } : set
      );
      setNewExercise({ ...newExercise, sets: updatedSets });
    }
  };

  const handleRowClick = (index) => {
    setEditingExerciseIndex(index);
    setNewExercise(newLog.exerciseLogs[index]);
    setShowNewExerciseFields(true);
  };

  const handleAddSet = () => {
    const newSet = {
      weight: '',
      reps: '',
      rpe: '',
      intensity: '',
      repsType: 'Reps',
      rpeType: 'RPE',
      repsMin: '',
      repsMax: '',
      rpeMin: '',
      rpeMax: '',
      rpeValue: ''
    };
    setNewExercise({ ...newExercise, sets: [...newExercise.sets, newSet] });
  };

  const exerciseNames = Array.from(new Set(newLog.exerciseLogs.map((exercise) => exercise.name)));

  const handleAddExercise = () => {
    const updatedExerciseLogs = editingExerciseIndex !== null
      ? newLog.exerciseLogs.map((exercise, index) =>
        index === editingExerciseIndex ? newExercise : exercise
      )
      : [...newLog.exerciseLogs, newExercise];

    setNewLog({ ...newLog, exerciseLogs: updatedExerciseLogs });
    setNewExercise({
      name: '',
      sets: [
        {
          weight: '',
          reps: '',
          rpe: '',
          intensity: '',
          repsType: 'Reps',
          rpeType: 'RPE',
          repsMin: '',
          repsMax: '',
          rpeMin: '',
          rpeMax: '',
          rpeValue: ''
        }
      ]
    });
    setShowNewExerciseFields(false);
    setEditingExerciseIndex(null);
  };

  const handleEditExercise = () => {
    const updatedExerciseLogs = newLog.exerciseLogs.map((exercise, index) =>
      index === editingExerciseIndex ? newExercise : exercise
    );
    setNewLog({ ...newLog, exerciseLogs: updatedExerciseLogs });
    setShowNewExerciseFields(false);
  };

  const handleDeleteExercise = () => {
    const updatedExerciseLogs = newLog.exerciseLogs.filter((_, i) => i !== editingExerciseIndex);
    setNewLog({ ...newLog, exerciseLogs: updatedExerciseLogs });
    setShowNewExerciseFields(false);
  };

  return (
    <div className="workout-log">
      <div className="Titlee">
        <h1>Workout Out Log</h1>
      </div>
      <button className="create-button" onClick={openCreatePopup}>Create</button>
      <table className="log-table">
        <thead>
          <tr>
            <th>#</th>
            <th>Note</th>
            <th>Duration</th>
            <th>Exercise</th>
            <th>Sets</th>
            <th>Weight</th>
            <th>Reps</th>
            <th>RPE</th>
            <th>Intensity</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {logs.map((log, index) => (
            <tr key={index}>
              <td>{index + 1}</td>
              <td>{log.note}</td>
              <td>{log.duration}</td>
              <td>{log.exerciseLogs.map((exercise, idx) => (
                <div key={idx}>{exercise.name}</div>
              ))}</td>
              <td>{log.exerciseLogs.map((exercise, idx) => (
                <div key={idx}>{exercise.sets.length}</div>
              ))}</td>
              <td>{log.exerciseLogs.map((exercise, idx) => (
                exercise.sets.map((set, setIndex) => (
                  <div key={setIndex}>{set.weight}</div>
                ))
              ))}</td>
              <td>{log.exerciseLogs.map((exercise, idx) => (
                exercise.sets.map((set, setIndex) => (
                  <div key={setIndex}>
                    {set.repsType === 'Reps' ? set.reps : `${set.repsMin}-${set.repsMax}`}
                  </div>
                ))
              ))}</td>
              <td>{log.exerciseLogs.map((exercise, idx) => (
                exercise.sets.map((set, setIndex) => (
                  <div key={setIndex}>
                    {set.rpeType === 'RPE'
                      ? set.rpe
                      : set.rpeType === 'RPE Range'
                        ? `${set.rpeMin}-${set.rpeMax}`
                        : set.rpeValue}
                  </div>
                ))
              ))}</td>
              <td>{log.exerciseLogs.map((exercise, idx) => (
                exercise.sets.map((set, setIndex) => (
                  <div key={setIndex}>{set.intensity}</div>
                ))
              ))}</td>
              <td>
                <div className="action-buttons">
                  <button className="action-button edit-buttonn" onClick={() => openEditPopup(index)}>Edit</button>
                  <button className="action-button delete-buttonn" onClick={() => handleDelete(index)}>Delete</button>
                </div>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      {showPopup && (
        isEdit ? (
          <EditWorkoutLogPopup
            newLog={newLog}
            setNewLog={setNewLog}
            exerciseNames={exerciseNames}
            showNewExerciseFields={showNewExerciseFields}
            setShowNewExerciseFields={setShowNewExerciseFields}
            newExercise={newExercise}
            setNewExercise={setNewExercise}
            handleInputChange={handleInputChange}
            handleAddExercise={handleAddExercise}
            handleNumericChange={handleNumericChange}
            handleAddSet={handleAddSet}
            errors={errors}
            handleEditConfirm={handleEditConfirm}
            closePopup={closePopup}
            handleEditExercise={handleEditExercise}
            handleDeleteExercise={handleDeleteExercise}
            editingExerciseIndex={editingExerciseIndex}
            setEditingExerciseIndex={setEditingExerciseIndex}
          />
        ) : (
          <CreateWorkoutLogPopup
            newLog={newLog}
            setNewLog={setNewLog}
            exerciseNames={exerciseNames}
            showNewExerciseFields={showNewExerciseFields}
            setShowNewExerciseFields={setShowNewExerciseFields}
            newExercise={newExercise}
            setNewExercise={setNewExercise}
            handleInputChange={handleInputChange}
            handleAddExercise={handleAddExercise}
            handleNumericChange={handleNumericChange}
            handleAddSet={handleAddSet}
            errors={errors}
            handleCreateConfirm={handleCreateConfirm}
            closePopup={closePopup}
          />
        )
      )}

      {showConfirmPopup && (
        <div className="confirm-popup">
          <div className="confirm-popup-content">
            <h3>{isEdit ? 'You definitely want to Edit' : 'You definitely want to Create'}</h3>
            <div className="button-group">
              <button className="confirm-button" onClick={isEdit ? confirmEdit : confirmCreate}>Yes</button>
              <button className="cancel-button" onClick={() => setShowConfirmPopup(false)}>No</button>
            </div>
          </div>
        </div>
      )}

      {showDeleteConfirmPopup && (
        <div className="confirm-popup">
          <div className="confirm-popup-content">
            <h3>You definitely want to Delete</h3>
            <div className="button-group">
              <button className="confirm-button" onClick={confirmDelete}>Yes</button>
              <button className="cancel-button" onClick={() => setShowDeleteConfirmPopup(false)}>No</button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default WorkoutLog;