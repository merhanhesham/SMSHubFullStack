import React, { useState, useEffect } from 'react';

function SenderIdForm() {
    const [senderIds, setSenderIds] = useState([]);
    const [senderId, setSenderId] = useState({
        id: '',
        senderIdText: '',
        description: '',
        isActive: true,
        userId: ''
    });
    const [mode, setMode] = useState('create');
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');
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
                fetchSenderIds();
            } else {
                setError('Access Denied: You do not have permission to view this page.');
            }
        } else {
            setError('Authentication token not found. Please log in.');
        }
    }, []);

    const fetchSenderIds = async () => {
        const token = localStorage.getItem('authToken');
        try {
            const response = await fetch('https://localhost:7012/api/SenderIds', {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            });
            if (response.ok) {
                const data = await response.json();
                setSenderIds(data);
            } else {
                throw new Error('Failed to fetch Sender IDs');
            }
        } catch (error) {
            setError('Failed to fetch Sender IDs. Please try again.');
            console.error('Error fetching Sender IDs:', error);
        }
    };

    const handleInputChange = (event) => {
        const { name, value, type, checked } = event.target;
        setSenderId(prevState => ({
            ...prevState,
            [name]: type === 'checkbox' ? checked : value
        }));
    };

    const handleSelectChange = (event) => {
        const selectedId = event.target.value;
        const selectedSenderId = senderIds.find(s => s.id.toString() === selectedId);
        if (selectedSenderId) {
            setSenderId({
                ...selectedSenderId,
                userId: senderId.userId
            });
        }
    };

    const handleSubmit = async (event) => {
        event.preventDefault();
        if (!isAuthorized) {
            setError('You are not authorized to perform this action.');
            return;
        }
        
        const token = localStorage.getItem('authToken');
        let url = 'https://localhost:7012/api/SenderIds';
        let method = 'POST';

        if (mode === 'update') {
            url += `/${senderId.id}`;
            method = 'PUT';
        } else if (mode === 'delete') {
            url += `/${senderId.id}`;
            method = 'DELETE';
        }

        try {
            const requestOptions = {
                method: method,
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: mode !== 'delete' ? JSON.stringify(senderId) : null,
            };

            const response = await fetch(url, requestOptions);

            if (response.ok) {
                const successMessage = mode === 'create' ? 'Sender ID created successfully.'
                    : mode === 'update' ? 'Sender ID updated successfully.'
                        : 'Sender ID deleted successfully.';
                setSuccess(successMessage);
                setTimeout(() => setSuccess(''), 3000); // Clear success message after 3 seconds
                setError('');
                fetchSenderIds(); // Refresh the list
            } else if (response.status === 401 || response.status === 403) {
                setError('Authentication failed. Please log in again.');
                setSuccess('');
            } else {
                setError(`Failed to ${mode} Sender ID. Please try again later.`);
                setSuccess('');
            }
        } catch (error) {
            console.error(`${mode} error:`, error);
            setError(`An error occurred while ${mode === 'create' ? 'creating' : mode} a Sender ID.`);
            setSuccess('');
        }
    };

    if (!isAuthorized) {
        return <div style={{ color: 'red', textAlign: 'center', marginTop: '20px' }}>{error}</div>;
    }

    return (
        <div style={{ marginTop: '100px' }}>
            <div style={{ maxWidth: '600px', margin: 'auto', padding: '50px', background: 'white', color: 'black' }}>
                <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: '10px' }}>
                    <h2 style={{ textAlign: 'center', margin: '40px' }}>{mode.charAt(0).toUpperCase() + mode.slice(1)} Sender ID</h2>
                    {(mode === 'update' || mode === 'delete') && (
                        <div>
                            <label>Select Sender ID:</label>
                            <select
                                name="id"
                                value={senderId.id}
                                onChange={handleSelectChange}
                                required
                                style={{ width: '100%', padding: '10px', margin: '10px 0' }}>
                                <option value="">Select Sender ID</option>
                                {senderIds.map((sId) => (
                                    <option key={sId.id} value={sId.id}>{sId.senderIdText}</option>
                                ))}
                            </select>
                        </div>
                    )}
                    {error && <p style={{ color: 'red', textAlign: 'center' }}>{error}</p>}
                    {success && <p style={{ color: 'green', textAlign: 'center' }}>{success}</p>}
                    <div>
                        <label>Sender ID Text:</label>
                        <input
                            type="text"
                            name="senderIdText"
                            value={senderId.senderIdText}
                            onChange={handleInputChange}
                            required
                            style={{ width: '100%', padding: '10px', margin: '10px 0' }}
                        />
                    </div>
                    <div>
                        <label>Description:</label>
                        <input
                            type="text"
                            name="description"
                            value={senderId.description}
                            onChange={handleInputChange}
                            style={{ width: '100%', padding: '10px', margin: '10px 0' }}
                        />
                    </div>
                    <div>
                        <label>Is Active:</label>
                        <input
                            type="checkbox"
                            name="isActive"
                            checked={senderId.isActive}
                            onChange={handleInputChange}
                            style={{ marginLeft: '10px' }}
                        />
                    </div>
                    <button type="submit" style={{ padding: '10px', backgroundColor: '#007bff', color: 'white', border: 'none', cursor: 'pointer', marginTop: '20px' }}>
                        {mode.charAt(0).toUpperCase() + mode.slice(1)} Sender ID
                    </button>
                </form>
                <div style={{ display: 'flex', justifyContent: 'center', gap: '10px', marginTop: '20px' }}>
                    <button onClick={() => setMode('create')} style={{ padding: '10px', backgroundColor: 'hsla(341, 94%, 49%, 1)', color: 'white', border: 'none', cursor: 'pointer' }}>Create Mode</button>
                    <button onClick={() => setMode('update')} style={{ padding: '10px', backgroundColor: 'hsla(16, 90%, 77%, 1)', color: 'white', border: 'none', cursor: 'pointer' }}>Update Mode</button>
                    <button onClick={() => setMode('delete')} style={{ padding: '10px', backgroundColor: 'hsla(358.5, 92%, 63%, 1)', color: 'white', border: 'none', cursor: 'pointer' }}>Delete Mode</button>
                </div>
            </div>
        </div>
    );
}

export default SenderIdForm;
