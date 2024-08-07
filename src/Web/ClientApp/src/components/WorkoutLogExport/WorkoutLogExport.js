// src/components/WorkoutLogExport.js
import React, { useState } from 'react';
import axiosInstance from '../../utils/axiosInstance';
import {
  Container,
  FormGroup,
  Label,
  Input,
  Button,
  Alert,
} from 'reactstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import './WorkoutLogExport.css';

const WorkoutLogExport = () => {
  const [exportFormat, setExportFormat] = useState('csv');
  const [startDate, setStartDate] = useState('2024-08-01');
  const [endDate, setEndDate] = useState('2024-08-04');
  const [userId] = useState('7e2a7512-fb8b-4387-a3ff-2b9136a0b3f1'); // Assuming userId is constant for now
  const [responseMessage, setResponseMessage] = useState(null);

  const handleExport = async () => {
    try {
      const response = await axiosInstance.get(`/WorkoutLog/export`, {
        params: {
          UserId: userId,
          StartDate: startDate,
          EndDate: endDate,
        },
        responseType: exportFormat === 'csv' ? 'blob' : 'json', // Handle CSV as a blob for download
      });

      if (exportFormat === 'csv') {
        const blob = new Blob([response.data], { type: 'text/csv' });
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.style.display = 'none';
        a.href = url;
        a.download = 'workout-log.csv';
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
      } else if (exportFormat === 'pdf') {
        const blob = new Blob([response.data], { type: 'application/pdf' });
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.style.display = 'none';
        a.href = url;
        a.download = 'workout-log.pdf';
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
      } else if (exportFormat === 'excel') {
        const blob = new Blob([response.data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.style.display = 'none';
        a.href = url;
        a.download = 'workout-log.xlsx';
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
      } else {
        setResponseMessage(JSON.stringify(response.data, null, 2));
      }
    } catch (error) {
      setResponseMessage(`Error: ${error.response ? error.response.data : error.message}`);
    }
  };

  return (
    <Container className="workout-log-export">
      <h1>Workout Log Data Export</h1>
      <FormGroup>
        <Label for="startDate">Start Date</Label>
        <Input
          type="date"
          id="startDate"
          value={startDate}
          onChange={(e) => setStartDate(e.target.value)}
        />
      </FormGroup>
      <FormGroup>
        <Label for="endDate">End Date</Label>
        <Input
          type="date"
          id="endDate"
          value={endDate}
          onChange={(e) => setEndDate(e.target.value)}
        />
      </FormGroup>
      <FormGroup tag="fieldset">
        <legend>Export Format</legend>
        <FormGroup check>
          <Label check>
            <Input
              type="radio"
              value="csv"
              checked={exportFormat === 'csv'}
              onChange={() => setExportFormat('csv')}
            />
            CSV
          </Label>
        </FormGroup>
        <FormGroup check>
          <Label check>
            <Input
              type="radio"
              value="pdf"
              checked={exportFormat === 'pdf'}
              onChange={() => setExportFormat('pdf')}
            />
            PDF
          </Label>
        </FormGroup>
        <FormGroup check>
          <Label check>
            <Input
              type="radio"
              value="excel"
              checked={exportFormat === 'excel'}
              onChange={() => setExportFormat('excel')}
            />
            Excel
          </Label>
        </FormGroup>
      </FormGroup>
      <Button color="primary" onClick={handleExport}>Export Data</Button>
      {responseMessage && (
        <Alert color={responseMessage.startsWith('Error') ? 'danger' : 'success'} className="mt-3">
          <pre>{responseMessage}</pre>
        </Alert>
      )}
    </Container>
  );
};

export default WorkoutLogExport;
