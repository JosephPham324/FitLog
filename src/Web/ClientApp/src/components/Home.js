import React, { Component } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import num1Image from '../Images/photo1.jpg';
import num2Image from '../Images/photo2.jpg';
import num3Image from '../Images/photo3.jpg';
import num4Image from '../Images/photo4.jpg';
import num5Image from '../Images/photo5.jpg';
import num6Image from '../Images/photo6.jpg';
import num7Image from '../Images/photo7.jpg';
import num8Image from '../Images/photo8.jpg';
import num9Image from '../Images/photo9.jpg';

import program1 from '../Images/program1.jpg'; // Add program images
import program2 from '../Images/program2.jpg';
import program3 from '../Images/program3.jpg';
import program4 from '../Images/program4.jpg';
import program5 from '../Images/program5.jpg';

import trainer1 from '../Images/hlv1.jpg'; // Add trainer images
import trainer2 from '../Images/hlv2.jpg';
import trainer3 from '../Images/hlv3.jpg';
import trainer4 from '../Images/hlv4.jpg';

export class Home extends Component {
  static displayName = Home.name;

  state = {
    showFooter: true,
  };

  componentDidMount() {
    window.addEventListener('scroll', this.handleScroll);
  }

  componentWillUnmount() {
    window.removeEventListener('scroll', this.handleScroll);
  }

  handleScroll = () => {
    const { innerHeight } = window;
    const { scrollTop, offsetHeight } = document.documentElement;

    if (innerHeight + scrollTop >= offsetHeight - 5) {
      this.setState({ showFooter: true });
    } else {
      this.setState({ showFooter: true });
    }
  };

  render() {
    const containerStyle = {
      minHeight: '100vh',
      display: 'flex',
      flexDirection: 'column',
      justifyContent: 'space-between',
    };

    const sectionStyle = {
      width: '100%',
      minHeight: '700px',
      padding: '20px',
    };

    const cardStyle = {
      height: '100%',
      display: 'flex',
      flexDirection: 'column',
      alignItems: 'center',
      justifyContent: 'center',
      backgroundColor: '#fff',
      boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)',
      border: 'none',
      marginBottom: '30px', // Increased margin to prevent overlap
    };

    const imageStyle = {
      height: '350px',
      objectFit: 'cover',
      width: '100%',
    };

    const programCardStyle = {
      display: 'flex',
      flexDirection: 'column',
      alignItems: 'center',
      justifyContent: 'center',
      backgroundColor: '#fff',
      boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)',
      border: 'none',
      marginBottom: '20px',
    };

    const programImageStyle = {
      width: '100%',
      height: '200px', // Fixed height to ensure uniformity
      objectFit: 'cover',
    };

    const aboutImageStyle = {
      height: '100%',
      width: '100%',
      objectFit: 'cover',
    };

    const cardTextStyle = {
      display: 'inline',
      justifyContent: 'center',
      alignItems: 'center',
      height: '40%',
      color: '#333',
    };

    const trainerCardStyle = {
      height: '450px', // Similar height to weight training cards
      display: 'flex',
      flexDirection: 'column',
      alignItems: 'center',
      justifyContent: 'center',
      backgroundColor: '#fff',
      boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)',
      border: 'none',
      marginBottom: '30px',
    };

    const trainerImageStyle = {
      height: '100%', // Similar height to weight training images
      width: '100%',  // Full width
      objectFit: 'cover',
    };

    const footerStyle = {
      backgroundColor: '#343a40',
      color: 'white',
      padding: '20px',
      textAlign: 'center',
      width: '100%',
      marginTop: '20px',
    };

    const headingStyle = {
      color: '#6c757d',
      fontSize: '3rem',
      fontWeight: 'bold',
    };

    const subheadingStyle = {
      color: '#6c757d',
      fontSize: '1.5rem',
      fontWeight: 'normal',
      marginBottom: '20px',
    };

    const listStyle = {
      color: '#6c757d',
    };

    const carouselStyle = {
      maxHeight: '100vh',
      overflow: 'hidden',
    };

    return (
      <div style={containerStyle}>
        <div id="carouselExampleIndicators" className="carousel slide" data-bs-ride="carousel" style={carouselStyle}>
          <ol className="carousel-indicators">
            <li data-bs-target="#carouselExampleIndicators" data-bs-slide-to="0" className="active"></li>
            <li data-bs-target="#carouselExampleIndicators" data-bs-slide-to="1"></li>
            <li data-bs-target="#carouselExampleIndicators" data-bs-slide-to="2"></li>
          </ol>
          <div className="carousel-inner">
            <div className="carousel-item active">
              <img className="d-block w-100" src={num1Image} alt="First slide" />
            </div>
            <div className="carousel-item">
              <img className="d-block w-100" src={num2Image} alt="Second slide" />
            </div>
            <div className="carousel-item">
              <img className="d-block w-100" src={num3Image} alt="Third slide" />
            </div>
          </div>
          <a className="carousel-control-prev" href="#carouselExampleIndicators" role="button" data-bs-slide="prev">
            <span className="carousel-control-prev-icon" aria-hidden="true"></span>
            <span className="sr-only">Previous</span>
          </a>
          <a className="carousel-control-next" href="#carouselExampleIndicators" role="button" data-bs-slide="next">
            <span className="carousel-control-next-icon" aria-hidden="true"></span>
            <span className="sr-only">Next</span>
          </a>
        </div>

        <div className="container-fluid mt-5 d-flex justify-content-center" style={sectionStyle}>
          <div className="row align-items-center w-100">
            <div className="col-md-4">
              <div>
                <h1 style={headingStyle}>About FitLog</h1>
              </div>
              <div>
                <p style={subheadingStyle}>
                  <b>Welcome to FitLog - Fitness Training Management and Tracking System, where we provide the perfect solution for managing and tracking your fitness progress.</b>
                </p>
                <p style={listStyle}>At FitLog - Fitness Training Management and Tracking System, we are committed to providing you with the tools and resources you need to achieve your fitness goals in the most effective and reliable way. With an advanced technology platform and a team of experienced experts, we bring you:</p>
                <ul style={listStyle}>
                  <li>Convenient Management</li>
                  <li>Personalization Options</li>
                  <li>Progress Tracking and Reporting</li>
                  <li>Community Support</li>
                </ul>
              </div>
            </div>
            <div className="col-md-4">
              <img className="d-block w-100" src={num4Image} alt="FitLog overview" style={aboutImageStyle} />
            </div>
            <div className="col-md-4">
              <img className="d-block w-100" src={num5Image} alt="FitLog overview" style={aboutImageStyle} />
            </div>
          </div>
        </div>

        <div className="container-fluid mt-5" style={sectionStyle}>
          <div className="d-flex justify-content-center align-items-center flex-column">
            <div className="mb-4">
              <h1 style={headingStyle}><b>PROGRAMS</b></h1>
            </div>
            <div className="row justify-content-center w-100">
              <div className="col-md-4 mb-4">
                <div className="card" style={programCardStyle}>
                  <img className="card-img-top" src={program1} alt="Program 1" style={programImageStyle} />
                  <div className="card-body" style={cardTextStyle}>
                    <p className="card-text text-center">
                      <b>Program 1</b>
                    </p>
                  </div>
                </div>
              </div>
              <div className="col-md-4 mb-4">
                <div className="card" style={programCardStyle}>
                  <img className="card-img-top" src={program2} alt="Program 2" style={programImageStyle} />
                  <div className="card-body" style={cardTextStyle}>
                    <p className="card-text text-center">
                      <b>Program 2</b>
                    </p>
                  </div>
                </div>
              </div>
              <div className="col-md-4 mb-4">
                <div className="card" style={programCardStyle}>
                  <img className="card-img-top" src={program3} alt="Program 3" style={programImageStyle} />
                  <div className="card-body" style={cardTextStyle}>
                    <p className="card-text text-center">
                      <b>Program 3</b>
                    </p>
                  </div>
                </div>
              </div>
              <div className="col-md-4 mb-4">
                <div className="card" style={programCardStyle}>
                  <img className="card-img-top" src={program4} alt="Program 4" style={programImageStyle} />
                  <div className="card-body" style={cardTextStyle}>
                    <p className="card-text text-center">
                      <b>Program 4</b>
                    </p>
                  </div>
                </div>
              </div>
              <div className="col-md-4 mb-4">
                <div className="card" style={programCardStyle}>
                  <img className="card-img-top" src={program5} alt="Program 5" style={programImageStyle} />
                  <div className="card-body" style={cardTextStyle}>
                    <p className="card-text text-center">
                      <b>Program 5</b>
                    </p>
                  </div>
                </div>
              </div>
            </div>
            <div className="font-bold mb-5">
              <a href="https://localhost:44447/WorkoutProgramsPage" className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded">
                See all program
              </a>
            </div>
          </div>
        </div>

        <div className="container-fluid mt-5" style={sectionStyle}>
          <div className="d-flex justify-content-center align-items-center flex-column">
            <div className="mb-4">
              <h1 style={headingStyle}><b>WEIGHT TRAINING</b></h1>
            </div>
            <div className="row justify-content-center w-100">
              <div className="col-md-3 mb-4">
                <div className="card" style={cardStyle}>
                  <img className="card-img-top" src={num6Image} alt="Card 1" style={imageStyle} />
                  <div className="card-body" style={cardTextStyle}>
                    <p className="card-text text-center">
                      <b>BUILD STRENGTH</b>
                    </p>
                  </div>
                </div>
              </div>
              <div className="col-md-3 mb-4">
                <div className="card" style={cardStyle}>
                  <img className="card-img-top" src={num7Image} alt="Card 2" style={imageStyle} />
                  <div className="card-body" style={cardTextStyle}>
                    <p className="card-text text-center">
                      <b>FLEXIBILITY</b>
                    </p>
                  </div>
                </div>
              </div>
              <div className="col-md-3 mb-4">
                <div className="card" style={cardStyle}>
                  <img className="card-img-top" src={num8Image} alt="Card 3" style={imageStyle} />
                  <div className="card-body" style={cardTextStyle}>
                    <p className="card-text text-center">
                      <b>STRONG BODY</b>
                    </p>
                  </div>
                </div>
              </div>
              <div className="col-md-3 mb-4">
                <div className="card" style={cardStyle}>
                  <img className="card-img-top" src={num9Image} alt="Card 4" style={imageStyle} />
                  <div className="card-body" style={cardTextStyle}>
                    <p className="card-text text-center">
                      <b>EXPERT GUIDANCE</b>
                    </p>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div className="container-fluid mt-5" style={sectionStyle}>
          <div className="d-flex justify-content-center align-items-center flex-column">
            <div className="mb-4">
              <h1 style={headingStyle}><b>OUR TRAINERS</b></h1>
            </div>
            <div className="row justify-content-center w-100">
              <div className="col-md-3 mb-4">
                <div className="card" style={trainerCardStyle}>
                  <img className="card-img-top" src={trainer1} alt="Trainer 1" style={trainerImageStyle} />
                  <div className="card-body" style={cardTextStyle}>
                    <p className="card-text text-center">
                      <b>John Doe</b>
                    </p>
                    <p>Training Body</p>
                  </div>
                </div>
              </div>
              <div className="col-md-3 mb-4">
                <div className="card" style={trainerCardStyle}>
                  <img className="card-img-top" src={trainer2} alt="Trainer 2" style={trainerImageStyle} />
                  <div className="card-body" style={cardTextStyle}>
                    <p className="card-text text-center">
                      <b>Jane Smith</b>
                    </p>
                    <p>Training Body</p>
                  </div>
                </div>
              </div>
              <div className="col-md-3 mb-4">
                <div className="card" style={trainerCardStyle}>
                  <img className="card-img-top" src={trainer3} alt="Trainer 3" style={trainerImageStyle} />
                  <div className="card-body" style={cardTextStyle}>
                    <p className="card-text text-center">
                      <b>Robert Brown</b>
                    </p>
                    <p>Training Body</p>
                  </div>
                </div>
              </div>
              <div className="col-md-3 mb-4">
                <div className="card" style={trainerCardStyle}>
                  <img className="card-img-top" src={trainer4} alt="Trainer 4" style={trainerImageStyle} />
                  <div className="card-body" style={cardTextStyle}>
                    <p className="card-text text-center">
                      <b>Emily Davis</b>
                    </p>
                    <p>Training Body</p>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        {this.state.showFooter && (
          <footer style={footerStyle}>
            <p>&copy; 2024 FitLog. All rights reserved.</p>
            <p>Contact us: <a href="mailto:info@fitlog.com" style={{ color: 'white' }}>info@fitlog.com</a></p>
            <p>
              Follow us on:
              <a href="https://www.facebook.com" target="_blank" rel="noopener noreferrer" style={{ color: 'white', marginLeft: '10px' }}>Facebook</a> |
              <a href="https://www.twitter.com" target="_blank" rel="noopener noreferrer" style={{ color: 'white', marginLeft: '10px' }}>Twitter</a> |
              <a href="https://www.instagram.com" target="_blank" rel="noopener noreferrer" style={{ color: 'white', marginLeft: '10px' }}>Instagram</a>
            </p>
          </footer>
        )}
      </div>
    );
  }
}
