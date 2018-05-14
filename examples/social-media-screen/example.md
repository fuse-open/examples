Here we combine several different UX and JavaScript features to create a more involved app screen.

This app screen shows how to:

- Summon and dismiss the side menu using a `Clicked`-event and an `EdgeNavigator`
- Move the main client area in response to the sidebar coming on screen
- Animate the side menu icon, turning a familiar "hamburger"-menu icon into a "cross" dismiss-icon using `Translation`-, `Rotation`- and `Opacity`-transforms
- Animate the menu choices in the side bar using the `WhileInactive`-trigger
- Create a typical circular portrait picture using `Circle` with `ImageFill`
- Easily databind news items to JavaScript fetched from the net

Let's take a look at points of interest from this example:

### App.ux

Handling the sidebar open state and getting Json from the network:

<!-- snippet-begin:code/MyApp.ux:FetchingJson -->

```
var Observable = require("FuseJS/Observable");

var sidebarOpen = Observable(false);
function setSidebarOpen() {
    sidebarOpen.value = true;
};
function setSidebarClosed() {
    sidebarOpen.value = false;
};

var data = Observable();

fetch('http://az664292.vo.msecnd.net/files/P6FteBeij9A7jTXL-edgenavresponse.json')
.then(function(response) { return response.json(); })
.then(function(responseObject) { data.value = responseObject; });

module.exports = {
    data: data,
    sidebarOpen: sidebarOpen,
    setSidebarOpen: setSidebarOpen,
    setSidebarClosed: setSidebarClosed
};
```

<!-- snippet-end -->

We want to show an instance of `NewsItem` per entry in the JSON. This is accomplished using `Each`. Note that `responseData` is part of the server side datastructure, and not a "magic" term; we are referencing straight into the JSON:

<!-- snippet-begin:code/MyApp.ux:MainScrollArea -->

```
<!-- This is the main scroll area -->
<ScrollView>
    <StackPanel Alignment="Top">
        <Panel Height="7" />
        <Each Items="{data.responseData}">
            <NewsItem />
        </Each>
    </StackPanel>
</ScrollView>
```

<!-- snippet-end -->

We add the side menu to the main `EdgeNavigator`. When the `Sidebar` (defined in `Sidebar.ux`) is activating, we move the main area of the app and animate the hamburger icon into a cross:

<!-- snippet-begin:code/MyApp.ux:AddAPanel -->

```
<!-- Add a panel to the left edge -->
<Sidebar Width="180" ux:Name="menu" EdgeNavigation.Edge="Left">
    <ActivatingAnimation>
        <Change mainAppTranslation.X="180" />
        <!-- Change to cross out -->
        <Change topMenuTranslation.Y="0" />
        <Change bottomMenuTranslation.Y="0" />
        <Change middleRectangle.Opacity="0" Easing="CircularOut" />
        <Change topMenuRotation.Degrees="45" Easing="ExponentialIn" />
        <Change bottomMenuRotation.Degrees="-45" Easing="ExponentialIn" />
        <Change topRectangle.Width="28" />
        <Change bottomRectangle.Width="28" />
    </ActivatingAnimation>
    <WhileActive>
        <Callback Handler="{setSidebarOpen}" />
    </WhileActive>
    <WhileInactive>
        <Callback Handler="{setSidebarClosed}" />
    </WhileInactive>
</Sidebar>
```

<!-- snippet-end -->

To draw the hamburger menu and to allow for the above translations to work, we implement it like this:

<!-- snippet-begin:code/MyApp.ux:TheHamburger -->

```
<Panel Margin="7,5,5,5" Height="32" Width="32" HitTestMode="LocalBounds">
    <WhileTrue Value="{sidebarOpen}">
        <Clicked>
            <NavigateTo Target="content" />
        </Clicked>
    </WhileTrue>
    <WhileFalse Value="{sidebarOpen}">
        <Clicked>
            <NavigateTo Target="menu" />
        </Clicked>
    </WhileFalse>
    <Rectangle ux:Name="topRectangle" Height="2" Width="26" Color="#000">
        <Translation Y="-9" ux:Name="topMenuTranslation" />
        <Rotation ux:Name="topMenuRotation" />
    </Rectangle>
    <Rectangle ux:Name="middleRectangle" Height="2" Width="26" Color="#000" />
    <Rectangle ux:Name="bottomRectangle" Height="2" Width="26" Color="#000">
        <Translation Y="9" ux:Name="bottomMenuTranslation" />
        <Rotation ux:Name="bottomMenuRotation" />
    </Rectangle>
</Panel>
```

<!-- snippet-end -->

This `Panel` also holds commands to activate the `Sidebar`, which we've named `menu`, and the main content area, named `content`. We achieve this by using a `WhileTrue` and `WhileFalse` trigger that are bound to an Observable `bool` named `sidebarOpen`. We set this value from a `WhileActive` and `WhileInactive` triggers on the `Sidebar`.

### Sidebar.ux

The `Sidebar` is defined as a normal `StackPanel`. It implements the interesting animate-in effect on the menu items using the `WhileInactive`-trigger. This assumes the normal layout is the _rest state_ of the panel, and then we change it when it is _inactive_. We did the opposite in `App.ux` with `ActivatingAnimation` where we animated the hamburger-menu. Here is makes sense to let the active state be the state without any offsets applied to the elements, as it makes it easier to tweak the effect or remove it fully.

Using `WhileInactive` comes with certain caveats, however. Because it describes the state we are animating _from_ as the `Sidebar` becomes active, the animations will effectively be run _backwards_ when the menu becomes _visible_.

This makes sense if you push the `Sidebar` off the screen to its inactive state, you can see the `Chat` moving first, followed by the `Feed`, the `Browser` and so on.

<!-- snippet-begin:code/Sidebar.ux:TheSidebar -->

```
<StackPanel ux:Class="Sidebar" Background="#212831">
    <WhileInactive Threshold="0.4">
        <Move Target="user" X="-180" Duration="0.2" Delay="0.3" Easing="CircularIn" />
        <Move Target="stats" X="-180" Duration="0.2" Delay="0.2" Easing="CircularIn" />
        <Move Target="browser" X="-180" Duration="0.2" Delay="0.15" Easing="CircularIn" />
        <Move Target="feed" X="-180" Duration="0.2" Delay="0.1" Easing="CircularIn" />
        <Move Target="chat" X="-180" Duration="0.2" Delay="0.05" Easing="CircularIn" />
    </WhileInactive>

    ...
</StackPanel>
```

<!-- snippet-end -->

As you can see, the `WhileInactive`-trigger has a `Threshold`. This `Threshold` goes from `0` to `1` and decides when the `Trigger`-actions are to be run. Keep in mind that as this animation runs backwards, the smaller the number, the more visible the panel needs to be for the `Trigger`-actions to play.
