import React, { useEffect, useState } from 'react';
import './RecommendPrograms.css';
import axiosInstance from '../../utils/axiosInstance';
import { Link } from 'react-router-dom';

const RecommendPrograms = () => {
  const [programs, setPrograms] = useState({
    DaysPerWeek: [],
    Goal: [],
    ExperienceLevel: [],
    GymType: [],
    AllCriteria: []
  });

  const [currentIndex, setCurrentIndex] = useState({
    DaysPerWeek: 0,
    Goal: 0,
    ExperienceLevel: 0,
    GymType: 0,
    AllCriteria: 0
  });

  const handlePrevClick = (category) => {
    setCurrentIndex(prevState => ({
      ...prevState,
      [category]: Math.max(prevState[category] - 8, 0)
    }));
  };

  const handleNextClick = (category) => {
    setCurrentIndex(prevState => {
      const newIndex = prevState[category] + 8;
      const maxIndex = programs[category].length;

      // Ensure newIndex does not exceed the total number of items
      if (newIndex >= maxIndex) {
        return {
          ...prevState,
          [category]: maxIndex - (maxIndex % 8 || 8) // Show the last remaining items
        };
      } else {
        return {
          ...prevState,
          [category]: newIndex
        };
      }
    });
  };

  useEffect(() => {
    axiosInstance.get('https://localhost:44447/api/TrainingRecommendation/programs-recommendation/user')
      .then(response => {
        const data = response.data;
        console.log('Fetched Data:', data);

        const sortedPrograms = {
          DaysPerWeek: data.DaysPerWeek || [],
          Goal: data.Goal || [],
          ExperienceLevel: data.ExperienceLevel || [],
          GymType: data.GymType || [],
          AllCriteria: data.AllCriteria || []
        };

        console.log('Sorted Programs:', sortedPrograms);
        setPrograms(sortedPrograms);
      })
      .catch(error => {
        console.error('Error fetching data:', error);
      });
  }, []);

  const renderProgramCards = (category) => {
    const startIndex = currentIndex[category];
    const visiblePrograms = programs[category].slice(startIndex, startIndex + 8); // Display 8 cards at a time

    return visiblePrograms.map((program) => (
      <div key={program.programId} className="program-card">
        <Link to={`/program-details/${program.programId}`} className="program-card-link">
          <img src={program.programThumbnail} alt={program.programName} />
          <div className="program-details">
            <p>{program.programName}</p>
            <span><strong>Creator:</strong> {program.creatorName}</span>
            <span><strong>Weeks:</strong> {program.numberOfWeeks}</span>
            <span><strong>Days/Week:</strong> {program.daysPerWeek}</span>
            <span><strong>Goal:</strong> {program.goal}</span>
            <span><strong>Experience Level:</strong> {program.experienceLevel}</span>
            <span><strong>Gym Type:</strong> {program.gymType}</span>
            <span><strong>Muscles Priority:</strong> {program.musclesPriority}</span>
          </div>
        </Link>
      </div>
    ));
  };

  return (
    <div id="programs-container">
      {['AllCriteria', 'DaysPerWeek', 'Goal', 'ExperienceLevel', 'GymType'].map(category => (
        <div key={category} className="category">
          <h3>{category}</h3>
          <button className="carousel-button prev" onClick={() => handlePrevClick(category)}>&lt;</button>
          <button className="carousel-button next" onClick={() => handleNextClick(category)}>&gt;</button>
          <div className="program-list">
            {programs[category].length > 0 ? (
              <div className="program-carousel grid-container">
                {renderProgramCards(category)}
              </div>
            ) : (
              <p className="no-programs">No programs available in this category.</p>
            )}
          </div>
        </div>
      ))}
    </div>
  );
};

export default RecommendPrograms;

