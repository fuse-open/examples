In this example we create a stopwatch native-module which we use to create a cool stopwatch app.

While this is a good example of how to make custom UX components, it can also be pulled off using pure JavaScript logic. You can check out the JavaScript implementation of the app [here.](stopwatch-javascript.md)

# The Stopwatch module

The `Stopwatch` class is used for the actual time measurement. It derives from `NativeModule` and defines four functions which we can call from JavaScript:

```
public class Stopwatch : NativeModule
{
	public Stopwatch()
	{
		AddMember(new NativeFunction("Start", (NativeCallback)Start));
		AddMember(new NativeFunction("Stop", (NativeCallback)Stop));
		AddMember(new NativeFunction("Pause", (NativeCallback)Pause));
		AddMember(new NativeFunction("GetSeconds", (NativeCallback)GetSeconds));
	}

	... implementation ...
}
```

By adding a `Stopwatch` instance to our UX and giving it a `ux:Global` name, we can control it from JavaScript using `require('Stopwatch')`:

```
<Stopwatch ux:Global="Stopwatch"/>
```
```
var Stopwatch = require("Stopwatch");
Stopwatch.GetSeconds();
Stopwatch.Start();
Stopwatch.Pause();
Stopwatch.Stop();
```

##  A trigger to go along

We also create a few triggers to go along with our Stopwatch component, so that we can animate our UX in response to time passing:

We define an abstract class in Uno code called `StopwatchTrigger`. It inherits from `Trigger` and implements the general functionality we need for each of the more specific triggers.

```
public abstract class StopwatchTrigger : Trigger
{
	Stopwatch _stopwatch;
	public Stopwatch Stopwatch
	{
		get { return _stopwatch; }
		set
		{
			if (_stopwatch != null)
				_stopwatch.Ticked -= Tick;
			_stopwatch = value;
			_stopwatch.Ticked += Tick;
		}
	}

	void Tick()
	{
		var time = Math.Mod(Time(), 1.0) * _factor;
		if (IsRooted)
			Seek(time);
	}

	double _factor = 1.0;
	public double Factor { get { return _factor; } set { _factor = value; } }

	protected abstract double Time();
}
```

We can inherit from this class to make triggers which responds more specifically. In our case, we want a trigger which triggers on seconds passing:

```
public class Seconds : StopwatchTrigger
{
	protected override double Time()
	{
		return Stopwatch.EllapsedSeconds;
	}
}
```

We can then use this trigger in our UX to animate the watch face.


# The watch face
The watch face is composed of four circles. The background is a circle with a `SolidColor` inside its `Stroke`. The foreground has an `ImageFill` with a gradient image for its `Stroke`. It also has a copy which is used for the tick animation. Lastly there is a small `Circle` which represents the clock hand.

<!-- snippet-begin:code/WatchFace.ux:WatchFaceCircles -->

```
<Circle Width="10" Height="10" Color="#fff">
    <Rotation ux:Name="trackerBall" Degrees="0" />
    <Translation Y="-0.487" RelativeTo="ParentSize" />
</Circle>
<Circle ux:Name="clock" StartAngleDegrees="0" EndAngleDegrees="0">
    <Rotation Degrees="-90" />
    <Stroke Width="6" Alignment="Inside" Offset="-1" LineCap="Round">
        <ImageFill File="gradient.png" WrapMode="ClampToEdge"/>
    </Stroke>
</Circle>
<Circle>
    <Stroke Width="6" Alignment="Inside" Offset="-1" Brush="#455493" />
</Circle>
<Circle ux:Name="tickCircle" Visibility="Hidden">
    <Stroke Width="6" Alignment="Inside" Offset="-1">
        <ImageFill File="gradient.png" WrapMode="ClampToEdge"/>
    </Stroke>
    <Scaling ux:Name="circleScale" />
</Circle>
```

<!-- snippet-end -->


We animate the `EndAngleDegrees` property of our "clock" `Circle` inside the `Seconds` trigger.
The tick animation is triggered by using a `Pulse` animator.

<!-- snippet-begin:code/WatchFace.ux:WatchFaceAnimation -->

```
<Timeline ux:Name="tickCircleAnimation">
    <Change Target="tickCircle.Visibility" Value="Visible" />
    <Change Target="tickCircle.Opacity" Value="0" Duration="0.5" Easing="QuadraticOut" DurationBack="0" />
    <Change Target="circleScale.Factor" Value="1.3" Duration="0.5" Easing="QuadraticOut" DurationBack="0" />
</Timeline>

<Seconds Stopwatch="Stopwatch">
    <Change Target="clock.EndAngleDegrees" Value="360" Duration="1" />
    <Change Target="trackerBall.Degrees" Value="360" Duration="1" />
    <Pulse Target="tickCircleAnimation" />
</Seconds>
```

<!-- snippet-end -->

* Notice that we can access the global `Stopwatch` object we made in MainView.ux even though our `Seconds` trigger is inside the WatchFace.ux file.

# Recording laps

We add laps to a an `Observable` whenever the user clicks the lap button while the stopwatch is running.

<!-- snippet-begin:code/MainView.js:Fields -->

```
var laps = Observable();
var running = Observable(false);
var timeString = Observable("");
```

<!-- snippet-end -->

<!-- snippet-begin:code/MainView.js:AddLap -->

```
function addLapOrReset(){
    if (running.value){
        if (Stopwatch.GetSeconds() > 0)
            laps.insertAt(0, {
                title:("Lap " + (laps.length + 1)),
                time: timeString.value
            });
    } else {
        Stopwatch.Stop();
        laps.clear();
        updateTimeString();
    }
}
```

<!-- snippet-end -->

We use Adding-, Removing- and LayoutAnimation to move all the list elements as we add new items. Since we use `laps.insertAt(0, item)`, we make sure new laps are always on the top of the list.

<!-- snippet-begin:code/MainView.ux:LapsUX -->

```
<StackPanel Margin="20,40">
    <Each Items="{laps}">
        <Panel Height="50" Clicked="{removeLap}" HitTestMode="LocalBoundsAndChildren">
            <DockPanel>
                <FadeText Alignment="Center" FontSize="24" Color="#fff" Dock="Left" Value="{title}"/>
                <FadeText Alignment="Center" FontSize="24" Color="#fff" Dock="Right" Value="{time}"/>
            </DockPanel>
            <Rectangle Color="#8FBFE8" Height="1" Alignment="Bottom"/>
            <AddingAnimation>
                <Move Y="-1" RelativeTo="Size" Duration="0.3"/>
            </AddingAnimation>
            <RemovingAnimation>
                <Move X="1.4" RelativeTo="ParentSize" Duration="0.3"/>
            </RemovingAnimation>
            <LayoutAnimation>
                <Move Y="1" RelativeTo="LayoutChange" Duration="0.3" Easing="CircularInOut"/>
            </LayoutAnimation>
        </Panel>
    </Each>
</StackPanel>
```

<!-- snippet-end -->

That's it!
