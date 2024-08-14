import React, { useEffect, useState } from 'react';
import { Icon } from "@iconify/react";
import axiosInstance from '../utils/axiosInstance';
import { useParams } from "react-router-dom";

const CoachProfile = () => {
  const [profile, setProfile] = useState({
    id: '',
    firstName: '',
    lastName: '',
    userName: '',
    gender: '',
    dateOfBirth: '',
    roles: '',
    phoneNumber: '',
    email: ''
  });

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const { id } = useParams(); // Get the dynamic templateId from the route

  useEffect(() => {
    const fetchProfile = async () => {
      try {
        const response = await axiosInstance.get(`${process.env.REACT_APP_BACKEND_URL}/CoachProfile/${id}`, {
          headers: {
            accept: 'application/json',
          },
        });
        setProfile(response.data);
      } catch (error) {
        setError('Error fetching profile');
        console.error('Error fetching profile:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchProfile();
  }, [id]);

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  if (!profile) {
    return <div>No profile found.</div>;
  }

  return (
    <div style={styles.container}>
      <h1 style={styles.header}>Coach Profile</h1>
      <div style={styles.profileContainer}>
        <div style={styles.profileHeader}>
          <img src={profile.profilePicture} alt="Profile" style={styles.profilePicture} />
          <div style={styles.profileInfo}>
            <h2 style={styles.profileName}>{profile.name}</h2>
            <p style={styles.profileBio}>{profile.bio}</p>
          </div>
        </div>
        <div style={styles.section}>
          <h2 style={styles.sectionHeader}>Major Achievements</h2>
          <p style={styles.sectionContent}>{profile.majorAchievements.join(', ')}</p>
        </div>
        <div style={styles.section}>
          <h2 style={styles.sectionHeader}>Gallery</h2>
          <div style={styles.gallery}>
            {profile.galleryImageLinks.map((link, index) => (
              <img key={index} src={link} alt={`Gallery ${index + 1}`} style={styles.galleryImage} />
            ))}
          </div>
        </div>
        <div style={styles.section}>
          <h2 style={styles.sectionHeader}>Programs Overview</h2>
          {profile.programsOverview.length > 0 ? (
            <ul style={styles.programsList}>
              {profile.programsOverview.map((program, index) => (
                <li key={index} style={styles.programItem}>{program}</li>
              ))}
            </ul>
          ) : (
            <p style={styles.noPrograms}>No programs available.</p>
          )}
        </div>
      </div>
    </div>
  );
};


const styles = {
  container: {
    fontFamily: 'Arial, sans-serif',
    color: '#333',
    padding: '20px',
    maxWidth: '900px',
    margin: '0 auto',
  },
  header: {
    textAlign: 'center',
    marginBottom: '20px',
    fontSize: '36px',
    color: '#4CAF50',
  },
  profileContainer: {
    background: '#fff',
    borderRadius: '8px',
    boxShadow: '0 0 10px rgba(0,0,0,0.1)',
    padding: '20px',
  },
  profileHeader: {
    display: 'flex',
    alignItems: 'center',
    marginBottom: '20px',
  },
  profilePicture: {
    width: '150px',
    height: '150px',
    borderRadius: '50%',
    marginRight: '20px',
  },
  profileInfo: {
    flex: 1,
  },
  profileName: {
    fontSize: '24px',
    marginBottom: '10px',
    color: '#4CAF50',
  },
  profileBio: {
    fontSize: '16px',
    lineHeight: '1.5',
  },
  section: {
    marginBottom: '20px',
  },
  sectionHeader: {
    fontSize: '20px',
    marginBottom: '10px',
    color: '#4CAF50',
  },
  sectionContent: {
    fontSize: '16px',
    lineHeight: '1.5',
  },
  gallery: {
    display: 'flex',
    flexWrap: 'wrap',
    gap: '10px',
  },
  galleryImage: {
    width: '100px',
    height: '100px',
    borderRadius: '8px',
    objectFit: 'cover',
  },
  programsList: {
    listStyleType: 'none',
    padding: '0',
  },
  programItem: {
    background: '#f5f5f5',
    padding: '10px',
    borderRadius: '4px',
    marginBottom: '10px',
  },
  noPrograms: {
    fontSize: '16px',
    lineHeight: '1.5',
  },
};


export default CoachProfile;

