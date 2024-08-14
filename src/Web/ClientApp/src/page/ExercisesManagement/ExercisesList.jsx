import React, { useState, useEffect } from 'react';
import {
    Container,
    Input,
    Row,
    Col,
    Button,
    Alert,
    Pagination,
    PaginationItem,
    PaginationLink,
    Modal,
    ModalHeader,
    ModalBody,
    ModalFooter
} from 'reactstrap';
import axiosInstance from '../../utils/axiosInstance';
import ExerciseTable from '../../components/ExercisesManagement/ExerciseTable';
import ExerciseModal from '../../components/ExercisesManagement/ExerciseModal';

const ExerciseList = () => {
    const [exercises, setExercises] = useState([]);
    const [openCreate, setOpenCreate] = useState(false);
    const [openUpdate, setOpenUpdate] = useState(false);
    const [openDeleteConfirm, setOpenDeleteConfirm] = useState(false);
    const [selectedExercise, setSelectedExercise] = useState(null);
    const [searchQuery, setSearchQuery] = useState('');
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [nameErrorMessage, setNameErrorMessage] = useState('');
    const [error, setError] = useState('');
    const [searchErrorMessage, setSearchErrorMessage] = useState('');
    const [newExercise, setNewExercise] = useState(null);
    const [openSuccess, setOpenSuccess] = useState(false);
    const [successMessage, setSuccessMessage] = useState('');
    const [openError, setOpenError] = useState(false);
    const [popupErrorMessage, setPopupErrorMessage] = useState('');
    const [validationErrors, setValidationErrors] = useState({});


    useEffect(() => {
        fetchExercises(currentPage, searchQuery);
    }, [currentPage, searchQuery]);

    const fetchExercises = async (page, searchTerm) => {
        try {
            const response = await axiosInstance.get('/Exercises/paginated-all', {
                params: {
                    PageNumber: page,
                    PageSize: 10,
                    exerciseName: searchTerm || '',
                },
            });

            setExercises(response.data.items);
            setTotalPages(response.data.totalPages);

            if (response.data.items.length === 0) {
                setSearchErrorMessage('No exercises found.');
            } else {
                setSearchErrorMessage('');
            }
        } catch (error) {
            console.error('Error fetching exercises:', error);
            setSearchErrorMessage('Error fetching exercises');
        }
    };

    const handleOpenCreate = () => {
        setNewExercise({
            muscleGroupIds: [],
            equipmentId: '',
            exerciseName: '',
            demoUrl: '',
            type: '',
            description: '',
            publicVisibility: true,
        });
        setError('');
        setNameErrorMessage('');
        setOpenCreate(true);
    };

    const handleOpenUpdate = (exercise) => {
        setSelectedExercise(exercise);
        setError('');
        setNameErrorMessage('');
        setOpenUpdate(true);
    };

    const handleCloseCreate = () => {
        setOpenCreate(false);
    };

    const handleCloseUpdate = () => {
        setOpenUpdate(false);
    };

    const handleCloseDeleteConfirm = () => {
        setOpenDeleteConfirm(false);
    };

    const handleCloseSuccess = () => {
        setOpenSuccess(false);
    };

    const handleCloseError = () => {
        setOpenError(false);
        setPopupErrorMessage('');
    };

    const validateInput = (exercise) => {
        let isValid = true;
        if (!exercise.exerciseName.trim()) {
            setNameErrorMessage('Exercise Name cannot be empty!');
            isValid = false;
        } else {
            setNameErrorMessage('');
        }
        return isValid;
    };

    const handleCreate = async () => {
        console.log('newExercise:', newExercise);
        if (!validateInput(newExercise)) {
            return;
        }
        try {
            const response = await axiosInstance.post('/Exercises', newExercise, {
                headers: {
                    'Content-Type': 'application/json',
                },
            });

            if (response.data.success) {
                fetchExercises(currentPage);
                handleCloseCreate();
                setSuccessMessage('Exercise created successfully');
                setOpenSuccess(true);
                setValidationErrors({})
            } else {
                setError(response.data.errors.join(', '));
                setOpenError(true);
            }
        } catch (error) {
            processErrors(error);
        }
    };

    const handleUpdate = async () => {
        if (!validateInput(selectedExercise)) {
            return;
        }

        try {
            const response = await axiosInstance.put(`/Exercises/${selectedExercise.exerciseId}`, selectedExercise, {
                headers: {
                    'Content-Type': 'application/json',
                },
            });

            if (response.data.success) {
                fetchExercises(currentPage);
                handleCloseUpdate();
                setSuccessMessage('Exercise updated successfully');
                setOpenSuccess(true);
                setValidationErrors({})

            } else {
                setError(response.data.errors.join(', '));
                setOpenError(true);
            }
        } catch (error) {
            processErrors(error);

        }
    };

    const handleDelete = async () => {
        if (!selectedExercise) {
            return;
        }
        try {
            const response = await axiosInstance.delete(`/Exercises/${selectedExercise.exerciseId}`, {
                data: {
                    exerciseId: selectedExercise.exerciseId,
                },
            });

            if (response.status === 200 && response.data.success) {
                setSuccessMessage('Exercise deleted successfully!');
                setOpenSuccess(true);
                fetchExercises(currentPage);
            } else {
                setPopupErrorMessage(response.data.errors.join(', ') || 'Error deleting exercise. Please try again.');
                setOpenError(true);
            }
            handleCloseDeleteConfirm();
        } catch (error) {
            processErrors(error);
        }
    };

    const handleSearch = (event) => {
        setSearchQuery(event.target.value);
        setCurrentPage(1); // Reset to the first page on search
    };

    const handleMuscleGroupSelection = (e, exercise) => {
        const value = parseInt(e.target.value);
        const isChecked = e.target.checked;

        if (isChecked) {
            setSelectedExercise({ ...exercise, muscleGroupIds: [...exercise.muscleGroupIds, value] });
        } else {
            setSelectedExercise({
                ...exercise,
                muscleGroupIds: exercise.muscleGroupIds.filter((id) => id !== value),
            });
        }
    };
    const handleExerciseUpdate = (updatedExercise) => {
        setSelectedExercise(updatedExercise);
    };
    const handleNewExerciseUpdate = (updatedExercise) => {
        setNewExercise(updatedExercise);
    };

    const processErrors = (error) => {
        if (error.response && error.response.data && error.response.data.errors) {
            handleValidationErrors(error.response.data.errors);
        } else {
            setPopupErrorMessage('An unexpected error occurred. Please try again.');
            setOpenError(true);
        }
    };

    const handleValidationErrors = (errors) => {
        let errorMessages = {};

        if (errors.Type) {
            errorMessages.type = errors.Type.join(', ');
        }
        if (errors.MuscleGroupIds) {
            errorMessages.muscleGroupIds = errors.MuscleGroupIds.join(', ');
        }
        if (errors.EquipmentId) {
            errorMessages.equipmentId = errors.EquipmentId.join(', ');
        }
        if (errors.ExerciseName) {
            errorMessages.exerciseName = errors.ExerciseName.join(', ');
        }
        if (errors.Description) {
            errorMessages.description = errors.Description.join(', ');
        }
        if (errors.DemoUrl) {
            errorMessages.demoUrl = errors.DemoUrl.join(', ');
        }

        setValidationErrors(errorMessages);
    };


    const renderPagination = () => (
        <Pagination aria-label="Page navigation example">
            <PaginationItem disabled={currentPage === 1}>
                <PaginationLink first onClick={() => setCurrentPage(1)} />
            </PaginationItem>
            <PaginationItem disabled={currentPage === 1}>
                <PaginationLink previous onClick={() => setCurrentPage(currentPage - 1)} />
            </PaginationItem>
            {[...Array(totalPages)].map((_, i) => (
                <PaginationItem key={i + 1} active={i + 1 === currentPage}>
                    <PaginationLink onClick={() => setCurrentPage(i + 1)}>
                        {i + 1}
                    </PaginationLink>
                </PaginationItem>
            ))}
            <PaginationItem disabled={currentPage === totalPages}>
                <PaginationLink next onClick={() => setCurrentPage(currentPage + 1)} />
            </PaginationItem>
            <PaginationItem disabled={currentPage === totalPages}>
                <PaginationLink last onClick={() => setCurrentPage(totalPages)} />
            </PaginationItem>
        </Pagination>
    );

    return (
        <Container>
            <h2 className="my-4"><strong>Exercises</strong></h2>
            <Row className="align-items-center mb-3">
                <Col xs="12" md="10" className="mb-3 mb-md-0">
                    <Input
                        type="text"
                        placeholder="Search Exercises..."
                        value={searchQuery}
                        onChange={handleSearch}
                        className="btn-search"
                    />
                    {searchErrorMessage && <Alert color="danger">{searchErrorMessage}</Alert>}
                </Col>
                <Col xs="12" md="2" className="text-md-right">
                    <Button color="primary" className="create-button" onClick={handleOpenCreate}>Create Exercise</Button>
                </Col>
            </Row>

            <ExerciseTable
                exercises={exercises}
                currentPage={currentPage}
                itemsPerPage={10} // Now fixed since API handles pagination
                handleOpenUpdate={handleOpenUpdate}
                handleDelete={(exercise) => { setSelectedExercise(exercise); setOpenDeleteConfirm(true); }}
            />

            {renderPagination()}

            <ExerciseModal
                isOpen={openCreate}
                toggle={handleCloseCreate}
                nameErrorMessage={nameErrorMessage}
                onExerciseUpdate={handleNewExerciseUpdate}
                handleMuscleGroupSelection={handleMuscleGroupSelection}
                handleSubmit={handleCreate}
                modalTitle="Create Exercise"
                submitButtonText="Create"
                validationErrors = {validationErrors}
            />

            <ExerciseModal
                isOpen={openUpdate}
                toggle={handleCloseUpdate}
                exerciseId={selectedExercise?.exerciseId}
                onExerciseUpdate={handleExerciseUpdate}  // Pass the update function
                nameErrorMessage={nameErrorMessage}
                handleChange={(field, value) => setSelectedExercise({ ...selectedExercise, [field]: value })}
                handleMuscleGroupSelection={(e) => handleMuscleGroupSelection(e, selectedExercise)}
                handleSubmit={handleUpdate}
                modalTitle="Update Exercise"
                submitButtonText="Update"
                validationErrors = {validationErrors}
            />

            {/* Delete Confirm Modal */}
            <Modal isOpen={openDeleteConfirm} toggle={handleCloseDeleteConfirm}>
                <ModalHeader toggle={handleCloseDeleteConfirm}>Confirmation Delete</ModalHeader>
                <ModalBody>
                    <p>Are you sure you want to delete this exercise?</p>
                </ModalBody>
                <ModalFooter>
                    <Button color="danger" onClick={handleDelete}>Yes</Button>
                    <Button color="primary" onClick={handleCloseDeleteConfirm}>No</Button>
                </ModalFooter>
            </Modal>

            {/* Success Modal */}
            <Modal isOpen={openSuccess} toggle={handleCloseSuccess}>
                <ModalHeader toggle={handleCloseSuccess}>Success</ModalHeader>
                <ModalBody>
                    <p>{successMessage}</p>
                </ModalBody>
                <ModalFooter>
                    <Button color="success" onClick={handleCloseSuccess}>OK</Button>
                </ModalFooter>
            </Modal>

            {/* Error Modal */}
            <Modal isOpen={openError} toggle={handleCloseError}>
                <ModalHeader toggle={handleCloseError}>Error</ModalHeader>
                <ModalBody>
                    <p>{popupErrorMessage}</p>
                </ModalBody>
                <ModalFooter>
                    <Button color="danger" onClick={handleCloseError}>OK</Button>
                </ModalFooter>
            </Modal>
        </Container>
    );
};

export default ExerciseList;
