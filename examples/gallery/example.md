It is not uncommon that you want to select multiple items and delete them from a collection. In this example we will see how that can be done in a gallery-like application, similar to what you might find in your mobile operating system.

In this example we will showcase:

- Layout of pictures in a style similar to _pinterest_
- Multi-selection of these images, allowing the user to tap pictures to add to selection
- Switching to selection mode using the `LongPressed` trigger
- Deleting pictures from the gallery, with a neat animation on the content as it does the new layout

The pictures we are using in this example are downloaded from http://www.unsplash.com, an awesome site to get free-to-use content.

## The JavaScript

We initialize the data from JavaScript:

<!-- snippet-begin:code/MainView.js:Initialization -->

```
function Picture(resource) {
    this.resource = "Assets/Pictures/" + resource + ".jpg";
    this.isSelected = Observable(false);
    this.indicateModeChange = Observable(false);
}

pictures = Observable();
for (i = 1; i < 21; i++) {
    pictures.add(new Picture('Unsplash' + i));
}
```

<!-- snippet-end -->

We use an observable `bool` to track whether we are in selection mode or not:

<!-- snippet-begin:code/MainView.js:SelectionMode -->

```
selectionMode = Observable(false);
numberOfSelected = Observable(0);
```

<!-- snippet-end -->

We also need to track how many items are selected.
These should be reflected in the header of the app:

<!-- snippet-begin:code/MainView.js:Header -->

```
header = Observable(function () {
    if (selectionMode.value === false)
        return 'Gallery';
    else return 'Gallery (' + numberOfSelected.value + ')';
});
```

<!-- snippet-end -->

Now we just need some simple logic to change the state of the app:

<!-- snippet-begin:code/MainView.js:ToggleSelections -->

```
function goToSelectionMode(args) {
    if (selectionMode.value === true) return;
    selectionMode.value = true;
    args.data.indicateModeChange.value = true;
    args.data.isSelected.value = true;
    numberOfSelected.value = 1;
}

function toggleSelect(args) {
    if (selectionMode.value === false) return;
    if (args.data.isSelected.value === false) {
        numberOfSelected.value = numberOfSelected.value + 1;
    } else {
        numberOfSelected.value = numberOfSelected.value - 1;
        if (numberOfSelected.value === 0) {
            selectionMode.value = false;
        }
    }
    args.data.isSelected.value = !args.data.isSelected.value;
}
```

<!-- snippet-end -->

We top off the logic in the app with the ability to delete the pictures:

<!-- snippet-begin:code/MainView.js:DeleteSelected -->

```
function deleteSelected(args) {
    pictures.removeWhere(function (p) {
        return p.isSelected.value === true;
    });
    numberOfSelected.value = 0;
    selectionMode.value = false;
}
```

<!-- snippet-end -->

## The UX

To display the top bar of the app, we add this code to the app `DockPanel`:

<!-- snippet-begin:code/MainView.ux:TopBar -->

```
<StackPanel Dock="Top" Background="#3F51B5">
    <StatusBarBackground />
    <Fuse.iOS.StatusBarConfig Style="Light"/>
    <Panel Height="60">
        <Text Value="{header}" Alignment="Center" Color="#fff"/>
        <Panel Clicked="{deleteSelected}"  Alignment="CenterRight">
            <Image File="Assets/Icons/delete.png" Height="30" Margin="0,0,5,0" />
        </Panel>
    </Panel>
</StackPanel>
```

<!-- snippet-end -->

We also bind the panel holding the trashcan-icon to the `deleteSelected` JavaScript function.

To the `Color` area of the `DockPanel` we add a `ScrollView` which contains this `Panel`:

<!-- snippet-begin:code/MainView.ux:ColumnLayout -->

```
<Panel Alignment="Top">
    <ColumnLayout ColumnCount="3" />
    <Each Items="{pictures}">
```

<!-- snippet-end -->

Simply adding the `ColumnLayout` to the `Panel` is what creates the interesting resulting layout.

All we need to do then is to define the actual instances of the pictures and wire them up to the previously mentioned JavaScript methods:

<!-- snippet-begin:code/MainView.ux:ImageInstances -->

```
<Panel Background="#333" Margin="3">
    <Image ux:Name="checkmark" File="Assets/Icons/Checkmark.png"
            Alignment="TopRight" Margin="5" Width="20" Height="20" Color="#4f6"
            Opacity="0" />
    <Circle ux:Name="checkmarkBackground" Width="20" Height="20" Margin="5" Color="#fff" Alignment="TopRight" Opacity="0" />
    <Tapped>
        <Callback Handler="{toggleSelect}" />
    </Tapped>
    <Image ux:Name="picture" StretchMode="UniformToFill" File="{resource}" />
    <WhileTrue Value="{isSelected}">
        <Change picture.Opacity="0" />
        <Change checkmark.Opacity="0.7" />
        <Change checkmarkBackground.Opacity="1" />
    </WhileTrue>
    <WhileTrue Value="{indicateModeChange}">
        <Change entryScaling.Factor="0.9" Duration="0.2" Easing="CircularInOut" />
        <Callback Handler="{nullModeChange}" Delay="0.4" />
    </WhileTrue>
    <LongPressed>
        <Callback Handler="{goToSelectionMode}" />
    </LongPressed>
</Panel>
```

<!-- snippet-end -->

The combination of `LongPressed` and `Tapped` in concert with some trivial state in the JavaScript creates the functionality we are after, we can press and hold on the first picture to enter multi-selection mode, and then we can tap images to add them to the selection.
