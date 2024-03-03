import React, { useState, useEffect } from 'react';

function AssignSenderIdForm() {
    const [users, setUsers] = useState([]);
    const [senderIds, setSenderIds] = useState([]);
    const [selectedUserId, setSelectedUserId] = useState('');
    const [selectedSenderIds, setSelectedSenderIds] = useState([]);
    const [error, setError] = useState('');
    const [isAuthorized, setIsAuthorized] = useState(false);

    // Decode JWT payload to check user role
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
        const token = localStorage.getItem('authToken');
        if (token) {
            const decodedToken = decodeJWT(token);
            const userRole = decodedToken && decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
            if (userRole === 'Administrator') {
                setIsAuthorized(true);
                (async () => {
                    await fetchUsers();
                    await fetchSenderIds();
                })();
            } else {
                setError('Access Denied: You do not have permission to view this page.');
            }
        } else {
            setError('Authentication token not found. Please log in.');
        }
    }, []);

    const fetchUsers = async () => {
        const token = localStorage.getItem('authToken');
        try {
            const response = await fetch('https://localhost:7012/api/SenderIds/GetUsers', {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            });
            if (response.ok) {
                const data = await response.json();
                setUsers(data);
            } else {
                setError('Failed to fetch users. Please try again later.');
            }
        } catch (error) {
            setError('An error occurred while fetching users.');
        }
    };

    const fetchSenderIds = async () => {
        const token = localStorage.getItem('authToken');
        try {
            const response = await fetch('https://localhost:7012/api/SenderIds', {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            });
            if (response.ok) {
                const data = await response.json();
                setSenderIds(data);
            } else {
                setError('Failed to fetch sender IDs. Please try again later.');
            }
        } catch (error) {
            setError('An error occurred while fetching sender IDs.');
        }
    };

    const handleCheckboxChange = (senderIdText) => {
        setSelectedSenderIds(prevSelected =>
            prevSelected.includes(senderIdText) ? prevSelected.filter(id => id !== senderIdText) : [...prevSelected, senderIdText]
        );
    };

    const handleSubmit = async (event) => {
        event.preventDefault();
        const token = localStorage.getItem('authToken');
        const payload = {
            userId: selectedUserId,
            senderIdTexts: selectedSenderIds,
        };

        try {
            const response = await fetch('https://localhost:7012/api/SenderIds/AssignSenderIds', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(payload),
            });

            if (response.ok) {
                alert('Sender IDs assigned successfully.');
            } else {
                setError('Failed to assign Sender IDs. Please try again later.');
            }
        } catch (error) {
            setError('An error occurred while assigning sender IDs.');
        }
    };

    if (!isAuthorized) {
        return <div>{error}</div>;
    }

    return (
        <div style={{ maxWidth: '600px', margin: 'auto', padding: '20px' }}>
            <h2 style={{ textAlign: 'center', margin: '40px' }}>Assign Sender IDs to User</h2>
            {error && <p style={{ color: 'red' }}>{error}</p>}
            <form onSubmit={handleSubmit}>
                <div>
                    <label>User:</label>
                    <select
                        value={selectedUserId}
                        onChange={(e) => setSelectedUserId(e.target.value)}
                        required
                        style={{ width: '100%', padding: '10px', margin: '10px 0' }}>
                        <option value="">Select a User</option>
                        
                        {users.map((user) => (
                            <option key={user.id} value={user.id}>
                                {user.displayName} ({user.role})
                            </option>
                        ))}
                    </select>
                </div>
                <div style={{ marginBottom: '30px' }}>
                    <label>Sender IDs:</label>
                    {senderIds.length > 0 ? senderIds.map((senderId) => (
                        <div key={senderId.id}>
                            <input
                                type="checkbox"
                                id={senderId.id}
                                checked={selectedSenderIds.includes(senderId.senderIdText)}
                                onChange={() => handleCheckboxChange(senderId.senderIdText)}
                            />
                            <label htmlFor={senderId.id} style={{ marginLeft: '8px', color:'black' }}>{senderId.senderIdText}</label>
                        </div>
                    )) : <p>No sender IDs available.</p>}
                </div>
                <button type="submit" style={{ padding: '10px', backgroundColor: '#007bff', color: 'white', border: 'none', cursor: 'pointer' }}>
                    Assign Sender IDs
                </button>
            </form>
        </div>
    );
}

export default AssignSenderIdForm;
