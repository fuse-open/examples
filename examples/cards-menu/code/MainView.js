var Observable = require('FuseJS/Observable');

var State = {
	Menu : 'menu',
	SelectedOrUnder: 'selectedOrUnder',
	AboveSelected: 'aboveSelected'
};

var StatusBarStyle = {
	Light : 'Light',
	Dark : 'Dark'
};



var currentStatusbarStyle = Observable(StatusBarStyle.Dark);

var pageSelected = Observable(false);

var pages = Observable();

var offsetSize = 60;
function Item(name, color, textColor, statusBarStyle, index, nPages){
	var offY = (offsetSize*(nPages - 1)) - offsetSize * (index +- 1);
	var offX = offY/4;

	this.name = name;
	this.color = color;
	this.textColor = textColor;

	this.index = index;
	this.statusBarStyle = statusBarStyle;
	this.offset = [offX,offY];

	this.state = Observable(State.Menu);

	this.isSelected = Observable(false);

	//snippet-begin:CalculatingStateInJS
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
	//snippet-end
}


pages.add(new Item("PROJECTS","#FFF199","#B0545E",StatusBarStyle.Dark,0,4));
pages.add(new Item("TEAM","#FFB37B","#B24B5D",StatusBarStyle.Dark,1,4));
pages.add(new Item("ABOUT","#B17179","#FAD96E",StatusBarStyle.Light,2,4));
pages.add(new Item("CONTACT","#4A304D","#D4725E",StatusBarStyle.Light,3,4));

module.exports = {
	pages : pages,
	pageSelected : pageSelected,
	currentStatusbarStyle : currentStatusbarStyle
};
