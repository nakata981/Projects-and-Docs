#pragma strict

var isQuit=false;

function OnMouseEnter(){
 	renderer.material.color=Color.red;
}

function OnMouseExit(){
    renderer.material.color = Color(Random.value, Random.value, Random.value);
}

function OnMouseUp(){
 if (isQuit==true) {
  Application.Quit();
 }
 else {
  Application.LoadLevel(1);
 }
}

function Update(){
 if (Input.GetKey(KeyCode.Escape)) { Application.Quit();
 }
}