In this example we have implemented [an awesome menu design](https://dribbble.com/shots/2389505-Cards-Menu-Concept) by [Gal Shir](https://dribbble.com/galshir)!

## The cards

Each card is defined as follows. A translational offset is calculated in JavaScript based on the cards index. Name, background color and text color are also data-bound values.

<!-- snippet-begin:code/MainView.ux:TheCards -->

```
<Panel Offset="{offset}" ux:Name="unselected">
    <Panel ux:Name="self" LayoutMaster="unselected">
        <Grid ux:Name="hamburger" RowCount="3" Height="30" Width="30" Alignment="TopLeft" Margin="20" Opacity="0">
            <Each Count="3">
                <Rectangle Height="4" Color="{textColor}" CornerRadius="2"/>
            </Each>
        </Grid>

        <Text ux:Name="text" Value="{name}" Margin="25" Color="{textColor}" Font="RobotoBold" FontSize="14"/>

        <Rectangle ux:Name="backgroundRect" CornerRadius="10" Layer="Background">
            <SolidColor ux:Name="backgroundRectCol" Color="{color}"/>
        </Rectangle>

        <WhileTrue Value="{isSelected}">
            <Move Target="text" X="40" Duration="0.3" Easing="QuadraticInOut"/>
            <Change hamburger.Opacity="1" Duration="0.3" Easing="QuadraticInOut" />
        </WhileTrue>

        <Clicked>
            <Callback Handler="{selectMe}" />
            <Change backgroundRectCol.Color="#fff" Duration="0.0" DurationBack="0.28" />
        </Clicked>

        <LayoutAnimation>
            <Move RelativeTo="PositionChange" X="1" Y="1" Duration="0.3" Easing="QuadraticInOut"/>
        </LayoutAnimation>

        <StateGroup Rest="menu" Active="{state}">
            <State Name="menu">

            </State>
            <State Name="selectedOrUnder">
                <Change self.LayoutMaster="selected" DelayBack="0"/>
                <Change backgroundRect.CornerRadius="0" Duration="0.3" Delay="0.1"/>
                <Change self.Padding="0,20,0,0" Duration="0.3" DelayBack="0" Easing="QuadraticInOut" />
            </State>
            <State Name="aboveSelected">
                <Move X="1" RelativeTo="Size" Duration="0.3" Easing="CircularInOut"/>
            </State>
        </StateGroup>
    </Panel>
</Panel>
```

<!-- snippet-end -->

## Three states

Each card can be in one of three states. The "menu" state is the rest state, and is where all the cards are visible. Clicking a card brings it to the "selectedOrUnder" state. At that point, all the other cards fall into either the "selectedOrUnder" state, or the "aboveSelected" state.

Notice that the cards in front of the selected card will move to the right, while the selected card and all the cards below it will scale to fill the screen. This is handled with some JavaScript logic.


## Calculating state in JavaScript

When a card is clicked, we fire the `selectMe` method. It will determine whether a card is above or below the selected card and change the state accordingly.

<!-- snippet-begin:code/MainView.js:CalculatingStateInJS -->

```
this.selectMe = function(){
    pageSelected.value = !pageSelected.value;

    if (pageSelected.value === false)
        currentStatusbarStyle.value = StatusBarStyle.Dark;

    if (this.isSelected.value){
        for (var i = 0; i < pages.length; i++){
            pages.getAt(i).state.value = State.Menu;
            pages.getAt(i).isSelected.value = false;
        }
    } else {
        for (var i = 0; i < pages.length; i++){
            var item = pages.getAt(i);
            item.state.value = i >= index ? State.SelectedOrUnder : State.AboveSelected;

            if (i === this.index){
                item.isSelected.value = true;
                currentStatusbarStyle.value = this.statusBarStyle;
            }
            else {
                item.isSelected.value = false;
            }
        }
    }

}.bind(this);
```

<!-- snippet-end -->
