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
} from '@mui/material';
import "./WorkoutOutLog.css"

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

const App = () => {
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
  const [showNewExerciseFields, setShowNewExerciseFields] = useState(false);

  const handleAddExercise = () => {
    setExercises([...exercises, newExercise]);
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
    setEditingIndex(exercises.length);
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

  return (
    <Box sx={{ p: 2 }}>
      <Typography variant="h6">Day 1 - Lower</Typography>
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
            {exercises.map((exercise, index) => (
              <React.Fragment key={index}>
                <ExerciseRow exercise={exercise} index={index} onClick={() => handleRowClick(index)} />
                {editingIndex === index && (
                  <TableRow>
                    <TableCell colSpan={7}>
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
                            onClick={() => setEditingIndex(null)}
                            sx={{ flex: 1 }}
                          >
                            Cancel
                          </Button>
                          <Button
                            variant="contained"
                            onClick={() => {
                              const updatedExercises = exercises.map((ex, i) =>
                                i === editingIndex ? newExercise : ex
                              );
                              setExercises(updatedExercises);
                              setEditingIndex(null);
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
                <TableCell colSpan={7}>
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
                                setExercises([...exercises, newExercise]);
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
                                setShowNewExerciseFields(false);
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
        onClick={() => {
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
          setShowNewExerciseFields(true);
        }}
      >
        Add Exercise
      </Button>
    </Box>
  );
};

export default App;
