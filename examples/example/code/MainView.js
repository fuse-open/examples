var Observable = require("FuseJS/Observable");

var foobar = Observable(1);
foobar.value = 10;


// snippet-begin:SomeJSSnippet
function someFunction(x){
	var thisFunctionDoesSomething = "foo";

	// strip-begin:We also do some logging here
	for (var i = 0; i < 10; i++){
		debug_log("We do some logging");
	}
	// strip-end

	return thisFunctionDoesSomething;
}

module.exports = {
	foobar: foobar,
	someFunction: someFunction
};
// snippet-end
