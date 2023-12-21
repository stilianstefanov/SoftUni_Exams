function attachEvents() {
    let baseURL = 'http://localhost:3030/jsonstore/tasks/';

    let titleInput = document.getElementById('course-name');
    let typeInput = document.getElementById('course-type');
    let descriptionInput = document.getElementById('description');
    let teacherInput = document.getElementById('teacher-name');

    let addButton = document.getElementById('add-course');
    let editButton = document.getElementById('edit-course');

    let courseList = document.getElementById('list');
    let loadButton = document.getElementById('load-course');

    loadButton.addEventListener('click', loadCourses);
    addButton.addEventListener('click', addCourse);
    editButton.addEventListener('click', editCourse);

    async function loadCourses() {
        courseList.innerHTML = '';
        editButton.disabled = true;

        let response = await fetch(baseURL);
        let courses = await response.json();

        for (const course of Object.values(courses)) {
            let container = document.createElement('div');
            container.classList.add('container');
            courseList.appendChild(container);

            let title = document.createElement('h2');
            title.textContent = course.title;
            container.appendChild(title);

            let teacher = document.createElement('h3');
            teacher.textContent = course.teacher;
            container.appendChild(teacher);

            let type = document.createElement('h3');
            type.textContent = course.type;
            container.appendChild(type);

            let description = document.createElement('h4');
            description.textContent = course.description;
            container.appendChild(description);

            let editButton = document.createElement('button');
            editButton.classList.add('edit-btn');
            editButton.textContent = 'Edit Course';
            editButton.value = course._id;
            container.appendChild(editButton);
            editButton.addEventListener('click', fillForm);

            let finishButton = document.createElement('button');
            finishButton.classList.add('finish-btn');
            finishButton.textContent = 'Finish Course';
            finishButton.value = course._id;
            container.appendChild(finishButton);
            finishButton.addEventListener('click', deleteCourse);
        }       
    }

    async function addCourse(event) {
        event.preventDefault();

        let title = titleInput.value;
        let type = typeInput.value;
        let teacher = teacherInput.value;
        let description = descriptionInput.value;

        let newCourse = {
            title,
            type,
            teacher,
            description
        };

        await fetch(baseURL, {
            method: 'POST',
            body: JSON.stringify(newCourse)
        });

        titleInput.value = '';
        typeInput.value = '';
        teacherInput.value = '';
        descriptionInput.value = '';
        await loadCourses();
    }

    function fillForm(event) {
        let courseId = event.currentTarget.value;
        let container = event.currentTarget.parentElement;

        let title = container.children[0].textContent;
        let teacher = container.children[1].textContent;
        let type = container.children[2].textContent;
        let description = container.children[3].textContent;

        titleInput.value = title;
        typeInput.value = type;
        teacherInput.value = teacher;
        descriptionInput.value = description;
        editButton.value = courseId;

        container.remove();
        editButton.disabled = false;
        addButton.disabled = true;
    }

    async function editCourse(event) {
        event.preventDefault();

        let courseId = event.currentTarget.value;
        let title = titleInput.value;
        let type = typeInput.value;
        let teacher = teacherInput.value;
        let description = descriptionInput.value;

        let editedCourse = {
            title,
            type,
            teacher,
            description
        };

        await fetch(baseURL + courseId, {
            method: 'PUT',
            body: JSON.stringify(editedCourse)
        });

        editButton.disabled = true;
        addButton.disabled = false;
        titleInput.value = '';
        typeInput.value = '';
        teacherInput.value = '';
        descriptionInput.value = '';
        await loadCourses();
    }

    async function deleteCourse(event) {
        let courseId = event.currentTarget.value;

        await fetch(baseURL + courseId, {
            method: 'DELETE'
        });

        await loadCourses();
    }
}

attachEvents();