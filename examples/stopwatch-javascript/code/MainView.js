var Observable = require("FuseJS/Observable");

//snippet-begin:Fields
var laps = Observable();
var running = Observable(false);
var timeString = Observable("");
//snippet-end

function updateTimeString(){
	function leftPad(d) {
		return (d < 10) ? '0' + d.toString() : d.toString();
	}
	var seconds = getStopwatchSeconds();
	var millis = seconds * 1000;

	mins = Math.floor(seconds / 60);
	secs = Math.floor(seconds) % 60,
	hundredths = Math.floor((millis % 10e2) / 10);
	timeString.value =  leftPad(mins) + ":" + leftPad(secs) + ":" + leftPad(hundredths);
}

var lastSecond = 0;
function checkRestartAnimation() {
	var currSec = Math.floor(getStopwatchSeconds());
	if(currSec != lastSecond) {
	if(currSec>lastSecond) { //Don't play the animation on a reset
		watchFace.seek(0);
		watchFace.playTo(1);
	}
	lastSecond = currSec;
	}
}

function updateTimeLoop(){
	updateTimeString();
	checkRestartAnimation();
	if(running.value) {
		setTimeout(updateTimeLoop, 1000/60);
	}
}


//snippet-begin:AddLap
function addLapOrReset(){
	if (running.value){
		if (getStopwatchSeconds() > 0)
			laps.insertAt(0, {
				title:("Lap " + (laps.length + 1)),
				time: timeString.value
			});
	} else {
		resetStopwatch();
		laps.clear();
		updateTimeString();
	}
}
//snippet-end

function removeLap(arg){
	laps.tryRemove(arg.data);
}

function stopStart(){
	running.value = !running.value;
	if (running.value){
		startStopwatch();
	} else {
		pauseStopwatch();
	}

}

//Stopwatch part

var baseTime = 0;
var startTime = Date.now();

function startStopwatch() {
	startTime = Date.now();
	watchFace.playTo(1);
	setTimeout(updateTimeLoop, 1000/60);
}
function pauseStopwatch() {
	baseTime += (Date.now()-startTime);
	watchFace.pause();
}
function resetStopwatch() {
	baseTime = 0;
	watchFace.seek(0);
}
function getStopwatchMillis() {
	return baseTime + (running.value ? Date.now()-startTime : 0);
}
function getStopwatchSeconds() {
	return getStopwatchMillis()/1000;
}

updateTimeLoop();

module.exports = {
	timeString: timeString,
	laps: laps,
	addLapOrReset: addLapOrReset,
	removeLap: removeLap,
	stopStart: stopStart,
	running: running
};
