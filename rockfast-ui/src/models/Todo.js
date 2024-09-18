export default class Todo {
    constructor(id, name, dateCreated, complete, dateCompleted, userId) {
      this.id = id;
      this.name = name;
      this.dateCreated = dateCreated;
      this.complete = complete;
      this.dateCompleted = dateCompleted;
      this.userId = userId;
    }
  }
  