import React, { useState } from 'react';
import { Table, Button, Input, FormGroup } from 'reactstrap';

const ExerciseTable = ({ exercises, currentPage, itemsPerPage, handleOpenUpdate, handleDelete }) => {
  const [searchQuery, setSearchQuery] = useState('');

  // Filter exercises based on search query
  const filteredExercises = exercises.filter(exercise =>
    exercise.exerciseName?.toLowerCase().includes(searchQuery.toLowerCase())
  );

  return (
    <>
      <FormGroup>
        <Input
          type="text"
          placeholder="Search by Exercise Name"
          value={searchQuery}
          onChange={(e) => setSearchQuery(e.target.value)}
        />
      </FormGroup>
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
          {filteredExercises.map((exercise, index) => (
            <tr key={exercise.exerciseId}>
              <td>{(currentPage - 1) * itemsPerPage + index + 1}</td>
              <td>{exercise.exerciseName}</td>
              <td>{exercise.type}</td>
              <td>
                <Button
                  color="success"
                  className="mr-2"
                  onClick={() => handleOpenUpdate(exercise)}
                >
                  Update
                </Button>
                <Button color="danger" onClick={() => handleDelete(exercise)}>
                  Delete
                </Button>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>
    </>
  );
};

export default ExerciseTable;
