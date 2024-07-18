import React, { useContext, useState, useEffect } from 'react';
import { buyDrink, deleteDrink, editDrink, linkImageToDrink, uploadImage } from '../api/api';
import '../styles/DrinkCard.css';
import BalanceContext from '../contexts/BalanceContext';
import Modal from './Modal';

const DrinkCard = ({ id, name, imgSrc, quantity, price, onPurchase, admin }) => {
  const { balance, setBalance, addPurchase, calculateChange } = useContext(BalanceContext);
  const isOutOfStock = quantity <= 0;
  const isAffordable = balance >= price;
  const [showModal, setShowModal] = useState(false);
  const [showImageModal, setShowImageModal] = useState(false);
  const [newDrink, setNewDrink] = useState({
    name: '',
    quantity: 0,
    price: 0,
    imageFile: undefined
  });
  const [selectedImage, setSelectedImage] = useState(null);

  const buy = async (id) => {
    if (isAffordable) {
      const drink = await buyDrink(id, 1);
      console.log(drink);
      onPurchase();
      addPurchase(name, price, 1);
      calculateChange(price); // рассчитываем сдачу после покупки
    }
  };

  const handleEditDrink = async () => {
    // Логика редактирования напитка
    const updated = await editDrink(id, newDrink);
    console.log(updated);
    onPurchase();
    setShowModal(false);
  };

  const handleDeleteDrink = async () => {
    // Логика удаления напитка
    const deleted = await deleteDrink(id);
    console.log(deleted);
    onPurchase();
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setNewDrink({ ...newDrink, [name]: value });
  }

  const handleImageInputChange = (e) => {
    setSelectedImage(e.target.files[0]);
  }

  const handleChangeImage = async () => {
    const response = await uploadImage(id, selectedImage);
    const drink = await linkImageToDrink(id, response.id);
    console.log(response);
    console.log(drink);
    onPurchase();
    setShowImageModal(false);
  }

  return (
    <div className={`drink ${isOutOfStock || !isAffordable ? 'drink--inactive' : ''}`}>
      <p className="drink__name">{name}</p>
      <img src={imgSrc} alt={name} className="drink__image" onClick={() => setShowImageModal(true)} />
      <p className="drink__quantity">Количество: {quantity}</p>
      {!admin ? (
        <button
          className="buy__drink"
          disabled={isOutOfStock || !isAffordable}
          onClick={() => buy(id)}
        >
          Купить за {price} рублей
        </button>
      ) : (
        <>
          <button
            className="buy__drink"
            disabled={isOutOfStock || !isAffordable}
            onClick={() => buy(id)}
          >
            Купить за {price} рублей
          </button>
          <button className="edit__drink" onClick={() => setShowModal(true)}>
            Редактировать
          </button>
          {showModal && (
            <Modal
              title={"Изменить напиток"}
              fields={[
                { label: 'Название', type: 'text', name: 'name', value: name },
                { label: 'Количество', type: 'number', name: 'quantity', value: quantity },
                { label: 'Цена', type: 'number', name: 'price', value: price },
              ]}
              handleCancel={() => setShowModal(false)}
              handleInputChange={handleInputChange}
              handleSubmit={handleEditDrink}
            />
          )}
          <button className="delete__drink" onClick={handleDeleteDrink}>
            Удалить
          </button>
        </>
      )}
      {admin && showImageModal && (
        <Modal
          title={"Изменить изображение"}
          fields={[
            { label: "Изображение", type: 'file' },
          ]}
          handleCancel={() => setShowImageModal(false)}
          handleInputChange={handleImageInputChange}
          handleSubmit={handleChangeImage}
        />
      )}
    </div>
  );
};

export default DrinkCard;
