# Cube transition

This example illustrates how you can create a neat 3D transition effect between two screens.

## The viewport

To get proper 3D perspective projection, we place everything inside a `Viewport` at the root of the app.
Note that we set `CullFace="Back"`. This makes it so that only the front face of any element is visible.

The `Perspective` property controls the distance (in points) between the camera and the `Z = 0` plane (where graphics are drawn by default).
The field of view is then calculated based on this distance and the size of the viewport.
By setting `PerspectiveRelativeTo="Width"`, the `Perspective` property is treated as a factor, and the final distance is computed as `Perspective * Viewport width`. Now the camera distance is always proportional to the width of the viewport, making the field of view independent of the width of the viewport – which is what we want in this case.

<!-- snippet-begin:code/MainView.ux:Viewport -->

```
<Viewport Perspective="1.7" PerspectiveRelativeTo="Width" CullFace="Back">
```

<!-- snippet-end -->

## The box

Here's the secret: our box is not actually a box. It's simply two Panels that line up correctly at a 90 degree angle.
To make the Panels line up, we set our `TransformOrigin` to `VerticalBoxCenter`.

The `TransformOrigin` property determines the origin of transformation for an element.
`VerticalBoxCenter` makes an element act as the front face of a box.
It achieves this by measuring the width of the element, and interpreting that as the depth of an "imaginary" box.
The resulting point of origin will be at `X = Y = -Z = width / 2`, since we want to transform around the center of the box.

Inside our `box` panel, we have a second panel called `sidePage`.
It also has the `VerticalBoxCenter` TransformOrigin, but is rotated 90 degrees around the Y axis.
This places it as the left face of the box.

<!-- snippet-begin:code/MainView.ux:Box -->

```
<Panel ux:Name="box" TransformOrigin="VerticalBoxCenter">
    <Panel ux:Name="sidePage" TransformOrigin="VerticalBoxCenter">
        <Rotation DegreesY="90" />
```

<!-- snippet-end -->

## The transition

The transition itself is actually very simple.
We rotate the `box` 90 degrees counter-clockwise around the Y-axis, so that the `sidePanel` faces the "camera".
We also fade out the content, fade in the menu, and tell the hamburger icon to morph into a close icon.

<!-- snippet-begin:code/MainView.ux:MenuIsOpenTrigger -->

```
<WhileTrue ux:Name="menuIsOpen" Value="false">
    <Rotate Target="box" DegreesY="-90" Duration=".7" Delay="0" DelayBack="0" Easing="ExponentialOut" EasingBack="ExponentialIn" />
    <Change content.Opacity="0" Duration=".7" DelayBack="0" Easing="ExponentialOut" EasingBack="ExponentialIn" />
    <Change menu.Opacity="1" Duration=".3" DelayBack="0" Easing="QuarticIn" EasingBack="QuarticIn" /> 
    <Change hamburger.IsOpen="true" DelayBack="0" />
</WhileTrue>
```

<!-- snippet-end -->

## The search field

Although the search button and the other menu items look the same initially, they're implemented completely differently.
To avoid copying the same code between the search field and the rest of the menu items, we've created a tiny `MenuItem` component that applies the same height and underline to both.


<!-- snippet-begin:code/MainView.ux:MenuItem -->

```
<Size ux:Global="MenuItemHeight" ux:Value="40" />
<Panel ux:Class="MenuItem" Height="MenuItemHeight">
    <Rectangle Alignment="Bottom" Height="2" Color="#fff2" />
</Panel>
```

<!-- snippet-end -->

The search field itself is a `MenuItem`, containing a `TextInput` set up to look like the other menu items.

There are two cases where we want the search field to be expanded – while it's either in focus, or has a non-empty value.
We implement this using a `WhileFocused` and a `WhileString` trigger, which both activate the same `searchFieldActive` trigger.

<!-- snippet-begin:code/MainView.ux:SearchField -->

```
<MenuItem ux:Name="searchField">
    <TextInput ux:Name="searchInput" FontSize="28" PlaceholderText="search" PlaceholderColor="ForegroundColor" TextColor="ForegroundColor" CaretColor="#fffa" Alignment="Top">
        <WhileFocused>
            <Change searchFieldActive.Value="true" />
        </WhileFocused>
        <WhileString Test="IsNotEmpty" Value="{ReadProperty searchInput.Value}">
            <Change searchFieldActive.Value="true" />
        </WhileString>
    </TextInput>
    
    <LayoutAnimation>
        <Resize X="1" Y="1" RelativeTo="SizeChange" DurationBack=".3" DelayBack=".2" Easing="ExponentialInOut" />
        <Move X="1" Y="1" RelativeTo="WorldPositionChange" DelayBack="0" Duration=".3" Easing="QuarticIn" />
    </LayoutAnimation>
</MenuItem>
```

<!-- snippet-end -->

`searchFieldActive` performs the transition from menu to search layout.
Most of the animation is simply setting the `LayoutMaster` and `Alignment` of the search field, which will trigger the `LayoutAnimation` above. We also move the close button and the other menu items out of view.

<!-- snippet-begin:code/MainView.ux:SearchFieldTransitionAndLayout -->

```
<WhileTrue ux:Name="searchFieldActive" Value="false">
    <Change searchField.LayoutMaster="searchFieldLayoutMaster" Delay=".07" DelayBack=".07" />
    <Change transitionToSearchLayout.TargetProgress="1" />
</WhileTrue>

<Timeline ux:Name="transitionToSearchLayout">
    <Move Target="normalMenuItems" Y="1" RelativeTo="Size" Delay=".1" Duration=".5" Easing="CubicInOut" />
    <Change normalMenuItems.Opacity="0" Delay=".1" Duration=".3" Easing="CubicInOut" />
    <Move Target="hamburger" Y="-2" RelativeTo="Size" Duration=".4" Easing="CubicIn" />
    <Change searchInput.PlaceholderColor="#ccc6" Duration=".5" Easing="QuadraticInOut" />
    <Change searchCloseButton.Opacity="1" Duration=".2" DelayBack="0" Easing="QuadraticInOut" />
</Timeline>

<DockPanel ux:Name="searchLayout" Alignment="Top" Margin="30" Height="MenuItemHeight">
    <Panel ux:Name="searchFieldLayoutMaster" />
    <Rectangle ux:Name="searchCloseButton" Dock="Right" Opacity="0" HitTestMode="LocalBounds" Margin="15,0,0,0">
        <Clicked>
            <Set searchInput.Value="" />
        </Clicked>
```

<!-- snippet-end -->


And that's pretty much it for this example!
As always, feel free to download and play around with the code yourself.