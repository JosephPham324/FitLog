import React, { useEffect, useState } from 'react';
import { Icon } from "@iconify/react/dist/iconify.js";
import axiosInstance from '../utils/axiosInstance'; // Import the configured Axios instance

export const Profile = () => {
    const [profile, setProfile] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchProfile = async () => {
            try {
                const userId = 'your-user-id'; // Replace with actual userId
                const response = await axiosInstance.get('/Users/profile', {
                    params: { userId }
                });
                setProfile(response.data);
            } catch (error) {
                setError('Error fetching user profile');
                console.error('Error fetching user profile:', error);
            } finally {
                setLoading(false);
            }
        };

        fetchProfile();
    }, []);

    if (loading) return <div>Loading...</div>;
    if (error) return <div>{error}</div>;

    return (
        <>
            <div className="bg-neutral-300 pt-20 pb-10 px-10">
                <div className="flex">
                    <div className="w-1/4 text-center">
                        <Icon className="text-5xl" icon="ic:baseline-arrow-back" />
                        <img alt="avatar" src={"https://static.vecteezy.com/system/resources/thumbnails/036/280/651/small/default-avatar-profile-icon-social-media-user-image-gray-avatar-icon-blank-profile-silhouette-illustration-vector.jpg"} className="rounded-full mb-5 m-auto" />
                        <div className="font-bold mb-3 text-lg">{profile.firstName} {profile.lastName}</div>
                        <div className="font-bold text-neutral-500 mb-3 tracking-wider">
                            {profile.email}
                        </div>
                        <div className="font-bold mb-5">
                            <a href="https://localhost:44447/changepassword" className="text-blue-500 hover:underline">Change password</a>
                        </div>
                    </div>
                    <div className="w-3/4 rounded-xl bg-white px-5 pt-10 pb-3">
                        <div className="mb-5 text-4xl font-black">MY PROFILE</div>
                        <div className="grid grid-cols-2 gap-10">
                            <div>
                                <div className="pl-2 text-neutral-500 mb-1 font-medium text-sm">
                                    First name
                                </div>
                                <input value={profile.firstName} className="w-2/3 bg-neutral-300 text-neutral-600 py-3 px-2 font-medium rounded-xl" />
                            </div>
                            <div>
                                <div className="pl-2 text-neutral-500 mb-1 font-medium text-sm">
                                    Last name
                                </div>
                                <input value={profile.lastName} className="w-2/3 bg-neutral-300 text-neutral-600 py-3 px-2 font-medium rounded-xl" />
                            </div>
                            <div>
                                <div className="pl-2 text-neutral-500 mb-1 font-medium text-sm">
                                    Username
                                </div>
                                <input value={profile.userName} className="w-2/3 bg-neutral-300 text-neutral-600 py-3 px-2 font-medium rounded-xl" readOnly />
                            </div>
                            <div>
                                <div className="pl-2 text-neutral-500 mb-1 font-medium text-sm">
                                    Gender
                                </div>
                                <select value={profile.gender} className="w-2/3 bg-neutral-300 text-neutral-600 py-3 px-2 font-medium rounded-xl">
                                    <option value="Male">Orther</option>
                                    <option value="Male">Male</option>
                                    <option value="Female">Female</option>
                                </select>
                            </div>
                            <div>
                                <div className="pl-2 text-neutral-500 mb-1 font-medium text-sm">
                                    Date of birth
                                </div>
                                <input type="date" value={profile.dateOfBirth} className="w-2/3 bg-neutral-300 text-neutral-600 py-3 px-2 font-medium rounded-xl" />
                            </div>
                            <div>
                                <div className="pl-2 text-neutral-500 mb-1 font-medium text-sm">
                                    Role
                                </div>
                                <input value="Member" className="w-2/3 bg-neutral-300 text-neutral-600 py-3 px-2 font-medium rounded-xl" readOnly />
                            </div>
                        </div>
                        <div className="mt-5">
                            <div className="pl-2 text-neutral-500 mb-1 font-medium text-sm">
                                Number Phone
                            </div>
                            <input value={profile.phoneNumber} className="w-full bg-neutral-300 text-neutral-600 py-3 px-2 font-medium rounded-xl" />
                        </div>
                        <div className="mt-5">
                            <div className="pl-2 text-neutral-500 mb-1 font-medium text-sm">
                                Email
                            </div>
                            <input value={profile.email} className="w-full bg-neutral-300 text-neutral-600 py-3 px-2 font-medium rounded-xl" readOnly />
                        </div>

                        <button className="my-5 px-4 py-3 rounded-lg m-auto bg-green-600 text-white font-medium block hover:bg-green-500">Save Profile</button>
                    </div>
                </div>
            </div>
        </>
    );
};
