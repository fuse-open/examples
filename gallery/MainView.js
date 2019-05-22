var Observable = require('FuseJS/Observable');

// snippet-begin:Initialization
function Picture(resource) {
	this.resource = "Assets/Pictures/" + resource + ".jpg";
	this.isSelected = Observable(false);
	this.indicateModeChange = Observable(false);
}

pictures = Observable();
for (i = 1; i < 21; i++) {
	pictures.add(new Picture('Unsplash' + i));
}
// snippet-end

// snippet-begin:SelectionMode
selectionMode = Observable(false);
numberOfSelected = Observable(0);
// snippet-end

// snippet-begin:Header
header = Observable(function () {
	if (selectionMode.value === false)
		return 'Gallery';
	else return 'Gallery (' + numberOfSelected.value + ')';
});
// snippet-end


// snippet-begin:ToggleSelections
function goToSelectionMode(args) {
	if (selectionMode.value === true) return;
	selectionMode.value = true;
	args.data.indicateModeChange.value = true;
	args.data.isSelected.value = true;
	numberOfSelected.value = 1;
}

function toggleSelect(args) {
	if (selectionMode.value === false) return;
	if (args.data.isSelected.value === false) {
		numberOfSelected.value = numberOfSelected.value + 1;
	} else {
		numberOfSelected.value = numberOfSelected.value - 1;
		if (numberOfSelected.value === 0) {
			selectionMode.value = false;
		}
	}
	args.data.isSelected.value = !args.data.isSelected.value;
}
// snippet-end

// snippet-begin:DeleteSelected
function deleteSelected(args) {
	pictures.removeWhere(function (p) {
		return p.isSelected.value === true;
	});
	numberOfSelected.value = 0;
	selectionMode.value = false;
}
// snippet-end

function nullModeChange(args) {
	args.data.indicateModeChange.value = false;
}

module.exports = {
	pictures: pictures,
	selectionMode : selectionMode,
	goToSelectionMode : goToSelectionMode,
	toggleSelect : toggleSelect,
	header : header,
	deleteSelected : deleteSelected,
	nullModeChange : nullModeChange
};
