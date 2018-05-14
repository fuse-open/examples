A tiny example of how to download a JSON file over HTTP, parse it and populate the user interface based on the contents.

<!-- snippet-begin:code/MainView.ux:AppUX -->

```
<App Background="#eee">
    <DockPanel>
        <StatusBarBackground Dock="Top" />
        <BottomBarBackground Dock="Bottom" />
        <ScrollView>
            <Grid ColumnCount="2">
                <JavaScript>
                    var Observable = require("FuseJS/Observable");

                    var data = Observable();

                    fetch('https://gist.githubusercontent.com/petterroea/5ed146454706990ea8386f147d592eff/raw/b157cfed331da3cb88150051ab74aa131022fef8/colors.json')
                        .then(function(response) { return response.json(); })
                        .then(function(responseObject) { data.replaceAll(responseObject); });

                    module.exports = {
                        data: data
                    };
                </JavaScript>
                <Each Items="{data}">
                    <DockPanel Height="120" Margin="10">
                        <Panel DockPanel.Dock="Top" Margin="10" Height="30">
                            <Rectangle Layer="Background" CornerRadius="10" Color="#fff"/>
                            <Text Value="{colorName}" TextAlignment="Center" Alignment="Center" />
                        </Panel>

                        <Rectangle Layer="Background" CornerRadius="10" Color="{hexValue}"/>
                    </DockPanel>
                </Each>
            </Grid>
        </ScrollView>
    </DockPanel>
</App>
```

<!-- snippet-end -->

The UX is pretty straight forward, we have an `Each` bound to `{data}` which represents the color array in the JSON file. For each entry in the file it creates a panel with color and text fetched from the JSON data:

The JavaScript simply fetches the JSON file and exposes it through the `Observable` data variable which is exposed to the UX code.

The JSON file looks like this:
```
[
    {
        "colorName":"Red",
        "hexValue":"#F44336"
    },
    {
        "colorName":"Pink",
        "hexValue":"#E91E63"
    },
    {
        "colorName":"Purple",
        "hexValue":"#9C27B0"
    },
    {
        "colorName":"Deep purple",
        "hexValue":"#673AB7"
    },
    {
        "colorName":"Indigo",
        "hexValue":"#3F51B5"
    },
    {
        "colorName":"Blue",
        "hexValue":"#2196F3"
    },
    {
        "colorName":"Light blue",
        "hexValue":"#03A9F4"
    },
    {
        "colorName":"Cyan",
        "hexValue":"#00BCD4"
    }
]
```
