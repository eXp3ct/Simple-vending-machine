import { React, useState } from 'react';
import DrinkCard from './DrinkCard';
import '../styles/DrinksPanel.css';
import Modal from './Modal';
import { addDrink, linkImageToDrink, uploadImage } from '../api/api';

const AdminDrinksPanel = ({ drinks, onPurchase }) => {
  const [showModal, setShowModal] = useState(false);
  const [showJsonModal, setShowJsonModal] = useState(false);
  const [newDrink, setNewDrink] = useState(
    {
      name: '',
      quantity: 0,
      price: 0,
    });
  const [selectedImage, setSelectedImage] = useState(null);
  const [selectedJson, setSelectedJson] = useState(null);

  const handleInputChange = (e) => {
    if (e.target.type === 'file') {
      setSelectedImage(e.target.files[0]);
    }
    const { name, value } = e.target;
    setNewDrink({ ...newDrink, [name]: value });
  }

  const handleJsonInputChange = (e) => {
    setSelectedJson(e.target.files[0]);
  }

  const handleAddDrink = async () => {
    console.log(newDrink);
    const added = await addDrink(newDrink);
    console.log(added);
    const image = await uploadImage(added.id, selectedImage);
    console.log(image);
    const attach = await linkImageToDrink(added.id, image.id);
    console.log(attach);
    setShowModal(false);
    onPurchase();
  };

  const importDrinks = async () => {
    const fileReader = new FileReader();
    fileReader.onload = async (e) => {
      const jsonText = e.target.result;
      const drinks = JSON.parse(jsonText);
      console.log(drinks);
      for (const key in drinks) {
        console.log(drinks[key]);
        const drink = await addDrink(drinks[key]);

        console.log(drink);
      }
      setShowJsonModal(false);
      onPurchase();
    }
    fileReader.readAsText(selectedJson);

  }

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
          admin
          onPurchase={onPurchase}
        />
      ))}
      <button className="add__drink" onClick={() => setShowModal(true)}>
        +
      </button>
      <button className="add__drink" onClick={() => setShowJsonModal(true)}>
        +json
      </button>
      {showModal && (
        <Modal
          title={"Добавить напиток"}
          fields={[
            { label: 'Название', type: 'text', name: 'name', value: newDrink.name },
            { label: 'Количество', type: 'number', name: 'quantity', value: newDrink.quantity },
            { label: 'Цена', type: 'number', name: 'price', value: newDrink.price },
            { label: 'Изображение', type: 'file', accept: 'image/jpeg' },
          ]}
          handleCancel={() => setShowModal(false)}
          handleInputChange={handleInputChange}
          handleSubmit={handleAddDrink}
        />
      )}
      {showJsonModal && (
        <Modal
          title={"Импорт напитков"}
          fields={[
            { label: 'Файл', type: 'file', accept: 'application/json' }
          ]}
          handleCancel={() => setShowJsonModal(false)}
          handleInputChange={handleJsonInputChange}
          handleSubmit={importDrinks}
        />
      )}
    </div>
  );
};

export default AdminDrinksPanel;
