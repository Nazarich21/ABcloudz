"use strict";
const content = document.getElementById("content");
const selectElement = document.getElementById("select");
const header = document.getElementById("header");
let progressElement = document.getElementById("progress_amount");
let totalElement = document.getElementById("progress_total");
let timer = document.getElementById("time_amount");
let dropElement = document.getElementById("word");
let timeElement = document.getElementById("time");
let copy = document.getElementById("copy"); 
let initial = document.getElementById("initial");
const log = document.getElementById('log');
let elem = document.createElement("div");
let infoElement = document.getElementById("info");
let progress = document.getElementById("progress");
let mistakesElement = document.getElementById("mistakes");

let letterAmountSelectLabel = document.createElement("label");
let letterAmountSelect = document.createElement("select");
let letterDropSpeedLabel = document.createElement("label");
let letterDropSpeedSelect = document.createElement("select");
let startButton = document.createElement("button");
let infinityButton = document.createElement("button");
let speedParameters =[{param1:15000,param2:30},{param1:13500,param2:27},{param1:2000,param2:5}];
let stageInfinityLabel = document.createElement("div")
let stageInfinity = document.createElement("div")


let xhr = new XMLHttpRequest();
let dictionery = {};
let language = "russian" ;
let currentRow;
let tempArr;
let ignoreArr = ["Control","Shift","Alt"];
let time = 0;
let timerId;
let progressCount = 0;
let positionY = -15;
let positionX = 0;
let keyDroptext = "";
let customWordsArray = [ ];
let lettersSelectArr = [20,50,100,200];
let stageInfinityCount = 0;
let infinityParam1 = 15000;
let infinityParam2 = 30;
xhr.open('GET', '../data/data.json');

xhr.send();

xhr.onload = function () {
    dictionery = JSON.parse(xhr.response);
    console.log(dictionery)
};

function languageChange(value) {
    console.log(value);
    language = value;
    let timeLabel = document.getElementById("time_label")
    timeLabel.innerHTML = dictionery[value].time;
    let mistakesLabel = document.getElementById("mistakes_label")
    mistakesLabel.innerHTML = dictionery[value].mistakes;
    let progress_label = document.getElementById("progress_label")
    progress_label.innerHTML = dictionery[value].progress;
    let symbolPerTime = document.getElementById("symbolPerTime");
    symbolPerTime.innerHTML = dictionery[value].symbolPerTime;
    let wordPerTime = document.getElementById("wordPerTime");
    wordPerTime.innerHTML = dictionery[value].wordPerTime;
    let textPerTime = document.getElementById("textPerTime");
    textPerTime.innerHTML = dictionery[value].textPerTime;;
    let exerciseKeyboard = document.getElementById("exerciseKeyboard");
    exerciseKeyboard.innerHTML = dictionery[value].exerciseKeyboard;
    let listOfTasks = document.getElementById("listOfTasks");
    listOfTasks.innerHTML = dictionery[value].listOfTasks;
    let dropLetter = document.getElementById("dropLetter");
    dropLetter.innerHTML = dictionery[value].letterDrop;
    let wordDrop = document.getElementById("wordDrop");
    wordDrop.innerHTML = dictionery[value].wordDrop;
    let ownDrop = document.getElementById("ownDrop");
    ownDrop.innerHTML = dictionery[value].ownDrop;
    let lang = document.getElementById("language");
    lang.innerHTML = dictionery[value].language;
    

};

function symbolPerTime() { 
   
    setTime(0);
    zeroing();  
    timeElement.style.display = "block";
    header.innerHTML = dictionery[language].symbolPerTime;
    let currentArr = [dictionery[language].symbolPerTimeArr];
    tempArr = currentArr.shift();
    totalElement.innerHTML = `/${total(tempArr)}`;
    currentRow = tempArr.shift();
    displayArr(pair, mistakes);
}

function wordPerTime() {
    setTime(0);
    zeroing();
    timeElement.style.display = "block";
    header.innerHTML = dictionery[language].wordPerTime;
    let currentArr = [dictionery[language].wordPerTimeArr];
    tempArr = currentArr.shift();
    totalElement.innerHTML = `/${total(tempArr)}`;
    currentRow = tempArr.shift();
    displayArr(pair, mistakes);
}

function textPerTime() {
    setTime(0);
    zeroing();
    header.innerHTML = dictionery[language].textPerTime;   
    let currentArr = [dictionery[language].textPertimeArr];
    tempArr = currentArr.shift();
    totalElement.innerHTML = `/${total(tempArr)}`;
    currentRow =tempArr.shift();
    displayArr(pair, mistakes);

}

function exerciseKeyboard() {   
    setTime(0);
    zeroing();
    header.innerHTML = dictionery[language].exerciseKeyboard;
    let currentArr = numbersArray;
    totalElement.innerHTML = `/${total(currentArr)}`;
    currentRow =currentArr.shift();
    displayArr(pair, mistakes);
}

function letterDrop() {   
    createatributs()
   /*  setTime(0);
    dropPreparing()
    timeElement.toggleAttribute("hidden");
    zeroing();*/
    header.innerHTML = dictionery[language].letterDrop;
    /* let currentArr = [dictionery[language].letterDropArr];
    tempArr = currentArr.shift();
    totalElement.innerHTML = `/${tempArr.length}`;
    dropLetter(tempArr[letter],tempArr) */

}

function wordDrop() {
    header.innerHTML = dictionery[language].wordDrop;
}

function ownDrop() { 
    header.innerHTML = dictionery[language].ownDrop;
}

function removeAllChildNodes(parent) {
    while (parent.firstChild) {
        parent.removeChild(parent.firstChild);
    }
}


let resultArr = [];
let mistakes = 0;
let pair = 0;
let letter = 0;

function displayArr(pair, mistakes) {
    displayMistakes(mistakes);
    removeAllChildNodes(initial);
    
    progressElement.innerHTML = progressCount;

    if ( currentRow === undefined) {
        clearInterval(timerId);
        return false;
    }

    for(let a = 0; a < currentRow.length; a++) {

        let span = document.createElement("span");
        span.innerHTML = currentRow[a] + " ";
        let condition = resultArr[a];
        
        if (a === pair) {
            span.classList.add("current");
            initial.append(span);
            continue;
        }

        if (  condition === true) {
            span.classList.add("done_ok");
        } else if(  condition === false) {
            span.classList.add("wrong");
        } else {
            span.classList.add("next");
        }

        initial.append(span);
    }
}

function displayMistakes(a) {
    let el = document.getElementById("mistakes_amount");
    el.innerHTML = a;
}

/* displayArr(pair, mistakes); */

function keyTime(event) {

    event.preventDefault();
    event.stopPropagation();
    let name = event.key;
    console.log(name)

    if( ignoreArr.includes(name)) {
        mistakes -= 1;
    }else if(name === " ") {
        copy.value += name;
        mistakes -= 1;
    }
    let max = currentRow[pair].length;
    if ( name === currentRow[pair][letter]) {
        if (letter === max-1) {
            pair += 1;
            progressCount += 1;  
            letter = -1;   
        }
        
        copy.value += name;
       

        if (resultArr[pair] === undefined) {
            resultArr[pair] = true;
        }

        letter += 1;

        if (pair === currentRow.length) {
            currentRow = tempArr.shift();
            resultArr = [];
            pair = 0;
            letter = 0;
            copy.value = '';
        }

        displayArr(pair, mistakes);
       
    } else {
        if(name != " ") {
            resultArr[pair] = false;
        }

        mistakes += 1;
        displayArr(pair,  mistakes);
    }  
}

function setTime (seconds) {
    
    if ( seconds < 60) {
        timer.innerHTML = `00:${`0${seconds}`.slice(-2)}`;
    } else {
        let minutes = Math.floor(seconds/60);
        seconds = seconds % 60;
        timer.innerHTML =  `${`0${minutes}`.slice(-2)}:${`0${seconds}`.slice(-2)}`;
    }
}

function zeroing() {
    removeAllChildNodes(initial);
    resultArr = [];
    mistakes = 0;
    pair = 0;
    letter = 0;
    time = 0;
    progressCount = 0;
    clearInterval(timerId);
    timerId = setInterval(() => setTime(time += 1) , 1000);
    customWordsArray = [ ];
}

function total(initialArr)
{
    let count = 0;
    let arr = initialArr;
    
    for(let a = 0;a<arr.length;a++)
    {
        count += arr[a].length;
    }
    return count;
}


document.addEventListener('keydown', logKey);
function logKey(e) {
 keyDroptext = e.key;
}



/* function dropLetter(dropelement,arr) {

    displayMistakes(mistakes);
    progressCount += 1;
    progressElement.innerHTML = progressCount;
    createDropLetter(dropelement);

    let start = Date.now();
    let timer = setInterval(() => {
    let timePassed = Date.now() - start;

        if(keyDroptext === elem.innerHTML && letter < arr.length-1) {
            clearInterval(timer);
            letter += 1;
            dropLetter(tempArr[letter],tempArr);
        } else if ( keyDroptext === elem.innerHTML && letter === arr.length-1) {
            displayMistakes(mistakes);
            elem.innerHTML = " ";
        }

        if ( timePassed >= 15000) {
            clearInterval(timer);

            if ( letter < arr.length) {
                letter += 1;
                mistakes += 1;
                dropLetter(tempArr[letter],tempArr); 
            } else {
                elem.innerHTML = " ";
                mistakes += 1;
                displayMistakes(mistakes);
            }

            return;
        }

        positionY += 1;
        elem.style.top = `${positionY}px`;
    } , 30);
} */

/* let droptime = 0;
let dropStep = 0;
function dropLetter(dropelement,arr,speed) {
  
    let speedNumber = dictionery[language].speedArray.indexOf(speed);
    displayMistakes(mistakes);
    progressCount += 1;
    progressElement.innerHTML = progressCount;
    createDropLetter(dropelement);
     droptime = speedParameters[speedNumber].param1;
     dropStep = speedParameters[speedNumber].param2;  

    let start = Date.now();
    let timer = setInterval(() => {
    let timePassed = Date.now() - start;

        if(keyDroptext === elem.innerHTML && letter < arr.length-1) {
            clearInterval(timer);
            letter += 1;
            dropLetter(customWordsArray[letter],customWordsArray,speed);
        } else if ( keyDroptext === elem.innerHTML && letter === arr.length-1) {
            displayMistakes(mistakes);
            elem.innerHTML = " ";
        }

        if ( timePassed >= droptime) {
            clearInterval(timer);
            if ( letter < arr.length-1) {
                letter += 1;
                mistakes += 1;
                dropLetter(customWordsArray[letter],customWordsArray,speed); 
            } else {
                elem.innerHTML = " ";
                mistakes += 1;
                displayMistakes(mistakes);
            }
            return;
        }

        positionY += 1;
        elem.style.top = `${positionY}px`;
    } , dropStep);
} */









let droptime = 0;
let dropStep = 0;
function dropLetter(dropelement,arr,speed) {
  
    let speedNumber = dictionery[language].speedArray.indexOf(speed);
    displayMistakes(mistakes);
    progressCount += 1;
    progressElement.innerHTML = progressCount;
    createDropLetter(dropelement);
     droptime = speedParameters[speedNumber].param1;
     dropStep = speedParameters[speedNumber].param2;  

    let start = Date.now();
    let timer = setInterval(() => {
    let timePassed = Date.now() - start;

        if(keyDroptext === elem.innerHTML && letter < arr.length-1) {
            clearInterval(timer);
            letter += 1;
            dropLetter(customWordsArray[letter],customWordsArray,speed);
        } else if ( keyDroptext === elem.innerHTML && letter === arr.length-1) {
            displayMistakes(mistakes);
            elem.innerHTML = " ";
        }
        /* if( droptime * 0.4 < timePassed < droptime * 0.6 ) {
           
            clearInterval(timer);
            letter += 1;
            dropLetter(customWordsArray[letter],customWordsArray,speed);
        } */
        if ( timePassed >= droptime) {
            clearInterval(timer);
            if ( letter < arr.length-1) {
                letter += 1;
                mistakes += 1;
                dropLetter(customWordsArray[letter],customWordsArray,speed); 
            } else {
                elem.innerHTML = " ";
                mistakes += 1;
                displayMistakes(mistakes);
            }
            return;
        }

        positionY += 1;
        elem.style.top = `${positionY}px`;
    } , dropStep);
}







function getRandomInt(max) {
    return Math.floor(Math.random() * max);
}

function dropPreparing() {
    copy.toggleAttribute("hidden");
    initial.style.height = "500px";
    initial.style.width = "40%";
    elem.style.fontSize = "30px";
    initial.style.margin = "0 auto";
} 

function createDropLetter(dropelement){
    positionY = -15;
    elem.id = "word";
    elem.innerHTML = dropelement;
    elem.style.left = `${getRandomInt(95)}%`;
    elem.style.top = `${positionY}px`;
    initial.append(elem);
}



function createatributs(){
    header.style.display = "none";
    infoElement.style.display = "none";
    initial.style.display = "none";
    copy.style.display = "none";
    timeElement.style.display = "none";
    mistakesElement.style.display = "none";
    progress.style.display = "none";
    content.style.display = "flex";
    content.style.flexDirection= "column";
    letterAmountSelectLabel.innerHTML = dictionery[language].letterAmountSelectLabel;
    content.append(letterAmountSelectLabel);
    content.append(letterAmountSelect);
    letterDropSpeedLabel.innerHTML = dictionery[language].letterDropSpeed;
    content.append(letterDropSpeedLabel);
    content.append(letterDropSpeedSelect);
    
    //Create and append the options
    selecFill(dictionery[language].speedArray,letterDropSpeedSelect);
    selecFill(lettersSelectArr,letterAmountSelect);


    startButton.innerHTML = dictionery[language].startButton;
    startButton.style.margin = "10px 0 0 0 ";
    startButton.onclick = function(){fixedletters(letterAmountSelect.value, letterDropSpeedSelect.value)};
    content.append(startButton);
    infinityButton.innerHTML = dictionery[language].infinityButton;
    infinityButton.style.margin = "10px 0 0 0 ";
    infinityButton.onclick = function(){ infinityLetters()};
    content.append( infinityButton);
}

function selecFill(arr,select){

    for (var i = 0; i < arr.length; i++) {
        var option = document.createElement("option");
        option.setAttribute("value",arr[i]);
        option.text = arr[i];
        select.appendChild(option);
    }
}

function fixedletters(letterAmount,speed){
     console.log(letterAmount,speed);
    generateArray(letterAmount);
    dropPreparing();
    displayAfterDropType();
    totalElement.innerHTML = `/${customWordsArray.length}`;
    header.innerHTML = dictionery[language].letterDrop;
    mistakesElement.style.display = "flex";
    progress.style.display = "flex";
    dropLetter(customWordsArray[letter],customWordsArray,speed)
}

function generateArray(amount){
    for (var i = 0; i < amount; i++){
    customWordsArray.push(getRandomLetter());
    }
    console.log(customWordsArray)
} 


function infinityLetters(){
    displayAfterDropType();
    header.innerHTML = dictionery[language].letterDrop;
    stageInfinityLabel.innerHTML = dictionery[language].stageInfinityLabel;
    stageInfinity.innerHTML  = stageInfinityCount;
    infoElement.append(stageInfinityLabel);
    infoElement.append(stageInfinity);
    console.log(getRandomLetter());
    dropPreparing();
    dropLetterInfinity();

}
function displayAfterDropType(){
    header.style.display = "block";
    infoElement.style.display = "flex";
    initial.style.display = "block";
    letterAmountSelect.style.display = "none";
    letterAmountSelectLabel.style.display = "none";
    letterDropSpeedLabel.style.display = "none";
    letterDropSpeedSelect.style.display = "none";
    startButton.style.display = "none";
    infinityButton.style.display = "none";
}

function dropLetterInfinity() {
    stageInfinityCount += 1;
    stageInfinity.innerHTML  = stageInfinityCount;
    infinityParam2 = infinityParam2 * 0.9;
    infinityParam1 = infinityParam1 * 0.95;

    createDropLetter(getRandomLetter());
    let start = Date.now();
    let timer = setInterval(() => {
    let timePassed = Date.now() - start;

        if(keyDroptext === elem.innerHTML) {
            clearInterval(timer);
            dropLetterInfinity()
        }

        if ( timePassed >= infinityParam1) {
            clearInterval(timer);
            elem.innerHTML = " ";
            return;
        }

        positionY += 1;
        elem.style.top = `${positionY}px`;
    } , infinityParam2);
}

function getRandomLetter(){
    return dictionery[language].letterDropArr[ getRandomInt(dictionery[language].letterDropArr.length)];
}