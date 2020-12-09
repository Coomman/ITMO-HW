const src = ["avatar.jpg", "avatar2.png", "avatar3.png", "avatar4.png"];
let curSrc = 0

const gallery = document.querySelector(".avatars-gallery");

let prev, cur, next;

function setAvatars(){
    prev = document.querySelector(".avatar-prev"), 
    cur = document.querySelector(".avatar-cur");
    next = document.querySelector(".avatar-next");
}

function movePrev() {
    setAvatars();

    cur.classList.replace("avatar-cur", "avatar-next");
    prev.classList.replace("avatar-prev", "avatar-cur");
    
    curSrc = curSrc == 0 ? src.length - 1 : curSrc - 1;

    next.remove();
    gallery.innerHTML += `<img class="avatar avatar-prev" src="src/${src[curSrc == 0 ? src.length - 1 : curSrc - 1]}" draggable="false"/>`
}

function moveNext(){
    setAvatars();

    cur.classList.replace("avatar-cur", "avatar-prev");
    next.classList.replace("avatar-next", "avatar-cur");

    curSrc = (curSrc + 1) % src.length;

    prev.remove();
    gallery.innerHTML += `<img class="avatar avatar-next" src="src/${src[(curSrc + 1) % src.length]}" draggable="false"/>`
}