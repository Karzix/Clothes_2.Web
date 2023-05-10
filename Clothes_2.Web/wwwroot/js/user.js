function dk(){
    var email=document.getElementById('email').value;
    var pass=document.getElementById('passwprd').value;

    var user={
        Email:email,
        Pass:pass
    }
    var json=JSON.stringify(user);
    localStorage.setItem(email,json);
}

var cr= Number(document.getElementById('wrapper').clientWidth);
document.getElementById('wrapper').style.left="calc(50% - " + (cr/2) +"px";
var cr= Number(document.getElementById('wrapper').clientHeight);
document.getElementById('wrapper').style.top="calc(50% - " + (cr/2) +"px";
