In this example we decided to take on another [UpLabs challenge](https://stories.uplabs.com/can-you-code-this-ui-concept-vol-2-9c9763269640#.abop8q8x6) by implementing this very cool [signup flow animation](http://www.materialup.com/posts/material-signup-interaction). You will learn how to animate layout between completely different app states.

# Defining some colors

We define all our colors in a separate file called Resources.ux, enclosed in a `ux:Resources` tag:

Resources.ux:

<!-- snippet-begin:code/Resources.ux:SomeColors -->

```
<ux:Resources>
    <float4 ux:Global="Green"      ux:Value="#14FFB5"/>
    <float4 ux:Global="Purple"     ux:Value="#5E2E91"/>
    <float4 ux:Global="White"      ux:Value="#FFFFFF"/>
    <float4 ux:Global="Gray"       ux:Value="#999999"/>
    <float4 ux:Global="TopGray"    ux:Value="#9F83BD"/>
    <float4 ux:Global="BottomGray" ux:Value="#7E58A7"/>
</ux:Resources>
```

<!-- snippet-end -->

# Three states

The app can be divided into three main states:

- The initial state where you only have a logo and a "signup button".
- The signup form with a "done button".
- The main view which becomes visible after signing up.

One thing that makes this animation special is the fact that we don't navigate between pages with completely different content. Instead, we move the same element around based on the current state so that that element can serve different visual purposes. This makes the flow of our app seem very elegant and natural.

# A convenient Panel

Since we need to show and hide quite a few different panels based on the state of our app we create a custom `Panel` class called `HidingPanel`. It is not much different from a normal `Panel` except that it fades away when being disabled by setting `IsEnabled="false"`.

<!-- snippet-begin:code/HidingPanel.ux:HidingPanel -->

```
<Panel ux:Class="HidingPanel" Opacity="0" IsEnabled="false" HitTestMode="LocalBoundsAndChildren">
    <WhileEnabled>
        <Change this.Opacity="1" Duration="0.4" />
    </WhileEnabled>
</Panel>
```

<!-- snippet-end -->

# The white rectangle

The white rectangle plays the main role of this animation. It starts of as the signup button, moves on to become the container for our registration form and ends up as the background for our entire app. Defining this kind of animation is a bit different from what we are used to. We usually define our animations as deviations from a rest state, but that implies that we only have two states to care about per animation.

This time, we have several states, and would like to be able to jump between them. We also dont want to move our white rectangle around using offsets from the start position. An easier approach is to use Fuse' layout system to define the various target positions. We can then use the same `Rectangle` put in a background layer and move it around from one panel to another based on the current state. We use the `LayoutMaster` property to make our white `Rectangle` take on the layout of another `Element` and the `Move` and `Resize` animators to make it animate smoothly between them.

<!-- snippet-begin:code/MainView.ux:MoveResize -->

```
<Move Target="whiteRect" RelativeTo="PositionOffset" RelativeNode="signupButton"
		Vector="1" Duration="0.2" DurationBack="0" />
<Resize Target="whiteRect" RelativeTo="Size" RelativeNode="signupButton"
		Vector="1" Duration="0.2" DurationBack="0" />
```

<!-- snippet-end -->

When the animation is finished, we also set the `LayoutMaster` property to the same node as we used for our `RelativeNode`, which acts as the target for our animation.

<!-- snippet-begin:code/MainView.ux:SetLayoutMaster -->

```
<Set whiteRect.LayoutMaster="signupButton" Delay="0.2" />
```

<!-- snippet-end -->

# The rest

We also have a green rectangle playing a major role in much the same way as the white rectangle, where it moves between layout positions. It works in the exact same way. The rest of the UX is standard Fuse layout and animation.

And that’s it! Feel free to download the source and play around.
