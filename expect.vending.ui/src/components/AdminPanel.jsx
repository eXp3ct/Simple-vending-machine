import { React, useContext } from 'react';
import AdminDrinksPanel from './AdminDrinksPanel';
import useDrinks from '../api/useDrinks.js';
import BalancePanel from './BalancePanel.jsx';
import { BalanceProvider } from '../contexts/BalanceContext.js';

const AdminPanel = () => {
  const { drinks, loading, fetchDrinks } = useDrinks();


  if (loading) {
    return <div>Loading...</div>;
  }

  return (
    <div className="App">
      <BalanceProvider>
        <BalancePanel isAdmin />
        <AdminDrinksPanel drinks={drinks} onPurchase={fetchDrinks} />
      </BalanceProvider>
    </div>
  );
};

export default AdminPanel;
