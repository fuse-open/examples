We found [this excellent concept](https://dribbble.com/shots/3202189-Fitness-App-Concept) by [Tanya Brassie](https://dribbble.com/tanyabrassie) of [inCahoots](https://www.instagram.com/incahootsdesign), and decided to implement the design in Fuse.

## Slicing and dicing

This example is quite heavy on image assets, and each image is sliced from the original vector art in 1x, 2x, and 4x densities.
This will result in a lot of clutter throughout our code, since we need a `MultiDensityImageSource` with three instances of `FileImageSource` for each image.

To clean this up, all images are defined separately in `StaticResources.ux`, along with fonts and text styles.

<!-- snippet-begin:code/StaticResources.ux:Images -->

```
<Image ux:Class="RunningWomanRightArm" StretchMode="PixelPrecise">
    <MultiDensityImageSource>
        <FileImageSource File="Assets/running-woman/arm-right@1x.png" Density="1" />
        <FileImageSource File="Assets/running-woman/arm-right@2x.png" Density="2" />
        <FileImageSource File="Assets/running-woman/arm-right@4x.png" Density="4" />
    </MultiDensityImageSource>
</Image>

... and so on ...
```

<!-- snippet-end -->

## Noise

The example has a couple of elements that include a subtle noise texture.
For this, we've created a `NoiseRectangle` component, which displays a repeating noise image overlaying a color.
The `NoiseIntensity` property controls the opacity of the noise.

<!-- snippet-begin:code/Components/NoiseRectangle.ux:NoiseRectangle -->

```
<Rectangle ux:Class="NoiseRectangle" Color="#fff" NoiseIntensity="0.12">
    <float ux:Property="NoiseIntensity" />

    <ImageFill File="../Assets/noise.png"
               StretchMode="PointPrecise"
               Density="2"
               ContentAlignment="TopLeft"
               Opacity="{ReadProperty NoiseIntensity}" />
</Rectangle>
```

<!-- snippet-end -->

## Running woman

The illustration of the running woman is split up into three parts – the body, the right arm, and the left arm.
Each arm has an `Anchor`, which determines its point of origin. We've set this to be approximately the point where the arm joins the torso.
Because we also set `TransformOrigin="Anchor"`, all transformations will revolve around this point.

When the page is *inactive*, we slightly `Rotate` each arm clockwise.
We then use a `WhileActive` trigger to revert this rotation when the containing page becomes active.
The bouncy effect is achieved by using the `ElasticOut` easing function.


<!-- snippet-begin:code/Components/RunningWoman.ux:RunningWoman -->

```
<Panel ux:Class="RunningWoman">
    <RunningWomanRightArm Alignment="Center" TransformOrigin="Anchor" Anchor="81%,11%" Offset="30,-45">
        <Rotation ux:Name="rightArmRotation" Degrees="90" />
    </RunningWomanRightArm>
    
    <RunningWomanBody />

    <RunningWomanLeftArm Alignment="Center" TransformOrigin="Anchor" Anchor="10%,54%" Offset="50,-45">
        <Rotation ux:Name="leftArmRotation" Degrees="40" />
    </RunningWomanLeftArm>
    
    <WhileActive Bypass="Never">
        <Change leftArmRotation.Degrees="0" Duration="1.8" Delay="0.3" DurationBack="0.3" Easing="ElasticOut" />
        <Change rightArmRotation.Degrees="0"  Duration="2" Delay="0.2" DurationBack="0.3" Easing="ElasticOut" />
    </WhileActive>
</Panel>
```

<!-- snippet-end -->

And that's pretty much it! As always, we encourage you to download and play around with the code.