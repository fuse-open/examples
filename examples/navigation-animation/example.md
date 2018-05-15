In this example we are using `Entering-` and `ExitingAnimation` to make an interesting card swipe mechanism.

# The card class

We define a class for our cards using the `ux:Class` syntax.

<!-- snippet-begin:code/Card.ux:CardClass -->

```
<Panel ux:Class="Card" Height="256" Width="256" Alignment="Bottom" ux:Name="p">
    <Rectangle Color="#eee" Opacity="0" ux:Name="overlayRect" CornerRadius="25" Layer="Overlay"/>
    <EnteringAnimation Scale="0.35">
        <Move X="-4" RelativeTo="Size" />
    </EnteringAnimation>
    <ExitingAnimation Scale="0.1">
        <Move RelativeTo="Size" Easing="QuadraticOut" Y="-1.3" Duration="1"/>
        <Move RelativeTo="Size" KeyframeInterpolation="CatmullRom">
            <Keyframe X="0.15" Time="0.15"/>
            <Keyframe X="0.2" Time="0.3"/>
            <Keyframe X="0.25" Time="0.45"/>
            <Keyframe X="0.2" Time="0.6"/>
            <Keyframe X="0" Time="1"/>
        </Move>

        <Change overlayRect.Opacity="1" Duration="1" Easing="QuadraticInOut"/>

        <Scale Factor="0.4" Duration="1" />
        <Rotate Degrees="-50" Duration="1" Easing="QuadraticInOut"/>
    </ExitingAnimation>
</Panel>
```

<!-- snippet-end -->


## Navigation animation

We assign the `Scale`-property of our `EnteringAnimation` and `ExitingAnimation` in order to achieve the longer navigation animation. We use `Move` together with `Rotate` in order to achieve the curly movement. Keyframes are used to tweak this curl a bit further. It allows us to define a custom path for our animation.

<!-- snippet-begin:code/Card.ux:ExitingAnimation -->

```
<ExitingAnimation Scale="0.1">
    <Move RelativeTo="Size" Easing="QuadraticOut" Y="-1.3" Duration="1"/>
    <Move RelativeTo="Size" KeyframeInterpolation="CatmullRom">
        <Keyframe X="0.15" Time="0.15"/>
        <Keyframe X="0.2" Time="0.3"/>
        <Keyframe X="0.25" Time="0.45"/>
        <Keyframe X="0.2" Time="0.6"/>
        <Keyframe X="0" Time="1"/>
    </Move>

    <Change overlayRect.Opacity="1" Duration="1" Easing="QuadraticInOut"/>

    <Scale Factor="0.4" Duration="1" />
    <Rotate Degrees="-50" Duration="1" Easing="QuadraticInOut"/>
</ExitingAnimation>
```

<!-- snippet-end -->

# Our resources

Our cards content is just an image asset. This is just to make the example smaller, since the focus is to illustrate the use of `EnteringAnimation` and `ExitingAnimation`.


You can check out these examples to learn more about layout:

- [Layout example](../layout/example.md)
- [Inbox](../inbox/example.md)
- [Reveal actions](../reveal-actions/example.md)



Notice how we generate our items in JavaScript, and we data-bind directly to the file paths relative to project root.
We can do that, because we have included the images in our bundle by adding this to our `.unoproj` file `"Includes"` section: `"Assets/*.png:Bundle`.

<!-- snippet-begin:code/MainView.ux:MainView -->

```
<App Background="#eeeeeeff">
    <DockPanel>
        <JavaScript>
            var Observable = require("FuseJS/Observable");

            var profiles = Observable(
                { resource : "Assets/profile_1.png" },
                { resource : "Assets/profile_2.png" },
                { resource : "Assets/profile_3.png" },
                { resource : "Assets/profile_6.png" },
                { resource : "Assets/profile_4.png" },
                { resource : "Assets/profile_5.png" },
                { resource : "Assets/profile_2.png" },
                { resource : "Assets/profile_5.png" },
                { resource : "Assets/profile_4.png" },
                { resource : "Assets/profile_1.png" },
                { resource : "Assets/profile_7.png" },
                { resource : "Assets/profile_3.png" },
                { resource : "Assets/profile_2.png" },
                { resource : "Assets/profile_5.png" },
                { resource : "Assets/profile_4.png" },
                { resource : "Assets/profile_1.png" },
                { resource : "Assets/profile_7.png" },
                { resource : "Assets/profile_3.png" }
            );

              module.exports = {
                  profiles : profiles
              }
        </JavaScript>

        ... the rest ...

        <BottomBarBackground DockPanel.Dock="Bottom" />
    </DockPanel>
</App>

```

<!-- snippet-end -->
