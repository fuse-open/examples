This example uses `Router` and `Navigator` to create a list of pages that can be navigated to with corresponding buttons.

We create a list of 20 pages by calling the `createPage` function inside a for-loop and adding each page to an `Observable`.
Each page has a `title` string and a `clicked` function.
When `clicked` is called on a page, it pushes a new instance of `subPage` onto the router's navigation stack, and sends the `title` of the page as a parameter.

<!-- snippet-begin:pages-using-js/MainView.js:MainJS -->

```
var Observable = require("FuseJS/Observable");

function createPage(title) {
    return {
        title: title,
        clicked: function() {
            router.push("subPage", { title: title })
        }
    };
}

var pages = Observable();

for (var i = 1; i <= 20; i++) {
    pages.add(createPage("PAGE " + i));
}

module.exports = { pages: pages };
```

<!-- snippet-end -->

Inside `mainPage`, we iterate over the `pages` Observable using an `Each`, creating a button for each page using a `Panel`. We bind the `Clicked` property of our button panel to the `clicked` function we defined above.

<!-- snippet-begin:pages-using-js/MainView.ux:MainPage -->

```
<Navigator DefaultPath="mainPage">
    <Page ux:Template="mainPage">
        <ScrollView>
            <StackPanel ItemSpacing="7" Margin="7">
                <Each Items="{pages}">
                    <Panel Clicked="{clicked}" Height="50">
                        <Rectangle Layer="Background" CornerRadius="3" Color="#FF8362" />
                        <Text Value="{title}" Alignment="Center" Color="#fff" FontSize="15" />
                    </Panel>
                </Each>
            </StackPanel>
        </ScrollView>
        
        <AlternateRoot ParentNode="navBar">
            <NavBarTitle>PAGES</NavBarTitle>
        </AlternateRoot>
    </Page>
```

<!-- snippet-end -->

Next up: `SubPage`. We map over the navigation parameter passed through `push()`, and export the title as an `Observable`. This is then data bound to a `Text` control inside the page.

<!-- snippet-begin:pages-using-js/SubPage.ux:SubPage -->

```
<Page ux:Class="SubPage">
    <JavaScript>
        exports.title = this.Parameter.map(function(param) {
            return param.title;
        });
    </JavaScript>
    
    <Text Value="{title}" FontSize="50" TextColor="#3C4663" Alignment="Center"/>
    
```

<!-- snippet-end -->

To make the navigation bar at the top change title depending on the currently active page, we introduce a new `NavBarTitle` component. This is simply a `Text` with an `EnteringAnimation` and an `ExitingAnimation` to fade it in/out when its parent page is navigated to and from.

<!-- snippet-begin:pages-using-js/NavBarTitle.ux:NavBarTitle -->

```
<Text ux:Class="NavBarTitle" HitTestMode="None" Alignment="Center" FontSize="25" TextColor="#FFF">
    <EnteringAnimation>
        <Move X="50" Duration="0.35" Easing="CubicInOut" />
        <Change this.Opacity="0" Duration="0.25" Easing="CubicIn" />
    </EnteringAnimation>
    <ExitingAnimation>
        <Move X="-50" Duration="0.35" Easing="CubicInOut" />
        <Change this.Opacity="0" Duration="0.25" Easing="CubicIn" />
    </ExitingAnimation>
</Text>
```

<!-- snippet-end -->

To make this appear inside the navigation bar, we use `AlternateRoot` and declare the navigation bar as a dependency of our `SubPage` component.

<!-- snippet-begin:pages-using-js/SubPage.ux:NavBarTitle -->

```
    <Visual ux:Dependency="navBar" />
    <Panel>
        <AlternateRoot ParentNode="navBar">
            <NavBarTitle Value="{title}" />
        </AlternateRoot>
    </Panel>
</Page>
```

<!-- snippet-end -->

Now, let's tie this together.
We instantiate `SubPage` as a *template* for our `Navigator` with the key `subPage`, as well as satisfying its dependency on the `navBar`.

Here is our final `MainView.ux`:

<!-- snippet-begin:pages-using-js/MainView.ux:MainView -->

```
<App Background="#eee">
    <Router ux:Name="router" />

    <DockPanel>
        <JavaScript File="MainView.js"/>
        <StackPanel Dock="Top" Background="#3CB5D0">
            <StatusBarBackground/>
            <Fuse.iOS.StatusBarConfig Style="Light"/>
            <Panel ux:Name="navBar" Dock="Top" Height="50">
                <WhileCanGoBack>
                    <Panel ux:Name="backButton" Width="90" Height="50" Alignment="Left" 
                           Padding="20,0,0,0" HitTestMode="LocalBounds">
                        <DockPanel>
                            <Image File="arrow-left-white.png" Height="20" Color="#fff" Dock="Left"/>
                            <Text Alignment="Center" Margin="5,0,0,0" FontSize="18" Color="#fff" Dock="Right">
                                BACK
                            </Text>
                        </DockPanel>
                        <AddingAnimation>
                            <Change backButton.Opacity="0" Duration=".3" />
                        </AddingAnimation>
                        <RemovingAnimation>
                            <Change backButton.Opacity="0" Duration=".3" />
                        </RemovingAnimation>
                        <Clicked>
                            <GoBack />
                        </Clicked>
                    </Panel>
                </WhileCanGoBack>
            </Panel>
        </StackPanel>
        
        <BottomBarBackground Dock="Bottom" />
        
        <Navigator DefaultPath="mainPage">
            <Page ux:Template="mainPage">
                <ScrollView>
                    <StackPanel ItemSpacing="7" Margin="7">
                        <Each Items="{pages}">
                            <Panel Clicked="{clicked}" Height="50">
                                <Rectangle Layer="Background" CornerRadius="3" Color="#FF8362" />
                                <Text Value="{title}" Alignment="Center" Color="#fff" FontSize="15" />
                            </Panel>
                        </Each>
                    </StackPanel>
                </ScrollView>
                
                <AlternateRoot ParentNode="navBar">
                    <NavBarTitle>PAGES</NavBarTitle>
                </AlternateRoot>
            </Page>

            <SubPage ux:Template="subPage" navBar="navBar" />
        </Navigator>
    </DockPanel>
</App>
```

<!-- snippet-end -->
