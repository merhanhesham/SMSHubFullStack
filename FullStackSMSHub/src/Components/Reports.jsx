import React, { useState, useEffect } from 'react';
import axios from 'axios';

function SmsDetails() {
    const [smsDetails, setSmsDetails] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState('');

    const token = localStorage.getItem('authToken');

    // Manually decode the JWT payload
    const decodeJWT = (token) => {
        try {
            const base64Url = token.split('.')[1];
            const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
            const payload = JSON.parse(window.atob(base64));
            return payload;
        } catch (error) {
            console.error('Failed to decode JWT:', error);
            return null;
        }
    };

    useEffect(() => {
        if (!token) {
            setError('Authentication token not found. Please log in.');
            setIsLoading(false);
            return;
        }

        const decodedToken = decodeJWT(token);

        // Adjusted role check to use the correct claim
        const userRole = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

        if (userRole !== 'Administrator') {
            setError('Access Denied: You do not have permission to view this page.');
            setIsLoading(false);
            return;
        }

        const fetchSmsDetails = async () => {
            setIsLoading(true);
            try {
                const response = await axios.get('https://localhost:7012/api/SenderIds/GetSmsDetails', {
                    headers: { Authorization: `Bearer ${token}` },
                });
                setSmsDetails(response.data);
            } catch (err) {
                console.error('Failed to fetch SMS details:', err);
                setError('Failed to load SMS details. Ensure you are authorized and the server is reachable.');
            } finally {
                setIsLoading(false);
            }
        };

        fetchSmsDetails();
    }, [token]);

    if (isLoading) return <div>Loading...</div>;
if (error) return <div style={{ color: 'red', textAlign: 'center', marginTop: '20px', fontSize: '18px' }}>{error}</div>;

    return (
        <div style={{ margin: '20px' }}>
            <h2>SMS Details</h2>
            {smsDetails.length > 0 ? (
                smsDetails.map((detail, index) => (
                    <div key={index} style={{ border: '1px solid #ddd', padding: '10px', marginBottom: '10px' }}>
                        <h3>Message Content:</h3>
                        <textarea 
                          value={detail.message} 
                          readOnly={true}
                          style={{ width: '100%', marginBottom: '10px' }}
                        />
                        <p>From: {detail.from}</p>
                        <p>Sent Time: {new Date(detail.sentTime).toLocaleString()}</p>
                        <div>Recipients:
                            <ul>
                                {detail.recipients?.map((recipient, i) => (
                                    <li key={i}>{recipient}</li>
                                ))}
                            </ul>
                        </div>
                    </div>
                ))
            ) : (
                <p>No SMS messages found.</p>
            )}
        </div>
    );
}

export default SmsDetails;
