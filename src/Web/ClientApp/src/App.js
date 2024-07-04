import React, { Component } from 'react';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import { Layout } from './components/Layout';
import './custom.css';
import { Profile } from './page/Profile';
import { ChangePassword } from './page/ChangePassword';
import TrainingBoard from './page/TrainingBoard';


export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Layout>
        <Routes>
          {AppRoutes.map((route, index) => {
            const { element, ...rest } = route;
            return <Route key={index} {...rest} element={element} />;
          })}
          <Route path='/profile' element={<Profile />} />
          <Route path='/changepassword' element={<ChangePassword />} />
          <Route path='/trainingBoard' element={<TrainingBoard />} />
        </Routes>
      </Layout>
    );
  }
}



