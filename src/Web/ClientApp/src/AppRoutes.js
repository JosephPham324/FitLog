import { Counter } from "./components/Counter";
import React from 'react';
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import { MuscleGroup } from "./components/MuscleGroup";
import WorkoutLog from "./components/WorkoutOutLog/WorkoutOutLog";
import CreateWorkoutLog from "./components/WorkoutOutLog/CreateWorkoutLog";
import Login from './components/GoogleLogin';
import Register from './components/Register';
import TrainingSurvey from "./components/TrainingSurvey/TrainingSurvey";
import EquipmentsList from './components/EquipmentsList/EquipmentsList';
import GoogleOAuthProvider from './components/GoogleLogin';
import { WorkoutHistory } from './components/WorkoutHistory';
import { ManageAccount } from './components/ManageAccount';
import { CoachServiceBooking } from './components/CoachServiceBooking';
import ChatPage from "./page/ChatPage";
import UserListPage from "./page/TestAxios";
import Logout from "./components/Logout";
import CoachApplicationNotification from "./components/CoachApplicationNotification/CoachApplicationNotification"
import WorkoutTemplatesListAdmin from "./components/Workout Templates List Admin/WorkoutTemplatesListAdmin";
import WorkoutLogExport from "./components/WorkoutLogExport/WorkoutLogExport";
import WorkoutLogGraphs from "./components/WorkoutLogGraphs/WorkoutLogGraphs";
import ExerciseLogGraphs from "./components/ExerciseLogGraphs/ExerciseLogGraphs";
import RolesListScreen from "./components/RolesListScreen/RolesListScreen";
import ExerciseListScreen from "./components/ExerciseListScreen/ExerciseListScreen";
import { Profile } from './page/Profile';
import { ChangePassword } from './page/ChangePassword';
import TrainingBoard from './page/TrainingBoard';
import withAuthProtection from './utils/withAuthProtection';
import RecoverAccount from './components/AccountRecovery/RecoverAccount'
import RecoverConfirmation from './components/AccountRecovery/RecoverConfirmation'
import RecoverInitiate from './components/AccountRecovery/RecoverInitiate'
import ConfirmEmail from './components/EmailConfirmation/ConfirmEmail'
import WorkoutProgramsPage from './components/WorkoutProgramsPage';
import WorkoutProgramsDetail from './components/WorkoutProgramsDetail';
import CoachProfile from './components/CoachProfile';
import WorkoutLogPage from './page/WorkoutLog/CreateWorkoutLog/WorkoutLog';
import CreateWorkoutTemplatePage from './page/WorkoutLog/CreateWorkoutTemplate/CreateWorkoutTemplate';
import CreateWorkoutLogFromTemplate from './page/WorkoutLog/CreateWorkoutLogFromTemplate/CreateWorkoutLogFromTemplate';
import UpdateWorkoutLogPage from './page/WorkoutLog/UpdateWorkoutLog/UpdateWorkoutLog';
import { Navigate } from 'react-router-dom';


const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/counter',
    element: React.createElement(withAuthProtection(Counter))
  },
  {
    path: '/coachprofile/:id',
    element: <CoachProfile /> // Adding CoachProfile to the routes
  },
  {
    path: '/fetch-data',
    element: React.createElement(withAuthProtection(FetchData))
  },
  {
    path: '/muscle-groups',
    element: React.createElement(withAuthProtection(MuscleGroup))
  },
  {
    path: '/workout-history',
    element: React.createElement(withAuthProtection(WorkoutHistory))
  },
  {
    path: '/coach-service-booking',
    element: <CoachServiceBooking />
  },
  {
    path: '/manage-account',
    element: <ManageAccount />
  },
  {
    path: '/workout-programs',
    element: <WorkoutProgramsPage />
  },
  {
    path: '/program-details/:id',
    element: <WorkoutProgramsDetail />
  },
  {
    path: '/login',
    element: <Login />
  },
  {
    path: '/google-login',
    element: <GoogleOAuthProvider />
  },
  {
    path: '/register',
    element: <Register />
  },
  {
    path: '/trainingsurvey',
    element: <TrainingSurvey />
  },
  {
    path: '/equipmentslist',
    element: <EquipmentsList />
  },
  {
    path: '/survey',
    element: React.createElement(withAuthProtection(TrainingSurvey))
  },
  {
    path: '/admin/management/equipments',
    element: React.createElement(withAuthProtection(EquipmentsList))
  },
  {
    path: '/chat',
    element: React.createElement(withAuthProtection(ChatPage))
  },
  {
    path: '/users-list',
    element: React.createElement(withAuthProtection(UserListPage))
  },
  {
    path: '/log-out',
    element: <Logout />
  },
  {
    path: '/coach-application-notification',
    element: <CoachApplicationNotification />
  },
  {
    path: '/workout-templates-admin',
    element: React.createElement(withAuthProtection(WorkoutTemplatesListAdmin))
  },
  {
    path: '/workout-log-export',
    element: React.createElement(withAuthProtection(WorkoutLogExport))
  },
  {
    path: '/workout-log-graphs',
    element: React.createElement(withAuthProtection(WorkoutLogGraphs))
  },
  {
    path: '/exercise-log-graphs',
    element: React.createElement(withAuthProtection(ExerciseLogGraphs))
  },
  {
    path: '/roles-list-screen',
    element: React.createElement(withAuthProtection(RolesListScreen))
  },
  {
    path: '/exercise-list-screen',
    element: <ExerciseListScreen />
  },
  {
    path: '/create-workout-log',
    element: React.createElement(withAuthProtection(CreateWorkoutLog))
  },
  {
    path: '/workout-log/create/:templateId',
    element: React.createElement(withAuthProtection(CreateWorkoutLogFromTemplate))
  },
  {
    path: '/profile/',
    element: React.createElement(withAuthProtection(Profile))
  },
  {
    path: '/changepassword',
    element: React.createElement(withAuthProtection(ChangePassword))
  },
  {
    path: '/training-board',
    element: React.createElement(withAuthProtection(TrainingBoard))
  },
  {
    path: '/recover-account',
    element: <RecoverAccount />
  },
  {
    path: '/recover-account/confirmation',
    element: <RecoverConfirmation />
  },
  {
    path: '/recover-account/initiate',
    element: <RecoverInitiate />
  },
  {
    path: '/confirm-email',
    element: <ConfirmEmail />
  },
  {
    path: '/workout-log/create',
    element: <WorkoutLogPage />
  },
  {
    path: '/workout-log/:workoutLogId/update',
    element: <UpdateWorkoutLogPage />
  },
  {
    path: '/workout-templates/create',
    element: React.createElement(withAuthProtection(CreateWorkoutTemplatePage))
  },
  // Wildcard route to catch all unmatched routes and redirect to home page
  {
    path: '*',
    element: <Navigate to="/" />
  }
];

export default AppRoutes;
