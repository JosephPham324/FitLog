import React, { useEffect, useState } from 'react';
import axiosInstance from '../utils/axiosInstance'; // Import the configured Axios instance

const UserListPage = () => {
    const [users, setUsers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchUsers = async () => {
            try {
                const response = await axiosInstance.get(`${process.env.REACT_APP_BACKEND_URL}/Users/all`, {
                    params: {
                        PageNumber: 1,
                        PageSize: 10,
                    },
                    headers: {
                        accept: 'application/json',
                    },
                });
                setUsers(response.data.items);
            } catch (error) {
                setError('Error fetching users');
                console.error('Error fetching users:', error);
            } finally {
                setLoading(false);
            }
        };

        fetchUsers();
    }, []);

    if (loading) {
        return <div>Loading...</div>;
    }

    if (error) {
        return <div>{error}</div>;
    }

    return (
        <div>
            <h1>User List</h1>
            {users.length > 0 ? (
                <ul>
                    {users.map((user) => (
                        <li key={user.id}>
                            <p>Username: {user.userName}</p>
                            <p>Email: {user.email}</p>
                            <p>Email Confirmed: {user.emailConfirmed ? 'Yes' : 'No'}</p>
                            <p>Phone Number: {user.phoneNumber || 'N/A'}</p>
                            <p>Phone Number Confirmed: {user.phoneNumberConfirmed ? 'Yes' : 'No'}</p>
                            <p>Lockout Enabled: {user.lockoutEnabled ? 'Yes' : 'No'}</p>
                            <p>Access Failed Count: {user.accessFailedCount}</p>
                        </li>
                    ))}
                </ul>
            ) : (
                <p>No users found.</p>
            )}
        </div>
    );
};

export default UserListPage;
