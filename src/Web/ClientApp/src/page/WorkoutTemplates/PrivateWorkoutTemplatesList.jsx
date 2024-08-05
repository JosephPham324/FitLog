import React, { useState, useEffect } from 'react';
import axiosInstance from '../../utils/axiosInstance';
import 'bootstrap/dist/css/bootstrap.min.css';
import { useNavigate } from 'react-router-dom';
import './WorkoutTemplateListPage.css';

const WorkoutTemplateListPage = () => {
    const [workoutTemplates, setWorkoutTemplates] = useState([]);
    const [pageNumber, setPageNumber] = useState(1);
    const [pageSize] = useState(10); // Assuming a fixed page size of 10
    const [totalPages, setTotalPages] = useState(1);
    const [totalCount, setTotalCount] = useState(0);
    const [hasPreviousPage, setHasPreviousPage] = useState(false);
    const [hasNextPage, setHasNextPage] = useState(false);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchWorkoutTemplates = async () => {
            try {
                const response = await axiosInstance.get(`/WorkoutTemplates/personal-templates?PageNumber=${pageNumber}&PageSize=${pageSize}`);
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

    const handleUseTemplate = (templateId) => {
        navigate(`/workout-log/create/${templateId}`);
    };

    const handleViewDetails = (templateId) => {
        navigate(`/workout-templates/${templateId}/details`);
    };

    const handleDelete = async (templateId) => {
        try {
            const result = await axiosInstance.delete(`/workout-templates/${templateId}/`);
            alert(result.data.success);
            // Refresh the list after deletion
            setPageNumber(1); // Reset to the first page or fetch the current page again
        } catch (error) {
            console.error('Error deleting template:', error);
        }
    };

    const handleCreateTemplate = () => {
        navigate('/workout-templates/create');
    };

    return (
        <div className="container mt-5">
            <h1 className="text-center mb-4">Workout Templates</h1>
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
                                    className="btn btn-success btn-action"
                                    onClick={() => handleUseTemplate(template.id)}
                                >
                                    Use Template
                                </button>
                                &nbsp;
                                <button
                                    className="btn btn-info btn-action"
                                    onClick={() => handleViewDetails(template.id)}
                                >
                                    Details
                                </button>
                                &nbsp;
                                <button
                                    className="btn btn-danger btn-action"
                                    onClick={async () => await handleDelete(template.id)}
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
        </div>
    );
};

export default WorkoutTemplateListPage;
