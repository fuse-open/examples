This example, based on the awesome [Ecommerce-UX](https://dribbble.com/shots/2249964-Ecommerce-UX) design by [Levani Ambokadze](https://dribbble.com/Amboka), shows the use of `SwipeGesture` and `SwipeNavigate`, as well as how you can achieve 3D transformations using `Rotate` and `Viewport`.

The icons are from Google's [Material Icons pack](https://design.google.com/icons/).

# Swipe navigation

Swiping left and right on both detail cards and the preview photos is done using a `PageControl`, which handles linear navigation between panels using swipes. The following is a simplification of the two `PageControl`s used in the app:

```
<Panel>
    <PageControl>
    	<Each Count="3">
			<Panel Margin="10,10,10,10">
				<Panel Color="#F00">
					<Translation Y="0.46" RelativeTo="ParentSize" ux:Name="pageTranslation" />
				</Panel>
			   	<Panel Alignment="Top" Height="60%"> <!-- Preview images -->
					<PageControl>
						<Panel Color="#0FA" />
						<Panel Color="#0AF" />
						<Panel Color="#F0A" />
					</PageControl>
				</Panel>
		   	</Panel>
	    </Each>
    </PageControl>
</Panel>
```

Note that the `PageControl` for navigating the preview images is a child of the `PageControl` which navigates the pages. In cases like this, the child `PageControl` will always get input, rather than the parent. Additionally, this snippet uses default transition animations instead of custom ones. In our example app, we set `Transition="None"` on the `PageControl`s to enable custom animations, and specified our own `EnteringAnimation` and `ExitingAnimation`. These will be discussed in further detail later in the article.

# Swiping up to reveal more details

This effect uses a `SwipingAnimation` hooked up to a `SwipeGesture` in order to translate the detail card upwards as the user swipes up. While a `PageControl` uses swipe gestures to navigate pages, `SwipeGesture` is a more general way of handling swipes.

The `SwipeGesture` itself is a child of the first panel after our `Viewport` element (More on this later). This ensures the entire app can take swipe events through that `SwipeGesture`. This is because `SwipeGesture`s listen for swipes in the area of the screen their parent container populates.

For the actual translation of the cards, we put a `SwipingAnimation` and `Translation` element in the cards, like this:

```
<SwipingAnimation Source="swipeGesture">
	<Change pageTranslation.Y=".12" />
	<Change moreDiv.Opacity="1" Easing="ExponentialOut" />
	<Change infoPointPanel.Opacity="1" Easing="CubicIn" />
	<Change ratingsGrid.Opacity="1" Easing="ExponentialIn" />
</SwipingAnimation>
<Translation Y="0.46" RelativeTo="ParentSize" ux:Name="pageTranslation" />
```

## The unused SwipeGesture in the preview image container

If you had a keen eye while looking over the example code, you might of noticed there is an unused `SwipeGesture` in the preview image container:

```
<Panel ux:Name="topPart" Alignment="Top" Height="60%">
	<SwipeGesture Direction="Up"/>
	<!-- [...] -->
</Panel>
```

This works as a mask, telling the `SwipeGesture` closer to the root node (`App`) to ignore swipes happening on that panel. This happens because `SwipeGesture` respects other `SwipeGesture`s deeper down on the node tree. This way, by putting an unused `SwipeGesture` on the top container, the parent `SwipeGesture` will ignore that area.

# Rotating the detail cards in 3D

Fuse supports 3D transformation out of the box. However, to pull this off, you need to wrap your UX in a `Viewport` element. This element handles perspective projection. We initialize the viewport like this:

```
<App>
	<Viewport Perspective="300" CullFace="Back">
		<!-- Your app -->
	</Viewport>
</App>
```

It is important to set `CullFace` to `Back` so any rotated elements aren't visible if they face away from the camera.

The cards look rather ugly the last degrees before they are rotated out of the way, as there is a considerable amount of aliasing. To fix this, we fade out the cards as they leave the center of the screen using an exponential easing method.

Lastly, the other elements that make up the cloth display have to be moved out of the way as the clothing card is swiped away. In our example, this is the preview image navigation panel, and the progress dots for said navigation panel. The following snippet shows the enter/exit animations in their entirety:

```
<EnteringAnimation>
	<Rotate Target="bottomPartOuter" DegreesY="70" Duration="1" />
	<Change bottomPart.Opacity="0" Easing="ExponentialIn" Duration=".75" Delay=".0"/>
	<Change topScaling.Factor="0.5" Duration="1" />
	<Change topPart.Opacity="0" Duration="1" Easing="ExponentialOut" />
	<Change pageIndicatorPart.Opacity="0" Duration="1" Easing="ExponentialOut" />
</EnteringAnimation>
<ExitingAnimation>
	<Rotate Target="bottomPartOuter" DegreesY="-70" Duration="1" />
	<Change bottomPart.Opacity="0" Easing="ExponentialIn" Duration=".75" Delay=".0"/>
	<Change topScaling.Factor="0.5" Duration="1" />
	<Change topPart.Opacity="0" Duration="1" Easing="ExponentialOut" />
	<Change pageIndicatorPart.Opacity="0" Duration="1" Easing="ExponentialOut" />
</ExitingAnimation>
```

# Styling

It is good practice when writing UX to define all custom UI elements as classes with semantic names. This results in the UX communicating well to a third party reader what different parts of the UX document does. The example has therefore almost all items that visually look like an UI element defined in classes. Additionally, due to the amount of classes used in this example, we put them in a separate UX file which we included. This reduces clutter in `MainView.ux`, something which is very useful when working with large projects.

We pre-define all our theme colors. This helps make the UX more understandable, as it is easier to comprehend which visual element you are looking at the UX for:

```
<float4 ux:Global="FontColor" ux:Value="#556E7A" />
<float4 ux:Global="LighterFontColor" ux:Value="#556D79" />
<float4 ux:Global="InactiveColor" ux:Value="#ABB5BC" />
<float4 ux:Global="HintColor" ux:Value="#DEEBF4" />

<float4 ux:Global="CardBackground" ux:Value="#FFF" />
<float4 ux:Global="BackgroundColor" ux:Value="#f3f3f5" />

<float4 ux:Global="ColorSelRed" ux:Value="#F25254" />
<float4 ux:Global="ColorSelBlue" ux:Value="#6389AF" />
<float4 ux:Global="ColorSelGreen" ux:Value="#70C1B3" />
<float4 ux:Global="ColorSelYellow" ux:Value="#FFE664" />
```

Further, we use classes to define both regularly used sets of properties, and more complicated custom UI elements. The following snippet shows how the circular `SizeButton` is defined. Notice how we utilize `ux:Property` to add our own property to our new class:

```
<Circle ux:Class="SizeButton" Width="40" Height="40" Margin="15">
  <string ux:Property="Label" />
  <Text Alignment="Center" Value="{ReadProperty Label}" Color="FontColor" />
  <Stroke Width="2">
    <SolidColor Color="InactiveColor" />
  </Stroke>
</Circle>
```

The following is an example of a more simple use of `ux:Class`, for often used properties.

```
<Image ux:Class="Icon" Margin="15" Color="FontColor" Height="40" />
```

Last but not least, we include `Style.ux` in our main file using:

```
<ux:Include File="Style.ux" />
```

That's about it!
