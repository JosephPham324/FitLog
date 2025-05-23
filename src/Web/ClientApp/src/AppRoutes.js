import { Counter } from "./components/Counter";
import React from 'react';
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import { MuscleGroup } from "./components/MuscleGroup";
import RecommendPrograms from "./page/RecommendPrograms/RecommendPrograms";
import CreateWorkoutLog from "./components/WorkoutOutLog/CreateWorkoutLog";
import Login from './components/GoogleLogin';
import Register from './components/Register';
import TrainingSurvey from "./components/TrainingSurvey/TrainingSurvey";
import EquipmentsList from './components/EquipmentsList/EquipmentsList';
import GoogleOAuthProvider from './components/GoogleLogin';
import WorkoutHistory from './components/WorkoutHistory';
import { ManageAccount } from './components/ManageAccount/ManageAccount';
import { CoachServiceBooking } from './components/CoachServiceBooking';
import ChatPage from "./page/ChatPage";
import UserListPage from "./page/TestAxios";
import Logout from "./components/Logout";
import CoachApplicationNotification from "./components/CoachApplicationNotification/CoachApplicationNotification";
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
import WorkoutProgramsPage from './components/WorkoutProgram/WorkoutProgramsPage';
import WorkoutProgramsDetail from './components/WorkoutProgram/WorkoutProgramsDetail';
import CoachProfile from './components/CoachProfile';
import WorkoutLogPage from './page/WorkoutLog/CreateWorkoutLog/WorkoutLog';
import CreateWorkoutTemplatePage from './page/WorkoutLog/CreateWorkoutTemplate/CreateWorkoutTemplate';
import CreateWorkoutTemplateAdminPage from './page/WorkoutLog/CreateWorkoutTemplateAdmin/CreateWorkoutTemplate';
import CreateWorkoutLogFromTemplate from './page/WorkoutLog/CreateWorkoutLogFromTemplate/CreateWorkoutLogFromTemplate';
//import ProgramsManagementPage from "./components/ProgramsManagementPage";
import UpdateWorkoutLogPage from './page/WorkoutLog/UpdateWorkoutLog/UpdateWorkoutLog';
import WorkoutLogDetailsPage from './page/WorkoutLog/WorkoutLogDetails/WorkoutLogDetails';
import { Navigate } from 'react-router-dom';
import UpdateWorkoutTemplatePage from "./page/WorkoutLog/UpdateWorkoutTemplate/UpdateWorkoutTemplate";
import WorkoutTemplateDetailsPage from "./page/WorkoutLog/WorkoutTemplateDetails/WorkoutTemplateDetails";
import WorkoutTemplateListPage from "./page/WorkoutTemplates/WorkoutTemplatesList";
import PrivateWorkoutTemplateListPage from "./page/WorkoutTemplates/PrivateWorkoutTemplatesList";
import AdminTemplatesListPage from "./page/WorkoutTemplates/AdminWorkoutTemplatesList";
import LoggedExercises from "./components/LoggedExercises/LoggedExercises"
import ExerciseListPage from "./page/ExercisesManagement/ExercisesList";
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
    path: '/recommend-programs',
    element: <RecommendPrograms />
  },
  {
    path: '/counter',
    element: React.createElement(withAuthProtection(Counter, [Roles['A']]))
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
    path: '/admin/manage/muscle-groups',
    element: React.createElement(withAuthProtection(MuscleGroup, [Roles['A']]))
  },
  {
    path: '/workout-history',
    element: React.createElement(withAuthProtection(WorkoutHistory, [Roles['M']]))
  },
  {
    path: '/logged-exercises',
    element: <LoggedExercises />
  },
  {
    path: '/coach-service-booking',
    element: React.createElement(withAuthProtection(CoachServiceBooking, [Roles['M'], Roles['C']]))
  },
  {
    path: '/workout-programs',
    element: <WorkoutProgramsPage />
  },
  {
    path: '/program-details/:id',
    element: React.createElement(withAuthProtection(WorkoutProgramsDetail, []))
  },
  {
    path: '/workout-programs/:id',
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
    path: '/survey',
    element: React.createElement(withAuthProtection(TrainingSurvey, [Roles['M']]))
  },
  {
    path: '/admin/manage/equipments',
    element: React.createElement(withAuthProtection(EquipmentsList, [Roles['A']]))
  },
  {
    path: '/admin/manage/exercises',
    element: React.createElement(withAuthProtection(ExerciseListPage, [Roles['A']]))
  },
  {
    path: '/admin/users-list',
    element: React.createElement(withAuthProtection(UserListPage, [Roles['A']]))
  },
  {
    path: '/admin/manage/accounts',
    element: React.createElement(withAuthProtection(ManageAccount, [Roles['A']]))
  },
  {
    path: '/chat',
    element: React.createElement(withAuthProtection(ChatPage, []))
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
    path: '/statistics/exercises/:id',
    element: <ExerciseLogGraphs />
  },
  {
    path: '/statistics/exercises/',
    element: <LoggedExercises/>
  },
  {

    path: '/recommended-programs',
    element: React.createElement(withAuthProtection(RecommendPrograms, []))
  },
  {

    path: '/coach-application-notification',
    element: React.createElement(withAuthProtection(CoachApplicationNotification, []))
  },
  {
    path: '/workout-templates-admin',
    element: React.createElement(withAuthProtection(AdminTemplatesListPage, [Roles['A']]))
  },
  {
    path: '/workout-templates-admin/create',
    element: React.createElement(withAuthProtection(CreateWorkoutTemplateAdminPage, [Roles['A']]))
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
    path: '/exercise-list-screen',
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
    element: React.createElement(withAuthProtection(UpdateWorkoutTemplatePage, [Roles['M'], Roles['C'], Roles['A']]))
  },
  {
    path: '/workout-templates/:templateId/details',
    element: React.createElement(withAuthProtection(WorkoutTemplateDetailsPage, [Roles['M'], Roles['C'], Roles['A']]))
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
