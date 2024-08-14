import React, { useState, useEffect } from 'react';
import axiosInstance from '../../utils/axiosInstance';
import 'bootstrap/dist/css/bootstrap.min.css';
import { useNavigate } from 'react-router-dom';
import { Modal, Button } from 'react-bootstrap';
import './WorkoutTemplateListPage.css';

const AdminWorkoutTemplatesListPage = () => {
    const [workoutTemplates, setWorkoutTemplates] = useState([]);
    const [pageNumber, setPageNumber] = useState(1);
    const [pageSize] = useState(10);
    const [totalPages, setTotalPages] = useState(1);
    const [totalCount, setTotalCount] = useState(0);
    const [hasPreviousPage, setHasPreviousPage] = useState(false);
    const [hasNextPage, setHasNextPage] = useState(false);
    const [showConfirmModal, setShowConfirmModal] = useState(false);
    const [showSuccessModal, setShowSuccessModal] = useState(false);
    const [templateToDelete, setTemplateToDelete] = useState(null);
    const navigate = useNavigate();
    const fetchWorkoutTemplates = async () => {
        try {
            const response = await axiosInstance.get(`/WorkoutTemplates/public-templates?PageNumber=${pageNumber}&PageSize=${pageSize}`);
            const data = response.data;
            setWorkoutTemplates(data.items);
            setPageNumber(data.pageNumber);
            setTotalPages(data.totalPages);
            setTotalCount(data.totalCount);
            setHasPreviousPage(data.hasPreviousPage);
            setHasNextPage(data.hasNextPage);
        } catch (error) {
            console.error('Error fetching workout templates:', error);
        }
    };

    useEffect(() => {
        fetchWorkoutTemplates();
    }, [pageNumber, pageSize]);

    const handlePreviousPage = () => {
        if (hasPreviousPage) {
            setPageNumber(pageNumber - 1);
        }
    };

    const handleNextPage = () => {
        if (hasNextPage) {
            setPageNumber(pageNumber + 1);
        }
    };

    const handlePageClick = (page) => {
        setPageNumber(page);
    };

    const renderPageNumbers = () => {
        const pageNumbers = [];
        for (let i = 1; i <= totalPages; i++) {
            pageNumbers.push(
                <button
                    key={i}
                    className={`btn ${pageNumber === i ? 'btn-primary' : 'btn-outline-primary'} mx-1`}
                    onClick={() => handlePageClick(i)}
                >
                    {i}
                </button>
            );
        }
        return pageNumbers;
    };

    const handleViewDetails = (templateId) => {
        navigate(`/workout-templates/${templateId}/details`);
    };

    const handleDelete = (templateId) => {
        setTemplateToDelete(templateId);
        setShowConfirmModal(true);
    };

    const confirmDelete = async () => {
        try {
            await axiosInstance.delete(`/WorkoutTemplates/delete-workout-template/${templateToDelete}`);
            setShowConfirmModal(false);
            setShowSuccessModal(true);
            fetchWorkoutTemplates();
        } catch (error) {
            console.error('Error deleting template:', error);
        }
    };

    const handleCreateTemplate = () => {
        navigate('/workout-templates-admin/create');
    };

    return (
        <div className="container mt-5">
            <h1 className="text-center mb-4">Admin Workout Templates</h1>
            <div className="d-flex justify-content-between mb-3">
                <button className="btn btn-primary" onClick={handleCreateTemplate}>
                    Create New Template
                </button>
            </div>
            <table className="table table-striped">
                <thead>
                    <tr>
                        <th>#</th>
                        <th>Template Name</th>
                        <th>Duration</th>
                        <th>Creator</th>
                        <th>Action</th>
                    </tr>
                </thead>
                <tbody>
                    {workoutTemplates.map((template, index) => (
                        <tr key={template.id}>
                            <td>{(pageNumber - 1) * pageSize + index + 1}</td>
                            <td>{template.templateName}</td>
                            <td>{template.duration}</td>
                            <td>{template.creatorName}</td>
                            <td>
                                <button
                                    className="btn btn-info"
                                    onClick={() => handleViewDetails(template.id)}
                                >
                                    Details
                                </button>
                                &nbsp;
                                <button
                                    className="btn btn-danger"
                                    onClick={() => handleDelete(template.id)}
                                >
                                    Delete
                                </button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
            <div className="d-flex justify-content-center my-3">
                <button
                    className="btn btn-primary mx-1"
                    onClick={handlePreviousPage}
                    disabled={!hasPreviousPage}
                >
                    Previous
                </button>
                {renderPageNumbers()}
                <button
                    className="btn btn-primary mx-1"
                    onClick={handleNextPage}
                    disabled={!hasNextPage}
                >
                    Next
                </button>
            </div>
            <div className="text-center">
                <span>
                    Page {pageNumber} of {totalPages} ({totalCount} total items)
                </span>
            </div>

            {/* Confirm Delete Modal */}
            <Modal show={showConfirmModal} onHide={() => setShowConfirmModal(false)}>
                <Modal.Header closeButton>
                    <Modal.Title>Confirm Delete</Modal.Title>
                </Modal.Header>
                <Modal.Body>Are you sure you want to delete this template?</Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={() => setShowConfirmModal(false)}>
                        Cancel
                    </Button>
                    <Button variant="danger" onClick={confirmDelete}>
                        Delete
                    </Button>
                </Modal.Footer>
            </Modal>

            {/* Success Modal */}
            <Modal show={showSuccessModal} onHide={() => setShowSuccessModal(false)}>
                <Modal.Header closeButton>
                    <Modal.Title>Success</Modal.Title>
                </Modal.Header>
                <Modal.Body>Template deleted successfully!</Modal.Body>
                <Modal.Footer>
                    <Button variant="primary" onClick={() => setShowSuccessModal(false)}>
                        OK
                    </Button>
                </Modal.Footer>
            </Modal>
        </div>
    );
};

export default AdminWorkoutTemplatesListPage;
