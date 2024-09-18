import React, { useState, useEffect } from 'react';
import api from '../api';

function TodoDetails({ todo, onSave, onDelete, onCancel, selectedUser }) {
    const [formData, setFormData] = useState({
        id: 0,
        name: '',
        complete: false,
        dateCompleted: '',
        userId: 0,
    });

    useEffect(() => {
        if (todo) {
            setFormData({
                id: todo.id || 0,
                name: todo.name || '',
                complete: todo.complete || false,
                dateCompleted: todo.dateCompleted ? todo.dateCompleted.split('T')[0] : '',
                userId: todo.userId || (selectedUser ? selectedUser.id : 0),
            });
        } else {
            setFormData(prevState => ({
                ...prevState,
                id: 0,
                userId: selectedUser ? selectedUser.id : 0,
            }));
        }
    }, [todo, selectedUser]);

    const handleChange = e => {
        const { name, value, type, checked } = e.target;
        setFormData(prevState => ({
            ...prevState,
            [name]: type === 'checkbox' ? checked : value,
        }));
    };

    const handleSubmit = e => {
        e.preventDefault();

        const submitData = {
            ...formData,
            dateCompleted: formData.dateCompleted || null,
        };

        if (submitData.id === 0) {
            
            api.post('/todos', submitData)
                .then(response => {
                    onSave(response.data);
                    alert('Todo created successfully!');
                })
                .catch(error => {
                    console.error('Error creating todo:', error);
                    const errorMessage = error.response?.data?.errors?.model?.[0] || 'Failed to create todo.';
                    alert(errorMessage);
                });
        } else {
           
            api.put(`/todos/${submitData.id}`, submitData)
                .then(response => {
                    onSave(response.data);
                    alert('Todo updated successfully!');
                })
                .catch(error => {
                    console.error('Error updating todo:', error);
                    const errorMessage = error.response?.data?.errors?.model?.[0] || 'Failed to update todo.';
                    alert(errorMessage);
                });
        }
    };

    const handleDelete = () => {
        if (formData.id !== 0) {
            api.delete(`/todos/${formData.id}?userId=${formData.userId}`)
                .then(() => {
                    onDelete(formData.id);
                    alert('Todo deleted successfully!');
                })
                .catch(error => {
                    console.error('Error deleting todo:', error);
                    alert('Failed to delete todo. Please try again.');
                });
        } else {
            onCancel();
        }
    };

    if (!todo && formData.id === 0 && !selectedUser) {
        return <p>Select a user to add a todo.</p>;
    }

    return (
        <div>
            
            <form onSubmit={handleSubmit}>
                <div className="mb-3">
                    <label htmlFor="name" className="form-label">Name</label>
                    <input
                        type="text"
                        className="form-control"
                        id="name"
                        name="name"
                        value={formData.name}
                        onChange={handleChange}
                        required
                    />
                </div>

                <div className="mb-3 form-check">
                    <input
                        type="checkbox"
                        className="form-check-input"
                        name="complete"
                        checked={formData.complete}
                        onChange={handleChange}
                        id="complete"
                    />
                    <label className="form-check-label" htmlFor="complete">Complete</label>
                </div>

                {formData.complete && (
                    <div className="mb-3">
                        <label htmlFor="dateCompleted" className="form-label">Date Completed</label>
                        <input
                            type="date"
                            className="form-control"
                            name="dateCompleted"
                            id="dateCompleted"
                            value={formData.dateCompleted}
                            onChange={handleChange}
                        />
                    </div>
                )}

                <button type="submit" className="btn btn-success me-2">Save</button>
                {formData.id !== 0 && (
                    <button type="button" className="btn btn-danger me-2" onClick={handleDelete}>Delete</button>
                )}
                <button type="button" className="btn btn-secondary" onClick={onCancel}>Cancel</button>
            </form>
        </div>
    );
}

export default TodoDetails;
