An example showing how to create a slide show using a `PageControl`.

## PageControl

A `PageControl` allows the user to navigate between several pages. It's basic structure is:

<!-- snippet-begin:code/MainView.ux:PageControl -->

```
<PageControl ux:Name="slides" ClipToBounds="true">
    4x <Page/> ...
</PageControl>
```

<!-- snippet-end -->

Only one page is displayed at a time. A basic swipe navigation is used to move between the pages.  The name `thePages` is used later to refer to this control.

You can put whatever content you'd like into the `Page` elements. In the example one of our pages looks like this:

<!-- snippet-begin:code/MainView.ux:JuicerPage -->

```
<Page>
    <Info>
        <Header>Juicer be juiciiiier!</Header>
        <Body>Look at that thing. It takes fruit and vegetables and turns it into juice. It's like magic.</Body>
    </Info>
    <BackgroundImage File="Assets/page_1.jpg" />
</Page>
```

<!-- snippet-end -->

The classes `Info`, `Header` and `Description` are defined to ensure a consistent look across pages without duplicating code.

## PageIndicator

It's often nice to show the user how many pages there are, and what page they are currently on. In the example we use the `PageIndicator` to do this.

<!-- snippet-begin:code/MainView.ux:PageIndicator -->

```
<PageIndicator Dock="Bottom" Alignment="Center" Margin="5" Navigation="slides">
    <Circle ux:Template="Dot" Width="10" Height="10"  Margin="4">
        <SolidColor ux:Name="dotStrokeBody" Color="#0000" />
        <Stroke ux:Name="dotStroke" Width="2">
            <SolidColor ux:Name="dotStrokeColor" Color="#bbb" />
        </Stroke>
        <ActivatingAnimation>
            <Change dotStrokeBody.Color="#aaa" />
            <Change dotStrokeColor.Color="#aaa" />
        </ActivatingAnimation>
    </Circle>
</PageIndicator>
```

<!-- snippet-end -->

The `PageIndicator` itself is merely the series of dots, so we've centered it in a panel and given it some padding blend into the app.

The attribute `Navigation="thePages"` is how we connect this indicator to a particular `PageControl`. This allows you to put the indicator wherever you'd like in your application; it doesn't need to be directly beside the pages.

> You can also set `Orientation="Vertical"` on the indicator to give it a vertical layout instead of horizontal.
