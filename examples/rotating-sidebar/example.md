In this example we're making a fancy rotating sidebar, as shown below:

----

Update: [Watch a video of Jake going through this example](https://www.youtube.com/watch?v=J2guDLZ3vcs&list=PLdlqWm6b-XALJgM3fGa4q95Yipsgb8Q1o&index=3).

We begin by creating our main panel. Since we want it to rotate around its top-left corner when the sidebar appears, we set `TransformOrigin="TopLeft"`.

<!-- snippet-begin:code/MainView.ux:TransformOrigin -->

```
<Panel ux:Name="content" TransformOrigin="TopLeft">
    ... the content of our app ...
</Panel>
```

<!-- snippet-end -->

Next up – the sidebar. Since we want the buttons to transition in from the left, we horizontally offset it by -130pt so it's off-screen.

<!-- snippet-begin:code/MainView.ux:Sidebar -->

```
<StackPanel Offset="-130,0" Alignment="BottomLeft" Margin="0,0,0,50">
    <DockPanel ux:Class="IconAndLabel" Margin="30">
        <string ux:Property="Text" />
        <Text Value="{ReadProperty Text}" Dock="Bottom" Color="#fffa" FontSize="14" Alignment="HorizontalCenter" Margin="0,10,0,0" Font="RobotoBold" />
    </DockPanel>

    <IconAndLabel ux:Name="profileButton" Text="PROFILE">
        <SidebarIcon>
            <AccountIcon />
        </SidebarIcon>

        <Clicked>
            <Set showMenu.Value="false" />
        </Clicked>
    </IconAndLabel>

    <IconAndLabel ux:Name="stickersButton" Text="STICKERS">
        <SidebarIcon>
            <StickersIcon />
        </SidebarIcon>

        <Clicked>
            <Set showMenu.Value="false" />
        </Clicked>
    </IconAndLabel>

    <IconAndLabel ux:Name="addButton" Text="ADD STICKER">
        <SidebarIcon>
            <AddIcon />
        </SidebarIcon>

        <Clicked>
            <Set showMenu.Value="false" />
        </Clicked>
    </IconAndLabel>
</StackPanel>
```

<!-- snippet-end -->

Now, for the menu button in the top-left corner. This is offset by half its size, so that we only see a quarter of it. The icons are placed in the left and right bottom corners, so we can rotate the whole thing by 90° to alternate between them.

<!-- snippet-begin:code/MainView.ux:MenuButton -->

```
<Panel ux:Name="menuButton" Width="170" Height="170" Alignment="TopLeft" Offset="-85,-85" Padding="40">
    <Circle Layer="Background" Color="#EE8485" />
    <HamburgerIcon Alignment="BottomRight"/>
    <CloseIcon Alignment="BottomLeft" />

    <Clicked>
        <Toggle Target="showMenu" />
    </Clicked>
</Panel>
```

<!-- snippet-end -->

And finally – the animation! You may have noticed the `Clicked` triggers in the two previous sections. They toggle the following `WhileTrue` trigger that animates the main panel, the menu button and the sidebar buttons.

<!-- snippet-begin:code/MainView.ux:ShowMenu -->

```
<WhileTrue ux:Name="showMenu">
    <Rotate Target="content" Degrees="-45"
            Duration="0.45"    Easing="ExponentialInOut"
            DurationBack="0.6" EasingBack="BounceIn" />

    <Rotate Target="menuButton" Degrees="-90"
            Duration="0.45"    Easing="ExponentialInOut"
            DurationBack="0.6" EasingBack="BounceIn" />

    <Move Target="profileButton" X="120"
          Delay="0.3"   Duration="0.6"     Easing="ElasticOut"
          DelayBack="0" DurationBack="0.3" EasingBack="QuarticInOut" />

    <Move Target="stickersButton" X="150"
          Delay="0.32"  Duration="0.65"    Easing="ElasticOut"
          DelayBack="0" DurationBack="0.3" EasingBack="QuarticInOut" />

    <Move Target="addButton" X="180"
          Delay="0.34"  Duration="0.7"     Easing="ElasticOut"
          DelayBack="0" DurationBack="0.3" EasingBack="QuarticInOut" />

    <Change statusBarConfig.IsVisible="false" Delay=".25" />
</WhileTrue>
```

<!-- snippet-end -->

When the icons in the sidebar are clicked, an animated circle appears which grows outwards from the icon. This animation is achieved with:

<!-- snippet-begin:code/MainView.ux:Tapped -->

```
<Tapped>
    <Scale Target="tapAnimationCircle" Factor="5"
           Duration="0.5" Easing="QuarticOut"
           DurationBack="0" />

    <Change tapAnimationCircle.Opacity="0"
            Duration="0.5" Easing="QuarticOut"
            DurationBack="0" />

    <Change tapAnimationCircle.Smoothness="10"
            Duration="0.5" Easing="QuarticOut"
            DurationBack="0" />

    <Change tapAnimationStroke.Width="0"
            Duration="0.5" Easing="ExponentialOut"
            DurationBack="0" />
</Tapped>
```

<!-- snippet-end -->
