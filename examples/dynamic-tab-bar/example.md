In this example we are using `WhileActive` and `ActivatingAnimation` to create a dynamic tab bar. The design was inspired by [this awesome concept](https://dribbble.com/shots/2702517-Review-App-Concept) by [Sergii Ganushchak](https://dribbble.com/ne_toy).
Icons are from Google's excellent [Material Icons](https://design.google.com/icons/) and the font is [Roboto](https://github.com/google/roboto). The placeholder graphics is from Sergii's [original Form prototype](https://dribbble.com/shots/2702517-Review-App-Concept/attachments/546093).

## The tab bar

We use a regular `PageControl` for the navigation and make sure to set its `TransitionEasing` to `BackOut`.


The tab bar itself is defined as follows:

<!-- snippet-begin:code/MainView.ux:TabBar -->

```
<Panel Dock="Top" Height="60" >
    <Rectangle ux:Name="indicator" CornerRadius="30" Color="color0" LayoutMaster="p1" Margin="0,10" Width="100%" ZOffset="0.1"/>
    <Grid ColumnCount="5" Margin="-30,0">
        <Panel ux:Name="p1" Column="0" ColumnSpan="2"/>
        <Panel ux:Name="p4" Column="3" ColumnSpan="2"/>
    </Grid>

    <Grid ZOffset="1" Margin="-20,0">
        <Column ux:Name="col1" Width="1" WidthMetric="Proportion"/>
        <Column ux:Name="col2" Width="1" WidthMetric="Proportion"/>
        <Column ux:Name="col3" Width="1" WidthMetric="Proportion"/>
        <Column ux:Name="col4" Width="1" WidthMetric="Proportion"/>

        <Text ux:Class="TabHeader" Color="White" Alignment="CenterLeft" Opacity="0" Margin="20,0,0,0" MinWidth="100"/>
        <Image ux:Class="TabIcon" Width="25" Height="25" />

        <Panel HitTestMode="LocalBounds">
            <Grid Columns="auto,1*" Margin="40,0" Alignment="Left">
                <TabIcon ux:Name="c1" File="Assets/basket.png" Color="color0"/>
                <TabHeader ux:Name="h1" Value="Products" />
            </Grid>
            <Clicked>
                <Set nav.Active="page1" />
            </Clicked>
        </Panel>
        <Panel HitTestMode="LocalBounds">
            <Grid Columns="auto,1*" Margin="30,0" Alignment="Left">
                <TabIcon ux:Name="c2" File="Assets/accountbalance.png" Color="color1"/>
                <TabHeader ux:Name="h2" Value="Places"/>
            </Grid>
            <Clicked>
                <Set nav.Active="page2" />
            </Clicked>
        </Panel>
        <Panel HitTestMode="LocalBounds">
            <Grid Columns="auto,1*" Margin="30,0" Alignment="Left">
                <TabIcon ux:Name="c3" File="Assets/play.png" Color="color2"/>
                <TabHeader ux:Name="h3" Value="Reviews"/>

            </Grid>
            <Clicked>
                <Set nav.Active="page3" />
            </Clicked>
        </Panel>
        <Panel HitTestMode="LocalBounds">
            <Grid Columns="auto,1*" Margin="30,0" Alignment="Left">
                <TabIcon ux:Name="c4" File="Assets/person.png" Color="color3" />
                <TabHeader ux:Name="h4" Value="Friends"/>
            </Grid>
            <Clicked>
                <Set nav.Active="page4" />
            </Clicked>
        </Panel>
    </Grid>
</Panel>
```

<!-- snippet-end -->

The indicator itself is just `Rectangle` with a `CornerRadius` of 30. Notice that there are two grids defined beneath it. The first one is just used to guide the animation of the indicator, while the second one actually contains the tab bar items.

The indicator has its `LayoutMaster` set to the first child of the guide grid. It is then animated by the page progress using a `ActivatingAnimation`:


```
<ActivatingAnimation Scale="0.333">
	<Move Target="indicator" X="1" RelativeTo="PositionOffset" RelativeNode="p4"/>
</ActivatingAnimation>
```

Notice that we set its `Scale` to 0.333. This is because it should be activated across all 4 pages (minus the first one). The `Move` animator has its `RelativeNode` property set to the second element of the guide grid. This is how we use the guide grid to define the end points of the indicator animation.

The contents of the `PageControl` (the pages themselves) consists mostly of animators changing the color of the background, title text, indicator and the tab bar items.

The actual contents are just `Image` placeholders and outside the scope of this example.

<!-- snippet-begin:code/MainView.ux:PageControl -->

```
<PageControl ux:Name="nav">
    <NavigationMotion GotoEasing="BackOut" />
    <Attractor ux:Name="indicatorColorAttractor" Target="indicator.Color" Value="color0" />
    <Rectangle ux:Name="bgColor" Layer="Background" Color="color0" Opacity="0.12"/>
    <Attractor ux:Name="bgColorAttractor" Target="bgColor.Color" Value="color0" />
    <Attractor ux:Name="titleTextAttractor" Target="titleText.Color" Value="color0" />

    <WhileTrue ux:Name="shrinkIndicatorWidth">
        <Change indicator.Width="90" Duration="0.25"/>
    </WhileTrue>

    <Image ux:Class="PagePlaceholder" Margin="8,10,8,0" StretchMode="UniformToFill" ContentAlignment="Top"/>

    <Page ux:Name="page1">
        <PagePlaceholder File="Assets/screen1.png"/>
        <WhileActive Threshold="0.5">
            <Set shrinkIndicatorWidth.Value="false" />
            <Set indicatorColorAttractor.Value="color0" />
            <Set bgColorAttractor.Value="color0" />
            <Set titleTextAttractor.Value="color0" />
        </WhileActive>
        <ActivatingAnimation>
            <Change h1.Opacity="1" />
            <Change col1.Width="2" />
            <Change c1.Color="White" />
        </ActivatingAnimation>
    </Page>
    <Page ux:Name="page2">
        <PagePlaceholder File="Assets/screen2.png" />
        <WhileActive Threshold="0.5">
            <Set shrinkIndicatorWidth.Value="true" />
            <Set indicatorColorAttractor.Value="color1" />
            <Set bgColorAttractor.Value="color1" />
            <Set titleTextAttractor.Value="color1" />
        </WhileActive>
        <ActivatingAnimation>
            <Change h2.Opacity="1" />
            <Change col2.Width="2" />
            <Change c2.Color="White" />
        </ActivatingAnimation>
    </Page>
    <Page ux:Name="page3">
        <PagePlaceholder File="Assets/screen3.png" />
        <WhileActive Threshold="0.5">
            <Set shrinkIndicatorWidth.Value="true" />
            <Set indicatorColorAttractor.Value="color2" />
            <Set bgColorAttractor.Value="color2" />
            <Set titleTextAttractor.Value="color2" />
        </WhileActive>
        <ActivatingAnimation>
            <Change h3.Opacity="1" />
            <Change col3.Width="2" />
            <Change c3.Color="White" />
        </ActivatingAnimation>
    </Page>
    <Page ux:Name="page4">
        <PagePlaceholder File="Assets/screen4.png" />
        <WhileActive Threshold="0.5">
            <Set shrinkIndicatorWidth.Value="false" />
            <Set indicatorColorAttractor.Value="color3" />
            <Set bgColorAttractor.Value="color3" />
            <Set titleTextAttractor.Value="color3" />
        </WhileActive>
        <ActivatingAnimation Scale="0.333">
            <Move Target="indicator" X="1" RelativeTo="PositionOffset" RelativeNode="p4"/>
        </ActivatingAnimation>
        <ActivatingAnimation>
            <Change h4.Opacity="1" />
            <Change col4.Width="2" />
            <Change c4.Color="White" />
        </ActivatingAnimation>
    </Page>
</PageControl>
```

<!-- snippet-end -->

And that's it!
