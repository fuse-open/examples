This example shows how to use `AddingAnimation` and `RemovingAnimation` to animate items being added and removed in a list. `LayoutAnimation` is used to adjust the position of the other list items inside the `StackPanel`.

## The markup

<!-- snippet-begin:code/AnimatedList.ux:App -->

```
<App Background="#eee">
    <DockPanel>
        <JavaScript>
            var Observable = require("FuseJS/Observable");
            var items = Observable();

            function addItem() {
                items.add({ text: "This is an item" });
            }

            function removeItem(sender) {
                items.remove(sender.data);
            }

            module.exports = {
                items: items,
                addItem: addItem,
                removeItem: removeItem
            };
        </JavaScript>

        <StatusBarBackground Dock="Top" />
        <Basic.Button Text="Add item" Clicked="{addItem}" Dock="Top" />
        <ScrollView>
            <StackPanel>
                <Each Items="{items}">
                    <DockPanel Padding="10" Margin="0,1" Background="#fff">
                        <Text Value="{text}" Alignment="CenterLeft" />
                        <Basic.Button Text="Delete" Clicked="{removeItem}" Dock="Right" />

                        <LayoutAnimation>
                            <Move RelativeTo="LayoutChange" Y="1" Duration="0.8" Easing="ElasticIn" />
                        </LayoutAnimation>

                        <AddingAnimation>
                            <Move RelativeTo="Size" X="1" Duration="0.3" Easing="CircularIn" />
                        </AddingAnimation>

                        <RemovingAnimation>
                            <Move RelativeTo="Size" X="-1" Duration="0.4" Easing="CircularOut" />
                        </RemovingAnimation>
                    </DockPanel>
                </Each>
            </StackPanel>
        </ScrollView>
    </DockPanel>
</App>
```

<!-- snippet-end -->
