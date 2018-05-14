This example shows how you can use a `ScrollView` together with `ScrollingAnimation` to create advanced visual effects to accompany scrolling. In this example, we have an article with a nice header that animates when you scroll through the article.

# Design

The basic layout is like this:

```
<App Theme="Basic" Background="#eeeeeeff">
    <iOS.StatusBarConfig ux:Name="statusBarConfig" Style="Dark"/>
    <Panel>
		<MultiLayoutPanel ux:Name="headerPanel" HitTestMode="None" Alignment="Top" Height="260">
            ... Header content ...
        </MultiLayoutPanel>
        <ScrollView>
            ... Main content ...
        </ScrollView>
    </Panel>
</App>
```

The main part of our app is created with a `Panel` which layers the `headerPanel` on top of a `ScrollView`.

Notice that we set `HitTestMode` to "None" for headerPanel which makes it ignore all input. This has the effect of sending the input to the next element that is under the pointer. This way, we can scroll the `ScrollView` even if the headerPanel is above it visually.

# ScrollingAnimation

A very central part of this example is the `ScrollingAnimation` trigger. It can be used to do animations based on a scrollviews scroll position.

One example is that the header gets smaller as you scroll down. Because the header and the body is on top of each other, they don't affect each other layout-wise.

We can move the `backgroundPicture` with a `Move` animator:

<!-- snippet-begin:code/MainView.ux:MoveBackgroundPicture -->

```
<ScrollingAnimation From="0" To="260">
    <Move Target="backgroundPicture" RelativeTo="Size" Y="-0.7" Easing="QuadraticOut"/>
    ... some other animators ...
</ScrollingAnimation>
```

<!-- snippet-end -->

All the `Change` blocks will be smoothly animated between scroll position 0 and 260. Note that `RelativeTo="Size" Y="-0.7"` means that the target element is moved 70% of its size in the negative Y-direction. The `RelativeTo` property gives us finer control of how to animate with relative numbers.

Throughout this example we use the `RelativeTo` property set to both "Size" and "ParentSize". If one wants to specify an arbitrary node as the relative node, one can use the `RelativeNode` property.


# Animating the profile picture

There is an interesting type of animation happening to the face picture. Notice that at scroll position 0, the picture is on top of the background picture, but as we start scrolling, it actually moves behind it. To achieve this effect, we use a `MultiLayoutPanel`. The `MultiLayoutPanel` allows us to actually move the element to a different spot in the visual tree using a `Placeholder` node.

<!-- snippet-begin:code/MainView.ux:MultiLayoutPanel -->

```
<MultiLayoutPanel ux:Name="headerPanel" HitTestMode="None" Alignment="Top" Height="260">
    <Placeholder ux:Name="overBackgroundLayout">
        <Image ux:Name="facePicture"  File="Assets/avatar.png" Alignment="BottomCenter"
               Width="150" Height="150" Offset="0,75"/>
    </Placeholder>

    ... background image ...

    <Placeholder ux:Name="underBackgroundLayout" Target="facePicture"/>
</MultiLayoutPanel>
```

<!-- snippet-end -->

Then we change which placeholder we want our picture to exist in:

<!-- snippet-begin:code/MainView.ux:SettingLayoutElement -->

```
<ScrollingAnimation From="125" To="0">
    <Set headerPanel.LayoutElement="overBackgroundLayout"/>
</ScrollingAnimation>
<ScrollingAnimation From="125" To="170">
    <Set headerPanel.LayoutElement="underBackgroundLayout"/>
</ScrollingAnimation>
```

<!-- snippet-end -->

Since the `Set` animator permanently changes its targets value, we have to set it back to its original state when we scroll back, hence the two animators.

# The status bar text color change

On iOS devices, the background image is displayed under the statusbar (not visible in the gif). The top half of the image has quite bright colors, so we set the statusbar style to be dark. When we start scrolling, and the darker parts of our image go beneath the statusbar, we want to change the style to light. This can also be done with the `ScrollingAnimation` trigger:

<!-- snippet-begin:code/MainView.ux:StatusBarConfig -->

```
<Fuse.iOS.StatusBarConfig ux:Name="statusBarConfig" Style="Dark"/>
```

<!-- snippet-end -->

Then, we simply use a `Change` on it during a `ScrollingAnimation`:

<!-- snippet-begin:code/MainView.ux:ConfigStyleToLight -->

```
<ScrollingAnimation From="50" To="250">
    <Change statusBarConfig.Style="Light" />
</ScrollingAnimation>
```

<!-- snippet-end -->

Since this is a boolean change, the actual switch from dark to light style will happen when the animator progress is at 50%.
In this case that would be at 150.

- Note that this only applies to iOS, and has no effect on Android devices.
