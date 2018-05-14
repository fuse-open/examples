var Observable = require('FuseJS/Observable');

// selectedImage stores the currently selected image index
var selectedImage = Observable();

// we then reactively return the low and high quality image URLs, depending on what the selected image index is
var lowQualityImage = selectedImage.map(function(idx) {
	return thumbs[idx].low;
});
var highQualityImage = selectedImage.map(function(idx) {
	return thumbs[idx].high;
});

// images array holds a list of unique images hosted on unsplash.com
var images = ["eWFdaPRFjwE", "_FjegPI89aU", "_RBcxo9AU-U", "PQvRco_SnpI", "6hxvm0NzYP8", "b-yEdfrvQ50", "pHANr-CpbYM", "45FJgZMXCK8", "9wociMvaquU", "tI_Odb7ZU6M"];

// thumbs array holds a list of our image objects, consisting of an id, low and high quality URLs, and an Observable boolean for tracking the selected state
var thumbs = [];
for (var i in images) {
	thumbs.push({
		id: thumbs.length,
		low: "https://source.unsplash.com/" + images[i] + "/108x192",
		high: "https://source.unsplash.com/" + images[i] + "/1080x1920",
		selected: Observable(false)
	});
}

// selectImage function takes care of toggling the selected states for thumbnails, as well as changes the selectedImage index
function selectImage(args) {
	for (var i in thumbs) thumbs[i].selected.value = false;
	thumbs[args.data.id].selected.value = true;
	selectedImage.value = args.data.id;
};

// here is the stuff that we want our MainView to have access to
module.exports = {
	thumbs: thumbs,
	selectImage: selectImage,
	lowQualityImage: lowQualityImage,
	highQualityImage: highQualityImage
};
