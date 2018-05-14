This example demonstrates how we can create an endless scrolling list that removes elements as they scroll out of view.

When making large lists, we use `Each` and data-bind its `Items` property to an Observable list in JavaScript. When making _infinite_ lists, we have to avoid extensive memory use by employing two less known properties available on `Each`: `Limit` and `Offset`. By setting the `Limit` we limit the number of items being output by the `Each` at any given moment, and by changing the `Offset` we can move through our list of items forward and back.

	<Each Items="{list}" Offset="{offset}" Limit="{limit}">

We want to trigger the offset change at the top and bottom of the `ScrollView`, and we achieve this by using a pair of `Scrolled` triggers which activate when the user scrolls to a certain distance from either of the edges. We bind the `Handler` properties to JavaScript functions that manipulate an offset variable.

	<ScrollView LayoutMode="PreserveVisual">
		<Scrolled To="Start" Within="104" Handler="{decreaseOffset}" />
		<Scrolled ux:Name="atEnd" To="End" Within="104" Handler="{increaseOffset}" />
	</ScrollView>

The `Within` property sets how close to the start or end of the `ScrollView` the user has to be for the trigger to be activated. Depending on the complexity of each item and the amount of items we add, we might want to adjust the `Within` property to trigger earlier (or later) to give the app more time to create all the elements.

It is important that we set the `limit` variable high enough so that the items take up a big enough space in the `ScrollView` to avoid unnecessary `Scrolled` triggering as we change the `offset`. We must also change the `offset` by a number that is _just right_: low enough so that the list can be pre-populated quickly, and high enough to avoid loading new items too frequently.

	var limit = 20;
	var offset = Observable(0);

	function increaseOffset() {
		changeOffset(5);
	}

	function decreaseOffset() {
		changeOffset(-5);
	}

	function changeOffset(diff) {
		var nextOffset = offset.value + diff;
		if (nextOffset >= 0) {
			offset.value = nextOffset;
			if ((list.length - nextOffset) <= limit) {
				loadItems().then(function() {
					atEnd.check();
				});
			}
		}
	}

If our users scroll really fast on a slow internet connection, they can hit the bottom of our `ScrollView` while we're still busy appending items to the list. To work around that, we wrap the loading of new items in a Promise-function and as seen in the `changeOffset` function above, we check if we're at the end of our `ScrollView` when it's done. In this example, we use a simple list of image IDs from [unsplash.com](https://unsplash.com/).

	var images = [
		"eWFdaPRFjwE","_FjegPI89aU","_RBcxo9AU-U","PQvRco_SnpI","6hxvm0NzYP8",
		"b-yEdfrvQ50","pHANr-CpbYM","45FJgZMXCK8","9wociMvaquU","tI_Odb7ZU6M",
		"o0RZkkL072U","N-sxA8vEGDk","324ovGl1BwM","RSOxw9X-suY","qv5yb436qRI",
		"iWFRUyqpCbQ","ZJ4yhJFIzaY","ze0tqwj86S4","gQR4STZ24kM","xMYnPo53ukE"
	];

	function loadItems() {
		var p = new Promise(function(resolve) {
			var items = images.map(function(i) {
				return {image: "https://source.unsplash.com/" + i + "/416x208"};
			});
			list.addAll(items);
			resolve();
		});
		return p;
	}


When it comes to displaying items, we data-bind the `Limit` and `Offset` properties on the `Each` to the JavaScript variables we created. This allows us to limit the number of items output by the `Each` and move through the list as we scroll up and down.

	<Each Items="{list}" Offset="{offset}" Limit="{limit}">
		<ListItem />
	</Each>


The `ListItem` component uses `Deferred` tag to prevent lag-spikes due to addition of new items by allowing Fuse to spread the work over multiple frames. Coupled with an `AddingAnimation`, this results in a subtle fade-in transition as the images arrive. Note that we set a `MemoryPolicy="UnloadUnused"` on the `ImageFill` tag to make sure memory is freed from the images that are off-screen.

	<Panel ux:Class="ListItem" Height="104">
		<Deferred>
			<Rectangle ux:Name="imageHolder" CornerRadius="2" Opacity="1">
				<AddingAnimation>
					<Change imageHolder.Opacity="0" Delay="0.16" Duration="0.32" />
				</AddingAnimation>
				<ImageFill Url="{image}" StretchMode="UniformToFill" WrapMode="ClampToEdge" MemoryPolicy="UnloadUnused" />
			</Rectangle>
		</Deferred>
		<Rectangle CornerRadius="2" Color="#0003" />
	</Panel>

That's it! Feel free to download the source code and play around.
