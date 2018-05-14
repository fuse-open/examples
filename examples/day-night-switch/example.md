In this example we'll implement a custom switch control by using the `ToggleControl`, with separate forward and backward animations. The design was inspired by [this awesome piece](https://dribbble.com/shots/1907553-Day-Night-Toggle-Button) by [Ramakrishna V](https://dribbble.com/RamakrishnaUX) (which was later animated by [Tsuriel](https://dribbble.com/Tsuriel)).

In this example, we will:

* Define our own `ToggleControl` with our own custom animation
* Utilize the `BackwardAnimation` attribute on `TriggerAnimation` in order to get separate activating and deactivating animations
* Have animations with several steps, using `Keyframe`

# The classes

We create clases for the different elements of the switch. The following is the class used for stars:

<!-- snippet-begin:code/MainView.ux:StarClass -->

```
<Circle ux:Class="Star" Height="5" Width="5" Alignment="Center" Color="#fff" />
```

<!-- snippet-end -->

We define our moon craters in the same way:

<!-- snippet-begin:code/MainView.ux:MoonDotClass -->

```
<Circle ux:Class="MoonDot" Color="#fff">
    <Stroke Width="5" Color="#e3e7c7" />
</Circle>
```

<!-- snippet-end -->

The following is how the background of the switch is made. Note how `Offset` is used to offset the position of the stars.

<!-- snippet-begin:code/MainView.ux:Stars -->

```
<Panel Layer="Background">
    <Star ux:Name="star1" Offset="-5,-20" />
    <Star ux:Name="star2" Offset="5,20" />
    <Star ux:Name="star3" Offset="15,-10" />
    <Star ux:Name="star4" Offset="27,8" />
    <Star ux:Name="star5" Offset="38,-23" />
    <Star ux:Name="star6" Offset="48,0" />
    <Star ux:Name="star7" Offset="45,15" />
    <Rectangle ux:Name="background" CornerRadius="75" Color="#3c4145">
        <Stroke ux:Name="backgroundBorder" Color="#1c1c1c" Width="5" />
    </Rectangle>
</Panel>
```

<!-- snippet-end -->

The switch thumb is defined as follows:

<!-- snippet-begin:code/MainView.ux:Thumb -->

```
<Panel ux:Name="thumb" Alignment="CenterLeft" Margin="10,0,0,0" Width="50" Height="50">
    <MoonDot ux:Name="moonDot1" Width="17" Height="17" Alignment="TopRight" Offset="-7,3" />
    <MoonDot ux:Name="moonDot2" Width="11" Height="11" Alignment="TopLeft" Offset="13,8" />
    <MoonDot ux:Name="moonDot3" Width="12" Height="12" Alignment="BottomRight" Offset="-12,-6" />

    <Circle Color="#fff" ux:Name="thumbCircle">
        <Stroke ux:Name="thumbBorder" Width="3" Color="#e3e7c7" />
    </Circle>
</Panel>
<Panel ux:Name="thumbFollow" Alignment="CenterLeft" Margin="10,0,0,0" Width="50" Height="50">
    <Circle ux:Name="thumbFollowFill" Color="#fff" />
</Panel>
```

<!-- snippet-end -->

The rest of the UX are mostly animations. As mentioned, this example uses separate forward and backward animations (to give the stars a "bounce" effect when animating backward). It is also used to animate the colors at different speeds depending on the direction of the animation.

Like so:

<!-- snippet-begin:code/MainView.ux:SwipingAnimation -->

```
<SwipingAnimation Source="swipe">
    ...forward animations go here...
    <TriggerAnimation ux:Binding="BackwardAnimation">
        ...and backward animations go here...
    </TriggerAnimation>
</SwipingAnimation>
```

<!-- snippet-end -->

It is important to note that when you bind `BackwardAnimation` to an animation, all the animators lose their default backward animations. This means that they have to be explicitly re-defined in the bound `BackwardAnimation`.
