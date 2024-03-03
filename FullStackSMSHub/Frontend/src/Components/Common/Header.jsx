import React from 'react';
import { Link } from 'react-router-dom';

const Header = () => {
    // Dynamically check if a user token exists to set the isAuthenticated state
    const isAuthenticated = !!localStorage.getItem('authToken'); // Example check

    return (
        <nav className="navbar navbar-expand-lg navbar-light bg-light">
            <Link to="/" className="navbar-brand">SMSHub</Link>
            <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                <span className="navbar-toggler-icon"></span>
            </button>
            <div className="collapse navbar-collapse" id="navbarNav">
                <ul className="navbar-nav mr-auto">
                    <li className="nav-item">
                        <Link className="nav-link" to="/Home">Home</Link>
                    </li>
                    <li className="nav-item">
                        <Link className="nav-link" to="/senderID">Sender IDs</Link>
                    </li>
                    <li className="nav-item">
                        <Link className="nav-link" to="/senderIDManagement">Sender ID Management</Link>
                    </li>
                    <li className="nav-item">
                        <Link className="nav-link" to="/messages">Send Message</Link>
                    </li>
                    <li className="nav-item">
                        <Link className="nav-link" to="/reports">Reports</Link>
                    </li>
                </ul>
                <ul className="navbar-nav">
                    {isAuthenticated ? (
                        <li className="nav-item">
                            <Link className="nav-link" to="/logout">Logout</Link>
                        </li>
                    ) : (
                        <>
                            <li className="nav-item">
                                <Link className="nav-link" to="/login">Login</Link>
                            </li>
                            <li className="nav-item">
                                <Link className="nav-link" to="/register">Register</Link>
                            </li>
                        </>
                    )}
                </ul>
            </div>
        </nav>
    );
};

export default Header;
