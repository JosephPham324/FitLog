import React, { useState } from 'react';
import Calendar from 'react-calendar';
import 'react-calendar/dist/Calendar.css';
import './WorkoutHistory.css';

const workoutData = {
  '2024-07-13': [
    { exercise: 'Neck curl', sets: 4, bestSet: '20 kg x 35' },
    { exercise: 'Cable hammer curls', sets: 2, bestSet: '55 kg x 19' },
    { exercise: 'Lu Raises', sets: 4, bestSet: '8 kg x 13' },
    { exercise: 'Ring Face Pulls', sets: 3, bestSet: '83 kg x 23' },
    { exercise: 'Preacher Curl', sets: 3, bestSet: '22.5 kg x 3' },
  ],
  '2024-07-14': [
    { exercise: 'Bench Press (Barbell)', sets: 3, bestSet: '90 kg x 2' },
    { exercise: 'Pull-Up (Weighted)', sets: 3, bestSet: '95 kg x 8' },
    { exercise: 'AD Press', sets: 3, bestSet: '60 kg x 6' },
    { exercise: 'Wide Grip Lat Pulldown', sets: 5, bestSet: '90 kg x 2' },
    { exercise: 'Cable Crossover', sets: 3, bestSet: '90 kg x 2' },
  ],
  '2024-07-15': [
    { exercise: 'Bench Press (Barbell)', sets: 3, bestSet: '90 kg x 2' },
    { exercise: 'Pull-Up (Weighted)', sets: 3, bestSet: '95 kg x 8' },
    { exercise: 'AD Press', sets: 3, bestSet: '60 kg x 6' },
    { exercise: 'Wide Grip Lat Pulldown', sets: 5, bestSet: '90 kg x 2' },
    { exercise: 'Cable Crossover', sets: 3, bestSet: '90 kg x 2' },
  ],
  '2024-07-16': [
    { exercise: 'Bench Press (Barbell)', sets: 3, bestSet: '90 kg x 2' },
    { exercise: 'Pull-Up (Weighted)', sets: 3, bestSet: '95 kg x 8' },
    { exercise: 'AD Press', sets: 3, bestSet: '60 kg x 6' },
    { exercise: 'Wide Grip Lat Pulldown', sets: 5, bestSet: '90 kg x 2' },
    { exercise: 'Cable Crossover', sets: 3, bestSet: '90 kg x 2' },
  ],
  '2024-07-17': [
    { exercise: 'Bench Press (Barbell)', sets: 3, bestSet: '90 kg x 2' },
    { exercise: 'Pull-Up (Weighted)', sets: 3, bestSet: '95 kg x 8' },
    { exercise: 'AD Press', sets: 3, bestSet: '60 kg x 6' },
    { exercise: 'Wide Grip Lat Pulldown', sets: 5, bestSet: '90 kg x 2' },
    { exercise: 'Cable Crossover', sets: 3, bestSet: '90 kg x 2' },
  ],
  '2024-07-18': [
    { exercise: 'Bench Press (Barbell)', sets: 3, bestSet: '90 kg x 2' },
    { exercise: 'Pull-Up (Weighted)', sets: 3, bestSet: '95 kg x 8' },
    { exercise: 'AD Press', sets: 3, bestSet: '60 kg x 6' },
    { exercise: 'Wide Grip Lat Pulldown', sets: 5, bestSet: '90 kg x 2' },
    { exercise: 'Cable Crossover', sets: 3, bestSet: '90 kg x 2' },
  ],
  '2024-07-19': [
    { exercise: 'Bench Press (Barbell)', sets: 3, bestSet: '90 kg x 2' },
    { exercise: 'Pull-Up (Weighted)', sets: 3, bestSet: '95 kg x 8' },
    { exercise: 'AD Press', sets: 3, bestSet: '60 kg x 6' },
    { exercise: 'Wide Grip Lat Pulldown', sets: 5, bestSet: '90 kg x 2' },
    { exercise: 'Cable Crossover', sets: 3, bestSet: '90 kg x 2' },
  ],
  '2024-07-20': [
    { exercise: 'Bench Press (Barbell)', sets: 3, bestSet: '90 kg x 2' },
    { exercise: 'Pull-Up (Weighted)', sets: 3, bestSet: '95 kg x 8' },
    { exercise: 'AD Press', sets: 3, bestSet: '60 kg x 6' },
    { exercise: 'Wide Grip Lat Pulldown', sets: 5, bestSet: '90 kg x 2' },
    { exercise: 'Cable Crossover', sets: 3, bestSet: '90 kg x 2' },
  ],


};

export function WorkoutHistory() {
  const itemsPerPage = 7; // Số lượng mục trên mỗi trang
  const [currentPage, setCurrentPage] = useState(1);

  // Tính toán các mục Workout History được hiển thị trên trang hiện tại
  const startIndex = (currentPage - 1) * itemsPerPage;
  const endIndex = startIndex + itemsPerPage;
  const displayedWorkouts = Object.keys(workoutData).slice(startIndex, endIndex);

  // Xử lý khi chuyển đến trang trước
  const goToPreviousPage = () => {
    if (currentPage > 1) {
      setCurrentPage(currentPage - 1);
    }
  };

  // Xử lý khi chuyển đến trang tiếp theo
  const goToNextPage = () => {
    const totalPages = Math.ceil(Object.keys(workoutData).length / itemsPerPage);
    if (currentPage < totalPages) {
      setCurrentPage(currentPage + 1);
    }
  };

  return (
    <div className="workout-history container-fluid">
      <div className="row">
        <div className="col-md-4 calendar-container">
          <h2>History</h2>
          <Calendar
            tileClassName={({ date }) => {
              const dateString = date.toISOString().split('T')[0];
              if (workoutData[dateString]) {
                return 'has-workout';
              }
            }}
          />
        </div>
        <div className="col-md-8 workout-container">
          <h2>Workout History ({Object.keys(workoutData).length})</h2>
          <div className="workout-list">
            {displayedWorkouts.map((dateString) => (
              <div key={dateString} className="workout-details">
                <h3>{new Date(dateString).toDateString()}</h3>
                {workoutData[dateString].map((workout, index) => (
                  <div key={index} className="workout-entry">
                    <span>{workout.exercise}</span>
                    <span>{workout.sets} sets</span>
                    <span>{workout.bestSet}</span>
                  </div>
                ))}
              </div>
            ))}
          </div>
          {/* Nút điều hướng trang */}
          <div className="pagination">
            <button onClick={goToPreviousPage} disabled={currentPage === 1}>
              Previous
            </button>
            <button onClick={goToNextPage} disabled={currentPage * itemsPerPage >= Object.keys(workoutData).length}>
              Next
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}
