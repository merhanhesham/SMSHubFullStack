import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import authService from '../Services/AuthService'; // Ensure this path matches your project structure
import { Link } from 'react-router-dom';

export default function Register() {
  const [email, setEmail] = useState('');
  const [displayName, setDisplayName] = useState('');
  const [phoneNumber, setPhoneNumber] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [role, setRole] = useState(''); // State to hold the selected role
  const [error, setError] = useState('');
  const [confirmPasswordError, setConfirmPasswordError] = useState('');
  const [successMessage, setSuccessMessage] = useState('');
  const navigate = useNavigate();

  const isValidEgyptianPhoneNumber = (number) => {
    const egyptianNumberRegex = /^(01)[0125][0-9]{8}$/;
    return egyptianNumberRegex.test(number);
  };

  const validatePassword = (password) => {
    const regex = /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@#$%^&+=]).{8,}$/;
    return regex.test(password);
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    setError('');
    setConfirmPasswordError('');

    if (!validatePassword(password)) {
      setError('Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character, and be at least 8 characters long.');
      return;
    }

    if (password !== confirmPassword) {
      setConfirmPasswordError('Passwords do not match.');
      return;
    }

    const emailRegex = /\S+@\S+\.\S+/;
    if (!emailRegex.test(email)) {
      setError('Please enter a valid email address.');
      return;
    }

    if (!isValidEgyptianPhoneNumber(phoneNumber)) {
      setError('Please enter a valid phone number.');
      return;
    }

    try {
      const data = await authService.register(email, displayName, phoneNumber, password, role);
      setSuccessMessage('Registration successful. Redirecting...');
      setTimeout(() => {
        navigate('/Login');
      }, 2000);
    } catch (error) {
      const errorObj = JSON.parse(error.message);
      if (errorObj.statusCode === 400 && errorObj.message === "This email already exists") {
        setError('This email already exists. Please use a different email or log in.');
      } else {
        setError('Failed to register. Please try again.');
      }
    }
  };

  return (
    <div className="register-container">
      <form onSubmit={handleSubmit}>
        <h2 style={{ fontWeight: 'bold' }}>Create Account</h2>
        {error && <p className="error-message">{error}</p>}
        {confirmPasswordError && <p className="error-message">{confirmPasswordError}</p>}
        {successMessage && <p className="success-message">{successMessage}</p>}
        {/* Form fields remain unchanged */}
        {/* Email Input */}
        <div className="form-group">
          <label>Email:</label>
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>

        {/* Display Name Input */}
        <div className="form-group">
          <label>Display Name:</label>
          <input
            type="text"
            value={displayName}
            onChange={(e) => setDisplayName(e.target.value)}
            required
          />
        </div>

        {/* Phone Number Input */}
        <div className="form-group">
          <label>Phone Number (Egypt):</label>
          <input
            type="text"
            value={phoneNumber}
            onChange={(e) => setPhoneNumber(e.target.value)}
            required
          />
        </div>

        {/* Password Input */}
        <div className="form-group">
          <label>Password:</label>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>

        {/* Confirm Password Input */}
        <div className="form-group">
          <label>Confirm Password:</label>
          <input
            type="password"
            value={confirmPassword}
            onChange={(e) => setConfirmPassword(e.target.value)}
            required
          />
        </div>
        <div className="form-group allrole">
          <label className='role'>Role:</label>
          <select
            value={role}
            onChange={(e) => setRole(e.target.value)}
            required
          >
            <option value="">Select a Role</option>
            <option value="Administrator">Administrator</option>
            <option value="CustomerSupport">Customer Support</option>
            <option value="MarketingManager">Marketing Manager</option>
          </select>
        </div>
        {/* Submit Button */}
        <button type="submit">Register</button>

        {/* Link to Login Page for Already Registered Users */}
        <h5>Already have an account?<span> <Link className="nav-link" to="/login">login now</Link></span> </h5>

      </form>
    </div>
  );
}
