import React from 'react';
import DrinkCard from './DrinkCard';
import '../styles/DrinksPanel.css';

const DrinksPanel = ({ drinks, onPurchase }) => {

  return (
    <div className="lower__panel">
      {drinks.map((drink) => (
        <DrinkCard
          key={drink.id}
          id={drink.id}
          name={drink.name}
          imgSrc={drink.imgSrc}
          quantity={drink.quantity}
          price={drink.price}
          onPurchase={onPurchase}
        />
      ))}
    </div>
  )
}

export default DrinksPanel;