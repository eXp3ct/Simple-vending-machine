import React from 'react';
import '../styles/DrinksPanel.css';

const Modal = ({ title, fields, handleInputChange, handleSubmit, handleCancel }) => {
  return (
    <div className="modal">
      <div className="modal-content">
        <h3>{title}</h3>
        {fields.map((field, index) => (
          <label key={index}>
            {field.label}:
            {field.type === 'file' ? (
              <input
                type="file"
                name={field.name}
                onChange={handleInputChange}
                accept={field.accept}
              />
            ) : (
              <input
                type={field.type}
                name={field.name}
                value={field.value}
                onChange={handleInputChange}
              />
            )}
          </label>
        ))}
        <button onClick={handleSubmit}>Добавить</button>
        <button onClick={handleCancel}>Отмена</button>
      </div>
    </div>
  );
};

export default Modal;
