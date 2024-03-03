/*import React from 'react';

const Home = () => {
  const gradientStyle = {
    background: 'linear-gradient(90deg, hsla(341, 94%, 49%, 1) 0%, hsla(16, 90%, 77%, 1) 100%)',
    color: 'white',
    textAlign: 'center',
    padding: '50px 20px',
  };

  const featureListStyle = {
    display: 'grid',
    gridTemplateColumns: 'repeat(auto-fill, minmax(250px, 1fr))',
    gap: '20px',
    textAlign: 'center',
    padding: '30px 50px',
    backgroundColor: '#f9f9f9',
  };

  const featureItemStyle = {
    backgroundColor: 'white',
    padding: '20px',
    borderRadius: '10px',
    boxShadow: '0 2px 5px rgba(0,0,0,0.1)',
    color: '#333',
    marginBottom: '10px',
  };

  const fullHeightWidthStyle = {
    width: '100%',
    minHeight: '100vh',
  };

  return (
    <div className="home" style={{ ...fullHeightWidthStyle, fontFamily: 'Arial, sans-serif' }}>
      <header className="hero-section" style={gradientStyle}>
        <h1>SMSHub</h1>
        <p>Connect, Communicate, and Convert with Confidence</p>
        <a href="/register" className="btn-get-started" style={{
          padding: '10px 20px',
          background: 'inherit',
          border: 'none',
          borderRadius: '5px',
          color: 'white',
          textTransform: 'uppercase',
          textDecoration: 'none',
          marginTop: '20px',
          display: 'inline-block',
          fontWeight: 'bold',
          cursor: 'pointer',
        }}>Get Started</a>
      </header>

      <section className="features" style={{ width: '100%', height: '100%' }}>
        <h2 style={{ color:'black',textAlign:'center' }}>Features</h2>
        <div className="feature-list" style={featureListStyle}>
          <Feature title="Send Messages" description="Easily send messages with verified sender IDs" />
          <Feature title="Sender ID Management" description="Manage your sender IDs with a few clicks" />
          <Feature title="Message Templates" description="Choose from templates for quick messaging" />
          <Feature title="Message Reports" description="Detailed reports for tracking and analytics" />
        </div>
      </section>
    </div>
  );
};

const Feature = ({ title, description }) => (
  <div className="feature-item" style={{
    backgroundColor: 'white',
    padding: '20px',
    borderRadius: '10px',
    boxShadow: '0 2px 5px rgba(0,0,0,0.1)',
    color: '#333',
    marginBottom: '10px',
  }}>
    <h3>{title}</h3>
    <p>{description}</p>
  </div>
);

export default Home;
*/
import React from 'react';
// Assuming the CSS file is named Home.css and is located in the same directory


const Home = () => {
  return (
    <main className="home">
      <section className="hero-section">
        <div className="container">
          <h1>Connect, Communicate, and Convert with SMSHub</h1>
          <p>Empower your business with reliable SMS communication.</p>
          <a href="/register" className="btn-get-started">Get Started</a>
        </div>
      </section>

      <section className="features-section">
        <div className="container feature-grid">
          <Feature
            title="Send Messages"
            description="Easily send messages with verified sender IDs."
            iconClassName="fas fa-paper-plane"
          />
          <Feature
            title="Sender ID Management"
            description="Manage your sender IDs with a few clicks."
            iconClassName="fas fa-id-badge"
          />
          <Feature
            title="Message Templates"
            description="Choose from templates for quick messaging."
            iconClassName="fas fa-clone"
          />
          <Feature
            title="Message Reports"
            description="Detailed reports for tracking and analytics."
            iconClassName="fas fa-chart-line"
          />
        </div>
      </section>
    </main>
  );
};

const Feature = ({ title, description, iconClassName }) => (
  <div className="feature-item">
    <i className={iconClassName}></i>
    <h3>{title}</h3>
    <p>{description}</p>
  </div>
);

export default Home;

