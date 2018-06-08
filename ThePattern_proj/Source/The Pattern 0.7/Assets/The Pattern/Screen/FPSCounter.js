#pragma strict

var updateInterval = 0.5;
private var accum = 0.0;
private var frames = 0; 
private var timeleft : float; 
 
function Start()
{
    timeleft = updateInterval;  
}
 
function Update()
{
    timeleft -= Time.deltaTime;
    accum += Time.timeScale/Time.deltaTime;
    ++frames;
    if( timeleft <= 0.0 )
    {
        guiText.text = (accum/frames).ToString("f2") + " fps";
        timeleft = updateInterval;
        accum = 0.0;
        frames = 0;
    }
}