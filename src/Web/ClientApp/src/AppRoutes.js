import { Counter } from "./components/Counter";
import React from 'react';
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import { MuscleGroup } from "./components/MuscleGroup";
import ExerciseLog from "./components/ExerciseLog";
import Login from './components/GoogleLogin';
import Register from './components/Register';
import TrainingSurvey from "./components/TrainingSurvey/TrainingSurvey";
import EquipmentsList from './components/EquipmentsList/EquipmentsList';
import GoogleOAuthProvider from './components/GoogleLogin';
import { WorkoutHistory } from './components/WorkoutHistory';
import ChatPage from "./page/ChatPage";
import UserListPage from "./page/TestAxios";
import Logout from "./components/Logout";
import { Profile } from './page/Profile';
import { ChangePassword } from './page/ChangePassword';
import TrainingBoard from './page/TrainingBoard';
import withAuthProtection from './utils/withAuthProtection';
import RecoverAccount from './components/AccountRecovery/RecoverAccount'
import RecoverConfirmation from './components/AccountRecovery/RecoverConfirmation'
import RecoverInitiate from './components/AccountRecovery/RecoverInitiate'
import ConfirmEmail from './components/EmailConfirmation/ConfirmEmail'

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
    path: '/fetch-data',
    element: React.createElement(withAuthProtection(FetchData))
  },
  {
    path: '/MuscleGroup',
    element: React.createElement(withAuthProtection(MuscleGroup))
  },
  {
    path: '/WorkoutHistory',
    element: React.createElement(withAuthProtection(WorkoutHistory))
  },
  {
    path: '/ExerciseLog',
    element: React.createElement(withAuthProtection(ExerciseLog))
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
    element: React.createElement(withAuthProtection(Logout))
  },
  {
    path: '/profile',
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
