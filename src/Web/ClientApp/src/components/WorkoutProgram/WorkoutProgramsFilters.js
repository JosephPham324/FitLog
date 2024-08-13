import React from 'react';

const WorkoutProgramsFilters = ({ search, filters, handleSearchChange, handleFilterChange }) => {
  return (
    <div className="search-filter-container">
      <h1 className="title text-center"><strong>Workout Programs</strong></h1>
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
  );
};

export default WorkoutProgramsFilters;
