const input = document.querySelector(".send-message-input");
const messageContent = document.querySelector(".chat-content");

const calcExp = /\d+[-+*/]\d+/;

function sendMessage() {
    const msgTxt = input.value;

    if (msgTxt == "")
        return;

    messageContent.innerHTML += `<div class="my-msg">${msgTxt}</div>`
    
    input.value = "";
    answer(msgTxt);
}

let adjs = ["good", "bad", "green", "blue"];
let nouns = ["ball", "sun", "man", "horse"];
let verbs = ["bouncing", "shining", "walking", "scratching"];

function answer(msgTxt) {
    let ansMsg = "";

    if (calcExp.test(msgTxt.replaceAll(" ", ""))) {
        ansMsg = eval(msgTxt)
    }
    else {
        ansMsg = adjs[Math.floor(Math.random() * adjs.length)] + " " + nouns[Math.floor(Math.random() * nouns.length)] + " is " + verbs[Math.floor(Math.random() * verbs.length)];
    }

    messageContent.innerHTML += `<div class="ans-msg">${ansMsg}</div>`
}