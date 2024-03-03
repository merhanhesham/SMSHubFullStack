import React, { useState, useEffect } from 'react';
import axios from 'axios';

function SendMessage() {
    const [senderIds, setSenderIds] = useState([]);
    const [templates, setTemplates] = useState([]);
    const [selectedSenderId, setSelectedSenderId] = useState('');
    const [selectedTemplate, setSelectedTemplate] = useState('');
    const [message, setMessage] = useState('');
    const [recipients, setRecipients] = useState('');
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

      // Retrieve the token at the top level of your component
      const token = localStorage.getItem('authToken');

      useEffect(() => {
          if (!token) {
              setError('Authentication token not found. Please log in.');
              return;
          }
  
          const fetchSenderIdsAndTemplates = async () => {
              try {
                  const senderIdsResponse = await axios.get('https://localhost:7012/api/SenderIds/assignedSenderIds', {
                      headers: { 'Authorization': `Bearer ${token}` }
                  });
                  setSenderIds(senderIdsResponse.data);
  
                  const templatesResponse = await axios.get('https://localhost:7012/api/SenderIds/GetTemplates', {
                      headers: { 'Authorization': `Bearer ${token}` }
                  });
                  setTemplates(templatesResponse.data);
              } catch (error) {
                  console.error('Error fetching data:', error);
                  setError('Failed to fetch data. Please try again.');
              }
          };
  
          fetchSenderIdsAndTemplates();
      }, [token]); // Add token as a dependency here to re-run the effect if the token changes
  
      const handleSendMessage = async (e) => {
          e.preventDefault();
          setError('');
          setSuccess('');
  
          if (!token) {
              setError('Authentication token not found. Please log in again.');
              return;
          }
  
          try {
              await axios.post('https://localhost:7012/api/Sms/Send', {
                  From: selectedSenderId,
                  Message: selectedTemplate || message, // Use selected template content or custom message
                  Recipients: recipients.split(',').map(r => r.trim()) // Assuming recipients are comma-separated
              }, {
                  headers: { 'Authorization': `Bearer ${token}` }
              });
  
              setSuccess('Message sent successfully!');
              setTimeout(() => setSuccess(''), 5000); // Clear success message after 5 seconds
          } catch (error) {
              console.error('Send message error:', error);
              setError('Failed to send message. Please try again.');
          }
      };

    return (
        <div style={{ marginTop: '100px' }}>
            <div style={{ maxWidth: '600px', margin: 'auto', padding: '50px', background: 'white', color: 'black' }}>
                <form onSubmit={handleSendMessage} style={{ display: 'flex', flexDirection: 'column', gap: '10px' }}>
                    <h2 style={{ textAlign: 'center', margin: '40px' }}>Send Message</h2>
                    <div>
                        <label>Sender ID:</label>
                        <select value={selectedSenderId} onChange={e => setSelectedSenderId(e.target.value)} style={{ width: '100%', padding: '10px', margin: '10px 0' }}>
                            <option value="">Select Sender ID</option>
                            {senderIds.map(sId => (
                                <option key={sId.senderIdText} value={sId.senderIdText}>
                                    {sId.senderIdText} - {sId.description}
                                </option>
                            ))}
                        </select>
                    </div>
                    <div>
                        <label>Template:</label>
                        <select value={selectedTemplate} onChange={e => setSelectedTemplate(e.target.value)} style={{ width: '100%', padding: '10px', margin: '10px 0' }}>
                            <option value="">Select Template</option>
                            {templates.map(template => (
                                <option key={template.id} value={template.content}>
                                    {template.name}
                                </option>
                            ))}
                        </select>
                    </div>
                    <div>
                        <label>Message:</label>
                        <textarea value={message} onChange={e => setMessage(e.target.value)} style={{ width: '100%', padding: '10px', margin: '10px 0' }} />
                    </div>
                    <div>
                        <label>Recipients:</label>
                        <input type="text" value={recipients} onChange={e => setRecipients(e.target.value)} placeholder="Enter comma-separated numbers" style={{ width: '100%', padding: '10px', margin: '10px 0' }} />
                    </div>
                    {error && <p style={{ color: 'red', textAlign: 'center' }}>{error}</p>}
                    {success && <p style={{ color: 'green', textAlign: 'center' }}>{success}</p>}
                    <button type="submit" style={{ padding: '10px', backgroundColor: '#007bff', color: 'white', border: 'none', cursor: 'pointer', marginTop: '20px' }}>Send Message</button>
                </form>
            </div>
        </div>
    );
}

export default SendMessage;
