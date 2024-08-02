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
import { WorkoutProgramsPage } from './components/WorkoutProgramsPage';
import { WorkoutProgramsDetail } from './components/WorkoutProgramsDetail';// Correct the import statement
import CoachProfile from './components/CoachProfile';
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
    path: '/CoachProfile/:id',
    element: <CoachProfile /> // Adding CoachProfile to the routes
  },
  {
    path: '/fetch-data',
    element: React.createElement(withAuthProtection(FetchData))
  },
  {
    path: '/MuscleGroup',
    element: React.createElement(withAuthProtection(MuscleGroup))
  },
  {
    path: '/WorkoutLog',
    element: <WorkoutLog />
  },
  {
    path: '/WorkoutLog',
    element: React.createElement(withAuthProtection(WorkoutLog))
  },
  {
    path: '/WorkoutHistory',
    element: <WorkoutHistory />
  },
  {
    path: '/WorkoutHistory',
    element: React.createElement(withAuthProtection(WorkoutHistory))
  },
  {
    path: '/CoachServiceBooking',
    element: <CoachServiceBooking />
  },
  {
    path: '/ManageAccount',
    element: <ManageAccount />
  },
  {
    path: '/WorkoutProgramsPage',
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
    path: '/TrainingSurvey',
    element: <TrainingSurvey />
  },
  {
    path: '/EquipmentsList',
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
    path: '/CoachApplicationNotification',
    element: <CoachApplicationNotification />
  },
  {
    path: '/WorkoutTemplatesListAdmin',
    element: <WorkoutTemplatesListAdmin />
  },
  {
    path: '/WorkoutLogExport',
    element: <WorkoutLogExport />
  },
  {
    path: '/WorkoutLogGraphs',
    element: <WorkoutLogGraphs />
  },
  {
    path: '/ExerciseLogGraphs',
    element: <ExerciseLogGraphs />
  },
  {
    path: '/RolesListScreen',
    element: <RolesListScreen />
  },
  {
    path: '/ExerciseListScreen',
    element: <ExerciseListScreen />
  },
  {
    path: '/CreateWorkoutLog',
    element: <CreateWorkoutLog />
  },
  {
    path: 'log-out',
    element: React.createElement(withAuthProtection(Logout))
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
  }
];

export default AppRoutes;
