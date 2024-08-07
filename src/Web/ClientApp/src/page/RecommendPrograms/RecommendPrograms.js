//import React, { useEffect, useState } from 'react';
//import './RecommendPrograms.css';
//import axiosInstance from '../../utils/axiosInstance';
//import { Link } from 'react-router-dom';

//const RecommendPrograms = () => {
//  const [programs, setPrograms] = useState({
//    Goal: [],
//    ExperienceLevel: [],
//    GymType: [],
//    MusclesPriority: []
//  });
//  const [currentIndex, setCurrentIndex] = useState({
//    Goal: 0,
//    ExperienceLevel: 0,
//    GymType: 0,
//    MusclesPriority: 0
//  });

//  const handlePrevClick = (category) => {
//    setCurrentIndex(prevState => ({
//      ...prevState,
//      [category]: Math.max(prevState[category] - 8, 0)
//    }));
//  };

//  const handleNextClick = (category) => {
//    setCurrentIndex(prevState => ({
//      ...prevState,
//      [category]: Math.min(prevState[category] + 8, programs[category].length - 1)
//    }));
//  };

//  useEffect(() => {
//    axiosInstance.get('https://localhost:44447/api/TrainingRecommendation/programs-recommendation/user')
//      .then(response => {
//        const data = response.data;
//        console.log('Fetched Data:', data);

//        const sortedPrograms = {
//          Goal: [],
//          ExperienceLevel: [],
//          GymType: [],
//          MusclesPriority: []
//        };

//        Object.values(data).forEach(programsArray => {
//          programsArray.forEach(program => {
//            if (program.goal && !sortedPrograms.Goal.some(p => p.programId === program.programId)) {
//              sortedPrograms.Goal.push(program);
//            }
//            if (program.experienceLevel && !sortedPrograms.ExperienceLevel.some(p => p.programId === program.programId)) {
//              sortedPrograms.ExperienceLevel.push(program);
//            }
//            if (program.gymType && !sortedPrograms.GymType.some(p => p.programId === program.programId)) {
//              sortedPrograms.GymType.push(program);
//            }
//            if (program.musclesPriority && !sortedPrograms.MusclesPriority.some(p => p.programId === program.programId)) {
//              sortedPrograms.MusclesPriority.push(program);
//            }
//          });
//        });

//        console.log('Sorted Programs:', sortedPrograms);
//        setPrograms(sortedPrograms);
//      })
//      .catch(error => {
//        console.error('Error fetching data:', error);
//      });
//  }, []);

//  const renderProgramCards = (category) => {
//    const startIndex = currentIndex[category];
//    const visiblePrograms = programs[category].slice(startIndex, startIndex + 8);
//    return visiblePrograms.map((program) => (
//      <div key={program.programId} className="program-card">
//        <Link to={`/program-details/${program.programId}`} className="program-card-link">
//          <img src={program.programThumbnail} alt={program.programName} />
//          <div className="program-details">
//            <p>{program.programName}</p>
//            <span><strong>Creator:</strong> {program.creatorName}</span>
//            <span><strong>Weeks:</strong> {program.numberOfWeeks}</span>
//            <span><strong>Days/Week:</strong> {program.daysPerWeek}</span>
//            <span><strong>Experience Level:</strong> {program.experienceLevel}</span>
//            <span><strong>Gym Type:</strong> {program.gymType}</span>
//            <span><strong>Muscles Priority:</strong> {program.musclesPriority}</span>
//          </div>
//        </Link>
//      </div>
//    ));
//  };

//  return (
//    <div id="programs-container">
//      {['Goal', 'ExperienceLevel', 'GymType', 'MusclesPriority'].map(category => (
//        <div key={category} className="category">
//          <h3>{category}</h3>
//          <button className="carousel-button prev" onClick={() => handlePrevClick(category)}>&lt;</button>
//          <button className="carousel-button next" onClick={() => handleNextClick(category)}>&gt;</button>
//          <div className="program-list">
//            {programs[category].length > 0 ? (
//              <div className="program-carousel">
//                {renderProgramCards(category)}
//              </div>
//            ) : (
//              <p className="no-programs">No programs available in this category.</p>
//            )}
//          </div>
//        </div>
//      ))}
//    </div>
//  );
//};

//export default RecommendPrograms;


import React, { useEffect, useState } from 'react';
import './RecommendPrograms.css';
import axiosInstance from '../../utils/axiosInstance';

const RecommendPrograms = () => {
  const [programs, setPrograms] = useState({});

  useEffect(() => {
    axiosInstance.get('https://localhost:44447/api/TrainingRecommendation/programs-recommendation/user')
      .then(response => {
        setPrograms(response.data);
      })
      .catch(error => {
        console.error('Error fetching data:', error);
      });
  }, []);

  // Helper function to reorder the categories
  const reorderCategories = (categories) => {
    const order = ['AllCriteria', 'DaysPerWeek'];
    const orderedCategories = order.concat(Object.keys(categories).filter(category => !order.includes(category)));
    return orderedCategories;
  };

  const orderedCategories = reorderCategories(programs);

  return (
    <div id="programs-container">
      {orderedCategories.map(category => (
        <div key={category} className="category">
          <h3>{category}</h3>
          <div className="program-list">
            {programs[category] && programs[category].length > 0 ? (
              programs[category].map(program => (
                <div key={program.programId} className="program">
                  <img src={program.programThumbnail} alt={program.programName} />
                  <p>{program.programName}</p>
                </div>
              ))
            ) : (
              <p>No programs available in this category.</p>
            )}
          </div>
        </div>
      ))}
    </div>
  );
};

export default RecommendPrograms;


