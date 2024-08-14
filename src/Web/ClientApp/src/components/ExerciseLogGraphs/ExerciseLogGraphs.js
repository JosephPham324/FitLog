import React, { useState, useEffect } from 'react';
import { Line } from 'react-chartjs-2';
import { Chart, registerables } from 'chart.js';
import axiosInstance from '../../utils/axiosInstance'; // Adjust the import path according to your project structure
import './ExerciseLogGraphs.css';
import { Bar } from 'recharts';
import { useParams } from 'react-router-dom'; // Import useParams from React Router
import generateContinuousData  from '../../utils/dataUtils';


Chart.register(...registerables);

const ExerciseLogGraphs = () => {
  const { id } = useParams(); // Get the exercise ID from the URL
  const [activeTab, setActiveTab] = useState('History');
  const [historyData, setHistoryData] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 5;

  const [exerciseName, setExerciseName] = useState('');
  const [estimated1RMData, setEstimated1RMData] = useState([]);
  const [actual1RMData, setActual1RMData] = useState([]);
  const [totalRepsData, setTotalRepsData] = useState([]);
  const [totalVolumeData, setTotalVolumeData] = useState([]);
  const [recordData, setRecordData] = useState(null);

  const handlePageChange = (pageNumber) => {
    setCurrentPage(pageNumber);
  };


  const currentHistoryData = historyData.slice(
    (currentPage - 1) * itemsPerPage,
    currentPage * itemsPerPage
  );

  const fetchExerciseName = async () => {
    const response = await axiosInstance.get(`https://localhost:44447/api/Exercises/${id}`);
    setExerciseName(response.data.exerciseName);
  };

  const fetchExerciseLogHistory = async () => {
    try {
      const response = await axiosInstance.get(`https://localhost:44447/api/Statistics/exercise/exercise-log-history/${id}`);
      setHistoryData(response.data);
    } catch (error) {
      console.error('Error fetching exercise log history:', error);
    }
  };

  const fetchEstimated1RMData = async () => {
    try {
      const response = await axiosInstance.get(`https://localhost:44447/api/Statistics/exercise/estimated1RM?UserId=1&ExerciseId=${id}`);
      const data = Object.keys(response.data).map(key => ({
        date: key,
        brzycki: response.data[key].brzycki
      }));
      setEstimated1RMData(data);
    } catch (error) {
      console.error('Error fetching estimated 1RM data:', error);
    }
  };
  const fetchActual1RMData = async () => {
    try {
      const response = await axiosInstance.get(`https://localhost:44447/api/Statistics/exercise/${id}/actual-1rms`);
      const data = Object.keys(response.data).map(key => ({
        date: key,
        data: response.data[key]
      }));
      console.log(response.data)
      console.log(data)
      setActual1RMData(data);
    } catch (error) {
      console.error('Error fetching estimated 1RM data:', error);
    }
  };

  const fetchTotalRepsData = async () => {
    try {
      const response = await axiosInstance.get(`https://localhost:44447/api/Statistics/exercise/${id}/total-reps?TimeFrame=weekly`);
      console.log(response.data)
      const continuousData = generateContinuousData(response.data, 'weekly');
      console.log(continuousData);
      const data = Object.keys(response.data).map(key => ({
        date: key,
        reps: response.data[key]
      }));
      setTotalRepsData(data);
    } catch (error) {
      console.error('Error fetching total reps data:', error);
      setTotalRepsData([]); // Set to an empty array to prevent further issues
    }
  };

  const fetchTotalTonnageData = async () => {
    try {
      const response = await axiosInstance.get(`https://localhost:44447/api/Statistics/exercise/${id}/total-tonnage?ExerciseId=${id}&TimeFrame=weekly`);
      const data = Object.keys(response.data).map(key => ({
        date: key,
        volume: response.data[key]
      }));
      setTotalVolumeData(data);
    } catch (error) {
      console.error('Error fetching total volume data:', error);
      setTotalVolumeData([]); // Set to an empty array to prevent further issues
    }
  };

  const fetchRecordData = async () => {
    try {
      const response = await axiosInstance.get(`https://localhost:44447/api/Statistics/exercise/${id}/records`);
      console.log(response.data)
      // Convert bestPerformances object to an array
      const bestPerformances = response.data.bestPerformances;
      setRecordData({ ...response.data, bestPerformances });
    } catch (error) {
      console.error('Error fetching record data:', error);
      setRecordData(null); // Set to null to prevent further issues
    }
  };

  useEffect(() => {
    fetchExerciseLogHistory();
    fetchEstimated1RMData();
    fetchTotalRepsData();
    fetchTotalTonnageData();
    fetchRecordData();
    fetchActual1RMData();
  }, [id]); // Depend on the ID from the URL

  const chartDataEstimated1RM = {
    labels: estimated1RMData.map(item => new Date(item.date).toLocaleDateString('en-GB')),
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
    labels: actual1RMData.map(item => new Date(item.date).toLocaleDateString('en-GB')), // Format the date
    datasets: [
      {
        label: 'Actual 1RM',
        data: actual1RMData.map(item => item.data), // Extract the 1RM values
        borderColor: 'yellow',
        backgroundColor: 'rgba(255, 255, 0, 0.2)',
      }
    ],
  };

  const chartDataTonnage = {
    labels: (totalVolumeData || []).map(item => new Date(item.date).toLocaleDateString('en-GB')),
    datasets: [
      {
        label: 'Weekly Training Tonnage',
        data: (totalVolumeData || []).map(item => item.volume),
        borderColor: 'orange',
        backgroundColor: 'rgba(255, 165, 0, 0.2)',
      }
    ],
  };

  const chartDataReps = {
    labels: (totalRepsData || []).map(item => new Date(item.date).toLocaleDateString('en-GB')),
    datasets: [
      {
        label: 'Weekly Reps',
        data: (totalRepsData || []).map(item => item.reps),
        borderColor: 'yellow',
        backgroundColor: 'rgba(255, 255, 0, 0.2)',
      }
    ],
  };

  return (
    <div className="exercise-log-graphs">
      <div className="he">
        <h1>{exerciseName} Statistics</h1> {/* Added title */}
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
        <div>
          {currentHistoryData.map((session, index) => {
            const weights = session.weightsUsed
              ? session.weightsUsed.slice(1, -1).split(',').map(w => w.trim())
              : [];
            const reps = session.numberOfReps
              ? session.numberOfReps.slice(1, -1).split(',').map(r => r.trim())
              : [];

            const numberOfSets = Math.min(weights.length, reps.length, session.numberOfSets);

            return (
              <div key={index} className="workout-session">
                <h2>{session.exerciseName}</h2>
                <p>{new Date(session.dateCreated).toDateString()}</p>
                <table>
                  <thead>
                    <tr>
                      <th>Set</th>
                      <th>Completed</th>
                    </tr>
                  </thead>
                  <tbody>
                    {Array.from({ length: numberOfSets }).map((_, idx) => (
                      <tr key={idx}>
                        <td>{idx + 1}</td>
                        <td>{weights[idx] ? weights[idx] : 'N/A'} x {reps[idx] ? reps[idx] : 'N/A'}</td>
                      </tr>
                    ))}
                    {numberOfSets < session.numberOfSets && (
                      <tr>
                        <td colSpan="2">Incomplete data for some sets</td>
                      </tr>
                    )}
                  </tbody>
                </table>
              </div>
            );
          })}

          {/* Pagination controls */}
          <div className="pagination-controls">
            {Array.from({ length: Math.ceil(historyData.length / itemsPerPage) }).map((_, idx) => (
              <button
                key={idx + 1}
                onClick={() => handlePageChange(idx + 1)}
                className={`page-button ${currentPage === idx + 1 ? 'active' : ''}`}
              >
                {idx + 1}
              </button>
            ))}
          </div>
        </div>
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
              <h3>Total Training Tonnage</h3>
              <Line data={chartDataTonnage} />
            </div>
            <div className="chart-container">
              <h3>Total Reps</h3>
              <Line data={chartDataReps} />
            </div>
          </div>
        </div>
      )}

      {activeTab === 'Records' && recordData && (
        <div className="records">
          <h3>Personal Records</h3>
          <div className="personal-records">
            <div className="record-item">
              <span>Actual 1 Rep Max</span>
              <span>{recordData.actual1RepMax.toFixed(1) + "kg" || 'N/A'}</span>
            </div>
            <div className="record-item">
              <span>Estimated 1 Rep Max</span>
              <span>{recordData.estimated1RepMax.toFixed(1) + "kg" || 'N/A'}</span>
            </div>
            <div className="record-item">
              <span>Max Tonnage</span>
              <span>{recordData.maxVolume.toFixed(1) + "kg" || 'N/A'}</span>
            </div>
          </div>
          <button className="view-record-history">
            <span className="gradient-text">Record History</span>
          </button>
          <table className="record-history">
            <thead>
              <tr>
                <th>Reps</th>
                <th>Weight</th>
                <th>Date</th>
              </tr>
            </thead>
            <tbody>
              {Object.keys(recordData.bestPerformances).map(key => {
                const data = recordData.bestPerformances[key];
                return (
                  <tr key={key}>
                    <td>{key}</td>
                    <td>{data.weight}</td>
                    <td>{new Date(data.date).toLocaleDateString()}</td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
};

export default ExerciseLogGraphs;
