This example app shows how to construct advanced layouts.

## The markup

<!-- snippet-begin:code/MainView.ux:AppUX -->

```
<App>
	<Image ux:Class="Icon" Density="2" StretchMode="PointPrecise" />
	<Font File="Assets/OpenSans-SemiBold.ttf" ux:Global="SemiBold" />

	<Grid ColumnCount="1" Rows="4.5*,100,4*">
		<Fuse.iOS.StatusBarConfig Style="Light" />

		<Text ux:Class="DefaultText" Color="#fff" Font="SemiBold" />

		<Grid Row="1" RowCount="1" Columns="Auto,Auto,1*" Color="#00000088">
			<DefaultText Margin="20,0,0,20" Alignment="Bottom" FontSize="70">73°</DefaultText>
			<Icon Margin="15,0,15,25" Alignment="Bottom" File="Assets/PartlyCloudyIconWhite.png" />
			<WrapPanel Margin="0,0,0,20" Alignment="Bottom">
				<DefaultText Value="SUNDAY, "/>
				<DefaultText Value="MARCH "/>
				<DefaultText Value="23" />
			</WrapPanel>
		</Grid>
		<Image Row="0" RowSpan="2" File="Assets/PaloAlto.png" StretchMode="UniformToFill" />
		<DockPanel Row="2" Color="#f3f3f3">
			<Grid ColumnCount="6" RowCount="1" Dock="Top" Height="80">
				<DefaultText ux:Class="StatText" Color="#1c1c1c" FontSize="14" Alignment="CenterLeft" />
				<Icon ux:Class="StatIcon" Margin="6" Alignment="CenterRight" />

				<Rectangle Alignment="Top" Height="1" Color="#999b9b" Layer="Background" />

				<StatIcon File="Assets/FlagIcon.png" />
				<StatText>4 MPH</StatText>
				<StatIcon File="Assets/CompassIcon.png" />
				<StatText>SOUTH</StatText>
				<StatIcon File="Assets/UmbrellaIcon.png" />
				<StatText>23%</StatText>
			</Grid>
			<Grid RowCount="4" ColumnCount="5">
				<Rectangle Alignment="Top" Height="1" Color="#999b9b" Layer="Background" />
				<DefaultText ux:Class="Weekday" Color="#939393" Alignment="Center" FontSize="12" />
				<DefaultText ux:Class="Temperature" Color="#333333" Alignment="Center" FontSize="20" Margin="6,0,0,0" />
				<Weekday>MON</Weekday>
				<Weekday>TUE</Weekday>
				<Weekday>WED</Weekday>
				<Weekday>THU</Weekday>
				<Weekday>FRI</Weekday>
				<Icon File="Assets/CloudsIcon.png" />
				<Icon File="Assets/SunnyIcon.png" />
				<Icon File="Assets/PartlyCloudyIcon.png" />
				<Icon File="Assets/PartlyCloudyIcon.png" />
				<Icon File="Assets/SunnyIcon.png" />
				<Temperature>83°</Temperature>
				<Temperature>85°</Temperature>
				<Temperature>81°</Temperature>
				<Temperature>82°</Temperature>
				<Temperature>86°</Temperature>
			</Grid>
		</DockPanel>
	</Grid>
</App>
```

<!-- snippet-end -->
