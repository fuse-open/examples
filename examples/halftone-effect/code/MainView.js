var Observable = require('FuseJS/Observable');
var Camera = require('FuseJS/Camera');

//snippet-begin:TakePicture
var picture = Observable('Icons/background.jpg');
var filterApplied = Observable(false);

function takePicture(){
	Camera.takePicture(1000, 1200).then(function(file){
		picture.value = file;
		filterApplied.value = false;
	}).catch(function(e) {
		console.log(e);
		filterApplied.value = false;
	});
};
//snippet-end

function applyFilter() {
	filterApplied.value = true;
};

module.exports = {
	picture: picture,
	takePicture: takePicture,
	applyFilter: applyFilter,
	filterApplied: filterApplied
};
