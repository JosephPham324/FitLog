import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import  ExerciseLog  from "./components/ExerciseLog";
import Login from './components/Login';
import Register from './components/Register';
import TrainingSurvey from "./components/TrainingSurvey/TrainingSurvey";
import EquipmentsList from './components/EquipmentsList/EquipmentsList';
import GoogleOAuthProvider from './components/GoogleLogin';

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
  }
];

export default AppRoutes;
