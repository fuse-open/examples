In this example we will implement a pull to reload design inspired by [this cool piece](https://dribbble.com/shots/1974767-gear-powered-pull-to-refresh-animation) by Michael B. Mayers.

# The PullToReload-trigger

Fuse has a `Trigger` called `PullToReload`, which we use in this example. The following snippet illustrates its basic usage.
```
<PullToReload IsLoading="{isLoading}" ReloadHandler="{reloadHandler}">
	<State ux:Binding="Pulling">
		<!-- active while pulling -->
	</State>
	<State ux:Binding="PulledPastThreshold">
		<!-- activates when pulled past threshold -->
	</State>
	<State ux:Binding="Loading">
		<!-- active while loading -->
	</State>
</PullToReload>
```

`PullToReload` has three properties which we can bind `State` objects to. Each `State` is activated in response to different parts of the pull gesture as commented on in the snippet.

The `ScrollView` normally allows the user to scroll a bit past the top and snaps back as it gets released. Since we want to control this behavior ourselves, we set the `SnapMinTransform` property of the `ScrollView` to `false`:
```
<ScrollView SnapMinTransform="false"/>
```

# The loading content

When pulling on the `ScrollView`, we show a `Panel` containing our cog wheel images. We define two classes for our cog wheels, that inherit from another class, `Cog`:

<!-- snippet-begin:code/CogWheelReload.ux:CogWheelClasses -->

```
<Image ux:Class="Cog" MaxHeight="200" MaxWidth="200" Alignment="Center" />
<Cog ux:Class="SmallCog" Color="#3e64b7" File="Assets/cog1_white.png" Width="60" Height="60" />
<Cog ux:Class="BigCog" Color="#3562c4" File="Assets/cog2_white.png" Width="120" Height="120" />
```

<!-- snippet-end -->

We can then place the cog wheels in our loading `Panel` using the `Offset` property and `Rotation` transform to make sure they fit together nicely. We also add a larger `Translation` to the `BigCog` elements so that we can animate them into place as we reach the pull threshold.
<!-- snippet-begin:code/CogWheelReload.ux:CogPanel -->

```
<Panel ux:Name="cogPanel" Background="#183A6F" Alignment="Top" Height="0" MinHeight="0">
    <SmallCog ux:Name="cog1">
        <Rotation ux:Name="cog1Rotation" />
    </SmallCog>
    <BigCog ux:Name="cog2" Offset="60,-60">
        <Translation ux:Name="cog2Trans" Y="-100" />
        <Rotation ux:Name="cog2Rotation" />
    </BigCog>
    <BigCog ux:Name="cog3" Offset="-60,60">
        <Translation ux:Name="cog3Trans" Y="100" />
        <Rotation Degrees="10" />
        <Rotation ux:Name="cog3Rotation" />
    </BigCog>
    <BigCog ux:Name="cog4" Offset="130,30">
        <Translation ux:Name="cog4Trans" Y="100" />
        <Rotation Degrees="5" />
        <Rotation ux:Name="cog4Rotation" />
    </BigCog>
    <BigCog ux:Name="cog5" Offset="-100, -47">
        <Translation ux:Name="cog5Trans" Y="-100" />
        <Rotation Degrees="8" />
        <Rotation ux:Name="cog5Rotation" />
    </BigCog>
</Panel>
```

<!-- snippet-end -->

`ScrollingAnimation` is used to animate the `Height` of our loading panel as we pull on the `ScrollView`. We set the `Range` property to `SnapMin`, which means that this animator will activate over the "SnapMin" range of our `ScrollView`. We also animate the rotation of our cog wheels for a nice effect.

<!-- snippet-begin:code/CogWheelReload.ux:ScrollingAnimation -->

```
<ScrollingAnimation Range="SnapMin">
    <Change cogPanel.Height="150" Duration="1" />
    <Change cog1Rotation.Degrees="200" Duration="1" />
    <Change cog2Rotation.Degrees="-93" Duration="1" />
    <Change cog3Rotation.Degrees="-93" Duration="1" />
    <Change cog4Rotation.Degrees="93" Duration="1" />
    <Change cog5Rotation.Degrees="93" Duration="1" />
</ScrollingAnimation>
```

<!-- snippet-end -->

We want to move the big cog wheels into place when we have reached the reload threshold. We do that by playing a `Timeline` which animates the `Translations` we added earlier:

<!-- snippet-begin:code/CogWheelReload.ux:Timeline -->

```
<Timeline ux:Name="moveCogsIntoPlace">
    <Change cog2Trans.Y="0" Duration="0.6" DurationBack="0.2" Easing="CircularOut" />
    <Change cog3Trans.Y="0" Duration="0.6" DurationBack="0.2" Easing="CircularOut" />
    <Change cog4Trans.Y="0" Duration="0.6" DurationBack="0.2" Easing="CircularOut" />
    <Change cog5Trans.Y="0" Duration="0.6" DurationBack="0.2" Easing="CircularOut" />
</Timeline>
```

<!-- snippet-end -->

We use the `Spin` animator to rotate the cog wheels endlessly while our `PullToReload` trigger is in the Loading state:

<!-- snippet-begin:code/CogWheelReload.ux:LoadingAnimator -->

```
<State ux:Binding="Loading">
    <Change retainSpace.Value="true" DurationBack="0.5" />
    <Spin Target="cog1" Frequency="0.53125" />
    <Spin Target="cog2" Frequency="-0.25" />
    <Spin Target="cog3" Frequency="-0.25" />
    <Spin Target="cog4" Frequency="0.25" />
    <Spin Target="cog5" Frequency="0.25" />
    <TimelineAction Target="moveCogsIntoPlace" How="PlayTo" When="Backward" Progress="0" />
</State>
```

<!-- snippet-end -->

We also activate a `WhileTrue` trigger which makes sure our loading panel maintains a certain size:

<!-- snippet-begin:code/CogWheelReload.ux:RetainSpace -->

```
<WhileTrue ux:Name="retainSpace">
    <Change cogPanel.MinHeight="75" Duration="0" DurationBack="0.3" Easing="CircularIn" />
</WhileTrue>
```

<!-- snippet-end -->


# Adding it to our app

The last thing we do is to add our custom pull to reload component to our app:

<!-- snippet-begin:code/MainView.ux:App -->

```
<ScrollView SnapMinTransform="false">
    <DockPanel>
        <CogWheelReload Dock="Top" />
        <StackPanel Background="#fff">
            <Shadow ux:Name="dropShadow" Color="#333" Distance="10" Size="10" Angle="180" />
            <Each Count="15">
                <Panel Margin="0,0,0,2" Height="70" Background="#ddd" />
            </Each>
        </StackPanel>
    </DockPanel>
</ScrollView>
```

<!-- snippet-end -->

We also have a few lines of JavaScript to simulate the loading state:
<!-- snippet-begin:code/MainView.ux:Javascript -->

```
<JavaScript>
    var Observable = require("FuseJS/Observable");

    function endLoading(){
        isLoading.value = false;
    }

    function reloadHandler(){
        isLoading.value = true;
        setTimeout(endLoading, 3000);
    }

    var isLoading = Observable(false);

    module.exports = {
        isLoading: isLoading,
        reloadHandler: reloadHandler
    };
</JavaScript>
```

<!-- snippet-end -->
