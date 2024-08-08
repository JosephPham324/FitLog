import React, { useEffect, useState } from 'react';
import { Icon } from "@iconify/react";
import axiosInstance from '../utils/axiosInstance';

const Modal = ({ show, onClose, message }) => {
    if (!show) return null;

    return (
        <div className="fixed inset-0 bg-gray-600 bg-opacity-50 flex justify-center items-center">
            <div className="bg-white rounded-lg overflow-hidden shadow-xl max-w-md w-full p-4">
                <div className="text-center">
                    <div className="text-lg font-semibold">{message}</div>
                    <button
                        onClick={onClose}
                        className="mt-4 px-4 py-2 bg-blue-500 text-white rounded-lg hover:bg-blue-400"
                    >
                        Close
                    </button>
                </div>
            </div>
        </div>
    );
};

export const Profile = () => {
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
    const [modalVisible, setModalVisible] = useState(false);

    useEffect(() => {
        const fetchProfile = async () => {
            try {
                const response = await axiosInstance.get('/Users/user-profile');
                console.log('Response data:', response.data);

                const { id, firstName, lastName, userName, gender, dateOfBirth, roles, phoneNumber, email } = response.data;
                setProfile({
                    id: id || '',
                    firstName: firstName || '',
                    lastName: lastName || '',
                    userName: userName || '',
                    gender: gender || '',
                    dateOfBirth: dateOfBirth ? dateOfBirth.split('T')[0] : '',
                    roles: roles || '',
                    phoneNumber: phoneNumber || '',
                    email: email || ''
                });
            } catch (error) {
                setError('Error fetching user profile');
                console.error('Error fetching user profile:', error);
            } finally {
                setLoading(false);
            }
        };

        fetchProfile();
    }, []);

    const handleChange = (e) => {
        setProfile({
            ...profile,
            [e.target.name]: e.target.value
        });
    };

    const updateProfile = async () => {
        try {
            const payload = {
                UserId: profile.id,
                FirstName: profile.firstName,
                LastName: profile.lastName,
                DateOfBirth: profile.dateOfBirth,
                Gender: profile.gender,
                Email: profile.email,
                PhoneNumber: profile.phoneNumber,
                UserName: profile.userName,
            };

            const url = `/Users/update-profile`;
            console.log('Sending profile data to:', url, 'with payload:', payload);

            await axiosInstance.put(url, payload, {
                headers: {
                    'accept': 'application/json'
                }
            });
            setModalVisible(true);
        } catch (error) {
            setError('Error updating profile');
            console.error('Error updating profile:', error.response ? error.response.data : error.message);
        }
    };

    if (loading) return <div className="flex justify-center items-center h-screen">Loading...</div>;
    if (error) return <div className="flex justify-center items-center h-screen">{error}</div>;

    return (
        <div className="bg-gray-100 pt-10 pb-10 px-5">
            <Modal show={modalVisible} onClose={() => setModalVisible(false)} message="Profile updated successfully!" />
            <div className="max-w-4xl mx-auto bg-white rounded-xl shadow-md overflow-hidden">
                <div className="flex flex-col md:flex-row">
                    <div className="w-full md:w-1/3 p-5 text-center bg-gray-200">
                        <Icon className="text-3xl mb-4 cursor-pointer" icon="ic:baseline-arrow-back" />
                        <img alt="avatar" src="https://static.vecteezy.com/system/resources/thumbnails/036/280/651/small/default-avatar-profile-icon-social-media-user-image-gray-avatar-icon-blank-profile-silhouette-illustration-vector.jpg" className="rounded-full mb-4 w-24 h-24 mx-auto border" />
                        <div className="font-semibold mb-2 text-lg">{profile.firstName}{''} {profile.lastName}</div>
                        <div className="font-medium text-sm text-gray-600 mb-2">{profile.email}</div>
                        <div className="font-medium">
                            <a href="https://localhost:44447/changepassword" className="text-blue-500 hover:underline">Change password</a>
                        </div>
                    </div>
                    <div className="w-full md:w-2/3 p-5">
                        <div className="mb-5 text-2xl font-black">MY PROFILE</div>
                        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                            {[
                                { label: 'First name', name: 'firstName', value: profile.firstName },
                                { label: 'Last name', name: 'lastName', value: profile.lastName },
                                { label: 'Username', name: 'userName', value: profile.userName, readOnly: true },
                                { label: 'Gender', name: 'gender', value: profile.gender, isSelect: true },
                                { label: 'Date of birth', name: 'dateOfBirth', value: profile.dateOfBirth, type: 'date' },
                                { label: 'Role', name: 'roles', value: profile.roles, readOnly: true },
                                { label: 'Phone Number', name: 'phoneNumber', value: profile.phoneNumber },
                                { label: 'Email', name: 'email', value: profile.email, readOnly: true }
                            ].map(({ label, name, value, type = 'text', readOnly = false, isSelect = false }) => (
                                <div key={name} className="mb-4">
                                    <label className="block text-gray-600 text-sm mb-2">{label}</label>
                                    {isSelect ? (
                                        <select name={name} value={value} onChange={handleChange} className="w-full border rounded-md py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline">
                                            <option value="Other">Other</option>
                                            <option value="Male">Male</option>
                                            <option value="Female">Female</option>
                                        </select>
                                    ) : (
                                        <input name={name} value={value} onChange={handleChange} type={type} readOnly={readOnly} className="w-full border rounded-md py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline" />
                                    )}
                                </div>
                            ))}
                        </div>
                        <button onClick={updateProfile} className="mt-5 px-4 py-2 rounded-lg bg-green-500 text-white font-medium hover:bg-green-400">Save Profile</button>
                    </div>
                </div>
            </div>
        </div>
    );
};
