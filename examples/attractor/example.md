It is easy to quickly set up interpolation between values using an `Attractor`. Here is a simple example:

Here an `Attractor` is set up to interpolate between the raw `Translation` and the desired target value of the `Rectangle`.

The code is simply:

<!-- snippet-begin:code/MainView.ux:AppUX -->

```
<App>
    <DockPanel>
        <StatusBarBackground Dock="Top" />
        <Button ux:Class="BottomButton" Height="75" />
        <StackPanel Dock="Bottom">
            <Panel>
                <BottomButton Text="Center" Alignment="Center">
                    <Clicked>
                        <Set greenPosition.Value="0,0,0" />
                    </Clicked>
                </BottomButton>
                <Grid ColumnCount="2" >
                    <BottomButton Text="TopLeft">
                        <Clicked>
                            <Set greenPosition.Value="-100,-100,0" />
                        </Clicked>
                    </BottomButton>
                    <BottomButton Text="TopRight">
                        <Clicked>
                            <Set greenPosition.Value="100,-100,0" />
                        </Clicked>
                    </BottomButton>
                    <BottomButton Text="BottomLeft">
                        <Clicked>
                            <Set greenPosition.Value="-100,100,0" />
                        </Clicked>
                    </BottomButton>
                    <BottomButton Text="BottomRight">
                        <Clicked>
                            <Set greenPosition.Value="100,100,0" />
                        </Clicked>
                    </BottomButton>
                </Grid>
            </Panel>
            <BottomFrameBackground />
        </StackPanel>

        <Panel>
            <Rectangle CornerRadius="5" Width="25" Height="25" Color="#3f4">
                <Translation ux:Name="rawPosition"/>
                <Attractor Target="rawPosition.Vector" ux:Name="greenPosition" />
            </Rectangle>
        </Panel>
    </DockPanel>
</App>
```

<!-- snippet-end -->

## A fancier example using Delay

Let's create a more elaborate example by adding more items and delaying some of them:

[VIDEO media/preview-delayed.mp4]

This effect is achieved by simply adding more rectangles, more instances of `Attractor` and putting a `Delay` on the triggers, like this:

<!-- snippet-begin:code/MainView.ux:MoreRectangles -->

```
<BottomButton Text="TopLeft">
    <Clicked>
        <Set GreenPosition1.Value="-100,-100,0" />
        <Set GreenPosition2.Value="-100,-100,0" Delay="0.05" />
        <Set GreenPosition3.Value="-100,-100,0" Delay="0.1" />
        <Set GreenPosition4.Value="-100,-100,0" Delay="0.15" />
        <Set GreenPosition5.Value="-100,-100,0" Delay="0.2" />
    </Clicked>
</BottomButton>
```

<!-- snippet-end -->
