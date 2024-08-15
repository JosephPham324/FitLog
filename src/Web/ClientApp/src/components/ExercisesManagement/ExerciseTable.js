import React from 'react';
import { Table, Button } from 'reactstrap';

const ExerciseTable = ({ exercises, currentPage, itemsPerPage, handleOpenUpdate, handleDelete }) => {
  return (
    <Table striped hover responsive>
      <thead>
        <tr>
          <th>#</th>
          <th>Exercise Name</th>
          <th>Type</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        {exercises.map((exercise, index) => (
          <tr key={exercise.exerciseId}>
            <td>{(currentPage - 1) * itemsPerPage + index + 1}</td>
            <td>{exercise.exerciseName}</td>
            <td>{exercise.type}</td>
            <td>
              <Button color="success" className="mr-2" onClick={() => handleOpenUpdate(exercise)}>Update</Button>
              <Button color="danger" onClick={() => handleDelete(exercise)}>Delete</Button>
            </td>
          </tr>
        ))}
      </tbody>
    </Table>
  );
};

export default ExerciseTable;
