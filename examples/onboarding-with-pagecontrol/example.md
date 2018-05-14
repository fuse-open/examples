In this example we show how you can use the `PageControl` in combination with a `Timeline` to create an animated on-boarding experience. We got inspired by [this design](https://dribbble.com/shots/2629732-Snaplee-App-Interaction) by [Sumit Chakraborty](https://dribbble.com/bearduo).

## The PageControl

We first set up a `PageControl` with five child panels to represent the five pages. The `PageControl` considers all direct children to be pages.

Each page has some textual content with the same styling. Instead of copying things like `Color`, `FontSize` and `Margin` to all the pages, we create a reusable class:

<!-- snippet-begin:code/MainView.ux:PageContentClass -->

```
<Grid ux:Class="PageContent" Padding="45,0" MinHeight="100">
    <string ux:Property="Title" />
    <string ux:Property="Content" />
    <Grid Rows="2*,3*">
        <Text Alignment="Top" Value="{ReadProperty this.Title}" FontSize="20" Color="#000" TextAlignment="Center" TextWrapping="Wrap" Margin="20,0" />
        <Text Alignment="Top" Value="{ReadProperty this.Content}" FontSize="16" Color="#777" TextAlignment="Center" TextWrapping="Wrap"/>
    </Grid>
</Grid>
```

<!-- snippet-end -->

We can then use this class in each of our pages like so:

<!-- snippet-begin:code/MainView.ux:PageContentInstance -->

```
<PageContent ux:Name="page1Content" Alignment="Top" Row="1"
             Title="Discover places"
             Content="Find out about new cool places that value your feedback by using our Nearby venues feature." >
```

<!-- snippet-end -->

## The icons

We also make classes for all the icons we need and put them in their own file called Resources.ux.

<!-- snippet-begin:code/Resources.ux:AllTheIcons -->

```
<Image ux:Class="Chat" File="Assets/chat.png"/>
<Image ux:Class="Feed" File="Assets/feed.png"/>
<Image ux:Class="Marker" File="Assets/marker.png"/>
<Image ux:Class="Mountain" File="Assets/mountain.png"/>
<Image ux:Class="MountainFill" File="Assets/mountainFill.png" />
<Image ux:Class="Phone" File="Assets/phone.png"/>
<Image ux:Class="PhoneHands" File="Assets/phoneHands.png"/>
<Image ux:Class="Tags" File="Assets/tags.png"/>
<Image ux:Class="Map" File="Assets/map.png" />
```

<!-- snippet-end -->

## Animating the mountain

Since the mountain moves independently of all the pages, we put it in a layer of its own outside the `PageControl`.

<!-- snippet-begin:code/MainView.ux:MountainGrid -->

```
<Grid ux:Name="mountainsGrid" Rows="3*,1*">
    <Panel ux:Name="mountains" Width="70%" HitTestMode="None" Alignment="Center">
        <MountainFill ux:Name="mountainFill" Width="50%" Alignment="Bottom" Offset="-4.0%,-0.5%"/>
        <Mountain/>
        <AddingAnimation>
            <Move Y="-2" RelativeTo="Size" Duration="0.6" Easing="QuinticIn" />
            <Change  mountains.Opacity="0" Duration="0.6" Easing="QuinticIn" />
        </AddingAnimation>
    </Panel>
</Grid>
```

<!-- snippet-end -->

As our app loads, we want this to animate slightly from the bottom of the screen. We achieve this with an `AddingAnimation`. `AddingAnimation` is triggered as an element is rooted to the visual tree. In this case, that happens as soon as the app loads.

## The Timeline

We animate the whole thing using a `Timeline`. A `Timeline` acts as a normal `Trigger`, except that we can manually set a progress for it. The progress should be a value between 0 and 1. Since we want the page progress of our `PageControl` to make our `Timeline` progress, we put an `ActivatingAnimation` on the first child of out `PageControl`. We also set the `Scale` property to be 0.25, since we want this trigger to activate over all the five pages. The scale of 0.25 gives us the following progression values for the five pages: 0, 0.25, 0.5, 0.75 and 1.0. You can see these values more or less represented in the following code, with just a few tweaks.

<!-- snippet-begin:code/MainView.ux:TheTimeline -->

```
<Timeline ux:Name="timeline" TargetProgress="1">
    <Move Target="mountains" RelativeTo="Size">
        <Keyframe Y="-0.38"         Time="0.25"/>
        <Keyframe Y="-0.2"          Time="0.5"/>
        <Keyframe X="-0.3" Y="-0"   Time="0.75"/>
        <Keyframe X="0.2"  Y="-0.2" Time="0.98"/>
        <Keyframe X="0.08" Y="-0.2" Time="1.0"/>
    </Move>

    <Scale Target="mountains">
        <Keyframe Value="1"    Time="0.25"/>
        <Keyframe Value="0.65" Time="0.5"/>
        <Keyframe Value="0.2"  Time="0.75"/>
        <Keyframe Value="0.45" Time="0.98"/>
    </Scale>

    <Change Target="mountainFill.Opacity">
        <Keyframe Value="0.7" Time="0.0"/>
        <Keyframe Value="0"   Time="0.25"/>
        <Keyframe Value="0.7" Time="0.5"/>
        <Keyframe Value="0.7" Time="0.75"/>
        <Keyframe Value="0"   Time="1.0"/>
    </Change>

    <Change Target="mountains.Opacity">
        <Keyframe Value="0.25" Time="0.25"/>
        <Keyframe Value="1"    Time="0.5"/>
        <Keyframe Value="0"    Time="0.65"/>
        <Keyframe Value="0"    Time="0.85"/>
        <Keyframe Value="1"    Time="1.0"/>
    </Change>
</Timeline>
```

<!-- snippet-end -->

That's it! Feel free to download and play around with the example code.
