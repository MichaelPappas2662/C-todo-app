import React, { useEffect, useState } from 'react';
import api from '../api';
import User from '../models/User';

function UsersList({ onUserSelect, selectedUser }) {
  const [users, setUsers] = useState([]);

  useEffect(() => {
    api.get('/users')
      .then(response => {
        console.log('Fetched users:', response.data);
        const usersData = response.data.map(userData => new User(userData.id, userData.username));
        setUsers(usersData);
      })
      .catch(error => {
        console.error('Error fetching users:', error);
      });
  }, []);

  return (
    <div>

      <div className="card">
        <ul className="list-group list-group-flush">
          {users.map(user => (
            <li
              key={user.id}
              className={`list-group-item ${selectedUser && selectedUser.id === user.id ? 'active' : ''}`}
              onClick={() => onUserSelect(user)}
              style={{ cursor: 'pointer' }}
            >
              {user.username}
            </li>
          ))}
        </ul>
      </div>
    </div>
  );
}

export default UsersList;
