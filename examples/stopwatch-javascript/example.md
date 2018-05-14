This example builds on the stopwatch example. If you haven't checked it out yet, we would recommend that you do so first, [here](stopwatch.md).

The stopwatch example was originally written partly in uno and is a good example of how you can write custom triggers. However, as Fuse has become more mature, this app can now also be written in only JavaScript and UX. In this example we will show you how.

# UX changes

In the watchface UX, we have to do a few changes to how we animate time passed. The first change is to replace the custom `Seconds` trigger with a `Timeline`. `Timeline` is very useful, as we can control it from JavaScript. This timeline controls the clock circle which rotates every second:

	<Timeline ux:Name="timeline">
		<Change Target="clock.EndAngleDegrees" Value="360" Duration="1" />
		<Change Target="trackerBall.Degrees" Value="360" Duration="1" />
		<Pulse Target="tickCircleAnimation" />
	</Timeline>

To make the JavaScript functions of the `Timeline` visible from our main file, we need to make them public functions of the class `WatchFace`:

	<JavaScript>
		this.resume = timeline.resume.bind(timeline);
		this.pause = timeline.pause.bind(timeline);
		this.seek = timeline.seek.bind(timeline);
		this.playTo = timeline.playTo.bind(timeline);
	</JavaScript>

Notice how we use `bind` to set the `this` parameter of the function bound, and is here used to make sure the timeline is still the context used when our new, exposed functions are called.

You can check out [the documentation](/docs/fuse/triggers/timeline) for a full list of all JavaScript functions `Timeline` exposes.

That's it for changes to the `WatchFace` class. Next comes a few changes to MainView. First, we remove our custom `Stowatch` element, as it isn't used any more. Then, we give our `WatchFace` a name. This is required for us to access its functions from JavaScript.

# Javascript implementation of the stopwatch

To simplify the implementation , the JavaScript stopwatch implementation consists of a few functions that are made to accurately replace the Uno stopwatch, so most of the old code can be used:

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

The elapsed time up until last pause is stored in `baseTime`. In order to measure time while the stopwatch is running, we store the time when the stopwatch was started in `startTime`. This way, we can calculate how long the stopwatch has been running for, by adding `baseTime` to the delta of now and `startTime`. This is implemented in `getStopwatchMillis()`.

Another thing to notice is how we control the timeline. Remember how we exposed some functions from the `Timeline` in our `WatchFace` class? These are now available in our named `WatchFace`, `watchFace`.

## Making the timeline repeat

Calling `watchFace.playTo(1)` makes the timeline play to the end of its animation, but we want to loop it every second. To do this, we write a function that re-plays the timeline every time the seconds passed on the stopwatch changes:

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

That's it!
