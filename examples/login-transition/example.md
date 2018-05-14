We saw this cool login transition from [Anton Aheichanka](https://dribbble.com/madebyanton) using the free to-do UI kit from [InVision](http://www.invisionapp.com/do) and wanted to recreate it in Fuse.

_Note: that this example is made to demonstrate the exact animations as shown in the original animated GIF, and is not focused on creating actual functionality in a login screen or calendar._

## The basic structure

This example has two parts; the "to logged in"-animation, and the "to logged out"-animation.

We start by defining the two screens separately and make sure the "logged out" screen is layered underneath the "logged in" screen. This allows for a nicer separation of concerns when animating.

## The first animation

We activate the "loading" `WhileTrue` trigger using a `Clicked` trigger. The "loading" trigger is in charge of coordinating the different aspects of the animation.
It does nothing more than to activate 4 other `WhileTrue` triggers with the correct timings.

<!-- snippet-begin:code/MainView.ux:LoadingWhileTrue -->

```
<WhileTrue ux:Name="loading">
    <Change changeWidth.Value="true" DelayBack="0"/>
    <Change loadCircle.Value="true" DelayBack="0"/>

    <Change scaleAndFade.Value="true" Delay="2.5" DelayBack="0"/>
    <Change showLoggedIn.Value="true" Delay="2.9" />
</WhileTrue>
```

<!-- snippet-end -->

The loading button is composed of a `Rectangle` with rounded corners. A `Circle` with a white `Stroke` is used as the loading indicator. After the loading is complete (in this case just because of a hard coded amount of seconds passing), the loading button is scaled up to fill the screen. This is done by scaling it to the size of an invisible, square `Panel` that is 5 times the size of the screen's smallest dimension:

<!-- snippet-begin:code/MainView.ux:TransitionScaleGuide -->

```
<Panel ux:Name="transitionScaleGuide" Width="500%" Height="500%" Alignment="Center" HitTestMode="None" BoxSizing="FillAspect" Aspect="1" />
```

<!-- snippet-end -->

The animation itself is performed using a `Scale` animator.

<!-- snippet-begin:code/MainView.ux:LoadingButtonScaleAnimation -->

```
<Scale Target="loadingButton" Factor="3" RelativeTo="SizeFactor" RelativeNode="transitionScaleGuide"
       Delay="0.01" Duration="0.7" Easing="ExponentialInOut" DurationBack="0" />
```

<!-- snippet-end -->

Because of how scaling works, we actually switch between the normal `Rectangle`, which defines our loading button, to a scaled down 240x240 `Circle` just as we are about to scale the button. This is to get rid of any ugly aliasing effects up-scaling the button would cause.

<!-- snippet-begin:code/MainView.ux:LoadingButtonPanel -->

```
<Panel Row="2" Width="150" Height="60">
    <Text ux:Name="text" Alignment="Center" Value="Sign in" FontSize="18" Color="#fff"/>
    <Panel ux:Name="loadingCirclePanel">
        <Circle ux:Name="loadingCircle" Width="70%" Height="70%" Opacity="0" StartAngleDegrees="0" LengthAngleDegrees="90">
            <Stroke Width="1" Brush="#fff" />
        </Circle>
    </Panel>
    <Clicked>
        <Toggle Target="loading" />
    </Clicked>
    <Rectangle ux:Name="rectNormalScale" CornerRadius="30" Color="#FF3366" Width="300" Height="60"/>
    <Circle ux:Name="loadingButton" Opacity="0" Alignment="Center" Layer="Background" Width="240" Height="240" Color="#FF3366">
        <Scaling Factor="0.25" />
    </Circle>
</Panel>
```

<!-- snippet-end -->

When the `Circle` fills the entire screen, we fade in the "loggedInView" `Panel`, along with a whole bunch of other tiny animations.

<!-- snippet-begin:code/MainView.ux:ShowLoggedInWhileTrue -->

```
<WhileTrue ux:Name="showLoggedIn">
    <Change loggedInView.Opacity="1" Delay="0.1" Duration="0.65" DurationBack="0.35" DelayBack="0.2" Easing="CubicInOut"/>
    <Change plusButton.Opacity="1" Delay="0.1" Duration="0.65" DurationBack="0.7" DelayBack="0.3" Easing="CubicInOut"/>

    <Change goodMorningText.Opacity="1" Duration="0.3" Delay="0.3"/>
    <Change monthPanel.Opacity="1" Duration="0.3" Delay="0.3"/>

    <Change weekTranslation.Y="0" Duration="0.8" Easing="QuadraticInOut"/>
    <Change weekScaling.Factor="1" Duration="0.8" Easing="QuadraticInOut"/>
    <Change headerScaling.Factor="1" Duration="0.6" Easing="CircularInOut" />
    <Change showPlusButton.Value="true" />
    <RaiseUserEvent EventName="ToggleLoggedIn" />

    <Change profile.Opacity="1" Delay="0.7" Duration="0.1" DurationBack="0.2" DelayBack="0"/>
    <Change profileScaling.Factor="1" Delay="0.7" Duration="0.4" Easing="CircularInOut"/>
    <Change loggedInView.IsEnabled="true" />
    <Change plusButton.IsEnabled="true" />
</WhileTrue>
```

<!-- snippet-end -->

## Logging out again

The second animation (going back to the "logged out view") is quite similar to the first one, but we also make sure to deactivate the "loading" `WhileTrue` trigger, so as to undo the previous animation.


## That is it!

Feel free to download and play with the example.
