import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";


const Register = () => {
    const navigate = useNavigate();
    const [email, setEmail] = useState('')
    const [username, setUsername] = useState('')
    const [password, seetPassword] = useState('')

    const handleSubmit = async(e) => {
        e.preventDefault()
        try {
            const response = await fetch(`http://localhost:8080/Auth/Register`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify( { email, username, password } )
            })
            if(response.ok) {
                navigate("/")
            } else {
                const data = await response.json();
                let errorMessage = '';
                const firstErrorKey = Object.keys(data)[0];
                errorMessage = data[firstErrorKey][0];
                window.alert(errorMessage)
            }
        } catch(err) {
            console.error(err)
        }
    }

    return (
        <>
            <form onSubmit={handleSubmit}>
                <label htmlFor="email">Email: </label>
                <input type="text" id="email" name="email" placeholder="Write email here" onChange={(e) => setEmail(e.target.value)} />
                <label htmlFor="username">Username: </label>
                <input type="text" id="username" name="username" placeholder="Write username here" onChange={(e) => setUsername(e.target.value)} />
                <label htmlFor="password">Password: </label>
                <input type="password" id="password" name="password" onChange={(e) => seetPassword(e.target.value)} />
                <button type="submit" >Register</button>
            </form>
        </>
    )
}

export default Register