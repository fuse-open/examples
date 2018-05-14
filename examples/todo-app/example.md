This example shows how one can make a simple todo-app with plenty of animations.

# The model

We make a JavaScript class to work as a model for each element and an `Observable` to contain them.
Each `Task` has a title and a checked field. They also have a derived field, hidden, which observes checked.
Hidden is used in markup to show and hide Tasks based on which tab is chosen.

<!-- snippet-begin:code/MainView.js:Task -->

```
function Task(title){
    var self = this;
    this.title = title;
    this.checked = Observable(false);
    this.hidden = Observable(function(){
        if (currentTab.value == "all"){
            return false;
        }
        else if (currentTab.value == "active"){
            return self.checked.value ? true : false;
        }
        else {
            return self.checked.value ? false : true;
        }
    });
}
```

<!-- snippet-end -->

An Observable is used to contain all our Tasks.

<!-- snippet-begin:code/MainView.js:Observable -->

```
var todoList = Observable();
```

<!-- snippet-end -->

Our add-button looks like a cross, and is made up of two thin panels layered on top of each other.
The buttons `Clicked` property is databound to the `addItem` function in JavaScript.

<!-- snippet-begin:code/MainView.ux:AddItemButton -->

```
<BlankButton Clicked="{addItem}" Width="60" Height="60" HitTestMode="LocalBoundsAndChildren">
    <Panel Layer="Background">
        <Panel Height="1" Width="30" Background="#ddd"/>
        <Panel Width="1" Height="30" Background="#ddd"/>
    </Panel>
</BlankButton>
```

<!-- snippet-end -->

<!-- snippet-begin:code/MainView.js:AddItemFunction -->

```
function addItem(arg) {
    todoList.add(new Task(titleInput.value));
}
```

<!-- snippet-end -->

Each item has a button which toggles whether the item is checked or not, and a button to delete it.
* Note: The following UX-snippet has been stripped to show the basic structure.

<!-- snippet-begin:code/MainView.ux:Each -->

```
<Each Items="{todoList}">
    <Grid ux:Name="itemPanel" Columns="auto,1*,auto" Alignment="Top" Height="60" Opacity="1" BoxSizing="Limit" LimitHeight="100%">


        <BlankButton Clicked="{toggleItem}" Alignment="CenterLeft" Width="60" Height="60">
            ... this button toggles an items checked state ...
        </BlankButton>

        <Panel>
            <Panel Alignment="Left">
                <Rectangle ux:Name="textLine" Height="1" Opacity="0" Margin="30,0,0,0" Color="#bbb"/>
                <Text ux:Name="itemText" Value="{title}" Alignment="CenterLeft" TextAlignment="Left" Margin="30,0,0,0" FontSize="20" Color="#999" TextWrapping="Wrap" />
            </Panel>
        </Panel>


        <WhileTrue Value="{checked}">
            <Change checkedCircleColor.Color="#50ac9a" Duration="0.3"/>
            <Change checkedIcon.Opacity="1" Duration="0.3"/>
            <Change textLine.Opacity="1" Duration="0.3"/>
            <Change itemText.Color="#ddd" Duration="0.3"/>
        </WhileTrue>

        <BlankButton Alignment="CenterRight" Clicked="{deleteItem}" Width="60" Height="60"
                HitTestMode="LocalBoundsAndChildren">
            ... the delete button ...
        </BlankButton>

        <LayoutAnimation>
            <Move RelativeTo="LayoutChange" Y="1" Duration="0.6" Easing="BounceIn"/>
        </LayoutAnimation>
        <AddingAnimation>
            <Move RelativeTo="Size" X="1" Duration="0.3" Easing="CircularInOut" />
        </AddingAnimation>
        <RemovingAnimation>
            <Move RelativeTo="Size" X="1" Duration="0.3" Easing="CircularInOut" />
        </RemovingAnimation>

        <Rectangle Height="1" ColumnSpan="3" Row="0" Alignment="Bottom" Color="#ddd"/>
    </Grid>
</Each>
```

<!-- snippet-end -->

Every Task-element has a delete button which is bound to the `deleteItem` function in javascript.

<!-- snippet-begin:code/MainView.js:DeleteItemFunction -->

```
function deleteItem(arg){
    todoList.tryRemove(arg.data);
}
```

<!-- snippet-end -->

# The tabs

The tabs in the bottom lets us toggle between vieweing "all", "completed" or "active" Tasks.
They have a `Rectangle` whith a red border which jumps between the tabs, indicating which one is active.
Such an animation is done easily with a `MultiLayoutPanel`.

MultiLayoutPanel lets us define several layouts which we can switch and animate between using the `Placeholder` class.
A `Placeholder` lets us reference the same element in the different layouts we put inside our `MultiLayoutPanel`.
In our case we use Placeholders to reference the red indicator-rectangle to make it move between the tabs.

<!-- snippet-begin:code/MainView.ux:MultiLayout -->

```
<MultiLayoutPanel ux:Name="multiLayout">
    <GridLayout Columns="1*,1*,1.5*"/>
    <BlankButton ux:Name="allButton" Clicked="{showAll}">
        <Panel Alignment="Center">
            <SmallText Value="All" TextAlignment="Center"/>
            <Placeholder>
                <Rectangle ux:Name="indicator" Column="0" Row="0" CornerRadius="3" Margin="-10,-5,-10,-5">
                    <LayoutAnimation>
                        <Move RelativeTo="LayoutChange" X="1" Duration="0.5" Easing="BackIn"/>
                        <Resize RelativeTo="LayoutChange" Duration="0.5" Easing="BackIn"/>
                    </LayoutAnimation>
                    <Stroke Width="1" Brush="#E7D9DE"/>
                </Rectangle>
            </Placeholder>
        </Panel>

        <Clicked>
            <Set multiLayout.LayoutElement="allButton" />
        </Clicked>
    </BlankButton>
    <BlankButton ux:Name="activeButton" Clicked="{showActive}">
        <Panel Alignment="Center">
            <SmallText TextAlignment="Center" Value="Active" />
            <Placeholder Target="indicator"/>
        </Panel>
        <Clicked>
            <Set multiLayout.LayoutElement="activeButton"/>
        </Clicked>
    </BlankButton>
    <BlankButton ux:Name="completedButton" Clicked="{showCompleted}">
        <Panel Alignment="Center">
            <SmallText TextAlignment="Center" Value="Completed"/>
            <Placeholder Target="indicator"/>
        </Panel>
        <Clicked>
            <Set multiLayout.LayoutElement="completedButton"/>
        </Clicked>
    </BlankButton>
</MultiLayoutPanel>
```

<!-- snippet-end -->

Each tab-button changes the `currentTab` variable in JavaScript by calling the following functions.

<!-- snippet-begin:code/MainView.js:ShowAllFunction -->

```
function showAll() {
    currentTab.value = "all";
}
```

<!-- snippet-end -->

<!-- snippet-begin:code/MainView.js:ShowActiveFunction -->

```
function showActive() {
    currentTab.value = "active";
}
```

<!-- snippet-end -->

<!-- snippet-begin:code/MainView.js:ShowCompletedFunction -->

```
function showCompleted() {
    currentTab.value = "completed";
}
```

<!-- snippet-end -->

# Remaining indicator and clear all button

The text indicating unfinished Tasks is made using a derived `Observable`.

First we use `observable.count(func)` to calculate the number of unchecked Tasks.

<!-- snippet-begin:code/MainView.js:RemainingCount -->

```
var remainingCount = todoList.count(function(x){
    return x.checked.not();
});
```

<!-- snippet-end -->

Then we use `observable.map(func)` on `remainingCount` to create an observable string.

<!-- snippet-begin:code/MainView.js:RemainingText -->

```
var remainingText = remainingCount.map(function(x){
    return x + " " + ((x == 1) ? "task" : "tasks") + " remaining";
});
```

<!-- snippet-end -->

We can then just databind to `remainingText` from UX and it will be updated whenever the number of checked Task changes.
Note that Task.checked is itself observable, which is necessary for this to work.

<!-- snippet-begin:code/MainView.ux:RemainingTextUX -->

```
<SmallText Value="{remainingText}" Margin="10" Alignment="Left"/>
```

<!-- snippet-end -->

The `Clear completed` button should only be visible when one or more Tasks are checked.
We hide the button with a `WhileDisabled` trigger and databinding the buttons IsEnabled property to the `canClearCompleted` variable in JavaScript.

<!-- snippet-begin:code/MainView.ux:ClearCompletedButton -->

```
<Panel ux:Name="clearCompletedButton" Alignment="TopRight" Clicked="{clearCompleted}"  Margin="10" IsEnabled="{canClearCompleted}">
    <Text Value="Clear completed" FontSize="14" Color="#999" Margin="10,0,10,0" />
    <WhileDisabled>
        <Change clearCompletedButton.Opacity="0" Duration="0.3"/>
    </WhileDisabled>
</Panel>
```

<!-- snippet-end -->

<!-- snippet-begin:code/MainView.js:CanClearCompleted -->

```
var canClearCompleted = todoList.count(function(x){
    return x.checked;
}).map(function(x){
    return x > 0;
});
```

<!-- snippet-end -->

<!-- snippet-begin:code/MainView.js:ClearCompleted -->

```
function clearCompleted() {
    todoList.removeWhere(function(x) { return x.checked.value; });
}
```

<!-- snippet-end -->
