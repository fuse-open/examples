We discovered a blog post by [MaterialUp](http://www.materialup.com/) with a nice list of UX designs and felt like taking on the [challenge of turning it into code](https://stories.uplabs.io/can-you-code-this-design-concept-efe0132b9744). Here is an implementation of their material up-vote design.

Update: [watch a video of Jake going through this example](https://www.youtube.com/watch?v=2asBMD9eMm8&list=PLdlqWm6b-XALJgM3fGa4q95Yipsgb8Q1o&index=19).

# The structure

The structure can be split into three parts. The button, the background and the animation.


# The inner circles

The actual button is composed of two filled circles. They each contain a `Scaling` with `Factor="0"` which gives them a size of 0. We want to animate them from the center and outwards and having a rest state at the center lets us describe our animations as deviation from size 0. `Scaling` is also a lot faster than animating `Width` and `Height`. This is because `Scaling` scales a texture instead of changing actual layout properties. Therefore, scaling an element with a `Width` and `Height` of 0 wouldn't work.

We group the circles using a panel. This is so that we can change their ordering using `BringToFront` on the individual circles without having them move all the way in front of the text. See the animation section further down.


<!-- snippet-begin:code/MainView.ux:InnerCircles -->

```
<Panel ClipToBounds="True" Height="90%" Width="90%">
    <Panel ux:Name="textPanel">
        <Arrow ux:Name="arrow"/>
        <DarkText ux:Name="unupvotedText" Value="22" Offset="0,5"/>
        <LightText ux:Name="upvotedText" Value="23">
            <Translation Y="1" RelativeTo="ParentSize"/>
        </LightText>
    </Panel>
</Panel>
<Panel>
    <Circle ux:Name="pinkCircle" Color="#FF4081">
        <Scaling ux:Name="pinkScaling" Factor="0"/>
    </Circle>
    <Circle ux:Name="grayCircle" Color="#D7D7D7">
        <Scaling ux:Name="grayScaling" Factor="0"/>
    </Circle>
</Panel>
```

<!-- snippet-end -->

# The background

The background works in pretty much the same way as the button. The circles are placed in the bottom of the UX so that they appear behind everything else.

<!-- snippet-begin:code/MainView.ux:OuterCircles -->

```
<Panel>
    <Circle ux:Name="lightCircle" Color="#D7D7D7" MaxWidth="2000" Width="2000" MaxHeight="2000" Height="1200">
        <Scaling ux:Name="lightScaling" Factor="0" />
    </Circle>
    <Circle ux:Name="darkCircle" Color="#363F45" MaxWidth="2000" Width="2000" MaxHeight="2000" Height="1200">
        <Scaling ux:Name="darkScaling" Factor="0" />
    </Circle>
</Panel>
```

<!-- snippet-end -->

# The animation

A `StateGroup` is used for animation. It has two states, "upvoted" and "unupvoted". They each describe an animated deviation from the rest state. The "unupvoted" is assigned to the `Rest` property of the `StateGroup` and so becomes the default state when the app is launched.

Both states animates the `Factor` property of the circles so that they animate back to their original size. We also use `BringToFront` to make sure the circles being animated are on top.

<!-- snippet-begin:code/MainView.ux:Animation -->

```
<StateGroup ux:Name="sg" Rest="unupvoted">
    <State ux:Name="unupvoted">
        <Change grayScaling.Factor="1" Duration="0.4" DurationBack="0.2" DelayBack="0.4" Easing="CircularInOut"/>
        <Change darkScaling.Factor="1" Duration="0.4" Delay="0.15" DelayBack="0.4" DurationBack="0.2" Easing="CircularInOut"/>
        <Change statusBar.Style="Light" Duration="0.4" DurationBack="0"/>

        <BringToFront Target="grayCircle"/>
        <BringToFront Target="darkCircle"/>
    </State>
    <State ux:Name="upvoted">
        <Change pinkScaling.Factor="1" Duration="0.4" DurationBack="0.2" DelayBack="0.4" Easing="CircularInOut"/>
        <Change lightScaling.Factor="1" Duration="0.4" Delay="0.15" DurationBack="0.2" DelayBack="0.4" Easing="CircularInOut"/>

        <Change statusBar.Style="Dark" Duration="0.4" DurationBack="0"/>
        <Move Target="arrow" RelativeTo="Size" Y="-1" Duration="0.4" DelayBack="0.2" Easing="CircularInOut"/>
        <Move Target="textPanel" RelativeTo="Size" Y="-1" Duration="0.4" Delay="0.2" Easing="CircularInOut"/>

        <BringToFront Target="pinkCircle"/>
        <BringToFront Target="lightCircle"/>
    </State>
</StateGroup>
```

<!-- snippet-end -->
