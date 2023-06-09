

function upslhang() {
    var sl = Number(document.getElementById('SL').value);
    sl++;
    document.getElementById('SL').value = sl;
}
function downslhang() {
    var sl = Number(document.getElementById('SL').value);
    sl--;
    if (sl >= 1) {
        document.getElementById('SL').value = sl
    }
}
// var tang= document.getElementById('tang');
// tang.addEventListener('click',funtiontang());
