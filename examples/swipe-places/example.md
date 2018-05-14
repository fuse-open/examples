In this example we will implement the card swipe control made famous by Tinder. Swipe to the right if you've visited a city, to the left if you have not.

To achieve the desired effect, we will use `PointAttractor` and `Draggable` to give elements the ability to respond to dragging gestures. `PointAttractor` will create a zone that the elements are attracted to. We will use three of these to create the Tinder sorting effect.

Let's start by looking at the JavaScript:

<!-- snippet-begin:code/MainView.ux:JavaScript -->

```
<JavaScript>
    var Observable = require("FuseJS/Observable");

    var resetting = Observable(false);

    function City(name, imageKey, country, visitors, nonVisitors)
    {
        this.name = name;
        this.imageKey = imageKey;
        this.country = country;
        this.visitors = Observable(visitors);
        this.nonVisitors = Observable(nonVisitors);
        this.degrees = -4 + (8 * Math.random());
    }

    function reset(x) {
        resetting.value = true;
        setTimeout(backToNormal, 300);
    }

    function backToNormal() {
        resetting.value = false;
    }

    module.exports = {
        cities : [
            new City("Oslo", "Assets/Cities/Oslo.jpg", "NORWAY", 3127, 3943),
            new City("Paris", "Assets/Cities/Paris.jpg", "FRANCE", 10250, 400),
            new City("San Francisco", "Assets/Cities/San Francisco.jpg", "USA", 6700, 5421),
            new City("Seoul", "Assets/Cities/Seoul.jpg", "KOREA", 5713, 4702),
            new City("Tokyo", "Assets/Cities/Tokyo.jpg", "JAPAN", 4512, 657)
        ],
        visited: function(x)
        {
            debug_log("Visited " + x.data.name);
        },
        notVisited: function(x)
        {
            debug_log("Not visited " + x.data.name);
        },
        addVisitor: function(x)
        {
            x.data.visitors.value = x.data.visitors.value + 1;
        },
        removeVisitor: function(x)
        {
            x.data.visitors.value = x.data.visitors.value - 1;
        },
        addNonVisitor: function(x)
        {
            x.data.nonVisitors.value = x.data.nonVisitors.value + 1;
        },
        removeNonVisitor: function(x) {
            x.data.nonVisitors.value = x.data.nonVisitors.value - 1;
        },
        reset: reset,
        resetting: resetting
    };

    reset();
</JavaScript>
```

<!-- snippet-end -->

Points of interest:

- We create a bunch of cities that are to be swiped left or right
- As the number of visitors will update on the cards, we make them `Observable`
- We also begin implementing a `reset` function to allow us to swipe through the stack multiple times. This is implemented using an `Observable` boolean which indicates whether the stack is currently in the process of resetting itself. We'll look at how to implement this in UX later
- The resetting `Observable` is flipped on a timer using `setTimeout`

## Each

We create a bunch of panels to hold the card definitions. These are rotated according to the random value set when the city is initialized:

<!-- snippet-begin:code/MainView.ux:Each -->

```
<Each Items="{cities}">
    <Panel Margin="0" Height="400" Width="293">
        <Rotation Degrees="{degrees}" />
```

<!-- snippet-end -->

This gives the stack a more organic look.

## Point attractors

Outside the `Each`-loop, we setup the point attractors:

<!-- snippet-begin:code/MainView.ux:PointAttractors -->

```
<PointAttractor ux:Name="centerAttractor" Radius="800" Strength="250" />
<PointAttractor ux:Name="notVisitedAttractor" Offset="-400,0,0" Radius="380" Strength="600" />
<PointAttractor ux:Name="visitedAttractor" Offset="400,0,0" Radius="380" Strength="600" />
```

<!-- snippet-end -->

The center attractor has its parameters governed by the resetting `Observable`:

<!-- snippet-begin:code/MainView.ux:WhileTrues -->

```
<WhileTrue Value="{resetting}">
    <Change centerAttractor.Radius="8000" />
    <Change centerAttractor.Strength="10000" />
</WhileTrue>
```

<!-- snippet-end -->

When the stack is resetting, the attractor becomes bigger and stronger. When it is not resetting, it gives a gentle attraction towards the center of the screen, covering a large area.

## Overlays

To give some visual indication that the city has been visited or not visited, we add some image overlays over the city picture with `Opacity` of 0.

<!-- snippet-begin:code/MainView.ux:images -->

```
<Image ux:Name="visitedOverlay" StretchMode="UniformToFill" File="Assets/Emblems/VisitedOverlay.png" Opacity="0" Dock="Fill" />
<Image ux:Name="notVisitedOverlay" StretchMode="UniformToFill" File="Assets/Emblems/NotVisitedOverlay.png" Opacity="0" Dock="Fill" />
<Rectangle Dock="Fill">
	<ImageFill File="{imageKey}" StretchMode="UniformToFill" WrapMode="ClampToEdge" />
</Rectangle>
```

<!-- snippet-end -->

The `Opacity` can then be increased using an `InForceFieldAnimation`:

<!-- snippet-begin:code/MainView.ux:InForcefieldAnimation -->

```
<InForceFieldAnimation ForceField="visitedAttractor" From="0.1" To="0.3">
    <Change visited.Opacity="1" Duration="1" />
    <Change visitedOverlay.Opacity="1" Duration="1" />
</InForceFieldAnimation>
```

<!-- snippet-end -->

## Rotation

We can also add a slight rotation to the card as it is being swiped, also using an `InForceFieldAnimation`:

<!-- snippet-begin:code/MainView.ux:Rotating -->

```
<InForceFieldAnimation ForceField="visitedAttractor">
    <Rotate Degrees="-8" />
</InForceFieldAnimation>
```

<!-- snippet-end -->

## Visitor updating

To update the visitor count correctly as the card is swiped, we use `EnteredForceField` and `ExitedForceField`

<!-- snippet-begin:code/MainView.ux:EnteredForcefield -->

```
<EnteredForceField ForceField="visitedAttractor" Threshold="0.05" Handler="{addVisitor}" />
<ExitedForceField ForceField="visitedAttractor" Threshold="0.05" Handler="{removeVisitor}" />
<EnteredForceField ForceField="notVisitedAttractor" Threshold="0.05" Handler="{addNonVisitor}" />
<ExitedForceField ForceField="notVisitedAttractor" Threshold="0.05" Handler="{removeNonVisitor}" />
```

<!-- snippet-end -->

## Draggable behavior

To make the cards draggable, we add the `Draggable` behavior:

<!-- snippet-begin:code/MainView.ux:Draggable -->

```
<Draggable />
```

<!-- snippet-end -->

We can also animate the city in response to being dragged:

<!-- snippet-begin:code/MainView.ux:WhileDragging -->

```
<WhileDragging>
    <BringToFront />
    <Scale Factor="1.1" Duration="0.5" Easing="BackOut" />
    <Change shadow.Distance="20" Duration="0.5" Easing="BackOut" />
    <Change shadow.Size="20" Duration="0.5" Easing="BackOut" />
</WhileDragging>
```

<!-- snippet-end -->

## Reset button

Finally, we hook up the reset icon to call the reset JavaScript function:

<!-- snippet-begin:code/MainView.ux:ImageClicked -->

```
<Panel>
    <Image Clicked="{reset}" Height="64" Width="64" File="Assets/Icons/Refresh.png" />
</Panel>
```

<!-- snippet-end -->
