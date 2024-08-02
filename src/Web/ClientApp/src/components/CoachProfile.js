import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom'; // Import useParams to get route parameters
import axiosInstance from '../utils/axiosInstance'; // Import the configured Axios instance

const CoachProfile = () => {
  const [profile, setProfile] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const { id } = useParams(); // Get the dynamic id from the route

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
    <div>
      <h1>Coach Profile</h1>
      <div>
        <img src={profile.profilePicture} alt="Profile" style={{ width: '150px', height: '150px', borderRadius: '50%' }} />
        <p>Bio: {profile.bio}</p>
        <p>Major Achievements: {profile.majorAchievements.join(', ')}</p>
        <div>
          <h2>Gallery</h2>
          {profile.galleryImageLinks.map((link, index) => (
            <img key={index} src={link} alt={`Gallery ${index + 1}`} style={{ width: '100px', height: '100px', margin: '5px' }} />
          ))}
        </div>
        <div>
          <h2>Programs Overview</h2>
          {profile.programsOverview.length > 0 ? (
            <ul>
              {profile.programsOverview.map((program, index) => (
                <li key={index}>{program}</li>
              ))}
            </ul>
          ) : (
            <p>No programs available.</p>
          )}
        </div>
      </div>
    </div>
  );
};

export default CoachProfile;
