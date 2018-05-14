A few simple examples of how to create animations in response to user input with triggers and animators.

<div class="row">
    <div class="col-xs-6">
        [VIDEO media/anchored-preview.mp4]
    </div>
    <div class="col-xs-6">
        [VIDEO media/click-events-preview.mp4]
    </div>
</div>

## Staggered animation

Here we use the animator's `Delay` property so that the right rectangle does not begin to rotate until the left one has completed.

By setting a `Target` for each of the two animators we can move separate objects (the two rectangles) with just one trigger. If we didn't set the `Target` properties the animators would instead rotate their parent, the center-aligned stackpanel. In the anchored animation example further down we simpy place our trigger inside the `<Rectangle>` without explicitly setting a target for the animator.

<!-- snippet-begin:code/StaggeredAnimation.ux:StaggeredAnimation -->

```
<Panel ux:Class="StaggeredAnimation" Background="#FF5C3A">

    <StackPanel Alignment="Center" Orientation="Horizontal">
        <MyRectangle ux:Name="rec1"/>
        <MyRectangle ux:Name="rec2" />
        <WhilePressed>
            <Rotate Target="rec1" Degrees="90" DelayBack="0.5" Duration="0.5" Easing="QuadraticOut" EasingBack="QuadraticIn" />
            <Rotate Target="rec2" Degrees="90" Delay="0.5" Duration="0.5" EasingBack="QuadraticIn" Easing="QuadraticOut" />
        </WhilePressed>

    </StackPanel>

</Panel>
```

<!-- snippet-end -->

[VIDEO media/preview.mp4]

## Anchored animation

By default all transformations such as scaling and rotation happen around the center of an element. You can change this by setting the `TransformOrigin` property of the element.

Also notice how we're explcitily setting the color of the rectangle here with `<SolidColor Color="#fff" />` as opposed to using the predefined shorthand `Color="White"` in the previous example. In this way you can specify any color (although it's of course better to specify them as [resources][Resources] in just one centralized location).

<!-- snippet-begin:code/AnchoredAnimation.ux:AnchoredAnimation -->

```
<Panel ux:Class="AnchoredAnimation" Background="#FFC05A">

    <MyRectangle Alignment="Center" TransformOrigin="TopLeft">
        <WhilePressed>
            <Rotate Degrees="90" Duration="1" Easing="BounceOut" EasingBack="BounceIn"/>
        </WhilePressed>
    </MyRectangle>

</Panel>
```

<!-- snippet-end -->

[VIDEO media/anchored-preview.mp4]

In the UX above we simply used the `TopLeft` shorthand to move the center of rotation but the real power of this mechanism becomes apparent when you combine it with [Offset and Anchor][Offset and Anchor].

The following UX shows how TransformOrigin can be set to any position, even outside the element itself. Also notice that we have now placed the rectangle inside an explicitly sized panel since the offset and anchor is set relative to the size of the rectangle's parent.

```
<Panel Background="#FFC05A">

	<Panel Alignment="Center" Width="100" Height="100">
		<Rectangle Width="100" Height="100" CornerRadius="5" TransformOrigin="Anchor" Color="White"
			AnchorUnit="Percent" Anchor="180,50" Offset="180,50" OffsetUnit="Percent">
			<WhilePressed>
				<Rotate Degrees="90" Duration="1" Easing="BounceOut" EasingBack="BounceIn"/>
			</WhilePressed>
		</Rectangle>
	</Panel>

</Panel>
```

[VIDEO media/anchored-2-preview.mp4]

## Click events

This shows how animators respond to different types of input triggers. When the rounded rectangle on the left is `Tapped` the entire forward and backward animation is run. The circle on the right instead uses a `Pressing` trigger which reverses the animation as soon as you stop pressing.

The morphing effect on the circle is achieved simply by animating the corner radius of the element (it was a rectangle in disguise!)

<!-- snippet-begin:code/ClickEvents.ux:ClickEvents -->

```
<Panel ux:Class="ClickEvents" Background="#FF8EB4">

    <StackPanel Orientation="Horizontal" Alignment="Center">

        <MyRectangle>
            <Tapped>
                <Scale Factor="0.8" Duration="0.3" Easing="QuadraticInOut"/>
                <Move Y="0.3" Duration="0.3" Easing="QuadraticInOut" RelativeTo="Size"/>
            </Tapped>
        </MyRectangle>

        <MyRectangle ux:Name="fillRectB" CornerRadius="50">
            <WhilePressed>
                <Rotate Degrees="180" Duration="0.5" Easing="QuadraticInOut"/>
                <Change fillRectB.CornerRadius="5" Duration="0.5" Easing="QuadraticInOut"/>
            </WhilePressed>
        </MyRectangle>
    </StackPanel>

</Panel>
```

<!-- snippet-end -->

[VIDEO media/click-events-preview.mp4]

We've bundled these 3 examples together into a small app using a `PageControl` which you can download in the menu to the left.
