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
let speedParameters =[3000,1500,500];
let livesInfinityLabel = document.createElement("div")
let livesInfinity = document.createElement("div")
let wordsElement  = document.createElement("div");
let counterForInfinity = document.createElement("div");


let xhr = new XMLHttpRequest();
let dictionery = {};
let language = "russian" ;
let currentRow;
let tempArr;
let ignoreArr = ["Control","Shift","LShift","Alt","Backspace","ShiftAlt","Enter"];
let time = 0;
let timerId;
let progressCount = 0;
let positionY = -15;
let positionX = 0;
let keyDroptext = "";
let lettersSelectArr = [20,50,100,200];
let numbersArray = [["8","+","6","=","14"],["9","-","5","=","4"],["11","*","11","=","121"],["144","/","12","=","12"]];
let mathArray = [["sin^2(x)","+","cos^2(x)","=","1"],["(a+b)^2","=","a^2","+","2ab","+","b^2"]];
let customWordsArray = [ ];
let Blocks = []; 
let stageInfinityCount = 0;
let Interval = 3000;
let deleted = 0;
let Timer;
let lives = 5;
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
    
    displayForTime();
    setTime(0);
    zeroing();  
    timeElement.style.display = "block";
    header.innerHTML = dictionery[language].symbolPerTime;
    let currentArr = [dictionery[language].symbolPerTimeArr];
    tempArr = currentArr.shift();
    totalElement.innerHTML = `/${getTotal(tempArr)}`;
    currentRow = tempArr.shift();
    displayArr(pair, mistakes);
}

function wordPerTime() {
    displayForTime();
    setTime(0);
    zeroing();
    timeElement.style.display = "block";
    header.innerHTML = dictionery[language].wordPerTime;
    let currentArr = [dictionery[language].wordPerTimeArr];
    tempArr = currentArr.shift();
    totalElement.innerHTML = `/${getTotal(tempArr)}`;
    currentRow = tempArr.shift();
    displayArr(pair, mistakes);
}

function textPerTime() {
    displayForTime();
    setTime(0);
    zeroing();
    header.innerHTML = dictionery[language].textPerTime;   
    let currentArr = [dictionery[language].textPertimeArr];
    tempArr = currentArr.shift();
    totalElement.innerHTML = `/${getTotal(tempArr)}`;
    currentRow = tempArr.shift();
    displayArr(pair, mistakes);

}

function exerciseKeyboard() {
    displayForTime();
    setTime(0);
    zeroing();
    header.innerHTML = dictionery[language].exerciseKeyboard;   
    let currentArr = numbersArray;
    tempArr = currentArr;
    totalElement.innerHTML = `/${getTotal(tempArr)}`;
    currentRow = tempArr.shift();
    displayArr(pair, mistakes);
}

function letterDrop() {     
    createAtributsLetters();
    header.innerHTML = dictionery[language].letterDrop;
}

function wordDrop() {
    createAtributsWords();
    header.innerHTML = dictionery[language].wordDrop;
}

function ownDrop() { 
    livesInfinityLabel.style.display = "none";
    livesInfinity.style.display = "none";
    displayForTime();
    setTime(0);
    zeroing();  
    header.innerHTML = dictionery[language].ownDrop;
    let currentArr = mathArray;
    tempArr = currentArr;
    totalElement.innerHTML = `/${getTotal(tempArr)}`;
    currentRow = tempArr.shift();
    displayArr(pair, mistakes);
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

    if (currentRow === undefined) {
        clearInterval(timerId);
        return false;
    }

    for (let a = 0; a < currentRow.length; a++) {

        let span = document.createElement("span");
        span.innerHTML = currentRow[a] + " ";
        let condition = resultArr[a];
        
        if (a === pair) {
            span.classList.add("current");
            initial.append(span);
            continue;
        }

        if (condition === true) {
            span.classList.add("done_ok");
        } else if(condition === false) {
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

function keyTime(event) {

    event.preventDefault();
    event.stopPropagation();
    let name = event.key;
    console.log(name)

    if (ignoreArr.includes(name)) {
        mistakes -= 1;
    }else if (name === " ") {
        copy.value += name;
        mistakes -= 1;
    }

    let max = currentRow[pair].length;

    if (name === currentRow[pair][letter]) {

        if (letter === max-1) {

            if (resultArr[pair] === undefined) {
                resultArr[pair] = true;
            }

            pair += 1;
            progressCount += 1;  
            letter = -1;   
        }
        
        copy.value += name;
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

        if (name != " " && !ignoreArr.includes(name)) {
            resultArr[pair] = false;
        }

        mistakes += 1;
        displayArr(pair,  mistakes);
    }  
}

function setTime (seconds) {
    
    if (seconds < 60) {
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

function getTotal(initialArr)
{
    let count = 0;
    let arr = initialArr;

    for (let a = 0;a<arr.length;a++) {
        count += arr[a].length;
    }

    return count;
}

function fixedletters(letterAmount,speed){
    document.addEventListener('keydown', logKey);

    function logKey(e) {
        keyDroptext = e.key;
    }

    console.log(letterAmount,speed);
    generateArray(letterAmount);
    dropPreparing();
    displayAfterDropType();
    totalElement.innerHTML = `/${customWordsArray.length}`;
    header.innerHTML = dictionery[language].letterDrop;
    mistakesElement.style.display = "flex";
    progress.style.display = "flex";
    livesInfinity.style.display = "none";
    livesInfinityLabel.style.display = "none";
    Interval = speedParameters[dictionery[language].speedArray.indexOf(speed)];
    nextLetter();
    showLetter();
    TimerEvent()
}

function infinityLetters(){
    document.addEventListener('keydown', logKey);
    livesInfinityLabel.style.display = "block";
    livesInfinity.style.display = "block";
    
    function logKey(e) {
        keyDroptext = e.key;
    }

    displayAfterDropType();
    header.innerHTML = dictionery[language].letterDrop;
    livesInfinityLabel.innerHTML = dictionery[language].livesInfinityLabel;
    livesInfinity.innerHTML = lives;
    infoElement.append(livesInfinityLabel);
    infoElement.append(livesInfinity);
    dropPreparing();
    nextLetterInfinity();
    showLetter();;
    Infinity1();
}

function nextLetter(){
    if (Blocks.length < 2) {
        positionY = -15;
        let block = new Array();
        block['keychar'] = customWordsArray[letter];
        block['x'] = getRandomInt(95);
        block['y'] = positionY;
        block['added'] = false;
        Blocks.push(block);
        letter += 1;
        progressElement.innerHTML = letter;
    }
}

function nextLetterInfinity(){

    if (Blocks.length < 2) {
        positionY = -15;
        let block = new Array();
        block['keychar'] = getRandomLetter();
        block['x'] = getRandomInt(95);
        block['y'] = positionY;
        block['added'] = false;
        Blocks.push(block);
    }
}

function showLetter(){
    initial.innerHTML = " ";
    for (let i in Blocks)
    {
      initial.innerHTML += '<div style="position:absolute;top:' + Blocks[i].y + 'px;left:' + Blocks[i].x + '%;font-size:30px;">'+ Blocks[i].keychar + '</div>';
    }
    
}

function Infinity1(){
    livesInfinity.innerHTML = lives;

    if (Blocks.length === 0) {
        return;
    }

    for (let i in Blocks) {
        Blocks[i].y += 1;
    }
  
    if (Blocks[0].y >= 250 && Blocks[0].added === false) {
        nextLetterInfinity();
        Blocks[0].added = true;
    }

    if (Blocks[0].y > 500) {
        lives -= 1;
        Blocks.splice(0,1);
        nextLetterInfinity();
    }

    if (lives === 0) {
        initial.innerHTML = " ";
        removeAllChildNodes(initial);
        livesInfinity.innerHTML = lives;
        return;
    }
    
    const foundIndex = Blocks.findIndex(({keychar}) => keychar === keyDroptext);
    
    if (foundIndex > -1) {
        Blocks.splice(foundIndex, 1);
        nextLetterInfinity();
        Interval -= 200;
    }

    if (lives > 0) {
        Timer = setTimeout(Infinity1, 
        Interval > 100 ? Math.ceil(Interval / 100) : 1);
        Interval -= 1;
    }
    showLetter();
}

function TimerEvent()  {  
    displayMistakes(mistakes);

    if (Blocks.length === 0) {
        return;
    }

    for (let i in Blocks) {
        Blocks[i].y += 1;
    }
  
    if (Blocks[0].y >= 250 && Blocks[0].added === false) {

        if (letter < customWordsArray.length) {
        nextLetter();
        Blocks[0].added = true;
        }  
    }

    if (Blocks[0].y > 500 ) {

        if (letter < customWordsArray.length) {
        Blocks.splice(0,1);
        nextLetter();
        deleted += 1
        mistakes += 1;
        } else {
            Blocks.splice(0,1);
            deleted += 1
            mistakes += 1;
            displayMistakes(mistakes);   
        } 
    }
    
    const foundIndex = Blocks.findIndex(({keychar}) => keychar === keyDroptext);
    console.log(keyDroptext);

    if (foundIndex > -1) {
        Blocks.splice(foundIndex, 1);
        deleted += 1
        keyDroptext = "";
        wordsElement.innerHTML = "";

        if (letter < customWordsArray.length) {
            nextLetter();
        }
    }
    

   if (deleted < customWordsArray.length) {
        Timer = setTimeout(TimerEvent, 
        Interval > 100 ? Math.ceil(Interval / 100) : 1);
        Interval -= 1;
    }
    showLetter();
}

function fixedWords(letterAmount,speed) {
    document.addEventListener('keydown', logKey);


    function logKey(e) {
        if (!ignoreArr.includes(e.key)) {
            wordsElement.innerHTML += e.key;
        }

        keyDroptext = e.key;
        console.log(keyDroptext);
    }

    console.log(letterAmount,speed);
    generateArrayOfWords(letterAmount);
    dropPreparing();
    wordsElement.style.width = "40%"
    wordsElement.style.backgroundColor ="#fff";
    wordsElement.style.margin = "10px auto 0 auto"; 
    wordsElement.style.height = "30px";
    wordsElement.style.display = "block";
    content.append(wordsElement);
    displayAfterDropType();
    removeAllChildNodes(counterForInfinity);
    mistakesElement.style.display = "flex";
    progress.style.display = "flex";
   
    totalElement.innerHTML = `/${customWordsArray.length}`;
    header.innerHTML = dictionery[language].wordDrop;
    Interval = speedParameters[dictionery[language].speedArray.indexOf(speed)];
    // InitialInterval = Interval
    nextLetter();
    showLetter();
    TimerEventWord()
}

function infinityWords(){
    document.addEventListener('keydown', logKey);

    function logKey(e) {

        if (!ignoreArr.includes(e.key)) {
            wordsElement.innerHTML += e.key;
        }

        keyDroptext = e.key;
        console.log(keyDroptext);
    }

    dropPreparing();
    displayAfterDropType();
    header.innerHTML = dictionery[language].letterDrop;
    livesInfinityLabel.innerHTML = dictionery[language].livesInfinityLabel;
    livesInfinityLabel.style.display = "block";
    livesInfinity.style.display = "block";
    livesInfinity.innerHTML = lives;
    infoElement.append(livesInfinityLabel);
    infoElement.append(livesInfinity);
    wordsElement.style.width = "40%"
    wordsElement.style.backgroundColor ="#fff";
    wordsElement.style.margin = "10px auto 0 auto"; 
    wordsElement.style.height = "30px";
    counterForInfinity.style.display = "block";
    
    wordsElement.style.display = "block"
    content.append(wordsElement);
    header.innerHTML = dictionery[language].wordDrop;
    nextWordInfinity();
    showLetter();
    infinityWordsEvent()
}

function TimerEventWord()  {  
    displayMistakes(mistakes);

    if (Blocks.length === 0) {
        return;
    }

    for (let i in Blocks) {
        Blocks[i].y += 1;
    }
  
    if (Blocks[0].y >= 250 && Blocks[0].added === false) {

        if (letter < customWordsArray.length) {
            nextLetter();
            Blocks[0].added = true;
        }  
    }

    if (Blocks[0].y > 500 ) {

        if (letter < customWordsArray.length) {
            Blocks.splice(0,1);
            nextLetter();
            deleted += 1
            mistakes += 1;
        }  else {
            Blocks.splice(0,1);
            deleted += 1
            mistakes += 1;
            displayMistakes(mistakes);
        } 
    }
    
    const foundIndex = Blocks.findIndex(({keychar}) => keychar === wordsElement.innerHTML);
    
    if (keyDroptext === "Enter" && foundIndex) {

        keyDroptext = "";
        Blocks.splice(foundIndex, 1);
        deleted += 1
        wordsElement.innerHTML = "";

        if (letter < customWordsArray.length) {
            nextLetter();
        }
    }

    if (keyDroptext === "Enter") {
        wordsElement.innerHTML = "";
    }
    

   if (deleted < customWordsArray.length) {
        Timer = setTimeout(TimerEventWord, 
        Interval > 100 ? Math.ceil(Interval / 100) : 1);
        Interval -= 1;
    }
    showLetter();
}

function nextWordInfinity(){
    if (Blocks.length < 2) {
        positionY = -15;
        let block = new Array();
        block['keychar'] = getRandomWord();
        block['x'] = getRandomInt(95);
        block['y'] = positionY;
        block['added'] = false;
        Blocks.push(block);
    }
}

function infinityWordsEvent(){

    livesInfinity.innerHTML = lives;
    displayMistakes(mistakes);

    if (Blocks.length === 0) {
        return;
    }

    for (let i in Blocks) {
        Blocks[i].y += 1;
    }
  
    if (Blocks[0].y >= 250 && Blocks[0].added === false) {
        nextWordInfinity();
        Blocks[0].added = true;
    }

    if (Blocks[0].y > 500 ) {
        Blocks.splice(0,1);
        lives -= 1;
        livesInfinity.innerHTML = lives;
    }

    if (lives === 0) {
        initial.innerHTML = " ";
        removeAllChildNodes(initial);
        livesInfinity.innerHTML = lives;
        return;
    }
    
    const foundIndex = Blocks.findIndex(({keychar}) => keychar === wordsElement.innerHTML);
    
    if (keyDroptext === "Enter" && foundIndex) {
        keyDroptext = "";
        Blocks.splice(foundIndex, 1);
        wordsElement.innerHTML = "";
        nextWordInfinity();
        Interval -= 200;
    }


    if (keyDroptext === "Enter") {
        wordsElement.innerHTML = "";
    }
    

   if (lives > 0) {
        Timer = setTimeout(infinityWordsEvent, 
        Interval > 100 ? Math.ceil(Interval / 100) : 1);
        Interval -= 1;
    }
    showLetter();
}

function getRandomInt(max) {
    return Math.floor(Math.random() * max)+1;
}

function dropPreparing() {
    copy.toggleAttribute("hidden");
    initial.style.height = "500px";
    initial.style.width = "40%";
    elem.style.fontSize = "30px";
    initial.style.margin = "0 auto";
} 

function createAtributsLetters(){
    header.style.display = "none";
    infoElement.style.display = "none";
    initial.style.display = "none";
    copy.style.display = "none";
    timeElement.style.display = "none";
    mistakesElement.style.display = "none";
    progress.style.display = "none";
    content.style.display = "flex";
    content.style.flexDirection= "column";
    livesInfinityLabel.style.display = "none";
    livesInfinity.style.display = "none";
    letterAmountSelectLabel.innerHTML = dictionery[language].letterAmountSelectLabel;
    removeAllChildNodes(letterAmountSelect);

    content.append(letterAmountSelectLabel);
    content.append(letterAmountSelect);
    letterDropSpeedLabel.innerHTML = dictionery[language].letterDropSpeed;
    content.append(letterDropSpeedLabel);
    content.append(letterDropSpeedSelect);
    //Create and append the options
    selecFill(dictionery[language].speedArray, letterDropSpeedSelect);
    selecFill(lettersSelectArr, letterAmountSelect);
    displayForDrop();
    startButton.innerHTML = dictionery[language].startButton;
    wordsElement.style.display = "none";
    livesInfinity.style.display = "none";
    livesInfinityLabel.style.display = "none";
    startButton.style.margin = "10px 0 0 0 ";
    startButton.onclick = function( ){ fixedletters( letterAmountSelect.value, letterDropSpeedSelect.value)};
    content.append(startButton);
    infinityButton.innerHTML = dictionery[language].infinityButton;
    infinityButton.style.margin = "10px 0 0 0 ";
    infinityButton.onclick = function(){ infinityLetters()};
    content.append( infinityButton);
}

function createAtributsWords(){
    header.style.display = "none";
    initial.style.display = "none";
    copy.style.display = "none";
    timeElement.style.display = "none";
    mistakesElement.style.display = "none";
    progress.style.display = "none";
    content.style.display = "flex";
    content.style.flexDirection= "column";
    counterForInfinity.style.display = "none";
    livesInfinityLabel.style.display = "none";
    livesInfinity.style.display = "none";
    wordsElement.style.display = "block"
    letterAmountSelectLabel.innerHTML = dictionery[language].wordsAmountSelectLabel;
    removeAllChildNodes(letterAmountSelect);
    content.append(letterAmountSelectLabel);
    content.append(letterAmountSelect);
    letterDropSpeedLabel.innerHTML = dictionery[language].letterDropSpeed;
    content.append(letterDropSpeedLabel);
    content.append(letterDropSpeedSelect);
    //Create and append the options
    selecFill(dictionery[language].speedArray, letterDropSpeedSelect);
    selecFill(lettersSelectArr, letterAmountSelect);
    displayForDrop();
    startButton.innerHTML = dictionery[language].startButton;
    startButton.style.margin = "10px 0 0 0 ";
    startButton.onclick = function( ){ fixedWords(letterAmountSelect.value, letterDropSpeedSelect.value)};
    content.append(startButton);
    infinityButton.innerHTML = dictionery[language].infinityButton;
    infinityButton.style.margin = "10px 0 0 0 ";
    infinityButton.onclick = function(){ infinityWords()};
    content.append(infinityButton);
}

function selecFill(arr,select){

    for (let i = 0; i < arr.length; i++) {
        let option = document.createElement("option");
        option.setAttribute("value",arr[i]);
        option.text = arr[i];
        select.appendChild(option);
    }
}

function generateArray(amount){

    for (let i = 0; i < amount; i++) {
        customWordsArray.push(getRandomLetter());
    }

    console.log(customWordsArray)
} 

function generateArrayOfWords(amount){

    for (let i = 0; i < amount; i++) {
        customWordsArray.push(getRandomWord());
    }

    console.log(customWordsArray)
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

function getRandomLetter(){
    return dictionery[language].letterDropArr[ getRandomInt(dictionery[language].letterDropArr.length - 1)];
}

function getRandomWord(){
    let word = "";
    let wordLength = getRandomInt(6);

    for (let a = 0; a < wordLength; a++) {
        word += getRandomLetter();
    } 

    return word;
}
function displayForTime(){
    header.style.display = "block";
    initial.style.display = "block";
    copy.style.display = "block";
    timeElement.style.display = "block";
    mistakesElement.style.display = "block";
    progress.style.display = "block";
    infoElement.style.display = "flex";
    initial.style.display = "block";
    letterAmountSelect.style.display = "none";
    letterAmountSelectLabel.style.display = "none";
    letterDropSpeedLabel.style.display = "none";
    letterDropSpeedSelect.style.display = "none";
    startButton.style.display = "none";
    infinityButton.style.display = "none";
    copy.style.display = "blok";
    livesInfinityLabel.style.display = "none";
    livesInfinity.style.display = "none";
    Blocks = [];
    counterForInfinity.style.display = "none";
    wordsElement.style.display = "none";
    initial.innerHTML = "";
    initial.style.height = "100px";
    initial.style.width = "100%";
    elem.style.fontSize = "30px";
    initial.style.margin = "0 auto";
    
}
function displayForDrop(){
    letterAmountSelect.style.display = "block";
    letterAmountSelectLabel.style.display = "block";
    letterDropSpeedLabel.style.display = "block";
    letterDropSpeedSelect.style.display = "block";
    startButton.style.display = "block";
    infinityButton.style.display = "block";
    wordsElement.style.display = "none"  
} 
