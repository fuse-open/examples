In this example we use the `Timeline` to make an animated introduction screen inspired by [Stephen Pearsons](https://dribbble.com/scr33nag3r) [animation](https://dribbble.com/shots/2372039-Tour-Animation).

## Creating our resources

We start of by defining our resources. In particular, the icons.

### Defining a base Icon class

Since we have several similar looking icons, we define a base class which contains a stroked circle and places the icon image in the middle.

<!-- snippet-begin:code/MainView.ux:baseicon -->

```
<Panel ux:Class="Icon" Width="80" Height="80">
    <Image Source="{Resource iconFile}" Width="60%" Height="60%"/>
    <Circle Color="#6334A4">
        <Stroke Brush="#fff" Width="3"/>
    </Circle>
</Panel>
```

<!-- snippet-end -->

### Deriving from Icon

We can then derive from this base class to create our icons. The last icon needs some animation of its inner elements, so we define it inline instead of creating a class.

<!-- snippet-begin:code/MainView.ux:DerivingIcon -->

```
<Icon ux:Class="Payment">
    <FileImageSource File="Icons/payment.png" ux:Key="iconFile"/>
</Icon>
<Icon ux:Class="Notes">
    <FileImageSource File="Icons/notes.png" ux:Key="iconFile"/>
</Icon>
<Icon ux:Class="Camera">
    <FileImageSource File="Icons/camera.png" ux:Key="iconFile"/>
</Icon>
```

<!-- snippet-end -->

### The custom send icon

We need to animate the send icons image element, and so we define it inline instead of creating a class. We add a larger copy of the image with and set its `Color` property to the same color as the background. The `Color` property of `Image` is multiplied by the image sources color. It's placed underneath the icon image so that scaling it beyond the circle gives the image a border.

<!-- snippet-begin:code/MainView.ux:SendIcon -->

```
<Panel ux:Name="Send" Width="80" Height="80" Opacity="0">
    <Panel ux:Name="sendImage">
        <Image File="Icons/send.png" Width="60%" Height="60%"/>
        <Image File="Icons/send.png" Width="90%" Height="90%" Color="#6334A4"/>
        <Scaling ux:Name="sendScaling" Factor="0.7"/>
        <Translation X="5" />
        <Rotation Degrees="-45" />
    </Panel>
    <Circle Color="#6334A4">
        <Stroke Brush="#fff" Width="3"/>
    </Circle>
</Panel>
```

<!-- snippet-end -->

## Animating with Timeline

We use a `Timeline` to define all our animation. The `Timeline` can be played by changing its `TargetProgress` property from 0 to 1. We do this when the `Page` becomes active using the [`WhileActive`](https://www.fusetools.com/learn/fuse#whileactive) trigger.

<!-- snippet-begin:code/MainView.ux:Timeline -->

```
<Timeline ux:Name="timeline">
    <Change camera.Opacity="1"     Duration="1" />
    <Change text1.Opacity="1"      Duration="1" />
    <Move Target="camera" Y="-2"   Duration="1"   Delay="1" Easing="BackOut" RelativeTo="Size" />
    <Change paymentTrans.X="-30"   Duration="1"   Delay="1" Easing="BackOut"/>
    <Change notesTrans.X="30"      Duration="1"   Delay="1" Easing="BackOut"/>
    <Change payment.Opacity="1"    Duration="1"   Delay="1" Easing="BackOut"/>
    <Change notes.Opacity="1"      Duration="1"   Delay="1" Easing="BackOut"/>
    <Change paymentRot.Degrees="0" Duration="1"   Delay="1" Easing="BackOut"/>
    <Change notesRot.Degrees="0"   Duration="1"   Delay="1" Easing="BackOut"/>
    <Change text2.Opacity="1"      Duration="1"   Delay="1"/>

    <Change arrow1.Opacity="1"     Duration="0.6"   Delay="1.5"/>
    <Change text3.Opacity="1"      Duration="1"   Delay="2"/>

    <Move Target="panel2" Y="-0.25" Duration="1"   Delay="3" Easing="BackOut" RelativeTo="Size"/>
    <Change Send.Opacity="1"       Duration="1"   Delay="3"/>
    <Change text3.Opacity="1"      Duration="1"   Delay="3"/>

    <Change arrow2.Opacity="1"     Duration="0.6"   Delay="3.5"/>
    <Move Target="sendImage"       Duration="0.8" Delay="3.5" Easing="BackOut" X="25"/>
    <Change sendScaling.Factor="1" Duration="0.8" Delay="3.5" Easing="BackOut" />

    <Change text4.Opacity="1"      Duration="1" Delay="4.7" />


</Timeline>
```

<!-- snippet-end -->

Since we do use some absolute values in our animation, we surround the entire thing in a [`Viewbox`](https://www.fusetools.com/learn/fuse#viewbox). This lets us scale the content to different screen sizes without layout making our absolute values invalid.

That's it!
