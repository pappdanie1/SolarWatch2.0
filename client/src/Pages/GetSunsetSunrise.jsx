import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";

const Home = () => {
    const navigate = useNavigate();
    const [city, setCity] = useState();
    const [search, setSearch] = useState('');
    const [isDelete, setIsDelete] = useState(false);
    const [deleteCity, setDeleteCity] = useState();
    const [name, setName] = useState();
    const [sunset, setSunset] = useState();
    const [sunrise, setSunrise] = useState();
    const [isUpdate, setIsUpdate] = useState(false);
    const [isAdd, setIsAdd] = useState(false);
    const [formData, setFormData] = useState({
        sunrise: '',
        sunset: '',
        city: {
            name: '',
            longitude: 0,
            latitude: 0,
            state: '',
            country: ''
        }
    });


    const fetchData = async () => {
        const token = localStorage.getItem('token');
        const response = await fetch(`http://localhost:8080/SolarWatch/GetSunsetSunrise?city=${search}`, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
        const jsonData = await response.json();
        setCity(jsonData);
    };

    const handleRemove = async (e) => {
        e.preventDefault()
        const token = localStorage.getItem('token');
        try {
            const response = await fetch(`http://localhost:8080/SolarWatch/DeleteSunsetSunrise?name=${deleteCity}`, {
                method: 'DELETE',
                headers: { 'Content-Type': 'application/json',
                            'Authorization': `Bearer ${token}` }
                })
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            setIsDelete(false)
            window.location.reload();
        } catch(error) {
            console.error(error)
        }
    }

    const handleUpdate = async (e) => {
        e.preventDefault()
        const token = localStorage.getItem('token');
        try {
            const response = await fetch(`http://localhost:8080/SolarWatch/UpdateSunsetSunrise`, {
                method: 'PATCH',
                headers: { 'Content-Type': 'application/json',
                        'Authorization': `Bearer ${token}` },
                body: JSON.stringify({ sunset: sunset, sunrise: sunrise, city: { name: name } })
            })
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            setIsUpdate(false);
            //window.location.reload();
        } catch(err) {
            console.error(err)
        }
    }

    const handlePost = async (e) => {
        console.log(formData);
        e.preventDefault()
        const token = localStorage.getItem('token');
        try {
            const response = await fetch(`http://localhost:8080/SolarWatch/AddSunsetSunrise`, {
                method: 'Post',
                headers: { 'Content-Type': 'application/json',
                        'Authorization': `Bearer ${token}` },
                body: JSON.stringify({ sunrise: formData.sunrise, sunset: formData.sunset, city: { name: formData.city.name, longitude: formData.city.longitude, latitude: formData.city.latitude, state: formData.city.state, country: formData.city.country } })
            })
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            setIsAdd(false);
            //window.location.reload();
        } catch(err) {
            console.error(err)
        }
    }

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData({
          ...formData,
          [name]: value, // Update top-level properties directly
          city: {
            ...formData.city, // Spread operator to copy city object
            [name]: value, // Update nested city properties
          },
        });
      };

    const goBack = () => {
        setIsDelete(false);
        setIsUpdate(false);
        setIsAdd(false);
    }

    const handleSearch = () => {
        fetchData();
    };

    const handleSearchChange = (e) => {
        setSearch(e.target.value);
    };

    const handleDeleteButton = () => {
        setIsDelete(true);
    }

    const handleUpdateButton = () => {
        setIsUpdate(true);
    }

    const handleAddButton = () => {
        setIsAdd(true);
    }

    const handleBack = () => {
        navigate("/solar-watch")
    }

    return (
        <div>
            <h1>User</h1>
            <h2>Get sunset sunrise times for a given city</h2>
            <input
                type="text"
                placeholder="Enter city name"
                value={search}
                onChange={handleSearchChange}
            />
            <button onClick={handleSearch}>Get</button>
            {city ? (
                <div>
                    <h3>{city.city.name}</h3>
                    <p>sunrise: {city.sunrise}</p>
                    <p>sunset: {city.sunset}</p>
                </div>
            ) : (
                <></>
            )}
            <h1>Admin</h1>
            {isDelete ? (
                <>
                    <form  onSubmit={handleRemove}>
                        <label htmlFor="name">City name:</label>
                        <input type="text" id="name" name="name" placeholder="Write name here" onChange={(e) => setDeleteCity(e.target.value)} />
                        <button type="submit" >Delete</button>
                        <button onClick={goBack}>Cancel</button>
                    </form>
                </>
            ) :  isUpdate ? (
                <>
                    <form onSubmit={handleUpdate}>
                        <label htmlFor="name">City name:</label>
                        <input type="text" id="name" name="name" placeholder="Write name here" onChange={(e) => setName(e.target.value)} />
                        <label htmlFor="sunrise">Sunrise:</label>
                        <input type="text" id="sunrise" name="sunrise" placeholder="Sunrise time here" onChange={(e) => setSunrise(e.target.value)} />
                        <label htmlFor="sunset">Sunset:</label>
                        <input type="text" id="sunset" name="sunset" placeholder="Sunset time here" onChange={(e) => setSunset(e.target.value)} />
                        <button type="submit">Update</button>
                        <button onClick={goBack}>Cancel</button>
                    </form>
                </>
            ) : isAdd ? (
                <>
                    <form onSubmit={handlePost}>
                        <label htmlFor="sunrise">Sunrise:</label>
                        <input type="text" id="sunrise" name="sunrise" placeholder="Sunrise time here" value={formData.sunrise} onChange={handleInputChange} />
                        <label htmlFor="sunset">sunset:</label>
                        <input type="text" id="sunset" name="sunset" placeholder="sunset time here" value={formData.sunset} onChange={handleInputChange} />
                        <label htmlFor="name">name:</label>
                        <input type="text" id="name" name="name" placeholder="write name here" value={formData.city.name} onChange={handleInputChange} />
                        <label htmlFor="longitude">longitude:</label>
                        <input type="number" id="longitude" name="longitude" placeholder="write longitude here" value={formData.city.longitude} onChange={handleInputChange} />
                        <label htmlFor="latitude">latitude:</label>
                        <input type="number" id="latitude" name="latitude" placeholder="write latitude here" value={formData.city.latitude} onChange={handleInputChange} />
                        <label htmlFor="state">state:</label>
                        <input type="text" id="state" name="state" placeholder="write state here" value={formData.city.state} onChange={handleInputChange} />
                        <label htmlFor="country">country:</label>
                        <input type="text" id="country" name="country" placeholder="write country here" value={formData.city.country} onChange={handleInputChange} />
                        <button type="submit">Add</button>
                        <button onClick={goBack}>Cancel</button>
                    </form>
                </>
            ) : (
                <>
                <button onClick={handleDeleteButton}>Delete</button>
                <button onClick={handleUpdateButton}>Edit</button>
                <button onClick={handleAddButton}>Add</button>
                </>
            )}
            <div>
            <button onClick={handleBack} >Go back</button>
            </div>
        </div>
    );
}

export default Home;