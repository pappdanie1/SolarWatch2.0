import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";


const Header = ({ setIsLoggedIn, isLoggedIn }) => {
    const navigate = useNavigate();

    const handleLogout = () => {
        localStorage.removeItem('token');
        setIsLoggedIn(false);
        navigate("/")
    };


return (
    <header>
        {isLoggedIn ? (
            <div>
                <h1>My App</h1>
                <button onClick={handleLogout}>Logout</button>
            </div>
        ) : (
            <div>
                <h1>My App</h1>
            </div>
        )}
    </header>
);
};

export default Header;