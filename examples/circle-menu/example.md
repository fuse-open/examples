Here is another awesome design we found by [Anton Aheichanka](https://dribbble.com/madebyanton) that we felt obligated to implement with Fuse. We used the free UI kit from [InVision](http://www.invisionapp.com/do) for the assets.

## The animations

This example implements two separate animations:

1. Clicking the pink plus-button makes it expand, revealing more options.
2. Clicking the avatar in the top right corner triggers the calendar page to animate.

In this example we handle them separately.

## Defining the resources

We start by defining some resources. We are using several icons, so we define a common base class for them:

<!-- snippet-begin:code/Resources.ux:IconClass -->

```
<Image ux:Class="Icon" />
```

<!-- snippet-end -->

The only reason we subclass here is to have a semantic type called "Icon". This makes it easier for us to perform styling later on.

We then create classes for the other icons:

<!-- snippet-begin:code/Resources.ux:TheOtherIcons -->

```
<Icon ux:Class="ClockIcon" File="Assets/icon_clock.png" />
<Icon ux:Class="TalkIcon" File="Assets/icon_talk.png" />
<Icon ux:Class="LocationIcon" File="Assets/icon_location.png" />
<Icon ux:Class="SunnyIcon" File="Assets/icon_sunny.png" />
<Icon ux:Class="SmallAvatarIcon" File="Assets/small_avatar.png" />
<Icon ux:Class="HamburgerIcon" File="Assets/icon_hamburger.png" />
```

<!-- snippet-end -->

We also want to apply a common style for our text, so we define a couple of text classes:

<!-- snippet-begin:code/Resources.ux:TextClasses -->

```
<Text ux:Class="DefaultText" Color="#fffd" />
<Text ux:Class="MutedText" Color="#fff8" />
```

<!-- snippet-end -->

We include the Resources.ux file in our MainView.ux with the `<ux:Include />` tag.


### The calendar item

Each item in the calendar can contain 1 to 3 items. We define just one class and let it handle all the cases by just hiding the unused fields using `WhileTrue`-triggers.

<!-- snippet-begin:code/Resources.ux:CalendarItem -->

```
<Grid ux:Class="CalendarItem" Columns="1*,4*">
    <AddingAnimation>
        <Move X="1" Duration="0.4" RelativeTo="Size" Easing="CircularInOut" DelayBack="{delay}" />
        <Move Target="underline" X="1" Duration="0.4" RelativeTo="Size" Easing="CircularInOut" DelayBack="{lineDelay}" />
    </AddingAnimation>
    <RemovingAnimation>
        <Move X="1" Duration="0.4" RelativeTo="Size" Easing="CircularInOut" DelayBack="{delay}" />
        <Move Target="underline" X="1" Duration="0.4" RelativeTo="Size" Easing="CircularInOut" DelayBack="{lineDelay}" />
    </RemovingAnimation>
    <StackPanel Alignment="TopLeft" Width="40" Margin="0,15,0,0">
        <DefaultText FontSize="25" Value="{item.time}" TextAlignment="Center" />
        <MutedText FontSize="10" Value="{item.ampm}" TextAlignment="Center"/>
    </StackPanel>
    <Panel Margin="0,18">

        <Panel>
            <StackPanel Alignment="VerticalCenter">
                <WhileTrue Value="{hasTitle}">
                    <DefaultText Value="{item.title}" />
                </WhileTrue>
                <WhileTrue Value="{hasProject}">
                    <MutedText Value="{item.project}" FontSize="14" Margin="0,2" />
                </WhileTrue>
                <WhileTrue Value="{hasPeople}">
                    <WrapPanel Margin="0,8,0,0">
                        <Each Items="{item.people}">
                            <Image File="{picture}" Width="30" Height="30" Margin="5,0" />
                        </Each>
                    </WrapPanel>
                </WhileTrue>
            </StackPanel>
        </Panel>
    </Panel>
    <WhileFalse Value="{isLast}">
        <Rectangle ux:Name="underline" Height="1" Color="#fff3" Alignment="Bottom" Column="1" />
    </WhileFalse>
</Grid>
```

<!-- snippet-end -->

We define a data-model for the item in JavaScript, and use it to populate each item with data:

<!-- snippet-begin:code/MainView.js:CalendarItemData -->

```
function Person(picture){
    this.picture = picture;
}

function Item(time, ampm, title, project, people){
    this.time = time;
    this.ampm = ampm;
    this.title = title;
    this.project = project;
    this.people = people;
}

var items = Observable(
    new Item(8, "AM", "Finish Home Screen", "Use Macaw", Observable()),
    new Item(11, "AM", "Lunch Break", "", Observable()),
    new Item(2, "PM", "Finish prototype", "InVision", [new Person("Assets/avatarcalendar3.png"), new Person("Assets/avatarcalendar2.png"), new Person("Assets/avatarcalendar.png")])
);
```

<!-- snippet-end -->

We then map over the `items` variable in order to assign some more view-related fields, like animation delays and boolean values for hiding unused fields as discussed above. Notice that since we are mapping with a function which takes two arguments, the second argument is the index of the item. This allows us to have different delays based on the position in the `Observable`.

<!-- snippet-begin:code/MainView.js:MapWithIndex -->

```
var itemsView = items.map(function(item, index){
    return {
        item : item, delay : index * 0.08,
        lineDelay : (items.length - index + 1) * 0.1,
        isLast : index === items.length - 1,
        hasTitle : item.title.length > 0,
        hasProject : item.project.length > 0,
        hasPeople : item.people.length > 0
    };
});
```

<!-- snippet-end -->

### The plus-button

The plus-button is defined in MainView.ux as a class called `WobbleButton`. It has two circles. The first one has a pink `Color`. The second one has its `Opacity` set to 0 and a slightly darker `Color`. When the button is clicked, we scale the first one and animate the second ones `Opacity` to 1.

<!-- snippet-begin:code/MainView.ux:TheTwoCircles -->

```
<Circle ux:Name="overlayCircle" Color="#0004" Opacity="0" Width="20%" Height="20%" Alignment="Center" >
    <Clicked>
        <Toggle Target="expanded" />
    </Clicked>
</Circle>
<Circle ux:Name="circle" Color="#F23363" Width="20%" Height="20%" >

</Circle>
```

<!-- snippet-end -->

There are three icons which are revealed when the button is scaled. Their rest-state is defined to be in a circular arrangement in the top left area of the button. By default, they have an `Opacity` of 0, and a `Translation`.

When the button is expanded, the translations on all the icons are animated back to 0, and the opacity of each one is also animated to 1.

<!-- snippet-begin:code/MainView.ux:TheButtonIcons -->

```
<ClockIcon ux:Name="icon1" Margin="40,30,0,20" Width="30" Height="30" Alignment="Left" Opacity="0" IsEnabled="false">
    <Translation ux:Name="trans1" X="1.5" RelativeTo="Size" />
    <Clicked>
        <Toggle Target="expanded" />
    </Clicked>
</ClockIcon>
<TalkIcon ux:Name="icon2" Margin="70,70,0,0" Width="30" Height="30" Alignment="TopLeft" Opacity="0" IsEnabled="false">
    <Translation ux:Name="trans2" X="1" Y="1" RelativeTo="Size" />
    <Clicked>
        <Toggle Target="expanded" />
    </Clicked>
</TalkIcon>
<LocationIcon ux:Name="icon3" Margin="30,40,20,0" Width="30" Height="30" Alignment="Top" Opacity="0" IsEnabled="false">
    <Translation ux:Name="trans3" Y="1.5" RelativeTo="Size" />
    <Clicked>
        <Toggle Target="expanded" />
    </Clicked>
</LocationIcon>
```

<!-- snippet-end -->

<!-- snippet-begin:code/MainView.ux:AnimatingTheButton -->

```
<WhileTrue ux:Name="expanded">
    <Change circle.Height="100" Easing="CubicInOut" Duration="0.2" DelayBack="0"/>
    <Change circle.Width="100" Easing="CubicInOut" Duration="0.2"  DelayBack="0"/>

    <Change enableOptions.Value="true" Delay="0.1" />
    <Change overlayCircle.Opacity="1" Delay="0.1" Duration="0.1" />
    <Rotate Target="cross" Degrees="-45" Duration="0.2" Easing="CubicInOut" />

    <Change trans1.X="0" Delay="0" Duration="0.4" Easing="BounceOut" EasingBack="QuadraticInOut" DurationBack="0.2"    DelayBack="0"/>
    <Change trans2.X="0" Delay="0.05" Duration="0.4" Easing="BounceOut" EasingBack="QuadraticInOut" DurationBack="0.2" DelayBack="0"/>
    <Change trans2.Y="0" Delay="0.05" Duration="0.4" Easing="BounceOut" EasingBack="QuadraticInOut" DurationBack="0.2" DelayBack="0"/>
    <Change trans3.Y="0" Delay="0.1" Duration="0.4" Easing="BounceOut" EasingBack="QuadraticInOut" DurationBack="0.2"  DelayBack="0"/>

    <Change icon1.Opacity="1" Delay="0.1" Duration="0.1" Easing="QuadraticInOut" DelayBack="0"/>
    <Change icon2.Opacity="1" Delay="0.1" Duration="0.1" Easing="QuadraticInOut" DelayBack="0"/>
    <Change icon3.Opacity="1" Delay="0.1" Duration="0.1" Easing="QuadraticInOut" DelayBack="0"/>
</WhileTrue>
```

<!-- snippet-end -->
