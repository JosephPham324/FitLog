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
  IconButton,
  Tooltip,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  Snackbar,
  Alert
} from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import FileCopyIcon from '@mui/icons-material/FileCopy';
import './CreateWorkoutLog.css';

const ExerciseRow = ({ exercise, index, onClickEdit, onClickDelete, onClickDuplicate }) => (
  <TableRow key={index}>
    <TableCell>{index + 1}</TableCell>
    <TableCell>{exercise.note}</TableCell>
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
    <TableCell>
      <Tooltip title="Edit" classes={{ tooltip: 'tooltip' }}>
        <IconButton onClick={() => onClickEdit(index)} className="edit-icon">
          <EditIcon />
        </IconButton>
      </Tooltip>
      <Tooltip title="Delete" classes={{ tooltip: 'tooltip' }}>
        <IconButton onClick={() => onClickDelete(index)} className="delete-icon">
          <DeleteIcon />
        </IconButton>
      </Tooltip>
      <Tooltip title="Duplicate" classes={{ tooltip: 'tooltip' }}>
        <IconButton onClick={() => onClickDuplicate(index)} className="duplicate-icon">
          <FileCopyIcon />
        </IconButton>
      </Tooltip>
    </TableCell>
  </TableRow>
);

const WorkoutOutLog = () => {
  const [exercises, setExercises] = useState([
    {
      name: 'Front Squat (Barbell)',
      note: 'First set of the day',
      sets: [
        {
          weight: '100',
          reps: '5',
          rpe: '8',
          intensity: '85%',
          repsType: 'Reps',
          rpeType: 'RPE',
          repsMin: '',
          repsMax: '',
          rpeMin: '',
          rpeMax: '',
          rpeValue: ''
        }
      ]
    }
  ]);
  const [newExercise, setNewExercise] = useState({
    name: '',
    note: '',
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
  const [editExercise, setEditExercise] = useState(null);
  const [editingIndex, setEditingIndex] = useState(null);
  const [showNewExerciseFields, setShowNewExerciseFields] = useState(false);
  const [deleteIndex, setDeleteIndex] = useState(null);
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [snackbarOpen, setSnackbarOpen] = useState(false);
  const [snackbarMessage, setSnackbarMessage] = useState('');
  const [errors, setErrors] = useState({
    reps: '',
    rpe: ''
  });

  const handleAddExercise = () => {
    setNewExercise({
      name: '',
      note: '',
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
    setEditingIndex(null);
    setEditExercise(null);
    setShowNewExerciseFields(true);
  };

  const handleNumericChange = (field, value, setIndex, isEditing = false) => {
    if (/^\d*$/.test(value)) {
      if (isEditing) {
        const updatedSets = editExercise.sets.map((set, index) =>
          index === setIndex ? { ...set, [field]: value } : set
        );
        setEditExercise({ ...editExercise, sets: updatedSets });
      } else {
        const updatedSets = newExercise.sets.map((set, index) =>
          index === setIndex ? { ...set, [field]: value } : set
        );
        setNewExercise({ ...newExercise, sets: updatedSets });
      }
    }
  };

  const handleRowClick = (index) => {
    setEditingIndex(index);
    setEditExercise(exercises[index]);
    setShowNewExerciseFields(false);
  };

  const handleAddSet = (isEditing = false) => {
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
    if (isEditing) {
      setEditExercise({ ...editExercise, sets: [...editExercise.sets, newSet] });
    } else {
      setNewExercise({ ...newExercise, sets: [...newExercise.sets, newSet] });
    }
  };

  const handleDeleteExercise = (index) => {
    setDeleteIndex(index);
    setDeleteDialogOpen(true);
  };

  const confirmDeleteExercise = () => {
    const updatedExercises = exercises.filter((_, i) => i !== deleteIndex);
    setExercises(updatedExercises);
    setDeleteDialogOpen(false);
    setDeleteIndex(null);
  };

  const handleDuplicateExercise = (index) => {
    const duplicateExercise = { ...exercises[index] };
    const updatedExercises = [
      ...exercises.slice(0, index + 1),
      duplicateExercise,
      ...exercises.slice(index + 1)
    ];
    setExercises(updatedExercises);
  };

  const exerciseNames = Array.from(new Set(exercises.map((exercise) => exercise.name)));

  return (
    <Box sx={{ p: 2 }}>
      <Typography variant="h6">Day 1 - Lower</Typography>
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>#</TableCell>
              <TableCell>Note</TableCell>
              <TableCell>Exercise</TableCell>
              <TableCell>Sets</TableCell>
              <TableCell>Weight</TableCell>
              <TableCell>Reps</TableCell>
              <TableCell>RPE</TableCell>
              <TableCell>Intensity</TableCell>
              <TableCell>Action</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {exercises.map((exercise, index) => (
              <React.Fragment key={index}>
                <ExerciseRow
                  exercise={exercise}
                  index={index}
                  onClickEdit={handleRowClick}
                  onClickDelete={handleDeleteExercise}
                  onClickDuplicate={handleDuplicateExercise}
                />
                {editingIndex === index && (
                  <TableRow>
                    <TableCell colSpan={9}>
                      <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                        <Autocomplete
                          freeSolo
                          options={exerciseNames}
                          value={editExercise.name}
                          onChange={(event, newValue) => {
                            setEditExercise({ ...editExercise, name: newValue });
                          }}
                          onInputChange={(event, newInputValue) => {
                            setEditExercise({ ...editExercise, name: newInputValue });
                          }}
                          renderInput={(params) => (
                            <TextField
                              {...params}
                              label="Exercise"
                              fullWidth
                            />
                          )}
                        />
                        <TextField
                          label="Note"
                          value={editExercise.note}
                          onChange={(e) => setEditExercise({ ...editExercise, note: e.target.value })}
                          fullWidth
                          sx={{ mt: 2 }}
                        />
                        {editExercise.sets.map((set, setIndex) => (
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
                                onChange={(e) => handleNumericChange('weight', e.target.value, setIndex, true)}
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
                                    const updatedSets = editExercise.sets.map((s, i) =>
                                      i === setIndex ? { ...s, repsType: e.target.value } : s
                                    );
                                    setEditExercise({ ...editExercise, sets: updatedSets });
                                  }}
                                >
                                  <MenuItem value="Reps">Reps</MenuItem>
                                  <MenuItem value="Rep Range">Rep Range</MenuItem>
                                </Select>
                              </FormControl>
                              {set.repsType === 'Reps' ? (
                                <TextField
                                  value={set.reps}
                                  onChange={(e) => handleNumericChange('reps', e.target.value, setIndex, true)}
                                  sx={{ mt: 1 }}
                                  fullWidth
                                />
                              ) : (
                                <Box sx={{ display: 'flex', gap: 2, mt: 1 }}>
                                  <TextField
                                    value={set.repsMin}
                                    onChange={(e) => handleNumericChange('repsMin', e.target.value, setIndex, true)}
                                    fullWidth
                                    label="Min"
                                  />
                                  <TextField
                                    value={set.repsMax}
                                    onChange={(e) => handleNumericChange('repsMax', e.target.value, setIndex, true)}
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
                                    const updatedSets = editExercise.sets.map((s, i) =>
                                      i === setIndex ? { ...s, rpeType: e.target.value } : s
                                    );
                                    setEditExercise({ ...editExercise, sets: updatedSets });
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
                                  onChange={(e) => handleNumericChange('rpe', e.target.value, setIndex, true)}
                                  sx={{ mt: 1 }}
                                  fullWidth
                                />
                              ) : set.rpeType === 'RPE Range' ? (
                                <Box sx={{ display: 'flex', gap: 2, mt: 1 }}>
                                  <TextField
                                    value={set.rpeMin}
                                    onChange={(e) => handleNumericChange('rpeMin', e.target.value, setIndex, true)}
                                    fullWidth
                                    label="Min"
                                  />
                                  <TextField
                                    value={set.rpeMax}
                                    onChange={(e) => handleNumericChange('rpeMax', e.target.value, setIndex, true)}
                                    fullWidth
                                    label="Max"
                                  />
                                </Box>
                              ) : (
                                <TextField
                                  value={set.rpeValue}
                                  onChange={(e) => handleNumericChange('rpeValue', e.target.value, setIndex, true)}
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
                                onChange={(e) => handleNumericChange('intensity', e.target.value, setIndex, true)}
                                sx={{ mt: 1 }}
                                fullWidth
                              />
                            </Box>
                          </Box>
                        ))}
                        <Box sx={{ display: 'flex', justifyContent: 'space-between', gap: 2, mt: 2 }}>
                          <Button
                            variant="outlined"
                            onClick={() => handleAddSet(true)}
                            sx={{ flex: 1 }}
                          >
                            + Add Set
                          </Button>
                          <Button
                            variant="outlined"
                            onClick={() => setEditingIndex(null)}
                            sx={{ flex: 1 }}
                          >
                            Cancel
                          </Button>
                          <Button
                            variant="contained"
                            onClick={() => {
                              let hasError = false;
                              for (const set of editExercise.sets) {
                                if (set.repsType === 'Rep Range' && (parseInt(set.repsMin) >= parseInt(set.repsMax))) {
                                  setErrors({ ...errors, reps: '' });
                                  hasError = true;
                                  setSnackbarMessage('The Rep Range start value cannot be greater than the end value.');
                                  setSnackbarOpen(true);
                                  break;
                                }
                                if (set.rpeType === 'RPE Range' && (parseInt(set.rpeMin) >= parseInt(set.rpeMax))) {
                                  setErrors({ ...errors, rpe: '' });
                                  hasError = true;
                                  setSnackbarMessage('The RPE Range start value cannot be greater than the end value.');
                                  setSnackbarOpen(true);
                                  break;
                                }
                              }
                              if (!hasError) {
                                const updatedExercises = exercises.map((ex, i) =>
                                  i === editingIndex ? editExercise : ex
                                );
                                setExercises(updatedExercises);
                                setEditingIndex(null);
                                setEditExercise(null);
                                setErrors({ reps: '', rpe: '' });
                              }
                            }}
                            sx={{ flex: 1 }}
                          >
                            OK
                          </Button>
                        </Box>
                      </Box>
                    </TableCell>
                  </TableRow>
                )}
              </React.Fragment>
            ))}
            {showNewExerciseFields && (
              <TableRow>
                <TableCell colSpan={9}>
                  <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                    <Autocomplete
                      freeSolo
                      options={exerciseNames}
                      value={newExercise.name}
                      onChange={(event, newValue) => {
                        setNewExercise({ ...newExercise, name: newValue });
                      }}
                      onInputChange={(event, newInputValue) => {
                        setNewExercise({ ...newExercise, name: newInputValue });
                      }}
                      renderInput={(params) => (
                        <TextField
                          {...params}
                          label="Exercise"
                          fullWidth
                        />
                      )}
                    />
                    <TextField
                      label="Note"
                      value={newExercise.note}
                      onChange={(e) => setNewExercise({ ...newExercise, note: e.target.value })}
                      fullWidth
                      sx={{ mt: 2 }}
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
                        onClick={() => handleAddSet()}
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
                        onClick={() => {
                          let hasError = false;
                          for (const set of newExercise.sets) {
                            if (set.repsType === 'Rep Range' && (parseInt(set.repsMin) >= parseInt(set.repsMax))) {
                              setErrors({ ...errors, reps: '' });
                              hasError = true;
                              setSnackbarMessage('The Rep Range start value cannot be greater than the end value.');
                              setSnackbarOpen(true);
                              break;
                            }
                            if (set.rpeType === 'RPE Range' && (parseInt(set.rpeMin) >= parseInt(set.rpeMax))) {
                              setErrors({ ...errors, rpe: '' });
                              hasError = true;
                              setSnackbarMessage('The RPE Range start value cannot be greater than the end value.');
                              setSnackbarOpen(true);
                              break;
                            }
                          }
                          if (!hasError) {
                            setExercises([...exercises, newExercise]);
                            setNewExercise({
                              name: '',
                              note: '',
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
                            setErrors({ reps: '', rpe: '' });
                          }
                        }}
                        sx={{ flex: 1 }}
                      >
                        OK
                      </Button>
                    </Box>
                  </Box>
                </TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
      </TableContainer>
      <Button
        variant="contained"
        fullWidth
        sx={{ my: 2 }}
        onClick={handleAddExercise}
      >
        Add Exercise
      </Button>
      <Dialog
        open={deleteDialogOpen}
        onClose={() => setDeleteDialogOpen(false)}
      >
        <DialogTitle className="delete-dialog-title">Delete</DialogTitle>
        <DialogContent>
          <DialogContentText className="delete-dialog-content">
            Are you sure you want to delete the exercise?
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setDeleteDialogOpen(false)} className="cancel-button">
            Cancel
          </Button>
          <Button onClick={confirmDeleteExercise} className="delete-button" autoFocus>
            Delete
          </Button>
        </DialogActions>
      </Dialog>
      <Snackbar
        open={snackbarOpen}
        autoHideDuration={6000}
        onClose={() => setSnackbarOpen(false)}
        anchorOrigin={{ vertical: 'top', horizontal: 'center' }}
        classes={{ root: 'snackbar' }}
      >
        <Alert onClose={() => setSnackbarOpen(false)} severity="warning" sx={{ width: '100%' }} className="snackbar-alert">
          {snackbarMessage}
        </Alert>
      </Snackbar>
    </Box>
  );
};

export default WorkoutOutLog;




