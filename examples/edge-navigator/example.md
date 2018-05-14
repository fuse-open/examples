This is a simple example of how to use the `EdgeNavigator` to let the user swipe content in from the edges of a panel (typically the edges of the device). You can also check out how `EdgeNavigator` can be used to create a side menu in a more complete [social media screen examle](social-media-screen.md).

## Basic case

Here we simply add a red rectangle to the left side of the application. Swiping in from the left will reveal it:

The code for this is minimal:

<!-- snippet-begin:code/MyApp.ux:EdgeNav -->

```
<App>
    <EdgeNavigator HitTestMode="LocalBoundsAndChildren">
        <!-- Add a panel handled by the EdgeNavigator -->
        <Panel Width="150" EdgeNavigation.Edge="Left" Background="#f63" />

        <!-- The main content -->
        <Panel>
            <Text Alignment="Center">
                This is an example of EdgeNavigator!
            </Text>
        </Panel>
    </EdgeNavigator>
</App>
```

<!-- snippet-end -->

The `HitTestMode` of the `EdgeNavigator` needs to be set to `LocalBoundsAndChildren` in this case, as the contents of the `EdgeNavigator` are transparent, so they will by default not report any events to the `EdgeNavigator`.

Under normal circumstances, the main area of your app will be populated with controls that do hit testing, so you don't need to worry about this.

To see how `EdgeNavigator` is used in a more involved example you can take a look at the [social media screen examle](social-media-screen.md).
