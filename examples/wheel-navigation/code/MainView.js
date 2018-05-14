var Observable = require("FuseJS/Observable");

var items = Observable([
	{ value: 25, color: "#4CD8FC" },
	{ value: 15, color: "#A943C1" },
	{ value: 30, color: "#FFCE6B" },
	{ value: 10, color: "#EB4CAF" },
	{ value: 20, color: "#33CB9F" }
]);

var currentPage = Observable(0);

function activated(arg) {
	currentPage.value = arg.data.index;
}

var defaultRotation = Observable(0);
module.exports = {
	items: items.map(function(i){
		var lastItem = {
			startAngle: 0,
			endAngle: 0,
			angle: 0
		};
		i.forEach(function(x, c) {
			x.index = c;
			x.angle = ((x.value / 100) * 360);
			if (c === 0) {
				defaultRotation.value = x.angle / 2 + 90;
				lastItem.wheelRotate = 0;
			}
			if (c > 0) {
				lastItem.wheelRotate = (x.angle / 2) + (lastItem.angle / 2);
			}

			x.startAngle = lastItem.startAngle - x.angle;
			x.endAngle = lastItem.startAngle;

			x.isActive = Observable(function(){
				return currentPage.value == x.index;
			});

			lastItem = x;

		});
		return i;
	}).expand(),
	currentPage: currentPage,
	activated: activated,
	defaultRotation: defaultRotation
};
