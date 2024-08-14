import React, { useState, useEffect } from 'react';
import {
  Modal,
  ModalHeader,
  ModalBody,
  ModalFooter,
  Form,
  FormGroup,
  Label,
  Input,
  Button,
  Alert
} from 'reactstrap';
import axiosInstance from '../../utils/axiosInstance';

const ExerciseModal = ({
  isOpen,
  toggle,
  exerciseId = null,  // Pass the exerciseId to the modal
  onExerciseUpdate,  // Function to update the exercise in the parent component
  nameErrorMessage,
  handleSubmit,
  modalTitle,
  submitButtonText,
}) => {
  const [exercise, setExercise] = useState({
    exerciseName: '',
    type: '',
    description: '',
    demoUrl: '',
    equipmentId: '',
    muscleGroupIds: [],
    publicVisibility: true,
  });

  const [muscleGroups, setMuscleGroups] = useState([]);
  const [equipments, setEquipments] = useState([]);
  const [error, setError] = useState(null);

  // Fetch muscle groups and equipment list on mount
  useEffect(() => {
    const fetchMuscleGroupsAndEquipments = async () => {
      try {
        const muscleGroupsResponse = await axiosInstance.get('/MuscleGroups/get-list?PageNumber=1&PageSize=100');
        setMuscleGroups(muscleGroupsResponse.data.items);

        const equipmentsResponse = await axiosInstance.get('/Equipments/get-all?PageNumber=1&PageSize=100');
        setEquipments(equipmentsResponse.data.items);
      } catch (err) {
        setError('Failed to fetch data. Please try again later.');
      }
    };

    fetchMuscleGroupsAndEquipments();
  }, []);

  // Fetch exercise details when the modal opens for an update
  useEffect(() => {
    const fetchExerciseDetails = async () => {
      if (isOpen && exerciseId) {
        try {
          const response = await axiosInstance.get(`/Exercises/${exerciseId}`);
          const exerciseData = {
            ...response.data,
            muscleGroupIds: muscleGroups
              .filter((group) => response.data.muscleGroupNames.includes(group.muscleGroupName))
              .map((group) => group.muscleGroupId),
          };
          setExercise(exerciseData);
        } catch (err) {
          setError('Failed to fetch exercise details. Please try again later.');
        }
      } else if (isOpen && !exerciseId) {
        // Reset the exercise data to the default when creating a new exercise
        setExercise({
          exerciseName: '',
          type: '',
          description: '',
          demoUrl: '',
          equipmentId: '',
          muscleGroupIds: [],
          publicVisibility: true,
        });
      }
    };

    fetchExerciseDetails();
  }, [isOpen, exerciseId, muscleGroups]);

  // Handle input changes
  const handleInputChange = (field, value) => {
    setExercise(prevExercise => {
      const updatedExercise = {
        ...prevExercise,
        [field]: value,
      };
      onExerciseUpdate(updatedExercise);  // Notify the parent component of the changes
      return updatedExercise;
    });
  };

  // Handle muscle group selection
  const handleMuscleGroupSelection = (e) => {
    const value = parseInt(e.target.value);
    const isChecked = e.target.checked;

    setExercise(prevExercise => {
      const updatedExercise = {
        ...prevExercise,
        muscleGroupIds: isChecked
          ? [...prevExercise.muscleGroupIds, value]
          : prevExercise.muscleGroupIds.filter(id => id !== value),
      };
      onExerciseUpdate(updatedExercise);  // Notify the parent component of the changes
      return updatedExercise;
    });
  };

  if (!exercise && exerciseId) {
    return null;  // Don't render the modal until exercise details are loaded
  }

  return (
    <Modal isOpen={isOpen} toggle={toggle} size="lg" backdrop="static">
      <ModalHeader toggle={toggle}>{modalTitle}</ModalHeader>
      <ModalBody style={{ maxHeight: '80vh', overflowY: 'auto' }}>
        <Form>
          <FormGroup>
            <Label for="exerciseName">
              Exercise Name <span style={{ color: 'red' }}>*</span>
            </Label>
            <Input
              type="text"
              id="exerciseName"
              value={exercise.exerciseName}
              onChange={(e) => handleInputChange('exerciseName', e.target.value)}
            />
            {nameErrorMessage && <Alert color="danger">{nameErrorMessage}</Alert>}
          </FormGroup>
          <FormGroup>
            <Label for="exerciseType">
              Exercise Type <span style={{ color: 'red' }}>*</span>
            </Label>
            <Input
              type="text"
              id="exerciseType"
              value={exercise.type}
              onChange={(e) => handleInputChange('type', e.target.value)}
            />
          </FormGroup>
          <FormGroup>
            <Label for="exerciseDescription">Description</Label>
            <Input
              type="textarea"
              id="exerciseDescription"
              value={exercise.description || ''}
              onChange={(e) => handleInputChange('description', e.target.value)}
            />
          </FormGroup>
          <FormGroup>
            <Label for="exerciseDemoUrl">Demo URL</Label>
            <Input
              type="url"
              id="exerciseDemoUrl"
              value={exercise.demoUrl || ''}
              onChange={(e) => handleInputChange('demoUrl', e.target.value)}
            />
          </FormGroup>
          <FormGroup>
            <Label for="equipment">Equipment</Label>
            <Input
              type="select"
              id="equipment"
              value={exercise.equipmentId || ''}
              onChange={(e) => handleInputChange('equipmentId', e.target.value)}
            >
              <option value="">Select Equipment</option>
              {equipments.map((equipment) => (
                <option key={equipment.equipmentId} value={equipment.equipmentId}>
                  {equipment.equipmentName}
                </option>
              ))}
            </Input>
            {error && <Alert color="danger">{error}</Alert>}
          </FormGroup>
          <FormGroup>
            <Label>Muscle Groups</Label>
            <div style={{ maxHeight: '150px', overflowY: 'auto', border: '1px solid #ced4da', padding: '10px', borderRadius: '4px' }}>
              {muscleGroups.map((muscleGroup) => (
                <FormGroup check key={muscleGroup.muscleGroupId}>
                  <Label check>
                    <Input
                      type="checkbox"
                      value={muscleGroup.muscleGroupId}
                      checked={exercise.muscleGroupIds.includes(muscleGroup.muscleGroupId)}
                      onChange={handleMuscleGroupSelection}
                    />
                    {muscleGroup.muscleGroupName}
                  </Label>
                </FormGroup>
              ))}
            </div>
          </FormGroup>
          <FormGroup check>
            <Label check>
              <Input
                type="checkbox"
                checked={exercise.publicVisibility}
                onChange={(e) => handleInputChange('publicVisibility', e.target.checked)}
              />
              Public Visibility
            </Label>
          </FormGroup>
        </Form>
      </ModalBody>
      <ModalFooter>
        <Button color="primary" onClick={handleSubmit}>{submitButtonText}</Button>
        <Button color="danger" onClick={toggle}>Cancel</Button>
      </ModalFooter>
    </Modal>
  );
};

export default ExerciseModal;
