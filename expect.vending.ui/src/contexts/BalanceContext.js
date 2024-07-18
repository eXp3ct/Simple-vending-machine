import React, { createContext, useState, useEffect } from 'react';

const BalanceContext = createContext();

export const BalanceProvider = ({ children }) => {
  const [balance, setBalance] = useState(() => {
    const storedBalance = localStorage.getItem('balance');
    return storedBalance ? parseInt(storedBalance) : 0;
  });
  const [purchaseHistory, setPurchaseHistory] = useState([]);
  const [coinValues, setCoinValues] = useState([1, 2, 5, 10]);
  const [change, setChange] = useState(null);

  useEffect(() => {
    localStorage.setItem('balance', balance);
  }, [balance]);

  const addPurchase = (name, price, quantity) => {
    setPurchaseHistory(prev => {
      const existingPurchase = prev.find(purchase => purchase.name === name && purchase.price === price);

      if (existingPurchase) {
        return prev.map(purchase =>
          purchase.name === name && purchase.price === price
            ? { ...purchase, quantity: purchase.quantity + quantity }
            : purchase
        );
      } else {
        return [...prev, { name, price, quantity }];
      }
    });
  };

  const calculateChange = (cost) => {
    const changeAmount = balance - cost;
    setChange(changeAmount);
    setBalance(0); // сбрасываем баланс после покупки
    setTimeout(() => setChange(null), 5000); // скрываем сдачу через 5 секунд
  }

  return (
    <BalanceContext.Provider value={{ balance, setBalance, purchaseHistory, addPurchase, coinValues, setCoinValues, calculateChange, change }}>
      {children}
    </BalanceContext.Provider>
  );
};

export default BalanceContext;