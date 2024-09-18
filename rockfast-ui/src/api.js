import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5086/', 
});

export default api;
