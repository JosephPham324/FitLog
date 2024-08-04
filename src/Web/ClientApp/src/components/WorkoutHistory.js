import React, { useEffect, useState } from 'react';
import Calendar from 'react-calendar';
import 'react-calendar/dist/Calendar.css';
import './WorkoutHistory.css';
import axiosInstance from '../utils/axiosInstance'; // Import the configured Axios instance

export function WorkoutHistory() {
  const itemsPerPage = 7; // Số lượng mục trên mỗi trang
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [workoutData, setWorkoutData] = useState({});
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchWorkoutData = async () => {
      try {
        const response = await axiosInstance.get(`/workoutLog/get-all`, {
          params: {
            PageNumber: currentPage,
            PageSize: itemsPerPage,
          },
          headers: {
            accept: 'application/json',
          },
        });

        console.log("API Response:", response.data);

        const data = response.data.items.reduce((acc, item) => {
          const dateString = item.created; // Use the `created` field from the workout log
          if (!dateString) {
            console.error("Invalid date format: undefined", item);
            return acc; // Skip this item
          }

          const date = new Date(dateString);
          if (isNaN(date)) {
            console.error("Invalid date format:", dateString, item);
            return acc; // Skip this item
          }

          const formattedDate = date.toISOString().split('T')[0];
          if (!acc[formattedDate]) {
            acc[formattedDate] = [];
          }

          item.exerciseLogs.forEach(exerciseLog => {
            // Ensure weightsUsed and numberOfReps are valid strings before parsing
            const weightsString = exerciseLog.weightsUsed || '';
            const repsString = exerciseLog.numberOfReps || '';

            // Split and parse weights and reps into arrays of numbers
            const weights = weightsString.split(',').map(weight => parseFloat(weight.trim())).filter(weight => !isNaN(weight));
            const reps = repsString.split(',').map(rep => parseInt(rep.trim())).filter(rep => !isNaN(rep));

            // Get the max values
            const maxWeight = weights.length > 0 ? Math.max(...weights) : 0;
            const maxReps = reps.length > 0 ? Math.max(...reps) : 0;

            acc[formattedDate].push({
              exercise: exerciseLog.exerciseName,
              sets: exerciseLog.numberOfSets,
              bestSet: `${maxWeight} kg x ${maxReps}`,
            });
          });

          return acc;
        }, {});

        setWorkoutData(data);
        setTotalPages(response.data.totalPages); // Ensure this is coming from the server response
      } catch (error) {
        setError('Error fetching workout data');
        console.error('Error fetching workout data:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchWorkoutData();
  }, [currentPage]);

  // Tính toán các mục Workout History được hiển thị trên trang hiện tại
  const displayedWorkouts = Object.keys(workoutData);

  // Xử lý khi chuyển đến trang trước
  const goToPreviousPage = () => {
    if (currentPage > 1) {
      setCurrentPage(currentPage - 1);
    }
  };

  // Xử lý khi chuyển đến trang tiếp theo
  const goToNextPage = () => {
    if (currentPage < totalPages) {
      setCurrentPage(currentPage + 1);
    }
  };

  const goToPage = (pageNumber) => {
    if (pageNumber >= 1 && pageNumber <= totalPages) {
      setCurrentPage(pageNumber);
    }
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div className="workout-history container-fluid">
      <div className="calendar-container">
        <h2>History</h2>
        <Calendar
          tileClassName={({ date }) => {
            const dateString = date.toISOString().split('T')[0];
            if (workoutData[dateString]) {
              return 'has-workout';
            }
            return null;
          }}
        />
      </div>
      <div className="workout-container">
        <h2>Workout History ({Object.keys(workoutData).length})</h2>
        <div className="workout-list">
          {displayedWorkouts.map((dateString) => (
            <div key={dateString} className="workout-details">
              <h3>{new Date(dateString).toDateString()}</h3>
              <table>
                <thead>
                  <tr>
                    <th>Exercise</th>
                    <th>Sets</th>
                    <th>Best Set</th>
                  </tr>
                </thead>
                <tbody>
                  {workoutData[dateString].map((workout, index) => (
                    <tr key={index}>
                      <td>{workout.exercise}</td>
                      <td>{workout.sets}</td>
                      <td>{workout.bestSet}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          ))}
        </div>
        {/* Pagination Control */}
        <div className="pagination">
          <button onClick={goToPreviousPage} disabled={currentPage === 1}>
            &laquo;
          </button>
          {Array.from({ length: totalPages }, (_, index) => (
            <button
              key={index + 1}
              onClick={() => goToPage(index + 1)}
              className={index + 1 === currentPage ? 'active' : ''}
            >
              {index + 1}
            </button>
          ))}
          <button onClick={goToNextPage} disabled={currentPage === totalPages}>
            &raquo;
          </button>
        </div>
      </div>
    </div>
  );
}
