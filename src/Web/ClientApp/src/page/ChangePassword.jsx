import React, { useState, useEffect } from "react";
import axiosInstance from '../utils/axiosInstance';
import { useNavigate } from 'react-router-dom';

export const ChangePassword = () => {
    const [passwords, setPasswords] = useState({
        currentPassword: '',
        newPassword: '',
        confirmPassword: ''
    });
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [profile, setProfile] = useState({
        firstName: '',
        lastName: '',
        email: ''
    });
    const navigate = useNavigate();

    // Fetch user profile data when the component loads
    useEffect(() => {
        const fetchProfile = async () => {
            try {
                const response = await axiosInstance.get('/Users/user-profile');
                setProfile({
                    firstName: response.data.firstName,
                    lastName: response.data.lastName,
                    email: response.data.email
                });
            } catch (error) {
                setError('Error fetching user profile');
                console.error('Error fetching user profile:', error.response ? error.response.data : error.message);
            }
        };

        fetchProfile();
    }, []);

    const handleChange = (e) => {
        setPasswords({
            ...passwords,
            [e.target.name]: e.target.value
        });
    };

    const handleSubmit = async () => {
        const passwordValidation = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;

        if (!passwordValidation.test(passwords.newPassword)) {
            setError('Password must be at least 8 characters long and include uppercase, lowercase, numbers, and special characters.');
            return;
        }

        if (passwords.newPassword !== passwords.confirmPassword) {
            setError('New password and confirm password do not match.');
            return;
        }

        try {
            setLoading(true);
            const payload = {
                oldPassword: passwords.currentPassword,
                newPassword: passwords.newPassword
            };

            const response = await axiosInstance.put('/Users/authenticated-reset-password', payload);

            if (response.data.success === false) {
                if (response.data.errors && response.data.errors.includes('Old password is incorrect.')) {
                    alert('Incorrect current password. Please try again.');
                } else {
                    setError('Error updating password');
                    console.error('Error updating password:', response.data.errors.join(', '));
                }
            } else {
                alert('Password updated successfully!');
                setPasswords({
                    currentPassword: '',
                    newPassword: '',
                    confirmPassword: ''
                });
                navigate('/profile'); // Redirect to profile page after success
            }
        } catch (error) {
            setError('An unexpected error occurred');
            console.error('Unexpected error:', error.response ? error.response.data : error.message);
        } finally {
            setLoading(false);
        }
    };

    if (loading) return <div className="flex justify-center items-center h-screen">Updating password...</div>;

    if (error) return (
        <div className="flex flex-col justify-center items-center h-screen text-red-500">
            <div>{error}</div>
            <button
                onClick={() => setError(null)} // Reset the error and allow the user to go back to the form
                className="mt-4 px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-400"
            >
                Go Back
            </button>
        </div>
    );

    return (
        <div className="bg-neutral-300 pt-20 pb-10 px-10">
            <div className="flex">
                <div className="w-1/4 text-center">
                    <a href="/profile" rel="noopener noreferrer">
                        <svg
                            xmlns="http://www.w3.org/2000/svg"
                            aria-hidden="true"
                            role="img"
                            className="iconify iconify--ic text-3xl mb-4 cursor-pointer"
                            width="1em"
                            height="1em"
                            viewBox="0 0 24 24"
                        >
                            <path fill="currentColor" d="M20 11H7.83l5.59-5.59L12 4l-8 8l8 8l1.41-1.41L7.83 13H20z"></path>
                        </svg>
                    </a>
                    <img
                        alt="avatar"
                        src="https://static.vecteezy.com/system/resources/thumbnails/036/280/651/small/default-avatar-profile-icon-social-media-user-image-gray-avatar-icon-blank-profile-silhouette-illustration-vector.jpg"
                        className="rounded-full mb-5 m-auto"
                    />
                    <div className="font-bold mb-3 text-lg">{profile.firstName} {profile.lastName}</div>
                    <div className="font-bold text-neutral-500 mb-3 tracking-wider">
                        {profile.email}
                    </div>
                    <div className="font-bold mb-5">Change password</div>
                </div>
                <div className="w-3/4 rounded-xl bg-white px-5 pt-10 pb-3">
                    <div className="mb-5 text-4xl font-black">CHANGE PASSWORD</div>
                    <div>
                        <div className="mb-4">
                            <div className="pl-2 text-neutral-500 mb-1 font-medium text-sm">
                                Current password
                            </div>
                            <input
                                name="currentPassword"
                                type="password"
                                value={passwords.currentPassword}
                                onChange={handleChange}
                                className="w-1/3 bg-neutral-300 text-neutral-600 py-3 px-2 font-medium rounded-xl"
                            />
                        </div>
                        <div className="mb-4">
                            <div className="pl-2 text-neutral-500 mb-1 font-medium text-sm">
                                New password
                            </div>
                            <input
                                name="newPassword"
                                type="password"
                                value={passwords.newPassword}
                                onChange={handleChange}
                                className="w-1/3 bg-neutral-300 text-neutral-600 py-3 px-2 font-medium rounded-xl"
                            />
                        </div>
                        <div className="mb-4">
                            <div className="pl-2 text-neutral-500 mb-1 font-medium text-sm">
                                Confirm password
                            </div>
                            <input
                                name="confirmPassword"
                                type="password"
                                value={passwords.confirmPassword}
                                onChange={handleChange}
                                className="w-1/3 bg-neutral-300 text-neutral-600 py-3 px-2 font-medium rounded-xl"
                            />
                        </div>
                    </div>
                    <button
                        onClick={handleSubmit}
                        className="my-5 px-4 py-3 rounded-lg m-auto bg-green-600 text-white font-medium block hover:bg-green-500"
                    >
                        Update
                    </button>
                </div>
            </div>
        </div>
    );
};
