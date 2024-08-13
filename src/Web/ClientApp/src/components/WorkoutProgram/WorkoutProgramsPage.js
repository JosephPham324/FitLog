import React, { useState, useEffect } from 'react';
import WorkoutProgramsFilters from './WorkoutProgramsFilters';
import ProgramsList from './ProgramsList';
import './WorkoutProgramsPage.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';

const WorkoutProgramsPage = () => {
  const [search, setSearch] = useState('');
  const [filters, setFilters] = useState({
    goal: '',
    experienceLevel: '',
    gymType: '',
    musclesPriority: ''
  });

  useEffect(() => {
    const surveyData = JSON.parse(localStorage.getItem('trainingSurvey'));
    if (surveyData) {
      setFilters({
        goal: surveyData.fitnessGoal,
        experienceLevel: surveyData.experience,
        gymType: surveyData.gymType,
        musclesPriority: surveyData.muscleGroups.join(', ')
      });
    }
  }, []);

  const handleSearchChange = (e) => {
    setSearch(e.target.value);
  };

  const handleFilterChange = (e) => {
    setFilters({
      ...filters,
      [e.target.name]: e.target.value
    });
  };

  return (
    <div className="container">
      <WorkoutProgramsFilters
        search={search}
        filters={filters}
        handleSearchChange={handleSearchChange}
        handleFilterChange={handleFilterChange}
      />
      <ProgramsList filters={filters} search={search} maxItemsPerPage={8} />
    </div>
  );
};

export default WorkoutProgramsPage;
