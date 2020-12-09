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

function answer(msgTxt) {
    let ansMsg = "dhsadhsahd";

    if (calcExp.test(msgTxt.replace(" ", ""))) {
        ansMsg = eval(msgTxt)
    }

    messageContent.innerHTML += `<div class="ans-msg">${ansMsg}</div>`
}