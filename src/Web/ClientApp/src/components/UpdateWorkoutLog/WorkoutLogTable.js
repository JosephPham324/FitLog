import React, { useState } from 'react';
import ExerciseSearchBox from './ExerciseSearchBox';
import 'bootstrap/dist/css/bootstrap.min.css';
import './WorkoutLogTable.css';

const WorkoutTable = ({ rows, setRows, onDeleteRow }) => {
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
    setRows([...rows, { exercise, sets: 1, data: [{ reps: '', weight: '' }], note: '', isDeleted: false }]);
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
    newRows[rowIndex].data[setIndex][field] = field === 'weight' || field === 'reps' ? Number(value) : value;
    setRows(newRows);
  };

  const handleSetsChange = (rowIndex, sets) => {
    const newRows = [...rows];
    const currentSets = newRows[rowIndex].sets;
    newRows[rowIndex].sets = sets;

    if (sets > currentSets) {
      for (let i = currentSets; i < sets; i++) {
        newRows[rowIndex].data.push({ reps: '', weight: '' });
      }
    } else {
      newRows[rowIndex].data = newRows[rowIndex].data.slice(0, sets);
    }

    setRows(newRows);
  };

  const deleteRow = (rowIndex) => {
    onDeleteRow(rowIndex);
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
            <th>Note</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {rows.map((row, rowIndex) => (
            <tr key={rowIndex} className={row.isDeleted ? 'table-danger' : ''}>
              <td>{rowIndex + 1}</td>
              <td>{row.exercise.exerciseName}</td>
              <td>
                <input
                  type="number"
                  value={row.sets}
                  onChange={(e) => handleSetsChange(rowIndex, parseInt(e.target.value))}
                  className="form-control"
                  disabled={row.isDeleted}
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
                    disabled={row.isDeleted}
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
                    disabled={row.isDeleted}
                  />
                ))}
              </td>
              <td>
                <button className="btn btn-secondary" onClick={() => openNotePopup(rowIndex)} disabled={row.isDeleted}>Note</button>
              </td>
              <td>
                <button className="btn btn-warning" onClick={() => { setExerciseChangeRow(rowIndex); openPopup(); }} disabled={row.isDeleted}>Change</button>
                <button className="btn btn-danger ml-2" style={{ height: '40px' }} onClick={() => deleteRow(rowIndex)}>Delete</button>
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
                <button className="btn btn-primary" style={{ height: '60px', width: '100px' }} onClick={saveNote}>Save</button>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default WorkoutTable;
