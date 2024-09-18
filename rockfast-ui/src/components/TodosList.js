import React, { useEffect, useState, useImperativeHandle, forwardRef } from 'react';
import api from '../api';
import Todo from '../models/Todo';
import { Button, ListGroup } from 'react-bootstrap';

const TodosList = forwardRef(({ user, onTodoSelect, onAddTodo, selectedTodo }, ref) => {
    const [todos, setTodos] = useState([]);

    const fetchTodos = () => {
        if (user) {
            api.get(`/todos?userId=${user.id}`)

                .then(response => {
                    console.log('Fetched todos:', response.data);
                    const todosData = response.data.map(todoData => new Todo(
                        todoData.id,
                        todoData.name,
                        todoData.dateCreated,
                        todoData.complete,
                        todoData.dateCompleted,
                        todoData.userId
                    ));
                    setTodos(todosData);
                })
                .catch(error => {
                    console.error('Error fetching todos:', error);
                });
        } else {
            setTodos([]);
        }
    };

    useEffect(() => {
        fetchTodos();
    }, [user]);

    useImperativeHandle(ref, () => ({
        refreshTodos() {
            fetchTodos();
        }
    }));

    return (
        <div>
            {user ? (
                <>
                    <Button variant="primary" onClick={onAddTodo} className="mb-2">Add Todo</Button>
                    <ListGroup>
                        {todos.map(todo => (
                            <ListGroup.Item
                                key={todo.id}
                                active={selectedTodo && selectedTodo.id === todo.id}
                                onClick={() => onTodoSelect(todo)}
                                style={{ cursor: 'pointer' }}
                            >
                                {todo.name}
                            </ListGroup.Item>
                        ))}
                    </ListGroup>
                </>
            ) : (
                <p>Select a user to view their todos.</p>
            )}
        </div>
    );
}
);

export default TodosList;
