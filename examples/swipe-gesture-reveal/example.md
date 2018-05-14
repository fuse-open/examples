In this example we are implementing a really cool [card navigation](https://dribbble.com/shots/2493845-ToFind-Transition-Test) with a "swipe to reveal details" action. Design by [Leo Leung](https://dribbble.com/leoleung).

Icons are from Google's excellent [Material Icons](https://design.google.com/icons/) and nature images from [UnSplash](https://unsplash.com/).

## The swipe gesture

Each card has a `SwipeGesture` with the `Direction` property set to `Up`. This means that one has to swipe upwards in order to activate the gesture.

<!-- snippet-begin:code/MainView.ux:SwipeGesture -->

```
<SwipeGesture ux:Name="swipe" Direction="Up" Type="Active" />
```

<!-- snippet-end -->

Setting the `Type` property to `Active` means that this gesture works like a switch, which stays on until swiped back down.

We use a `SwipingAnimation` to declare what should happen while the user is swiping. The `Source` property points on the `SwipeGesture` we want to animate in response to.

<!-- snippet-begin:code/MainView.ux:SwipingAnimation -->

```
<SwipingAnimation Source="swipe">
    <Move Target="topPanel" Y="-0.3" RelativeTo="Size" Duration="0.5" />
    <Change bottomPanelScaling.X="1" Duration="0.5" />
    <Change bottomPanelScaling.Y="1" Duration="0.5" />
    <Change bottomPanel.Opacity="1" Duration="0.5" />
</SwipingAnimation>
```

<!-- snippet-end -->

Each card has two layers, the "topPanel" and the "bottomPanel". As we swipe upwards, the "topPanel" is moved in the negative Y direction, while the "bottomPanel" is scaled.

## The navigation

The navigation between cards is a `PageControl` with two of its default properties overriden. We set `InactiveState="Unchanged"` so that inactive pages are not hidden. We set `Transition="None"` because our cards have their own transitions specified.

<!-- snippet-begin:code/MainView.ux:PageControl -->

```
<PageControl InactiveState="Unchanged" Transition="None">
	<NavigationMotion GotoEasing="QuadraticOut" GotoDuration="0.3" />
```

<!-- snippet-end -->

We use `EnteringAnimation` and `ExitingAnimation` to animate the cards as they are navigated to and from. Since we need to show the edges of the next cards, we add some `Scale` to the triggers.

<!-- snippet-begin:code/MainView.ux:EnterExit -->

```
<EnteringAnimation Scale="0.5">
    <Move X="-1.4" RelativeTo="ParentSize" Duration="0.5" />
</EnteringAnimation>
<ExitingAnimation Scale="0.5">
    <Move X="1.4" RelativeTo="ParentSize" Duration="0.5" />
</ExitingAnimation>
```

<!-- snippet-end -->

The inactive cards, should lose some opacity and also scale down slightly. This is done inside an `ActivatingAnimation` trigger.

<!-- snippet-begin:code/MainView.ux:ActivatingAnim -->

```
<ActivatingAnimation>
    <Change self.Opacity="1" Duration="0.5" />
    <Change pageScaling.Factor="1" Duration="0.5" />
</ActivatingAnimation>
```

<!-- snippet-end -->
