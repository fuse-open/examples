This example shows another use of `ScrollingAnimation` to make a collapsing and expanding top section for a scrolling list. The example is inspired by [this cool pice](https://dribbble.com/shots/2367675-Ticket-scroll) by [Cam Macbeth](https://dribbble.com/highfelt). Icons are taken from Google's excellent [Material Icons pack](https://design.google.com/icons/).

## Defining our colors

The color palette is defined in its own class called `ColorPalette` and is instantiated in the top of our `MainView.ux`.

<!-- snippet-begin:code/ColorPalette.ux:ColorPalette -->

```
<Panel ux:Class="ColorPalette">
    <float4 ux:Global="LightGray"       ux:Value="#FDFDFD"   />
    <float4 ux:Global="LightGrayFaded"  ux:Value="#FDFDFD88" />
    <float4 ux:Global="TicketPurple"    ux:Value="#3D206A"   />
    <float4 ux:Global="FlagColor"       ux:Value="#973067"   />
    <float4 ux:Global="MediumGray"      ux:Value="#A5A3A8"   />
    <float4 ux:Global="FadedMediumGray" ux:Value="#A5A3A877" />
    <float4 ux:Global="TicketRed"       ux:Value="#CC2421"   />
    <float4 ux:Global="IndicatorColor"  ux:Value="#A34379"   />
    <SolidColor ux:Global="FlagColorBrush" Color="FlagColor" />
</Panel>
```

<!-- snippet-end -->

We can then use these colors by their names instead of inputting the hex codes everywhere.



## The `ScrollingAnimation`

The purple part containing the content which should "wrap" away as the user scrolls is defined in its own layer by having them in their own `Grid`. This `Grid` is then put underneath the actual `ScrollView` which is offset by the size of the `Grid` in its Y-direction.

This make sure that we always hit the `ScrollView`, but for the top most part we control the animation ourselves.

<!-- snippet-begin:code/MainView.ux:WrappingPart -->

```
<Text ux:Class="T" Color="LightGray" Alignment="CenterLeft"/>
<Grid ux:Name="ticketInfo" Rows="150,80,130" Alignment="Top" >
    <Panel ux:Name="trip" >
        <HorizontalStripedLine ux:Name="stripedLine" NSegments="25" Padding="25,0" Alignment="Bottom"/>
        <Grid RowCount="4" Columns="60,1*" Color="TicketPurple" Padding="20">
            <Image File="Icons/arrow.png" Alignment="CenterLeft" Width="30">
                <Rotation Degrees="180" />
            </Image> <T Value="Fri 15 May 2015"  FontSize="22"/>
            <T Value="Dep." Color="LightGrayFaded" FontSize="12"/> <T Value="11:07 Manchester Piccadilly" />
            <T Value="Arr." Color="LightGrayFaded" FontSize="12"/> <T Value="14:35 London King`s Cross" />
            <Panel /> <T FontSize="14" Color="LightGray" Value="Duration: 3hr 28m Changes: 2" />
        </Grid>
    </Panel>

    <Panel ux:Name="ticket" Color="TicketPurple">

        <Grid Columns="40,1*" Padding="15" ux:Name="ticketContent">
            <Flag Height="15" Width="20" Alignment="TopLeft"/>
            <StackPanel>
                <DockPanel>
                    <Text Value="ABX456789" Color="LightGray"/>
                    <Text Value="&#163; 87.60" Color="LightGray" Dock="Right"/>
                </DockPanel>
                <Text Value="Your ticket is ready for collection." Color="MediumGray" FontSize="12" Margin="0,5,0,0"/>
                <Text Value="Find out how" Color="MediumGray" FontSize="12"/>
            </StackPanel>
        </Grid>
    </Panel>
    <Panel ux:Name="info" Color="LightGray">
        <Panel ux:Name="infoContent">
            <Panel Alignment="TopRight" Margin="15">
                <Text Value="1st" Color="LightGray" Alignment="Center" FontSize="13"/>
                <Rectangle Color="TicketPurple" CornerRadius="10" Height="20" Width="40"/>
            </Panel>
            <StackPanel Padding="20">
                <Text Value="Anytime Return" FontSize="15"/>

                <Text Value="2 Adults 1 Child" FontSize="14" Margin="0,10,0,0"/>
                <Text Value="1 Senior Railcard 1 Annual Gold Card" FontSize="14"/>

                <Text Value="Valid only on your chosen service. Not refundable." Color="MediumGray" FontSize="12" Margin="0,10,0,0"/>
                <Text Value="Changeable prior to date of travel for a fee."      Color="MediumGray" FontSize="12"/>
            </StackPanel>
        </Panel>
    </Panel>
</Grid>
```

<!-- snippet-end -->

The `ScrollView` contains several `ScrollingAnimation` animators, which are used to animate the collapsing items. There are three main main ranges; (0,150), (150,240) and (220,370). They roughly correspond to the height of the elements they animate.

<!-- snippet-begin:code/MainView.ux:ScrollView -->

```
<ScrollView>
    <StackPanel>
        <Panel Height="360" />
        <StackPanel>
            <StackPanel Color="White" >
                <Each Count="5">
                    <Trip />
                </Each>
            </StackPanel>

            <ScrollingAnimation From="0" To="150">
                <Move Target="info" Y="-1" RelativeTo="Size"/>
            </ScrollingAnimation>
            <ScrollingAnimation From="0" To="130">
                <Move Target="infoContent" Y="0.5" RelativeTo="Size" />
                <Change Target="infoContent.Opacity" Value="0" Easing="QuadraticInOut"/>
            </ScrollingAnimation>

            <ScrollingAnimation From="150" To="240">
                <Move Target="ticket" Y="-1" RelativeTo="Size"/>
            </ScrollingAnimation>
            <ScrollingAnimation From="150" To="220">
                <Move Target="ticketContent" Y="0.4" RelativeTo="ParentSize" />
                <Move Target="stripedLine" Y="-0.05" RelativeTo="ParentSize" RelativeNode="ticket"/>
                <Change ticketContent.Opacity="0" Easing="QuadraticInOut"/>
                <Change stripedLine.Opacity="0" Easing="QuadraticInOut"/>
            </ScrollingAnimation>

            <ScrollingAnimation From="220" To="370">
                <Move Target="trip" Y="-150"/>
            </ScrollingAnimation>
        </StackPanel>
    </StackPanel>

</ScrollView>
```

<!-- snippet-end -->

## The striped lines

Striped strokes are soon coming to Fuse, but until then, we have to be a little clever. For this limited use-case we can get away with creating a couple of classes.

<!-- snippet-begin:code/StripedLine.ux:StripedLines -->

```
<Panel ux:Class="StripedLine" StripeColor="FadedMediumGray">
        <float4 ux:Property="StripeColor" />
        <int ux:Property="NSegments" />
    </Panel>

    <StripedLine ux:Class="VerticalStripedLine">
        <Grid RowCount="{ReadProperty this.NSegments}">
            <Each Count="{ReadProperty this.NSegments}">
                <Rectangle Color="{ReadProperty this.StripeColor}" Margin="1" Width="1"/>
            </Each>
        </Grid>
    </StripedLine>

    <StripedLine ux:Class="HorizontalStripedLine">
        <Grid ColumnCount="{ReadProperty this.NSegments}">
            <Each Count="{ReadProperty this.NSegments}">
                <Rectangle Color="{ReadProperty this.StripeColor}" Margin="1" Height="1"/>
            </Each>
        </Grid>
    </StripedLine>
```

<!-- snippet-end -->

These can then be used like so:

```
<HorizontalStripedLine NSegments="25" />
<VerticalStripedLine NSegments="10" />
```

The rest of the UX is pretty much just standard Fuse layout and animation, so download the source and play around - have fun!
