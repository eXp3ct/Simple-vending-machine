import { React, useContext } from 'react';
import BalancePanel from '../components/BalancePanel';
import DrinksPanel from '../components/DrinksPanel';
import useDrinks from '../api/useDrinks.js';
import { BalanceProvider } from '../contexts/BalanceContext.js';

const UserPanel = () => {
  const { drinks, loading, fetchDrinks } = useDrinks();

  if (loading) {
    return <div>Loading...</div>;
  }

  return (
    <div className="App">
      <BalanceProvider>
        <BalancePanel />
        <DrinksPanel drinks={drinks} onPurchase={fetchDrinks} />
      </BalanceProvider>
    </div>
  );
};

export default UserPanel;
