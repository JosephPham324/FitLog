//import React, { useState, useEffect } from 'react';
//import { Line } from 'react-chartjs-2';
//import { Chart, registerables } from 'chart.js';
//import axiosInstance from '../../utils/axiosInstance'; // Adjust the import path according to your project structure
//import './ExerciseLogGraphs.css';

//Chart.register(...registerables);

//const ExerciseLogGraphs = () => {
//  const [activeTab, setActiveTab] = useState('History');
//  const [historyData, setHistoryData] = useState([]);

//  const fetchExerciseLogHistory = async () => {
//    try {
//      const response = await axiosInstance.get('https://localhost:44447/api/Statistics/exercise/exercise-log-history/1');
//      const data = response.data;
//      setHistoryData(data);
//    } catch (error) {
//      console.error('Error fetching exercise log history:', error);
//    }
//  };

//  useEffect(() => {
//    fetchExerciseLogHistory();
//  }, []);

//  const chartDataEstimated1RM = {
//    labels: ['2/5/2024', '4/15/2024', '4/22/2024', '5/27/2024', '6/10/2024', '6/17/2024', '6/24/2024', '7/1/2024', '7/8/2024'],
//    datasets: [
//      {
//        label: 'Estimated 1RM using Brzycki',
//        data: [150, 140, 145, 135, 130, 140, 150, 145, 133],
//        borderColor: 'orange',
//        backgroundColor: 'rgba(255, 165, 0, 0.2)',
//      }
//    ],
//  };

//  const chartDataActual1RM = {
//    labels: ['2/5/2024', '4/22/2024'],
//    datasets: [
//      {
//        label: 'Actual 1RM',
//        data: [150, 150],
//        borderColor: 'yellow',
//        backgroundColor: 'rgba(255, 255, 0, 0.2)',
//      }
//    ],
//  };

//  const chartDataVolume = {
//    labels: ['2/5/2024', '4/15/2024', '4/22/2024', '5/27/2024', '6/10/2024', '6/17/2024', '6/24/2024', '7/1/2024', '7/8/2024'],
//    datasets: [
//      {
//        label: 'Total Training Volume',
//        data: [1000, 1500, 1200, 1300, 1400, 1800, 2000, 2500, 3290],
//        borderColor: 'orange',
//        backgroundColor: 'rgba(255, 165, 0, 0.2)',
//      }
//    ],
//  };

//  const chartDataReps = {
//    labels: ['2/5/2024', '4/15/2024', '4/22/2024', '5/27/2024', '6/10/2024', '6/17/2024', '6/24/2024', '7/1/2024', '7/8/2024'],
//    datasets: [
//      {
//        label: 'Total Reps',
//        data: [10, 20, 15, 18, 12, 25, 28, 30, 46],
//        borderColor: 'yellow',
//        backgroundColor: 'rgba(255, 255, 0, 0.2)',
//      }
//    ],
//  };

//  const recordData = {
//    personalRecords: {
//      actual1RM: '152.5 kg',
//      estimated1RM: '152.9 kg',
//      maxVolume: '735 kg',
//    },
//    history: [
//      { reps: 1, bestPerformance: '152.5 kg', date: 'Feb 07, 2024' },
//      { reps: 2, bestPerformance: '120 kg', date: 'May 30, 2024' },
//      { reps: 3, bestPerformance: '130 kg', date: 'Apr 19, 2024' },
//      { reps: 4, bestPerformance: '130 kg', date: 'Jul 06, 2024' },
//      { reps: 5, bestPerformance: '130 kg', date: 'May 30, 2024' },
//      { reps: 6, bestPerformance: '120 kg', date: 'Apr 19, 2024' },
//      { reps: 7, bestPerformance: '105 kg', date: 'Jul 12, 2023' },
//      { reps: 8, bestPerformance: '-', date: '-' },
//      { reps: 9, bestPerformance: '-', date: '-' },
//    ],
//  };

//  return (
//    <div className="exercise-log-graphs">
//      <div className="he">
//        <h1>Exercise Log</h1> {/* Added title */}
//      </div>

//      <div className="tabss">
//        <div
//          className={`tab ${activeTab === 'History' ? 'active' : ''}`}
//          onClick={() => setActiveTab('History')}
//        >
//          History
//        </div>
//        <div
//          className={`tab ${activeTab === 'Charts' ? 'active' : ''}`}
//          onClick={() => setActiveTab('Charts')}
//        >
//          Charts
//        </div>
//        <div
//          className={`tab ${activeTab === 'Records' ? 'active' : ''}`}
//          onClick={() => setActiveTab('Records')}
//        >
//          Records
//        </div>
//      </div>

//      {activeTab === 'History' && (
//        historyData.map((session, index) => (
//          <div key={index} className="workout-session">
//            <h2>{session.exerciseName}</h2>
//            <p>{new Date(session.dateCreated).toDateString()}</p>
//            <table>
//              <thead>
//                <tr>
//                  <th>Sets</th>
//                  <th>Completed</th>
//                </tr>
//              </thead>
//              <tbody>
//                {Array.from({ length: session.numberOfSets }).map((_, idx) => (
//                  <tr key={idx}>
//                    <td>{idx + 1}</td>
//                    <td>{session.weightsUsed} x {session.numberOfReps}</td>
//                  </tr>
//                ))}
//              </tbody>
//            </table>
//          </div>
//        ))
//      )}

//      {activeTab === 'Charts' && (
//        <div className="charts">
//          <div className="chart-row">
//            <div className="chart-container">
//              <h3>Estimated 1RM using Brzycki</h3>
//              <Line data={chartDataEstimated1RM} />
//            </div>
//            <div className="chart-container">
//              <h3>Actual 1RM</h3>
//              <Line data={chartDataActual1RM} />
//            </div>
//          </div>
//          <div className="chart-row">
//            <div className="chart-container">
//              <h3>Total Training Volume</h3>
//              <Line data={chartDataVolume} />
//            </div>
//            <div className="chart-container">
//              <h3>Total Reps</h3>
//              <Line data={chartDataReps} />
//            </div>
//          </div>
//        </div>
//      )}

//      {activeTab === 'Records' && (
//        <div className="records">
//          <h3>Personal Records</h3>
//          <div className="personal-records">
//            <div className="record-item">
//              <span>Actual 1 Rep Max</span>
//              <span>{recordData.personalRecords.actual1RM}</span>
//            </div>
//            <div className="record-item">
//              <span>Estimated 1 Rep Max</span>
//              <span>{recordData.personalRecords.estimated1RM}</span>
//            </div>
//            <div className="record-item">
//              <span>Max Volume</span>
//              <span>{recordData.personalRecords.maxVolume}</span>
//            </div>
//          </div>
//          <button className="view-record-history">
//            <span className="gradient-text">View Record History </span>
//          </button>
//          <table className="record-history">
//            <thead>
//              <tr>
//                <th>Reps</th>
//                <th>Best Performance</th>
//                <th>Date</th>
//              </tr>
//            </thead>
//            <tbody>
//              {recordData.history.map((record, index) => (
//                <tr key={index}>
//                  <td>{record.reps}</td>
//                  <td>{record.bestPerformance}</td>
//                  <td>{record.date}</td>
//                </tr>
//              ))}
//            </tbody>
//          </table>
//        </div>
//      )}
//    </div>);
//};

//export default ExerciseLogGraphs;


import React, { useState, useEffect } from 'react';
import { Line } from 'react-chartjs-2';
import { Chart, registerables } from 'chart.js';
import axiosInstance from '../../utils/axiosInstance'; // Adjust the import path according to your project structure
import './ExerciseLogGraphs.css';

Chart.register(...registerables);

const ExerciseLogGraphs = () => {
  const [activeTab, setActiveTab] = useState('History');
  const [historyData, setHistoryData] = useState([]);
  const [estimated1RMData, setEstimated1RMData] = useState([]);

  const fetchExerciseLogHistory = async () => {
    try {
      const response = await axiosInstance.get('https://localhost:44447/api/Statistics/exercise/exercise-log-history/1');
      setHistoryData(response.data);
    } catch (error) {
      console.error('Error fetching exercise log history:', error);
    }
  };

  const fetchEstimated1RMData = async () => {
    try {
      const response = await axiosInstance.get('https://localhost:44447/api/Statistics/exercise/estimated1RM?UserId=1&ExerciseId=1');
      const data = Object.keys(response.data).map(key => ({
        date: key,
        brzycki: response.data[key].brzycki
      }));
      setEstimated1RMData(data);
    } catch (error) {
      console.error('Error fetching estimated 1RM data:', error);
    }
  };

  useEffect(() => {
    fetchExerciseLogHistory();
    fetchEstimated1RMData();
  }, []);

  const chartDataEstimated1RM = {
    labels: estimated1RMData.map(item => item.date),
    datasets: [
      {
        label: 'Estimated 1RM using Brzycki',
        data: estimated1RMData.map(item => item.brzycki),
        borderColor: 'orange',
        backgroundColor: 'rgba(255, 165, 0, 0.2)',
      }
    ],
  };

  const chartDataActual1RM = {
    labels: ['2/5/2024', '4/22/2024'],
    datasets: [
      {
        label: 'Actual 1RM',
        data: [150, 150],
        borderColor: 'yellow',
        backgroundColor: 'rgba(255, 255, 0, 0.2)',
      }
    ],
  };

  const chartDataVolume = {
    labels: ['2/5/2024', '4/15/2024', '4/22/2024', '5/27/2024', '6/10/2024', '6/17/2024', '6/24/2024', '7/1/2024', '7/8/2024'],
    datasets: [
      {
        label: 'Total Training Volume',
        data: [1000, 1500, 1200, 1300, 1400, 1800, 2000, 2500, 3290],
        borderColor: 'orange',
        backgroundColor: 'rgba(255, 165, 0, 0.2)',
      }
    ],
  };

  const chartDataReps = {
    labels: ['2/5/2024', '4/15/2024', '4/22/2024', '5/27/2024', '6/10/2024', '6/17/2024', '6/24/2024', '7/1/2024', '7/8/2024'],
    datasets: [
      {
        label: 'Total Reps',
        data: [10, 20, 15, 18, 12, 25, 28, 30, 46],
        borderColor: 'yellow',
        backgroundColor: 'rgba(255, 255, 0, 0.2)',
      }
    ],
  };

  const recordData = {
    personalRecords: {
      actual1RM: '152.5 kg',
      estimated1RM: '152.9 kg',
      maxVolume: '735 kg',
    },
    history: [
      { reps: 1, bestPerformance: '152.5 kg', date: 'Feb 07, 2024' },
      { reps: 2, bestPerformance: '120 kg', date: 'May 30, 2024' },
      { reps: 3, bestPerformance: '130 kg', date: 'Apr 19, 2024' },
      { reps: 4, bestPerformance: '130 kg', date: 'Jul 06, 2024' },
      { reps: 5, bestPerformance: '130 kg', date: 'May 30, 2024' },
      { reps: 6, bestPerformance: '120 kg', date: 'Apr 19, 2024' },
      { reps: 7, bestPerformance: '105 kg', date: 'Jul 12, 2023' },
      { reps: 8, bestPerformance: '-', date: '-' },
      { reps: 9, bestPerformance: '-', date: '-' },
    ],
  };

  return (
    <div className="exercise-log-graphs">
      <div className="he">
        <h1>Exercise Log</h1> {/* Added title */}
      </div>

      <div className="tabss">
        <div
          className={`tab ${activeTab === 'History' ? 'active' : ''}`}
          onClick={() => setActiveTab('History')}
        >
          History
        </div>
        <div
          className={`tab ${activeTab === 'Charts' ? 'active' : ''}`}
          onClick={() => setActiveTab('Charts')}
        >
          Charts
        </div>
        <div
          className={`tab ${activeTab === 'Records' ? 'active' : ''}`}
          onClick={() => setActiveTab('Records')}
        >
          Records
        </div>
      </div>

      {activeTab === 'History' && (
        historyData.map((session, index) => (
          <div key={index} className="workout-session">
            <h2>{session.exerciseName}</h2>
            <p>{new Date(session.dateCreated).toDateString()}</p>
            <table>
              <thead>
                <tr>
                  <th>Sets</th>
                  <th>Completed</th>
                </tr>
              </thead>
              <tbody>
                {Array.from({ length: session.numberOfSets }).map((_, idx) => (
                  <tr key={idx}>
                    <td>{idx + 1}</td>
                    <td>{session.weightsUsed} x {session.numberOfReps}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        ))
      )}

      {activeTab === 'Charts' && (
        <div className="charts">
          <div className="chart-row">
            <div className="chart-container">
              <h3>Estimated 1RM using Brzycki</h3>
              <Line data={chartDataEstimated1RM} />
            </div>
            <div className="chart-container">
              <h3>Actual 1RM</h3>
              <Line data={chartDataActual1RM} />
            </div>
          </div>
          <div className="chart-row">
            <div className="chart-container">
              <h3>Total Training Volume</h3>
              <Line data={chartDataVolume} />
            </div>
            <div className="chart-container">
              <h3>Total Reps</h3>
              <Line data={chartDataReps} />
            </div>
          </div>
        </div>
      )}

      {activeTab === 'Records' && (
        <div className="records">
          <h3>Personal Records</h3>
          <div className="personal-records">
            <div className="record-item">
              <span>Actual 1 Rep Max</span>
              <span>{recordData.personalRecords.actual1RM}</span>
            </div>
            <div className="record-item">
              <span>Estimated 1 Rep Max</span>
              <span>{recordData.personalRecords.estimated1RM}</span>
            </div>
            <div className="record-item">
              <span>Max Volume</span>
              <span>{recordData.personalRecords.maxVolume}</span>
            </div>
          </div>
          <button className="view-record-history">
            <span className="gradient-text">View Record History </span>
          </button>
          <table className="record-history">
            <thead>
              <tr>
                <th>Reps</th>
                <th>Best Performance</th>
                <th>Date</th>
              </tr>
            </thead>
            <tbody>
              {recordData.history.map((record, index) => (
                <tr key={index}>
                  <td>{record.reps}</td>
                  <td>{record.bestPerformance}</td>
                  <td>{record.date}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
};

export default ExerciseLogGraphs;
