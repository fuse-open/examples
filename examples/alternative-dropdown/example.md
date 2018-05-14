In this example we will create a button that serves as a good alternative to a dropdown.

We noticed this clever approach in the [MailChimp](https://mailchimp.com/) mobile app and decided to recreate it in Fuse. That little button, when clicked, simply cycles through a list of modes unlike a dropdown menu that you select things from.

# Basic app structure

Our `MainView.ux` holds the basic markup for our apps layout, as well as the top-level `JavaScript` data context responsible for showing the right numbers, depending on the selected value in our alternative dropdown button.

Here's what the module that feeds our view looks like. `modes` holds the labels for the button, and `data` holds the numbers for all of the modes. `info` is a reactive Observable that picks a subset of data from the `data` array, depending on what the value of `currentMode` Observable is.
```
<JavaScript>
    var Observable = require("FuseJS/Observable");
    var currentMode = Observable(0);

    var modes = [
        {label: "7 DAYS"},
        {label: "30 DAYS"},
        {label: "90 DAYS"},
        {label: "1 YEAR"}
    ];

    var data = [
        {change: 492, total: 12774, unsubs: 327},
        {change: 1279, total: 42891, unsubs: 1183},
        {change: 2631, total: 99657, unsubs: 2478},
        {change: 7395, total: 125758, unsubs: 5492}
    ];

    var info = currentMode.map(function(x) {
        return data[x];
    });

    module.exports = {
        modes: modes,
        currentMode: currentMode,
        info: info
    };
</JavaScript>
```

The UX code responsibe for showing the data and the alternative dropdown button is just a `Grid` with 4 cells. Three of the cells hold our statistics and one is occupied by our `CycleButton` component.
```
<Grid RowCount="2" ColumnCount="2" Margin="16">
    <StackPanel Alignment="CenterLeft">
        <Text Value="{info.change}" TextColor="DarkerTextColor" FontSize="24" />
        <Text Value="Audience change" TextColor="LighterTextColor" FontSize="15" />
    </StackPanel>

    <CycleButton Width="100" Alignment="TopRight" Modes="{modes}" CurrentMode="{currentMode}" />
    
    <StackPanel Alignment="CenterLeft">
        <Text Value="{info.total}" TextColor="DarkerTextColor" FontSize="18" />
        <Text Value="Total audience" TextColor="LighterTextColor" FontSize="15" />
    </StackPanel>
    <StackPanel Alignment="CenterLeft">
        <Text Value="{info.unsubs}" TextColor="DarkerTextColor" FontSize="18" />
        <Text Value="Unsubs/Bounces" TextColor="LighterTextColor" FontSize="15" />
    </StackPanel>
</Grid>
```

As you can see, we're passing `{modes}` and `{currentMode}` as properties to the `CycleButton` component, so let's take a look at how that is implemented.

# The alternative dropdown component

Our custom `CycleButton` component is a simple `Rectangle` that accepts two properties: `Modes` and `CurrentMode`.
```
<Rectangle ux:Class="CycleButton" Padding="4" StrokeColor="BorderColor" StrokeWidth="2" Color="White" CornerRadius="2">
    <object ux:Property="Modes" />
    <int ux:Property="CurrentMode" />
```

Inside of the parent `Rectangle`, we put a `DockPanel` which holds the current selected mode label, as well as a list of dots to represent the number of modes (and highlight the active one). To determine which mode is active, we use the `index()` UX expression, available when we're inside of an `Each` tag - all we need to do is compare the index to the current selected mode.
```
<DockPanel>
    <Text Value="{currentModeLabel}" Alignment="Center" Dock="Fill" Color="LightBlue"/>
    <Panel Margin="8,0,0,0" Width="16" Dock="Right">
        <StackPanel Alignment="Center" ItemSpacing="2">
            <Each Items="{ReadProperty Modes}">
                <ModeIndicator IsActive="index() == {currentMode}" />
            </Each>
        </StackPanel>
    </Panel>
</DockPanel>

<Circle ux:Class="ModeIndicator" IsActive="false" Width="4" BoxSizing="FillAspect" Color="BorderColor">
    <bool ux:Property="IsActive" />
    <WhileTrue Value="{Property IsActive}">
        <Change this.Color="LightBlue" />
    </WhileTrue>
</Circle>
```

Finally, we add a `Clicked` handler on the component so that we can cycle through the different modes.
```
<Clicked Handler="{nextState}" />
```

Our component has its own `JavaScript` module responsible for cycling through the different modes and picking the right label to show on the button. Since variables that are passed to components via `ux:Property` are implicitly available as derived Observables in `JavaScript`, we can refer to them using the `this.*` syntax. That way, we get access to both the list of modes passed to the component, as well as the original instance of the `currentMode` Observable. That is very important, because we want the parent data context to know about mode changes in our component, so that it can select the right subset of data to show in UI.
```
<JavaScript>
    var modes = this.Modes;
    var currentMode = this.CurrentMode;

    var currentModeLabel = currentMode.combineLatest(modes, function(idx, list) {
        return list[idx].label;
    });

    function nextState() {
        currentMode.value = (currentMode.value + 1) % modes.value.length;
    }

    module.exports = {
        nextState: nextState,
        currentMode: currentMode,
        currentModeLabel: currentModeLabel
    }
</JavaScript>
```

And that's about it. Feel free to download the full example, play with it and use it in your own projects!
