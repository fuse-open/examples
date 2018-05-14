In this example we are using `Entering-` and `ExitingAnimation` together with `Keyframe` in order to implement this [awesome transition animation](https://dribbble.com/shots/2575761-Webshop-UI-UX-experiments) by [Remco Bakker](https://dribbble.com/remcobakker)

# The card class

Each page has a Card, which contains the images and their up and down movements. Each page also displays some text, but this part has its own `Entering-` and `ExitingAnimation`.


<!-- snippet-begin:code/MainView.ux:CardClass -->

```
<Panel ux:Class="Card" Width="150" Height="150" Opacity="0.4" ux:Name="self">
    <ActivatingAnimation>
        <Change self.Opacity="1" Duration="1"  />
    </ActivatingAnimation>
    <EnteringAnimation Scale="0.25">
        <Move RelativeTo="Size">
            <Keyframe Time="0.25" X="-0.7" Y="0.7"/>
            <Keyframe Time="0.5"  X="-1.4"  Y="0"/>
            <Keyframe Time="0.75" X="-2.1" Y="-0.7"/>
            <Keyframe Time="1"    X="-3.8"  Y="-1.4"/>
        </Move>
    </EnteringAnimation>
    <ExitingAnimation Scale="0.25">
        <Move RelativeTo="Size">
            <Keyframe Time="0.25" X="0.7" Y="0.7"/>
            <Keyframe Time="0.5"  X="1.4"  Y="0"/>
            <Keyframe Time="0.75" X="2.1" Y="-0.7"/>
            <Keyframe Time="1"    X="3.8"  Y="-1.4"/>
        </Move>
    </ExitingAnimation>

    <float4 ux:Property="PanelColor" />

    <Image File="{image}" Width="55%"/>
    <Panel Color="{ReadProperty self.PanelColor}" Margin="14" ClipToBounds="True">
        <Rotation Degrees="45" />
    </Panel>
</Panel>
```

<!-- snippet-end -->

Notice that we are using `Keyframe`s inside the `Move` animators in order to move our cards up and down over the course of the navigation.

We also set the `Scale` property of the `Entering-` and `ExitingAnimation` to "0.25". This allows us to show more cards on the screen at once, but also means we have to move them four times as far to get them off the screen.

# The rotated Grid

The tilted quadratic pattern is achieved using a large rotated `Grid`.

<!-- snippet-begin:code/MainView.ux:RotatedGrid -->

```
<Panel Alignment="Center" Width="0" Height="0" Offset="0,255">
    <Grid Columns="150,150,150,150,150,150" Rows="150,150,150,150,150,150" Alignment="Center" TransformOrigin="TopLeft">
```

<!-- snippet-end -->

The Grid has 6 equally sized columns and rows, which is enough to fit all the content properly aligned. We add a `<Rotation Degrees="45" />` transform in order to rotate the whole `Grid`. The reason we have put the `Grid` inside a `Panel` with `Width="0"` and `Height="0"` is to make sure the `Grid` gets properly aligned to the center even though it is rotated.

# The data

The data is very simple. Just a small JavaScript structure and some content:

<!-- snippet-begin:code/MainView.js:TheJS -->

```
function Item(image, category, name, text){
    this.category = category;
    this.name = name;
    this.text = text;
    this.image = image;
}


var items = Observable(
    new Item("Assets/food1.jpg","Fruit", "Lime","A lime (from Arabic and French lim) is a hybrid citrus fruit, which is typically round, lime green, 3-6 centimetres in diameter, and containing acidic juice vesicles. There are several species of citrus trees whose fruits are called limes, including the Key lime (Citrus aurantifolia), Persian lime, kaffir lime, and desert lime. Limes are an excellent source of vitamin C, and are often used to accent the flavours of foods and beverages. They are grown year-round. Plants with fruit called \"limes\" have diverse genetic origins; limes do not form a monophyletic group."),
```

<!-- snippet-end -->

To be able to data-bind directly to file names, we have to explicitly bundle the .jpg files in Assets folder:

```
  "Includes": [
    "*",
    "Assets/*.jpg:Bundle"
  ]
```

When the files are included in the bundle, we can use a simple syntax that refers to the image file path relative to project root:

<!-- snippet-begin:code/MainView.ux:ImageFile -->

```
<Image File="{image}" Width="55%"/>
```

<!-- snippet-end -->

The icons are from [Googles Material Icons pack](https://design.google.com/icons/), and the font is called [Lora](https://www.google.com/fonts/specimen/Lora) and is available under the CIL license from Google Fonts.


And that's it! Feel free to download the example and play around.
