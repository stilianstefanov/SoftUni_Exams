function attachEvents() {
    const baseURL = 'http://localhost:3030/jsonstore/tasks/';

    let locationInput = document.getElementById('location');
    let temperatureInput = document.getElementById('temperature');
    let dateInput = document.getElementById('date');

    let addWeatherButton = document.getElementById('add-weather');
    let editWeatherButton = document.getElementById('edit-weather');
    let loadHistoryButton = document.getElementById('load-history');

    let weatherList = document.getElementById('list');


    loadHistoryButton.addEventListener('click', loadWeather);
    addWeatherButton.addEventListener('click', addWeatherRecord);
    editWeatherButton.addEventListener('click', editWeather);

    async function loadWeather() {
        editWeatherButton.disabled = true;
        weatherList.innerHTML = '';

        let response = await fetch(baseURL);
        let weatherHistory = await response.json();
        
        for (const day of Object.values(weatherHistory)) {
            let container = document.createElement('div');
            container.classList.add('container');
            weatherList.appendChild(container);

            let city = document.createElement('h2');
            city.textContent = day.location;
            container.appendChild(city);

            let date = document.createElement('h3');
            date.textContent = day.date;
            container.appendChild(date);

            let temp = document.createElement('h3');
            temp.id = 'celsius';
            temp.textContent = day.temperature;
            container.appendChild(temp);

            let buttonsContainer = document.createElement('div');
            buttonsContainer.classList.add('buttons-container');
            container.appendChild(buttonsContainer);

            let changeButton = document.createElement('button');
            changeButton.classList.add('change-btn');
            changeButton.textContent = 'Change';
            changeButton.value = day._id;
            buttonsContainer.appendChild(changeButton);
            changeButton.addEventListener('click', fillForm);

            let deleteButton = document.createElement('button');
            deleteButton.classList.add('delete-btn');
            deleteButton.textContent = 'Delete';
            deleteButton.value = day._id;
            buttonsContainer.appendChild(deleteButton);
            deleteButton.addEventListener('click', deleteRecord);
        }
    }

    async function addWeatherRecord(event) {
        event.preventDefault();
        let location = locationInput.value;
        let temperature = temperatureInput.value;
        let date = dateInput.value;

        if (!location || !temperature || !date) return;

        let newRecord = {
            location,
            temperature,
            date
        };

        await fetch(baseURL, {
            method: 'POST',
            body: JSON.stringify(newRecord)
        });

        locationInput.value = '';
        temperatureInput.value = '';
        dateInput.value = '';
        await loadWeather();
    }

    function fillForm(event) {
        let changeButton = event.currentTarget;
        let dayId = changeButton.value;
        let container = changeButton.parentElement.parentElement;
        let location = container.children[0].textContent;
        let date = container.children[1].textContent;
        let temperature = container.children[2].textContent;

        locationInput.value = location;
        dateInput.value = date;
        temperatureInput.value = temperature;
        editWeatherButton.value = dayId;

        container.remove();
        editWeatherButton.disabled = false;
        addWeatherButton.disabled = true;
    }

    async function editWeather(event) {
        event.preventDefault();
        let dayId = event.currentTarget.value;
        let location = locationInput.value;
        let date = dateInput.value;
        let temperature = temperatureInput.value;

        let updatedDay = {
            location,
            date,
            temperature
        };

        await fetch (baseURL + dayId, {
            method: 'PUT',
            body: JSON.stringify(updatedDay)
        });

        locationInput.value = '';
        temperatureInput.value = '';
        dateInput.value = '';
        editWeatherButton.disabled = true;
        addWeatherButton.disabled = false;
        await loadWeather();
    }

    async function deleteRecord(event) {
        let dayId = event.currentTarget.value;

        await fetch(baseURL + dayId, {
            method: 'DELETE'
        });
        await loadWeather();
    }
}

attachEvents();