This example shows how we can create sticky headers that hover at the top of the screen as we scroll the content. Pictures for this example are taken from [unsplash.com](https://unsplash.com/). Design inspiration: [Read App Interface](https://dribbble.com/shots/3335104-07-Read-App-Interface) by Kun-Li.

The desired effect can be implemented with the use of the `x()` and `y()` UX expression functions. Since in this case we only need a vertical translation, we will use the `y()` function, which tells us the value of an elements' vertical position.

The main structure of our app is a `ScrollView` that holds a list of cards.

```
<ScrollView Background="#f3f4f6">
    <StackPanel ItemSpacing="8">
        <Each Items="{list}">
            <ListItem />
        </Each>
    </StackPanel>
</ScrollView>
```

The cards are defined as components using `ux:Class`. We have a separate `content` element, which holds the large image, and on top of it - our slightly transparent `header` `Panel` with author details. Below that, we have the footer section that shows the image title and some icons.
```
<StackPanel ux:Class="ListItem" Background="#fff">
    <StackPanel ux:Name="content">
        <Panel ux:Name="header" Height="68" Padding="24,0" Background="#fffffff8">
            <DockPanel Alignment="VerticalCenter">
                <Icon File="Assets/Icons/more.png" Size="24" Dock="Right" />
                <Circle Dock="Left" Width="40" Height="40">
                    <ImageFill File="{authorFile}" />
                </Circle>
                <StackPanel Padding="16,0">
                    <Text Value="{authorName}" Color="#8e979e" FontSize="16" Alignment="Left" />
                    <Text Value="{authorPlace}" Color="#8e979e" FontSize="12" Alignment="Left" />
                </StackPanel>
            </DockPanel>
        </Panel>

        <Image Height="256" File="{imageFile}" StretchMode="UniformToFill" />
    </StackPanel>

    <Panel Height="56" Padding="24,0">
        <DockPanel Alignment="VerticalCenter">
            <StackPanel Dock="Right" Orientation="Horizontal" ItemSpacing="16">
                <Icon File="Assets/Icons/favorite.png" Size="18" />
                <Icon File="Assets/Icons/chat.png" Size="18" />
            </StackPanel>
            <Text Value="{imageTitle}" Color="#8e979e" FontSize="20" Alignment="CenterLeft" ClipToBounds="true" Margin="0,0,16,0" />
        </DockPanel>
    </Panel>
</StackPanel>
```

To make the `header` items position react to our `ScrollView` being scrolled, we use a `ScrollingAnimation` trigger together with a `Move` animator.
Since we put the `ScrollingAnimation` triggers inside of our card class, the position values we get in our UX expression functions are relative to the particular instance of that class.

The first `ScrollingAnimation` trigger sticks the `header` to the top of the `ScrollView`. As we scroll, the `Move` animator moves the `header` vertically, relative to the size of the `content` area. Since the `ScrollingAnimation` trigger length matches the height of the `content` node, it makes the `header` appear like it's stuck to the top.

```
<ScrollingAnimation From="y(this)" To="y(this) + height(content)">
    <Move Target="header" Y="1" RelativeTo="Size" RelativeNode="content" />
</ScrollingAnimation>
```

If we do the math for the first `ListItem`, the `ScrollingAnimation` is starting from `0` and ending with `0 + (68 + 256) = 324`. For every other `ListItem`, the `y(this)` expression translates to the vertical offset of that item relative to the `StackPanel` that holds the list.

The second `ScrollingAnimation` trigger is responsible for moving the `header` out of the view when it hits the bottom of the card. As we scroll, the `Move` animator moves the `header` upwards relative to its size. Since in this case the `ScrollingAnimation` trigger length matches the height of the `header` element, it gets smoothly scrolled out of the view.

```
<ScrollingAnimation From="y(this) + height(content) - height(header)" To="y(this) + height(content)">
    <Move Target="header" Y="-1" RelativeTo="Size" />
</ScrollingAnimation>
```

The expression for the starting point takes into account the position of the card, adds the height of the content area and subtracts the height of the `header`. The expression for the end point only needs to take into account the position of the card and add the height of the content area.

Now we have a simple starting point for any sticky header situation. We can't wait to see what you will come up with!
