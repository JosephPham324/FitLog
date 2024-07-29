import React, { useState } from 'react';
import './WorkoutLogExport.css';

const WorkoutLogExport = () => {
  const [exportFormat, setExportFormat] = useState('csv');

  const handleExport = () => {
    // Logic to export the workout log data
    console.log(`Exporting data in ${exportFormat} format`);
  };

  return (
    <div className="workout-log-export">
      <h1>Workout Log Data Export</h1>
      <div className="export-options">
        <label>
          <input
            type="radio"
            value="pdf"
            checked={exportFormat === 'pdf'}
            onChange={() => setExportFormat('pdf')}
          />
          PDF
        </label>
        <label>
          <input
            type="radio"
            value="excel"
            checked={exportFormat === 'excel'}
            onChange={() => setExportFormat('excel')}
          />
          Excel
        </label>
      </div>
      <button className="export-button" onClick={handleExport}>Export Data</button>
    </div>
  );
};

export default WorkoutLogExport;
