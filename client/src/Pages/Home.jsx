import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";

const Home = () => {


    return (
        <div>
            <div>
                <Link to={`/sunset-sunrise`}>
                    Get Sunset Sunrise Times
                </Link>
            </div>
                <Link to={`/city`}>
                    Get Cities
                </Link>
        </div>
    );
}

export default Home;