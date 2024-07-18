import axios from "axios";

const API_URL = 'http://localhost:5051/api';


export const getDrinks = async () => {
    try{
        const response = await axios.get(`${API_URL}/drink`, {
            headers: {
                'Content-Type': 'application/json'
            }
        });
        return response.data;
    } catch(error){
        console.error('Error fetching drinks: ', error);
        throw error;
    }
};

export const getImageById = async (imageId) => {
    try{
        const response = await axios.get(`${API_URL}/image/${imageId}`, { responseType: 'blob' });
        return URL.createObjectURL(response.data);
    } catch(error){
        console.error('Error fetching image: ', error);
        throw error;
    }
};

export const buyDrink = async (drinkId, quantity) => {
    try{
        const response = await axios.post(`${API_URL}/drink/${drinkId}?quantity=${quantity}`);
        return response.data;
    } catch(error){
        console.error('Error buying drink: ', error);
        throw error;
    }
}

export const addDrink = async ({name, quantity, price}) => {
    try {
        const response = await axios.post(`${API_URL}/drink`, {
            name: name,
            quantity: quantity,
            price: price
        });
        return response.data;
    } catch(error) {
        console.error('Error posting drink: ', error);
        throw error;
    }
}

export const deleteDrink = async(id) => {
    try{
        const response = await axios.delete(`${API_URL}/drink/${id}`);
        return response.data;
    } catch(error){
        console.error("Error deleting drink: ", error);
        throw error;
    }
}

export const editDrink = async (id, {name, quantity, price}) => {
    try{
        const response = await axios.put(`${API_URL}/drink/${id}`, {
            name: name,
            quantity: quantity,
            price: price
        });
        return response.data;
    } catch(error){
        console.error("Error edit dirnk: ", error);
        throw error;
    }
}

export const uploadImage = async (id, imageFile) => {
    try{
        const formData = new FormData();
        formData.append('drinkId', id);
        formData.append('imageFile', imageFile);

        const response = await axios.post(`${API_URL}/image`, formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        });

        return response.data;
    } catch(error){
        console.error("Error uploading file, ", error);
        throw error;
    }
}

export const linkImageToDrink = async (drinkId, imageId) => {
    try{
        const response = await axios.patch(`${API_URL}/drink/${drinkId}?imageId=${imageId}`);
        return response.data;
    } catch(error){
        console.error("Error linking image: ", error);
        throw error;
    }
}