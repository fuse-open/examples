This examples shows how to make a cool weather app with animated weather icons.

# Structuring the app

The app is structured using a `LinearNavigation` with a `SwipeNavigate` behavior. By setting the `SwipeDirection` property of `SwipeNavigate` to Down, we make the gesture a downward swipe instead of horizontal.

Each page of the app actually contains the same elements, but with different settings. Some elements, like the rain and snow, are turned on or off using data-bound `WhileTrue` triggers. We use data-binding and JavaScript to supply the different pages, and draw them with an `Each`.

# The rain

The rain is composed of two classes: `Raindrop` and `RaindropRow`.

<!-- snippet-begin:code/MainView.ux:raindrop -->

```
<Panel ux:Class="Raindrop">
    <Image Width="15" Height="15" File="Assets/raindrops.png" Color="{dropletcolor}">
        <Rotation Degrees="20.6" />
    </Image>
</Panel>
```

<!-- snippet-end -->

<!-- snippet-begin:code/MainView.ux:raindroprow -->

```
<Grid ux:Class="RaindropRow" ColumnCount="3">
    <Raindrop ux:Name="drop1"/>
    <Raindrop ux:Name="drop2"/>
    <Raindrop ux:Name="drop3"/>
</Grid>
```

<!-- snippet-end -->

We then instantiate 5 rows, and animate each of them using `Cycle` animators with their `Waveform` property set to `Sawtooth`. This means that the animation will go linearly from `Low` to `High` and then instantly jump back to `Low`.

<!-- snippet-begin:code/MainView.ux:rainanimation -->

```
<WhileTrue Value="{runDroplets}">
    <Cycle Frequency="8" Low="0" High="20" Target="dropTranslation1.Y"  Waveform="Sawtooth"/>
    <Cycle Frequency="8" Low="0" High="-5" Target="dropTranslation1.X"  Waveform="Sawtooth"/>
    <Cycle Frequency="8" Low="0" High="20" Target="dropTranslation2.Y"  Waveform="Sawtooth"/>
    <Cycle Frequency="8" Low="0" High="-5" Target="dropTranslation2.X"  Waveform="Sawtooth"/>
    <Cycle Frequency="8" Low="0" High="20" Target="dropTranslation3.Y"  Waveform="Sawtooth"/>
    <Cycle Frequency="8" Low="0" High="-5" Target="dropTranslation3.X"  Waveform="Sawtooth"/>
    <Cycle Frequency="8" Low="0" High="20" Target="dropTranslation4.Y"  Waveform="Sawtooth"/>
    <Cycle Frequency="8" Low="0" High="-5" Target="dropTranslation4.X"  Waveform="Sawtooth"/>
    <Cycle Frequency="8" Low="0" High="20" Target="dropTranslation5.Y"  Waveform="Sawtooth"/>
    <Cycle Frequency="8" Low="0" High="-5" Target="dropTranslation5.X"  Waveform="Sawtooth"/>
    <Cycle Frequency="8" Low="1" High="0.5" Target="raindropRow2.Opacity"   Waveform="Sawtooth"/>
    <Cycle Frequency="8" Low="0.5" High="0" Target="raindropRow3.Opacity"      Waveform="Sawtooth"/>
</WhileTrue>
```

<!-- snippet-end -->

# The snow

The snow flakes are also animated using `Cycle` animators. They have slightly different `Frequency` in order to make the animation look more natural.

<!-- snippet-begin:code/MainView.ux:snowflakeanimation -->

```
<Cycle Target="snowflakeTranslation1.Y" Frequency="0.4"  Low="0" High="155" Waveform="Sawtooth"/>
<Cycle Target="snowflakeTranslation2.Y" Frequency="0.43" Low="0" High="155" Waveform="Sawtooth"/>
<Cycle Target="snowflakeTranslation3.Y" Frequency="0.46" Low="0" High="155" Waveform="Sawtooth"/>
<Cycle Target="snowflakeTranslation4.Y" Frequency="0.50" Low="0" High="155" Waveform="Sawtooth"/>
```

<!-- snippet-end -->


# The info

The weather information panel is made with a simple `StackPanel` containing data-bound `Text` elements. They are also animated using `Entering-` and `ExitingAnimation` triggers in order to make them "jump in" a bit as we swipe.

<!-- snippet-begin:code/MainView.ux:textpart -->

```
<StackPanel Width="43%" Alignment="TopRight" Height="100%" >
    <Text Value="{TOD}"  Font="NunitoBold"  FontSize="19" Color="#ffffff80" Margin="0,14,0,0"/>
    <Text Value="{Temp}" Font="NunitoLight" FontSize="32" Color="#fff"      Margin="0,0,0,14" Alignment="CenterLeft"/>
    <StackPanel Alignment="TopLeft">
        <Body Value="{Summary}" FontSize="26" />
        <Body Value="{Wind}" />
        <Body Value="{Humidity}" />
        <ExitingAnimation>
            <Move Y="0.5" RelativeTo="ParentSize" Easing="Linear" />
        </ExitingAnimation>
        <EnteringAnimation>
            <Move Y="0.5" RelativeTo="ParentSize" Easing="Linear"/>
        </EnteringAnimation>
    </StackPanel>
</StackPanel>
```

<!-- snippet-end -->
