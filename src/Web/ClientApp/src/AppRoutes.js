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

import WorkoutHistory from './components/WorkoutHistory';

import { ManageAccount } from './components/ManageAccount';
import { CoachServiceBooking } from './components/CoachServiceBooking';
import ChatPage from "./page/ChatPage";
import UserListPage from "./page/TestAxios";
import Logout from "./components/Logout";
import CoachApplicationNotification from "./components/CoachApplicationNotification/CoachApplicationNotification";
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
import RecoverAccount from './components/AccountRecovery/RecoverAccount';
import RecoverConfirmation from './components/AccountRecovery/RecoverConfirmation';
import RecoverInitiate from './components/AccountRecovery/RecoverInitiate';
import ConfirmEmail from './components/EmailConfirmation/ConfirmEmail';
import WorkoutProgramsPage from './components/WorkoutProgramsPage';
import WorkoutProgramsDetail from './components/WorkoutProgramsDetail';// Correct the import statement
import CoachProfile from './components/CoachProfile';
import WorkoutLogPage from './page/WorkoutLog/CreateWorkoutLog/WorkoutLog';
import CreateWorkoutTemplatePage from './page/WorkoutLog/CreateWorkoutTemplate/CreateWorkoutTemplate';
import CreateWorkoutLogFromTemplate from './page/WorkoutLog/CreateWorkoutLogFromTemplate/CreateWorkoutLogFromTemplate';
import ProgramsDisplay from './page/RecommendPrograms/RecommendPrograms'
import LoggedExercises from "./components/LoggedExercises/LoggedExercises";
import UpdateWorkoutLogPage from './page/WorkoutLog/UpdateWorkoutLog/UpdateWorkoutLog';
import WorkoutLogDetailsPage from './page/WorkoutLog/WorkoutLogDetails/WorkoutLogDetails';
import { Navigate } from 'react-router-dom';
import UpdateWorkoutTemplatePage from "./page/WorkoutLog/UpdateWorkoutTemplate/UpdateWorkoutTemplate";
import WorkoutTemplateDetailsPage from "./page/WorkoutLog/WorkoutTemplateDetails/WorkoutTemplateDetails";
import WorkoutTemplateListPage from "./page/WorkoutTemplates/WorkoutTemplatesList";
import PrivateWorkoutTemplateListPage from "./page/WorkoutTemplates/PrivateWorkoutTemplatesList";

const Roles = {
  A: "Administrator",
  M: "Member",
  C: "Coach"
}

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/counter',
    element: React.createElement(withAuthProtection(Counter, [Roles['A']]))
  },
  {
    path: '/recommended-programs',
    element: React.createElement(withAuthProtection(ProgramsDisplay))
  },
  {
    path: '/coachprofile/:id',
    element: React.createElement(withAuthProtection(CoachProfile, [])) // Adding CoachProfile to the routes
  },
  {
    path: '/fetch-data',
    element: React.createElement(withAuthProtection(FetchData, []))
  },
  {
    path: '/muscle-groups',
    element: React.createElement(withAuthProtection(MuscleGroup, [Roles['A']]))
  },
  {
    path: '/workout-history',
    element: React.createElement(withAuthProtection(WorkoutHistory, [Roles['M']]))
  },
  /*{*/
  //  path: '/workouthistory',
  //  element: <WorkoutHistory />
  //},
  //{
  //  path: '/workouthistory',
  //  element: React.createElement(withAuthProtection(WorkoutHistory))
  //},
  {
    path: '/logged-exercises',
    element: <LoggedExercises />
  },
  {
    path: '/coach-service-booking',
    element: React.createElement(withAuthProtection(CoachServiceBooking, [Roles['M'], Roles['C']]))
  },
  {
    path: '/manage-account',
    element: React.createElement(withAuthProtection(ManageAccount, [Roles['A']]))
  },
  {
    path: '/workout-programs',
    element: React.createElement(withAuthProtection(WorkoutProgramsPage, []))
  },
  {
    path: '/program-details/:id',
    element: React.createElement(withAuthProtection(WorkoutProgramsDetail, []))
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
    element: React.createElement(withAuthProtection(TrainingSurvey, [Roles['M']]))
  },
  {
    path: '/admin/management/equipments',
    element: React.createElement(withAuthProtection(EquipmentsList, [Roles['A']]))
  },
  {
    path: '/chat',
    element: React.createElement(withAuthProtection(ChatPage, []))
  },
  {
    path: '/users-list',
    element: React.createElement(withAuthProtection(UserListPage, [Roles['A']]))
  },
  {
    path: '/log-out',
    element: React.createElement(withAuthProtection(Logout, []))
  },
  {
    path: '/workout-log-export',
    element: <WorkoutLogExport />
  },
  {
    path: '/workout-log-graphs',
    element: <WorkoutLogGraphs />
  },
  {
    path: '/exercise-log-graphs',
    element: <ExerciseLogGraphs />
  },
  {
    path: '/coach-application-notification',
    element: React.createElement(withAuthProtection(CoachApplicationNotification, []))
  },
  {
    path: '/workout-templates-admin',
    element: React.createElement(withAuthProtection(WorkoutTemplatesListAdmin, [Roles['A']]))
  },
  {
    path: '/workout-log-export',
    element: React.createElement(withAuthProtection(WorkoutLogExport, []))
  },
  {
    path: '/workout-log-graphs',
    element: React.createElement(withAuthProtection(WorkoutLogGraphs, [Roles['M']]))
  },
  {
    path: '/exercise-log-graphs',
    element: React.createElement(withAuthProtection(ExerciseLogGraphs, [Roles['M']]))
  },
  {
    path: '/roles-list-screen',
    element: React.createElement(withAuthProtection(RolesListScreen, [Roles['A']]))
  },
  {
    path: '/exerciselistscreen',
    element: <ExerciseListScreen />
  },
  {
    path: '/create-workout-log',
    element: React.createElement(withAuthProtection(CreateWorkoutLog, [Roles['M']]))
  },
  {
    path: '/workout-log/create/:templateId',
    element: React.createElement(withAuthProtection(CreateWorkoutLogFromTemplate, [Roles['M']]))
  },
  {
    path: '/profile/',
    element: React.createElement(withAuthProtection(Profile, [Roles['M'], Roles['C']]))
  },
  {
    path: '/change-password',
    element: React.createElement(withAuthProtection(ChangePassword, []))
  },
  {
    path: '/training-board',
    element: React.createElement(withAuthProtection(TrainingBoard, []))
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
    element: React.createElement(withAuthProtection(WorkoutLogPage, [Roles['M']]))
  },
  {
    path: '/workout-log/:workoutLogId/update',
    element: React.createElement(withAuthProtection(UpdateWorkoutLogPage, [Roles['M'], Roles['A']]))
  },
  {
    path: '/workout-log/:workoutLogId/details',
    element: React.createElement(withAuthProtection(WorkoutLogDetailsPage, [Roles['M']]))
  },
  {
    path: '/workout-templates/create',
    element: React.createElement(withAuthProtection(CreateWorkoutTemplatePage, [Roles['M'], Roles['C']]))
  },
  {
    path: '/workout-templates/:templateId/update',
    element: React.createElement(withAuthProtection(UpdateWorkoutTemplatePage, [Roles['M'], Roles['C']]))
  },
  {
    path: '/workout-templates/:templateId/details',
    element: React.createElement(withAuthProtection(WorkoutTemplateDetailsPage, [Roles['M'], Roles['C']]))
  },
  {
    path: '/workout-templates/',
    element: React.createElement(withAuthProtection(WorkoutTemplateListPage, []))
  },
  {
    path: '/workout-templates/private',
    element: React.createElement(withAuthProtection(PrivateWorkoutTemplateListPage, [Roles['M'], Roles['C']]))
  },
  // Wildcard route to catch all unmatched routes and redirect to home page
  {
    path: '*',
    element: <Navigate to="/" />
  }
];


export default AppRoutes;


