import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";


const Login = ({ setIsLoggedIn }) => {
    const navigate = useNavigate();
    const [email, setEmail] = useState('')
    const [password, seetPassword] = useState('')

    const handleSubmit = async(e) => {
        e.preventDefault()
        try {
            const response = await fetch(`http://localhost:8080/Auth/Login`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify( { email, password } )
            })
            if(response.ok) {
                const data = await response.json();
                const token = data.token;
                localStorage.setItem('token', token);
                navigate("/solar-watch")
                setIsLoggedIn(true)
            } else {
                const data = await response.json();
                window.alert(data['Bad credentials'][0])
            }
        } catch(err) {
            console.error(err)
        }
    }

    const handleRegisterClick = () => {
        navigate("/register");
    };


    return (
        <>
            <form onSubmit={handleSubmit} >
                <label htmlFor="email">Email: </label>
                <input type="text" id="email" name="email" placeholder="Write email here" onChange={(e) => setEmail(e.target.value)} />
                <label htmlFor="password">Password: </label>
                <input type="password" id="password" name="password" onChange={(e) => seetPassword(e.target.value)} />
                <button type="submit" >Login</button>
            </form>
            <button onClick={handleRegisterClick} >Register</button>
        </>
    )
}

export default Login