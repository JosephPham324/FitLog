import axiosInstance from '../../utils/axiosInstance';
import React, { useState, useEffect } from 'react';
import './WorkoutLogGraphs.css';
import MuscleGroupsExercises from '../../assets/MuscleGroupsExercises.png';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, BarChart, Bar } from 'recharts';
import { format, startOfWeek, endOfWeek, startOfMonth, endOfMonth, addWeeks, subWeeks, addMonths, subMonths, addYears, subYears } from 'date-fns';

const WorkoutLogGraphs = () => {
  const [activeTab, setActiveTab] = useState('Weekly');
  const [modalOpen, setModalOpen] = useState(false);
  const [summaryData, setSummaryData] = useState({
    numberOfWorkouts: 0,
    hoursAtTheGym: 0,
    weightLifted: 0,
    weekStreak: 0,
  });
  const [chartData, setChartData] = useState([]);
  const [muscleEngagementData, setMuscleEngagementData] = useState([]);
  const [totalRepsData, setTotalRepsData] = useState([]);
  const [frequencyData, setFrequencyData] = useState([]);
  const [dateRange, setDateRange] = useState({ start: new Date(), end: new Date() });

  const toggleModal = () => {
    setModalOpen(!modalOpen);
  };

  const updateDateRange = (timeFrame, newStart) => {
    let start = newStart || dateRange.start;
    let end;
    switch (timeFrame) {
      case 'Weekly':
        start = startOfWeek(start, { weekStartsOn: 1 });
        end = endOfWeek(start, { weekStartsOn: 1 });
        break;
      case 'Monthly':
        start = startOfMonth(start);
        end = endOfMonth(start);
        break;
      case 'Yearly':
        start = new Date(start.getFullYear(), 0, 1);
        end = new Date(start.getFullYear(), 11, 31);
        break;
      default:
        start = startOfWeek(start, { weekStartsOn: 1 });
        end = endOfWeek(start, { weekStartsOn: 1 });
    }
    setDateRange({ start, end });
  };

  const fetchData = async () => {
    try {
      // Fetch and handle the summary data (as before)
      const summaryResponse = await axiosInstance.get('/Statistics/overall/summary', {
        params: { TimeFrame: activeTab },
      });
      console.log("API Summary Response:", summaryResponse.data);

      const summaryData = summaryResponse.data;
      for (let key in summaryData) {
        if (summaryData.hasOwnProperty(key)) {
          setSummaryData(summaryData[key]);
        }
      }

      if (summaryData === null) {
        setSummaryData({
          numberOfWorkouts: 0,
          hoursAtGym: 0,
          totalWeightLifted: 0,
          weekStreak: 0,
        });
      }

      // Fetch and handle the muscle engagement data (as before)
      const muscleEngagementResponse = await axiosInstance.get('/Statistics/overall/muscles-engagement', {
        params: { TimeFrame: activeTab },
      });
      console.log("API Muscle Engagement Response:", muscleEngagementResponse.data);

      const muscleData = [];
      for (let key in muscleEngagementResponse.data) {
        if (muscleEngagementResponse.data.hasOwnProperty(key)) {
          muscleData.push(...muscleEngagementResponse.data[key]);
        }
      }
      setMuscleEngagementData(muscleData);

      // Fetch and handle total reps data
      const totalRepsResponse = await axiosInstance.get('/Statistics/overall/total-training-reps', {
        params: { TimeFrame: activeTab },
      });
      console.log("API Total Training Reps Response:", totalRepsResponse.data);


      const repsData = [];
      for (let key in totalRepsResponse.data) {
        if (totalRepsResponse.data.hasOwnProperty(key)) {
          repsData.push({
            date: format(new Date(key), 'yyyy-MM-dd'), // Format the date
            reps: totalRepsResponse.data[key],
          });
        }
      }
      setTotalRepsData(repsData);

      // Fetch and handle total tonnage data
      const totalTonnageResponse = await axiosInstance.get('/Statistics/overall/total-training-tonnage', {
        params: { TimeFrame: activeTab },
      });
      console.log("API Total Training Tonnage Response:", totalTonnageResponse.data);

      const tonnageData = [];
      for (let key in totalTonnageResponse.data) {
        if (totalTonnageResponse.data.hasOwnProperty(key)) {
          tonnageData.push({
            date: format(new Date(key), 'yyyy-MM-dd'), // Format the date
            tonnage: totalTonnageResponse.data[key],
          });
        }
      }
      setChartData(tonnageData);

      // Fetch and handle frequency data
      const frequencyResponse = await axiosInstance.get('/Statistics/overall/training-frequency', {
        params: { TimeFrame: activeTab },
      });
      console.log("API Training Frequency Response:", frequencyResponse.data);

      const frequencyData = [];
      for (let key in frequencyResponse.data) {
        if (frequencyResponse.data.hasOwnProperty(key)) {
          frequencyData.push({
            date: format(new Date(key), 'yyyy-MM-dd'), // Format the date
            workouts: frequencyResponse.data[key],
          });
        }
      }
      setFrequencyData(frequencyData);

    } catch (error) {
      console.error("Error fetching data:", error);
      setSummaryData({
        numberOfWorkouts: 0,
        hoursAtGym: 0,
        totalWeightLifted: 0,
        weekStreak: 0,
      });
      setMuscleEngagementData([]);
      setTotalRepsData([]);
      setChartData([]);
      setFrequencyData([]);
    }
  };

  useEffect(() => {
    updateDateRange(activeTab);
  }, [activeTab]);

  useEffect(() => {
    fetchData();
  }, [dateRange, activeTab]);

  const handlePreviousClick = () => {
    let newStart;
    switch (activeTab) {
      case 'Weekly':
        newStart = subWeeks(dateRange.start, 1);
        break;
      case 'Monthly':
        newStart = subMonths(dateRange.start, 1);
        break;
      case 'Yearly':
        newStart = subYears(dateRange.start, 1);
        break;
      default:
        newStart = subWeeks(dateRange.start, 1);
    }
    updateDateRange(activeTab, newStart);
  };

  const handleNextClick = () => {
    let newStart;
    switch (activeTab) {
      case 'Weekly':
        newStart = addWeeks(dateRange.start, 1);
        break;
      case 'Monthly':
        newStart = addMonths(dateRange.start, 1);
        break;
      case 'Yearly':
        newStart = addYears(dateRange.start, 1);
        break;
      default:
        newStart = addWeeks(dateRange.start, 1);
    }
    updateDateRange(activeTab, newStart);
  };

  return (
    <div className="dashboard">
      <div className="summary-dashboard">
        <h2>Summary Dashboard</h2>
        <div className="tabs">
          <button className={activeTab === 'Weekly' ? 'active' : ''} onClick={() => setActiveTab('Weekly')}>Weekly</button>
          <button className={activeTab === 'Monthly' ? 'active' : ''} onClick={() => setActiveTab('Monthly')}>Monthly</button>
          <button className={activeTab === 'Yearly' ? 'active' : ''} onClick={() => setActiveTab('Yearly')}>Yearly</button>
        </div>
        <div className="summary">
          <div className="summary-item">
            <span className="number">{summaryData.numberOfWorkouts}</span>
            <br />
            <span className="label">Number of Workouts</span>
          </div>
          <div className="summary-item">
            <span className="number">{summaryData.hoursAtTheGym.toFixed(1)}</span> {/* Rounded to 1 decimal place */}
            <br />
            <span className="label">Hours at the Gym</span>
          </div>
          <div className="summary-item">
            <span className="number">{summaryData.weightLifted} <span className="unit">kg</span></span>
            <br />
            <span className="label">Total Weight Lifted</span>
          </div>
          <div className="summary-item">
            <span className="number">{summaryData.weekStreak}</span>
            <br />
            <span className="label">Week Streak</span>
          </div>
        </div>

      </div>

      <div className="muscle-tracker">
        <div className="muscle-tracker-header">
          <h2>Muscle Engagement Tracker</h2>
          <div className="date-selector">
            <button onClick={handlePreviousClick}>&lt;</button>
            <span>{`${format(dateRange.start, 'MMM d')} - ${format(dateRange.end, 'MMM d')}`}</span>
            <button onClick={handleNextClick}>&gt;</button>
          </div>
        </div>
        <div className="muscle-tracker-content">
          <div className="muscle-image">
            <img src={MuscleGroupsExercises} alt="Muscle Engagement Tracker" />
          </div>
          <div className="muscle-sets">
            <table>
              <thead>
                <tr>
                  <th>Muscle</th>
                  <th>Sets</th>
                </tr>
              </thead>
              <tbody>
                {muscleEngagementData.length > 0 ? (
                  muscleEngagementData.map((muscle, index) => (
                    <tr key={index}>
                      <td><span className={`dot ${muscle.muscle.toLowerCase().replace(' ', '-')}`}></span>{muscle.muscle}</td>
                      <td>{muscle.sets} sets</td>
                    </tr>
                  ))
                ) : (
                  <tr>
                    <td colSpan="2">No data available</td>
                  </tr>
                )}
              </tbody>
            </table>
            <div className="sets-calculation">
              <button onClick={toggleModal}>How are sets calculated?</button>
            </div>
          </div>
        </div>
      </div>

      <div className="frequency">
        <div className="graph-title">Frequency</div>
        <ResponsiveContainer width="20%" height={300}>
          <BarChart data={frequencyData} margin={{ left: 50 }}>
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis dataKey="date" />
            <YAxis />
            <Tooltip />
            <Bar dataKey="workouts" fill="#ff7300" />
          </BarChart>
        </ResponsiveContainer>
      </div>

      <div className="graphs">
        <div className="graph">
          <div className="graph-title">Total Reps</div>
          <ResponsiveContainer width="100%" height={300}>
            <LineChart data={totalRepsData} margin={{ left: 50 }}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="date" />
              <YAxis />
              <Tooltip />
              <Line type="monotone" dataKey="reps" stroke="#ff7300" />
            </LineChart>
          </ResponsiveContainer>
        </div>
        <div className="graph">
          <div className="graph-title">Total Training Volume</div>
          <ResponsiveContainer width="100%" height={300}>
            <LineChart data={chartData} margin={{ left: 50 }}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="date" />
              <YAxis />
              <Tooltip />
              <Line type="monotone" dataKey="tonnage" stroke="#ff7300" />
            </LineChart>
          </ResponsiveContainer>
        </div>
      </div>

      {modalOpen && (
        <div className="modal">
          <div className="modal-content">
            <span className="close" onClick={toggleModal}>&times;</span>
            <p>The general set formula is expressed as n(A∪B) = n(A) + n(B) - n(A∩B), where A and B represent two sets. Here, n(A∪B) denotes the count of elements existing in either set A or B, while n(A∩B) indicates the count of elements shared by both sets A and B.</p>
          </div>
        </div>
      )}
    </div>
  );
};

export default WorkoutLogGraphs;
