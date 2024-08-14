import React, { useState, useEffect } from 'react';
import {
  Container,
  TextField,
  Typography,
  FormControl,
  FormLabel,
  RadioGroup,
  FormControlLabel,
  Radio,
  Checkbox,
  FormGroup,
  Button,
  MenuItem,
  Select,
  Grid,
  Modal,
  Box
} from '@mui/material';
import { makeStyles } from '@mui/styles';
import axiosInstance from '../../utils/axiosInstance'; // Import the Axios instance

const useStyles = makeStyles((theme) => ({
  formControl: {
    marginBottom: '16px',
  },
  selectControl: {
    marginBottom: '16px',
  },
  textFieldControl: {
    marginBottom: '16px',
  },
  submitButton: {
    marginTop: '16px',
    marginBottom: 40,
  },
  smallText: {
    fontSize: '0.875rem',
    color: 'red',
  },
  checkboxGroup: {
    display: 'flex',
    flexDirection: 'row',
    flexWrap: 'wrap',
    justifyContent: 'space-between',
  },
  radioGroupRow: {
    display: 'flex',
    flexDirection: 'row',
    flexWrap: 'wrap',
  },
  radioColumn: {
    display: 'flex',
    flexDirection: 'column',
    flex: 1,
  },
  redAsterisk: {
    color: 'red',
  },
  boldText: {
    fontWeight: 'bold',
  },
  modalStyle: {
    position: 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    width: 400,
    backgroundColor: 'white',
    border: '2px solid #000',
    boxShadow: 24,
    padding: '16px',
  },
}));

const experienceLevels = [
  "Novice",
  "Beginner",
  "Intermediate",
  "Advanced"
];

const gymTypes = {
  "Home": "Contain minimal equipment like dumbbells, barbell, power rack, pull-up bar",
  "Full gym": "Most standard exercise selections are available",
  "Calisthenics": "Pull-up bar, dip bar, gymnastic rings"
};

const goals = [
  "Bodybuilding",
  "Powerlifting",
  "Powerbuilding",
  "Calisthenics skills",
  "Strongman",
  "Street lifting"
];

const TrainingSurvey = () => {
  const classes = useStyles();
  const [experienceLevel, setExperienceLevel] = useState('');
  const [gymType, setGymType] = useState('');
  const [goal, setGoal] = useState('');
  const [daysPerWeek, setDaysPerWeek] = useState('');
  const [musclesPriority, setMusclesPriority] = useState([]);
  const [muscleGroups, setMuscleGroups] = useState([]);
  const [age, setAge] = useState('');
  const [surveyId, setSurveyId] = useState(null);
  const [modalOpen, setModalOpen] = useState(false);
  const [errors, setErrors] = useState({
    experienceLevel: false,
    gymType: false,
    goal: false,
    daysPerWeek: false,
    musclesPriority: false,
    age: false,
    ageMessage: '',
  });

  useEffect(() => {
    // Fetch muscle groups
    const fetchMuscleGroups = async () => {
      try {
        const response = await axiosInstance.get('/MuscleGroups/get-list?PageNumber=1&PageSize=15');
        if (Array.isArray(response.data.items)) {
          setMuscleGroups(response.data.items);
        } else {
          console.error('Unexpected response format:', response.data);
        }
      } catch (error) {
        console.error('Error fetching muscle groups:', error);
      }
    };

    // Fetch existing survey data
    const fetchSurveyData = async () => {
      try {
        const response = await axiosInstance.get('/TrainingSurvey/user');
        if (response.data) {
          const data = response.data;
          setSurveyId(data.surveyAnswerId);
          setExperienceLevel(data.experienceLevel);
          setGymType(data.gymType);
          setGoal(data.goal);
          setDaysPerWeek(data.daysPerWeek.toString());
          setMusclesPriority(data.musclesPriority.split(','));
          setAge(data.age.toString());
        }
      } catch (error) {
        console.error('Error fetching survey data:', error);
      }
    };

    fetchMuscleGroups();
    fetchSurveyData();
  }, []);

  const getCurrentDate = () => {
    const today = new Date();
    const yyyy = today.getFullYear();
    const mm = String(today.getMonth() + 1).padStart(2, '0');
    const dd = String(today.getDate()).padStart(2, '0');
    return `${yyyy}-${mm}-${dd}`;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const birthDate = new Date(age);
    const today = new Date();
    const ageDiff = today.getFullYear() - birthDate.getFullYear();
    const isOldEnough = ageDiff > 14 || (ageDiff === 14 && (today.getMonth() > birthDate.getMonth() || (today.getMonth() === birthDate.getMonth() && today.getDate() >= birthDate.getDate())));

    const newErrors = {
      experienceLevel: experienceLevel === '',
      gymType: gymType === '',
      goal: goal === '',
      daysPerWeek: daysPerWeek === '',
      musclesPriority: musclesPriority.length === 0,
      age: age === '' || !isOldEnough,
      ageMessage: age === '' ? 'This is a mandatory question.' : !isOldEnough ? 'You are not old enough to participate in the survey.' : '',
    };
    setErrors(newErrors);

    if (!Object.values(newErrors).includes(true)) {
      const surveyData = {
        "surveyAnswerId": surveyId || 0,
        goal,
        daysPerWeek: parseInt(daysPerWeek),
        experienceLevel,
        gymType,
        musclesPriority: musclesPriority.join(','),
        age: parseInt(age),
      };

      try {
        let response;
        if (surveyId) {
          // Update existing survey
          response = await axiosInstance.put(`/TrainingSurvey/update/${surveyId}`, surveyData, {
            headers: {
              'Content-Type': 'application/json'
            }
          });
        } else {
          // Create new survey
          response = await axiosInstance.post('/TrainingSurvey/create', surveyData, {
            headers: {
              'Content-Type': 'application/json'
            }
          });
        }
        console.log(response.data);
        setModalOpen(true);
      } catch (error) {
        console.error('There was an error submitting the form!', error);
      }
    }
  };

  const handleClose = () => {
    setModalOpen(false);
    window.location.href = '/recommend-programs';
  };
  const handleMusclePriorityChange = (e) => {
    const { value, checked } = e.target;
    setMusclesPriority((prev) =>
      checked ? [...prev, value] : prev.filter((item) => item !== value)
    );
    
  };

  return (
    <Container>
      <Grid container alignItems="left" justify="space-between">
        <Typography variant="h4" className="Training Survey">
          <strong> Training Survey </strong>
          <span className={classes.smallText}>
            (You are required to fill out all of the questions).
          </span>
        </Typography>
      </Grid>
      <form onSubmit={handleSubmit}>
        <FormControl component="fieldset" fullWidth className={classes.formControl}>
          <FormLabel component="legend" className={classes.boldText}>
            1. What is your experience level? <span className={classes.redAsterisk}>(*)</span>
          </FormLabel>
          <RadioGroup
            value={experienceLevel}
            onChange={(e) => setExperienceLevel(e.target.value)}
            className={classes.radioGroupRow}
          >
            {experienceLevels.map(level => (
              <FormControlLabel key={level} value={level} control={<Radio />} label={level} />
            ))}
          </RadioGroup>
          {errors.experienceLevel && <Typography color="error">This is a mandatory question.</Typography>}
        </FormControl>

        <FormControl component="fieldset" fullWidth className={classes.formControl}>
          <FormLabel component="legend" className={classes.boldText}>
            2. What type of gym do you usually train in? <span className={classes.redAsterisk}>(*)</span>
          </FormLabel>
          <RadioGroup
            value={gymType}
            onChange={(e) => setGymType(e.target.value)}
            error={errors.gymType}
          >
            {Object.keys(gymTypes).map(key => (
              <FormControlLabel key={key} value={key} control={<Radio />} label={key} />
            ))}
          </RadioGroup>
          {errors.gymType && <Typography color="error">This is a mandatory question.</Typography>}
        </FormControl>

        <FormControl component="fieldset" fullWidth className={classes.formControl}>
          <FormLabel component="legend" className={classes.boldText}>
            3. What is your primary fitness goal? <span className={classes.redAsterisk}>(*)</span>
          </FormLabel>
          <RadioGroup
            value={goal}
            onChange={(e) => setGoal(e.target.value)}
            error={errors.goal}
          >
            {goals.map(goal => (
              <FormControlLabel key={goal} value={goal} control={<Radio />} label={goal} />
            ))}
          </RadioGroup>
          {errors.goal && <Typography color="error">This is a mandatory question.</Typography>}
        </FormControl>

        <FormControl component="fieldset" fullWidth className={classes.formControl}>
          <FormLabel component="legend" className={classes.boldText}>
            4. Which muscle groups do you prioritize? <span className={classes.redAsterisk}>(*)</span>
          </FormLabel>
          <FormGroup className={classes.checkboxGroup}>
            {Array.isArray(muscleGroups) && muscleGroups.map(group => (
              <FormControlLabel
                key={group.muscleGroupId} // Use muscleGroupId as the key
                control={
                  <Checkbox
                    checked={musclesPriority.includes(group.muscleGroupName)}
                    onChange={handleMusclePriorityChange}
                    value={group.muscleGroupName}
                  />
                }
                label={group.muscleGroupName}
              />
            ))}
          </FormGroup>
          {errors.musclesPriority && <Typography color="error">This is a mandatory question.</Typography>}
        </FormControl>

        <FormControl component="fieldset" fullWidth className={classes.selectControl}>
          <FormLabel component="legend" className={classes.boldText}>
            5. How many days per week do you train? <span className={classes.redAsterisk}>(*)</span>
          </FormLabel>
          <Select
            value={daysPerWeek}
            onChange={(e) => setDaysPerWeek(e.target.value)}
            error={errors.daysPerWeek}
          >
            {[...Array(7)].map((_, i) => (
              <MenuItem key={i + 1} value={i + 1}>
                {i + 1}
              </MenuItem>
            ))}
          </Select>
          {errors.daysPerWeek && <Typography color="error">This is a mandatory question.</Typography>}
        </FormControl>

        <FormControl component="fieldset" fullWidth className={classes.textFieldControl}>
          <FormLabel component="legend" className={classes.boldText}>
            6. What is your birthdate? <span className={classes.redAsterisk}>(*)</span>
          </FormLabel>
          <TextField
            type="date"
            value={age}
            onChange={(e) => setAge(e.target.value)}
            error={errors.age}
            InputLabelProps={{
              shrink: true,
            }}
            inputProps={{
              max: getCurrentDate(), // Restrict to the current date
            }}
          />
          {errors.age && (
            <Typography color="error">
              {errors.ageMessage}
            </Typography>
          )}
        </FormControl>

        <Button type="submit" variant="contained" color="primary" fullWidth className={classes.submitButton}>
          {surveyId ? 'Update' : 'Submit'}
        </Button>
      </form>

      <Modal
        open={modalOpen}
        onClose={handleClose}
        aria-labelledby="success-modal-title"
        aria-describedby="success-modal-description"
      >
        <Box className={classes.modalStyle}>
          <Typography id="success-modal-title" variant="h6" component="h2">
            Success
          </Typography>
          <Typography id="success-modal-description" sx={{ mt: 2 }}>
            You have successfully filled out the Survey.
          </Typography>
          <Button onClick={handleClose} variant="contained" color="primary">
            OK
          </Button>
        </Box>
      </Modal>
    </Container>
  );
};

export default TrainingSurvey;