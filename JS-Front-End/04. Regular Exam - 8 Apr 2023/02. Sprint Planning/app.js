window.addEventListener('load', solve);

function solve() {
    let totalPoints = 0;

    let taskIdHiddenInput = document.getElementById('task-id');
    let titleInput = document.getElementById('title');
    let descriptionInput = document.getElementById('description');
    let labelSelect = document.getElementById('label');
    let pointsInput = document.getElementById('points');
    let assigneeInput = document.getElementById('assignee');

    let createButton = document.getElementById('create-task-btn');
    let deleteButton = document.getElementById('delete-task-btn');

    let totalPointsElement = document.getElementById('total-sprint-points');
    let tasksSectionElement = document.getElementById('tasks-section');

    let taskNo = 1;
    createButton.addEventListener('click', createTask);
    deleteButton.addEventListener('click', deleteTask);

    function createTask() {
        if (!titleInput.value || !descriptionInput.value || !labelSelect.value || !pointsInput.value || !assigneeInput.value) return;

        deleteButton.disabled = true;
        
        let newTaskId = `task-${taskNo}`;
        taskNo++;

        let article = document.createElement('article');
        article.id = newTaskId;
        article.classList.add('task-card');
        tasksSectionElement.appendChild(article);

        let labelDiv = createLabel(labelSelect.value);
        article.appendChild(labelDiv);

        let title = document.createElement('h3');
        title.classList.add('task-card-title');
        title.textContent = titleInput.value;
        article.appendChild(title);

        let description = document.createElement('p');
        description.classList.add('task-card-description');
        description.textContent = descriptionInput.value;
        article.appendChild(description);

        let points = document.createElement('div');
        points.classList.add('task-card-points');
        points.textContent = `Estimated at ${pointsInput.value} pts`;
        article.appendChild(points);

        let assignee = document.createElement('div');
        assignee.classList.add('task-card-assignee');
        assignee.textContent = `Assigned to: ${assigneeInput.value}`;
        article.appendChild(assignee);

        let actionsDiv = document.createElement('div');
        actionsDiv.classList.add('task-card-actions');
        article.appendChild(actionsDiv);

        let deleteCurrentButton = document.createElement('button');
        deleteCurrentButton.textContent = 'Delete';
        actionsDiv.appendChild(deleteCurrentButton);
        deleteCurrentButton.addEventListener('click', fillForm);

        updateTotalPoints(pointsInput.value, 'add');
        clearInputFields();
    }

    function fillForm(event) {
        let article = event.currentTarget.parentElement.parentElement;

        let label = article.children[0].textContent.split(' ')[0];
        let title = article.children[1].textContent;
        let description = article.children[2].textContent;
        let points = article.children[3].textContent.split(' ')[2];
        let assignee = article.children[4].textContent.split(': ')[1];
        let id = article.id;

        deleteButton.disabled = false;
        createButton.disabled = true;

        labelSelect.selectedIndex = getOptionIndex(label);
        titleInput.value = title;
        descriptionInput.value = description;
        pointsInput.value = points;
        assigneeInput.value = assignee;
        taskIdHiddenInput.value = id;

        disableInputs();
    }

    function deleteTask() {
        let articleId = taskIdHiddenInput.value;
        let article = document.getElementById(articleId);
        let points = article.children[3].textContent.split(' ')[2];

        article.remove();
        clearInputFields();
        enableInputs();

        deleteButton.disabled = true;
        createButton.disabled = false;
        updateTotalPoints(points, 'subtract');
    }

    function updateTotalPoints(points, operation) {
        if (operation === 'add') {
            totalPoints += Number(points);

        } else {
            totalPoints -= Number(points);
        }

        totalPointsElement.innerText = `Total Points ${totalPoints}pts`;
    }

    function getOptionIndex(label) {
        if (label.includes('Feature')) {
            return 0;

        } else if (label.includes('Low')) {
            return 1;

        } else {
            return 2;
        }
    }

    function disableInputs() {
        labelSelect.disabled = true;
        titleInput.disabled = true;
        descriptionInput.disabled = true;
        pointsInput.disabled = true;
        assigneeInput.disabled = true;
    }

    function enableInputs() {
        labelSelect.disabled = false;
        titleInput.disabled = false;
        descriptionInput.disabled = false;
        pointsInput.disabled = false;
        assigneeInput.disabled = false;
    }

    function createLabel(label) {
        let labelDiv = document.createElement('div');
        labelDiv.classList.add('task-card-label');

        if (label === 'Feature') {
            labelDiv.innerHTML = `${label} &#8865`;
            labelDiv.classList.add('feature');

        } else if (label === 'Low Priority Bug') {
            labelDiv.innerHTML = `${label} &#9737`;
            labelDiv.classList.add('low-priority');

        } else if (label === 'High Priority Bug') {
            labelDiv.innerHTML = `${label} &#9888`;
            labelDiv.classList.add('high-priority');

        }
        return labelDiv;
    }

    function clearInputFields() {
        titleInput.value = '';
        descriptionInput.value = '';
        labelSelect.value = '';
        pointsInput.value = '';
        assigneeInput.value = '';
        taskIdHiddenInput.value = '';
    }
}