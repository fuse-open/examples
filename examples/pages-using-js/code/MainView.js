// snippet-begin:MainJS
var Observable = require("FuseJS/Observable");

function createPage(title) {
	return {
		title: title,
		clicked: function() {
			router.push("subPage", { title: title })
		}
	};
}

var pages = Observable();

for (var i = 1; i <= 20; i++) {
	pages.add(createPage("PAGE " + i));
}

module.exports = { pages: pages };
//snippet-end
