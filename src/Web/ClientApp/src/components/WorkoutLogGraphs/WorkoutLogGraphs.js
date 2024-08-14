import axiosInstance from '../../utils/axiosInstance';
import React, { useState, useEffect } from 'react';
import './WorkoutLogGraphs.css';
import MuscleGroupsExercises from '../../assets/MuscleGroupsExercises.png';
import { Line } from 'react-chartjs-2';
import { Chart, registerables } from 'chart.js';
import 'chartjs-adapter-date-fns';
import { format, startOfWeek, endOfWeek, startOfMonth, endOfMonth, addWeeks, subWeeks, addMonths, subMonths, addYears, subYears } from 'date-fns';
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from 'recharts';
import { first } from 'lodash-es';

Chart.register(...registerables);

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
  const [muscleEngagementDataFull, setMuscleEngagementDataFull] = useState([]);
  const [totalRepsData, setTotalRepsData] = useState([]);
  const [frequencyData, setFrequencyData] = useState([]);
  const [dateRange, setDateRange] = useState({ start: new Date(), end: new Date() });
  function getFirstDayOfCurrentWeek() {
    const today = new Date();
    // Get the start of the week, assuming week starts on Monday
    const firstDayOfWeek = startOfWeek(today, { weekStartsOn: 1 });
    return firstDayOfWeek;
  }
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

  function normalizeAndCompareDates(date1, date2) {
    // Convert the date strings to Date objects
    const normalizedDate1 = new Date(date1);
    const normalizedDate2 = new Date(date2);
    console.log(normalizedDate1)
    console.log(normalizedDate2)

    // Normalize the dates to only compare the year, month, and day
    const normalizedDate1String = normalizedDate1.toISOString().split('T')[0];
    const normalizedDate2String = normalizedDate2.toISOString().split('T')[0];

    // Compare the normalized dates
    return normalizedDate1String === normalizedDate2String;
  }



  const fetchData = async () => {
    try {
      const summaryResponse = await axiosInstance.get('/Statistics/overall/summary', {
        params: { TimeFrame: activeTab },
      });
      const muscleEngagementResponse = await axiosInstance.get('/Statistics/overall/muscles-engagement', {
        params: { TimeFrame: activeTab },
      });
      const fetchedSummaryData = summaryResponse.data;

     
      const today = new Date();
      switch (activeTab) {
        case "Weekly": {
          console.log("weekly")
          const firstDay = getFirstDayOfCurrentWeek();
          const summaryKeys = Object.keys(fetchedSummaryData);
          const musclesKeys = Object.keys(muscleEngagementResponse.data);

          summaryKeys.forEach(key => {
            if (normalizeAndCompareDates(key, firstDay)) {
              const currentData = fetchedSummaryData[key];
              console.log(fetchedSummaryData[key])
              setSummaryData(currentData);
            }
          })
          console.log(summaryData)
          musclesKeys.forEach(key => {
            if (normalizeAndCompareDates(key, firstDay)) {
              const currentData = muscleEngagementResponse.data[key];
              console.log(currentData)
              setMuscleEngagementData(currentData);
            }
          })
        }
          break;
        case "Monthly": {
          const firstDayOfMonth = new Date(today.getFullYear(), today.getMonth(), 1);
          const summaryKeys = Object.keys(fetchedSummaryData);
          const musclesKeys = Object.keys(muscleEngagementResponse.data);

          summaryKeys.forEach(key => {
            if (normalizeAndCompareDates(key, firstDayOfMonth)) {
              const currentData = fetchedSummaryData[key];
              console.log(fetchedSummaryData[key])
              setSummaryData(currentData);
            }
          })
          console.log(summaryData)
          musclesKeys.forEach(key => {
            if (normalizeAndCompareDates(key, firstDayOfMonth)) {
              const currentData = muscleEngagementResponse.data[key];
              console.log(currentData)
              setMuscleEngagementData(currentData);
            }
          })
        }
          break;
        case "Yearly": { 
          const firstDayOfYear = new Date(today.getFullYear(), 0, 1);
          const summaryKeys = Object.keys(fetchedSummaryData);
          const musclesKeys = Object.keys(muscleEngagementResponse.data);

          summaryKeys.forEach(key => {
            if (normalizeAndCompareDates(key, firstDayOfYear)) {
              const currentData = fetchedSummaryData[key];
              console.log(fetchedSummaryData[key])
              setSummaryData(currentData);
            }
          })
          console.log(summaryData)
          musclesKeys.forEach(key => {
            if (normalizeAndCompareDates(key, firstDayOfYear)) {
              const currentData = muscleEngagementResponse.data[key];
              console.log(currentData)
              setMuscleEngagementData(currentData);
            }
          })
      }
          break;
        default: break;
      }

      const totalRepsResponse = await axiosInstance.get('/Statistics/overall/total-training-reps', {
        params: { TimeFrame: activeTab },
      });

      const repsData = Object.keys(totalRepsResponse.data).map(key => ({
        date: format(new Date(key), 'yyyy-MM-dd'),
        reps: totalRepsResponse.data[key],
      }));
      setTotalRepsData(repsData);

      const totalTonnageResponse = await axiosInstance.get('/Statistics/overall/total-training-tonnage', {
        params: { TimeFrame: activeTab },
      });

      const tonnageData = Object.keys(totalTonnageResponse.data).map(key => ({
        date: format(new Date(key), 'yyyy-MM-dd'),
        tonnage: totalTonnageResponse.data[key],
      }));
      setChartData(tonnageData);

      const frequencyResponse = await axiosInstance.get('/Statistics/overall/training-frequency', {
        params: { TimeFrame: activeTab },
      });

      const frequencyData = Object.keys(frequencyResponse.data).map(key => ({
        date: format(new Date(key), 'yyyy-MM-dd'),
        workouts: frequencyResponse.data[key],
      }));
      setFrequencyData(frequencyData);

    } catch (error) {
      console.error("Error fetching data:", error);
      setSummaryData({
        numberOfWorkouts: 0,
        hoursAtTheGym: 0,
        weightLifted: 0,
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

  const chartDataReps = {
    labels: totalRepsData.map(item => item.date),
    datasets: [
      {
        label: 'Weekly Reps',
        data: totalRepsData.map(item => item.reps),
        borderColor: 'green',
        backgroundColor: 'rgba(255, 255, 0, 0.2)',
      }
    ]
  };

  const chartDataTonnage = {
    labels: chartData.map(item => item.date),
    datasets: [
      {
        label: 'Weekly Training Tonnage',
        data: chartData.map(item => item.tonnage),
        borderColor: 'blue',
        backgroundColor: 'rgba(255, 165, 0, 0.2)',
      }
    ]
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
            <span className="number">{summaryData.hoursAtTheGym?.toFixed(1)}</span>
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
        <ResponsiveContainer width="100%" height={300}>
          <BarChart data={frequencyData} margin={{ left: 50 }}>
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis dataKey="date" tickFormatter={(tick) => format(new Date(tick), 'dd/MM/yyyy')} />
            <YAxis />
            <Tooltip />
            <Bar dataKey="workouts" fill="#ff7300" barSize={80} />
          </BarChart>
        </ResponsiveContainer>
      </div>

      <div className="graphs">
        <div className="graph">
          <div className="graph-title">Total Reps</div>
          <Line
            data={chartDataReps}
            options={{
              scales: {
                x: {
                  type: 'time',
                  time: {
                    unit: 'week',
                  },
                  ticks: {
                    callback: function (value, index, values) {
                      return format(new Date(value), 'dd/MM/yyyy');
                    }
                  }
                },
                y: {
                  beginAtZero: true,
                },
              },
              responsive: true,
              plugins: {
                tooltip: {
                  callbacks: {
                    label: function (context) {
                      return `${context.dataset.label}: ${context.raw}`;
                    },
                  },
                },
              },
            }}
          />
        </div>
        <div className="graph">
          <div className="graph-title">Total Training Volume</div>
          <Line
            data={chartDataTonnage}
            options={{
              scales: {
                x: {
                  type: 'time',
                  time: {
                    unit: 'week',
                  },
                  ticks: {
                    callback: function (value, index, values) {
                      return format(new Date(value), 'dd/MM/yyyy');
                    }
                  }
                },
                y: {
                  beginAtZero: true,
                },
              },
              responsive: true,
              plugins: {
                tooltip: {
                  callbacks: {
                    label: function (context) {
                      return `${context.dataset.label}: ${context.raw}`;
                    },
                  },
                },
              },
            }}
          />
        </div>
      </div>

      {modalOpen && (
        <div className="modal">
          <div className="modal-content">
            <span className="close" onClick={toggleModal}>&times;</span>
            <p> <strong>- Definition: </strong> A set is a predefined number of repetitions (e.g., 10 reps of squats).
              <br></br>
              <strong>- Calculation: </strong> Count each time a set is performed during a workout. For example, if you do 3 sets of 10 reps of squats, that would count as 3 sets.</p>
          </div>
        </div>
      )}
    </div>
  );
};

export default WorkoutLogGraphs;
