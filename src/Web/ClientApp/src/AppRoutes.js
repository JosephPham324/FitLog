import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import { MuscleGroup } from "./components/MuscleGroup";
import WorkoutOutLog from "./components/WorkoutOutLog";
import Login from './components/GoogleLogin';
import Register from './components/Register';
import TrainingSurvey from "./components/TrainingSurvey/TrainingSurvey";
import EquipmentsList from './components/EquipmentsList/EquipmentsList';
import GoogleOAuthProvider from './components/GoogleLogin';
import { WorkoutHistory } from './components/WorkoutHistory';
import ChatPage from "./page/ChatPage";
import UserListPage from "./page/TestAxios"; 
import Logout from "./components/Logout";
import CoachApplicationNotification from "./components/CoachApplicationNotification/CoachApplicationNotification"
import WorkoutTemplatesListAdmin from "./components/Workout Templates List Admin/WorkoutTemplatesListAdmin";
import WorkoutLogExport from "./components/WorkoutLogExport/WorkoutLogExport"
import WorkoutLogGraphs from "./components/WorkoutLogGraphs"
import ExerciseLogGraphs from "./components/ExerciseLogGraphs/ExerciseLogGraphs"
import RolesListScreen from "./components/RolesListScreen/RolesListScreen"


const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/counter',
    element: <Counter />
  },
  {
    path: '/fetch-data',
    element: <FetchData />
  },
  {
    path: '/MuscleGroup',
    element: <MuscleGroup />
  },
  {
    path: '/WorkoutOutLog',
    element: <WorkoutOutLog />
  },
  {
    path: '/WorkoutHistory',
    element: <WorkoutHistory />
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
    element: <TrainingSurvey />
  },
  {
    path: '/admin/management/equipments',
    element: <EquipmentsList />
  },
  {
    path: '/chat',
    element: <ChatPage/>
  },
  {
    path: '/users-list',
    element: <UserListPage />
  },
  {
    path: '/log-out',
    element: <Logout/>
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

];

export default AppRoutes;
