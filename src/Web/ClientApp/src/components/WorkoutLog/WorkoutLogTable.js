import React, { useState } from 'react';
import ExerciseSearchBox from './ExerciseSearchBox';
import 'bootstrap/dist/css/bootstrap.min.css';
import './WorkoutLogTable.css';

const WorkoutTable = ({ rows, setRows }) => {
  const [isPopupOpen, setIsPopupOpen] = useState(false);
  const [isNotePopupOpen, setIsNotePopupOpen] = useState(false);
  const [currentNote, setCurrentNote] = useState('');
  const [currentNoteRow, setCurrentNoteRow] = useState(null);
  const [exerciseChangeRow, setExerciseChangeRow] = useState(null);

  const openPopup = () => {
    setIsPopupOpen(true);
  };

  const closePopup = () => {
    setIsPopupOpen(false);
  };

  const openNotePopup = (rowIndex) => {
    setCurrentNoteRow(rowIndex);
    setCurrentNote(rows[rowIndex].note);
    setIsNotePopupOpen(true);
  };

  const closeNotePopup = () => {
    setIsNotePopupOpen(false);
  };

  const saveNote = () => {
    const newRows = [...rows];
    newRows[currentNoteRow].note = currentNote;
    setRows(newRows);
    closeNotePopup();
  };

  const addRow = (exercise) => {
    setRows([...rows, { exercise, sets: 1, data: [{ reps: '', weight: '', intensity: '' }], note: '' }]);
  };

  const changeExercise = (exercise) => {
    const newRows = [...rows];
    newRows[exerciseChangeRow].exercise = exercise;
    setRows(newRows);
    setExerciseChangeRow(null);
    closePopup();
  };

  const handleInputChange = (rowIndex, setIndex, field, value) => {
    const newRows = [...rows];
    newRows[rowIndex].data[setIndex][field] = value;
    setRows(newRows);
  };

  const handleSetsChange = (rowIndex, sets) => {
    const newRows = [...rows];
    const currentSets = newRows[rowIndex].sets;
    newRows[rowIndex].sets = sets;

    if (sets > currentSets) {
      for (let i = currentSets; i < sets; i++) {
        newRows[rowIndex].data.push({ reps: '', weight: '', intensity: '' });
      }
    } else {
      newRows[rowIndex].data = newRows[rowIndex].data.slice(0, sets);
    }

    setRows(newRows);
  };

  const deleteRow = (rowIndex) => {
    const newRows = rows.filter((_, index) => index !== rowIndex);
    setRows(newRows);
  };

  return (
    <div>
      <table className="table table-striped workout-table">
        <thead>
          <tr>
            <th>#</th>
            <th>Exercise</th>
            <th>Sets</th>
            <th>Weight (kg)</th>
            <th>Reps</th>
            <th>Intensity</th>
            <th>Note</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {rows.map((row, rowIndex) => (
            <tr key={rowIndex}>
              <td>{rowIndex + 1}</td>
              <td>{row.exercise.exerciseName}</td>
              <td>
                <input
                  type="number"
                  value={row.sets}
                  onChange={(e) => handleSetsChange(rowIndex, parseInt(e.target.value))}
                  className="form-control"
                />
              </td>
              <td>
                {row.data.map((set, setIndex) => (
                  <input
                    key={`weight-${rowIndex}-${setIndex}`}
                    type="number"
                    value={set.weight}
                    onChange={(e) => handleInputChange(rowIndex, setIndex, 'weight', e.target.value)}
                    className="form-control mb-2"
                    placeholder={`Set ${setIndex + 1}`}
                  />
                ))}
              </td>
              <td>
                {row.data.map((set, setIndex) => (
                  <input
                    key={`reps-${rowIndex}-${setIndex}`}
                    type="number"
                    value={set.reps}
                    onChange={(e) => handleInputChange(rowIndex, setIndex, 'reps', e.target.value)}
                    className="form-control mb-2"
                    placeholder={`Set ${setIndex + 1}`}
                  />
                ))}
              </td>
              <td>
                {row.data.map((set, setIndex) => (
                  <input
                    key={`intensity-${rowIndex}-${setIndex}`}
                    type="text"
                    value={set.intensity}
                    onChange={(e) => handleInputChange(rowIndex, setIndex, 'intensity', e.target.value)}
                    className="form-control mb-2"
                    placeholder={`Set ${setIndex + 1}`}
                  />
                ))}
              </td>
              <td>
                <button className="btn btn-secondary" onClick={() => openNotePopup(rowIndex)}>Note</button>
              </td>
              <td>
                <button className="btn btn-warning" onClick={() => { setExerciseChangeRow(rowIndex); openPopup(); }}>Change</button>
                <button className="btn btn-danger ml-2" onClick={() => deleteRow(rowIndex)}>Delete</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      <button className="btn btn-primary" onClick={openPopup}>
        Add Exercise
      </button>
      {isPopupOpen && (
        <ExerciseSearchBox
          closePopup={closePopup}
          setSelectedExercise={exerciseChangeRow !== null ? changeExercise : addRow}
        />
      )}
      {isNotePopupOpen && (
        <div className="modal show d-block" role="dialog">
          <div className="modal-dialog" role="document">
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title">Edit Note</h5>
                <button type="button" className="close" onClick={closeNotePopup}>
                  <span>&times;</span>
                </button>
              </div>
              <div className="modal-body">
                <textarea
                  className="form-control"
                  value={currentNote}
                  onChange={(e) => setCurrentNote(e.target.value)}
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

export default WorkoutTable;
