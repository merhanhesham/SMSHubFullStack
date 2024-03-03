
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import authService from '../Services/AuthLoginService';
import { Link } from 'react-router-dom';

export default function Login() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const [successMessage, setSuccessMessage] = useState('');
  const navigate = useNavigate();

  const handleSubmit = async (event) => {
    event.preventDefault();
    setError(''); // Reset error message at the beginning
    setSuccessMessage(''); // Reset success message at the beginning
    setLoading(true);
  
    try {
      const token = await authService.login(email, password);
      console.log('Login successful', token);
      setLoading(false);
      setSuccessMessage('Login successful. Redirecting...');
      setTimeout(() => {
        navigate('/Home');
      }, 2000); // Delay for showing success message
    } catch (error) {
      setLoading(false);
      // Set a generic error message for any login failure
      setError('Email or password is incorrect.');
    }
  };
  

  return (
    <div className="login-container">
      <form onSubmit={handleSubmit}>
        <h2>Login</h2>
        {loading && <p>Loading...</p>}
        {error && <p className="error-message">{error}</p>}
        {successMessage && <p className="success-message">{successMessage}</p>}
        <div className="form-group">
          <label>Email:</label>
          <input type="email" value={email} onChange={(e) => setEmail(e.target.value)} required />
        </div>
        <div className="form-group">
          <label>Password:</label>
          <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} required />
        </div>
        <button type="submit" disabled={loading}>Login</button>
        <h5>Not Registered?<span> <Link className="nav-link" to="/register">Sign up now</Link></span> </h5>
      </form>
    </div>
  );
}
