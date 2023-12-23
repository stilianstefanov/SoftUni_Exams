function attachEvents() {
    const baseURL = 'http://localhost:3030/jsonstore/tasks/';

    let foodInput = document.getElementById('food');
    let timeInput = document.getElementById('time');
    let caloriesInput = document.getElementById('calories');

    let addButton = document.getElementById('add-meal');
    let editButton = document.getElementById('edit-meal');
    let loadButton = document.getElementById('load-meals');

    let foodList = document.getElementById('list');

    loadButton.addEventListener('click', loadMeals);
    addButton.addEventListener('click', addFood);
    editButton.addEventListener('click', editFood);

    async function loadMeals() {
        editButton.disabled = true;
        foodList.innerHTML = '';

        let response = await fetch(baseURL);
        let foods = await response.json();

        for (const food of Object.values(foods)) {
            let container = document.createElement('div');
            container.classList.add('meal');
            foodList.appendChild(container);

            let foodName = document.createElement('h2');
            foodName.textContent = food.food;
            container.appendChild(foodName);

            let time = document.createElement('h3');
            time.textContent = food.time;
            container.appendChild(time);

            let calories = document.createElement('h3');
            calories.textContent = food.calories;
            container.appendChild(calories);

            let buttonsContainer = document.createElement('div');
            buttonsContainer.id = 'meal-buttons';
            container.appendChild(buttonsContainer);

            let changeButton = document.createElement('button');
            changeButton.classList.add('change-meal');
            changeButton.textContent = 'Change';
            changeButton.value = food._id;
            buttonsContainer.appendChild(changeButton);
            changeButton.addEventListener('click', fillForm);

            let deleteButton = document.createElement('button');
            deleteButton.classList.add('delete-meal');
            deleteButton.textContent = 'Delete';
            deleteButton.value = food._id;
            buttonsContainer.appendChild(deleteButton);
            deleteButton.addEventListener('click', deleteFood);
        }
    }

    async function addFood(event) {
        event.preventDefault();

        let food = foodInput.value;
        let calories = caloriesInput.value;
        let time = timeInput.value;

        let newFood = {
            food,
            calories,
            time
        };

        await fetch(baseURL, {
            method: 'POST',
            body: JSON.stringify(newFood)
        });

        foodInput.value = '';
        caloriesInput.value = '';
        timeInput.value = '';
        await loadMeals();
    }

    function fillForm(event) {
        let changeButton = event.currentTarget;
        let foodId = changeButton.value;
        let container = changeButton.parentElement.parentElement;

        let foodName = container.children[0].textContent;
        let time = container.children[1].textContent;
        let calories = container.children[2].textContent;

        foodInput.value = foodName;
        timeInput.value = time;
        caloriesInput.value = calories;
        editButton.value = foodId;

        container.remove();
        editButton.disabled = false;
        addButton.disabled = true;
    }

    async function editFood(event) {
        event.preventDefault();

        let foodId = event.currentTarget.value;
        let food = foodInput.value;
        let time = timeInput.value;
        let calories = caloriesInput.value;

        let updatedFood = {
            food,
            time,
            calories
        };

        await fetch (baseURL + foodId, {
            method: 'PUT',
            body: JSON.stringify(updatedFood)
        });

        foodInput.value = '';
        timeInput.value = '';
        caloriesInput.value = '';
        editButton.disabled = true;
        addButton.disabled = false;
        await loadMeals();
    }

    async function deleteFood(event) {
        let foodId = event.currentTarget.value;

        await fetch(baseURL + foodId, {
            method: 'DELETE'
        });

        await loadMeals();
    }
}

attachEvents();