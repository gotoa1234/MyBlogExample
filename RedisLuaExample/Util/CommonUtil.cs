using System.IO.Compression;
using System.Text;

namespace RedisLuaExample.Util
{
    public static class CommonUtil
    {
        /// <summary>
        /// 壓縮 Json 工具
        /// </summary>
        public static byte[] Compress(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    msi.CopyTo(gs);
                }
                return mso.ToArray();
            }
        }

        /// <summary>
        /// 32kb 由 https://json-generator.com/ 產生
        /// </summary>
        public static string GetTestJSon = """
            [
              {
                "_id": "6703e9c3ee51e6fd03d69a03",
                "index": 0,
                "guid": "7cbbd3df-37f1-4666-9400-6b180eb8d6e4",
                "isActive": true,
                "balance": "$2,729.77",
                "picture": "http://placehold.it/32x32",
                "age": 26,
                "eyeColor": "brown",
                "name": "Mcintosh Clemons",
                "gender": "male",
                "company": "DIGIQUE",
                "email": "mcintoshclemons@digique.com",
                "phone": "+1 (919) 471-2389",
                "address": "225 Rutledge Street, Suitland, District Of Columbia, 6631",
                "about": "Magna aute occaecat ullamco ullamco sit cupidatat do eiusmod. Reprehenderit anim ut nulla cupidatat elit ea qui commodo. Lorem exercitation proident irure culpa quis Lorem proident. Sit laborum nostrud aute eu incididunt mollit adipisicing aliquip. Sunt excepteur eu non consectetur proident tempor irure reprehenderit. Aliquip quis quis duis cillum magna enim occaecat sit magna ex eiusmod incididunt aliqua. Dolore dolor tempor deserunt irure ut fugiat incididunt qui non est anim.\r\n",
                "registered": "2022-03-15T02:38:41 -08:00",
                "latitude": 38.927036,
                "longitude": -136.861007,
                "tags": [
                  "incididunt",
                  "nisi",
                  "exercitation",
                  "ea",
                  "laborum",
                  "ex",
                  "eiusmod"
                ],
                "friends": [
                  {
                    "id": 0,
                    "name": "Benson Franks"
                  },
                  {
                    "id": 1,
                    "name": "Freeman Reeves"
                  },
                  {
                    "id": 2,
                    "name": "Noreen Cantu"
                  }
                ],
                "greeting": "Hello, Mcintosh Clemons! You have 4 unread messages.",
                "favoriteFruit": "strawberry"
              },
              {
                "_id": "6703e9c379862db30ffdb738",
                "index": 1,
                "guid": "d0f0d5f7-5163-4732-99a1-1c8938305d6c",
                "isActive": false,
                "balance": "$3,211.24",
                "picture": "http://placehold.it/32x32",
                "age": 22,
                "eyeColor": "brown",
                "name": "Alisha Mccall",
                "gender": "female",
                "company": "PORTALIS",
                "email": "alishamccall@portalis.com",
                "phone": "+1 (976) 502-3966",
                "address": "947 McKibbin Street, Cartwright, Idaho, 4962",
                "about": "Ullamco reprehenderit eiusmod incididunt cillum. Aliquip ea incididunt esse in. Duis ullamco nulla officia ipsum adipisicing deserunt qui incididunt pariatur. Officia non fugiat occaecat officia fugiat do ex esse consequat anim culpa laboris occaecat.\r\n",
                "registered": "2019-01-15T12:25:14 -08:00",
                "latitude": 65.503172,
                "longitude": 121.546747,
                "tags": [
                  "enim",
                  "eiusmod",
                  "velit",
                  "qui",
                  "cillum",
                  "et",
                  "incididunt"
                ],
                "friends": [
                  {
                    "id": 0,
                    "name": "Melissa Acevedo"
                  },
                  {
                    "id": 1,
                    "name": "Kaye Ortiz"
                  },
                  {
                    "id": 2,
                    "name": "Dawson Shields"
                  }
                ],
                "greeting": "Hello, Alisha Mccall! You have 1 unread messages.",
                "favoriteFruit": "banana"
              },
              {
                "_id": "6703e9c3966d5c4588043a09",
                "index": 2,
                "guid": "7370b9a4-5d3f-48cc-bc0f-13b2cb62b88a",
                "isActive": true,
                "balance": "$2,677.94",
                "picture": "http://placehold.it/32x32",
                "age": 29,
                "eyeColor": "blue",
                "name": "Christy Hatfield",
                "gender": "female",
                "company": "ASSISTIX",
                "email": "christyhatfield@assistix.com",
                "phone": "+1 (948) 563-3553",
                "address": "322 Sandford Street, Baden, Kansas, 8480",
                "about": "Voluptate adipisicing officia eu aute laborum cupidatat elit. Ex ex labore pariatur ipsum Lorem Lorem laborum quis aliquip non labore aute. Irure ex quis incididunt commodo et labore deserunt ipsum eiusmod aute ipsum. Exercitation eu in reprehenderit adipisicing duis quis veniam ipsum in occaecat do quis Lorem quis. Id excepteur pariatur eu ea velit et cillum cupidatat. Sunt ea voluptate non esse ex nostrud irure cupidatat veniam cillum.\r\n",
                "registered": "2023-08-29T07:26:43 -08:00",
                "latitude": 51.047162,
                "longitude": 153.622288,
                "tags": [
                  "labore",
                  "occaecat",
                  "qui",
                  "adipisicing",
                  "consequat",
                  "aliquip",
                  "esse"
                ],
                "friends": [
                  {
                    "id": 0,
                    "name": "Whitney Joyce"
                  },
                  {
                    "id": 1,
                    "name": "Ester Ramirez"
                  },
                  {
                    "id": 2,
                    "name": "Garza Carey"
                  }
                ],
                "greeting": "Hello, Christy Hatfield! You have 2 unread messages.",
                "favoriteFruit": "banana"
              },
              {
                "_id": "6703e9c3fb94496352f30a2b",
                "index": 3,
                "guid": "bc55a665-d913-44aa-8075-59833c82f7c0",
                "isActive": false,
                "balance": "$2,347.08",
                "picture": "http://placehold.it/32x32",
                "age": 26,
                "eyeColor": "blue",
                "name": "Snider Henderson",
                "gender": "male",
                "company": "MARKETOID",
                "email": "sniderhenderson@marketoid.com",
                "phone": "+1 (815) 515-3991",
                "address": "108 Provost Street, Como, Alabama, 5831",
                "about": "Officia fugiat laboris sunt incididunt Lorem sint elit in duis culpa. Veniam ipsum mollit occaecat qui ullamco. Cupidatat irure tempor non quis nulla magna laborum consectetur.\r\n",
                "registered": "2021-07-08T02:15:00 -08:00",
                "latitude": -26.347151,
                "longitude": -150.237739,
                "tags": [
                  "cillum",
                  "enim",
                  "cupidatat",
                  "consectetur",
                  "aliquip",
                  "reprehenderit",
                  "anim"
                ],
                "friends": [
                  {
                    "id": 0,
                    "name": "Cobb Atkins"
                  },
                  {
                    "id": 1,
                    "name": "Gabriela Rich"
                  },
                  {
                    "id": 2,
                    "name": "Colon Rivas"
                  }
                ],
                "greeting": "Hello, Snider Henderson! You have 1 unread messages.",
                "favoriteFruit": "strawberry"
              },
              {
                "_id": "6703e9c39c197a122991b238",
                "index": 4,
                "guid": "b7a2e462-fa34-4bbb-8566-5a8df7515d4d",
                "isActive": true,
                "balance": "$3,768.26",
                "picture": "http://placehold.it/32x32",
                "age": 26,
                "eyeColor": "green",
                "name": "Spears Higgins",
                "gender": "male",
                "company": "PRINTSPAN",
                "email": "spearshiggins@printspan.com",
                "phone": "+1 (900) 429-2418",
                "address": "503 Surf Avenue, Kirk, Oklahoma, 3551",
                "about": "Excepteur magna aliqua labore ad. Velit ut nisi magna aliqua dolor aute officia et proident adipisicing. Elit ipsum consequat culpa voluptate pariatur nostrud commodo officia. Consequat esse aute culpa et mollit aliquip dolore laboris amet est. Voluptate elit veniam minim esse id labore.\r\n",
                "registered": "2015-07-10T04:00:39 -08:00",
                "latitude": 25.290872,
                "longitude": 143.766291,
                "tags": [
                  "eiusmod",
                  "adipisicing",
                  "in",
                  "aliquip",
                  "laborum",
                  "fugiat",
                  "nisi"
                ],
                "friends": [
                  {
                    "id": 0,
                    "name": "Jamie Campbell"
                  },
                  {
                    "id": 1,
                    "name": "Torres Heath"
                  },
                  {
                    "id": 2,
                    "name": "Amy Whitney"
                  }
                ],
                "greeting": "Hello, Spears Higgins! You have 9 unread messages.",
                "favoriteFruit": "apple"
              },
              {
                "_id": "6703e9c31e234596b0122d6c",
                "index": 5,
                "guid": "9999a0d6-4983-409b-a000-530411cd7d4e",
                "isActive": true,
                "balance": "$2,271.66",
                "picture": "http://placehold.it/32x32",
                "age": 20,
                "eyeColor": "brown",
                "name": "Sexton Mclaughlin",
                "gender": "male",
                "company": "HONOTRON",
                "email": "sextonmclaughlin@honotron.com",
                "phone": "+1 (988) 449-2967",
                "address": "955 Aster Court, Evergreen, Maine, 7104",
                "about": "Voluptate culpa labore fugiat pariatur officia enim dolore excepteur dolore. Sit qui id pariatur pariatur proident laborum sunt enim reprehenderit minim. Excepteur ea tempor labore aliqua mollit exercitation consequat Lorem consectetur. Duis ad velit Lorem consectetur ea id exercitation sit eu. Et aliquip dolore ullamco eu eiusmod sit dolor aliqua cupidatat. Dolor aute occaecat fugiat enim.\r\n",
                "registered": "2015-12-12T11:13:36 -08:00",
                "latitude": 72.881491,
                "longitude": -76.425212,
                "tags": [
                  "irure",
                  "minim",
                  "commodo",
                  "eu",
                  "et",
                  "nisi",
                  "minim"
                ],
                "friends": [
                  {
                    "id": 0,
                    "name": "Combs Wiggins"
                  },
                  {
                    "id": 1,
                    "name": "Augusta Fitzpatrick"
                  },
                  {
                    "id": 2,
                    "name": "Brock Chambers"
                  }
                ],
                "greeting": "Hello, Sexton Mclaughlin! You have 3 unread messages.",
                "favoriteFruit": "banana"
              },
              {
                "_id": "6703e9c31041b31a222cf506",
                "index": 6,
                "guid": "28d9a5ab-6a58-462f-8a6d-33c92254ab39",
                "isActive": true,
                "balance": "$3,605.91",
                "picture": "http://placehold.it/32x32",
                "age": 29,
                "eyeColor": "brown",
                "name": "Noemi Hahn",
                "gender": "female",
                "company": "ENORMO",
                "email": "noemihahn@enormo.com",
                "phone": "+1 (841) 592-3086",
                "address": "747 Lincoln Avenue, Jenkinsville, Iowa, 647",
                "about": "Sint cillum dolore deserunt reprehenderit excepteur. Excepteur eiusmod labore ad sunt magna amet tempor sunt est sunt dolor cupidatat. Adipisicing magna commodo in adipisicing sit elit culpa proident duis ad laboris. Veniam fugiat anim occaecat irure culpa anim. Laboris adipisicing aute mollit reprehenderit veniam ipsum. Reprehenderit labore officia dolor nulla cillum irure pariatur ipsum cillum veniam nulla. Enim proident pariatur ipsum quis deserunt qui.\r\n",
                "registered": "2021-10-20T05:48:13 -08:00",
                "latitude": -23.37637,
                "longitude": 19.225834,
                "tags": [
                  "dolor",
                  "reprehenderit",
                  "ad",
                  "anim",
                  "pariatur",
                  "ex",
                  "fugiat"
                ],
                "friends": [
                  {
                    "id": 0,
                    "name": "Danielle Sullivan"
                  },
                  {
                    "id": 1,
                    "name": "Hyde Cantrell"
                  },
                  {
                    "id": 2,
                    "name": "Burks Dickson"
                  }
                ],
                "greeting": "Hello, Noemi Hahn! You have 2 unread messages.",
                "favoriteFruit": "strawberry"
              },
              {
                "_id": "6703e9c3a7bfade819329669",
                "index": 7,
                "guid": "4cdbb33d-3c66-451f-86c1-07814433f796",
                "isActive": true,
                "balance": "$3,102.99",
                "picture": "http://placehold.it/32x32",
                "age": 27,
                "eyeColor": "brown",
                "name": "Alyson Navarro",
                "gender": "female",
                "company": "ANIVET",
                "email": "alysonnavarro@anivet.com",
                "phone": "+1 (885) 407-2285",
                "address": "466 Utica Avenue, Strong, Puerto Rico, 6292",
                "about": "Adipisicing nostrud amet do quis laboris aliqua ullamco aliqua do reprehenderit ut eu reprehenderit adipisicing. In esse elit minim dolore et. Nulla cupidatat reprehenderit excepteur ad occaecat eiusmod irure id ut reprehenderit. Aute anim non ea laboris consequat amet ad dolore incididunt laborum.\r\n",
                "registered": "2014-03-07T07:26:22 -08:00",
                "latitude": -34.779415,
                "longitude": -58.80303,
                "tags": [
                  "incididunt",
                  "incididunt",
                  "quis",
                  "et",
                  "sint",
                  "do",
                  "id"
                ],
                "friends": [
                  {
                    "id": 0,
                    "name": "Obrien Oneal"
                  },
                  {
                    "id": 1,
                    "name": "Raymond Good"
                  },
                  {
                    "id": 2,
                    "name": "Marquez Hester"
                  }
                ],
                "greeting": "Hello, Alyson Navarro! You have 9 unread messages.",
                "favoriteFruit": "apple"
              },
              {
                "_id": "6703e9c3ec062f8008ab59ae",
                "index": 8,
                "guid": "af65c73d-1e56-450a-9900-287f8b18ee63",
                "isActive": true,
                "balance": "$2,872.74",
                "picture": "http://placehold.it/32x32",
                "age": 31,
                "eyeColor": "green",
                "name": "Cristina Byers",
                "gender": "female",
                "company": "NEXGENE",
                "email": "cristinabyers@nexgene.com",
                "phone": "+1 (939) 471-3755",
                "address": "759 Dobbin Street, Bowden, Alaska, 2272",
                "about": "Aliquip reprehenderit et ut magna culpa et amet sunt quis eu Lorem laborum sit. Cillum cupidatat id cupidatat minim in. Quis do et Lorem qui sunt ut enim sunt incididunt et. Magna do ullamco laboris reprehenderit aliqua aliqua dolor incididunt quis tempor elit laborum magna dolore. Voluptate exercitation voluptate ullamco elit ipsum exercitation nisi sint commodo in adipisicing. Nulla mollit cillum do et cillum dolor nulla ullamco tempor.\r\n",
                "registered": "2015-04-01T07:57:59 -08:00",
                "latitude": 33.299182,
                "longitude": -3.975472,
                "tags": [
                  "commodo",
                  "aliqua",
                  "consequat",
                  "ad",
                  "esse",
                  "anim",
                  "culpa"
                ],
                "friends": [
                  {
                    "id": 0,
                    "name": "Holcomb Clarke"
                  },
                  {
                    "id": 1,
                    "name": "Rivas Copeland"
                  },
                  {
                    "id": 2,
                    "name": "Merritt Graham"
                  }
                ],
                "greeting": "Hello, Cristina Byers! You have 10 unread messages.",
                "favoriteFruit": "banana"
              },
              {
                "_id": "6703e9c3e38ef1140a4e5a22",
                "index": 9,
                "guid": "19175b4e-d12c-4bbb-9554-843a8aaabe1e",
                "isActive": false,
                "balance": "$1,752.65",
                "picture": "http://placehold.it/32x32",
                "age": 29,
                "eyeColor": "blue",
                "name": "Edith Donovan",
                "gender": "female",
                "company": "REPETWIRE",
                "email": "edithdonovan@repetwire.com",
                "phone": "+1 (942) 580-3997",
                "address": "486 Losee Terrace, Coultervillle, Virgin Islands, 4303",
                "about": "Ipsum magna ipsum voluptate ex reprehenderit sint adipisicing in duis nisi ullamco est commodo aliqua. Aliquip pariatur pariatur Lorem reprehenderit Lorem id incididunt do dolor ut do aliqua pariatur laborum. Cupidatat Lorem ea consectetur sit magna excepteur duis culpa mollit cillum excepteur fugiat officia in. Qui occaecat id laborum ea magna commodo ex nulla duis qui et aliquip laboris ad. Proident pariatur enim proident dolore sunt aliqua ipsum reprehenderit ea nisi. Eiusmod exercitation in ipsum et consectetur in sunt incididunt nulla non dolor est non consectetur.\r\n",
                "registered": "2021-10-15T01:24:54 -08:00",
                "latitude": 4.088697,
                "longitude": -83.580961,
                "tags": [
                  "pariatur",
                  "duis",
                  "nisi",
                  "sit",
                  "consequat",
                  "qui",
                  "aliqua"
                ],
                "friends": [
                  {
                    "id": 0,
                    "name": "Price Williamson"
                  },
                  {
                    "id": 1,
                    "name": "Riggs Sawyer"
                  },
                  {
                    "id": 2,
                    "name": "Mendez Ryan"
                  }
                ],
                "greeting": "Hello, Edith Donovan! You have 3 unread messages.",
                "favoriteFruit": "apple"
              },
              {
                "_id": "6703e9c32a033263fcd2200c",
                "index": 10,
                "guid": "b7fa21d4-b51a-4f9f-823e-49951e095a15",
                "isActive": false,
                "balance": "$2,466.09",
                "picture": "http://placehold.it/32x32",
                "age": 25,
                "eyeColor": "blue",
                "name": "Thomas Garza",
                "gender": "male",
                "company": "KIOSK",
                "email": "thomasgarza@kiosk.com",
                "phone": "+1 (965) 459-3453",
                "address": "545 Rockwell Place, Weogufka, Delaware, 6594",
                "about": "Sint laborum qui id adipisicing proident deserunt eiusmod ad consectetur proident pariatur quis ex. Aliqua est nulla ut tempor. Do consectetur eu labore velit magna velit cillum cillum id.\r\n",
                "registered": "2022-08-07T11:21:42 -08:00",
                "latitude": -65.104197,
                "longitude": -137.624921,
                "tags": [
                  "pariatur",
                  "cillum",
                  "magna",
                  "non",
                  "ea",
                  "ea",
                  "occaecat"
                ],
                "friends": [
                  {
                    "id": 0,
                    "name": "Beatriz Mullen"
                  },
                  {
                    "id": 1,
                    "name": "Brown Britt"
                  },
                  {
                    "id": 2,
                    "name": "Mable Haynes"
                  }
                ],
                "greeting": "Hello, Thomas Garza! You have 1 unread messages.",
                "favoriteFruit": "strawberry"
              },
              {
                "_id": "6703e9c3d5e3ff0ae5068da3",
                "index": 11,
                "guid": "b194e333-21d6-4500-9aa6-716540a1e794",
                "isActive": false,
                "balance": "$3,091.16",
                "picture": "http://placehold.it/32x32",
                "age": 34,
                "eyeColor": "brown",
                "name": "Bradley Barrett",
                "gender": "male",
                "company": "ESCHOIR",
                "email": "bradleybarrett@eschoir.com",
                "phone": "+1 (924) 485-2647",
                "address": "190 Tompkins Avenue, Tibbie, South Dakota, 3962",
                "about": "Duis adipisicing sunt sunt minim aliquip ad. Esse officia dolor enim aliquip ad do incididunt laborum commodo officia et irure. Ea culpa magna et tempor. Non eiusmod eu enim ea est dolore. Id duis esse ea do ullamco.\r\n",
                "registered": "2014-04-17T05:07:14 -08:00",
                "latitude": 47.686333,
                "longitude": 41.241145,
                "tags": [
                  "Lorem",
                  "aute",
                  "id",
                  "enim",
                  "elit",
                  "sit",
                  "ex"
                ],
                "friends": [
                  {
                    "id": 0,
                    "name": "Bonita Herrera"
                  },
                  {
                    "id": 1,
                    "name": "Meyers Kirby"
                  },
                  {
                    "id": 2,
                    "name": "Marie Yates"
                  }
                ],
                "greeting": "Hello, Bradley Barrett! You have 8 unread messages.",
                "favoriteFruit": "apple"
              },
              {
                "_id": "6703e9c36de3c2f1b2a65244",
                "index": 12,
                "guid": "5bac2f0d-74df-4eca-a61a-1445417e96e8",
                "isActive": false,
                "balance": "$2,940.30",
                "picture": "http://placehold.it/32x32",
                "age": 21,
                "eyeColor": "blue",
                "name": "Rosalie Mendoza",
                "gender": "female",
                "company": "ASSISTIA",
                "email": "rosaliemendoza@assistia.com",
                "phone": "+1 (860) 418-2730",
                "address": "978 Colonial Road, Whitestone, Montana, 7397",
                "about": "Ex nulla ullamco ut excepteur sit. Est pariatur dolore commodo dolor eiusmod exercitation. Do id consectetur excepteur ipsum do esse quis voluptate do consequat nisi tempor occaecat ex. Reprehenderit proident nisi pariatur nulla duis anim occaecat nulla nostrud elit aute do. Veniam incididunt est proident est eu. Do minim ullamco nisi voluptate labore consequat qui minim fugiat anim in ullamco.\r\n",
                "registered": "2016-02-23T03:00:06 -08:00",
                "latitude": -38.649655,
                "longitude": -3.317956,
                "tags": [
                  "ea",
                  "consequat",
                  "incididunt",
                  "sint",
                  "eiusmod",
                  "occaecat",
                  "non"
                ],
                "friends": [
                  {
                    "id": 0,
                    "name": "Herminia Ratliff"
                  },
                  {
                    "id": 1,
                    "name": "Margret Barron"
                  },
                  {
                    "id": 2,
                    "name": "Hayes Faulkner"
                  }
                ],
                "greeting": "Hello, Rosalie Mendoza! You have 9 unread messages.",
                "favoriteFruit": "strawberry"
              },
              {
                "_id": "6703e9c38fb801af740595ac",
                "index": 13,
                "guid": "6e8f3241-47f1-40ee-bdd1-862b186474d1",
                "isActive": true,
                "balance": "$3,438.41",
                "picture": "http://placehold.it/32x32",
                "age": 37,
                "eyeColor": "brown",
                "name": "Mercado Strong",
                "gender": "male",
                "company": "DIGITALUS",
                "email": "mercadostrong@digitalus.com",
                "phone": "+1 (925) 522-3062",
                "address": "715 Glendale Court, Curtice, Guam, 6278",
                "about": "Qui proident sunt ea proident non ut irure eiusmod eu reprehenderit aliquip laborum sint. Dolore anim officia sunt officia qui amet commodo duis. Commodo velit nostrud mollit cupidatat amet enim aute ullamco. Labore velit excepteur exercitation mollit esse labore officia ullamco Lorem enim mollit dolore esse.\r\n",
                "registered": "2021-12-24T05:13:21 -08:00",
                "latitude": -73.763969,
                "longitude": 134.417916,
                "tags": [
                  "sunt",
                  "aliquip",
                  "sunt",
                  "commodo",
                  "esse",
                  "eu",
                  "laboris"
                ],
                "friends": [
                  {
                    "id": 0,
                    "name": "Helene Williams"
                  },
                  {
                    "id": 1,
                    "name": "Sweeney Rosales"
                  },
                  {
                    "id": 2,
                    "name": "Marlene Garrett"
                  }
                ],
                "greeting": "Hello, Mercado Strong! You have 8 unread messages.",
                "favoriteFruit": "apple"
              },
              {
                "_id": "6703e9c3698d2d0c9a60aa0a",
                "index": 14,
                "guid": "369ff313-de2f-433a-9af2-f0ed619eb914",
                "isActive": false,
                "balance": "$1,071.24",
                "picture": "http://placehold.it/32x32",
                "age": 24,
                "eyeColor": "green",
                "name": "Ballard Baldwin",
                "gender": "male",
                "company": "ZAGGLES",
                "email": "ballardbaldwin@zaggles.com",
                "phone": "+1 (876) 578-3501",
                "address": "358 Ebony Court, Boykin, Pennsylvania, 561",
                "about": "Nostrud elit aliquip ipsum non tempor culpa ea. Cupidatat nisi cupidatat anim quis commodo officia occaecat ea sit excepteur ullamco cupidatat officia. Nostrud proident velit occaecat dolore anim et occaecat quis ad velit. Voluptate qui sunt nostrud magna quis reprehenderit minim. Do magna ad dolor consectetur occaecat mollit. Occaecat ipsum eu consequat occaecat nisi. Deserunt ut esse tempor esse.\r\n",
                "registered": "2016-02-26T02:38:35 -08:00",
                "latitude": 26.142833,
                "longitude": 47.990521,
                "tags": [
                  "sunt",
                  "veniam",
                  "aliqua",
                  "consectetur",
                  "deserunt",
                  "laboris",
                  "culpa"
                ],
                "friends": [
                  {
                    "id": 0,
                    "name": "Constance Spencer"
                  },
                  {
                    "id": 1,
                    "name": "Rosa Finch"
                  },
                  {
                    "id": 2,
                    "name": "Brooks Lambert"
                  }
                ],
                "greeting": "Hello, Ballard Baldwin! You have 9 unread messages.",
                "favoriteFruit": "banana"
              },
              {
                "_id": "6703e9c373ba7cdded1a4fef",
                "index": 15,
                "guid": "7fb72f84-4bd3-435d-bfd8-a27ec48672a4",
                "isActive": true,
                "balance": "$1,418.42",
                "picture": "http://placehold.it/32x32",
                "age": 40,
                "eyeColor": "green",
                "name": "Summer Vincent",
                "gender": "female",
                "company": "CORMORAN",
                "email": "summervincent@cormoran.com",
                "phone": "+1 (964) 595-2441",
                "address": "550 Drew Street, Osage, New Hampshire, 2655",
                "about": "Nostrud labore ullamco ea sunt nisi. Ipsum exercitation mollit aliqua aliquip ad. Do esse aliquip laboris dolore.\r\n",
                "registered": "2015-04-30T12:33:03 -08:00",
                "latitude": 32.53337,
                "longitude": 4.614609,
                "tags": [
                  "proident",
                  "laborum",
                  "nisi",
                  "aliquip",
                  "ut",
                  "duis",
                  "culpa"
                ],
                "friends": [
                  {
                    "id": 0,
                    "name": "Conrad English"
                  },
                  {
                    "id": 1,
                    "name": "Michelle Estrada"
                  },
                  {
                    "id": 2,
                    "name": "Pate Mccormick"
                  }
                ],
                "greeting": "Hello, Summer Vincent! You have 6 unread messages.",
                "favoriteFruit": "banana"
              },
              {
                "_id": "6703e9c3a4f2d088aa5d91bc",
                "index": 16,
                "guid": "8bbc6b99-2e9d-4078-b013-58be14618c4c",
                "isActive": false,
                "balance": "$2,692.68",
                "picture": "http://placehold.it/32x32",
                "age": 23,
                "eyeColor": "brown",
                "name": "Russell Albert",
                "gender": "male",
                "company": "OCEANICA",
                "email": "russellalbert@oceanica.com",
                "phone": "+1 (908) 447-2771",
                "address": "834 Box Street, Blue, Florida, 8334",
                "about": "Ad anim minim officia quis qui cupidatat incididunt. Non est mollit eiusmod est qui. Duis consequat qui proident cillum sunt duis anim anim labore minim quis.\r\n",
                "registered": "2021-02-28T11:08:09 -08:00",
                "latitude": 37.905982,
                "longitude": -86.243305,
                "tags": [
                  "officia",
                  "laborum",
                  "nisi",
                  "aliquip",
                  "magna",
                  "cupidatat",
                  "esse"
                ],
                "friends": [
                  {
                    "id": 0,
                    "name": "Suzette Cabrera"
                  },
                  {
                    "id": 1,
                    "name": "Vazquez Hayes"
                  },
                  {
                    "id": 2,
                    "name": "Angel Hardin"
                  }
                ],
                "greeting": "Hello, Russell Albert! You have 2 unread messages.",
                "favoriteFruit": "banana"
              },
              {
                "_id": "6703e9c35bcc7a2f53106152",
                "index": 17,
                "guid": "ea3c3b7a-44e3-489e-a4db-6dbcf7ba87bc",
                "isActive": true,
                "balance": "$3,436.31",
                "picture": "http://placehold.it/32x32",
                "age": 22,
                "eyeColor": "brown",
                "name": "Stevens Kidd",
                "gender": "male",
                "company": "PHORMULA",
                "email": "stevenskidd@phormula.com",
                "phone": "+1 (821) 413-2463",
                "address": "729 Adams Street, Lumberton, Mississippi, 1873",
                "about": "Veniam velit consequat velit amet ea do commodo consequat enim exercitation labore. Lorem ad id laborum adipisicing non amet quis. Nostrud commodo amet elit incididunt non esse cillum laboris in mollit dolore tempor labore. Sunt aliquip ea ullamco quis aliqua quis eiusmod amet excepteur fugiat magna amet.\r\n",
                "registered": "2020-11-02T12:15:38 -08:00",
                "latitude": 53.319597,
                "longitude": -32.594331,
                "tags": [
                  "tempor",
                  "adipisicing",
                  "anim",
                  "voluptate",
                  "excepteur",
                  "cillum",
                  "laboris"
                ],
                "friends": [
                  {
                    "id": 0,
                    "name": "Wright Peters"
                  },
                  {
                    "id": 1,
                    "name": "Chen Fernandez"
                  },
                  {
                    "id": 2,
                    "name": "Lydia Moreno"
                  }
                ],
                "greeting": "Hello, Stevens Kidd! You have 10 unread messages.",
                "favoriteFruit": "strawberry"
              },
              {
                "_id": "6703e9c325340b997fbddef2",
                "index": 18,
                "guid": "e17de40d-0e44-472e-bb20-6a082502f442",
                "isActive": false,
                "balance": "$2,361.26",
                "picture": "http://placehold.it/32x32",
                "age": 24,
                "eyeColor": "green",
                "name": "Roy Walker",
                "gender": "male",
                "company": "EXTRAWEAR",
                "email": "roywalker@extrawear.com",
                "phone": "+1 (983) 401-2093",
                "address": "329 Truxton Street, Newkirk, North Dakota, 4729",
                "about": "Aliquip aliqua proident proident Lorem adipisicing magna mollit veniam reprehenderit ex tempor. Et duis amet laborum sint proident Lorem ut fugiat cupidatat. Magna officia exercitation et et sit quis non veniam ad.\r\n",
                "registered": "2023-03-15T09:49:35 -08:00",
                "latitude": -22.943884,
                "longitude": 96.248013,
                "tags": [
                  "consectetur",
                  "eu",
                  "nulla",
                  "ad",
                  "eu",
                  "ex",
                  "anim"
                ],
                "friends": [
                  {
                    "id": 0,
                    "name": "Tammie Pennington"
                  },
                  {
                    "id": 1,
                    "name": "Gilmore Bowman"
                  },
                  {
                    "id": 2,
                    "name": "Felicia Flynn"
                  }
                ],
                "greeting": "Hello, Roy Walker! You have 8 unread messages.",
                "favoriteFruit": "strawberry"
              },
              {
                "_id": "6703e9c326123aa80e2e3125",
                "index": 19,
                "guid": "8cd8ec42-525c-410c-b39d-ddfcb4e8be73",
                "isActive": false,
                "balance": "$2,265.91",
                "picture": "http://placehold.it/32x32",
                "age": 32,
                "eyeColor": "green",
                "name": "Oliver Battle",
                "gender": "male",
                "company": "PARAGONIA",
                "email": "oliverbattle@paragonia.com",
                "phone": "+1 (997) 568-3558",
                "address": "769 Batchelder Street, Slovan, West Virginia, 681",
                "about": "Nulla enim ex amet cupidatat et irure. Ipsum ad in velit veniam Lorem enim incididunt elit. Adipisicing nostrud pariatur id deserunt cillum ullamco velit sit anim aliqua qui incididunt qui. Fugiat nostrud amet ea amet labore ea et sit aute dolore. Est ut reprehenderit ullamco laboris pariatur elit. Nostrud culpa do ipsum mollit cillum mollit sunt culpa eu. Eu labore nostrud fugiat aliquip.\r\n",
                "registered": "2023-05-03T07:40:54 -08:00",
                "latitude": 85.539727,
                "longitude": -110.413625,
                "tags": [
                  "labore",
                  "nulla",
                  "irure",
                  "laborum",
                  "sunt",
                  "commodo",
                  "aliqua"
                ],
                "friends": [
                  {
                    "id": 0,
                    "name": "Connie Ayers"
                  },
                  {
                    "id": 1,
                    "name": "Rocha Schroeder"
                  },
                  {
                    "id": 2,
                    "name": "Mathews Lee"
                  }
                ],
                "greeting": "Hello, Oliver Battle! You have 6 unread messages.",
                "favoriteFruit": "strawberry"
              },
              {
                "_id": "6703e9c30102239ee52ee68c",
                "index": 20,
                "guid": "8de64c2a-d047-4933-92c9-a524d2f5af1c",
                "isActive": true,
                "balance": "$2,643.26",
                "picture": "http://placehold.it/32x32",
                "age": 34,
                "eyeColor": "brown",
                "name": "Hartman Stark",
                "gender": "male",
                "company": "PHEAST",
                "email": "hartmanstark@pheast.com",
                "phone": "+1 (956) 541-2870",
                "address": "889 Woodruff Avenue, Garnet, Vermont, 6266",
                "about": "Excepteur id ipsum pariatur mollit mollit. Culpa esse et officia adipisicing consectetur ut non elit amet aliquip sit cillum officia magna. Reprehenderit quis ullamco laborum nulla aliqua elit.\r\n",
                "registered": "2022-02-17T02:46:42 -08:00",
                "latitude": -21.209452,
                "longitude": -38.524557,
                "tags": [
                  "officia",
                  "pariatur",
                  "ex",
                  "aliquip",
                  "ipsum",
                  "voluptate",
                  "fugiat"
                ],
                "friends": [
                  {
                    "id": 0,
                    "name": "Vicki Bean"
                  },
                  {
                    "id": 1,
                    "name": "Wiggins Conrad"
                  },
                  {
                    "id": 2,
                    "name": "Hobbs Morales"
                  }
                ],
                "greeting": "Hello, Hartman Stark! You have 2 unread messages.",
                "favoriteFruit": "banana"
              },
              {
                "_id": "6703e9c3dc4a9b2aa451382a",
                "index": 21,
                "guid": "8e276137-acbe-4646-a2fb-5edfe939616a",
                "isActive": false,
                "balance": "$2,325.22",
                "picture": "http://placehold.it/32x32",
                "age": 32,
                "eyeColor": "green",
                "name": "Tonia Rutledge",
                "gender": "female",
                "company": "FIBRODYNE",
                "email": "toniarutledge@fibrodyne.com",
                "phone": "+1 (893) 499-2358",
                "address": "118 Coventry Road, Lloyd, Illinois, 573",
                "about": "Voluptate irure reprehenderit velit ex. Aute proident labore exercitation aliqua eu fugiat dolore velit cupidatat. In aliquip nostrud tempor adipisicing magna do et mollit incididunt sint commodo nisi. Ut aute voluptate duis est cupidatat dolor deserunt quis labore. Reprehenderit non magna do amet officia ad mollit eiusmod elit veniam minim.\r\n",
                "registered": "2022-10-21T12:26:38 -08:00",
                "latitude": 34.302251,
                "longitude": -69.698074,
                "tags": [
                  "ipsum",
                  "reprehenderit",
                  "dolor",
                  "dolor",
                  "non",
                  "occaecat",
                  "ad"
                ],
                "friends": [
                  {
                    "id": 0,
                    "name": "Woodard Kline"
                  },
                  {
                    "id": 1,
                    "name": "Lynch Greer"
                  },
                  {
                    "id": 2,
                    "name": "Dean Potts"
                  }
                ],
                "greeting": "Hello, Tonia Rutledge! You have 5 unread messages.",
                "favoriteFruit": "strawberry"
              },
              {
                "_id": "6703e9c3863ecde83ac380ad",
                "index": 22,
                "guid": "6786ab4e-50fb-4fb1-aad4-87d717d113b3",
                "isActive": true,
                "balance": "$3,945.68",
                "picture": "http://placehold.it/32x32",
                "age": 26,
                "eyeColor": "brown",
                "name": "England Franklin",
                "gender": "male",
                "company": "VIASIA",
                "email": "englandfranklin@viasia.com",
                "phone": "+1 (944) 458-2648",
                "address": "912 Louis Place, Deseret, Virginia, 8855",
                "about": "Consectetur aliqua ad ullamco elit adipisicing do consequat amet voluptate fugiat adipisicing labore. Occaecat anim nisi irure ex occaecat pariatur ullamco ipsum aute deserunt aute. Nostrud commodo pariatur ea tempor incididunt tempor eu magna enim ut anim officia velit elit. Aute magna magna et voluptate enim pariatur veniam anim qui sint cillum in aliqua exercitation. Pariatur non incididunt ipsum voluptate magna exercitation enim ea voluptate pariatur laborum nulla cupidatat. Quis dolor id id sint velit. Non est quis ad consequat commodo velit pariatur incididunt irure culpa pariatur.\r\n",
                "registered": "2021-04-15T09:43:47 -08:00",
                "latitude": 34.489235,
                "longitude": -131.150656,
                "tags": [
                  "do",
                  "amet",
                  "fugiat",
                  "Lorem",
                  "do",
                  "irure",
                  "ipsum"
                ],
                "friends": [
                  {
                    "id": 0,
                    "name": "Matthews Miranda"
                  },
                  {
                    "id": 1,
                    "name": "Luann Hammond"
                  },
                  {
                    "id": 2,
                    "name": "Ofelia Kemp"
                  }
                ],
                "greeting": "Hello, England Franklin! You have 10 unread messages.",
                "favoriteFruit": "banana"
              },
              {
                "_id": "6703e9c33e2ee64b3a40cd83",
                "index": 23,
                "guid": "e2fd82c0-d400-4fac-bdfc-69e5778375f3",
                "isActive": true,
                "balance": "$1,808.63",
                "picture": "http://placehold.it/32x32",
                "age": 38,
                "eyeColor": "green",
                "name": "Leticia Martin",
                "gender": "female",
                "company": "JAMNATION",
                "email": "leticiamartin@jamnation.com",
                "phone": "+1 (890) 439-2149",
                "address": "893 Lincoln Terrace, Islandia, Indiana, 3555",
                "about": "Magna tempor exercitation esse ut deserunt dolor ullamco exercitation aute officia ea elit aliqua. Ipsum cupidatat laborum laborum do sit consequat elit ipsum elit in fugiat adipisicing officia. Consectetur cupidatat id duis laboris sit dolore mollit. Magna ullamco excepteur aliqua Lorem veniam ipsum.\r\n",
                "registered": "2023-04-14T06:38:57 -08:00",
                "latitude": -18.144711,
                "longitude": -42.413027,
                "tags": [
                  "cupidatat",
                  "deserunt",
                  "incididunt",
                  "fugiat",
                  "aute",
                  "ad",
                  "sunt"
                ],
                "friends": [
                  {
                    "id": 0,
                    "name": "Lena Hays"
                  },
                  {
                    "id": 1,
                    "name": "Abbott Sanchez"
                  },
                  {
                    "id": 2,
                    "name": "Carlene Savage"
                  }
                ],
                "greeting": "Hello, Leticia Martin! You have 9 unread messages.",
                "favoriteFruit": "banana"
              },
              {
                "_id": "6703e9c347411cc47e13f8b2",
                "index": 24,
                "guid": "4973e673-80e7-4a68-a639-8672ac892356",
                "isActive": false,
                "balance": "$1,984.59",
                "picture": "http://placehold.it/32x32",
                "age": 30,
                "eyeColor": "brown",
                "name": "Fitzpatrick Lowe",
                "gender": "male",
                "company": "BILLMED",
                "email": "fitzpatricklowe@billmed.com",
                "phone": "+1 (961) 408-2846",
                "address": "971 Knight Court, Trucksville, Northern Mariana Islands, 5367",
                "about": "Do cupidatat sit Lorem in eu ad pariatur nostrud irure cillum. Eiusmod id qui dolor do consectetur. Culpa aliquip duis veniam in velit voluptate cillum qui enim. Magna anim ipsum fugiat cupidatat consequat pariatur voluptate consectetur qui. Mollit qui laborum ullamco amet nulla culpa officia laborum. Qui deserunt nostrud mollit est occaecat dolore ad ullamco laboris culpa officia magna sint. Adipisicing veniam fugiat ea occaecat irure exercitation labore.\r\n",
                "registered": "2024-03-28T08:48:07 -08:00",
                "latitude": 4.30593,
                "longitude": 129.470558,
                "tags": [
                  "laborum",
                  "consectetur",
                  "sit",
                  "ea",
                  "commodo",
                  "duis",
                  "aliquip"
                ],
                "friends": [
                  {
                    "id": 0,
                    "name": "Lindsay Maldonado"
                  },
                  {
                    "id": 1,
                    "name": "Dolly Key"
                  },
                  {
                    "id": 2,
                    "name": "Avis Walsh"
                  }
                ],
                "greeting": "Hello, Fitzpatrick Lowe! You have 8 unread messages.",
                "favoriteFruit": "apple"
              },
              {
                "_id": "6703e9c3102fff92ca9baaa3",
                "index": 25,
                "guid": "4a535d96-0dd4-441e-a454-e8390e9bd54f",
                "isActive": false,
                "balance": "$2,435.72",
                "picture": "http://placehold.it/32x32",
                "age": 39,
                "eyeColor": "blue",
                "name": "Roslyn Owens",
                "gender": "female",
                "company": "KOZGENE",
                "email": "roslynowens@kozgene.com",
                "phone": "+1 (886) 401-2919",
                "address": "161 Noel Avenue, Cornfields, Missouri, 8269",
                "about": "Sit mollit dolore eu ea consequat tempor ex aute irure laborum aliquip exercitation. Ipsum esse cillum voluptate sunt velit excepteur ad ut exercitation labore Lorem veniam. Ea labore est nisi veniam aliqua proident nostrud elit ea sint reprehenderit non veniam. Fugiat adipisicing ut do occaecat tempor qui commodo id mollit nulla laboris in velit laboris. Exercitation ut magna non ad mollit et aute pariatur culpa tempor. Excepteur enim aliqua consequat aliqua nostrud do et duis non officia. Aliquip deserunt exercitation occaecat deserunt dolore eiusmod sit duis.\r\n",
                "registered": "2022-11-26T03:38:50 -08:00",
                "latitude": -35.709025,
                "longitude": -113.778763,
                "tags": [
                  "ex",
                  "pariatur",
                  "voluptate",
                  "fugiat",
                  "qui",
                  "occaecat",
                  "est"
                ],
                "friends": [
                  {
                    "id": 0,
                    "name": "Janette Wolfe"
                  },
                  {
                    "id": 1,
                    "name": "Nadia Kelley"
                  },
                  {
                    "id": 2,
                    "name": "Karyn Perez"
                  }
                ],
                "greeting": "Hello, Roslyn Owens! You have 2 unread messages.",
                "favoriteFruit": "apple"
              },
              {
                "_id": "6703e9c379a7a3c3643fe9bb",
                "index": 26,
                "guid": "a8f1dbf9-bf12-457a-afcd-7cbaedbfd6aa",
                "isActive": true,
                "balance": "$1,642.20",
                "picture": "http://placehold.it/32x32",
                "age": 29,
                "eyeColor": "brown",
                "name": "Fitzgerald Hodges",
                "gender": "male",
                "company": "ZYTRAC",
                "email": "fitzgeraldhodges@zytrac.com",
                "phone": "+1 (967) 423-2179",
                "address": "546 Bay Avenue, Sandston, American Samoa, 1474",
                "about": "Dolor adipisicing minim id aliqua consectetur irure voluptate dolor esse magna adipisicing consectetur consectetur. Eu nulla ut nisi sint duis amet pariatur duis cupidatat esse Lorem qui commodo aliquip. Reprehenderit in eiusmod excepteur ex.\r\n",
                "registered": "2023-10-08T11:53:03 -08:00",
                "latitude": -84.017762,
                "longitude": 80.381227,
                "tags": [
                  "ea",
                  "fugiat",
                  "veniam",
                  "fugiat",
                  "nulla",
                  "anim",
                  "ex"
                ],
                "friends": [
                  {
                    "id": 0,
                    "name": "Lowery Jacobson"
                  },
                  {
                    "id": 1,
                    "name": "Bond Crane"
                  },
                  {
                    "id": 2,
                    "name": "Luz Watkins"
                  }
                ],
                "greeting": "Hello, Fitzgerald Hodges! You have 5 unread messages.",
                "favoriteFruit": "apple"
              },
              {
                "_id": "6703e9c31ea7370e239c1c61",
                "index": 27,
                "guid": "875cc8cb-e7ee-48c5-be2c-201f699b4484",
                "isActive": false,
                "balance": "$2,552.62",
                "picture": "http://placehold.it/32x32",
                "age": 38,
                "eyeColor": "blue",
                "name": "Effie Conner",
                "gender": "female",
                "company": "COMVENE",
                "email": "effieconner@comvene.com",
                "phone": "+1 (879) 445-2334",
                "address": "931 Seigel Street, Rockhill, Minnesota, 781",
                "about": "Magna velit dolor fugiat ut laboris elit. Culpa non pariatur dolore laboris amet voluptate commodo ullamco esse. In ex amet amet magna sunt fugiat labore ut. Excepteur voluptate incididunt enim aliqua amet cupidatat voluptate qui adipisicing magna laboris reprehenderit nulla ex. Ex proident esse cupidatat fugiat laborum do nisi officia in incididunt dolore. Enim non nulla esse deserunt elit id. Eiusmod laborum in eu laborum.\r\n",
                "registered": "2019-11-03T12:16:20 -08:00",
                "latitude": 73.533404,
                "longitude": -165.443528,
                "tags": [
                  "consectetur",
                  "incididunt",
                  "veniam",
                  "ad",
                  "duis",
                  "non",
                  "incididunt"
                ],
                "friends": [
                  {
                    "id": 0,
                    "name": "Young House"
                  },
                  {
                    "id": 1,
                    "name": "Zamora Ochoa"
                  },
                  {
                    "id": 2,
                    "name": "Joanna Burgess"
                  }
                ],
                "greeting": "Hello, Effie Conner! You have 1 unread messages.",
                "favoriteFruit": "strawberry"
              },
              {
                "_id": "6703e9c395546bd01c5700ea",
                "index": 28,
                "guid": "b1c56a91-982a-463a-98c0-43a7e65b382b",
                "isActive": false,
                "balance": "$3,309.37",
                "picture": "http://placehold.it/32x32",
                "age": 21,
                "eyeColor": "brown",
                "name": "Kathleen Nguyen",
                "gender": "female",
                "company": "HIVEDOM",
                "email": "kathleennguyen@hivedom.com",
                "phone": "+1 (866) 584-2059",
                "address": "909 Berriman Street, Carrizo, North Carolina, 3527",
                "about": "Aute ut nisi aute consectetur non aliquip commodo consequat deserunt tempor ad ut ullamco. Nostrud eu in non non veniam sit. Eiusmod nostrud veniam adipisicing adipisicing ex reprehenderit. Aliquip ut veniam nisi dolor excepteur.\r\n",
                "registered": "2020-11-23T08:46:07 -08:00",
                "latitude": -13.090967,
                "longitude": -11.764396,
                "tags": [
                  "anim",
                  "sint",
                  "fugiat",
                  "ipsum",
                  "id",
                  "amet",
                  "elit"
                ],
                "friends": [
                  {
                    "id": 0,
                    "name": "Horne Logan"
                  },
                  {
                    "id": 1,
                    "name": "Emily Bryan"
                  },
                  {
                    "id": 2,
                    "name": "Gregory Bowen"
                  }
                ],
                "greeting": "Hello, Kathleen Nguyen! You have 5 unread messages.",
                "favoriteFruit": "apple"
              },
              {
                "_id": "6703e9c30b0021c191cb6fc7",
                "index": 29,
                "guid": "a392bc8a-c91e-4c0a-b505-82b13cfb7b5a",
                "isActive": false,
                "balance": "$2,943.61",
                "picture": "http://placehold.it/32x32",
                "age": 21,
                "eyeColor": "blue",
                "name": "Whitfield Mckenzie",
                "gender": "male",
                "company": "PARLEYNET",
                "email": "whitfieldmckenzie@parleynet.com",
                "phone": "+1 (854) 525-3351",
                "address": "524 Balfour Place, Fillmore, Michigan, 8487",
                "about": "Lorem non minim ipsum reprehenderit adipisicing. Ea ad ut tempor sint cillum laboris nisi Lorem aute dolore. Cillum quis nisi dolor ullamco exercitation proident cillum deserunt aute. Lorem occaecat culpa reprehenderit irure in id eiusmod ullamco consectetur. Lorem deserunt nisi excepteur eu tempor laboris duis sint proident. Laboris anim quis id sunt consequat irure pariatur cillum anim nostrud laboris magna. Ad do consequat deserunt non sunt pariatur ullamco voluptate.\r\n",
                "registered": "2020-01-09T09:45:01 -08:00",
                "latitude": 17.606318,
                "longitude": 63.645055,
                "tags": [
                  "pariatur",
                  "ad",
                  "laboris",
                  "qui",
                  "magna",
                  "est",
                  "exercitation"
                ],
                "friends": [
                  {
                    "id": 0,
                    "name": "Leta Bray"
                  },
                  {
                    "id": 1,
                    "name": "Robyn Atkinson"
                  },
                  {
                    "id": 2,
                    "name": "Hendrix Lyons"
                  }
                ],
                "greeting": "Hello, Whitfield Mckenzie! You have 10 unread messages.",
                "favoriteFruit": "apple"
              }
            ]


            """;

    }
}
