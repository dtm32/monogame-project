// battle sim for testing unit stats and damage formula

let vSelectedCell;
let vAttacker;
let vDefender;

function init()
{
  // add units to unit list element
  let vList = document.getElementById('unit-list');

  for(let pKey in UNITS)
  {
    let vLi = document.createElement('li');

    vLi.innerHTML = pKey;
    vLi.addEventListener('click', function()
    {
      console.log(UNITS[pKey]);
      vSelectedCell.dataset.unit = pKey;
      vSelectedCell.dataset.health = UNITS[pKey].HP;
      vSelectedCell.querySelector('.unit-name').innerHTML = pKey;
      hideUnitSelection();
    });

    vList.appendChild(vLi);
  }

  document.getElementById('unit-list-cancel').addEventListener('click', function(pEvent)
  {
    hideUnitSelection();
  });

  // add click listeners to grid
  let vTableLeft = document.getElementById('left-table');
  let vTableLeftData = vTableLeft.children[0].children;

  for(let pIndex = 0; pIndex < 5; pIndex++)
  {
    for(let pCell = 0; pCell < 3; pCell++)
    {
      // setup edit unit button
      let vEditButton = document.createElement('div');
      let vText       = document.createElement('span');

      vEditButton.classList = 'edit-unit-button';
      vEditButton.innerHTML = 'U';

      vText.classList = "unit-name";

      vEditButton.addEventListener('click', function(pEvent)
      {
        vSelectedCell = this.parentElement;

        displayUnitSelection(pEvent.pageX, pEvent.pageY);
      });
      
      vTableLeftData[pIndex].children[pCell].appendChild(vText);
      vTableLeftData[pIndex].children[pCell].appendChild(vEditButton);

      // setup unit click to battle
      vTableLeftData[pIndex].children[pCell].addEventListener('click', function()
      {
        if(this.dataset.unit)
        {
          vAttacker = this.dataset.unit;
        }
      });
    }
  }

  // defenders grid
  let vTableRight = document.getElementById('right-table');
  let vTableRightData = vTableRight.children[0].children;

  for(let pIndex = 0; pIndex < 5; pIndex++)
  {
    for(let pCell = 0; pCell < 3; pCell++)
    {
      let vUnitElement = vTableRightData[pIndex].children[pCell];

      // setup edit unit button
      let vEditButton = document.createElement('div');
      let vText       = document.createElement('span');

      vEditButton.classList = 'edit-unit-button';
      vEditButton.innerHTML = 'U';

      vText.classList = "unit-name";

      vEditButton.addEventListener('click', function(pEvent)
      {
        vSelectedCell = this.parentElement;

        displayUnitSelection(pEvent.pageX, pEvent.pageY);
      });
      
      vUnitElement.appendChild(vText);
      vUnitElement.appendChild(vEditButton);
      
      // setup unit health bar
      let vHealthBar = document.createElement('div');

      vHealthBar.className = 'health-bar';

      vUnitElement.appendChild(vHealthBar);

      // setup unit click to battle
      vUnitElement.addEventListener('click', function()
      {
        if(this.dataset.unit)
        {
          vDefender = this.dataset.unit;

          // calculate attack damage
          let vPower = getPower();
          let vAttack = UNITS[vAttacker].Str;
          let vDefense = UNITS[vDefender].Amr;
          let vRandom = Math.random() * 0.15 + 0.45;
          let vDamage = (vPower * vAttack * vRandom) / (vDefense);

          // calculate new health
          let vHealth = vUnitElement.dataset.health;
          vHealth - Math.floor(vDamage) >= 0 ? vHealth = vHealth - Math.floor(vDamage) : vHealth = 0;

          // update health bar
          let vHealthPercent = Math.floor(vHealth / parseInt(UNITS[vDefender].HP) * 100);
          vUnitElement.querySelector('.health-bar').style.width = vHealthPercent + '%';

          vUnitElement.dataset.health = vHealth;

          console.log(`${vAttacker} attacked ${vDefender} for ${Math.floor(vDamage)} damage`);
        }
      });

    }
  }
}

function displayUnitSelection(pX, pY)
{
  let vContainer = document.getElementById('unit-list-container');

  vContainer.style.display = 'inline';
  vContainer.style.top  = pY;
  vContainer.style.left = pX;
}

function hideUnitSelection()
{
  document.getElementById('unit-list-container').style.display = 'none';
}

function getPower()
{
  let vPower = document.getElementById('power').value;

  if(!vPower) {
    vPower = 0;
  }

  return vPower;
}

init();