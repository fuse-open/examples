This example shows how to make a neat loading button with different success and error states.

# The button

The button uses a `StateGroup` to animate between four states: defaultState, loadingState, success and error.

The check and error icons are contained inside `WhileTrue` triggers, which are activated by their respective states.

The loading animation is achieved by animating the `StartAngleDegrees` and `EndAngleDegrees` properties of an `Arc`.

<!-- snippet-begin:code/LoadButton.ux:LoadButton -->

```
<Panel ux:Class="LoadButton" Width="150" Height="60">
	<Text ux:Name="text" Alignment="Center" Value="Load" Color="#090" FontSize="16" Opacity="1"/>
	<Panel>
		<WhileTrue ux:Name="showCross">
			<CrossIcon ux:Name="crossIcon" Opacity="0"/>
			<Change crossIcon.Opacity="1" Duration="0.05"/>
		</WhileTrue>
	</Panel>
	<Panel>
		<WhileTrue ux:Name="showCheck">
			<CheckIcon ux:Name="checkIcon" Opacity="0"/>
			<Change checkIcon.Opacity="1" Duration="0.05"/>
		</WhileTrue>
	</Panel>

	<Arc ux:Name="arc" StartAngleDegrees="-90" EndAngleDegrees="-90" Width="56" Height="56">
		<Stroke ux:Name="arcStroke" Width="4" Alignment="Center" Color="#090" />
	</Arc>

	<Rectangle ux:Name="rect" CornerRadius="30" Width="146" Height="56">
		<SolidColor ux:Name="rectColor" Color="#fff" />
		<Stroke ux:Name="rectStroke" Width="4" Alignment="Center">
			<SolidColor ux:Name="rectStrokeColor" Color="#090"/>
		</Stroke>
	</Rectangle>

	<WhilePressed>
		<Scale Target="text" Factor="0.9" Duration="0.05"/>
	</WhilePressed>
	<Clicked>
		<Callback Handler="{startLoading}"/>
	</Clicked>


	<StateGroup Active="{finishState}" Rest="defaultState" Transition="Exclusive">
		<State ux:Name="defaultState"/>
		<State ux:Name="loadingState">
			<Change DurationBack="0" rectStrokeColor.Color="#ddd" Duration="0.3" />
			<Change DurationBack="0" text.Opacity="0" Duration="0.15"/>
			<Change DurationBack="0" rect.Width="56" Duration="0.4" Easing="CircularInOut"/>
			<Change DurationBack="0" rectColor.Color="#fff" Duration=".3"/>
			<Change DurationBack="0" arc.EndAngleDegrees="269.9" Delay="0.4" Duration="1" />
			<Callback Handler="{doneLoading}" Delay="1.4" When="Forward"/>
		</State>
		<State ux:Name="success">
			<Change text.Opacity="0" DurationBack="0.2" DelayBack="0.3"/>
			<Change rect.Width="56" Duration="0" DurationBack="0.4" Easing="CircularInOut"/>
			<Change rectStrokeColor.Color="#090" Duration=".3"/>
			<Change rectColor.Color="#090" Duration=".3"/>
			<Change showCheck.Value="true"/>
		</State>
		<State ux:Name="error">
			<Change text.Opacity="0" DurationBack="0.2" DelayBack="0.3"/>
			<Change rect.Width="56" Duration="0" DurationBack="0.4" Easing="CircularInOut"/>
			<Change arcStroke.Color="#900" Duration=".3"/>
			<Change rectStrokeColor.Color="#900" Duration=".3"/>
			<Change rectColor.Color="#900" Duration=".3"/>
			<Change showCross.Value="true"/>
		</State>
	</StateGroup>
</Panel>
```

<!-- snippet-end -->


# The check icon

The check icon is made with two rotated rectangles. One of them is half the size of the other and rotated in the opposite direction. It has an `AddingAnimation` which animates the rectangles `Width`-properties when the icon is added to the visual tree. We set the `TransformOrigin` to anchor which allows us to rotate the rectangles based on their `Anchor` property.

<!-- snippet-begin:code/CheckIcon.ux:CheckIcon -->

```
<Panel ux:Class="CheckIcon" ux:Name="self" Width="25" Height="25">
    <Panel Offset="-10%,30%">
        <Panel Width="100%" Height="100%">
            <Rotation Degrees="45" />
            <Rectangle ux:Name="checkRect1" Alignment="Left" TransformOrigin="Anchor" Anchor="0%,50%" Width="50%" Height="10%" Color="White"/>
        </Panel>
        <Rectangle ux:Name="checkRect2" Offset="-5%,0%" TransformOrigin="Anchor" Anchor="100%,50%" Width="100%" Height="10%" Color="White">

            <Rotation Degrees="135"/>
        </Rectangle>
        <AddingAnimation>
            <Change checkRect2.Width="0" Duration="0.5" Easing="BounceIn"/>
            <Change checkRect1.Width="0" Duration="0.15" Delay="0.5" Easing="QuadraticOut"/>
        </AddingAnimation>
        <RemovingAnimation>
            <Change self.Opacity="0" Duration="0.1"/>
        </RemovingAnimation>
    </Panel>
</Panel>
```

<!-- snippet-end -->

# The cross icon

The cross icon is also composed of two rectangles, rotated at 45 and -45 degrees. The `Width` of the recrangles are animated with an `AddingAnimation`.

<!-- snippet-begin:code/CrossIcon.ux:CrossIcon -->

```
<Panel Width="30" Height="30" ux:Class="CrossIcon" ux:Name="self">
    <Rectangle ux:Name="crossRect1" Width="100%" Height="10%" Color="White">
        <Rotation Degrees="-45"/>
    </Rectangle>
    <Rectangle ux:Name="crossRect2" Width="100%" Height="10%" Color="White">
        <Rotation Degrees="45"/>
    </Rectangle>
    <AddingAnimation>
        <Change crossRect1.Width="15" Duration="0" DurationBack="0.5" Easing="BounceIn"/>
        <Change crossRect2.Width="15" Duration="0" DurationBack="0.5" Easing="BounceIn"/>
    </AddingAnimation>
    <RemovingAnimation>
        <Change self.Opacity="0" Duration="0.1"/>
    </RemovingAnimation>
</Panel>
```

<!-- snippet-end -->

# JavaScript

JavaScript is just used to maintain some state per button and allows us to simulate a loading state.

<!-- snippet-begin:code/MainView.js:ButtonState -->

```
function LoadButton(doneLoadingState){
    this.doneLoadingState = doneLoadingState;
    this.finishState = Observable("defaultState");
    this.doneLoading = function(){
        this.finishState.value = this.doneLoadingState;
    }.bind(this);
    this.startLoading = function(){
        if (this.finishState.value === "defaultState")
            this.finishState.value = "loadingState";
        else
            this.finishState.value = "defaultState";
    }.bind(this);
}
```

<!-- snippet-end -->
