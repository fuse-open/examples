In this example, we are making a button with a fancy loading animation.

The icons used in this example are from [Font Awesome](http://fontawesome.io/).

> *Note:* Although this example contains a complete app screen for context, the focus of this article is only on the button itself.

## Making the button

We begin by declaring our `SendButton` component.
It has a boolean property, `IsLoading`, which controls the animation.
The animation begins when `IsLoading` becomes `true`, and plays until it becomes `false`.
From here on, we'll refer to these two states as *idle* (`IsLoading="false"`) and *loading*  (`IsLoading="true"`).

We also declare the `PrimaryColor` and `SecondaryColor` properties to allow customizing the colors of the component.

<!-- snippet-begin:code/SendButton.ux:Declaration -->

```
<Panel ux:Class="SendButton" Width="70" Height="70" IsLoading="false" PrimaryColor="#F73859" SecondaryColor="#fff">
    <bool ux:Property="IsLoading" />
    
    <float4 ux:Property="PrimaryColor" />
    <float4 ux:Property="SecondaryColor" />
```

<!-- snippet-end -->

Our component is made up of a `Panel` containing several controls.
Because of how `Panel` performs layout on its children, the controls will be layered on top of each other – which is exactly what we want in this case.


The paper plane icon is simply an `Image` element backed by a `MultiDensityImageSource`.
We give it a name, `icon`, so we can animate it later on.

<!-- snippet-begin:code/SendButton.ux:Icon -->

```
<Image ux:Name="icon" StretchMode="Uniform" Width="24" Alignment="Center" Offset="-2,1" Color="{ReadProperty this.SecondaryColor}">
    <MultiDensityImageSource>
        <FileImageSource Density="1" File="Assets/fa-paper-plane@1x.png" />
        <FileImageSource Density="2" File="Assets/fa-paper-plane@2x.png" />
        <FileImageSource Density="4" File="Assets/fa-paper-plane@4x.png" />
    </MultiDensityImageSource>
</Image>
```

<!-- snippet-end -->

We want to give the user some visual feedback while they are pressing down on the button, but before the animation starts.
For that, we use a `Circle` with a semi-transparent white fill, and a `WhilePressed` trigger with a `Change` animator that fades its opacity from `0` to `1`.

<!-- snippet-begin:code/SendButton.ux:TapOverlayCircle -->

```
<Circle ux:Name="tapOverlayCircle" Color="#fff6" Opacity="0" />
<WhilePressed>
    <Change tapOverlayCircle.Opacity="1" Duration="0.05" DurationBack="0.15" />
</WhilePressed>
```

<!-- snippet-end -->

Now for the good stuff.

We have a `Circle` named `mainCircle`, which has a single `Stroke` and no other brushes.
`mainCircle` is used as both the background while the button is idle, *and* as the rotating arc while it is loading.

While the button is idle, the `Width` of this `Stroke` is larger than the radius of the circle.
This results in the circle being completely covered, as if we would have just assigned it a `Color`.
When transitioning to the loading state however, we animate the `Width` of this stroke to a lower value.

During this transition, we also animate the `LengthAngleDegrees` property of the `Circle` to `90`.
Combined with `StartAngleDegrees`, this property allows us to draw a slice of the circle instead of the whole 360 degrees.

<!-- snippet-begin:code/SendButton.ux:MainCircle -->

```
<Circle ux:Name="mainCircle" StartAngleDegrees="0" LengthAngleDegrees="360">
    <Stroke ux:Name="mainCircleStroke" Width="100" Color="{ReadProperty this.PrimaryColor}"  />
    <Rotation ux:Name="mainCircleRotation" />
</Circle>
```

<!-- snippet-end -->

The remaining elements are static `Circle` elements which serve as a background while the button is loading.
These are hidden underneath the `mainCircle` while the button is idle.

The first of these is the "track" underneath the spinning arc.
It is scaled with a factor of `0.85` to match the same scaling being performed on `mainCircle` during the transition to the loading state.

The second `Circle` is the background that's visible while loading, and also provides the shadow underneath the button.

<!-- snippet-begin:code/SendButton.ux:StaticBackgroundCircles -->

```
<Circle>
    <Stroke Width="5" Color="#6662" />
    <Scaling Factor="0.85" />
</Circle>

<Circle Color="{ReadProperty this.SecondaryColor}" Margin="1">
    <Shadow Angle="90" Size="5" Distance="3" Color="#0005" />
</Circle>
```

<!-- snippet-end -->

Let's move on to animation.
We'll add a `WhileTrue` trigger to play back the animation based on the value of the `IsLoading` property.

<!-- snippet-begin:code/SendButton.ux:WhileTrue -->

```
<WhileTrue Value="{ReadProperty this.IsLoading}">
    <PulseForward Target="flightAnimation" />
    <Change icon.Opacity="0" Delay="0.68" DurationBack="0.1" Easing="CubicInOut" />
    
    <Scale Target="mainCircle" Factor="0.85" Duration="0.5" Easing="BackInOut" Delay="0.35" />
    <Change mainCircleStroke.Width="5" Duration="0.2" Delay="0.35" Easing="SinusoidalInOut" />
    <Change mainCircle.LengthAngleDegrees="90" Duration="0.5" Delay="0.75" DelayBack="0" Easing="CubicInOut" />
    <Change spin.Value="true" Delay="1.4" />
</WhileTrue>
<WhileTrue ux:Name="spin">
    <Cycle Target="mainCircleRotation.Degrees" Low="0" High="360" Frequency="0.666" Easing="CubicInOut" Waveform="Sawtooth" />
    <Cycle Target="mainCircleRotation.Degrees" Low="0" High="360" Frequency="0.999" FrequencyBack="-1" Waveform="Sawtooth" MixOp="Add" />
</WhileTrue>
```

<!-- snippet-end -->

The `PulseForward` above sets off the animation that makes the plane fly away.
By keeping this animation in a separate `Timeline` we ensure it is played in its entirety, even though `IsLoading` stays active for shorter than the duration of the animation.

<!-- snippet-begin:code/SendButton.ux:FlightAnimation -->

```
<Timeline ux:Name="flightAnimation">
    <Move Target="icon" RelativeTo="Size" KeyframeInterpolation="Smooth">
        <Keyframe Time="0" X="0" Y="0" />
        <Keyframe TimeDelta="0.38" X="-0.8" Y="0.5" />
        <Keyframe TimeDelta="0.3" X="20" Y="-12" />
    </Move>
    <Rotate Target="icon" Degrees="25" Duration="0.4" Easing="SinusoidalInOut" DurationBack="0" />
    <Scale Target="icon" Factor="0" Easing="CubicIn" Delay="0.38" Duration="0.3" />
    <Change icon.Opacity="0" Duration="0.25" Delay="0.38" Easing="CubicInOut" />
</Timeline>
```

<!-- snippet-end -->

And that's it! Here is our finished button:

<!-- snippet-begin:code/SendButton.ux:SendButton -->

```
<Panel ux:Class="SendButton" Width="70" Height="70" IsLoading="false" PrimaryColor="#F73859" SecondaryColor="#fff">
    <bool ux:Property="IsLoading" />
    
    <float4 ux:Property="PrimaryColor" />
    <float4 ux:Property="SecondaryColor" />

    <Image ux:Name="icon" StretchMode="Uniform" Width="24" Alignment="Center" Offset="-2,1" Color="{ReadProperty this.SecondaryColor}">
        <MultiDensityImageSource>
            <FileImageSource Density="1" File="Assets/fa-paper-plane@1x.png" />
            <FileImageSource Density="2" File="Assets/fa-paper-plane@2x.png" />
            <FileImageSource Density="4" File="Assets/fa-paper-plane@4x.png" />
        </MultiDensityImageSource>
    </Image>
    
    <Circle ux:Name="tapOverlayCircle" Color="#fff6" Opacity="0" />
    <WhilePressed>
        <Change tapOverlayCircle.Opacity="1" Duration="0.05" DurationBack="0.15" />
    </WhilePressed>
    
    <Circle ux:Name="mainCircle" StartAngleDegrees="0" LengthAngleDegrees="360">
        <Stroke ux:Name="mainCircleStroke" Width="100" Color="{ReadProperty this.PrimaryColor}"  />
        <Rotation ux:Name="mainCircleRotation" />
    </Circle>
    
    <Circle>
        <Stroke Width="5" Color="#6662" />
        <Scaling Factor="0.85" />
    </Circle>

    <Circle Color="{ReadProperty this.SecondaryColor}" Margin="1">
        <Shadow Angle="90" Size="5" Distance="3" Color="#0005" />
    </Circle>
    
    <Timeline ux:Name="flightAnimation">
        <Move Target="icon" RelativeTo="Size" KeyframeInterpolation="Smooth">
            <Keyframe Time="0" X="0" Y="0" />
            <Keyframe TimeDelta="0.38" X="-0.8" Y="0.5" />
            <Keyframe TimeDelta="0.3" X="20" Y="-12" />
        </Move>
        <Rotate Target="icon" Degrees="25" Duration="0.4" Easing="SinusoidalInOut" DurationBack="0" />
        <Scale Target="icon" Factor="0" Easing="CubicIn" Delay="0.38" Duration="0.3" />
        <Change icon.Opacity="0" Duration="0.25" Delay="0.38" Easing="CubicInOut" />
    </Timeline>
    <WhileTrue Value="{ReadProperty this.IsLoading}">
        <PulseForward Target="flightAnimation" />
        <Change icon.Opacity="0" Delay="0.68" DurationBack="0.1" Easing="CubicInOut" />
        
        <Scale Target="mainCircle" Factor="0.85" Duration="0.5" Easing="BackInOut" Delay="0.35" />
        <Change mainCircleStroke.Width="5" Duration="0.2" Delay="0.35" Easing="SinusoidalInOut" />
        <Change mainCircle.LengthAngleDegrees="90" Duration="0.5" Delay="0.75" DelayBack="0" Easing="CubicInOut" />
        <Change spin.Value="true" Delay="1.4" />
    </WhileTrue>
    <WhileTrue ux:Name="spin">
        <Cycle Target="mainCircleRotation.Degrees" Low="0" High="360" Frequency="0.666" Easing="CubicInOut" Waveform="Sawtooth" />
        <Cycle Target="mainCircleRotation.Degrees" Low="0" High="360" Frequency="0.999" FrequencyBack="-1" Waveform="Sawtooth" MixOp="Add" />
    </WhileTrue>
</Panel>
```

<!-- snippet-end -->

## Using the button

Now, the button doesn't automatically play the animation when clicked, since it doesn't know how long to play it for.
Instead, we have to change `IsLoading` to `true` when the button is clicked, then perform some time-expensive operation, and set it back to `false` when the operation has completed.
Below is an example of how you can use the button in your own app.

```
<JavaScript>
    var Observable = require("FuseJS/Observable");
    
    var isLoading = Observable(false);
    
    function clicked() {
        isLoading.value = true;
        
        setTimeout(function() {
            isLoading.value = false;
        }, 3000);
    }
    
    module.exports = {
        isLoading: isLoading,
        clicked: clicked
    };
</JavaScript>

<SendButton Clicked="{clicked}" IsLoading="{isLoading}" />
```

