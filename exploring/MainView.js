var Observable = require('FuseJS/Observable');
var places = require("places");

var current = Observable();

var inDetailsMode = current.map(function(x){
	return !!x;
});

module.exports = {
	places: places.map(function(item, index){
		item.alignment = index % 2 === 0 ? "Left" : "Right";
		return item;
	}),
	current: current,
	inDetailsMode: inDetailsMode
};
