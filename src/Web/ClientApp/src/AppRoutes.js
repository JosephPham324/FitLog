import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import  Login  from './components/Login';
import Register  from './components/Register';
import TrainingSurvey from "./components/TrainingSurvey";
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
    path: '/Login',
    element: <Login />
  },
  {
    path: '/Register',
    element: <Register />
  },
  {
    path: '/TrainingSurvey',
    element: <TrainingSurvey />
  },
  {
    path: '/fetch-data',
    element: <FetchData />
  },
  {
    path: '/EquipmentsList',
    element: <EquipmentsList />
  },
];

export default AppRoutes;
