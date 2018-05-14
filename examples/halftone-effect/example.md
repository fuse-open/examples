Using the JavaScript Camera API, we create a small app that lets you take pictures with a halftone effect. The effect parameters can then be tweaked with some custom styled slider controls.

## Taking a picture

We create a function in JavaScript called `takePicture` which we bind to from UX.

<!-- snippet-begin:code/MainView.js:TakePicture -->

```
var picture = Observable('Icons/background.jpg');
var filterApplied = Observable(false);

function takePicture(){
    Camera.takePicture(1000, 1200).then(function(file){
        picture.value = file;
        filterApplied.value = false;
    }).catch(function(e) {
        console.log(e);
        filterApplied.value = false;
    });
};
```

<!-- snippet-end -->

It will open the default camera app on the device, and return the resulting file. We can then put that file in our `picture` `Observable` which we bind to from UX.
In addition to working with the picture, we also change the value of a boolean `Observable` called `filterApplied`, so that we can decide to show the picture with or without the halftone filter in UX.

## Halftone

<!-- snippet-begin:code/MainView.ux:HalftoneEffect -->

```
<Image ux:Name="picture" File="{picture}" StretchMode="UniformToFill">
    <WhileTrue Value="{filterApplied}">
        <Halftone ux:Name="halftone" Intensity="0" Smoothness="0" Spacing="5" PaperTint="0" DotTint="0" />
    </WhileTrue>
    <AddingAnimation>
        <Change picture.Opacity="0" Duration="0.5" />
    </AddingAnimation>
</Image>
```

<!-- snippet-end -->

We data-bind an `Image` element to the `picture` `Observable` and add a `Halftone` effect to it with some default values.
We give the effect a name, which we will reference from some sliders. Note how we only enable the filter when the `filterApplied` boolean `Observable` is true.

## Creating a custom slider

<!-- snippet-begin:code/MySlider.ux:CustomSlider -->

```
<RangeControl ux:Class="MySlider" Focus.IsFocusable="true" MinHeight="30" Padding="5" Margin="18,4" HitTestMode="LocalBounds">
    <string ux:Property="Text" />
    <float4 ux:Property="Highlight" />

    <LinearRangeBehavior />
    <StackLayout />

    <JavaScript>
        var Observable = require("FuseJS/Observable");
        var sliderValue = Observable(50);
        var sliderText = Observable(function() { return sliderValue.value.toFixed(2) })
        module.exports = {
            sliderValue: sliderValue,
            sliderText: sliderText
        };
    </JavaScript>

    <Panel Alignment="Top" Margin="6,10,0,0">
        <DataBinding Target="this.Value" Key="sliderValue" />
        <Text ux:Name="currentValue" Value="{sliderText}" Alignment="Right" Opacity="0.7" />
        <Panel ux:Name="thumb" Anchor="50%,50%" Alignment="Left" Width="15" Height="15" HitTestMode="LocalBounds">
            <Rectangle Color="{ReadProperty Highlight}" />
        </Panel>
        <Panel Layer="Background">
            <Rectangle Height="6" Color="{ReadProperty Highlight}" Opacity="0.3" />
            <Rectangle Height="6" Color="#fff" />
        </Panel>
    </Panel>
    <Text Value="{ReadProperty Text}" Alignment="BottomLeft" Margin="6,0,0,0" />

    <ProgressAnimation>
        <Move Target="thumb" X="1" RelativeTo="ParentSize" />
    </ProgressAnimation>

</RangeControl>
```

<!-- snippet-end -->

To create a custom slider, we use the `LinearRangeBehavior`. This behavior handles everything related to the input of a `RangeControl`.
We then use the `ProgressAnimation` trigger to animate the appearance of our `Slider` in response to the users input.


## Tweaking the effect with our sliders

We can tweak the halftone effect directly from UX using our newly created sliders. We use `ux:Property` bindings to customize the sliders color and text properties by passing them in from `MainView.ux`.

<!-- snippet-begin:code/MainView.ux:TweakingWithSliders -->

```
<WhileTrue Value="{filterApplied}">
    <Grid RowCount="3" Margin="0,30,0,10">
        <FadeInPanel>
            <MySlider Text="Intensity" Highlight="#50D2C2">
                <ProgressAnimation>
                    <Change halftone.Intensity="2" />
                </ProgressAnimation>
            </MySlider>
            <Rectangle Height="1" Color="#eee" Alignment="Bottom" />
        </FadeInPanel>
        <FadeInPanel>
            <MySlider Text="Smoothness" Highlight="#FCAB53">
                <ProgressAnimation>
                    <Change halftone.Smoothness="15" />
                </ProgressAnimation>
            </MySlider>
            <Rectangle Height="1" Color="#eee" Alignment="Bottom" />
        </FadeInPanel>
        <FadeInPanel>
            <MySlider Text="Spacing" Highlight="#BA77FD">
                <ProgressAnimation>
                    <Change halftone.Spacing="15" />
                </ProgressAnimation>
            </MySlider>
        </FadeInPanel>
    </Grid>
</WhileTrue>
```

<!-- snippet-end -->

We use the `ProgressAnimation` trigger together with `Change` animators to react to the user sliding.

Each `MySlider` is surrounded by a `FadeInPanel`. This is just a normal `Panel` with an `AddingAnimation` which animated the panels `Opacity`.

<!-- snippet-begin:code/MainView.ux:FadeInPanel -->

```
<Panel ux:Class="FadeInPanel" ux:Name="fadeInPanel">
    <AddingAnimation>
        <Change fadeInPanel.Opacity="0" Duration="0.5" />
    </AddingAnimation>
</Panel>
```

<!-- snippet-end -->
