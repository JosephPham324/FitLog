import React, { useEffect, useState } from 'react';
import axiosInstance from '../utils/axiosInstance';
import { Link } from 'react-router-dom';

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
    const [initialProfile, setInitialProfile] = useState({
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
    const [errors, setErrors] = useState({});
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchProfile = async () => {
            try {
                const response = await axiosInstance.get('/Users/user-profile');
                const { id, firstName, lastName, userName, gender, dateOfBirth, roles, phoneNumber, email } = response.data;

                const initialProfileData = {
                    id: id || '',
                    firstName: firstName || '',
                    lastName: lastName || '',
                    userName: userName || '',
                    gender: gender || '',
                    dateOfBirth: dateOfBirth ? dateOfBirth.split('T')[0] : '',
                    roles: roles || '',
                    phoneNumber: phoneNumber || '',
                    email: email || ''
                };

                setProfile(initialProfileData);
                setInitialProfile(initialProfileData);
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
        const { name, value } = e.target;
        setProfile((prevProfile) => ({
            ...prevProfile,
            [name]: value,
        }));
    };

    const updateProfile = async () => {
        const newErrors = {};

        // Kiểm tra các trường bắt buộc
        if (!profile.firstName.trim()) {
            newErrors.firstName = 'First name is required.';
        }
        if (!profile.lastName.trim()) {
            newErrors.lastName = 'Last name is required.';
        }
        if (!profile.dateOfBirth.trim()) {
            newErrors.dateOfBirth = 'Date of birth is required.';
        }
        if (!profile.phoneNumber.trim()) {
            newErrors.phoneNumber = 'Phone number is required.';
        }

        // Nếu có lỗi, cập nhật state `errors` và dừng quá trình cập nhật
        if (Object.keys(newErrors).length > 0) {
            setErrors(newErrors);
            return;
        }

        try {
            // Khởi tạo payload với tất cả các trường cần thiết, kể cả những trường không thay đổi
            const payload = {
                UserId: profile.id,
                FirstName: profile.firstName,
                LastName: profile.lastName,
                UserName: profile.userName,
                Gender: profile.gender,
                DateOfBirth: profile.dateOfBirth,
                Roles: profile.roles,
                PhoneNumber: profile.phoneNumber,
                Email: profile.email, // Gửi email hiện tại (cũ)
            };

            console.log('Payload being sent to API:', payload);

            await axiosInstance.put('/Users/update-profile', payload);
            alert('Profile updated successfully!');
        } catch (error) {
            setError('Error updating profile');
            console.error('Error updating profile:', error.response ? error.response.data : error.message);
        }
    };

    if (loading) return <div className="flex justify-center items-center h-screen">Loading...</div>;
    if (error) return <div className="flex justify-center items-center h-screen">{error}</div>;

    const token = localStorage.getItem('authToken');

    return (
        <div className="bg-gray-100 pt-10 pb-10 px-5">
            <div className="max-w-4xl mx-auto bg-white rounded-xl shadow-md overflow-hidden">
                <div className="flex flex-col md:flex-row">
                    <div className="w-full md:w-1/3 p-5 text-center bg-gray-200">
                        <img alt="avatar" src="https://static.vecteezy.com/system/resources/thumbnails/036/280/651/small/default-avatar-profile-icon-social-media-user-image-gray-avatar-icon-blank-profile-silhouette-illustration-vector.jpg" className="rounded-full mb-4 w-24 h-24 mx-auto border" />
                        <div className="font-semibold mb-2 text-lg">{profile.firstName} {profile.lastName}</div>
                        <div className="font-medium text-sm text-gray-600 mb-2">{profile.email}</div>
                        <div className="font-medium">
                            <Link to={`/changepassword?token=${token}&email=${profile.email}`} className="text-blue-500 hover:underline">Change password</Link>
                        </div>
                    </div>
                    <div className="w-full md:w-2/3 p-5">
                        <div className="mb-5 text-2xl font-black">MY PROFILE</div>
                        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                            {[
                                { label: 'First name', name: 'firstName', value: profile.firstName, error: errors.firstName },
                                { label: 'Last name', name: 'lastName', value: profile.lastName, error: errors.lastName },
                                { label: 'Username', name: 'userName', value: profile.userName, readOnly: true, grayBackground: true },
                                { label: 'Gender', name: 'gender', value: profile.gender, isSelect: true },
                                { label: 'Date of birth', name: 'dateOfBirth', value: profile.dateOfBirth, type: 'date', error: errors.dateOfBirth },
                                { label: 'Role', name: 'roles', value: profile.roles, readOnly: true, grayBackground: true },
                                { label: 'Phone Number', name: 'phoneNumber', value: profile.phoneNumber, error: errors.phoneNumber },
                                { label: 'Email', name: 'email', value: profile.email, readOnly: true, grayBackground: true }
                            ].map(({ label, name, value, type = 'text', readOnly = false, isSelect = false, grayBackground = false, error }) => (
                                <div key={name} className="mb-4">
                                    <label className="block text-gray-600 text-sm mb-2">{label}</label>
                                    {isSelect ? (
                                        <select name={name} value={value} onChange={handleChange} className="w-full border rounded-md py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline">
                                            <option value="Other">Other</option>
                                            <option value="Male">Male</option>
                                            <option value="Female">Female</option>
                                        </select>
                                    ) : (
                                        <input
                                            name={name}
                                            value={value}
                                            onChange={handleChange}
                                            type={type}
                                            readOnly={readOnly}
                                            className={`w-full border rounded-md py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline ${grayBackground ? 'bg-gray-200' : ''}`}
                                        />
                                    )}
                                    {error && <p className="text-red-500 text-xs mt-2">{error}</p>}
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
