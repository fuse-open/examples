This time we've implemented an awesome [interactive infographic concept](https://dribbble.com/shots/3266096--16-Dashboard-Navigation) by [Yaroslav Zubko](https://twitter.com/ZubkoYaroslav).

## The pie chart

This example displays a pie chart where each piece has a corresponding page in a `PageControl`. The chart rotates with the navigation in the `PageControl`, but since each piece has a different size, each page must affect the rotation relative to its size (and as it turns out, the size of the previous piece). For this reason, we need to do use `JavaScript` to create a view-model from the raw values.

The original data looks like this:

```
var items = Observable([
	{ value: 25, color: "#4CD8FC" },
	{ value: 15, color: "#A943C1" },
	{ value: 30, color: "#FFCE6B" },
	{ value: 10, color: "#EB4CAF" },
	{ value: 20, color: "#33CB9F" }
]);
```

For convenience, we've made them all add up to 100 (%). We could take another step back and make finding the percentage a part of our calculations, but we felt it was an unnecessary reason to convolute the example further.

## Creating a view-model

From these data, we do a mapping to compute some additional values that's needed by our view:

- Start and end angles for our pie pieces.
- How much should each page rotate the chart.
- Which piece currently corresponds to the active page.

```
items.map(function(i){
	var lastItem = {
		startAngle: 0,
		endAngle: 0,
		angle: 0
	};
	i.forEach(function(x, c) {
		x.index = c;
		x.angle = ((x.value / 100) * 360);
		if (c === 0) {
			defaultRotation.value = x.angle / 2 + 90;
		}
		if (c > 0) {
			lastItem.wheelRotate = (x.angle / 2) + (lastItem.angle / 2);
		}

		x.startAngle = lastItem.startAngle - x.angle;
		x.endAngle = lastItem.startAngle;

		x.isActive = Observable(function(){
			return currentPage.value == x.index;
		});

		lastItem = x;

	});
	return i;
}).expand()
```

The rotation of the pie chart is done using an `EnteringAnimation` inside each page in the `PageControl`:

```
<EnteringAnimation>
	<Rotate Target="wheel" Degrees="{wheelRotate}" />
</EnteringAnimation>
```

This works well because as we navigate to the right, each `EnteringAnimation` will stack on top of each other, rotating the chart smoothly from one page to the next. Because of this fact however, we need information about both the previous page and the current to figure out how much we need to rotate the chart:

```
lastItem.wheelRotate = (x.angle / 2) + (lastItem.angle / 2);
```

This is why we make our original data be an `Observable` of an array. This way we can use knowledge about the whole data-set as we map, and then use `Observable.expand` in the end, to turn it into a proper observable list.

## Scrolling

The rest of the example is fairly straight forward. It uses a few `ScrollingAnimation` triggers to hide and show various parts of the UI. Not how the pie chart fades into becoming a more traditional set of page indicators. This effect is achieved simply by having two separate elements. One fades in while the other fades out by animating their `Opacity` property.

```
<ScrollingAnimation From="height(wheelPanel) - 30" To="height(wheelPanel)">
	<Change pageIndicator.Opacity="1" />
	<Change topBarBackground.Opacity="1" />
	<Change coverDot.Opacity="0" />
	<Change flatPageIndicator.Opacity="1" />
</ScrollingAnimation>
```

That's it for this example. Feel free to download and play with the source code.
