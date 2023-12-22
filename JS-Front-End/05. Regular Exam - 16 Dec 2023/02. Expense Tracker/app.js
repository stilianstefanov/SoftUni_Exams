window.addEventListener("load", solve);

function solve() {
    let expenseInput = document.getElementById('expense');
    let amountInput = document.getElementById('amount');
    let dateInput = document.getElementById('date');

    let addButton = document.getElementById('add-btn');

    let previewList = document.getElementById('preview-list');
    let expenseList = document.getElementById('expenses-list');

    let clearButton = document.querySelector('.delete');

    addButton.addEventListener('click', addToPreview);
    clearButton.addEventListener('click', reloadApp);

    function addToPreview() {
        if (!expenseInput.value || !amountInput.value || !dateInput.value) return;

        let wrapper = document.createElement('li');
        wrapper.classList.add('expense-item');
        previewList.appendChild(wrapper);

        let article = document.createElement('article');
        wrapper.appendChild(article);

        let expense = document.createElement('p');
        expense.textContent = `Type: ${expenseInput.value}`;
        article.appendChild(expense);

        let amount = document.createElement('p');
        amount.textContent = `Amount: ${amountInput.value}$`;
        article.appendChild(amount);

        let date = document.createElement('p');
        date.textContent = `Date: ${dateInput.value}`;
        article.appendChild(date);

        let buttonsContainer = document.createElement('div');
        buttonsContainer.classList.add('buttons');
        wrapper.appendChild(buttonsContainer);

        let editButton = document.createElement('button');
        editButton.classList.add('btn');
        editButton.classList.add('edit');
        editButton.textContent = 'edit';
        buttonsContainer.appendChild(editButton);
        editButton.addEventListener('click', editExpense);

        let okButton = document.createElement('button');
        okButton.classList.add('btn');
        okButton.classList.add('ok');
        okButton.textContent = 'ok';
        buttonsContainer.appendChild(okButton);
        okButton.addEventListener('click', addToExpenses);

        expenseInput.value = '';
        amountInput.value = '';
        dateInput.value = '';
        addButton.disabled = true;
    }

    function editExpense(event) {
        let wrapper = event.currentTarget.parentElement.parentElement;
        let article = wrapper.children[0];

        let expense = article.children[0].textContent.split(': ')[1];
        let amount = article.children[1].textContent.split(': ')[1].slice(0, -1);
        let date = article.children[2].textContent.split(': ')[1];

        expenseInput.value = expense;
        amountInput.value = amount;
        dateInput.value = date;
        addButton.disabled = false;
        wrapper.remove();
    }

    function addToExpenses(event) {
        let wrapper = event.currentTarget.parentElement.parentElement;
        let buttonsDiv = wrapper.children[1];

        addButton.disabled = false;
        buttonsDiv.remove();

        expenseList.appendChild(wrapper);
        previewList.removeChild(wrapper);
    }

    function reloadApp() {
        location.reload();
    }
}