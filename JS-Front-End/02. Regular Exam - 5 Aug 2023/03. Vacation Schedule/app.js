function attachEvents() {
    const baseURL = 'http://localhost:3030/jsonstore/tasks/';
    let vacationsListDiv = document.getElementById('list');
    let loadVacationsButton = document.getElementById('load-vacations');

    let nameInput = document.getElementById('name');
    let daysNumInput = document.getElementById('num-days');
    let fromDateInput = document.getElementById('from-date');
    let addVacationButton = document.getElementById('add-vacation');
    let editVacationButton = document.getElementById('edit-vacation');

    loadVacationsButton.addEventListener('click', loadAllVacations);
    addVacationButton.addEventListener('click', addNewVacation);
    editVacationButton.addEventListener('click', editVacation);

    async function loadAllVacations() {
        vacationsListDiv.innerHTML = '';
        let response = await fetch(baseURL);
        let vacations = await response.json();

        for (const vacation of Object.values(vacations)) {
            let container = document.createElement('div');
            container.classList.add('container');
            vacationsListDiv.appendChild(container);

            let h2 = document.createElement('h2');
            h2.textContent = vacation.name;
            container.appendChild(h2);

            let dateh3 = document.createElement('h3');
            dateh3.textContent = vacation.date;
            container.appendChild(dateh3);

            let daysh3 = document.createElement('h3');
            daysh3.textContent = vacation.days;
            container.appendChild(daysh3);

            let changeButton = document.createElement('button');
            changeButton.classList.add('change-btn');
            changeButton.textContent = 'Change';
            changeButton.value = vacation._id;
            container.appendChild(changeButton);
            changeButton.addEventListener('click', changeVacationDetails);

            let doneButton = document.createElement('button');
            doneButton.classList.add('done-btn');
            doneButton.textContent = 'Done';
            container.appendChild(doneButton);
            doneButton.addEventListener('click', async () => {
                await fetch(baseURL + vacation._id, {
                    method: 'DELETE'
                });
                await loadAllVacations();
            });

            editVacationButton.disabled = true;
        }
    }

    async function addNewVacation(event) {
        event.preventDefault();
        let newVacation = {
            name: nameInput.value,
            days: daysNumInput.value,
            date: fromDateInput.value
        };
        
        await fetch(baseURL, {
            method: 'POST',
            body: JSON.stringify(newVacation)
        });       

        nameInput.value = '';
        daysNumInput.value = '';
        fromDateInput.value = '';
        await loadAllVacations();
    }

    function changeVacationDetails(event) {
        let vacationId = event.currentTarget.value;
        let container = event.currentTarget.parentElement;
        let name = container.children[0].textContent;
        let date = container.children[1].textContent;
        let days = container.children[2].textContent;

        container.remove();
        nameInput.value = name;
        fromDateInput.value = date;
        daysNumInput.value = days;
        editVacationButton.value = vacationId;

        editVacationButton.disabled = false;
        addVacationButton.disabled = true;
    }

    async function editVacation(event) {
        event.preventDefault();
        let editButton = event.currentTarget;
        let vacationId = editButton.value;

        let name = nameInput.value;
        let date = fromDateInput.value;
        let days = daysNumInput.value;

        let editedVacation = {
            name,
            days,
            date
        };

        await fetch(baseURL + vacationId, {
            method: 'PUT',
            body: JSON.stringify(editedVacation)
        });

        editButton.disabled = true;
        addVacationButton.disabled = false;
        nameInput.value = '';
        daysNumInput.value = '';
        fromDateInput.value = '';
        await loadAllVacations();
    }
}

attachEvents();