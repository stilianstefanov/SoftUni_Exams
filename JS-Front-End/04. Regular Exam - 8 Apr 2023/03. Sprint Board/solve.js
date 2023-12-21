function attachEvents() {
    const baseURL = 'http://localhost:3030/jsonstore/tasks/';

    let loadButton = document.getElementById('load-board-btn');
    let createButton = document.getElementById('create-task-btn');

    let titleInput = document.getElementById('title');
    let descriptionInput = document.getElementById('description');

    let toDoList = document.querySelector('#todo-section ul');
    let inProgressList = document.querySelector('#in-progress-section ul');
    let codeReviewList = document.querySelector('#code-review-section ul');
    let doneList = document.querySelector('#done-section ul');

    loadButton.addEventListener('click', loadTasks);
    createButton.addEventListener('click', createTask);

    async function loadTasks() {
        toDoList.innerHTML = '';
        inProgressList.innerHTML = '';
        codeReviewList.innerHTML = '';
        doneList.innerHTML = '';

        let response = await fetch(baseURL);
        let tasks = await response.json();

        for (const task of Object.values(tasks)) {
            let wrapper = document.createElement('li');
            wrapper.classList.add('task');

            let title = document.createElement('h3');
            title.textContent = task.title;
            wrapper.appendChild(title);

            let description = document.createElement('p');
            description.textContent = task.description;
            wrapper.appendChild(description)

            let button = document.createElement('button');
            button.textContent = getButtonText(task.status);
            button.value = task._id;
            wrapper.appendChild(button);
            button.addEventListener('click', (e) => moveOrClose(e, task.status));

            if (task.status === 'ToDo') {
                toDoList.appendChild(wrapper);

            } else if (task.status === 'In Progress') {
                inProgressList.appendChild(wrapper);

            } else if (task.status === 'Code Review') {
                codeReviewList.appendChild(wrapper);

            } else {
                doneList.appendChild(wrapper);
            }
        }
    }

    async function moveOrClose(event, status) {
        let taskId = event.currentTarget.value;

        let updatedStatus = '';
        if (status === 'ToDo') {
            updatedStatus = 'In Progress';

        } else if (status === 'In Progress') {
            updatedStatus = 'Code Review';

        } else if (status === 'Code Review') {
            updatedStatus = 'Done'

        } else {
            await fetch(baseURL + taskId, {
                method: 'DELETE'
            });

            await loadTasks();
            return;
        }

        await fetch(baseURL + taskId, {
            method: 'PATCH',
            body: JSON.stringify({
                status: updatedStatus
            })
        });

        await loadTasks();
    }

    async function createTask(event) {
        event.preventDefault();

        let title = titleInput.value;
        let description = descriptionInput.value;
        let status = 'ToDo';

        let newTask = {
            title,
            description,
            status
        };

        await fetch(baseURL, {
            method: 'POST',
            body: JSON.stringify(newTask)
        });

        clearInputs();
        await loadTasks();
    }

    function clearInputs() {
        titleInput.value = '';
        descriptionInput.value = '';
    }

    function getButtonText(status) {
        if (status === 'ToDo') {
            return 'Move to In Progress';

        } else if (status === 'In Progress') {
            return 'Move to Code Review';

        } else if (status === 'Code Review') {
            return 'Move to Done';

        } else {
            return 'Close';
        }
    }
}

attachEvents();