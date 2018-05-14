This example shows how you can create a neat material design card animation.
It is inspired by [this awesome piece](https://dribbble.com/shots/1893589-Material-design-card-animation) by [Ivan Bjelajac](https://dribbble.com/ibjelajac).

The icons are from Google's [Material Icons](https://material.io/icons/), and images from [Unsplash](https://unsplash.com/).

# Resources, data and components

Global, static resources such as fonts, text styles and the color palette are defined as a `ux:Class` in a separate file, `StaticResources.ux`.
Since it only contains static resources (`ux:Class` and `ux:Global`) it doesn't need to be instantiated – its contents are automatically available from any other file in our project.

<!-- snippet-begin:code/StaticResources.ux:Resources -->

```
<Panel ux:Class="StaticResources">
    <Font ux:Global="RobotoMedium" File="Assets/Roboto-Medium.ttf" />

    <float4 ux:Global="PrimaryColor" ux:Value="#29B6F6" />
    <float4 ux:Global="AccentColor" ux:Value="#E91E63" />
    
    <Text ux:Class="SmallHeaderText" Font="RobotoMedium" FontSize="17" Color="PrimaryColor" Margin="0,15,0,0" />
    <Text ux:Class="BodyText" FontSize="14" Color="#263238" TextWrapping="Wrap" />
    <BodyText ux:Class="MutedText" Color="#888" />
</Panel>
```

<!-- snippet-end -->

Our made-up mock data comes from a JavaScript file which exports an array of events under the key `events`.

<!-- snippet-begin:code/MainView.ux:IncludeData -->

```
<JavaScript File="data.js" />
```

<!-- snippet-end -->

The app also contains a number of components whose internals aren't relevant to the effect this example focuses on, but mainly serve to mimic parts of a real-world app screen.
To reduce clutter in our `MainView.ux`, these are defined in separate files under the `Components/` directory.

# Navigation and layout

Although this example demonstrates a navigation flow, we are going to use `Selection` and `Selectable` to handle the navigation between the list of cards and the expanded state.
The reason is that we're not actually navigating between pages, but rather moving elements around to expose the content and fill the screen when a card is expanded.
`Selection` fits this use case perfectly, since we can have a `WhileSelected` trigger for each card that animates *that* particular card to the expanded state.
Additionally, we've configured our `Selection` to allow no more than one item to be selected at a time.
This gives us the behavior of a two-state navigation system, where we can either have *zero* selected items (list of cards), or *one* selected item (expanded view of a particular card).

Below is the basic outer structure of the app.

<!-- snippet-begin:code/MainView.ux:OuterStructure -->

```
<DockPanel>
    <StatusAndAppBar ux:Name="statusAndAppBar" Dock="Top" />
    
    <ScrollView ux:Name="mainScrollView" ClipToBounds="false" LayoutMode="PreserveVisual">
        <StackPanel Margin="0,0,0,15">
            <Selection MinCount="0" MaxCount="1" />
            
            <Each Items="{events}">
                <EventCard layoutTarget="contentPlaceholder">
                    <Selectable Value="{id}" />
                    <WhileSelected>
                        <Change mainScrollView.UserScroll="false" DelayBack="0" />
                        <Move Target="statusAndAppBar" Y="-1" RelativeTo="Size" Duration="1.2" Delay="0.1" Easing="QuarticOut" EasingBack="QuarticIn" />
                    </WhileSelected>
                </EventCard>
            </Each>
        </StackPanel>
    </ScrollView>
    
    <BottomBarBackground Dock="Bottom" />
</DockPanel>
```

<!-- snippet-end -->

# The cards

In our mock dataset, we've given each item a unique `id`, which we bind to the `Value` of each card's `Selectable`.
This is required by `Selectable` to identify what is currently selected.

We've also declared a `ImageHeight` property with a default value of 200.
As we'll see next, this is done to reduce duplication, as we'll use this value twice.

Additionally, we have defined a `ux:Dependency="layoutTarget"` that requires us to pass in a reference to the contentPlaceholder `Panel`, so that we can change the `LayoutMaster` of the card contents to an element outside of the `ux:Class`.

<!-- snippet-begin:code/MainView.ux:CardSelectable -->

```
<StackPanel ux:Class="EventCard" ImageHeight="200">
    <float ux:Property="ImageHeight" />
    <Panel ux:Dependency="layoutTarget" />
```

<!-- snippet-end -->

The image part of each card actually contains both the image *and* the expanded content.
However, the `DockPanel` containing the image and content has `ClipToBounds="true"` and is wrapped in a panel whose `Height` is set to the `ImageHeight` property we defined earlier.
This results in only the topmost 200pt of the `imageAndContent` panel being visible.

<!-- snippet-begin:code/MainView.ux:ContentLimitPanel -->

```
<Panel ux:Name="contentLimitPanel" Height="{ReadProperty ImageHeight}">
    <DockPanel ux:Name="imageAndContent" ClipToBounds="true" HitTestMode="LocalBoundsAndChildren">
```

<!-- snippet-end -->

We set the `Height` of the panel containing the image and title to the same `ImageHeight` property as `contentLimitPanel`.
Now the image has the same height as `contentLimitPanel` and thus fits perfectly within the clipped area, rendering the expanded content invisible.

<!-- snippet-begin:code/MainView.ux:ImageAndTitlePanel -->

```
<Panel Dock="Top" Height="{ReadProperty ImageHeight}">
```

<!-- snippet-end -->

# The transition

At the root of our app, we have defined an empty panel named `contentPlaceholder` and as shown before, we have passed it in as a `ux:Dependency` to all of our cards.

<!-- snippet-begin:code/MainView.ux:ContentPlaceholder -->

```
<Panel ux:Name="contentPlaceholder" />
```

```
<Each Items="{events}">
    <EventCard layoutTarget="contentPlaceholder">
```

```
<StackPanel ux:Class="EventCard" ImageHeight="200">
    <Panel ux:Dependency="layoutTarget" />
```

<!-- snippet-end -->

When a card is selected, we change the `LayoutMaster` of the panel containing the image and content to this `layoutTarget`.
This results in `contentLimitPanel` no longer limiting the panel's height, and it can take up all the space inside `contentPlaceholder`, making the content visible.
Since `contentPlaceholder` is placed at the root of our app, this will make it fill the whole screen.

<!-- snippet-begin:code/MainView.ux:ChangeLayoutMaster -->

```
<WhileSelected>
    <Change imageAndContent.LayoutMaster="layoutTarget" DelayBack="0" Delay="0" />
```

<!-- snippet-end -->

We'll add a `LayoutAnimation` to the `imageAndContent` panel to smoothly animate to the new position and size.

<!-- snippet-begin:code/MainView.ux:LayoutAnimation -->

```
<LayoutAnimation>
    <Move X="1" RelativeTo="WorldPositionChange" DelayBack="0" Duration="0.2" Easing="QuadraticInOut" />
    <Move Y="1" RelativeTo="WorldPositionChange" DelayBack="0" Duration="0.2" Easing="SinusoidalIn" />
    <Resize X="1" Y="1" RelativeTo="SizeChange" DelayBack="0" Duration="0.25" Easing="QuadraticIn" />
</LayoutAnimation>
```

<!-- snippet-end -->


Next, we add a `BringToFront` to bring the card in front of all other cards.

Because the card is still rooted within the main `ScrollView`, the `imageAndContent` panel is still tied to the scroll position, even when we change its `LayoutMaster`.
Thus, we immediately disable user interaction for this `ScrollView`, so that the user can't scroll the list of cards while a card is expanded.

Since the expanded content shouldn't have any margin at all, we can't just add a margin around the whole card.
Instead, the same 15pt margin is applied to both the `image` and the card's background separately.
This lets us animate the margin of only the `image` to to 0 during the transition.
Because `image` has `Layer="Background"`, this will not result in any significant performance impact.

The status bar and app bar are moved out of view as well.

It is important to note that we have added a `WhileSelected` trigger in two places - one in the `Each` loop outside of the card `ux:Class`, and another inside of of the `ux:Class`.
This lets us have direct access to UX elements in global and local scopes, respectively. The instructions in both triggers get stacked and executed simultaneously.

<!-- snippet-begin:code/MainView.ux:WhileSelected -->

```
<WhileSelected>
    <Change mainScrollView.UserScroll="false" DelayBack="0" />
    <Move Target="statusAndAppBar" Y="-1" RelativeTo="Size" Duration="1.2" Delay="0.1" Easing="QuarticOut" EasingBack="QuarticIn" />
</WhileSelected>
```

```
<WhileSelected>
    <Change imageAndContent.LayoutMaster="layoutTarget" DelayBack="0" Delay="0" />
    <BringToFront />
    
    <Change image.CornerRadius="0" Duration="0.1" DelayBack="0" />
    <Change image.Margin="0" Duration="0.25" Delay="0" DelayBack="0" Easing="CubicInOut" />
</WhileSelected>
```

<!-- snippet-end -->

Some parts of the transition make more sense to animate the other way, that is, use the active state of a trigger to hide an element instead of expanding or revealing it.
Because of this, we have a separate inverted version of `WhileSelected`, which will be active while the card is *not* selected.

Most of these animations are small details such as scaling the plus button, fading out the content, etc.
We also use it to fade out and disable hit tests for the `detailNavigationBar`, which contains the back button.

<!-- snippet-begin:code/MainView.ux:WhileNotSelected -->

```
<WhileSelected Invert="true">
    <Change contentScrollView.Opacity="0.4" Duration="0.3" DelayBack="0" />
    <Change content.Opacity="0.5" Duration="0.5" DelayBack="0" />
    <Move Target="content" Y="30" Duration="0.7" DelayBack="0" Delay="0" Easing="QuadraticIn" />

    <Scale Target="plusButton" Factor="0.01" Delay="0" Duration="0.2" DelayBack="0.25" DurationBack="0.55" Easing="CubicInOut" />
    <Rotate Target="plusButton" Degrees="-90" Delay="0" Duration="0.5" DelayBack="0.25" DurationBack="1.1" EasingBack="CubicIn" />
    <Change plusButton.Opacity="0" Duration="0.2" DelayBack="0.2" DurationBack="0.5" />
    
    <Change detailNavigationBar.Opacity="0" Duration="0.2" Delay="0" />
    <Change detailNavigationBar.HitTestMode="None" />
</WhileSelected>
```

<!-- snippet-end -->

And that's it! Feel free download the code and play around.
