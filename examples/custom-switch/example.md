Fuse has a flexible and powerful system to help you style your own and existing controls. Let's look at how it works by styling a `ToggleControl` control inspired by [this](https://dribbble.com/shots/1749645-Contact-Sync) example by [Ramotion](http://www.ramotion.com).

In this example, we'll be using:

- Subclassing of the `ToggleControl` to create our own control that has multiple states (in this case, on and off)
- Allowing the user of the control to choose between swiping the thumb using `SwipeGesture`, or clicking the control to toggle the state

The main things we are addressing with our styling are:

- The control should have a border that turns white when it is `On`
- The background of the track should turn transparent as the control is turned `On`


The code for the custom switch is simply:

<!-- snippet-begin:code/MySwitch.ux:ToggleControl -->

```
<ToggleControl ux:Class="MySwitch" Margin="4,4,4,4"
               Width="80"
               Height="48" Focus.IsFocusable="true">
    <Clicked>
        <Toggle Target="this" />
    </Clicked>
    <SwipeGesture ux:Name="swipe" Direction="Right" Length="38" Type="Active" IsActive="{Property Value}"/>
   
    <SwipingAnimation Source="swipe">
        <Move Target="thumb" X="38"/>
        <Change thumb.Color="#fff"/>
        <Change track.Color="#ffffff00"/>
        <Change track.StrokeColor="#ffffffff"/>
    </SwipingAnimation>
   
    <Panel Layer="Background">
        <Circle ux:Name="thumb" Color="#fafafaff" 
                Alignment="CenterLeft"
                Width="34"
                Height="34"
                Margin="4,0,14,0">
            <Shadow Angle="90" Distance="0" Size="2" />
        </Circle>
   
        <Rectangle ux:Name="track" Color="#e7e7e7" StrokeColor="#ffffff00"
            CornerRadius="23"
            Width="80"
            Height="40"
            Alignment="Center"/>
    </Panel>

    <WhileDisabled>
        <Change thumb.Color="#bdbdbdff" Easing="QuadraticInOut" Duration="0.25" />
        <Change track.Color="#0000001e" Easing="QuadraticInOut" Duration="0.25" />
    </WhileDisabled>

    <WhileTrue>
        <Change thumb.Color="#fff" Easing="QuadraticInOut" Duration="0.25" />
        <Change track.Color="#fff0" Easing="QuadraticInOut" Duration="0.25" />
        <Change track.StrokeColor="#ffff" Easing="QuadraticInOut" Duration="0.25" />
    </WhileTrue>
</ToggleControl>
```

<!-- snippet-end -->

And that's basically it for styling the `ToggleControl` itself. The rest of the effect is accomplished as we react to the state change of the `ToggleControl`-control. Let's look at some of the points of interest:

<!-- snippet-begin:code/MySwitch.ux:Clicked -->

```
<Clicked>
    <Toggle Target="this" />
</Clicked>
```

<!-- snippet-end -->

A typical behavior for a switch is to toggle it's state when clicked. We can simply modify the state of the `ToggleControl` directly.

<!-- snippet-begin:code/MySwitch.ux:SwipeGesture -->

```
<SwipeGesture ux:Name="swipe" Direction="Right" Length="38" Type="Active" IsActive="{Property Value}"/>
```

<!-- snippet-end -->

We add a `SwipeGesture` with the same length as the travel of the switch thumb. We bind the `IsActive` state of the gesture to the `Value` of the `ToggleControl`. This keeps the states synchronized, and what allows us to just call `Toggle` on the switch in the `Clicked` handler.

We use `SwipingAnimation` to define the change in appearance when the toggle is on. This works both when the user is sliding the thumb and when the state is toggled.

<!-- snippet-begin:code/MySwitch.ux:SwipingAnimation -->

```
<SwipingAnimation Source="swipe">
    <Move Target="thumb" X="38"/>
    <Change thumb.Color="#fff"/>
    <Change track.Color="#ffffff00"/>
    <Change track.StrokeColor="#ffffffff"/>
</SwipingAnimation>
```

<!-- snippet-end -->


### The surrounding areas

In the areas surrounding the switch, we want to:

- Create a `Circle` that expands and contracts as the switch is toggled
- Subtly change the colors of the icons
- Change the `Color` of the `Header` (inline class)
- Change the `Color` in the background when the `Circle` has expanded to its maximum value
- Make sure the expanding `Circle` never extends outside its containing `DockPanel`, this is achieved using `ClipToBounds="true"`
- Slightly move the `Circle` as the switch changes state, so it expands from and contracts to slightly different locations

#### Code

We're using these _inline classes_:

```
<Text ux:Class="HeaderText" FontSize="24" TextColor="#11abfe" TextAlignment="Center" />
<Text ux:Class="DescriptionText" TextWrapping="Wrap" FontSize="14" TextAlignment="Center" />
<Image ux:Class="Icon" Height="80" Width="80" Color="#dedede" />
```

Then the configuration entry becomes just a matter of:

<!-- snippet-begin:code/CustomSwitch.ux:Entry -->

```
<DockPanel ux:Name="secondBox" Color="White" ClipToBounds="true">
    <StackPanel Dock="Top" ItemSpacing="35" Margin="4,35,4,0">
        <HeaderText ux:Name="secondHeader" Value="Automatic synchronization"/>
        <DescriptionText Value="Synchronize all contact information when connecting using USB"/>
    </StackPanel>

    <StackPanel Orientation="Horizontal" Alignment="Center" ItemSpacing="35">
        <Icon File="Assets/Connect.png" ux:Name="connectIcon" />
        <Panel>
            <MySwitch Alignment="VerticalCenter">
                <WhileTrue>
                    <Change greenCircleScaling.Factor="10" Duration="0.25" Easing="QuadraticOut" Delay="0.20" />
                    <Change secondHeader.TextColor="White" Duration="0.25" Delay="0.20" />
                    <Change connectIcon.Color="White" Duration="0.25" Delay="0.35" />
                    <Change greenCircle.Color="#8cb542" Duration="0.25" Easing="QuadraticOut" Delay="0.20" />
                    <Change secondBox.Color="#8cb542" Duration="0.05" Delay="0.35" />
                    <Change greenCircleTranslation.X="19" Duration="0" DurationBack="0" Delay="0.25" Easing="QuadraticInOut"/>
                </WhileTrue>
            </MySwitch>
            <Circle ux:Name="greenCircle" Color="White">
                <Translation ux:Name="greenCircleTranslation" X="-19" />
                <Scaling ux:Name="greenCircleScaling" Factor="0" />
            </Circle>
        </Panel>
    </StackPanel>
</DockPanel>
```

<!-- snippet-end -->
