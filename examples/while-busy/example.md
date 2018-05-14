This example shows how we can use the `WhileBusy` trigger to display meaningful visual feedback while a large image is loading. We use the `WhileBusy` trigger on our thumbnail images to display a gray rectangle until the thumbnails arrive. We use the same technique to animate between low and high resolution images to get quicker feedback while loading.

```
<App>
	<JavaScript>
	var Images = require("images");

	// on app startup, select the first image to initiate the high quality image loading
	Images.selectImage({data:{id:0}});

	module.exports = {
		thumbs: Images.thumbs,
		selectImage: Images.selectImage,
		highQualityImage: Images.highQualityImage,
		lowQualityImage: Images.lowQualityImage
	};
	</JavaScript>

	<ClientPanel>

		<ScrollView Dock="Top" AllowedScrollDirections="Horizontal">
			<StackPanel Padding="5" ItemSpacing="5" Orientation="Horizontal">
				<Each Items="{thumbs}">
					<Image Url="{low}" Width="50" Height="50" Clicked="{selectImage}" StretchMode="UniformToFill" MemoryPolicy="UnloadUnused">
						<WhileBusy>
							<Rectangle Color="#EEE">
								<Stroke Width="1" Color="#CCC" />
							</Rectangle>
						</WhileBusy>
						<WhileFalse Value="{selected}">
							<Desaturate Amount="1" />
						</WhileFalse>
					</Image>
				</Each>
			</StackPanel>
		</ScrollView>

		<Panel Dock="Fill" Margin="5,0,5,5" ClipToBounds="true">
			<WhileBusy>
				<Image ux:Name="lowQualityImage" Url="{lowQualityImage}" StretchMode="UniformToFill" MemoryPolicy="UnloadUnused">
					<Desaturate Amount="1" />
					<RemovingAnimation>
						<Change lowQualityImage.Opacity="0" Duration="0.35"/>
					</RemovingAnimation>
				</Image>
			</WhileBusy>
			<Image Url="{highQualityImage}" StretchMode="UniformToFill" MemoryPolicy="UnloadUnused" />
		</Panel>

	</ClientPanel>
</App>
```
