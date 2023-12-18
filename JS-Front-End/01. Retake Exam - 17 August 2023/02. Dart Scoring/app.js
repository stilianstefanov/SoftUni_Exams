window.addEventListener("load", solve);

function solve() {
    let playerNameInput = document.getElementById('player');
    let scoreInput = document.getElementById('score');
    let roundInput = document.getElementById('round');
    let addButton = document.getElementById('add-btn');

    let sureList = document.getElementById('sure-list');
    let scoreBoardList = document.getElementById('scoreboard-list');
    let clearButton = document.getElementsByClassName('clear')[0];

    addButton.addEventListener('click', addtoSureList);
    clearButton.addEventListener('click', reloadApp);

    function addtoSureList() {
      if (!playerNameInput.value || !scoreInput.value || !roundInput.value) return;

      let li = document.createElement('li');
      li.classList.add('dart-item');
      sureList.appendChild(li);

      let article = document.createElement('article');
      li.appendChild(article);

      let name = document.createElement('p');
      name.textContent = playerNameInput.value;
      article.appendChild(name);

      let score = document.createElement('p');
      score.textContent = `Score: ${scoreInput.value}`;
      article.appendChild(score);

      let round = document.createElement('p');
      round.textContent = `Round: ${roundInput.value}`;
      article.appendChild(round);

      let editButton = document.createElement('button');
      editButton.classList.add('btn');
      editButton.classList.add('edit');
      editButton.textContent = 'edit';
      li.appendChild(editButton);
      editButton.addEventListener('click', editRecord);

      let okButton = document.createElement('button');
      okButton.classList.add('btn');
      okButton.classList.add('ok');
      okButton.textContent = 'ok';
      li.appendChild(okButton);
      okButton.addEventListener('click', addToScoreboard);

      playerNameInput.value = '';
      scoreInput.value = '';
      roundInput.value = '';
      addButton.disabled = true;
    }

    function editRecord(event) {
      let wrapper = event.currentTarget.parentElement;
      let article = wrapper.children[0];
      let name = article.children[0].textContent;
      let score = article.children[1].textContent.split(': ')[1];
      let round = article.children[2].textContent.split(': ')[1];

      playerNameInput.value = name;
      scoreInput.value = score;
      roundInput.value = round;
      addButton.disabled = false;
      wrapper.remove();
    }

    function addToScoreboard(event) {
      let wrapper = event.currentTarget.parentElement;
      let editButton = wrapper.children[1];
      let okButton = event.currentTarget;

      addButton.disabled = false;
      editButton.remove();
      okButton.remove();

      scoreBoardList.appendChild(wrapper);
      sureList.removeChild(wrapper);      
    }

    function reloadApp() {
      location.reload();
    }
}
  