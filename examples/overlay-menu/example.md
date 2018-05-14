This example shows how to use `Keyframe` with `Scaling` and `Translation` to make a set of wobbly buttons. The example was inspired by [this awesome piece](https://dribbble.com/shots/2493667-overlay-menu-Add-animation) by [Radek Skrzypczak](https://dribbble.com/Radziu), which is a design for a task list app.

The [skull icon](https://www.iconfinder.com/icons/185091/danger_death_delete_destroy_skull_streamline_icon) was created by [Vincent Le Moign](http://www.webalys.com/).

The arrow icon is from Google's [Material Icons pack](https://design.google.com/icons/).

This example has two main parts: the background task page and the animated overlay menu. The overlay menu is probably the most interesting part, so we'll start with that.

## Animating the menu

We place the "menu"-`Panel` on top of the task page by placing the `Panel` on top of the task `Panel` as its sibling.

<!-- snippet-begin:code/MainView.ux:AddTaskButtonpage -->

```
<DockPanel>
    <Panel HitTestMode="None">
        <Rectangle ux:Name="addTaskButtons" Layer="Background" Margin="0,0,0,80"/>
        <DockPanel>
            <StatusBarBackground Dock="Top"/>
            <ux:Include File="AddTaskPage.ux" />
        </DockPanel>
    </Panel>

    <Grid Rows="auto,auto,3*,2*,auto">
```

<!-- snippet-end -->

The buttons and their animation is defined in their own file and included in `MainView.ux` using the `<ux:Include />` syntax:

```
<ux:Include File="AddTaskPage.ux"/>
```

<!-- snippet-begin:code/AddTaskPage.ux:TheEllipses -->

```
<Panel ux:Name="ellipse1" Alignment="Bottom" Width="60" Height="60" Opacity="0">
    <Image File="Assets/skull.png" Margin="17"/>
    <Circle Color="Red1">
        <Stroke Color="Red3" Width="2" Offset="4" />
    </Circle>

    <Translation ux:Name="ellipseTrans1" />
    <Scaling ux:Name="ellipseScaling1" Y="0.1" X="0.1"/>
</Panel>

<Text ux:Class="EllipseText" Alignment="Center" FontSize="26"/>
<Panel ux:Name="ellipse2" Alignment="Bottom"  Width="60" Height="60" Opacity="0">
    <EllipseText Value="!!!" Color="Orange1"/>
    <Circle Color="White1"/>
    <Translation ux:Name="ellipseTrans2" />
    <Scaling ux:Name="ellipseScaling2" Y="0.1" X="0.1"/>
</Panel>
<Panel ux:Name="ellipse3" Alignment="Bottom" Width="60" Height="60" Opacity="0">
    <EllipseText Value="!!" Color="Gray1"/>
    <Circle Color="White1"/>
    <Translation ux:Name="ellipseTrans3" />
    <Scaling ux:Name="ellipseScaling3" Y="0.1" X="0.1"/>
</Panel>
<Panel ux:Name="ellipse4" Alignment="Bottom" Width="60" Height="60" Opacity="0">
    <EllipseText Value="!" Color="Gray2"/>
    <Circle Color="White1" />
    <Translation ux:Name="ellipseTrans4" />
    <Scaling ux:Name="ellipseScaling4" Y="0.1" X="0.1"/>
</Panel>
```

<!-- snippet-end -->

The buttons are created using a `Panel` with a containing `Circle`. Note that we do not actually use the `Button` class in this case. They also each have their own `Translation` and `Scaling`, which we use to create their animation.

Their animation is triggered by toggling a `WhileTrue` in response to a `Clicked` event. Each button is animated individually by a `Change` of their `Scaling` and `Transform`. They also have their `Opacity` animated in the same way.

<!-- snippet-begin:code/AddTaskPage.ux:TheButtonAnimations -->

```
<Change Target="ellipseScaling1.Vector">
    <Keyframe Time="0.1" Value="1.4,1.0"/>
    <Keyframe Time="0.2" Value="0.8,1.0"/>
    <Keyframe Time="0.3" Value="1.0,1.0"/>
</Change>
<Change ellipseTrans1.Y="-80"  Duration="0.2" Delay="0" Easing="QuinticInOut"/>
<Change ellipse1.Opacity="1" Duration="0.3" Easing="QuinticOut"/>

<Change Target="ellipseScaling2.Vector">
    <Keyframe Time="0.15" Value="1.0,1.4"/>
    <Keyframe Time="0.3" Value="1.0,0.9"/>
    <Keyframe Time="0.4" Value="1.0,1.0"/>
</Change>
<Change ellipseTrans2.Y="-160"  Duration="0.25" Delay="0" Easing="QuinticInOut" />
<Change ellipse2.Opacity="1" Duration="0.4" Easing="QuinticOut"/>

<Change Target="ellipseScaling3.Vector">
    <Keyframe Time="0.2" Value="1.0,1.6"/>
    <Keyframe Time="0.3" Value="1.0,0.9"/>
    <Keyframe Time="0.4" Value="1.0,1.0"/>
</Change>
<Change ellipseTrans3.Y="-240" Duration="0.3" Delay="0" Easing="QuinticInOut"/>
<Change ellipse3.Opacity="1" Duration="0.45" Easing="QuinticOut"/>

<Change Target="ellipseScaling4.Vector">
    <Keyframe Time="0.25" Value="1.0,1.4"/>
    <Keyframe Time="0.4" Value="1.0,0.9"/>
    <Keyframe Time="0.5" Value="1.0,1.0"/>
</Change>
<Change ellipseTrans4.Y="-320" Duration="0.35" Delay="0" Easing="QuinticInOut"/>
<Change ellipse4.Opacity="1" Duration="0.5" Easing="QuinticOut"/>
```

<!-- snippet-end -->

## The task page

The task page has no animation but is composed of a few building blocks. The main layout is a `Grid` with five rows. The first row contain the `StatusBarBackground`, which is used to compensate for the height of the status bar. The second row contains the top bar, with a title and a familiar hamburger icon (like them or not, they're very common these days).

<!-- snippet-begin:code/MainView.ux:TopBar -->

```
<Grid Columns="1*,1*,1*" Height="50" Margin="20,0">
    <Grid Width="20" RowCount="2" Height="15" Alignment="Left">
        <Rectangle Height="2" Color="White"/>
        <Rectangle Height="2" Color="White"/>
    </Grid>
    <T Value="Today" Alignment="Center"/>
</Grid>
```

<!-- snippet-end -->

The next two rows contains the mock list of tasks:

<!-- snippet-begin:code/MainView.ux:TheTaskList -->

```
<StackPanel ItemSpacing="20">
    <JavaScript>
        module.exports = { items: ['The first task', 'The second task', 'The third task'] };
    </JavaScript>
    <Each Items="{items}">
        <Grid Columns="1*,auto" Margin="20,0">
            <T Alignment="CenterLeft" Value="{}" />
            <Rectangle Width="30" Height="30" Opacity="0.5">
                <Stroke Color="Gray2" Width="2"/>
            </Rectangle>
        </Grid>
        <Rectangle Height="1" Color="Gray2" Opacity="0.6"/>
    </Each>
</StackPanel>
```

<!-- snippet-end -->

and the mock listing of the three task severities:

<!-- snippet-begin:code/MainView.ux:TaskCounterItems -->

```
<Grid ColumnCount="3">
    <Rectangle Color="Gray2" Height="2" Layer="Background" Y="70%"/>
    <TaskCounter Severity="!!!" SeverityColor="#EF804F"
                 Count="3" Text="IMPORTANT" />
    <TaskCounter Severity="!!" SeverityColor="#939393"
                 Count="2" Text="REALLY?" />
    <TaskCounter Severity="!" SeverityColor="#DBDBDB"
                 Count="5" Text="MEH!" />
</Grid>
```

<!-- snippet-end -->

The `TaskCounter` class is defined as follows, and was made just as a convenience.

<!-- snippet-begin:code/TaskCounter.ux:TaskCounterClass -->

```
<Grid ux:Class="TaskCounter" Rows="1*,3*,2*"
      Margin="10,20" Padding="10" Color="White">
    <string ux:Property="Severity" />
    <float4 ux:Property="SeverityColor" />

    <string ux:Property="Count" />
    <string ux:Property="Text" />

    <Panel Alignment="TopCenter" Y="-20" Background="White" Layer="Overlay">
        <Text Value="{ReadProperty this.Severity}"
              Color="{ReadProperty this.SeverityColor}"
              FontSize="22" Margin="3,0"/>
    </Panel>
    <Rectangle Layer="Background">
        <Stroke Width="2" Color="Gray2" />
    </Rectangle>
    <Text Value="{ReadProperty this.Text}" Alignment="Center" FontSize="10"/>
    <Text Value="{ReadProperty this.Count}" FontSize="70" Alignment="Center"/>
    <Image File="Assets/arrowForward.png" Color="Gray2" Width="20" Height="20"/>
</Grid>
```

<!-- snippet-end -->

And that's it! Feel free to download the example code and play around.
