import React, { useState, useRef } from 'react';
import { Container, Row, Col, Card } from 'react-bootstrap';
import UsersList from './components/UsersList';
import TodosList from './components/TodosList';
import TodoDetails from './components/TodoDetails';
import 'bootstrap/dist/css/bootstrap.min.css';

function App() {
  const [selectedUser, setSelectedUser] = useState(null);
  const [selectedTodo, setSelectedTodo] = useState(null);

  const todosListRef = useRef(null);

  const handleUserSelect = user => {
    setSelectedUser(user);
    setSelectedTodo(null);
  };

  const handleTodoSelect = todo => {
    setSelectedTodo(todo);
  };

  const handleAddTodo = () => {
    if (selectedUser) {
      setSelectedTodo({
        id: 0,
        name: '',
        complete: false,
        dateCompleted: '',
        userId: selectedUser.id,
      });
    } else {
      alert('Please select a user first.');
    }
  };

  const handleSaveTodo = savedTodo => {
    setSelectedTodo(null);

    if (todosListRef.current) {
      todosListRef.current.refreshTodos();
    }
  };

  const handleDeleteTodo = deletedTodoId => {
    setSelectedTodo(null);

    if (todosListRef.current) {
      todosListRef.current.refreshTodos();
    }
  };

  const handleCancel = () => {
    setSelectedTodo(null);
  };

  return (
    <Container className="mt-4">

      <Row className="mb-4">

        <Col md={4}>
          <Card>
            <Card.Header>Users</Card.Header>
            <Card.Body>
              <UsersList onUserSelect={handleUserSelect} selectedUser={selectedUser} />
            </Card.Body>
          </Card>
        </Col>
        <Col md={2} />
        <Col md={5}>
          <Card>
            <Card.Header>Todos</Card.Header>
            <Card.Body>
              {selectedUser ? (
                <TodosList
                  ref={todosListRef}
                  user={selectedUser}
                  onTodoSelect={handleTodoSelect}
                  onAddTodo={handleAddTodo}
                  selectedTodo={selectedTodo}
                />
              ) : (
                <p>Select a user to view their todos.</p>
              )}
            </Card.Body>
          </Card>
        </Col>
      </Row>


      <Row>
        <Col>
          {selectedTodo && selectedUser && (
            <Card>
              <Card.Header>
                {selectedTodo.id !== 0
                  ? `Edit Todo for ${selectedUser.username}`
                  : `Add Todo for ${selectedUser.username}`}
              </Card.Header>
              <Card.Body>
                <TodoDetails
                  todo={selectedTodo}
                  onSave={handleSaveTodo}
                  onDelete={handleDeleteTodo}
                  onCancel={handleCancel}
                  selectedUser={selectedUser}
                />
              </Card.Body>
            </Card>
          )}
        </Col>
      </Row>
    </Container>
  );
}

export default App;
