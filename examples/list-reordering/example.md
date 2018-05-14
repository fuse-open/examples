In this example we show how to create a drag-to-reorder behaviour on a list of items. This was made possible by the introduction of `IdentityKey` property on `Each` tag, which takes care of figuring out which items are where after a list has been modified.


# Making a component

Since we want to have code that we can reuse in multiple places, we build a custom `ux:Class` component for our `SortableList` element. We will need to change the order of items that we pass in through `ux:Property`, so that list has to be an Observable. The type of the property needs to be `object`. Since we are going to make changes to the list inside of our custom component, we need to make a component-local viewmodel (in-line JavaScript in our component) that will hold the business logic and access the list of items.
```
<Panel ux:Class="SortableList">
    <object ux:Property="Items" />
    <string ux:Property="Label" />

    <JavaScript>
    var items = this.Items.inner();
    module.exports = {
        items: items
    };
    </JavaScript>
```
Am important detail to keep in mind when [passing Observables through properties](https://www.fusetools.com/docs/ux-markup/properties#passing-observables-through-properties) is that the property itself is a [derived Observable](https://www.fusetools.com/docs/fusejs/observable-api#state-observables-and-derived-observables), so we need to use `.inner()` to get access to the Observable list.

The visual part of our component is relatively simple: it's a `StackPanel` that stacks the label and list of items. What's interesting is the `Each` tag with `IdentityKey="id"` property set. When the items in the data-bound list change, `Each` will check the property `id` on every item in the list and figure out which items are the same.
```
<StackPanel ItemSpacing="8">
    <Text Value="{ReadProperty Label}" TextColor="#777" FontSize="18" Font="Bold" Margin="8,0" Alignment="CenterLeft" />
    <StackPanel ItemSpacing="1">
        <Each Items="{items}" IdentityKey="id">
            <Item />
        </Each>
    </StackPanel>
</StackPanel>
```

Each item in the list is represented by another `ux:Class`. It shows the title of the item and includes a handle that we will drag the items by.
```
<DockPanel ux:Class="Item" Height="56">
    <Text Value="{title}" Alignment="CenterLeft" TextColor="#444" Margin="8,0" />
    <Panel Width="56" Dock="Right" HitTestMode="LocalBounds">
        <StackPanel Width="16" ItemSpacing="2" Alignment="Center">
            <Each Count="4">
                <Rectangle Height="2" Color="#bbb" CornerRadius="1" />
            </Each>
        </StackPanel>
    </Panel>
    <Rectangle ux:Name="underlay" Color="#fff" Opacity="1" CornerRadius="2" Layer="Background" />
</DockPanel>
```

Our `MainView.ux` will hold 3 instances of the `SortableList` component, and we will put them inside of a `ScrollView` so that we can scroll to see items that are beyond the screen. This raises a challenge when we need to reorder the list, because tapping and then dragging the items will simply make the `ScrollView` scroll.

To solve this challenge, we need to pass in the parent `ScrollView` via `ux:Dependency` and disable `UserScroll` on it while we're reordering items.
```
<ScrollView ux:Dependency="parentScrollView" />
...
<WhileTrue Value="{reordering}">
    <Change parentScrollView.UserScroll="false" />
</WhileTrue>
```
We will set the variable `{reordering}` to `true` when any one of the drag handles are `Pressed` and set it back to `false` when the list item is `Released`.
```
<Pressed>
    <Callback Handler="{select}" />
</Pressed>
...
<Released>
    <Callback Handler="{deselect}" />
</Released>
```

Thanks to the `IdentityKey` on `Each`, an item in a list now gets its `LayoutAnimation` triggered when it changes its position in that list. We take advantage of that, and specify how that animation should look. In our example, an item can only be moved by one index at a time, so it makes sense to have a really simple and quick animation.
```
<LayoutAnimation>
    <Move RelativeTo="PositionChange" Vector="1" Duration="0.16" />
</LayoutAnimation>
```

To change the order of items within our list, we use a `WhileHovering` trigger. It works like this: while we have an item `Pressed` and we're moving the pointer (finger) over another list item, the `{hover}` callback fires. In there, we will remove the item we originally `Pressed` from the list and insert it at the new index.
```
<WhileHovering>
    <Callback Handler="{hover}" />
</WhileHovering>
```

This is how the UX code for our `Item` component looks like when we put all the bits together:
```
<DockPanel ux:Class="Item" Height="56">
    <Text Value="{title}" Alignment="CenterLeft" TextColor="#444" Margin="8,0" />
    <Panel Width="56" Dock="Right" HitTestMode="LocalBounds">
        <Pressed>
            <Callback Handler="{select}" />
        </Pressed>
        <StackPanel Width="16" ItemSpacing="2" Alignment="Center">
            <Each Count="4">
                <Rectangle Height="2" Color="#bbb" CornerRadius="1" />
            </Each>
        </StackPanel>
    </Panel>
    <LayoutAnimation>
        <Move RelativeTo="PositionChange" Vector="1" Duration="0.16" />
    </LayoutAnimation>
    <WhileHovering>
        <Callback Handler="{hover}" />
    </WhileHovering>
    <WhileTrue Value="{selected}">
        <Change underlay.Opacity="0.6" Duration="0.24" />
    </WhileTrue>
    <Rectangle ux:Name="underlay" Color="#fff" Opacity="1" CornerRadius="2" Layer="Background" />
</DockPanel>
```

Now we need to implement the business logic in JavaScript so that our callbacks actually do something. As described before, the list reordering logic resides in the `hover()` function, while the `select()` and `deselect()` functions toggle the `reordering` state and make sure the right data is in place when necessary.
```
<JavaScript>
var Observable = require("FuseJS/Observable");
var items = this.Items.inner();

var selected = null;
var reordering = Observable(false);

function select(args) {
    if (selected === null) {
        selected = args.data.id;
        items.forEach(function(x) {
            if (x.id === selected) {
                x.selected.value = true;
            }
        });
    }
    reordering.value = true;
}

function deselect() {
    selected = null;
    items.forEach(function(x) {
        x.selected.value = false;
    });
    reordering.value = false;
}

function hover(args) {
    if (reordering.value === true && selected !== null) {
        var from;
        var to;
        items.forEach(function(item, index) {
            if (item.id === selected) {
                from = index;
            }
            if (item.id === args.data.id) {
                to = index;
            }
        });
        if (to !== from && to !== undefined) {
            var tmp = items.toArray();
            var elem = tmp[from];
            tmp.splice(from, 1);
            tmp.splice(to, 0, elem);
            items.replaceAll(tmp);
        }
    }
}

module.exports = {
    items: items,
    reordering: reordering,
    select: select,
    deselect: deselect,
    hover: hover
};
</JavaScript>
```

# Using the component

With the `SortableList` component done, it's time we use it in our `MainView.ux`!

We start by creating 3 Observable lists of items in JavaScript:
```
<JavaScript>
var Observable = require("FuseJS/Observable");

function Item(id, title) {
    this.id = id;
    this.title = title;
    this.selected = Observable(false);
}

var morning = Observable(
    new Item(1, "Brush your teeth"),
    new Item(2, "Take out the trash"),
    new Item(3, "Take the stairs")
);
var day = Observable(
    new Item(1, "Buy milk"),
    new Item(2, "Make an app"),
    new Item(3, "Learn something new")
);
var evening = Observable(
    new Item(1, "Dinner with mom"),
    new Item(2, "Play chess with a friend"),
    new Item(3, "Watch TV")
);

module.exports = {
    morning: morning,
    day: day,
    evening: evening
};
</JavaScript>
```

Next, we add our `ScrollView` and assign it a `ux:Name`. Inside there, we stack 3 instances of our `SortableList` component, pass the `ux:Dependency` for `ScrollView` and data-bind the respective lists of items.
```
<ScrollView ux:Name="parentScrollView">
    <StackPanel Margin="8,16" ItemSpacing="24">
        <SortableList Items="{morning}" parentScrollView="parentScrollView" Label="Morning" />
        <SortableList Items="{day}" parentScrollView="parentScrollView" Label="Day" />
        <SortableList Items="{evening}" parentScrollView="parentScrollView" Label="Evening" />
    </StackPanel>
</ScrollView>
```
And now we can reorder lists by dragging items! Go ahead, download the example and make your own drag-to-reorder components!