This example shows how you can make your own transitions for pages within a `PageControl`.

The trick is setting `Transition="None"` on the `PageControl`, which removes the default page animations.
This lets you add your own `EnteringAnimation` and `ExitingAnimation` to each page, which controls how they animate in and out.

```
<App Background="#222">
	<Page ux:Class="MyPage">
		<EnteringAnimation>
			<Move X="-1" RelativeTo="ParentSize" />
		</EnteringAnimation>
		<ExitingAnimation>
			<Scale Factor="0.75" />
			<Change this.Opacity="0.7" />
		</ExitingAnimation>
	</Page>
	
	<PageControl Transition="None">
		<NavigationMotion Overflow="Elastic" />
		<MyPage Color="#D32F2F" />
		<MyPage Color="#8E24AA" />
		<MyPage Color="#3F51B5" />
	</PageControl>
</App>
```
