window.addEventListener("load", solve);

function solve() {
    let titleInput = document.getElementById('task-title');
    let categoryInput = document.getElementById('task-category');
    let contentInput = document.getElementById('task-content');
    let publishButton = document.getElementById('publish-btn');

    let reviewList = document.getElementById('review-list');
    let publishedList = document.getElementById('published-list');

    publishButton.addEventListener('click', addToReview);

    function addToReview() {
        if (!titleInput.value || !contentInput.value || !categoryInput.value) return;

        let wrapper = document.createElement('li');
        wrapper.classList.add('rpost');
        reviewList.appendChild(wrapper);

        let article = document.createElement('article');
        wrapper.appendChild(article);

        let title = document.createElement('h4');
        title.textContent = titleInput.value;
        article.appendChild(title);

        let category = document.createElement('p');
        category.textContent = `Category: ${categoryInput.value}`;
        article.appendChild(category);

        let content = document.createElement('p');
        content.textContent = `Content: ${contentInput.value}`;
        article.appendChild(content);

        let editButton = document.createElement('button');
        editButton.classList.add('action-btn');
        editButton.classList.add('edit');
        editButton.textContent = 'Edit';
        wrapper.appendChild(editButton);
        editButton.addEventListener('click', editTask);

        let postButton = document.createElement('button');
        postButton.classList.add('action-btn');
        postButton.classList.add('post');
        postButton.textContent = 'Post';
        wrapper.appendChild(postButton);
        postButton.addEventListener('click', postTask);

        titleInput.value = '';
        categoryInput.value = '';
        contentInput.value = '';
    }

    function editTask(event) {
        let wrapper = event.currentTarget.parentElement;
        let article = wrapper.children[0];
        
        let title = article.children[0].textContent;
        let category = article.children[1].textContent.split(': ')[1];
        let content = article.children[2].textContent.split(': ')[1];

        titleInput.value = title;
        categoryInput.value = category;
        contentInput.value = content;
        wrapper.remove();
    }

    function postTask(event) {
        let wrapper = event.currentTarget.parentElement;
        let postButton = event.currentTarget;
        let editButton = wrapper.children[1];

        postButton.remove();
        editButton.remove();

        publishedList.appendChild(wrapper);
        reviewList.removeChild(wrapper);
    }
}