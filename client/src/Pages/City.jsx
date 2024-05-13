import { useNavigate } from "react-router-dom";
import React, { useState, useEffect } from "react";

const City = () => {
    const navigate = useNavigate();
    const [cities, setCities] = useState([]);
    const [isDelete, setIsDelete] = useState(false);
    const [isUpdate, setIsUpdate] = useState(false);
    const [isAdd, setIsAdd] = useState(false);
    const [formData, setFormData] = useState({
        name: '',
        longitude: 0,
        latitude: 0,
        state: '',
        country: ''
    });
    const [deleteCity, setDeleteCity] = useState();


    useEffect(() => {
        const fetchData = async () => {
            try {
                const token = localStorage.getItem('token');
                const response = await fetch('http://localhost:8080/SolarWatch/GetCities', {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                });
                const data = await response.json()
                setCities(data)
            } catch (error) {
                console.error(error)
            }
        }
        fetchData()
    }, [])

    const handlePost = async (e) => {
        console.log(formData);
        e.preventDefault()
        const token = localStorage.getItem('token');
        try {
            const response = await fetch(`http://localhost:8080/SolarWatch/AddCity`, {
                method: 'Post',
                headers: { 'Content-Type': 'application/json',
                        'Authorization': `Bearer ${token}` },
                body: JSON.stringify({ name: formData.name, longitude: formData.longitude, latitude: formData.latitude, state: formData.state, country: formData.country })
            })
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            setIsAdd(false);
            window.location.reload();
        } catch(err) {
            console.error(err)
        }
    }

    const handleRemove = async (e) => {
        e.preventDefault()
        const token = localStorage.getItem('token');
        try {
            const response = await fetch(`http://localhost:8080/SolarWatch/DeleteCity?name=${deleteCity}`, {
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
            const response = await fetch(`http://localhost:8080/SolarWatch/UpdateCity`, {
                method: 'PATCH',
                headers: { 'Content-Type': 'application/json',
                        'Authorization': `Bearer ${token}` },
                body: JSON.stringify({ name: formData.name, longitude: formData.longitude, latitude: formData.latitude, state: formData.state, country: formData.country })
            })
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            setIsUpdate(false);
            window.location.reload();
        } catch(err) {
            console.error(err)
        }
    }

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData({
          ...formData,
          [name]: value
        });
      };

    const goBack = () => {
        setIsDelete(false);
        setIsUpdate(false);
        setIsAdd(false);
    }

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
        <div className="cities">
            <div>
                <h2>All The cities available in the db</h2>
                {cities.map((city) => (
                    <div key={city.id}>
                        <h3>{city.name}</h3>
                        <p>longitude: {city.longitude}</p>
                        <p>latitude: {city.latitude}</p>
                    </div>
                ))}
            </div>
            <h1>Admin</h1>
            {isDelete ? (
                <>
                    <form  onSubmit={handleRemove}>
                        <label htmlFor="name">City name:</label>
                        <input type="text" id="name" name="name" placeholder="Write city here" onChange={(e) => setDeleteCity(e.target.value)} />
                        <button type="submit" >Delete</button>
                        <button onClick={goBack}>Cancel</button>
                    </form>
                </>
            ) :  isUpdate ? (
                <>
                    <form onSubmit={handleUpdate}>
                        <label htmlFor="name">City name:</label>
                        <input type="text" id="name" name="name" placeholder="write name here" value={formData.name} onChange={handleInputChange} />
                        <label htmlFor="longitude">longitude:</label>
                        <input type="number" id="longitude" name="longitude" placeholder="write longitude here" value={formData.longitude} onChange={handleInputChange} />
                        <label htmlFor="latitude">latitude:</label>
                        <input type="number" id="latitude" name="latitude" placeholder="write latitude here" value={formData.latitude} onChange={handleInputChange} />
                        <label htmlFor="state">state:</label>
                        <input type="text" id="state" name="state" placeholder="write state here" value={formData.state} onChange={handleInputChange} />
                        <label htmlFor="country">country:</label>
                        <input type="text" id="country" name="country" placeholder="write country here" value={formData.country} onChange={handleInputChange} />
                        <button type="submit">Update</button>
                        <button onClick={goBack}>Cancel</button>
                    </form>
                </>
            ) : isAdd ? (
                <>
                    <form onSubmit={handlePost}>
                        <label htmlFor="sunrise">Sunrise:</label>
                        <label htmlFor="name">name:</label>
                        <input type="text" id="name" name="name" placeholder="write name here" value={formData.name} onChange={handleInputChange} />
                        <label htmlFor="longitude">longitude:</label>
                        <input type="number" id="longitude" name="longitude" placeholder="write longitude here" value={formData.longitude} onChange={handleInputChange} />
                        <label htmlFor="latitude">latitude:</label>
                        <input type="number" id="latitude" name="latitude" placeholder="write latitude here" value={formData.latitude} onChange={handleInputChange} />
                        <label htmlFor="state">state:</label>
                        <input type="text" id="state" name="state" placeholder="write state here" value={formData.state} onChange={handleInputChange} />
                        <label htmlFor="country">country:</label>
                        <input type="text" id="country" name="country" placeholder="write country here" value={formData.country} onChange={handleInputChange} />
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
    )
}

export default City;