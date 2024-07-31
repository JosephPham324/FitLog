import React, { useState } from 'react';
import './WorkoutLogGraphs.css';
import MuscleGroupsExercises from '../../assets/MuscleGroupsExercises.png';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from 'recharts';

const data = [
  { date: '5/20', reps: 1500, volume: 200000 },
  { date: '5/27', reps: 1300, volume: 180000 },
  { date: '6/3', reps: 1200, volume: 160000 },
  { date: '6/10', reps: 1100, volume: 150000 },
  { date: '6/17', reps: 1400, volume: 190000 },
  { date: '6/24', reps: 1000, volume: 140000 },
  { date: '7/1', reps: 900, volume: 130000 },
  { date: '7/8', reps: 1021, volume: 136181 },
];

const WorkoutLogGraphs = () => {
  const [activeTab, setActiveTab] = useState('Weekly');
  const [modalOpen, setModalOpen] = useState(false);

  const toggleModal = () => {
    setModalOpen(!modalOpen);
  };

  return (
    <div className="dashboard">
      <div className="summary-dashboard">
        <h2>Summary Dashboard</h2>
        <div className="tabs">
          <button className={activeTab === 'Daily' ? 'active' : ''} onClick={() => setActiveTab('Daily')}>Daily</button>
          <button className={activeTab === 'Weekly' ? 'active' : ''} onClick={() => setActiveTab('Weekly')}>Weekly</button>
          <button className={activeTab === 'Monthly' ? 'active' : ''} onClick={() => setActiveTab('Monthly')}>Monthly</button>
        </div>
        <div className="summary">
          <div className="summary-item">
            <span className="number">235</span>
            <br />
            <span className="label">Number of Workouts</span>
          </div>
          <div className="summary-item">
            <span className="number">313.87</span>
            <br />
            <span className="label">Hours at the Gym</span>
          </div>
          <div className="summary-item">
            <span className="number">2,538,598 <span className="unit">kg</span></span>
            <br />
            <span className="label">Total Weight Lifted</span>
          </div>
          <div className="summary-item">
            <span className="number">37</span>
            <br />
            <span className="label">Week Streak</span>
          </div>
        </div>
      </div>

      <div className="muscle-tracker">
        <div className="muscle-tracker-header">
          <h2>Muscle Engagement Tracker</h2>
          <div className="date-selector">
            <button>&lt;</button>
            <span>Jul 8 - Jul 14</span>
            <button>&gt;</button>
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
                <tr><td><span className="dot chest"></span>Chest</td><td>11.0 sets <span className="percent">-41%</span></td></tr>
                <tr><td><span className="dot upper-back"></span>Upper Back</td><td>10.8 sets <span className="percent">-26%</span></td></tr>
                <tr><td><span className="dot lats"></span>Lats</td><td>8.8 sets <span className="percent">-61%</span></td></tr>
                <tr><td><span className="dot triceps"></span>Triceps</td><td>7.4 sets <span className="percent">-60%</span></td></tr>
                <tr><td><span className="dot glutes"></span>Glutes</td><td>7.2 sets <span className="percent">-49%</span></td></tr>
                <tr><td><span className="dot quadriceps"></span>Quadriceps</td><td>7.0 sets <span className="percent">-43%</span></td></tr>
                <tr><td><span className="dot hamstrings"></span>Hamstrings</td><td>7.0 sets <span className="percent">-36%</span></td></tr>
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
          {/* Your chart component or library goes here */}
        </div>
      </div>
      <div className="graphs">
        <div className="graph">
          <div className="graph-title">Total Reps</div>
          <ResponsiveContainer width="100%" height={300}>
            <LineChart data={data}>
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
            <LineChart data={data}>
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
            <p>The general set formula is expressed as n(A?B) = n(A) + n(B) - n(A?B), where A and B represent two sets. Here, n(A?B) denotes the count of elements existing in either set A or B, while n(A?B) indicates the count of elements shared by both sets A and B.</p>
          </div>
        </div>
      )}
    </div>
  );
};

export default WorkoutLogGraphs;

