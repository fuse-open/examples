var Observable = require("FuseJS/Observable");

module.exports = Observable(
	{
		id: 0,
		name: 'Ancient Bristlecone Pine Forest',
		country: 'United States',
		color: '#4F99EE',
		image: "Assets/bristlecone.png",
		numbers: [
			{ title: 'Elevation', fact: '3,410 m' },
			{ title: 'Area', fact: '113 km2' }
		],
		facts: [
			'The Ancient Bristlecone Pine Forest is a protected area high in the White Mountains in Inyo County in eastern California.',
			'The Great Basin Bristlecone Pine (Pinus longaeva) trees grow between 9,800 and 11,000 feet (3,000–3,400 m) above sea level',
			"For many years, it was the world's oldest known living non-clonal organism."
		]
	},{
		id:1,
		name: 'Iceland',
		country: 'Iceland',
		color: '#7D83D2',
		image: "Assets/iceland.png",
		numbers: [
			{ title: 'Population', fact: '323,002 (2013)' },
			{ title: 'Currency', fact: 'Icelandic króna'}
		],
		facts: [
			'Icelandic is a Nordic island country in the North Atlantic Ocean.',
			'According to Landnámabók, the settlement of Iceland began in the year 874 AD when the Norwegian chieftain Ingólfr Arnarson became the first permanent settler on the island.',
			'Iceland has a market economy with relatively low taxes compared to other OECD countries.'
		]
	},{
		id: 2,
		name: 'Monte Altissimo di Nago',
		country: 'Italy',
		color: '#4F99EE',
		image: "Assets/monte.png",
		numbers: [
			{ title: 'Elevation', fact: '2,074 m' },
			{ title: 'Province', fact: 'Trentino' }
		],
		facts:  [
			'Monte Altissimo di Nago is one of the highest summits of the Monte Baldo mountain range and thereby part of the Garda Mountains in northern Italy.',
			'The Altissimo is the highest peak in the northern part of the Monte-Baldo range.',
			'On top of the Altissimo there is a mountain hut, the Rifugio Damiano Chiesa. The easiest way to reach the top is a hike over a dirt road from Strada Provinciale del Monte Baldo.'
		]
	},{
		id: 3,
		name: 'Setagaya, Tokyo',
		country: 'Japan',
		color: '#4F99EE',
		image: "Assets/setagaya.png",
		numbers: [
			{ title: 'Population', fact: '890,927' },
			{ title: 'Area', fact: '58.08 km2' }
		],
		facts:  [
			'Setagaya is a special ward in Tokyo, Japan.',
			"Setagaya has the largest population and second largest area (after Ōta) of Tokyo's 23 special wards.",
			'Setagaya is located at the southwestern corner of the 23 special wards and Tama River separates the boundary between Tokyo Metropolis and Kanagawa Prefecture.'
		]
	},{
		id: 4,
		name: 'Provo Canyon',
		country: 'USA',
		color: '#4F99EE',
		image: "Assets/provo.png",
		numbers: [
			{ title: 'From', fact: 'Orem' },
			{ title: 'To', fact: 'Heber City' }
		],
		facts: [
			'Provo Canyon is a canyon located in unincorporated Utah County, Utah, USA.',
			'Provo Canyon splits between Mount Timpanogos on the north and Mount Cascade on the south.',
			'Attractions in Provo Canyon include Vivian Park and Bridal Veil Falls.'
		]
	},{
		id: 5,
		name: 'Ciucaș Peak',
		country: 'Romania',
		color: '#4F99EE',
		image: "Assets/ciucas.png",
		numbers: [
			{ title: 'Elevation', fact: '1,954 m' },
			{ title: 'Parent range', fact: 'Ciucaş Mountains' }
		],
		facts:  [
			'Ciucaş Peak (Romanian: Vârful Ciucaş) is the highest peak of Ciucaş Mountains, Southern Carpathians.',
		]
	}
);
 
