var Observable = require('FuseJS/Observable');

//snippet-begin:ButtonState
function LoadButton(doneLoadingState){
	this.doneLoadingState = doneLoadingState;
	this.finishState = Observable("defaultState");
	this.doneLoading = function(){
		this.finishState.value = this.doneLoadingState;
	}.bind(this);
	this.startLoading = function(){
		if (this.finishState.value === "defaultState")
			this.finishState.value = "loadingState";
		else
			this.finishState.value = "defaultState";
	}.bind(this);
}
//snippet-end


var buttons = Observable(
	new LoadButton("success"),
	new LoadButton("error")
);

module.exports = {
	buttons: buttons
};
