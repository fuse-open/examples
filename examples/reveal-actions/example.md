In this example we implement a cool reveal animation from the [Stripe Dashboard](http://medium.stfi.re/swlh/exploring-the-product-design-of-the-stripe-dashboard-for-iphone-e54e14f3d87e?ref=hackingui&sf=lroyzg#.bgjivpv68)

# The animated menu

We define a class called `MenuButton`. This allows for a nicer split up of code, but in this case we do not make it a reusable component.

The menu icon consists of three circles. We want to animate their size and position so that they morph into one larger circle when clicked. We use the `Scale` animator for this. One thing to notice is that we define the circles at their maximum size, and scale them down using the `Scaling` transform. This ensures that we do not get aliasing when using the `Scale` animator to increase their size.

<!-- snippet:reveal-actions:MenuButton.ux:CirclesButton -->

We trigger a `WhileTrue` in our `Clicked`-trigger, and perform almost all the animation there. The menu icon moves from the right to the left. This is done by changing its `Alignment` property, and then using a `LayoutAnimation` to make it move smoothly.

<!-- snippet:reveal-actions:MenuButton.ux:LayoutAnimation -->

In our `WhileTrue` we also animate the `Scaling` and `Opacity` of the menu items and the thin divider rectangle.

<!-- snippet-begin:code/MenuButton.ux:MenuAnimation -->

```
<WhileTrue ux:Name="menuLayout" >
    <Change sp.Alignment="Left" DurationBack="0"/>
    <Change circleScale1.Factor="1" Duration="0.3" DelayBack="0"/>
    <Change circleScale2.Factor="1" Duration="0.3" DelayBack="0.025"/>
    <Change circleScale3.Factor="1" Duration="0.3" DelayBack="0.05"/>
    <Move Target="c1" X="0.5" RelativeTo="Size" Duration=".3" DelayBack="0"/>
    <Move Target="c3" X="-0.5" RelativeTo="Size" Duration=".3" DelayBack="0"/>

    <Change menuButtons.Opacity="1" Duration="0.4" DurationBack=".2" />

    <Change Easing="BackOut" shareScaling.Factor="1"  Duration="0.5" DelayBack="0.1" DurationBack="0.1"/>
    <Change Easing="BackOut" refundScaling.Factor="1" Duration="0.5" Delay="0.1" DelayBack="0" DurationBack="0.1"/>
    <Change arrow.Opacity="1"       Duration="0.4"             DurationBack="0.15" DelayBack="0"/>
    <Change dividerRect.Opacity="1" Duration="0.2" Delay="0.2" DurationBack="0.15" DelayBack="0"/>
</WhileTrue>
```

<!-- snippet-end -->


# The list items

Notice that there are two types of list items. One with green icons and only one row, and one with blue icons and two rows, as well as an arrow icon on the right.

We define two classes do represent the two types:

<!-- snippet-begin:code/MainView.ux:ListItems -->

```
<Grid ux:Class="Entry" Columns="40, 60, 1*" Padding="10,10">
    <Image Source="{Resource icon}" Width="25" Alignment="CenterLeft" Color="#89C372"/>
    <Text Value="{Resource name}" FontSize="15" Color="#777" Alignment="CenterLeft" />
    <Text Value="{Resource text}" FontSize="15" Alignment="CenterLeft"/>
</Grid>

<Grid ux:Class="ButtonEntry" Columns="40, 1*, 40" Padding="10,10">
    <Image Source="{Resource icon}" Width="25" Alignment="CenterLeft" Color="#018BDB"/>
    <StackPanel>
        <Text Value="{Resource name}" Alignment="CenterLeft" />
        <Text Value="{Resource text}" Color="#777" FontSize="13" Alignment="CenterLeft"/>
    </StackPanel>
    <Image File="Icons/arrow.png" Width="20" Color="#018bdb"/>
</Grid>
```

<!-- snippet-end -->

We use resource-bindings for the icon, name and text values of the items, so that we can set these values per item as we instantiate them:

<!-- snippet-begin:code/MainView.ux:ListItemsInstances -->

```
    <StackPanel Background="#eee" ClipToBounds="True" Dock="Top">
        <Divider />
        <Entry>
            <string ux:Key="name" ux:Value="Date"/>
            <string ux:Key="text" ux:Value="Apr 2, 2015, 9:41 AM"/>
            <FileImageSource  ux:Key="icon" File="Icons/date.png"/>
        </Entry>
        <Divider />
        <Entry>
            <string ux:Key="name" ux:Value="Fee"/>
            <string ux:Key="text" ux:Value="$0.31"/>
            <FileImageSource  ux:Key="icon" File="Icons/credit_card.png"/>
        </Entry>
        <Divider />
        <Entry>
            <string ux:Key="name" ux:Value="Status"/>
            <string ux:Key="text" ux:Value="Paid"/>
            <FileImageSource  ux:Key="icon" File="Icons/info.png"/>
        </Entry>
        <Divider />
        <Entry>
            <string ux:Key="name" ux:Value="Desc."/>
            <string ux:Key="text" ux:Value="Recurrent Pro Plan"/>
            <FileImageSource  ux:Key="icon" File="Icons/menu.png"/>
        </Entry>
    </StackPanel>
    <StackPanel>
        <Rectangle Color="#FDFDFD" CornerRadius="8" Layer="Background"/>
        <Divider />
        <ButtonEntry>
            <string ux:Key="name" ux:Value="Card"/>
            <string ux:Key="text" ux:Value="Visa **** 4242"/>
            <FileImageSource  ux:Key="icon" File="Icons/credit_card.png"/>
        </ButtonEntry>
        <Divider />
        <ButtonEntry>
            <string ux:Key="name" ux:Value="Customer"/>
            <string ux:Key="text" ux:Value="example@example.com"/>
            <FileImageSource  ux:Key="icon" File="Icons/account.png"/>
        </ButtonEntry>
    </StackPanel>
</DockPanel>
```

<!-- snippet-end -->

That's it. Feel free to download the project.
