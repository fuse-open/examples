var Observable = require("FuseJS/Observable");


//snippet-begin:Task
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
//snippet-end

//snippet-begin:Observable
var todoList = Observable();
//snippet-end
var titleInput = Observable("");
var currentTab = Observable("all");

//snippet-begin:RemainingCount
var remainingCount = todoList.count(function(x){
	return x.checked.not();
});
//snippet-end

//snippet-begin:RemainingText
var remainingText = remainingCount.map(function(x){
	return x + " " + ((x == 1) ? "task" : "tasks") + " remaining";
});
//snippet-end

//snippet-begin:CanClearCompleted
var canClearCompleted = todoList.count(function(x){
	return x.checked;
}).map(function(x){
	return x > 0;
});
//snippet-end

//snippet-begin:AddItemFunction
function addItem(arg) {
	todoList.add(new Task(titleInput.value));
}
//snippet-end

//snippet-begin:DeleteItemFunction
function deleteItem(arg){
	todoList.tryRemove(arg.data);
}
//snippet-end

function toggleAll(arg) {
	var remaining = remainingCount.value;
	todoList.forEach(function(x){
		x.checked.value = (remaining == 0) ? false : true;
	});
}

function toggleItem(arg) {
	arg.data.checked.value = !arg.data.checked.value;
}

//snippet-begin:ClearCompleted
function clearCompleted() {
	todoList.removeWhere(function(x) { return x.checked.value; });
}
//snippet-end

//snippet-begin:ShowAllFunction
function showAll() {
	currentTab.value = "all";
}
//snippet-end

//snippet-begin:ShowActiveFunction
function showActive() {
	currentTab.value = "active";
}
//snippet-end

//snippet-begin:ShowCompletedFunction
function showCompleted() {
	currentTab.value = "completed";
}
//snippet-end

module.exports = {
	todoList: todoList,
	titleInput: titleInput,
	currentTab: currentTab,
	remainingText: remainingText,
	canClearCompleted: canClearCompleted,
	addItem: addItem,
	deleteItem: deleteItem,
	toggleAll: toggleAll,
	toggleItem: toggleItem,
	clearCompleted: clearCompleted,
	showAll: showAll,
	showActive: showActive,
	showCompleted: showCompleted
};
