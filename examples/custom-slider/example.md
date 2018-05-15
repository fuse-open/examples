In this example, we're creating a custom slider component, inspired by this beautifully designed [Fluid Slider](https://dribbble.com/shots/3868232-ios-Fluid-Slider-ui-ux) by [Virgil Pana](https://dribbble.com/virgilpana).

The last time we published custom sliders was in our [Halftone effect](https://fuse-open.github.io/examples/halftone-effect) example, so it's time for a re-do. With this new one we especially liked the gooey animation when the circle leaves the slider. Since we've introduced the `Curve` element in Fuse, we can make it work almost exactly like that. Let's dive in!

# Create the slider component

We start out by creating a `ux:Class` with a set of properties that allows us to configure our new slider component.
```
<DockPanel ux:Class="GooeySlider">
    <float ux:Property="SliderHeight" />
    <string ux:Property="Label" />
    <float ux:Property="Min" />
    <float ux:Property="Max" />
    <float4 ux:Property="TintColor" />
    <float4 ux:Property="FaceColor" />
    <object ux:Property="Val" />

    <Text ux:Name="label" Value="{ReadProperty Label}" Color="{ReadProperty TintColor}" Width="width(this)"
        TextWrapping="Wrap" Alignment="Center" TextAlignment="Center" Margin="8" />

</DockPanel>
```
Next, we add labels on the left and right sides of the slider component to show the `Min` and `Max` values. In the middle, we put a `RangeControl` that occupies the remaining available space between the two labels. You can see how we take advantage of [UX expressions](https://fuse-open.github.io/docs/ux-markup/expressions) to dynamically calculate the `Margin` for the `RangeControl`: `Margin="{ReadProperty SliderHeight}/2,0"`.
```
<DockPanel Dock="Bottom" Height="{ReadProperty SliderHeight}">
    <Panel Dock="Left" Width="60">
        <Text Value="{ReadProperty Min}" Alignment="Center" TextColor="{ReadProperty FaceColor}" />
        <Rectangle Width="1" Color="{ReadProperty FaceColor}" Alignment="Right" Margin="0,8" />
    </Panel>
    <Panel Dock="Right" Width="60">
        <Text Value="{ReadProperty Max}" Alignment="Center" TextColor="{ReadProperty FaceColor}" />
        <Rectangle Width="1" Color="{ReadProperty FaceColor}" Alignment="Left" Margin="0,8" />
    </Panel>

    <RangeControl ux:Name="range" Value="{Property Val}" Minimum="{ReadProperty Min}" Maximum="{ReadProperty Max}" UserStep="10" Margin="{ReadProperty SliderHeight}/2,0">
        <LinearRangeBehavior />
    </RangeControl>

    <Rectangle Layer="Background" CornerRadius="4" Color="{ReadProperty TintColor}" />
</DockPanel>
```

# Customize the RangeControl

Let's take a closer look at the `RangeControl` configuration:
```
<RangeControl ux:Name="range" Value="{Property Val}" Minimum="{ReadProperty Min}" Maximum="{ReadProperty Max}" UserStep="10" Margin="{ReadProperty SliderHeight}/2,0">
    <LinearRangeBehavior />
</RangeControl>
```
We see `Value="{Property Val}"` here, which means that we have a two-way property-binding to the `Val` property passed to our slider. If we're passing an Observable, it gets updated as we move the slider left and right. Note that we also set `UserStep="10"` property, which means that the value will be incremented by 10 per step.

With that cleared up, let's move on to the fun part :)

# Move the handle

Inside of the `RangeControl`, we need to have a handle that the slider can be moved by. The handle is a white circle that has the current slider value as a label on it, so that's exactly what we make:
```
<RangeControl ux:Name="range" Value="{Property Val}" Minimum="{ReadProperty Min}" Maximum="{ReadProperty Max}" UserStep="10" Margin="{ReadProperty SliderHeight}/2,0">
    <LinearRangeBehavior />

    <Circle ux:Name="handle" Width="{ReadProperty SliderHeight}" Alignment="Left" Anchor="50%,50%" X="attract({Property range.RelativeValue}, RangeSnap) * 100%">
        <Text Value="{= round({ReadProperty range.Value})}" FontSize="13" Alignment="Center" TextColor="{ReadProperty TintColor}" />
        <Circle Margin="8" Color="{ReadProperty FaceColor}" />
        <Circle Margin="4" Color="{ReadProperty TintColor}" />
    </Circle>

</RangeControl>
```
The interesting bit on the handle is how we set the `X` position of it to a UX expression: `X="attract({Property range.RelativeValue}, RangeSnap) * 100%"`. As the docs state, the [attract() UX expression](https://fuse-open.github.io/docs/ux-markup/expressions#misc) _animates the change in a value by using an `AttractorConfig` to define the animation style_. In our use-case, this means that whenever the `Value` of our `RangeControl` changes by an increment of 10, the `RelativeValue` of it is updated too. This in turn changes the `X` coordinate of the handle with a smooth, snappy animation defined by the `AttractorConfig` that we have in our `MainView.ux`:
```
<AttractorConfig ux:Global="RangeSnap" Unit="Normalized" Type="SmoothSnap" />
```

# Bend the Curve

What about the gooey animation where the background caves in at edges? This is what we use `Curve` for. Inside of our handle, we draw a trapezoid shape using `Curve` and four `CurvePoint` elements. A really cool thing that we can specify on `CurvePoint` is incoming and outgoing tangents. Since these describe _how_ the curve should bend, that's exactly what we're going to animate.
```
<Circle ux:Name="handle" Width="{ReadProperty SliderHeight}" Alignment="Left" Anchor="50%,50%" X="attract({Property range.RelativeValue}, RangeSnap) * 100%">
    <Text Value="{= round({ReadProperty range.Value})}" FontSize="13" Alignment="Center" TextColor="{ReadProperty TintColor}" />
    <Circle Margin="8" Color="{ReadProperty FaceColor}" />
    <Circle Margin="4" Color="{ReadProperty TintColor}" />
    <Curve Color="{ReadProperty TintColor}" Close="Auto">
        <CurvePoint ux:Name="t1" At="-0.5,1" TangentIn="0,0" TangentOut="1,0" />
        <CurvePoint ux:Name="t2" At="0.1,0.5" TangentIn="0,-0.5" TangentOut="0,0" />
        <CurvePoint ux:Name="t3" At="0.9,0.5" TangentIn="0,0" TangentOut="0,0.5" />
        <CurvePoint ux:Name="t4" At="1.5,1" TangentIn="1,0" TangentOut="0,0" />
    </Curve>
</Circle>
```
As for the animation, it happens when we press on the control. All we need to do is move the handle up above the slider, animate the tangents on the `Curve` and hide the label:
```
<WhilePressed>
    <Move Target="handle" Y="-1" RelativeTo="ParentSize"
        Duration="0.24" Easing="CircularOut"
        DelayBack="0" EasingBack="ExponentialOut" />
    <Change t2.TangentIn="-1,-2"
        Duration="0.92" Delay="0.16" Easing="ElasticOut"
        DurationBack="0.24" EasingBack="Linear" />
    <Change t3.TangentOut="-1,2"
        Duration="0.92" Delay="0.16" Easing="ElasticOut"
        DurationBack="0.24" EasingBack="Linear" />
    <Change label.Opacity="0" Duration="0.16" DelayBack="0.16" />
</WhilePressed>
```
And with that, our beautiful gooey custom slider is done. Go ahead, download the source and make your own crazy animated shapes!
