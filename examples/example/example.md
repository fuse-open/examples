# This is the example article

We want to be able to reference snippets of code from the example project.

## Showing some UX snippet

Here we show a snippet with a StackPanel inside a ScrollViewer:

<!-- snippet-begin:code/MainView.ux:StackPanelInsideScrollViewer -->

```
<ScrollView ClipToBounds="true">
    <StackPanel>
        <Slider />
        the stackpanel also contains a button and a switch
    </StackPanel>
</ScrollView>
```

<!-- snippet-end -->

## Showing some UNO snippet

Here we show some cool UNO code!

<!-- snippet-begin:code/SomeCode.uno:SomeExampleUnoMethod -->

```
public float ThisMethodCalculatesSomething(float f1, float f2)
{
    var toIllustrateSnippetsFromUno = "Snippets are fun";
    debug_log("WoaaaH");

    We do some debug logging here

    return f1 + f2;
}
```

<!-- snippet-end -->

## Showing some JS snippet

Here we show some cool JS code:

<!-- snippet-begin:code/MainView.js:SomeJSSnippet -->

```
function someFunction(x){
    var thisFunctionDoesSomething = "foo";

    We also do some logging here

    return thisFunctionDoesSomething;
}

module.exports = {
    foobar: foobar,
    someFunction: someFunction
};
```

<!-- snippet-end -->
