//import React, { useState } from 'react';
//import {
//  Container,
//  TextField,
//  Typography,
//  FormControl,
//  FormLabel,
//  RadioGroup,
//  FormControlLabel,
//  Radio,
//  Checkbox,
//  FormGroup,
//  Button,
//  MenuItem,
//  Select,
//  Grid,
//} from '@mui/material';
//import { makeStyles } from '@mui/styles';
//import axiosInstance from '../../utils/axiosInstance'; // Import the Axios instance

//const useStyles = makeStyles((theme) => ({
//  formControl: {
//    marginBottom: '16px',
//  },
//  selectControl: {
//    marginBottom: '16px',
//  },
//  textFieldControl: {
//    marginBottom: '16px',
//  },
//  submitButton: {
//    marginTop: '16px',
//    marginBottom: 40,
//  },
//  smallText: {
//    fontSize: '0.875rem',
//    color: 'red',
//  },
//  checkboxGroup: {
//    display: 'flex',
//    flexDirection: 'row',
//    flexWrap: 'wrap',
//    justifyContent: 'space-between',
//  },
//  radioGroupRow: {
//    display: 'flex',
//    flexDirection: 'row',
//    flexWrap: 'wrap',
//  },
//  radioColumn: {
//    display: 'flex',
//    flexDirection: 'column',
//    flex: 1,
//  },
//  redAsterisk: {
//    color: 'red',
//  },
//  boldText: {
//    fontWeight: 'bold',
//  },
//}));

//const TrainingSurvey = () => {
//  const classes = useStyles();
//  const [experience, setExperience] = useState('');
//  const [gymType, setGymType] = useState('');
//  const [fitnessGoal, setFitnessGoal] = useState('');
//  const [daysPerWeek, setDaysPerWeek] = useState('');
//  const [muscleGroups, setMuscleGroups] = useState([]);
//  const [age, setAge] = useState('');
//  const [errors, setErrors] = useState({
//    experience: false,
//    gymType: false,
//    fitnessGoal: false,
//    daysPerWeek: false,
//    muscleGroups: false,
//    age: false,
//    ageMessage: '',
//  });

//  const getCurrentDate = () => {
//    const today = new Date();
//    const yyyy = today.getFullYear();
//    const mm = String(today.getMonth() + 1).padStart(2, '0');
//    const dd = String(today.getDate()).padStart(2, '0');
//    return `${yyyy}-${mm}-${dd}`;
//  };

//  const handleSubmit = async (e) => {
//    e.preventDefault();

//    const birthDate = new Date(age);
//    const today = new Date();
//    const ageDiff = today.getFullYear() - birthDate.getFullYear();
//    const isOldEnough = ageDiff > 14 || (ageDiff === 14 && (today.getMonth() > birthDate.getMonth() || (today.getMonth() === birthDate.getMonth() && today.getDate() >= birthDate.getDate())));

//    const newErrors = {
//      experience: experience === '',
//      gymType: gymType === '',
//      fitnessGoal: fitnessGoal === '',
//      daysPerWeek: daysPerWeek === '',
//      muscleGroups: muscleGroups.length === 0,
//      age: age === '' || !isOldEnough,
//      ageMessage: age === '' ? 'This is a mandatory question.' : !isOldEnough ? 'You are not old enough to participate in the survey.' : '',
//    };
//    setErrors(newErrors);

//    if (!Object.values(newErrors).includes(true)) {
//      try {
//        const response = await axiosInstance.post('/TrainingSurvey/create', {
//          experience,
//          gymType,
//          fitnessGoal,
//          daysPerWeek,
//          muscleGroups,
//          age,
//        });
//        console.log(response.data);
//      } catch (error) {
//        console.error('There was an error submitting the form!', error);
//      }
//    }
//  };

//  return (
//    <Container>
//      <Grid container alignItems="left" justify="space-between">
//        <Typography variant="h4" className="Training Survey">
//          <span className="gradient-text">Training Survey</span>{' '}
//          <span className={classes.smallText}>
//            (You are required to fill out all of the questions).
//          </span>
//        </Typography>
//      </Grid>
//      <form onSubmit={handleSubmit}>
//        <FormControl component="fieldset" fullWidth className={classes.formControl}>
//          <FormLabel component="legend" className={classes.boldText}>
//            1. What is your experience level? <span className={classes.redAsterisk}>(*)</span>
//          </FormLabel>
//          <RadioGroup
//            value={experience}
//            onChange={(e) => setExperience(e.target.value)}
//            className={classes.radioGroupRow}
//          >
//            <div className={classes.radioColumn}>
//              <FormControlLabel value="beginner" control={<Radio />} label="Beginner" />
//              <FormControlLabel value="novice" control={<Radio />} label="Novice" />
//            </div>
//            <div className={classes.radioColumn}>
//              <FormControlLabel value="intermediate" control={<Radio />} label="Intermediate" />
//              <FormControlLabel value="advanced" control={<Radio />} label="Advanced" />
//            </div>
//          </RadioGroup>
//          {errors.experience && <Typography color="error">This is a mandatory question.</Typography>}
//        </FormControl>

//        <FormControl component="fieldset" fullWidth className={classes.formControl}>
//          <FormLabel component="legend" className={classes.boldText}>
//            2. What type of gym do you usually train in? <span className={classes.redAsterisk}>(*)</span>
//          </FormLabel>
//          <RadioGroup
//            value={gymType}
//            onChange={(e) => setGymType(e.target.value)}
//            error={errors.gymType}
//          >
//            <FormControlLabel value="home" control={<Radio />} label="Home gym (barbells and dumbbells)" />
//            <FormControlLabel value="calisthenics" control={<Radio />} label="Calisthenics" />
//            <FormControlLabel value="full" control={<Radio />} label="Full gym" />
//          </RadioGroup>
//          {errors.gymType && <Typography color="error">This is a mandatory question.</Typography>}
//        </FormControl>

//        <FormControl component="fieldset" fullWidth className={classes.formControl}>
//          <FormLabel component="legend" className={classes.boldText}>
//            3. What is your primary fitness goal? <span className={classes.redAsterisk}>(*)</span>
//          </FormLabel>
//          <RadioGroup
//            value={fitnessGoal}
//            onChange={(e) => setFitnessGoal(e.target.value)}
//            error={errors.fitnessGoal}
//          >
//            <FormControlLabel value="powerlifting" control={<Radio />} label="Powerlifting" />
//            <FormControlLabel value="hypertrophy" control={<Radio />} label="Hypertrophy" />
//            <FormControlLabel value="skills" control={<Radio />} label="Calisthenics Skills" />
//            <FormControlLabel value="olympic" control={<Radio />} label="Olympic Weightlifting" />
//            <FormControlLabel value="strongman" control={<Radio />} label="Strongman" />
//          </RadioGroup>
//          {errors.fitnessGoal && <Typography color="error">This is a mandatory question.</Typography>}
//        </FormControl>

//        <FormControl component="fieldset" fullWidth className={classes.formControl}>
//          <FormLabel component="legend" className={classes.boldText}>
//            4. Which muscle groups do you prioritize? <span className={classes.redAsterisk}>(*)</span>
//          </FormLabel>
//          <FormGroup className={classes.checkboxGroup}>
//            <FormControlLabel
//              control={
//                <Checkbox
//                  checked={muscleGroups.includes('Chest')}
//                  onChange={(e) => {
//                    const { value, checked } = e.target;
//                    setMuscleGroups((prev) =>
//                      checked ? [...prev, value] : prev.filter((group) => group !== value)
//                    );
//                  }}
//                  value="Chest"
//                />
//              }
//              label="Chest"
//            />
//            <FormControlLabel
//              control={
//                <Checkbox
//                  checked={muscleGroups.includes('Back')}
//                  onChange={(e) => {
//                    const { value, checked } = e.target;
//                    setMuscleGroups((prev) =>
//                      checked ? [...prev, value] : prev.filter((group) => group !== value)
//                    );
//                  }}
//                  value="Back"
//                />
//              }
//              label="Back"
//            />
//            <FormControlLabel
//              control={
//                <Checkbox
//                  checked={muscleGroups.includes('Legs')}
//                  onChange={(e) => {
//                    const { value, checked } = e.target;
//                    setMuscleGroups((prev) =>
//                      checked ? [...prev, value] : prev.filter((group) => group !== value)
//                    );
//                  }}
//                  value="Legs"
//                />
//              }
//              label="Legs"
//            />
//            <FormControlLabel
//              control={
//                <Checkbox
//                  checked={muscleGroups.includes('Arms')}
//                  onChange={(e) => {
//                    const { value, checked } = e.target;
//                    setMuscleGroups((prev) =>
//                      checked ? [...prev, value] : prev.filter((group) => group !== value)
//                    );
//                  }}
//                  value="Arms"
//                />
//              }
//              label="Arms"
//            />
//            <FormControlLabel
//              control={
//                <Checkbox
//                  checked={muscleGroups.includes('Shoulders')}
//                  onChange={(e) => {
//                    const { value, checked } = e.target;
//                    setMuscleGroups((prev) =>
//                      checked ? [...prev, value] : prev.filter((group) => group !== value)
//                    );
//                  }}
//                  value="Shoulders"
//                />
//              }
//              label="Shoulders"
//            />
//            <FormControlLabel
//              control={
//                <Checkbox
//                  checked={muscleGroups.includes('Core')}
//                  onChange={(e) => {
//                    const { value, checked } = e.target;
//                    setMuscleGroups((prev) =>
//                      checked ? [...prev, value] : prev.filter((group) => group !== value)
//                    );
//                  }}
//                  value="Core"
//                />
//              }
//              label="Core"
//            />
//          </FormGroup>
//          {errors.muscleGroups && <Typography color="error">This is a mandatory question.</Typography>}
//        </FormControl>

//        <FormControl component="fieldset" fullWidth className={classes.selectControl}>
//          <FormLabel component="legend" className={classes.boldText}>
//            5. How many days per week do you train? <span className={classes.redAsterisk}>(*)</span>
//          </FormLabel>
//          <Select
//            value={daysPerWeek}
//            onChange={(e) => setDaysPerWeek(e.target.value)}
//            error={errors.daysPerWeek}
//          >
//            {[...Array(7)].map((_, i) => (
//              <MenuItem key={i + 1} value={i + 1}>
//                {i + 1}
//              </MenuItem>
//            ))}
//          </Select>
//          {errors.daysPerWeek && <Typography color="error">This is a mandatory question.</Typography>}
//        </FormControl>

//        <FormControl component="fieldset" fullWidth className={classes.textFieldControl}>
//          <FormLabel component="legend" className={classes.boldText}>
//            6. What is your birthdate? <span className={classes.redAsterisk}>(*)</span>
//          </FormLabel>
//          <TextField
//            type="date"
//            value={age}
//            onChange={(e) => setAge(e.target.value)}
//            error={errors.age}
//            InputLabelProps={{
//              shrink: true,
//            }}
//            inputProps={{
//              max: getCurrentDate(), // Restrict to the current date
//            }}
//          />
//          {errors.age && (
//            <Typography color="error">
//              {errors.ageMessage}
//            </Typography>
//          )}
//        </FormControl>

//        <Button type="submit" variant="contained" color="primary" fullWidth className={classes.submitButton}>
//          Submit
//        </Button>
//      </form>
//    </Container>
//  );
//};

//export default TrainingSurvey;



import React, { useState } from 'react';
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
}));

const TrainingSurvey = () => {
  const classes = useStyles();
  const [experience, setExperience] = useState('');
  const [gymType, setGymType] = useState('');
  const [fitnessGoal, setFitnessGoal] = useState('');
  const [daysPerWeek, setDaysPerWeek] = useState('');
  const [muscleGroups, setMuscleGroups] = useState([]);
  const [age, setAge] = useState('');
  const [errors, setErrors] = useState({
    experience: false,
    gymType: false,
    fitnessGoal: false,
    daysPerWeek: false,
    muscleGroups: false,
    age: false,
    ageMessage: '',
  });

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
      experience: experience === '',
      gymType: gymType === '',
      fitnessGoal: fitnessGoal === '',
      daysPerWeek: daysPerWeek === '',
      muscleGroups: muscleGroups.length === 0,
      age: age === '' || !isOldEnough,
      ageMessage: age === '' ? 'This is a mandatory question.' : !isOldEnough ? 'You are not old enough to participate in the survey. You must 14 years old.' : '',
    };
    setErrors(newErrors);

    if (!Object.values(newErrors).includes(true)) {
      const dataToSend = {
        experience,
        gymType,
        fitnessGoal,
        daysPerWeek,
        muscleGroups,
        age,
      };
      console.log('Data to send:', dataToSend);
      try {
        const response = await axiosInstance.post('/TrainingSurvey/create', dataToSend);
        console.log(response.data);
      } catch (error) {
        console.error('There was an error submitting the form!', error);
      }
    }
  };

  return (
    <Container>
      <Grid container alignItems="left" justify="space-between">
        <Typography variant="h4" className="Training Survey">
          <span className="gradient-text">Training Survey</span>{' '}
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
            value={experience}
            onChange={(e) => setExperience(e.target.value)}
            className={classes.radioGroupRow}
          >
            <div className={classes.radioColumn}>
              <FormControlLabel value="beginner" control={<Radio />} label="Beginner" />
              <FormControlLabel value="novice" control={<Radio />} label="Novice" />
            </div>
            <div className={classes.radioColumn}>
              <FormControlLabel value="intermediate" control={<Radio />} label="Intermediate" />
              <FormControlLabel value="advanced" control={<Radio />} label="Advanced" />
            </div>
          </RadioGroup>
          {errors.experience && <Typography color="error">This is a mandatory question.</Typography>}
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
            <FormControlLabel value="home" control={<Radio />} label="Home gym (barbells and dumbbells)" />
            <FormControlLabel value="calisthenics" control={<Radio />} label="Calisthenics" />
            <FormControlLabel value="full" control={<Radio />} label="Full gym" />
          </RadioGroup>
          {errors.gymType && <Typography color="error">This is a mandatory question.</Typography>}
        </FormControl>

        <FormControl component="fieldset" fullWidth className={classes.formControl}>
          <FormLabel component="legend" className={classes.boldText}>
            3. What is your primary fitness goal? <span className={classes.redAsterisk}>(*)</span>
          </FormLabel>
          <RadioGroup
            value={fitnessGoal}
            onChange={(e) => setFitnessGoal(e.target.value)}
            error={errors.fitnessGoal}
          >
            <FormControlLabel value="powerlifting" control={<Radio />} label="Powerlifting" />
            <FormControlLabel value="hypertrophy" control={<Radio />} label="Hypertrophy" />
            <FormControlLabel value="skills" control={<Radio />} label="Calisthenics Skills" />
            <FormControlLabel value="olympic" control={<Radio />} label="Olympic Weightlifting" />
            <FormControlLabel value="strongman" control={<Radio />} label="Strongman" />
          </RadioGroup>
          {errors.fitnessGoal && <Typography color="error">This is a mandatory question.</Typography>}
        </FormControl>

        <FormControl component="fieldset" fullWidth className={classes.formControl}>
          <FormLabel component="legend" className={classes.boldText}>
            4. Which muscle groups do you prioritize? <span className={classes.redAsterisk}>(*)</span>
          </FormLabel>
          <FormGroup className={classes.checkboxGroup}>
            <FormControlLabel
              control={
                <Checkbox
                  checked={muscleGroups.includes('Chest')}
                  onChange={(e) => {
                    const { value, checked } = e.target;
                    setMuscleGroups((prev) =>
                      checked ? [...prev, value] : prev.filter((group) => group !== value)
                    );
                  }}
                  value="Chest"
                />
              }
              label="Chest"
            />
            <FormControlLabel
              control={
                <Checkbox
                  checked={muscleGroups.includes('Back')}
                  onChange={(e) => {
                    const { value, checked } = e.target;
                    setMuscleGroups((prev) =>
                      checked ? [...prev, value] : prev.filter((group) => group !== value)
                    );
                  }}
                  value="Back"
                />
              }
              label="Back"
            />
            <FormControlLabel
              control={
                <Checkbox
                  checked={muscleGroups.includes('Legs')}
                  onChange={(e) => {
                    const { value, checked } = e.target;
                    setMuscleGroups((prev) =>
                      checked ? [...prev, value] : prev.filter((group) => group !== value)
                    );
                  }}
                  value="Legs"
                />
              }
              label="Legs"
            />
            <FormControlLabel
              control={
                <Checkbox
                  checked={muscleGroups.includes('Arms')}
                  onChange={(e) => {
                    const { value, checked } = e.target;
                    setMuscleGroups((prev) =>
                      checked ? [...prev, value] : prev.filter((group) => group !== value)
                    );
                  }}
                  value="Arms"
                />
              }
              label="Arms"
            />
            <FormControlLabel
              control={
                <Checkbox
                  checked={muscleGroups.includes('Shoulders')}
                  onChange={(e) => {
                    const { value, checked } = e.target;
                    setMuscleGroups((prev) =>
                      checked ? [...prev, value] : prev.filter((group) => group !== value)
                    );
                  }}
                  value="Shoulders"
                />
              }
              label="Shoulders"
            />
            <FormControlLabel
              control={
                <Checkbox
                  checked={muscleGroups.includes('Core')}
                  onChange={(e) => {
                    const { value, checked } = e.target;
                    setMuscleGroups((prev) =>
                      checked ? [...prev, value] : prev.filter((group) => group !== value)
                    );
                  }}
                  value="Core"
                />
              }
              label="Core"
            />
          </FormGroup>
          {errors.muscleGroups && <Typography color="error">This is a mandatory question.</Typography>}
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
          Submit
        </Button>
      </form>
    </Container>
  );
};

export default TrainingSurvey;
