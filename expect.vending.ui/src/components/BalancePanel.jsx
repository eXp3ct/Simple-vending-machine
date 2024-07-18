import { React, useContext, useState, useEffect } from 'react';
import Coin from './Coin';
import '../styles/BalancePanel.css';
import BalanceContext from '../contexts/BalanceContext';
import Modal from './Modal';

const BalancePanel = ({ isAdmin = false }) => {
  const { balance, setBalance, purchaseHistory, coinValues, setCoinValues, change } = useContext(BalanceContext);
  const [showModal, setShowModal] = useState(false);
  const [newCoin, setNewCoin] = useState({ value: 0 });

  useEffect(() => {
    const storedCoinValues = JSON.parse(localStorage.getItem('coinValues'));
    if (storedCoinValues) {
      setCoinValues(storedCoinValues);
    }
  }, [setCoinValues]);

  const addBalance = (value) => {
    setBalance(balance + parseInt(value, 10));
  }

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setNewCoin({ ...newCoin, [name]: value });
  }

  const addCoin = () => {
    const newCoinValues = [...coinValues, parseInt(newCoin.value, 10)];
    setCoinValues(newCoinValues);
    localStorage.setItem('coinValues', JSON.stringify(newCoinValues));
    setShowModal(false);
  }

  return (
    <div className="upper__panel">
      <div className="buttons">
        {coinValues.map(coin => (
          <Coin
            key={coin}
            value={coin}
            onClick={addBalance}
            isAdmin={isAdmin}
          />
        ))}
        {isAdmin && (
          <button className="add__coin" style={{ width: "30px", height: "30px" }} onClick={() => setShowModal(true)}>
            +
          </button>
        )}
        {showModal && (
          <Modal
            title={"Добавить монету"}
            fields={[
              { label: "Номинал монеты", type: 'number', name: 'value', value: newCoin.value }
            ]}
            handleCancel={() => setShowModal(false)}
            handleInputChange={handleInputChange}
            handleSubmit={addCoin}
          />
        )}
      </div>
      <div className="balance__container">
        <div className="balance">
          Внесено: {balance} рублей
        </div>
        {change !== null && (
          <div className="change">
            Сдача: {change} рублей
          </div>
        )}
      </div>
      <div className="purchase__history">
        <h3>Куплено:</h3>
        {purchaseHistory.map((purchase, index) => (
          <div key={index}>
            {purchase.name} ({purchase.price} руб) x {purchase.quantity}
          </div>
        ))}
      </div>
    </div>
  );
}

export default BalancePanel;