//import React, { useState, useEffect } from 'react';
//import axiosInstance from '../../utils/axiosInstance'; // Adjust the import path according to your project structure
//import './WorkoutLogGraphs.css';
//import MuscleGroupsExercises from '../../assets/MuscleGroupsExercises.png';
//import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, BarChart, Bar } from 'recharts';
//import { format, startOfWeek, endOfWeek, startOfMonth, endOfMonth, addWeeks, subWeeks, addMonths, subMonths, addYears, subYears } from 'date-fns';

//const WorkoutLogGraphs = () => {
//  const [activeTab, setActiveTab] = useState('Weekly');
//  const [modalOpen, setModalOpen] = useState(false);
//  const [summaryData, setSummaryData] = useState({
//    numberOfWorkouts: 0,
//    hoursAtGym: 0,
//    totalWeightLifted: 0,
//    weekStreak: 0,
//  });
//  const [chartData, setChartData] = useState([]);
//  const [muscleEngagementData, setMuscleEngagementData] = useState([]);
//  const [totalRepsData, setTotalRepsData] = useState([]);
//  const [frequencyData, setFrequencyData] = useState([]); // New state for frequency data
//  const [dateRange, setDateRange] = useState({ start: new Date(), end: new Date() });

//  const toggleModal = () => {
//    setModalOpen(!modalOpen);
//  };

//  const fetchSummaryData = async (timeFrame, start, end) => {
//    try {
//      const response = await axiosInstance.get(`https://localhost:44447/api/Statistics/overall/summary?TimeFrame=${timeFrame}&Start=${start.toISOString()}&End=${end.toISOString()}`);
//      const data = response.data;
//      setSummaryData({
//        numberOfWorkouts: data.numberOfWorkouts,
//        hoursAtGym: data.hoursAtGym,
//        totalWeightLifted: data.totalWeightLifted,
//        weekStreak: data.weekStreak,
//      });
//      setChartData(data.chartData); // Assuming the API returns the data needed for the charts
//    } catch (error) {
//      console.error('Error fetching summary data:', error);
//    }
//  };

//  const fetchMuscleEngagementData = async (timeFrame, start, end) => {
//    try {
//      const response = await axiosInstance.get(`https://localhost:44447/api/Statistics/overall/muscles-engagement?TimeFrame=${timeFrame}&Start=${start.toISOString()}&End=${end.toISOString()}`);
//      const data = response.data;
//      setMuscleEngagementData(data.muscleEngagement || []); // Adjust according to the actual API response structure
//    } catch (error) {
//      console.error('Error fetching muscle engagement data:', error);
//    }
//  };

//  const fetchTotalRepsData = async (timeFrame, start, end) => {
//    try {
//      const response = await axiosInstance.get(`https://localhost:44447/api/Statistics/overall/total-training-reps?TimeFrame=${timeFrame}&Start=${start.toISOString()}&End=${end.toISOString()}`);
//      const data = response.data;
//      setTotalRepsData(data.totalReps || []); // Adjust according to the actual API response structure
//    } catch (error) {
//      console.error('Error fetching total reps data:', error);
//    }
//  };

//  const fetchFrequencyData = async (timeFrame, start, end) => {
//    try {
//      const response = await axiosInstance.get(`https://localhost:44447/api/Statistics/overall/frequency?TimeFrame=${timeFrame}&Start=${start.toISOString()}&End=${end.toISOString()}`);
//      const data = response.data;
//      setFrequencyData(data.frequency || []); // Adjust according to the actual API response structure
//    } catch (error) {
//      console.error('Error fetching frequency data:', error);
//    }
//  };

//  const updateDateRange = (timeFrame) => {
//    let start, end;
//    const today = new Date();

//    switch (timeFrame) {
//      case 'Weekly':
//        start = startOfWeek(dateRange.start, { weekStartsOn: 1 });
//        end = endOfWeek(dateRange.start, { weekStartsOn: 1 });
//        break;
//      case 'Monthly':
//        start = startOfMonth(dateRange.start);
//        end = endOfMonth(dateRange.start);
//        break;
//      case 'Yearly':
//        start = new Date(dateRange.start.getFullYear(), 0, 1);
//        end = new Date(dateRange.start.getFullYear(), 11, 31);
//        break;
//      default:
//        start = startOfWeek(dateRange.start, { weekStartsOn: 1 });
//        end = endOfWeek(dateRange.start, { weekStartsOn: 1 });
//    }

//    setDateRange({ start, end });
//    fetchSummaryData(timeFrame, start, end);
//    fetchMuscleEngagementData(timeFrame, start, end);
//    fetchTotalRepsData(timeFrame, start, end);
//    fetchFrequencyData(timeFrame, start, end); // Fetch frequency data as well
//  };

//  useEffect(() => {
//    updateDateRange(activeTab);
//  }, [activeTab, dateRange.start]);

//  const handlePreviousClick = () => {
//    let newStart;
//    switch (activeTab) {
//      case 'Weekly':
//        newStart = subWeeks(dateRange.start, 1);
//        break;
//      case 'Monthly':
//        newStart = subMonths(dateRange.start, 1);
//        break;
//      case 'Yearly':
//        newStart = subYears(dateRange.start, 1);
//        break;
//      default:
//        newStart = subWeeks(dateRange.start, 1);
//    }
//    setDateRange({ ...dateRange, start: newStart });
//  };

//  const handleNextClick = () => {
//    let newStart;
//    switch (activeTab) {
//      case 'Weekly':
//        newStart = addWeeks(dateRange.start, 1);
//        break;
//      case 'Monthly':
//        newStart = addMonths(dateRange.start, 1);
//        break;
//      case 'Yearly':
//        newStart = addYears(dateRange.start, 1);
//        break;
//      default:
//        newStart = addWeeks(dateRange.start, 1);
//    }
//    setDateRange({ ...dateRange, start: newStart });
//  };

//  return (
//    <div className="dashboard">
//      <div className="summary-dashboard">
//        <h2>Summary Dashboard</h2>
//        <div className="tabs">
//          <button className={activeTab === 'Weekly' ? 'active' : ''} onClick={() => setActiveTab('Weekly')}>Weekly</button>
//          <button className={activeTab === 'Monthly' ? 'active' : ''} onClick={() => setActiveTab('Monthly')}>Monthly</button>
//          <button className={activeTab === 'Yearly' ? 'active' : ''} onClick={() => setActiveTab('Yearly')}>Yearly</button>
//        </div>
//        <div className="summary">
//          <div className="summary-item">
//            <span className="number">{summaryData.numberOfWorkouts}</span>
//            <br />
//            <span className="label">Number of Workouts</span>
//          </div>
//          <div className="summary-item">
//            <span className="number">{summaryData.hoursAtGym}</span>
//            <br />
//            <span className="label">Hours at the Gym</span>
//          </div>
//          <div className="summary-item">
//            <span className="number">{summaryData.totalWeightLifted} <span className="unit">kg</span></span>
//            <br />
//            <span className="label">Total Weight Lifted</span>
//          </div>
//          <div className="summary-item">
//            <span className="number">{summaryData.weekStreak}</span>
//            <br />
//            <span className="label">Week Streak</span>
//          </div>
//        </div>
//      </div>

//      <div className="muscle-tracker">
//        <div className="muscle-tracker-header">
//          <h2>Muscle Engagement Tracker</h2>
//          <div className="date-selector">
//            <button onClick={handlePreviousClick}>&lt;</button>
//            <span>{`${format(dateRange.start, 'MMM d')} - ${format(dateRange.end, 'MMM d')}`}</span>
//            <button onClick={handleNextClick}>&gt;</button>
//          </div>
//        </div>
//        <div className="muscle-tracker-content">
//          <div className="muscle-image">
//            <img src={MuscleGroupsExercises} alt="Muscle Engagement Tracker" />
//          </div>
//          <div className="muscle-sets">
//            <table>
//              <thead>
//                <tr>
//                  <th>Muscle</th>
//                  <th>Sets</th>
//                </tr>
//              </thead>
//              <tbody>
//                {muscleEngagementData.length > 0 ? (
//                  muscleEngagementData.map((muscle, index) => (
//                    <tr key={index}>
//                      <td><span className={`dot ${muscle.name.toLowerCase().replace(' ', '-')}`}></span>{muscle.name}</td>
//                      <td>{muscle.sets} sets <span className="percent">{muscle.changePercentage}%</span></td>
//                    </tr>
//                  ))
//                ) : (
//                  <tr>
//                    <td colSpan="2">No data available</td>
//                  </tr>
//                )}
//              </tbody>
//            </table>
//            <div className="sets-calculation">
//              <button onClick={toggleModal}>How are sets calculated?</button>
//            </div>
//          </div>
//        </div>
//      </div>

//      <div className="frequency">
//        <div className="chart">
//          <h2>Frequency</h2>
//          <ResponsiveContainer width="100%" height={300}>
//            <BarChart data={frequencyData} margin={{ left: 50 }}>
//              <CartesianGrid strokeDasharray="3 3" />
//              <XAxis dataKey="date" />
//              <YAxis />
//              <Tooltip />
//              <Bar dataKey="workouts" fill="#82ca9d" />
//            </BarChart>
//          </ResponsiveContainer>
//        </div>
//      </div>

//      <div className="graphs">
//        <div className="graph">
//          <div className="graph-title">Total Reps</div>
//          <ResponsiveContainer width="100%" height={300}>
//            <LineChart data={totalRepsData} margin={{ left: 50 }}>
//              <CartesianGrid strokeDasharray="3 3" />
//              <XAxis dataKey="date" />
//              <YAxis />
//              <Tooltip />
//              <Line type="monotone" dataKey="reps" stroke="#ff7300" />
//            </LineChart>
//          </ResponsiveContainer>
//        </div>
//        <div className="graph">
//          <div className="graph-title">Total Training Volume</div>
//          <ResponsiveContainer width="100%" height={300}>
//            <LineChart data={chartData} margin={{ left: 50 }}>
//              <CartesianGrid strokeDasharray="3 3" />
//              <XAxis dataKey="date" />
//              <YAxis />
//              <Tooltip />
//              <Line type="monotone" dataKey="volume" stroke="#ff7300" />
//            </LineChart>
//          </ResponsiveContainer>
//        </div>
//      </div>

//      {modalOpen && (
//        <div className="modal">
//          <div className="modal-content">
//            <span className="close" onClick={toggleModal}>&times;</span>
//            <p>The general set formula is expressed as n(A?B) = n(A) + n(B) - n(A?B), where A and B represent two sets. Here, n(A?B) denotes the count of elements existing in either set A or B, while n(A?B) indicates the count of elements shared by both sets A and B.</p>
//          </div>
//        </div>
//      )}
//    </div>
//  );
//};

//export default WorkoutLogGraphs;



import React, { useState, useEffect } from 'react';
import axiosInstance from '../../utils/axiosInstance'; // Adjust the import path according to your project structure
import './WorkoutLogGraphs.css';
import MuscleGroupsExercises from '../../assets/MuscleGroupsExercises.png';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, BarChart, Bar } from 'recharts';
import { format, startOfWeek, endOfWeek, startOfMonth, endOfMonth, addWeeks, subWeeks, addMonths, subMonths, addYears, subYears } from 'date-fns';

const WorkoutLogGraphs = () => {
  const [activeTab, setActiveTab] = useState('Weekly');
  const [modalOpen, setModalOpen] = useState(false);
  const [summaryData, setSummaryData] = useState({
    numberOfWorkouts: 0,
    hoursAtGym: 0,
    totalWeightLifted: 0,
    weekStreak: 0,
  });
  const [chartData, setChartData] = useState([]);
  const [muscleEngagementData, setMuscleEngagementData] = useState([]);
  const [totalRepsData, setTotalRepsData] = useState([]);
  const [frequencyData, setFrequencyData] = useState([]); // New state for frequency data
  const [dateRange, setDateRange] = useState({ start: new Date(), end: new Date() });

  const toggleModal = () => {
    setModalOpen(!modalOpen);
  };

  const fetchSummaryData = async (timeFrame, start, end) => {
    try {
      const response = await axiosInstance.get(`https://localhost:44447/api/Statistics/overall/summary?TimeFrame=${timeFrame}&Start=${start.toISOString()}&End=${end.toISOString()}`);
      const data = response.data;
      setSummaryData({
        numberOfWorkouts: data.numberOfWorkouts,
        hoursAtGym: data.hoursAtGym,
        totalWeightLifted: data.totalWeightLifted,
        weekStreak: data.weekStreak,
      });
      setChartData(data.chartData); // Assuming the API returns the data needed for the charts
    } catch (error) {
      console.error('Error fetching summary data:', error);
    }
  };

  const fetchMuscleEngagementData = async (timeFrame, start, end) => {
    try {
      const response = await axiosInstance.get(`https://localhost:44447/api/Statistics/overall/muscles-engagement?TimeFrame=${timeFrame}&Start=${start.toISOString()}&End=${end.toISOString()}`);
      const data = response.data;
      setMuscleEngagementData(data.muscleEngagement || []); // Adjust according to the actual API response structure
    } catch (error) {
      console.error('Error fetching muscle engagement data:', error);
    }
  };

  const fetchTotalRepsData = async (timeFrame, start, end) => {
    try {
      const response = await axiosInstance.get(`https://localhost:44447/api/Statistics/overall/total-training-reps?TimeFrame=${timeFrame}&Start=${start.toISOString()}&End=${end.toISOString()}`);
      const data = response.data;
      setTotalRepsData(data.totalReps || []); // Adjust according to the actual API response structure
    } catch (error) {
      console.error('Error fetching total reps data:', error);
    }
  };

  const fetchFrequencyData = async (timeFrame, start, end) => {
    try {
      const response = await axiosInstance.get(`https://localhost:44447/api/Statistics/overall/frequency?TimeFrame=${timeFrame}&Start=${start.toISOString()}&End=${end.toISOString()}`);
      const data = response.data;
      setFrequencyData(data.frequency || []); // Adjust according to the actual API response structure
    } catch (error) {
      console.error('Error fetching frequency data:', error);
    }
  };

  const fetchTotalTrainingVolumeData = async (timeFrame, start, end) => {
    try {
      const response = await axiosInstance.get(`https://localhost:44447/api/Statistics/overall/total-training-tonnage?TimeFrame=${timeFrame}&Start=${start.toISOString()}&End=${end.toISOString()}`);
      const data = response.data;
      setChartData(data.totalTonnage || []); // Adjust according to the actual API response structure
    } catch (error) {
      console.error('Error fetching total training volume data:', error);
    }
  };

  const updateDateRange = (timeFrame) => {
    let start, end;
    const today = new Date();

    switch (timeFrame) {
      case 'Weekly':
        start = startOfWeek(dateRange.start, { weekStartsOn: 1 });
        end = endOfWeek(dateRange.start, { weekStartsOn: 1 });
        break;
      case 'Monthly':
        start = startOfMonth(dateRange.start);
        end = endOfMonth(dateRange.start);
        break;
      case 'Yearly':
        start = new Date(dateRange.start.getFullYear(), 0, 1);
        end = new Date(dateRange.start.getFullYear(), 11, 31);
        break;
      default:
        start = startOfWeek(dateRange.start, { weekStartsOn: 1 });
        end = endOfWeek(dateRange.start, { weekStartsOn: 1 });
    }

    setDateRange({ start, end });
    fetchSummaryData(timeFrame, start, end);
    fetchMuscleEngagementData(timeFrame, start, end);
    fetchTotalRepsData(timeFrame, start, end);
    fetchFrequencyData(timeFrame, start, end); // Fetch frequency data as well
    fetchTotalTrainingVolumeData(timeFrame, start, end); // Fetch total training volume data as well
  };

  useEffect(() => {
    updateDateRange(activeTab);
  }, [activeTab, dateRange.start]);

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
    setDateRange({ ...dateRange, start: newStart });
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
    setDateRange({ ...dateRange, start: newStart });
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
            <span className="number">{summaryData.hoursAtGym}</span>
            <br />
            <span className="label">Hours at the Gym</span>
          </div>
          <div className="summary-item">
            <span className="number">{summaryData.totalWeightLifted} <span className="unit">kg</span></span>
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
                      <td><span className={`dot ${muscle.name.toLowerCase().replace(' ', '-')}`}></span>{muscle.name}</td>
                      <td>{muscle.sets} sets <span className="percent">{muscle.changePercentage}%</span></td>
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
        <div className="chart">
          <h2>Frequency</h2>
          <ResponsiveContainer width="100%" height={300}>
            <BarChart data={frequencyData} margin={{ left: 50 }}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="date" />
              <YAxis />
              <Tooltip />
              <Bar dataKey="workouts" fill="#82ca9d" />
            </BarChart>
          </ResponsiveContainer>
        </div>
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
              <Line type="monotone" dataKey="volume" stroke="#ff7300" />
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
