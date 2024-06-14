import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import Login from './components/Login';
import Register from './components/Register';
import TrainingSurvey from "./components/TrainingSurvey/TrainingSurvey";
import EquipmentsList from './components/EquipmentsList/EquipmentsList';

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
    path: '/login',
    element: <Login />
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
