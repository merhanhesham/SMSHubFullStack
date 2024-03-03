import React from 'react';
//import './Footer.css'; // Assuming you have a separate CSS file for styling

export default function Footer() {
  return (
    <footer className="footer-container text-center">
      <div className="footer-content">
        <p>© 2024 SMS Management Platform. All rights reserved.</p>
        <p>Powered by <a href="https://smseg.com" target="_blank" rel="noopener noreferrer">SMSeg</a></p>
      </div>
    </footer>
  );
}
