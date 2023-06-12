

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
//function themThanhCong() {
//    let sizeElement = document.querySelector('input[name="size"]:checked').value;
//    if (sizeElement != null) {
//        document.getElementById('themGioHang').style.display = 'flex';
//        setTimeout( dlflex(), 2000);
//    }
//}
//function dlflex() {
//    document.getElementById('themGioHang').style.display = 'none';

//        document.getElementById('Submit').setAttribute('type', 'submit');
//        document.getElementById('Submit').addEventListener('click', function () { });

//}
/*document.getElementById('Submit').setAttribute('type', 'submit');*/

function themThanhCong() {
    let sizeElement = document.querySelector('input[name="size"]:checked');
    if (sizeElement != null) {
        document.getElementById('themGioHang').style.display = 'flex';
        setTimeout(dlflex, 2000);

    }
    else {
        document.getElementById('error_size').innerHTML = "chọn size trước khi thêm vào giỏ hàng";
    }
}

function dlflex() {
    document.getElementById('themGioHang').style.display = 'none';
    document.getElementById('form_muahang').submit();
}