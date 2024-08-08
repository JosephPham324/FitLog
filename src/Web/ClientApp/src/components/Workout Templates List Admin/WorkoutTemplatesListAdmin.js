import React, { useState } from 'react';
import './WorkoutTemplatesListAdmin.css';
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  TextField,
  Box,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Autocomplete,
  MenuItem,
  Select,
  FormControl,
  InputLabel,
  Typography
} from '@mui/material';

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

const WorkoutTemplatesListAdmin = () => {
  const [open, setOpen] = useState(false);
  const [exercises, setExercises] = useState([
    {
      name: 'Front Squat (Barbell)',
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
          rpeValue: '',
        },
      ],
    },
  ]);
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
        rpeValue: '',
      },
    ],
  });
  const [editingIndex, setEditingIndex] = useState(null);

  const handleAddExercise = () => {
    if (editingIndex !== null) {
      const updatedExercises = exercises.map((exercise, index) =>
        index === editingIndex ? newExercise : exercise
      );
      setExercises(updatedExercises);
    } else {
      setExercises([...exercises, newExercise]);
    }
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
          rpeValue: '',
        },
      ],
    });
    setEditingIndex(null);
    setOpen(false);
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
    setEditingIndex(index);
    setNewExercise(exercises[index]);
    setOpen(true);
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
      rpeValue: '',
    };
    setNewExercise({ ...newExercise, sets: [...newExercise.sets, newSet] });
  };

  const exerciseNames = Array.from(new Set(exercises.map((exercise) => exercise.name)));

  const templates = [
    { id: 1, templateName: 'Cardio Blast', duration: '30 mins', creatorName: 'John Doe' },
    { id: 2, templateName: 'Strength Training', duration: '45 mins', creatorName: 'Jane Smith' },
    { id: 3, templateName: 'Yoga Session', duration: '60 mins', creatorName: 'Alice Johnson' },
  ];

  return (
    <div className="container">
      <div className="texth1">
        <h1> Workout Templates List Admin</h1>
      </div>
      <div className="header">
        <button className="create-btn" onClick={() => setOpen(true)}>Create</button>
      </div>
      <div className="table-container">
        <table>
          <thead>
            <tr>
              <th>#</th>
              <th>Template Name</th>
              <th>Duration</th>
              <th>Creator Name</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            {templates.map((template, index) => (
              <tr key={template.id}>
                <td>{index + 1}</td>
                <td>{template.templateName}</td>
                <td>{template.duration}</td>
                <td>{template.creatorName}</td>
                <td>
                  <button className="update-btn">Update</button>
                  <button className="delete-btn" >Delete</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      <Dialog open={open} onClose={() => setOpen(false)} maxWidth="md" fullWidth>
        <DialogTitle>{editingIndex !== null ? 'Edit Exercise' : 'Create New Exercise'}</DialogTitle>
        <DialogContent>
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
                onClick={() => setOpen(false)}
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
        </DialogContent>
      </Dialog>
    </div>
  );
};

export default WorkoutTemplatesListAdmin;

