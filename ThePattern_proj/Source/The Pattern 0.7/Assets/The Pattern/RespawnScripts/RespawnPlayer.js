#pragma strict 
function Update() {
	if (gameObject.transform.position.y <= -50){
		gameObject.transform.position.x = 0;
		gameObject.transform.position.y = 1;
		gameObject.transform.position.z = 0;
	}
}