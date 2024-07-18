import {React, useState, useContext, useEffect} from 'react';
import '../styles/Coin.css';
import BalanceContext from '../contexts/BalanceContext';

const Coin = ({ value, onClick, isAdmin }) => {
    const { coinValues, setCoinValues } = useContext(BalanceContext);
    const [isDisabled, setIsDisabled] = useState(() => {
        const storedDisabledCoins = JSON.parse(localStorage.getItem('disabledCoins')) || [];
        return storedDisabledCoins.includes(value);
    });

    useEffect(() => {
        const storedDisabledCoins = JSON.parse(localStorage.getItem('disabledCoins')) || [];
        if (isDisabled) {
            if (!storedDisabledCoins.includes(value)) {
                storedDisabledCoins.push(value);
            }
        } else {
            const index = storedDisabledCoins.indexOf(value);
            if (index !== -1) {
                storedDisabledCoins.splice(index, 1);
            }
        }
        localStorage.setItem('disabledCoins', JSON.stringify(storedDisabledCoins));
    }, [isDisabled, value]);

    const blockCoin = () => {
        setIsDisabled(!isDisabled);
    }

    const deleteCoin = () => {
        const newCoinValues = coinValues.filter(coin => coin !== value);
        setCoinValues(newCoinValues);
    }

    return (
        <div className="coin__container">
            <button className={`coin__button ${isDisabled ? 'inactive' : ''}`} onClick={() => onClick(value)} disabled={isDisabled}>
                {value}
            </button>
            {isAdmin && (
                <>
                    <button className="coin__delete" onClick={deleteCoin} />
                    <button className="coin__block" onClick={blockCoin} />
                </>
            )}
        </div>
    );
};

export default Coin;