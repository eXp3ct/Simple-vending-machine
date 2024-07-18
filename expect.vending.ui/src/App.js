import './App.css';
import { React, useState, createContext } from 'react';
import { BrowserRouter as Router, Route, Switch, Routes } from 'react-router-dom';
import UserPanel from './components/UserPanel.jsx';
import AdminPanel from './components/AdminPanel.jsx';
import { BalanceProvider } from './contexts/BalanceContext.js';

function App() {

  return (
    <Router>
      <BalanceProvider>
        <Routes>
          <Route path="/" element={<UserPanel />} />
          <Route path="/admin" element={<AdminPanel />} />
        </Routes>
      </BalanceProvider>
    </Router>
  );
}

export default App;
