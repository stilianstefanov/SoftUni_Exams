window.addEventListener("load", solve);

function solve() {
    let studentNameInput = document.getElementById('student');
    let universityInput = document.getElementById('university');
    let scoreInput = document.getElementById('score');
    let nextButton = document.getElementById('next-btn');

    let previewList = document.getElementById('preview-list');
    let candidatesList = document.getElementById('candidates-list');

    nextButton.addEventListener('click', addToPreview);

    function addToPreview() {
      if (!studentNameInput.value || !universityInput.value || !scoreInput.value) return; 

      let li = document.createElement('li');
      li.classList.add('application');
      previewList.appendChild(li);

      let article = document.createElement('article');
      li.appendChild(article);

      let h4 = document.createElement('h4');
      h4.textContent = studentNameInput.value;
      article.appendChild(h4);

      let universityP = document.createElement('p');
      universityP.textContent = `University: ${universityInput.value}`;
      article.appendChild(universityP);

      let scoreP = document.createElement('p');
      scoreP.textContent = `Score: ${scoreInput.value}`;
      article.appendChild(scoreP);

      let editButton = document.createElement('button');
      editButton.classList.add('action-btn');
      editButton.classList.add('edit');
      editButton.textContent = 'edit';
      li.appendChild(editButton);
      editButton.addEventListener('click', editFromPreview);

      let applyButton = document.createElement('button');
      applyButton.classList.add('action-btn');
      applyButton.classList.add('apply');
      applyButton.textContent = 'apply';
      li.appendChild(applyButton);
      applyButton.addEventListener('click', applyToScholarship);

      studentNameInput.value = '';
      universityInput.value = '';
      scoreInput.value = '';
      nextButton.disabled = true;
    }

    function editFromPreview(event) {
      let liWrapper = event.currentTarget.parentElement;
      let article = liWrapper.children[0];

      let studentName = article.children[0].textContent;
      let universityName = article.children[1].textContent.split(': ')[1];
      let score = article.children[2].textContent.split(': ')[1];

      studentNameInput.value = studentName;
      universityInput.value = universityName;
      scoreInput.value = score;
      nextButton.disabled = false;

      liWrapper.remove();
    }

    function applyToScholarship(event) {
      let applyButton = event.currentTarget;
      let liWrapper = applyButton.parentElement;
      let editButton = liWrapper.children[1];

      applyButton.remove();
      editButton.remove();

      previewList.removeChild(liWrapper);
      candidatesList.appendChild(liWrapper);
      nextButton.disabled = false;      
    }
}
  