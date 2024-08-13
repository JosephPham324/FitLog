import React, { useEffect, useState } from 'react';
import axiosInstance from '../utils/axiosInstance'; // Import the configured Axios instance
import Calendar from 'react-calendar';
import 'react-calendar/dist/Calendar.css';
import 'bootstrap/dist/css/bootstrap.min.css'; // Import Bootstrap CSS
import './WorkoutHistory.css'; // Import custom CSS for additional styling
import { Link } from 'react-router-dom';
import {
  Button,
  Modal,
  ModalHeader,
  ModalBody,
  ModalFooter,
  Alert
} from 'reactstrap';

export default function WorkoutHistory() {
  const [workoutHistory, setWorkoutHistory] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [dates, setDates] = useState(getCurrentWeek()); // Default to current week
  const [currentPage, setCurrentPage] = useState(1);
  const workoutsPerPage = 7;
  const [deleteModal, setDeleteModal] = useState(false);
  const [deleteId, setDeleteId] = useState(null);
  const [successMessage, setSuccessMessage] = useState('');

  function getCurrentWeek() {
    const now = new Date();
    const dayOfWeek = now.getDay(); // Day of the week (0 - Sunday, 6 - Saturday)
    const monday = new Date(now);
    monday.setDate(now.getDate() - ((dayOfWeek + 6) % 7)); // Set to Monday of the current week
    const sunday = new Date(monday);
    sunday.setDate(monday.getDate() + 6); // Set to Sunday of the current week
    return [monday, sunday];
  }

  function formatDate(date) {
    return date.toLocaleDateString('en-GB', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric',
    });
  }

  const fetchWorkoutHistory = async (startDate, endDate) => {
    setLoading(true);
    try {
      console.log("Fetching workout history for dates:", formatDate(startDate), "to", formatDate(endDate));
      const response = await axiosInstance.get(`${process.env.REACT_APP_BACKEND_URL}/WorkoutLog/history`, {
        params: {
          startDate: startDate.toISOString().split('T')[0],
          endDate: endDate.toISOString().split('T')[0],
        },
        headers: {
          accept: 'application/json',
        },
      });
      setWorkoutHistory(response.data);
    } catch (error) {
      setError('Error fetching workout history');
      console.error('Error fetching workout history:', error);
    } finally {
      setLoading(false);
    }
  };

  const toggleDeleteModal = (workoutLogId) => {
    setDeleteId(workoutLogId);
    setDeleteModal(!deleteModal);
  };

  const deleteWorkout = async () => {
    setLoading(true);
    try {
      await axiosInstance.delete(`${process.env.REACT_APP_BACKEND_URL}/WorkoutLog/${deleteId}`, {
        headers: {
          'accept': 'application/json',
          'Content-Type': 'application/json',
        },
        data: {
          workoutLogId: deleteId,
        },
      });
      setWorkoutHistory(workoutHistory.filter(workout => workout.id !== deleteId));
      setDeleteModal(false);
      setSuccessMessage('Workout history deleted successfully!');
      setTimeout(() => setSuccessMessage(''), 3000); // Clear the success message after 3 seconds
    } catch (error) {
      setError('Error deleting workout history');
      console.error('Error deleting workout history:', error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    const startDate = dates[0];
    const endDate = new Date(dates[1]);
    endDate.setDate(endDate.getDate() + 1); // Add one day to endDate to include data till the end of the selected day
    fetchWorkoutHistory(startDate, endDate);
  }, [dates]);

  const filteredWorkoutHistory = workoutHistory.filter(workout => {
    const workoutDate = new Date(workout.created);
    const endOfDay = new Date(dates[1]);
    endOfDay.setHours(23, 59, 59, 999); // Set to end of the selected day
    return workoutDate >= dates[0] && workoutDate <= endOfDay;
  }).sort((a, b) => new Date(b.created) - new Date(a.created)); // Sort from newest to oldest

  // Pagination logic
  const indexOfLastWorkout = currentPage * workoutsPerPage;
  const indexOfFirstWorkout = indexOfLastWorkout - workoutsPerPage;
  const currentWorkouts = filteredWorkoutHistory.slice(indexOfFirstWorkout, indexOfLastWorkout);

  const paginate = (pageNumber) => setCurrentPage(pageNumber);
  const nextPage = () => setCurrentPage(prevPage => Math.min(prevPage + 1, Math.ceil(filteredWorkoutHistory.length / workoutsPerPage)));
  const prevPage = () => setCurrentPage(prevPage => Math.max(prevPage - 1, 1));
  const getBestSet = (weightsUsedStr, numberOfRepsStr) => {
    const weightsUsed = JSON.parse(weightsUsedStr);
    const numberOfReps = JSON.parse(numberOfRepsStr);

    let bestSetIndex = 0;
    for (let i = 1; i < weightsUsed.length; i++) {
      if (
        weightsUsed[i] > weightsUsed[bestSetIndex] ||
        (weightsUsed[i] === weightsUsed[bestSetIndex] && numberOfReps[i] > numberOfReps[bestSetIndex])
      ) {
        bestSetIndex = i;
      }
    }
    return { weight: weightsUsed[bestSetIndex], reps: numberOfReps[bestSetIndex] };
  };

  return (
    <div className="container mt-5 workout-history-container">
      <div className="row">
        <div className="col-md-4">
          <div className="calendar-container p-3 shadow-sm rounded bg-light">
            <h1 className="mt-3 mb-3"><strong>History</strong></h1>
            <Calendar
              selectRange
              onChange={(dateRange) => {
                setDates(dateRange);
                const startDate = dateRange[0];
                const endDate = new Date(dateRange[1]);
                endDate.setDate(endDate.getDate() + 1);
                fetchWorkoutHistory(startDate, endDate);
              }}
              value={dates}
            />
            <div className="mt-3">
              <strong>From:</strong> {formatDate(dates[0])} <br />
              <strong>To:</strong> {formatDate(new Date(dates[1]))}
            </div>
          </div>
        </div>
        <div className="col-md-8">
          <div className="workout-history-list p-3 shadow-sm rounded bg-light">
            <h2>Workout History ({filteredWorkoutHistory.length})</h2>

            {loading && <div className="alert alert-info">Loading...</div>}
            {error && <div className="alert alert-danger">{error}</div>}
            {successMessage && <Alert color="success">{successMessage}</Alert>}
            {currentWorkouts.length > 0 ? (
              <div className="scrollable-workout-list">
                {currentWorkouts.map((workout, index) => (
                  <div key={index} className="workout-day mb-4">
                    <h3 className="">{formatDate(new Date(workout.created))}</h3>
                    <Link to={`/workout-log/${workout.id}/details`}>
                      <button className="btn btn-success mt-2 mb-2">Details</button>
                    </Link>
                    <button className="btn btn-danger-delete mt-2 mb-2" onClick={() => toggleDeleteModal(workout.id)}>Delete</button>
                    <table className="table table-striped table-bordered workout-table">
                      <thead className="thead-dark">
                        <tr>
                          <th>Exercise</th>
                          <th>Sets</th>
                          <th>Best Set</th>
                        </tr>
                      </thead>
                      <tbody>
                        {workout.exerciseLogs.length > 0 ? (
                          workout.exerciseLogs.map((log) => {
                            const bestSet = getBestSet(log.weightsUsed, log.numberOfReps);
                            return (
                              <tr key={log.exerciseLogId}>
                                <td>{log.exerciseName}</td>
                                <td>{log.numberOfSets}</td>
                                <td>{`${bestSet.weight} kg x ${bestSet.reps}`}</td>
                              </tr>
                            );
                          })
                        ) : (
                          <tr>
                            <td colSpan="3">No exercises logged</td>
                          </tr>
                        )}
                      </tbody>
                    </table>
                  </div>
                ))}
                {filteredWorkoutHistory.length > workoutsPerPage && (
                  <nav>
                    <ul className="pagination justify-content-center mt-4">
                      <li className={`page-item ${currentPage === 1 ? 'disabled' : ''}`}>
                        <button onClick={prevPage} className="page-link">Previous</button>
                      </li>
                      {Array.from({ length: Math.ceil(filteredWorkoutHistory.length / workoutsPerPage) }, (_, index) => (
                        <li key={index} className={`page-item ${currentPage === index + 1 ? 'active' : ''}`}>
                          <button onClick={() => paginate(index + 1)} className="page-link">
                            {index + 1}
                          </button>
                        </li>
                      ))}
                      <li className={`page-item ${currentPage === Math.ceil(filteredWorkoutHistory.length / workoutsPerPage) ? 'disabled' : ''}`}>
                        <button onClick={nextPage} className="page-link">Next</button>
                      </li>
                    </ul>
                  </nav>
                )}
              </div>
            ) : (
              !loading && <p>No workout history found.</p>
            )}
          </div>
        </div>
      </div>

      {/* Delete Confirmation Modal */}
      <Modal isOpen={deleteModal} toggle={() => toggleDeleteModal(null)}>
        <ModalHeader toggle={() => toggleDeleteModal(null)}>Delete Workout History</ModalHeader>
        <ModalBody>
          Are you sure you want to delete this workout history entry?
        </ModalBody>
        <ModalFooter>
          <Button color="danger" onClick={deleteWorkout}>Yes</Button>
          <Button
            color="secondary"
            style={{ height: '60px', backgroundColor: '#1b6ec', marginRight: '10px' }}
            onClick={() => toggleDeleteModal(null)}
          >
            No
          </Button>
        </ModalFooter>
      </Modal>
    </div >
  );
}
