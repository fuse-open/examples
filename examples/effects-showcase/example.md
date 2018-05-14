# Effects

This example shows how you can apply various real-time visual effects to elements in your app.

<!-- snippet-begin:code/MainView.ux:App -->

```
<App>
	<Text ux:Class="Title" Color="#fff" FontSize="30" Margin="10" Alignment="TopLeft" HitTestMode="None">
		<Shadow Size="10" Distance="1" Color="#000" Mode="PerPixel" />
	</Text>
	<Image ux:Class="MyImage" StretchMode="UniformToFill" />
	
	<Grid RowCount="4" DefaultRow="1*">
		
		<Panel ClipToBounds="true">
			<Panel>
				<StackPanel>
					<StatusBarBackground />
					<Title>BLUR</Title>
				</StackPanel>
				<MyImage File="assets/blur.png" Margin="-20" />
				<Blur ux:Name="blur" Radius="0" />
			</Panel>
			
			<WhilePressed>
				<Change blur.Radius="10" Duration="0.3" />
			</WhilePressed>
		</Panel>
		<Panel>
			<MyImage File="assets/halftone.png">
				<Title>HALFTONE</Title>
				<Halftone ux:Name="halftone" Smoothness="1" DotTint="1" PaperTint="1" Spacing="5" Intensity="0.2" />
			</MyImage>
			
			<WhilePressed>
				<Change halftone.DotTint="0.1" Duration="0.3" />
				<Change halftone.PaperTint=".5" Duration="0.3" />
				<Change halftone.Intensity="0.6" Duration="0.3" />
			</WhilePressed>
		</Panel>
		<Panel>
			<Title>DESATURATE</Title>
			
			<MyImage File="assets/desaturate.png">
				<Desaturate ux:Name="desaturate" Amount="0" />
				
				<WhilePressed>
					<Change desaturate.Amount="1" Duration="0.3" Easing="QuadraticInOut" />
				</WhilePressed>
			</MyImage>
		</Panel>
		<Panel>
			<Text Value="MASK" Alignment="Center" FontSize="130" Color="#fff">
				<Mask File="assets/mask.png" Mode="Grayscale" />
			</Text>
			<MyImage ux:Name="maskBackground" File="assets/mask_bg.png" />
			
			<WhilePressed>
				<Change maskBackground.Color="#111" Duration="0.3" Easing="QuadraticInOut" />
			</WhilePressed>
		</Panel>
	</Grid>
</App>
```

<!-- snippet-end -->
