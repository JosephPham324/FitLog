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
  const [totalRepsData, setTotalRepsData] = useState([]);
  const [totalVolumeData, setTotalVolumeData] = useState([]);
  const [recordData, setRecordData] = useState(null);

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

  const fetchTotalRepsData = async () => {
    try {
      const response = await axiosInstance.get('https://localhost:44447/api/Statistics/exercise/1/total-reps?TimeFrame=weekly');
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

  const fetchTotalVolumeData = async () => {
    try {
      const response = await axiosInstance.get('https://localhost:44447/api/Statistics/exercise/1/total-tonnage?TimeFrame=weekly');
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
      const response = await axiosInstance.get('https://localhost:44447/api/Statistics/exercise/1/records');
      // Convert bestPerformances object to an array
      const bestPerformances = Object.values(response.data.bestPerformances);
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
    fetchTotalVolumeData();
    fetchRecordData();
  }, []);

  const chartDataEstimated1RM = {
    labels: (estimated1RMData || []).map(item => new Date(item.date).toLocaleDateString()),
    datasets: [
      {
        label: 'Estimated 1RM using Brzycki',
        data: estimated1RMData.map(item => item.brzycki),
        borderColor: 'red',
        backgroundColor: 'rgba(255, 165, 0, 0.2)',
      }
    ],
  };

  const chartDataActual1RM = {
    labels: (recordData?.bestPerformances || []).map(item => new Date(item.date).toLocaleDateString()),
    datasets: [
      {
        label: 'Actual 1RM',
        data: (recordData?.bestPerformances || []).map(item => item.weight),
        borderColor: 'yellow',
        backgroundColor: 'rgba(255, 255, 0, 0.2)',
      }
    ],
  };

  const chartDataVolume = {
    labels: (totalVolumeData || []).map(item => new Date(item.date).toLocaleDateString()),
    datasets: [
      {
        label: 'Total Training Volume',
        data: (totalVolumeData || []).map(item => item.volume),
        borderColor: 'blue',
        backgroundColor: 'rgba(255, 165, 0, 0.2)',
      }
    ],
  };

  const chartDataReps = {
    labels: (totalRepsData || []).map(item => new Date(item.date).toLocaleDateString()),
    datasets: [
      {
        label: 'Total Reps',
        data: (totalRepsData || []).map(item => item.reps),
        borderColor: 'green',
        backgroundColor: 'rgba(255, 255, 0, 0.2)',
      }
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

      {activeTab === 'Records' && recordData && (
        <div className="records">
          <h3>Personal Records</h3>
          <div className="personal-records">
            <div className="record-item">
              <span>Actual 1 Rep Max</span>
              <span>{recordData.actual1RepMax || 'N/A'}</span>
            </div>
            <div className="record-item">
              <span>Estimated 1 Rep Max</span>
              <span>{recordData.estimated1RepMax || 'N/A'}</span>
            </div>
            <div className="record-item">
              <span>Max Volume</span>
              <span>{recordData.maxVolume || 'N/A'}</span>
            </div>
          </div>
          <button className="view-record-history">
            <span className="gradient-text">View Record History </span>
          </button>
          <table className="record-history">
            <thead>
              <tr>
                <th>Weight</th>
                <th>Date</th>
              </tr>
            </thead>
            <tbody>
              {(recordData.bestPerformances || []).map((record, index) => (
                <tr key={index}>
                  <td>{record.weight}</td>
                  <td>{new Date(record.date).toLocaleDateString()}</td>
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
