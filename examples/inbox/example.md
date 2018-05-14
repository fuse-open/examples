A quite common use case is to use swipe gestures to perform actions on items in a list. This example uses `SwipeGesture` to create an inbox with items that can either be postponed or removed by swiping it to the left or right.

## Setting up the data

The data is defined as a simple Observable collection of `Message` objects:

<!-- snippet-begin:code/Inbox.ux:Message -->

```
function Message(icon, sender, subject, summary) {
    this.sender = sender;
    this.subject = subject;
    this.icon = icon;
    this.summary = summary;
}
```

<!-- snippet-end -->

<!-- snippet-begin:code/Inbox.ux:DataSource -->

```
var messages = Observable(
	new Message(...
    // ...The actual data...
);
```

<!-- snippet-end -->

This is then bound to from UX using an `Each` to create an item for each `Message`.

## Handling swipe gestures

For this example we'll use two SwipeGestures per item. One which listens to left swipes, and one that listens to right swipes:

Structurally, each element in the inbox looks like this:

	<Panel>
		<SwipeGesture ux:Name="swipeRight" LengthNode="appDock" Direction="Right" Type="Active" />
		<SwipeGesture ux:Name="swipeLeft" LengthNode="appDock" Direction="Left" Type="Active" />

		<!-- Swipe animations -->
		<SwipingAnimation Source="swipeRight">
		</SwipingAnimation>
		<SwipingAnimation Source="swipeLeft">
		</SwipingAnimation>

		<!-- Swipe triggers -->
		<Swiped Source="swipeRight">
		</Swiped>
		<Swiped Source="swipeLeft">
		</Swiped>

		<!-- The inbox entry -->
		<Panel ux:Name="contents" />
	</Panel>

The animation code for the "postpone" animation looks like this:

	<SwipingAnimation Source="swipeRight">
		<Set postponeText.Opacity="0" />
		<Set doneText.Opacity="1" />
		<Set postponeIcon.Opacity="0" />
		<Set checkmarkIcon.Opacity="1" />
		<Change background.Color="#050" Easing="ExponentialOut" />
		<Scale Target="checkmarkIcon" Factor="2.8" Easing="BackOut" />
	</SwipingAnimation>
	<Swiped Source="swipeRight">
		<Callback Handler="{removeItem}" />
	</Swiped>


As you can see, we use @(SwipingAnimation) to change the color of the background element and at the same time fade out the icon at the opposite end. Then, when the item has been fully swiped to the right, we call the `removeItem` function in JavaScript using the @(Swiped)-trigger, which fires when the @(SwipeGesture) is complete.

```
// removes the item from the list
function removeItem(sender) {
    messages.remove(sender.data);
}

// move the item to the end of the list
function postponeItem(sender) {
    messages.remove(sender.data);
    messages.add(sender.data);
}
```

Finally, the main content is simple layout:

<!-- snippet-begin:code/Inbox.ux:Contents -->

```
<Page Background="#fff" ux:Name="contents">
    <DockPanel Margin="7,1,7,0">
        <Circle Height="50" Width="50" Dock="Left" Margin="7,7,0,7" Alignment="TopLeft">
            <ImageFill File="{icon}" />
        </Circle>

        <StackPanel Margin="7,3,7,7">
            <Text Font="Regular" Value="{subject}" FontSize="16" />
            <Text Font="Regular" Value="{sender}" FontSize="16" Color="#999" />
            <Text TextWrapping="Wrap" Font="Light" Value="{summary}" FontSize="14" Color="#000" />
        </StackPanel>
    </DockPanel>
</Page>
```

<!-- snippet-end -->

We add some elements in the background so that we get an indication what operation we are doing:

<!-- snippet-begin:code/Inbox.ux:BackgroundElements -->

```
<Text ux:Class="Operation" Font="Regular" TextAlignment="Center" Alignment="Center"
    Color="#fff" FontSize="22" />
<Operation ux:Name="doneText" >Done</Operation>
<Operation ux:Name="postponeText">Postponed</Operation>
<Image ux:Class="Icon" Width="20" Height="20" Margin="30" />
<Icon ux:Name="checkmarkIcon" Alignment="CenterLeft"
    File="Assets/Icons/Checkmark.png" />
<Icon ux:Name="postponeIcon" Alignment="CenterRight"
    File="Assets/Icons/Postpone.png" />
```

<!-- snippet-end -->

## Animation

We finish the example off by adding some animations for when an item is removed:

<!-- snippet-begin:code/Inbox.ux:Animations -->

```
<RemovingAnimation>
    <Move RelativeTo="Size" Y="-1" Duration="0.4" Easing="CircularInOut" />
</RemovingAnimation>

<LayoutAnimation>
    <Move RelativeTo="LayoutChange" Y="1" Duration="0.8" Easing="CircularInOut" />
</LayoutAnimation>
```

<!-- snippet-end -->

This ensures that the user gets the desired visual feedback as the collection changes.
