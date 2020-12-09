const fs = require("fs");

fs.readdir("css", (err, items) => {
  console.log(items);
  for (let i = 0; i < items.length; i++) {

    fs.readFile(`css/${items[i]}`, "utf8", (err, data) => {
      const newData = data.replace(/(\d+\.?(\d+)?)px/gi, replaceToRem);
      //console.log(newData);

      fs.writeFileSync(`result/${items[i]}`, newData);
    });
  }
});

function replaceToRem(match, p1, p2) {
  return p1 / 16 + "rem"; 
}