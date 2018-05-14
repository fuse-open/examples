This examples shows how to implement an animated menu icon. It was inspired by [this cool piece](https://dribbble.com/shots/2281614-Menu-Button-2) by Joby.

The icon itself is composed of three rectangles in a `Grid` of three rows.

The animation is divided into two parts. The scaling of the button when its pressed, and the animation from hamburger to cross icon.

A `Clicked`-trigger is used with a `Scale`-animator for the first part. The `Clicked`-trigger also toggles a `WhileTrue`-trigger which handles moving and rotating the rectangles.

<!-- snippet-begin:code/MainView.ux:App -->

```
<App Background="#333">
	<Panel ux:Class="Hamburger" HitTestMode="LocalBounds">
		<Rectangle ux:Class="BurgerPart" Color="White" StrokeWidth="1" StrokeColor="#999" CornerRadius="5" Height="10" Width="50" Margin="0, 2.5"/>

		<Grid RowCount="3">
			<BurgerPart ux:Name="rect1" />
			<BurgerPart ux:Name="rect2" />
			<BurgerPart ux:Name="rect3" />
		</Grid>
		
		<Clicked>
			<Scale>
				<Keyframe Value="0.8,0.8" Time="0.05"/>
				<Keyframe Value="1,1" Time="0.1"/>
			</Scale>
			<Toggle Target="clickedState" Delay="0.1"/>
		</Clicked>
		
		<WhileTrue ux:Name="clickedState">
			<Change rect2.Width="10" Duration="0.4" Easing="BounceOut" EasingBack="BounceIn" DelayBack="0.6"/>
			<Move Target="rect1" Easing="QuadraticInOut" Y="1.5" RelativeTo="Size" Duration="0.3" Delay="0.3"/>
			<Move Target="rect3" Easing="QuadraticInOut" Y="-1.5" RelativeTo="Size" Duration="0.3" Delay="0.3"/>
			<Rotate Target="rect1" Easing="QuadraticInOut" Degrees="45" Duration="0.3" Delay="0.45" DelayBack="0"/>
			<Rotate Target="rect3" Easing="QuadraticInOut" Degrees="-45" Duration="0.3" Delay="0.45" DelayBack="0"/>
		</WhileTrue>
	</Panel>

	<Hamburger Alignment="Center" />
</App>
```

<!-- snippet-end -->
