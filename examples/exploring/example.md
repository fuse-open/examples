In this example we implement [this](https://dribbble.com/shots/2311820-Exploring) really cool list item to detail view animation by [Dejan Markovic](https://dribbble.com/DejanMarkovic)

In this article we focus on the high level concepts used in creating the transition from list item to details view.

# The transition

The transition from list item to details view is achieved by predefining the details view with placeholder panels. Each part of this placeholder layout is passed in to each of the list items using ux:Dependency. When an item is selected, it is then animated to the correct placeholder panel using the `RelativeTo="PositionOffset"` of `Move`. We also use `Resize` since the items has to change slightly in size to fit the new layout. The `PositionOffset` option of `RelativeTo` means that the item will move in the direction of the item specified by `RelativeNode`.

Here is an example of those animators:

```
<Move   Easing="CubicInOut" Target="img" Vector="1" RelativeTo="PositionOffset" RelativeNode="detailsImagePanel" Duration="0.6" />
<Resize Easing="CubicInOut" Target="img" Vector="1" RelativeTo="Size" RelativeNode="detailsImagePanel" Duration="0.6" />
```

# Exclusive selection

To make sure only one item can be selected at once (in case the user clicked on two at the same time), we make all the items in the list be a part of a `Selection` with max selection count of 1:

```
<Selection MinCount="0" MaxCount="1" Value="{Write current}"/>
```

We give each item a `Selectable` with its `Value` set to a unique `id` coming from JavaScript. We can then use a `WhileSelected` trigger to flip the switch on the `WhileTrue` which animates the item to its details view layout.

Feel free to download the full source code and dig around.
