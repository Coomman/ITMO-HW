function setFontSize() {
    const width = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;

    document.getElementsByTagName("html")[0].style.fontSize = width / 1920 * 16 + "px";
}

setFontSize();

window.addEventListener("resize", setFontSize);