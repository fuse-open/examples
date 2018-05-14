In this example we use `PanGesture`, `ZoomGesture` and `RotateGesture` to allow direct manipulation of an element's 2D transformation.
All of these gestures require an `InteractiveTransform`, which is passed through the `Target` parameter.
This `InteractiveTransform` is what actually transforms its parent element in response to the movement interpreted by Pan- Zoom- and RotateGesture.

We've also included a `Shadow` whose parameters are controlled by [UX Expressions](/docs/ux-markup/expressions) to give the illusion of depth.

**Note:** Some of the gestures in this example requires multi-touch, and thus won't be usable from local preview. Please run the example on a device.

```
<App>
	<iOS.StatusBarConfig Style="Light" />
	<Panel Color="#2196F3">
		<Rectangle ux:Name="theRect" Color="#fff" Width="120" Height="120" CornerRadius="3" HitTestMode="None">
			<InteractiveTransform ux:Name="transform" />
			<Shadow Color="#0004"
			        Angle="{Property transform.Rotation} * (180/3.14159) + 90"
			     Distance="max(1, ({ReadProperty transform.ZoomFactor} - 1) * 25)"
			         Size="max(2, ({ReadProperty transform.ZoomFactor} - 1) * 30)" />
		</Rectangle>
		
		<PanGesture Target="transform" ConstrainElement="theRect" />
		<ZoomGesture Target="transform" Minimum="1" Maximum="2.5" />
		<RotateGesture Target="transform" />
	</Panel>
</App>
```
