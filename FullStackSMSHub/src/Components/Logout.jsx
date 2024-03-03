import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

const Logout = () => {
  const navigate = useNavigate();

  useEffect(() => {
    localStorage.removeItem('authToken');
    // Set a timeout to delay the redirection, giving time for the message to be read
    const timer = setTimeout(() => {
      navigate('/Home');
    }, 2000); // 2000 milliseconds delay

    // Cleanup function to clear the timeout if the component unmounts before the timeout is completed
    return () => clearTimeout(timer);
  }, [navigate]);

  return <div style={{ marginTop: '20px', textAlign: 'center' }}>Logging out, please wait...</div>;
};

export default Logout;
