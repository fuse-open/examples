This example shows how we can create a segmented code input control with neat animations between focused and non-focused states.
Design inspiration was taken from the clean and attractive [Code input field concept](https://dribbble.com/shots/2163930-Code-input-field-concept) by Samuel Kantala.

Our `SegmentedInput` is a reusable component and contains both UX code for layout and JavaScript for the logic. Let's take a look at the basic structure of it:
```
<StackPanel ux:Class="SegmentedInput" HitTestMode="LocalBounds">
    <string ux:Property="Label" />
    <string ux:Property="Code" />

    <JavaScript>
        var Observable = require("FuseJS/Observable");

        var editMode = Observable(false);
        function enterEditMode() {
            editMode.value = true;
        };
        function exitEditMode() {
            editMode.value = false;
        };

        var codeLength = 5;
        var code = this.Code;

        module.exports = {
            code: code,
            codeLength: codeLength,
            editMode: editMode,
            enterEditMode: enterEditMode,
            exitEditMode: exitEditMode
        };
    </JavaScript>

    <Clicked>
        <GiveFocus Target="codeInput" />
    </Clicked>

    <TextInput ux:Name="codeInput" Value="{code}" MaxLength="{codeLength}" InputHint="Number"
        Layer="Background" Visibility="Hidden" HitTestMode="None"
        Focus.Gained="{enterEditMode}" Focus.Lost="{exitEditMode}" />

    <Panel Height="24">
        <Text Value="{ReadProperty Label}" FontSize="20" Color="Highlight" Alignment="TopLeft" TransformOrigin="TopLeft">
            <Translation ux:Name="labelTrans" Y="1" RelativeTo="ParentSize" />
            <WhileTrue Value="{editMode}">
                <Change labelTrans.Y="0" Delay="0.2" Duration="0.42" Easing="ExponentialOut" EasingBack="ExponentialIn" />
                <Scale Factor="0.8" Delay="0.2" Duration="0.42" Easing="ExponentialOut" EasingBack="ExponentialIn" />
            </WhileTrue>
        </Text>
    </Panel>

    <Grid ColumnCount="{codeLength}" Height="40">
        <Each Items="{symbols}">
            <SymbolBox Symbol="{symbol}" />
        </Each>
    </Grid>
</StackPanel>
```
We put the label in a `Panel` on top of the `Grid` that will hold our symbol boxes. We offset the label vertically by the height of its parent so it moves closer to the bottom line:
```
<Panel Height="24">
    <Text Value="{ReadProperty Label}" FontSize="20" Color="Highlight" Alignment="TopLeft" TransformOrigin="TopLeft">
        <Translation ux:Name="labelTrans" Y="1" RelativeTo="ParentSize" />
    </Text>
</Panel>
```

As we are making a reusable component, we expose two properties that we specify when we use the component: `Label` and `Code`. The `Code` property is implicitly available in our JavaScript as an `Observable` that we can refer to as `this.Code`;
```
    <string ux:Property="Label" />
    <string ux:Property="Code" />
    <JavaScript>
        var code = this.Code;
```

We then add a `TextInput` and data-bind it to our `code` Observable in JavaScript. Since the `TextInput` is only necessary for summoning the on-screen keyboard and gathering user input, we hide it completely. We will later split the `code` variable in symbols and show them in our symbol boxes instead.

Since we want the on-screen keyboard to show up whenever user clicks the control, we add a `HitTestMode="LocalBounds"` to the whole control, and a `Clicked` trigger that gives focus to our hidden `TextInput`.

The label and symbol boxes will animate when our control gains and loses focus, so we introduce an `editMode` state in JavaScript. In UX, we data-bind the `enterEditMode` and `exitEditMode` functions to `Focus.Gained` and `Focus.Lost` properties of our `TextInput`, so that the functions are called when appropriate.

To implement the label animation, we make use of the `editMode` boolean `Observable`. Note how we specify `TransformOrigin="TopLeft"` to ensure that the label travels to the right place:
```
<Panel Height="24">
    <Text Value="{ReadProperty Label}" FontSize="20" Color="Highlight" Alignment="TopLeft" TransformOrigin="TopLeft">
        <Translation ux:Name="labelTrans" Y="1" RelativeTo="ParentSize" />
        <WhileTrue Value="{editMode}">
            <Change labelTrans.Y="0" Delay="0.2" Duration="0.42" Easing="ExponentialOut" EasingBack="ExponentialIn" />
            <Scale Factor="0.8" Delay="0.2" Duration="0.42" Easing="ExponentialOut" EasingBack="ExponentialIn" />
        </WhileTrue>
    </Text>
</Panel>
```

Before we go to our symbol boxes, let's split our `code` variable in pieces so that we have the separate symbols. We `.map()` on the `code` variable and return an array that holds the symbols present in `code`, plus empty strings up to the expected code length. Since we now have an `Observable` of an array, we use `.expand()` to get an `Observable` collection of all the items of the array.
```
var codeLength = 5;
var code = this.Code;
var splitCode = code.map(function(item) {
    var entered = item.split("");
    while (entered.length < codeLength) {
        entered.push("");
    }
    return entered;
}).expand();
```

Since we want to show the user which symbol is about to be entered, we need a way to mark a given symbol as selected. We achieve that by using another `.map()` on the new `splitCode` `Observable` list. For each item in the collection, we check if the index of the item matches the length of our `code` variable and set a boolean property depending on that. Ultimately, the variable `symbols` becomes a list of objects that is reactively updated whenever a character is entered in our hidden `TextInput`.
```
var symbols = splitCode.map(function(item, index) {
    var obj = {symbol: item, selected: false};
    if (index == code.value.length) {
        obj.selected = true;
    }
    return obj;
});
```


With the main control done, let's move on to the symbol boxes. They contain the bottom lines, and are responsible for resizing them as the control transitions to the edit mode. Let's take a look at our `SymbolBox`:
```
<Panel ux:Class="SymbolBox">
    <string ux:Property="Symbol" />

    <WhileTrue Value="{editMode}">
        <Change backgroundRect.Margin="8,0" Delay="0.2" DelayBack="0" />
        <Change backgroundRect.CornerRadius="1" Delay="0.2" DelayBack="0" />
        <Change label.Opacity="1" Delay="0.2" Duration="0.42" Easing="ExponentialOut" EasingBack="ExponentialIn" />
    </WhileTrue>

    <WhileTrue Value="{selected} && {editMode}">
        <Change backgroundRect.Color="Black" Delay="0.2" Duration="0.42" Easing="ExponentialOut" EasingBack="ExponentialIn" />
    </WhileTrue>

    <Text ux:Name="label" Alignment="Center" Value="{ReadProperty Symbol}" FontSize="32" Color="Highlight" Opacity="0" />
    <Rectangle ux:Name="backgroundRect" Color="Highlight" Height="2" Alignment="Bottom">
        <LayoutAnimation>
            <Move X="1" Y="1" RelativeTo="PositionChange" Duration="0.42" Easing="ExponentialOut" EasingBack="ExponentialIn" />
            <Resize X="1" Y="1" RelativeTo="SizeChange" Duration="0.42" Easing="ExponentialOut" EasingBack="ExponentialIn" />
        </LayoutAnimation>
    </Rectangle>
</Panel>
```

When we enter edit mode, we `Change` the `Margin` on the `backgroundRect` to create spacing between symbol boxes. In addition to that, we apply `CornerRadius` to make them smoother. This triggers the `LayoutAnimation` on the `backgroundRect` and it neatly resizes all of the lines. We also `Change` the `Opacity` on the label so that the entered symbols become visible while in edit mode.
To highlight the selected symbol, we add another `WhileTrue` trigger with a UX expression that combines both `editMode` and `selected` JavaScript boolean variables.

Using our new SegmentedCode component is now fairly straightforward. In our `App`, we create an `Observable` variable `code` that we pass to the component. Now all that remains is implementing a code submit function and we're good to go!
```
<App>
    <float4 ux:Global="Highlight" ux:Value="#999" />

    <JavaScript>
        var Observable = require("FuseJS/Observable");
        var code = Observable("");
        module.exports = {
            code: code
        };
    </JavaScript>

    <ClientPanel>
        <SegmentedInput Label="Enter Code" Code="{code}" Alignment="Top" Margin="16,64,16,0" />
    </ClientPanel>
</App>
```

Now go ahead, download the example, make your own segmented inputs and hook them up to actual authorization mechanisms!
