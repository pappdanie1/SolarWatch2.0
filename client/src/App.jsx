import { useState } from 'react'
import { BrowserRouter, Routes, Route } from "react-router-dom";
import './App.css'
import Home from './Pages/Home';
import Login from './Pages/Login';
import Register from './Pages/Register';
import PageNotFound from './Pages/PageNotFound';
import Header from './Layout/Header';
import ProtectedRoute from './Components/ProtectedRoute'
import GetSunsetSunrise from './Pages/GetSunsetSunrise'
import City from './Pages/City';

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(localStorage.getItem('token') !==  null ? true : false);

  return (
    <BrowserRouter>
        <Header setIsLoggedIn={setIsLoggedIn} isLoggedIn={isLoggedIn}/>
        <Routes>
            <Route path="/solar-watch" element={<ProtectedRoute><Home /></ProtectedRoute>} />
            <Route path="/sunset-sunrise" element={<ProtectedRoute><GetSunsetSunrise /></ProtectedRoute>} />
            <Route path="/city" element={<ProtectedRoute><City /></ProtectedRoute>} />
            <Route path="/" element={<Login setIsLoggedIn={setIsLoggedIn}/>} />
            <Route path="register" element={<Register />} />
            <Route path="*" element={<PageNotFound />} />
        </Routes>
    </BrowserRouter>
);
}

export default App
