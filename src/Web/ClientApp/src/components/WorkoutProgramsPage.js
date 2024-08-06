import React, { useEffect, useState } from 'react';
import axiosInstance from '../utils/axiosInstance';
import { Link } from 'react-router-dom';
import './WorkoutProgramsPage.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';

const WorkoutProgramsPage = () => {
  const [programs, setPrograms] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [search, setSearch] = useState('');
  const [filters, setFilters] = useState({
    goal: '',
    experienceLevel: '',
    gymType: '',
    musclesPriority: ''
  });

  useEffect(() => {
    const fetchPrograms = async () => {
      try {
        const response = await axiosInstance.get('/WorkoutPrograms', {
          headers: {
            accept: 'application/json',
          },
        });
        console.log('API Response:', response.data);
        setPrograms(response.data);
      } catch (error) {
        setError('Error fetching workout programs');
        console.error('Error fetching workout programs:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchPrograms();
  }, []);

  useEffect(() => {
    const surveyData = JSON.parse(localStorage.getItem('trainingSurvey'));
    console.log('Survey Data from localStorage:', surveyData);

    if (surveyData) {
      setFilters({
        goal: surveyData.fitnessGoal,
        experienceLevel: surveyData.experience,
        gymType: surveyData.gymType,
        musclesPriority: surveyData.muscleGroups.join(', ')
      });
    }
  }, []);

  const filteredPrograms = programs.filter(program => {
    return (
      (program.programName.toLowerCase().includes(search.toLowerCase())) &&
      (filters.goal === '' || program.goal === filters.goal) &&
      (filters.experienceLevel === '' || program.experienceLevel === filters.experienceLevel) &&
      (filters.gymType === '' || program.gymType === filters.gymType) &&
      (filters.musclesPriority === '' || program.musclesPriority.split(', ').some(muscle => filters.musclesPriority.includes(muscle)))
    );
  }).sort((a, b) => {
    const priorityOrder = ['experienceLevel', 'gymType', 'fitnessGoal', 'musclesPriority', 'daysPerWeek'];
    const aPriority = priorityOrder.findIndex(item => a[item] === filters[item]);
    const bPriority = priorityOrder.findIndex(item => b[item] === filters[item]);
    return aPriority - bPriority;
  });

  console.log('Filtered Programs:', filteredPrograms);

  const handleSearchChange = (e) => {
    setSearch(e.target.value);
  };

  const handleFilterChange = (e) => {
    setFilters({
      ...filters,
      [e.target.name]: e.target.value
    });
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div className="container">
      <h1 className="title text-center"><strong>Workout Programs</strong></h1>
      <div className="search-filter-container">
        <div className="row mb-2 mt-3">
          <div className="col-md-12">
            <input
              type="text"
              className="form-control"
              placeholder="Search programs..."
              value={search}
              onChange={handleSearchChange}
            />
          </div>
        </div>

        <div className="row mb-2">
          <div className="col-md-3">
            <div className="title-filter"><strong><p>Goal</p></strong></div>
            <select name="goal" className="form-control" value={filters.goal} onChange={handleFilterChange}>
              <option value="">All Goals</option>
              <option value="Bodybuilding">Bodybuilding</option>
              <option value="Powerlifting">Powerlifting</option>
              <option value="Powerbuilding">Powerbuilding</option>
              <option value="Bodyweight Fitness">Bodyweight Fitness</option>
              {/* Add more goals as needed */}
            </select>
          </div>
          <div className="col-md-3">
            <div className="title-filter"><strong><p>Difficulty level</p></strong></div>
            <select name="experienceLevel" className="form-control" value={filters.experienceLevel} onChange={handleFilterChange}>
              <option value="">All Levels</option>
              <option value="Beginner">Beginner</option>
              <option value="Intermediate">Intermediate</option>
              <option value="Advanced">Advanced</option>
              <option value="Novice">Novice</option>
            </select>
          </div>
          <div className="col-md-3">
            <div className="title-filter"><strong><p>Equipments</p></strong></div>
            <select name="gymType" className="form-control" value={filters.gymType} onChange={handleFilterChange}>
              <option value="">All Gym Types</option>
              <option value="At home">At home</option>
              <option value="Full gym">Full gym</option>
              <option value="Garage gym">Garage gym</option>
              <option value="Dumbbell only">Dumbbell only</option>
              {/* Add more gym types as needed */}
            </select>
          </div>
          <div className="col-md-3">
            <div className="title-filter"><strong><p>Muscle priority</p></strong></div>
            <select name="musclesPriority" className="form-control" value={filters.musclesPriority} onChange={handleFilterChange}>
              <option value="">All Muscles Priority</option>
              <option value="Upper body">Upper body</option>
              <option value="Lower body">Lower body</option>
              <option value="Chest,Back,Arms">Chest, Back, Arms</option>
              {/* Add more muscle priorities as needed */}
            </select>
          </div>
        </div>
      </div>

      <div className="row">
        {filteredPrograms.map((program) => (
          <div className="col-md-4 mb-4" key={program.programId}>
            <Link to={`/program-details/${program.programId}`} className="program-card">
              <img src={program.programThumbnail} alt={program.programName} className="program-image img-fluid" />
              <div className="program-details">
                <h2 className="program-name">{program.programName}</h2>
                <p className="program-goal"><strong>Goal:</strong> {program.goal}</p>
                <p className="program-level"><strong>Experience Level:</strong> {program.experienceLevel}</p>
                <p className="program-type"><strong>Gym Type:</strong> {program.gymType}</p>
                <p className="program-priority"><strong>Muscles Priority:</strong> {program.musclesPriority}</p>
                <p className="program-group"><strong>Age Group:</strong> {program.ageGroup}</p>
                {/*       <p className="program-public"><strong>Public Program:</strong> {program.publicProgram ? 'Yes' : 'No'}</p>
                <p className="program-creator"><strong>Created by:</strong> {program.userName}</p>*/}
                <p className="program-creatorfullname"><strong>Coach name:</strong> {program.creatorFullName}</p>
              </div>
            </Link>
          </div>
        ))}
      </div>
    </div>
  );
};

export default WorkoutProgramsPage;

