This example shows how to create an immersive swipeable cards experience. The screen pops with color and you can even interact with the products shown!

Inspiration: [this good-looking cards animation](https://dribbble.com/shots/3467160-Sample-interaction-with-cards-interface) by [Artem Miskevich](https://dribbble.com/imickaa). All images are taken from [unsplash.com](https://unsplash.com/).

# Animated folder and vinyl

The products in our application are represented as "cards", so we start out by creating a reusable `ux:Class` component for them. Although the `Card` class holds the whole layout for a product, we'll take a look at how the animated folder and vinyl are created.

Inside of our `Card` class, we put a `Panel` that holds our folder and vinyl. While the folder `Rectangle` is simply styled with `ImageFill` that fills it with the supplied cover image, the vinyl element uses a semi-transparent `vinyl.png` image as a mask on top of a scaled-down version of the same cover art. Finally, we set the `Alignment="HorizontalCenter"` on both elements, so that the folder is right on top of the vinyl.
```
<Panel Margin="16" MaxWidth="320">

	<Rectangle ux:Name="folder" CornerRadius="1" Color="#fff8" Alignment="HorizontalCenter" Width="168">
		<ImageFill File="{ReadProperty Cover}" StretchMode="UniformToFill" WrapMode="ClampToEdge" />
		<Stroke Width="1" Color="#FFF3" />
		<Shadow />
	</Rectangle>

	<Panel ux:Name="vinyl" Alignment="HorizontalCenter" Width="168">
		<Circle ux:Name="disc" Margin="2">
			<Image File="Assets/vinyl.png" />
			<Circle Width="50%" Height="50%">
				<ImageFill File="{ReadProperty Cover}" StretchMode="UniformToFill" WrapMode="ClampToEdge" />
			</Circle>
		</Circle>
		<Circle Color="#0004" Smoothness="24" />
	</Panel>

</Panel>
```

To make the vinyl slide in and out of the folder, we will change their `Alignment`. For that to work, we need to do several things. On the parent container, we set `MaxWidth="320"` so that the vinyl can never fully leave the folder. In addition to that, we add a `WhileTrue` trigger that changes the `Alignment` on both `vinyl` and `folder`.
```
<Panel Margin="16" MaxWidth="320">
	<WhileTrue ux:Name="folderOpen">
		<Change folder.Alignment="Left" />
		<Change vinyl.Alignment="Right" />
	</WhileTrue>
</Panel>
```

Since we don't want to move the `folder` and `vinyl` elements over a fixed width, we add a `LayoutAnimation` trigger to both of them. As we `Change` the `Alignment`, they will `Move` in an arbitrary direction (specified by `Vector="1"`), relative to the position change.
```
<LayoutAnimation>
	<Move Vector="1" RelativeTo="PositionChange" DurationBack="0.4" Easing="QuarticOut" EasingBack="QuarticIn" />
</LayoutAnimation>
```

Finally, we add a `Tapped` trigger on the `folder` element, which calls `Toggle` on the `WhileTrue` that triggers the alignment change.
```
<Tapped>
	<Toggle Target="folderOpen" />
</Tapped>
```

For the final touch here, we want to get the record spinning! We add another `WhileTrue` and `Toggle` it from a `Tapped` trigger on the `vinyl` circle. The enclosed `Spin` animator then continuously rotates the `disc` element.
```
<WhileTrue ux:Name="recordSpinning">
	<Spin Target="disc" Frequency="0.8" />
</WhileTrue>

<Panel ux:Name="vinyl" Alignment="Center" Width="168" Height="100%">
	<LayoutAnimation>
		<Move Vector="1" RelativeTo="PositionChange" DurationBack="0.4" Easing="QuarticOut" EasingBack="QuarticIn" />
	</LayoutAnimation>
	<Tapped>
		<Toggle Target="recordSpinning" />
	</Tapped>
	<Circle ux:Name="disc" Margin="2">
		<Image File="Assets/vinyl.png" />
		<Circle Width="50%" Height="50%">
			<ImageFill File="{ReadProperty Cover}" StretchMode="UniformToFill" WrapMode="ClampToEdge" />
		</Circle>
	</Circle>
	<Circle Color="#0004" Smoothness="24" />
</Panel>
```

# Background gradient transitions

We put all the cards in a `PageControl` which allows us to swipe between them. The data for our cards comes from JavaScript.
```
<JavaScript>
	var cards = [
		{
			title: "FREE LIMITED LP",
			description: "Despite the title, Hundredth's third LP Free doesn't feel liberating by any means. There hasn't been a reinvention of t...",
			artist: "Hundredth",
			artistPic: "Assets/artist1.jpeg",
			cover: "Assets/1.jpg",
			albums: "4",
			topColor: "#cdb8b5",
			bottomColor: "#4f3250"
		},
		...
	];
	module.exports = {
		cards: cards
	};
</JavaScript>
...
<PageControl ux:Name="cards" Padding="24,32,24,8">
	<Each Items="{cards}">
		<Card Title="{title}" Description="{description}" Artist="{artist}"
				ArtistPic="{artistPic}" Cover="{cover}" Albums="{albums}"
				TopColor="{topColor}" BottomColor="{bottomColor}">
		</Card>
	</Each>
</PageControl>
```

We can use a `WhileActive` trigger together with our `PageControl`, and that comes in handy for the gradient color transitions. Under each card, we put a `WhileActive` trigger with a `Threshold="0.5"` property, making the trigger activate when the card we're navigating to is half-way in. From there we set values of two `Attractor` nodes.

The `Attractor` values are bound to the `Color` property of `GradientStop` nodes in a `LinearGradient`. The use of an `Attractor` allows us to smoothly animate colors between multiple arbitrary values, - and since we can swipe between our cards both left and right, it's exactly what we need!
```
<Card>
	<WhileActive Threshold="0.5">
		<Set topColor.Value="{topColor}" />
		<Set bottomColor.Value="{bottomColor}" />
	</WhileActive>
</Card>
...
<Attractor ux:Name="topColor" Target="colorTop.Color" Value="#cdb8b5" Type="Easing" Duration="0.2" DurationExp="0" />
<Attractor ux:Name="bottomColor" Target="colorBottom.Color" Value="#4f3250" Type="Easing" Duration="0.2" DurationExp="0" />
<Rectangle>
	<LinearGradient StartPoint="0,0" EndPoint="0,1" AngleDegrees="72">
		<GradientStop ux:Name="colorTop" Offset="0" />
		<GradientStop ux:Name="colorBottom" Offset="1" />
	</LinearGradient>
</Rectangle>
```

# Custom page indicators

The use of `PageControl` presents another advantage, since it allows us to use the flexible `PageIndicator` class. Our indicator is a simple white `Circle` with a tiny `Rectangle` inside. We make it pop with a `DeactivatingAnimation` - as we navigate away from a card, the corresponding page indicator is faded out, rotated by 360 degrees and made a bit smaller. When a card is activated, the same animation is played backwards, so we only need one!
```
<PageIndicator Height="56" Navigation="cards" Alignment="Center">
	<StackLayout ItemSpacing="12" Orientation="Horizontal" />
	<Panel ux:Template="Dot">
		<DeactivatingAnimation>
			<Scale Factor="0.8" />
			<Change indicator.Opacity="0.4" />
			<Rotate Degrees="360" />
		</DeactivatingAnimation>
		<Circle ux:Name="indicator" Width="12" Height="12" Color="#fff">
			<Rectangle Width="2" Height="7" Color="#0008" CornerRadius="1">
				<Translation Vector="-2,-2,0" />
				<Rotation Degrees="-45" />
			</Rectangle>
		</Circle>
	</Panel>
</PageIndicator>
```

That's it for this example. Feel free to download and play with the source code.
