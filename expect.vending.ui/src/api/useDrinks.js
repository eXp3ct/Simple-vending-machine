import { useState, useEffect } from 'react';
import {getDrinks, getImageById} from '../api/api';

const useDrinks = () => {
  const [drinks, setDrinks] = useState([]);
  const [loading, setLoading] = useState(true);

  const fetchDrinks = async () => {
    try {
      const drinksData = await getDrinks();
      const drinksWithImage = await Promise.all(drinksData.map(async drink => {
        let imgSrc = '';
        if (drink.imageId) {
          imgSrc = await getImageById(drink.imageId);
        }
        return { ...drink, imgSrc };
      }));
      setDrinks(drinksWithImage);
    } catch (error) {
      console.error('Error fetching drinks: ', error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchDrinks();
  }, []);

  return { drinks, loading, fetchDrinks };
};

export default useDrinks;