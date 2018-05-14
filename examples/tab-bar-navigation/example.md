This example shows how to implement tab bar navigation using `PageControl`, `PageIndicator` and `GridLayout`.

## The markup

<!-- snippet-begin:code/TabBarNavigation.ux:UXApp -->

```
<App Background="#333">
    <JavaScript>
        var pages = [
            {"name":"page1", "highlight":"#34495e", "icon":"Assets/icon-hexagon.png"},
            {"name":"page2", "highlight":"#3498db", "icon":"Assets/icon-star.png"},
            {"name":"page3", "highlight":"#aa3377", "icon":"Assets/icon-square.png"},
            {"name":"page4", "highlight":"#88cc22", "icon":"Assets/icon-triangle.png"}
        ];
        module.exports = {
            pages: pages,
            pageCount: pages.length
        };
    </JavaScript>

    <Page ux:Class="MyPage">
        <ResourceFloat4 Key="Highlight" Value="{highlight}" />
        <FileImageSource ux:Key="Icon" File="{icon}" />
        <Image File="Assets/fuse-logo.png" StretchDirection="DownOnly" />
        <Rectangle Color="{highlight}" />
    </Page>


    <DockPanel>
        <StatusBarBackground Dock="Top"/>
        <BottomBarBackground Dock="Bottom"/>

        <PageControl ux:Name="pages">
            <Each Items="{pages}">
                <MyPage />
            </Each>
        </PageControl>

        <PageIndicator Dock="Bottom" Height="45" Navigation="pages">
            <GridLayout ColumnCount="{pageCount}" />
            <Panel ux:Template="Dot" Height="45">
                <ActivatingAnimation>
                    <Scale Target="icon" Factor="1.5" />
                </ActivatingAnimation>
                <Clicked>
                    <NavigateTo Target="{Page Visual}"/>
                </Clicked>
                <Panel ux:Name="icon" Padding="10">
                    <Image Source="{Page Icon}" />
                </Panel>
                <Rectangle Color="{Page Highlight}" />
            </Panel>
        </PageIndicator>

    </DockPanel>
</App>
```

<!-- snippet-end -->
