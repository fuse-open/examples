var Observable = require('FuseJS/Observable');

//snippet-begin:TheJS
function Item(image, category, name, text){
	this.category = category;
	this.name = name;
	this.text = text;
	this.image = image;
}


var items = Observable(
	new Item("Assets/Images/food1.jpg","Fruit", "Lime","A lime (from Arabic and French lim) is a hybrid citrus fruit, which is typically round, lime green, 3-6 centimetres in diameter, and containing acidic juice vesicles. There are several species of citrus trees whose fruits are called limes, including the Key lime (Citrus aurantifolia), Persian lime, kaffir lime, and desert lime. Limes are an excellent source of vitamin C, and are often used to accent the flavours of foods and beverages. They are grown year-round. Plants with fruit called \"limes\" have diverse genetic origins; limes do not form a monophyletic group."),
//snippet-end
	new Item("Assets/Images/food2.jpg","Vegetable", "Kohlrabi","Kohlrabi - the German turnip or turnip cabbage (Brassica oleracea Gongylodes Group) is an annual vegetable, and is a low, stout cultivar of cabbage. Edible preparations are made with both the stem and the leaves. One commonly used variety grows without a swollen stem, having just leaves and a very thin stem, and is called Haakh. Haakh and Monj are popular Kashmiri dishes made using this vegetable. Kohlrabi can be eaten raw as well as cooked." ),
	new Item("Assets/Images/food3.jpg", "Fruit", "Plums", "A plum is a fruit of the subgenus Prunus of the genus Prunus. The subgenus is distinguished from other subgenera (peaches, cherries, bird cherries, etc.) in the shoots having a terminal bud and solitary side buds (not clustered), the flowers in groups of one to five together on short stems, and the fruit having a groove running down one side and a smooth stone (or pit). Mature plum fruit may have a dusty-white coating that gives them a glaucous appearance. This is an epicuticular wax coating and is known as \"wax bloom\". Dried plum fruits are called dried plums or prunes, although prunes are a distinct type of plum, and may have antedated the fruits now commonly known as plums."),
	new Item("Assets/Images/food4.jpg","Baked goods", "Crisp cookies", "A cookie is a small, flat, sweet, baked good, usually containing flour, eggs, sugar, and either butter, cooking oil or another oil or fat. It may include other ingredients such as raisins, oats, chocolate chips or nuts. In most English-speaking countries except for the US and Canada, crisp cookies are called biscuits. Chewier cookies are commonly called cookies even in the UK. Some cookies may also be named by their shape, such as date squares or bars."),
	new Item("Assets/Images/food5.jpg","Vegetable", "Tomatoes", "The tomato is the edible, often red berry-type fruit of the nightshade Solanum lycopersicum, commonly known as a tomato plant. The tomato is consumed in diverse ways, including raw, as an ingredient in many dishes, sauces, salads, and drinks. The English word tomato comes from the Spanish word, tomate, derived from the Nahuatl word tomatl. It first appeared in print in 1595."),
	new Item("Assets/Images/food6.jpg","Vegetable", "Lettuce", "Lettuce is most often used for salads, although it is also seen in other kinds of food, such as soups, sandwiches and wraps; it can also be grilled. One variety, the woju, or asparagus lettuce, is grown for its stems, which are eaten either raw or cooked. Lettuce is a rich source of vitamin K and vitamin A, and is a moderate source of folate and iron."),
	new Item("Assets/Images/food7.jpg","Vegetable", "Onion", "Onions are cultivated and used around the world. As a food item, they are usually served cooked, as a vegetable or part of a prepared savoury dish, but can also be eaten raw or used to make pickles or chutneys. They are pungent when chopped and contain certain chemical substances which irritate the eyes.")
);

module.exports = {
	items: items
};
