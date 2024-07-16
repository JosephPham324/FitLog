import { Counter } from "./components/Counter";
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
    path: '/WorkoutHistory',
    element: <WorkoutHistory />
  },
  {
    path: '/ExerciseLog',
    element: <ExerciseLog />
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
  }

];

export default AppRoutes;
