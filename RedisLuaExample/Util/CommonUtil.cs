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

        public static string GetFormalJson = """
{
  "Data": {
  "Level": 0,
  "IsReadOnly": true,
  "IsAllowPause": true,
  "IsAllowSell": false,
  "IsAllowOuterSell": true,
  "HoldSellMoney": 0,
  "HandicapIds": [
    1,
    2,
    3,
    4,
    5,
    6,
    7,
    8,
    9
  ],
  "ProxyList": [
    {
      "Key": 2001,
      "Value": "company1"
    },
    {
      "Key": 2002,
      "Value": "company2"
    },
    {
      "Key": 2013,
      "Value": "mimir0"
    },
    {
      "Key": 2016,
      "Value": "rickco2"
    },
    {
      "Key": 2018,
      "Value": "rockycom1"
    },
    {
      "Key": 2019,
      "Value": "james99"
    },
    {
      "Key": 2026,
      "Value": "rickyco1"
    },
    {
      "Key": 2036,
      "Value": "blueco1"
    },
    {
      "Key": 2048,
      "Value": "henrycom33"
    },
    {
      "Key": 2049,
      "Value": "henrycom88"
    },
    {
      "Key": 2056,
      "Value": "samt1"
    },
    {
      "Key": 2057,
      "Value": "samt2"
    },
    {
      "Key": 2062,
      "Value": "maxcom01"
    },
    {
      "Key": 2068,
      "Value": "peterchiu11"
    },
    {
      "Key": 2072,
      "Value": "ppp001"
    },
    {
      "Key": 2074,
      "Value": "joy01"
    },
    {
      "Key": 2075,
      "Value": "rockycom2"
    },
    {
      "Key": 2101,
      "Value": "rockycomp3"
    },
    {
      "Key": 2107,
      "Value": "sc000"
    },
    {
      "Key": 2115,
      "Value": "neilc01"
    },
    {
      "Key": 2118,
      "Value": "zc001"
    },
    {
      "Key": 2138,
      "Value": "peterchiu12"
    },
    {
      "Key": 2139,
      "Value": "joy02"
    },
    {
      "Key": 2151,
      "Value": "bluev2co1"
    },
    {
      "Key": 2157,
      "Value": "rex_lv1"
    },
    {
      "Key": 2161,
      "Value": "petertest1"
    },
    {
      "Key": 2167,
      "Value": "louistestc1"
    },
    {
      "Key": 2169,
      "Value": "companyv2"
    },
    {
      "Key": 2195,
      "Value": "xer_lv1"
    },
    {
      "Key": 2199,
      "Value": "louistest04"
    },
    {
      "Key": 2200,
      "Value": "louistest005"
    },
    {
      "Key": 2201,
      "Value": "nbackend01"
    },
    {
      "Key": 2203,
      "Value": "louistest006"
    },
    {
      "Key": 2204,
      "Value": "louistest007"
    },
    {
      "Key": 2206,
      "Value": "louistest008"
    },
    {
      "Key": 2207,
      "Value": "louistest009"
    },
    {
      "Key": 2208,
      "Value": "louistest010"
    },
    {
      "Key": 2209,
      "Value": "rickco3333"
    },
    {
      "Key": 2210,
      "Value": "rickco444"
    },
    {
      "Key": 2211,
      "Value": "rickco2221"
    },
    {
      "Key": 2212,
      "Value": "samt9"
    },
    {
      "Key": 2215,
      "Value": "joy03"
    },
    {
      "Key": 2750,
      "Value": "joy04"
    },
    {
      "Key": 2752,
      "Value": "peterchiu999"
    },
    {
      "Key": 2753,
      "Value": "jtest099"
    },
    {
      "Key": 2754,
      "Value": "rickco222"
    },
    {
      "Key": 2777,
      "Value": "joy05"
    },
    {
      "Key": 2788,
      "Value": "peterchiu13"
    },
    {
      "Key": 2847,
      "Value": "maxcom02"
    },
    {
      "Key": 2855,
      "Value": "rickccc1"
    },
    {
      "Key": 2866,
      "Value": "louiscutest1"
    },
    {
      "Key": 2867,
      "Value": "louiscutest2"
    },
    {
      "Key": 2869,
      "Value": "louistest999"
    },
    {
      "Key": 2871,
      "Value": "jamesnew99"
    },
    {
      "Key": 2876,
      "Value": "evancomp011"
    },
    {
      "Key": 2892,
      "Value": "evancomp201"
    },
    {
      "Key": 2894,
      "Value": "sc000_c1"
    },
    {
      "Key": 2903,
      "Value": "neilc02"
    },
    {
      "Key": 2907,
      "Value": "rickmcom1"
    },
    {
      "Key": 2927,
      "Value": "rickcom41"
    },
    {
      "Key": 2928,
      "Value": "rickcom411"
    },
    {
      "Key": 2929,
      "Value": "neilc03"
    },
    {
      "Key": 2942,
      "Value": "sc000_c2"
    },
    {
      "Key": 2950,
      "Value": "rockycom4"
    },
    {
      "Key": 2963,
      "Value": "rickcu2"
    },
    {
      "Key": 2974,
      "Value": "ricocom4"
    },
    {
      "Key": 2978,
      "Value": "rickcom21"
    },
    {
      "Key": 3028,
      "Value": "neilc05"
    },
    {
      "Key": 3065,
      "Value": "joy06"
    },
    {
      "Key": 3080,
      "Value": "tommy1"
    },
    {
      "Key": 3214,
      "Value": "marcus_co1"
    },
    {
      "Key": 3217,
      "Value": "t1company"
    },
    {
      "Key": 3233,
      "Value": "test0001"
    },
    {
      "Key": 3234,
      "Value": "test0002"
    },
    {
      "Key": 3274,
      "Value": "tt888"
    },
    {
      "Key": 3280,
      "Value": "tt999"
    },
    {
      "Key": 3281,
      "Value": "tt777"
    },
    {
      "Key": 3282,
      "Value": "tt666"
    },
    {
      "Key": 3283,
      "Value": "tt555"
    },
    {
      "Key": 3284,
      "Value": "tt444"
    },
    {
      "Key": 3285,
      "Value": "tt333"
    },
    {
      "Key": 3286,
      "Value": "tt222"
    },
    {
      "Key": 3290,
      "Value": "tt111"
    },
    {
      "Key": 3291,
      "Value": "hank01"
    },
    {
      "Key": 3295,
      "Value": "louistest011"
    },
    {
      "Key": 3296,
      "Value": "louistest012"
    },
    {
      "Key": 3297,
      "Value": "louistest013"
    },
    {
      "Key": 3298,
      "Value": "louistest014"
    }
  ],
  "MainData": {
    "LastDrawPeriodNo": 20240926270,
    "CurrentPeriod": {
      "PeriodNo": 20240930274,
      "PeriodStatus": 2,
      "IsOpened": false,
      "IsClosed": true,
      "IsDrawed": false,
      "IsSettled": false,
      "StopCountDown": 0,
      "CloseCountDown": 0,
      "DrawCountDown": 0
    },
    "SumMoney": [
      [
        0,
        0
      ],
      [
        0,
        0
      ],
      [
        0,
        0
      ],
      [
        0,
        0
      ],
      [
        0,
        0
      ],
      [
        0,
        0
      ],
      [
        0,
        0
      ],
      [
        0,
        0
      ],
      [
        0
      ],
      [
        0
      ],
      [
        0
      ],
      [
        0
      ],
      [
        0
      ],
      [
        0
      ],
      [
        0
      ],
      [
        0
      ],
      [
        0
      ],
      [
        0
      ]
    ],
    "TotalStatData": [
      {
        "BetNo": 1,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 2,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 3,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 4,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 5,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 6,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 7,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 8,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 9,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 10,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 11,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 12,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 13,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 14,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 15,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 16,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 17,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 18,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 19,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 20,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 21,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 22,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 23,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 24,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 25,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 26,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 27,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 28,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 29,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 30,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 31,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 32,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 33,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 34,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 35,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 36,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 37,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 38,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 39,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 40,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 41,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 42,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 43,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 44,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 45,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 46,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 47,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 48,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      },
      {
        "BetNo": 49,
        "HoldMoney": 0,
        "MaxLose": 0,
        "SellMoney": 0
      }
    ],
    "OddsData": [
      {
        "BetTypeId": 1711,
        "BetItemId": 1,
        "BetNo": 171101,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 2,
        "BetNo": 171102,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 3,
        "BetNo": 171103,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 4,
        "BetNo": 171104,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 5,
        "BetNo": 171105,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 6,
        "BetNo": 171106,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 7,
        "BetNo": 171107,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 8,
        "BetNo": 171108,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 9,
        "BetNo": 171109,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 10,
        "BetNo": 171110,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 11,
        "BetNo": 171111,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 12,
        "BetNo": 171112,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 13,
        "BetNo": 171113,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 14,
        "BetNo": 171114,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 15,
        "BetNo": 171115,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 16,
        "BetNo": 171116,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 17,
        "BetNo": 171117,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 18,
        "BetNo": 171118,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 19,
        "BetNo": 171119,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 20,
        "BetNo": 171120,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 21,
        "BetNo": 171121,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 22,
        "BetNo": 171122,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 23,
        "BetNo": 171123,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 24,
        "BetNo": 171124,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 25,
        "BetNo": 171125,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 26,
        "BetNo": 171126,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 27,
        "BetNo": 171127,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 28,
        "BetNo": 171128,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 29,
        "BetNo": 171129,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 30,
        "BetNo": 171130,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 31,
        "BetNo": 171131,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 32,
        "BetNo": 171132,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 33,
        "BetNo": 171133,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 34,
        "BetNo": 171134,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 35,
        "BetNo": 171135,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 36,
        "BetNo": 171136,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 37,
        "BetNo": 171137,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 38,
        "BetNo": 171138,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 39,
        "BetNo": 171139,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 40,
        "BetNo": 171140,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 41,
        "BetNo": 171141,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 42,
        "BetNo": 171142,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 43,
        "BetNo": 171143,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 44,
        "BetNo": 171144,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 45,
        "BetNo": 171145,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 46,
        "BetNo": 171146,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 47,
        "BetNo": 171147,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 48,
        "BetNo": 171148,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1711,
        "BetItemId": 49,
        "BetNo": 171149,
        "IsPaused": false,
        "Odds1": 42.45,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1720,
        "BetItemId": 1,
        "BetNo": 172001,
        "IsPaused": false,
        "Odds1": 1.989,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1720,
        "BetItemId": 2,
        "BetNo": 172002,
        "IsPaused": false,
        "Odds1": 1.989,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1730,
        "BetItemId": 1,
        "BetNo": 173001,
        "IsPaused": false,
        "Odds1": 1.989,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1730,
        "BetItemId": 2,
        "BetNo": 173002,
        "IsPaused": false,
        "Odds1": 1.989,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1740,
        "BetItemId": 1,
        "BetNo": 174001,
        "IsPaused": false,
        "Odds1": 1.989,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1740,
        "BetItemId": 2,
        "BetNo": 174002,
        "IsPaused": false,
        "Odds1": 1.989,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1750,
        "BetItemId": 1,
        "BetNo": 175001,
        "IsPaused": false,
        "Odds1": 2.857,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1750,
        "BetItemId": 2,
        "BetNo": 175002,
        "IsPaused": false,
        "Odds1": 3.041,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1750,
        "BetItemId": 3,
        "BetNo": 175003,
        "IsPaused": false,
        "Odds1": 3.041,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1760,
        "BetItemId": 1,
        "BetNo": 176001,
        "IsPaused": false,
        "Odds1": 1.989,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1760,
        "BetItemId": 2,
        "BetNo": 176002,
        "IsPaused": false,
        "Odds1": 1.989,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1770,
        "BetItemId": 1,
        "BetNo": 177001,
        "IsPaused": false,
        "Odds1": 12.16,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1770,
        "BetItemId": 2,
        "BetNo": 177002,
        "IsPaused": false,
        "Odds1": 12.16,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1770,
        "BetItemId": 3,
        "BetNo": 177003,
        "IsPaused": false,
        "Odds1": 12.16,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1770,
        "BetItemId": 4,
        "BetNo": 177004,
        "IsPaused": false,
        "Odds1": 12.16,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1770,
        "BetItemId": 5,
        "BetNo": 177005,
        "IsPaused": false,
        "Odds1": 9.72,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1770,
        "BetItemId": 6,
        "BetNo": 177006,
        "IsPaused": false,
        "Odds1": 12.16,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1770,
        "BetItemId": 7,
        "BetNo": 177007,
        "IsPaused": false,
        "Odds1": 12.16,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1770,
        "BetItemId": 8,
        "BetNo": 177008,
        "IsPaused": false,
        "Odds1": 12.16,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1770,
        "BetItemId": 9,
        "BetNo": 177009,
        "IsPaused": false,
        "Odds1": 12.16,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1770,
        "BetItemId": 10,
        "BetNo": 177010,
        "IsPaused": false,
        "Odds1": 12.16,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1770,
        "BetItemId": 11,
        "BetNo": 177011,
        "IsPaused": false,
        "Odds1": 12.16,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      },
      {
        "BetTypeId": 1770,
        "BetItemId": 12,
        "BetNo": 177012,
        "IsPaused": false,
        "Odds1": 12.16,
        "Odds2": null,
        "HoldMoney": 0,
        "MaxLoss": 0,
        "CompanyOdds1": 0,
        "CompanyOdds2": null,
        "SellMoney": 0,
        "LimitMoney": 5000
      }
    ],
    "PausedStatus": {
      "IsPausedSpecialStatus": true,
      "IsPausedNonSpecialStatus": true,
      "PausedBetTypeIds": []
    },
    "OthersData": {
      "Level": 0,
      "IsAllowSell": false,
      "IsAllowOuterSell": false,
      "Datas": null,
      "PageIndex": 1,
      "PageSize": 30,
      "RecordCount": 0,
      "PageCount": 1
    },
    "InnerSellBetStatus": null
  }
},
  "Status": 1
}
""";


        public static string GetTestJSon = $@"
{{
  ""tribe"": ""raw"",
  ""steel"": {{
    ""somewhere"": ""minerals"",
    ""tank"": ""blind"",
    ""poor"": -505199521.5027485,
    ""law"": [
      ""tightly"",
      [
        {{
          ""smooth"": [
            {{
              ""captain"": ""still"",
              ""steep"": ""touch"",
              ""of"": 983312495.4004397,
              ""cheese"": ""these"",
              ""become"": -1530093326.7527275,
              ""if"": false,
              ""office"": false,
              ""steam"": ""card"",
              ""further"": ""trip"",
              ""instance"": {{
                ""garden"": -780945769,
                ""still"": false,
                ""nodded"": [
                  -1398657727,
                  false,
                  true,
                  false,
                  [
                    [
                      ""differ"",
                      -1899592396,
                      [
                        {{
                          ""cup"": [
                            [
                              true,
                              {{
                                ""sister"": 486105984,
                                ""date"": true,
                                ""partly"": -2010658644.1245623,
                                ""cannot"": ""garage"",
                                ""tide"": ""evidence"",
                                ""lay"": false,
                                ""count"": [
                                  [
                                    [
                                      {{
                                        ""meet"": [
                                          -1667185814,
                                          [
                                            {{
                                              ""somehow"": {{
                                                ""atmosphere"": ""shine"",
                                                ""cow"": 1634940589.090087,
                                                ""star"": -1716399953.7047448,
                                                ""toward"": {{
                                                  ""between"": 1415136396.9084105,
                                                  ""mission"": [
                                                    [
                                                      -2125115249.9804235,
                                                      {{
                                                        ""coat"": {{
                                                          ""necessary"": true,
                                                          ""dollar"": ""bell"",
                                                          ""remove"": [
                                                            {{
                                                              ""sense"": {{
                                                                ""safe"": {{
                                                                  ""saw"": 1424935984,
                                                                  ""eye"": 1743348977.2128706,
                                                                  ""deep"": {{
                                                                    ""traffic"": {{
                                                                      ""structure"": [
                                                                        false,
                                                                        ""rose"",
                                                                        {{
                                                                          ""anyone"": {{
                                                                            ""these"": ""activity"",
                                                                            ""habit"": ""arrow"",
                                                                            ""managed"": [
                                                                              ""toward"",
                                                                              false,
                                                                              {{
                                                                                ""taken"": ""visit"",
                                                                                ""likely"": [
                                                                                  false,
                                                                                  ""cowboy"",
                                                                                  ""inside"",
                                                                                  {{
                                                                                    ""success"": true,
                                                                                    ""station"": 1689699025.8032422,
                                                                                    ""hidden"": ""snake"",
                                                                                    ""cast"": -248175465,
                                                                                    ""trip"": true,
                                                                                    ""blow"": true,
                                                                                    ""additional"": [
                                                                                      false,
                                                                                      ""keep"",
                                                                                      {{
                                                                                        ""palace"": ""number"",
                                                                                        ""whom"": [
                                                                                          {{
                                                                                            ""trade"": {{
                                                                                              ""fort"": {{
                                                                                                ""silence"": ""station"",
                                                                                                ""away"": -1778469194.7246752,
                                                                                                ""evening"": {{
                                                                                                  ""purple"": ""terrible"",
                                                                                                  ""nearest"": [
                                                                                                    {{
                                                                                                      ""girl"": [
                                                                                                        [
                                                                                                          {{
                                                                                                            ""afternoon"": ""income"",
                                                                                                            ""also"": [
                                                                                                              {{
                                                                                                                ""must"": [
                                                                                                                  true,
                                                                                                                  {{
                                                                                                                    ""fighting"": 883987891,
                                                                                                                    ""perhaps"": -644528774.1240039,
                                                                                                                    ""unit"": ""government"",
                                                                                                                    ""seldom"": -418892682.2257261,
                                                                                                                    ""actual"": true,
                                                                                                                    ""save"": {{
                                                                                                                      ""highway"": -999880882.4842105,
                                                                                                                      ""death"": 792735802.0902085,
                                                                                                                      ""fifteen"": 1789963943.8020005,
                                                                                                                      ""realize"": false,
                                                                                                                      ""dream"": false,
                                                                                                                      ""result"": 389552654.5055785,
                                                                                                                      ""walk"": false,
                                                                                                                      ""half"": false,
                                                                                                                      ""quietly"": ""birth"",
                                                                                                                      ""who"": ""mental""
                                                                                                                    }},
                                                                                                                    ""prepare"": {{
                                                                                                                      ""white"": -863803607.1165552,
                                                                                                                      ""replied"": [
                                                                                                                        [
                                                                                                                          [
                                                                                                                            -802491180,
                                                                                                                            178396635,
                                                                                                                            false,
                                                                                                                            true,
                                                                                                                            {{
                                                                                                                              ""airplane"": {{
                                                                                                                                ""apple"": 1447027517,
                                                                                                                                ""scene"": ""door"",
                                                                                                                                ""pride"": ""bow"",
                                                                                                                                ""represent"": [
                                                                                                                                  false,
                                                                                                                                  {{
                                                                                                                                    ""forward"": true,
                                                                                                                                    ""freedom"": ""rather"",
                                                                                                                                    ""solve"": ""breeze"",
                                                                                                                                    ""cell"": [
                                                                                                                                      {{
                                                                                                                                        ""give"": false,
                                                                                                                                        ""depend"": [
                                                                                                                                          1271394249.3755622,
                                                                                                                                          1716015626,
                                                                                                                                          {{
                                                                                                                                            ""drove"": false,
                                                                                                                                            ""time"": [
                                                                                                                                              [
                                                                                                                                                {{
                                                                                                                                                  ""port"": [
                                                                                                                                                    1449518245.7172465,
                                                                                                                                                    2016237276.5782743,
                                                                                                                                                    {{
                                                                                                                                                      ""purpose"": 1425231343.6725588,
                                                                                                                                                      ""settlers"": -511414200.70252705,
                                                                                                                                                      ""planet"": false,
                                                                                                                                                      ""could"": -350236949,
                                                                                                                                                      ""allow"": {{
                                                                                                                                                        ""purple"": [
                                                                                                                                                          [
                                                                                                                                                            true,
                                                                                                                                                            ""ever"",
                                                                                                                                                            313214053.48562145,
                                                                                                                                                            [
                                                                                                                                                              [
                                                                                                                                                                -2056927861.7612844,
                                                                                                                                                                68065572.76988173,
                                                                                                                                                                true,
                                                                                                                                                                [
                                                                                                                                                                  [
                                                                                                                                                                    [
                                                                                                                                                                      -1642763463,
                                                                                                                                                                      [
                                                                                                                                                                        {{
                                                                                                                                                                          ""gas"": {{
                                                                                                                                                                            ""master"": ""left"",
                                                                                                                                                                            ""mysterious"": {{
                                                                                                                                                                              ""outer"": 1010457550,
                                                                                                                                                                              ""completely"": {{
                                                                                                                                                                                ""teach"": -1774879313.1031022,
                                                                                                                                                                                ""degree"": {{
                                                                                                                                                                                  ""warn"": ""war"",
                                                                                                                                                                                  ""tower"": ""onto"",
                                                                                                                                                                                  ""driven"": false,
                                                                                                                                                                                  ""skin"": [
                                                                                                                                                                                    {{
                                                                                                                                                                                      ""see"": [
                                                                                                                                                                                        ""suppose"",
                                                                                                                                                                                        [
                                                                                                                                                                                          127845830,
                                                                                                                                                                                          {{
                                                                                                                                                                                            ""almost"": 1501360843.417632,
                                                                                                                                                                                            ""law"": [
                                                                                                                                                                                              2087581692.9904747,
                                                                                                                                                                                              false,
                                                                                                                                                                                              -1814290667,
                                                                                                                                                                                              {{
                                                                                                                                                                                                ""crack"": [
                                                                                                                                                                                                  true,
                                                                                                                                                                                                  true,
                                                                                                                                                                                                  ""needle"",
                                                                                                                                                                                                  [
                                                                                                                                                                                                    -1851073497,
                                                                                                                                                                                                    [
                                                                                                                                                                                                      false,
                                                                                                                                                                                                      143999028.32950354,
                                                                                                                                                                                                      false,
                                                                                                                                                                                                      false,
                                                                                                                                                                                                      -1249459856,
                                                                                                                                                                                                      920827225.3157411,
                                                                                                                                                                                                      -1454944657,
                                                                                                                                                                                                      false,
                                                                                                                                                                                                      {{
                                                                                                                                                                                                        ""hungry"": 734607761,
                                                                                                                                                                                                        ""low"": false,
                                                                                                                                                                                                        ""plan"": ""apart"",
                                                                                                                                                                                                        ""fish"": ""such"",
                                                                                                                                                                                                        ""group"": true,
                                                                                                                                                                                                        ""speak"": 1141143791.638939,
                                                                                                                                                                                                        ""worry"": -1503374965.1496992,
                                                                                                                                                                                                        ""pig"": ""physical"",
                                                                                                                                                                                                        ""ill"": 1477347334,
                                                                                                                                                                                                        ""shot"": false
                                                                                                                                                                                                      }},
                                                                                                                                                                                                      -1181204611
                                                                                                                                                                                                    ],
                                                                                                                                                                                                    -1729048672.032076,
                                                                                                                                                                                                    false,
                                                                                                                                                                                                    ""baby"",
                                                                                                                                                                                                    ""whale"",
                                                                                                                                                                                                    -632509113.423943,
                                                                                                                                                                                                    1215562643.3328738,
                                                                                                                                                                                                    true,
                                                                                                                                                                                                    -196146866.85671234
                                                                                                                                                                                                  ],
                                                                                                                                                                                                  937224703,
                                                                                                                                                                                                  928845857.5007467,
                                                                                                                                                                                                  -1971169453,
                                                                                                                                                                                                  ""manner"",
                                                                                                                                                                                                  ""importance"",
                                                                                                                                                                                                  true
                                                                                                                                                                                                ],
                                                                                                                                                                                                ""hundred"": true,
                                                                                                                                                                                                ""copper"": true,
                                                                                                                                                                                                ""port"": ""cause"",
                                                                                                                                                                                                ""easy"": true,
                                                                                                                                                                                                ""massage"": 1455005766,
                                                                                                                                                                                                ""trick"": true,
                                                                                                                                                                                                ""obtain"": 433376238,
                                                                                                                                                                                                ""road"": ""engine"",
                                                                                                                                                                                                ""diagram"": false
                                                                                                                                                                                              }},
                                                                                                                                                                                              false,
                                                                                                                                                                                              ""turn"",
                                                                                                                                                                                              ""almost"",
                                                                                                                                                                                              ""combine"",
                                                                                                                                                                                              true,
                                                                                                                                                                                              488250854
                                                                                                                                                                                            ],
                                                                                                                                                                                            ""man"": -382466594,
                                                                                                                                                                                            ""cook"": -1836547876.9513364,
                                                                                                                                                                                            ""additional"": ""yet"",
                                                                                                                                                                                            ""potatoes"": 2108503504.1801653,
                                                                                                                                                                                            ""football"": -660524959.4343276,
                                                                                                                                                                                            ""clock"": true,
                                                                                                                                                                                            ""throat"": false,
                                                                                                                                                                                            ""water"": false
                                                                                                                                                                                          }},
                                                                                                                                                                                          ""ice"",
                                                                                                                                                                                          false,
                                                                                                                                                                                          -984232162.0903006,
                                                                                                                                                                                          -840710966,
                                                                                                                                                                                          false,
                                                                                                                                                                                          ""rope"",
                                                                                                                                                                                          -1938587621,
                                                                                                                                                                                          true
                                                                                                                                                                                        ],
                                                                                                                                                                                        false,
                                                                                                                                                                                        ""frame"",
                                                                                                                                                                                        false,
                                                                                                                                                                                        ""transportation"",
                                                                                                                                                                                        -272433939,
                                                                                                                                                                                        true,
                                                                                                                                                                                        -812919266.5171304,
                                                                                                                                                                                        552892349
                                                                                                                                                                                      ],
                                                                                                                                                                                      ""more"": ""usual"",
                                                                                                                                                                                      ""climate"": false,
                                                                                                                                                                                      ""eventually"": ""zero"",
                                                                                                                                                                                      ""giant"": 2114806235.962215,
                                                                                                                                                                                      ""member"": ""unhappy"",
                                                                                                                                                                                      ""could"": false,
                                                                                                                                                                                      ""letter"": false,
                                                                                                                                                                                      ""sugar"": ""want"",
                                                                                                                                                                                      ""almost"": true
                                                                                                                                                                                    }},
                                                                                                                                                                                    -215146305,
                                                                                                                                                                                    ""certain"",
                                                                                                                                                                                    2106260133.6774578,
                                                                                                                                                                                    false,
                                                                                                                                                                                    ""development"",
                                                                                                                                                                                    -1296369100,
                                                                                                                                                                                    306093573.25317955,
                                                                                                                                                                                    ""classroom"",
                                                                                                                                                                                    1249278139.3265047
                                                                                                                                                                                  ],
                                                                                                                                                                                  ""as"": false,
                                                                                                                                                                                  ""fifth"": true,
                                                                                                                                                                                  ""division"": 797821541.2081299,
                                                                                                                                                                                  ""affect"": ""thread"",
                                                                                                                                                                                  ""our"": false
                                                                                                                                                                                }},
                                                                                                                                                                                ""finally"": 352911628.81488657,
                                                                                                                                                                                ""theory"": true,
                                                                                                                                                                                ""muscle"": ""wore"",
                                                                                                                                                                                ""return"": true,
                                                                                                                                                                                ""themselves"": 1246680526.22816,
                                                                                                                                                                                ""palace"": 1715215540.4362144,
                                                                                                                                                                                ""not"": true,
                                                                                                                                                                                ""grass"": false
                                                                                                                                                                              }},
                                                                                                                                                                              ""was"": 1173104490,
                                                                                                                                                                              ""third"": false,
                                                                                                                                                                              ""speed"": true,
                                                                                                                                                                              ""appearance"": ""trouble"",
                                                                                                                                                                              ""trouble"": ""dried"",
                                                                                                                                                                              ""support"": -895402538,
                                                                                                                                                                              ""movement"": false,
                                                                                                                                                                              ""step"": true
                                                                                                                                                                            }},
                                                                                                                                                                            ""blood"": ""climate"",
                                                                                                                                                                            ""sun"": ""pupil"",
                                                                                                                                                                            ""past"": false,
                                                                                                                                                                            ""wet"": 356325120.27706623,
                                                                                                                                                                            ""food"": 569431058.3538518,
                                                                                                                                                                            ""stretch"": true,
                                                                                                                                                                            ""ground"": 1305871187,
                                                                                                                                                                            ""principle"": ""nature""
                                                                                                                                                                          }},
                                                                                                                                                                          ""warn"": 1542306867.4851632,
                                                                                                                                                                          ""came"": false,
                                                                                                                                                                          ""function"": 1692726531,
                                                                                                                                                                          ""dance"": true,
                                                                                                                                                                          ""result"": -1359096336.6194067,
                                                                                                                                                                          ""push"": ""came"",
                                                                                                                                                                          ""rise"": false,
                                                                                                                                                                          ""middle"": true,
                                                                                                                                                                          ""ahead"": false
                                                                                                                                                                        }},
                                                                                                                                                                        -74456971.29514122,
                                                                                                                                                                        1351460306.6513863,
                                                                                                                                                                        true,
                                                                                                                                                                        -630950313.9994655,
                                                                                                                                                                        false,
                                                                                                                                                                        ""diameter"",
                                                                                                                                                                        ""win"",
                                                                                                                                                                        ""clear"",
                                                                                                                                                                        ""wrote""
                                                                                                                                                                      ],
                                                                                                                                                                      true,
                                                                                                                                                                      ""entire"",
                                                                                                                                                                      704130133.9769683,
                                                                                                                                                                      1074561029,
                                                                                                                                                                      ""president"",
                                                                                                                                                                      2080263320.2506204,
                                                                                                                                                                      311502580,
                                                                                                                                                                      -1681551661.680151
                                                                                                                                                                    ],
                                                                                                                                                                    false,
                                                                                                                                                                    true,
                                                                                                                                                                    ""terrible"",
                                                                                                                                                                    false,
                                                                                                                                                                    1601389441.0187483,
                                                                                                                                                                    false,
                                                                                                                                                                    ""volume"",
                                                                                                                                                                    ""tribe"",
                                                                                                                                                                    true
                                                                                                                                                                  ],
                                                                                                                                                                  403268646.9122081,
                                                                                                                                                                  -1342122641,
                                                                                                                                                                  ""women"",
                                                                                                                                                                  false,
                                                                                                                                                                  1738876479,
                                                                                                                                                                  -1549229175.5866141,
                                                                                                                                                                  true,
                                                                                                                                                                  ""raw"",
                                                                                                                                                                  812191127
                                                                                                                                                                ],
                                                                                                                                                                -2044095863.867169,
                                                                                                                                                                -359975885.93941164,
                                                                                                                                                                1898426870.9393077,
                                                                                                                                                                1157103599.5180497,
                                                                                                                                                                ""particularly"",
                                                                                                                                                                true
                                                                                                                                                              ],
                                                                                                                                                              1239665808.1850233,
                                                                                                                                                              true,
                                                                                                                                                              false,
                                                                                                                                                              ""guard"",
                                                                                                                                                              -1001544362.7676048,
                                                                                                                                                              false,
                                                                                                                                                              false,
                                                                                                                                                              ""parallel"",
                                                                                                                                                              ""airplane""
                                                                                                                                                            ],
                                                                                                                                                            -934134841,
                                                                                                                                                            false,
                                                                                                                                                            true,
                                                                                                                                                            ""fairly"",
                                                                                                                                                            ""pack"",
                                                                                                                                                            true
                                                                                                                                                          ],
                                                                                                                                                          ""cup"",
                                                                                                                                                          1842317828,
                                                                                                                                                          ""hold"",
                                                                                                                                                          -1757875641,
                                                                                                                                                          464864517.1105523,
                                                                                                                                                          ""headed"",
                                                                                                                                                          -1868431577,
                                                                                                                                                          ""doing"",
                                                                                                                                                          false
                                                                                                                                                        ],
                                                                                                                                                        ""check"": ""seems"",
                                                                                                                                                        ""terrible"": ""prevent"",
                                                                                                                                                        ""massage"": ""close"",
                                                                                                                                                        ""age"": true,
                                                                                                                                                        ""lie"": 1112372491,
                                                                                                                                                        ""smell"": 1212553160.142128,
                                                                                                                                                        ""behavior"": -2019607906.491212,
                                                                                                                                                        ""owner"": -1636191790.2475376,
                                                                                                                                                        ""river"": false
                                                                                                                                                      }},
                                                                                                                                                      ""curve"": true,
                                                                                                                                                      ""your"": true,
                                                                                                                                                      ""everyone"": ""powerful"",
                                                                                                                                                      ""character"": ""lamp"",
                                                                                                                                                      ""sheet"": false
                                                                                                                                                    }},
                                                                                                                                                    -731003128.2401958,
                                                                                                                                                    true,
                                                                                                                                                    -1960590871.3586679,
                                                                                                                                                    ""castle"",
                                                                                                                                                    ""observe"",
                                                                                                                                                    true,
                                                                                                                                                    false
                                                                                                                                                  ],
                                                                                                                                                  ""force"": true,
                                                                                                                                                  ""wall"": true,
                                                                                                                                                  ""valuable"": ""gather"",
                                                                                                                                                  ""fuel"": 1020388720.4186764,
                                                                                                                                                  ""shape"": false,
                                                                                                                                                  ""sell"": false,
                                                                                                                                                  ""pleasure"": ""examine"",
                                                                                                                                                  ""clearly"": ""wrote"",
                                                                                                                                                  ""remarkable"": true
                                                                                                                                                }},
                                                                                                                                                ""finish"",
                                                                                                                                                -818273770.1433783,
                                                                                                                                                true,
                                                                                                                                                1557730041,
                                                                                                                                                false,
                                                                                                                                                false,
                                                                                                                                                ""forward"",
                                                                                                                                                448239874.5269518,
                                                                                                                                                ""history""
                                                                                                                                              ],
                                                                                                                                              -373500338.3821478,
                                                                                                                                              true,
                                                                                                                                              ""jump"",
                                                                                                                                              false,
                                                                                                                                              -1353491346,
                                                                                                                                              ""low"",
                                                                                                                                              679234421.8915863,
                                                                                                                                              true,
                                                                                                                                              false
                                                                                                                                            ],
                                                                                                                                            ""power"": -1097673534.002223,
                                                                                                                                            ""short"": false,
                                                                                                                                            ""sick"": ""anyway"",
                                                                                                                                            ""closer"": false,
                                                                                                                                            ""depth"": -623329824,
                                                                                                                                            ""merely"": ""ranch"",
                                                                                                                                            ""therefore"": -2106329124.9088573,
                                                                                                                                            ""sum"": false
                                                                                                                                          }},
                                                                                                                                          ""proper"",
                                                                                                                                          -1130200531,
                                                                                                                                          -2056224892.9891224,
                                                                                                                                          true,
                                                                                                                                          407685358,
                                                                                                                                          true,
                                                                                                                                          false
                                                                                                                                        ],
                                                                                                                                        ""present"": true,
                                                                                                                                        ""getting"": true,
                                                                                                                                        ""arrow"": -163180337,
                                                                                                                                        ""successful"": ""receive"",
                                                                                                                                        ""indicate"": 1474970247.5848641,
                                                                                                                                        ""gather"": -1886961196.0706868,
                                                                                                                                        ""taught"": ""brought"",
                                                                                                                                        ""process"": 616499919
                                                                                                                                      }},
                                                                                                                                      546278562,
                                                                                                                                      false,
                                                                                                                                      1102170672,
                                                                                                                                      952996867.1055541,
                                                                                                                                      -1950273642.04357,
                                                                                                                                      149856398,
                                                                                                                                      ""row"",
                                                                                                                                      ""burn"",
                                                                                                                                      ""beneath""
                                                                                                                                    ],
                                                                                                                                    ""father"": false,
                                                                                                                                    ""tie"": -1464994459,
                                                                                                                                    ""slide"": ""handsome"",
                                                                                                                                    ""activity"": 1779820272.9495788,
                                                                                                                                    ""written"": true,
                                                                                                                                    ""mail"": -1348423474.692966
                                                                                                                                  }},
                                                                                                                                  -522707459,
                                                                                                                                  ""nor"",
                                                                                                                                  -99789703.95675945,
                                                                                                                                  ""species"",
                                                                                                                                  ""manner"",
                                                                                                                                  ""was"",
                                                                                                                                  false,
                                                                                                                                  ""tight""
                                                                                                                                ],
                                                                                                                                ""fireplace"": true,
                                                                                                                                ""listen"": false,
                                                                                                                                ""hour"": false,
                                                                                                                                ""develop"": -2065836739,
                                                                                                                                ""species"": false,
                                                                                                                                ""both"": true
                                                                                                                              }},
                                                                                                                              ""troops"": -179303058.4362936,
                                                                                                                              ""crowd"": -925792693.8637877,
                                                                                                                              ""higher"": -1122327559.444121,
                                                                                                                              ""sad"": ""list"",
                                                                                                                              ""shinning"": 88165870,
                                                                                                                              ""afternoon"": 56640104.15792322,
                                                                                                                              ""coal"": false,
                                                                                                                              ""simply"": ""lucky"",
                                                                                                                              ""properly"": ""month""
                                                                                                                            }},
                                                                                                                            1603188159.7027812,
                                                                                                                            true,
                                                                                                                            false,
                                                                                                                            ""shallow"",
                                                                                                                            true
                                                                                                                          ],
                                                                                                                          ""wherever"",
                                                                                                                          931646999.1311722,
                                                                                                                          -1494511361.2369633,
                                                                                                                          ""tax"",
                                                                                                                          1777049441,
                                                                                                                          false,
                                                                                                                          ""art"",
                                                                                                                          false,
                                                                                                                          ""scientific""
                                                                                                                        ],
                                                                                                                        -1885565064,
                                                                                                                        ""ants"",
                                                                                                                        573753726.470715,
                                                                                                                        ""uncle"",
                                                                                                                        ""active"",
                                                                                                                        ""impossible"",
                                                                                                                        ""follow"",
                                                                                                                        false,
                                                                                                                        ""memory""
                                                                                                                      ],
                                                                                                                      ""wind"": true,
                                                                                                                      ""partly"": 1821900945.6410084,
                                                                                                                      ""chose"": 1946084657,
                                                                                                                      ""fifty"": false,
                                                                                                                      ""period"": true,
                                                                                                                      ""history"": false,
                                                                                                                      ""draw"": true,
                                                                                                                      ""necessary"": -1529382401.2818165
                                                                                                                    }},
                                                                                                                    ""gulf"": 1108371156.2977204,
                                                                                                                    ""impossible"": false,
                                                                                                                    ""creature"": false
                                                                                                                  }},
                                                                                                                  true,
                                                                                                                  ""people"",
                                                                                                                  1135541183.1843529,
                                                                                                                  ""my"",
                                                                                                                  -1098372296,
                                                                                                                  false,
                                                                                                                  -788698737,
                                                                                                                  false
                                                                                                                ],
                                                                                                                ""pain"": false,
                                                                                                                ""mean"": ""bar"",
                                                                                                                ""gun"": 491481664.4417324,
                                                                                                                ""chosen"": true,
                                                                                                                ""when"": true,
                                                                                                                ""raise"": ""hit"",
                                                                                                                ""therefore"": -222906809.62172556,
                                                                                                                ""headed"": 1026459538,
                                                                                                                ""warm"": ""reader""
                                                                                                              }},
                                                                                                              ""bell"",
                                                                                                              ""change"",
                                                                                                              false,
                                                                                                              true,
                                                                                                              -90433373,
                                                                                                              ""sure"",
                                                                                                              878951956.703383,
                                                                                                              false,
                                                                                                              false
                                                                                                            ],
                                                                                                            ""practical"": true,
                                                                                                            ""widely"": 1579606013.5982947,
                                                                                                            ""decide"": ""mostly"",
                                                                                                            ""product"": ""bat"",
                                                                                                            ""went"": -1176430743,
                                                                                                            ""summer"": true,
                                                                                                            ""newspaper"": -1488454823,
                                                                                                            ""finger"": ""silent""
                                                                                                          }},
                                                                                                          false,
                                                                                                          false,
                                                                                                          -1180828271.2388372,
                                                                                                          ""pocket"",
                                                                                                          true,
                                                                                                          ""old"",
                                                                                                          true,
                                                                                                          -220452012.78247404,
                                                                                                          ""square""
                                                                                                        ],
                                                                                                        true,
                                                                                                        202548780.16365862,
                                                                                                        1570820587.7659678,
                                                                                                        true,
                                                                                                        ""face"",
                                                                                                        ""major"",
                                                                                                        1407789597,
                                                                                                        1163966787,
                                                                                                        1463730322.0243182
                                                                                                      ],
                                                                                                      ""tower"": true,
                                                                                                      ""mountain"": ""master"",
                                                                                                      ""aid"": ""piano"",
                                                                                                      ""experiment"": ""poem"",
                                                                                                      ""distance"": ""fairly"",
                                                                                                      ""poetry"": -2080017317,
                                                                                                      ""can"": false,
                                                                                                      ""scene"": ""construction"",
                                                                                                      ""population"": false
                                                                                                    }},
                                                                                                    true,
                                                                                                    true,
                                                                                                    true,
                                                                                                    ""farmer"",
                                                                                                    -719686237,
                                                                                                    17590246,
                                                                                                    false,
                                                                                                    -1654772489.4941263,
                                                                                                    -201539055.51403522
                                                                                                  ],
                                                                                                  ""grandmother"": ""right"",
                                                                                                  ""spend"": true,
                                                                                                  ""talk"": true,
                                                                                                  ""population"": ""pencil"",
                                                                                                  ""surrounded"": -1960685982,
                                                                                                  ""paid"": false,
                                                                                                  ""hearing"": -1630322306,
                                                                                                  ""bright"": ""organized""
                                                                                                }},
                                                                                                ""process"": ""spell"",
                                                                                                ""carried"": false,
                                                                                                ""agree"": ""straw"",
                                                                                                ""using"": 982611995.6148095,
                                                                                                ""tales"": ""apartment"",
                                                                                                ""only"": -136016153,
                                                                                                ""mail"": ""fastened""
                                                                                              }},
                                                                                              ""blood"": ""position"",
                                                                                              ""article"": -546687875.7851977,
                                                                                              ""however"": false,
                                                                                              ""possible"": ""was"",
                                                                                              ""gather"": false,
                                                                                              ""wind"": ""man"",
                                                                                              ""happened"": false,
                                                                                              ""hearing"": ""positive"",
                                                                                              ""most"": false
                                                                                            }},
                                                                                            ""apart"": ""apartment"",
                                                                                            ""cow"": 761035763,
                                                                                            ""community"": false,
                                                                                            ""thank"": false,
                                                                                            ""yesterday"": false,
                                                                                            ""image"": -380914501,
                                                                                            ""eye"": 781519440.5607538,
                                                                                            ""mood"": -230882685,
                                                                                            ""rubber"": false
                                                                                          }},
                                                                                          true,
                                                                                          false,
                                                                                          1177250896,
                                                                                          false,
                                                                                          ""fall"",
                                                                                          -1550140067,
                                                                                          false,
                                                                                          935321497,
                                                                                          1685138179
                                                                                        ],
                                                                                        ""twelve"": ""corn"",
                                                                                        ""railroad"": false,
                                                                                        ""discovery"": true,
                                                                                        ""valley"": -926339215.4542809,
                                                                                        ""coach"": 1676861654.4240065,
                                                                                        ""wheat"": -2000904896,
                                                                                        ""picture"": ""mind"",
                                                                                        ""nest"": true
                                                                                      }},
                                                                                      ""engineer"",
                                                                                      ""physical"",
                                                                                      -313817649,
                                                                                      true,
                                                                                      false,
                                                                                      true,
                                                                                      -243389115.28897285
                                                                                    ],
                                                                                    ""prove"": -1167659634.7089438,
                                                                                    ""having"": ""fine"",
                                                                                    ""conversation"": 370771880
                                                                                  }},
                                                                                  1743372109,
                                                                                  -973481713.7450032,
                                                                                  true,
                                                                                  -287354994,
                                                                                  ""value"",
                                                                                  ""weight""
                                                                                ],
                                                                                ""deep"": true,
                                                                                ""chosen"": ""cage"",
                                                                                ""shelf"": true,
                                                                                ""trail"": true,
                                                                                ""model"": -740134301.4114189,
                                                                                ""begun"": true,
                                                                                ""goes"": true,
                                                                                ""strip"": true
                                                                              }},
                                                                              -2014688108.4501567,
                                                                              2031361124,
                                                                              2136880892.4472466,
                                                                              ""essential"",
                                                                              1420652238.1381445,
                                                                              ""glad"",
                                                                              -1974545716.0666142
                                                                            ],
                                                                            ""six"": -723234763.4124184,
                                                                            ""necessary"": ""detail"",
                                                                            ""brought"": true,
                                                                            ""bank"": ""village"",
                                                                            ""earth"": -1799366941.655941,
                                                                            ""ability"": -932632901,
                                                                            ""fresh"": ""count""
                                                                          }},
                                                                          ""create"": false,
                                                                          ""picture"": ""now"",
                                                                          ""yesterday"": false,
                                                                          ""funny"": -789041145.5235581,
                                                                          ""race"": 381378886.0340605,
                                                                          ""deal"": ""present"",
                                                                          ""neighborhood"": -348904122.2852917,
                                                                          ""do"": true,
                                                                          ""settle"": ""pride""
                                                                        }},
                                                                        true,
                                                                        true,
                                                                        ""rule"",
                                                                        ""baseball"",
                                                                        ""electricity"",
                                                                        false,
                                                                        ""blew""
                                                                      ],
                                                                      ""taken"": ""division"",
                                                                      ""report"": 492228014,
                                                                      ""down"": true,
                                                                      ""enter"": ""football"",
                                                                      ""egg"": -186686898.13972902,
                                                                      ""length"": false,
                                                                      ""single"": ""thing"",
                                                                      ""prove"": -1782193254.0333343,
                                                                      ""luck"": ""terrible""
                                                                    }},
                                                                    ""escape"": ""western"",
                                                                    ""beside"": 1204645292.9301028,
                                                                    ""sink"": ""eye"",
                                                                    ""sale"": ""camera"",
                                                                    ""queen"": 1024796141,
                                                                    ""machinery"": false,
                                                                    ""nest"": true,
                                                                    ""means"": ""angle"",
                                                                    ""pull"": -1695836959.5605679
                                                                  }},
                                                                  ""automobile"": ""four"",
                                                                  ""son"": -119142425,
                                                                  ""excellent"": ""strange"",
                                                                  ""none"": 222202348,
                                                                  ""able"": false,
                                                                  ""elephant"": -30242502.7800169,
                                                                  ""article"": 872778128
                                                                }},
                                                                ""belt"": 361533471.9347997,
                                                                ""actual"": ""above"",
                                                                ""ruler"": ""suit"",
                                                                ""kitchen"": true,
                                                                ""sister"": -1359264732,
                                                                ""factor"": -851133723,
                                                                ""walk"": true,
                                                                ""wing"": 248583252.53674746,
                                                                ""remember"": -1793841340
                                                              }},
                                                              ""wood"": ""blood"",
                                                              ""thin"": -761564541,
                                                              ""shoot"": -144261262.74489546,
                                                              ""result"": true,
                                                              ""telephone"": ""black"",
                                                              ""variety"": ""level"",
                                                              ""describe"": -1882897729.2975411,
                                                              ""put"": true,
                                                              ""trace"": false
                                                            }},
                                                            ""carbon"",
                                                            ""total"",
                                                            -2069038590,
                                                            false,
                                                            849016664.5974126,
                                                            false,
                                                            ""count"",
                                                            ""frog"",
                                                            ""half""
                                                          ],
                                                          ""care"": false,
                                                          ""future"": 2005553802.6321163,
                                                          ""complete"": true,
                                                          ""smallest"": 393769286.2952857,
                                                          ""accident"": false,
                                                          ""fear"": -1624624319,
                                                          ""dish"": -1881791985
                                                        }},
                                                        ""including"": false,
                                                        ""dust"": false,
                                                        ""sang"": false,
                                                        ""arm"": ""area"",
                                                        ""industry"": ""changing"",
                                                        ""hand"": ""audience"",
                                                        ""pool"": ""rain"",
                                                        ""wolf"": -1893925785,
                                                        ""repeat"": ""thus""
                                                      }},
                                                      -544038464,
                                                      ""ship"",
                                                      1947526304.7184634,
                                                      false,
                                                      -2013830980.752193,
                                                      ""corn"",
                                                      true,
                                                      false
                                                    ],
                                                    ""moving"",
                                                    ""victory"",
                                                    false,
                                                    -1836462502,
                                                    true,
                                                    1590776909,
                                                    -598653395.8577628,
                                                    true,
                                                    891464856
                                                  ],
                                                  ""daughter"": false,
                                                  ""shade"": 770002479.6465926,
                                                  ""road"": -1088315187.670168,
                                                  ""cat"": ""broke"",
                                                  ""thread"": ""ancient"",
                                                  ""least"": ""introduced"",
                                                  ""barn"": ""available"",
                                                  ""ready"": true
                                                }},
                                                ""post"": 520207540,
                                                ""told"": -13859754.106442928,
                                                ""beauty"": true,
                                                ""selection"": -181626515.06962013,
                                                ""trip"": ""got"",
                                                ""struck"": 1878134253.5047631
                                              }},
                                              ""cowboy"": false,
                                              ""feed"": -2020543228,
                                              ""interest"": 51256166.178281784,
                                              ""pool"": ""grass"",
                                              ""shirt"": true,
                                              ""society"": -1460922556,
                                              ""unusual"": -1013114149,
                                              ""egg"": -1246159123.0971484,
                                              ""grandfather"": -764410941
                                            }},
                                            ""desk"",
                                            132492568.92148066,
                                            true,
                                            true,
                                            ""but"",
                                            true,
                                            -266596976,
                                            -1774966156,
                                            true
                                          ],
                                          false,
                                          true,
                                          false,
                                          true,
                                          ""know"",
                                          -164380644.18203878,
                                          ""talk"",
                                          false
                                        ],
                                        ""swept"": 2102201653,
                                        ""farm"": ""tongue"",
                                        ""roll"": true,
                                        ""baseball"": ""accept"",
                                        ""immediately"": ""shine"",
                                        ""gentle"": ""flow"",
                                        ""trick"": 1489556224,
                                        ""through"": false,
                                        ""wrote"": -117211239
                                      }},
                                      -1732269178.7883205,
                                      ""throat"",
                                      -71950309.44125938,
                                      ""amount"",
                                      true,
                                      -1358981368,
                                      -1454262260,
                                      false,
                                      ""letter""
                                    ],
                                    false,
                                    false,
                                    ""get"",
                                    true,
                                    -1447855764,
                                    -2143043250,
                                    1514942997,
                                    -2083036793.5823946,
                                    137589206
                                  ],
                                  false,
                                  -1687162866,
                                  -599193585.931201,
                                  ""spend"",
                                  false,
                                  272045899.01409817,
                                  ""on"",
                                  1768801066,
                                  -182191822.61702585
                                ],
                                ""climb"": true,
                                ""knowledge"": -262340968,
                                ""maybe"": true
                              }},
                              -319390975.506413,
                              ""sometime"",
                              ""include"",
                              ""upward"",
                              484166291,
                              ""sound"",
                              ""government"",
                              1631664590.9842348
                            ],
                            ""parts"",
                            true,
                            1280968071.103074,
                            ""vast"",
                            true,
                            -218597413,
                            ""species"",
                            ""unless"",
                            false
                          ],
                          ""leaf"": true,
                          ""lucky"": 1079521162.1744285,
                          ""rock"": ""truck"",
                          ""neck"": -1008723551,
                          ""bat"": ""disappear"",
                          ""lying"": -378214988,
                          ""sweet"": 1398251380,
                          ""gradually"": -2139846134,
                          ""west"": false
                        }},
                        ""passage"",
                        ""environment"",
                        1329924248.822289,
                        ""lot"",
                        -949909336,
                        ""calm"",
                        true,
                        false,
                        ""wish""
                      ],
                      778952395,
                      ""cheese"",
                      -1730416279,
                      -269161736,
                      ""shadow"",
                      ""mission"",
                      -1992901484.196889
                    ],
                    ""secret"",
                    ""most"",
                    false,
                    ""refused"",
                    false,
                    false,
                    false,
                    true,
                    true
                  ],
                  -1438058416,
                  true,
                  813851678.0066323,
                  ""help"",
                  ""depend""
                ],
                ""golden"": -887814088.6767349,
                ""palace"": true,
                ""evidence"": -370283186.9087162,
                ""climb"": true,
                ""wonder"": ""burn"",
                ""blue"": ""black"",
                ""office"": 611593220.6301827
              }}
            }},
            false,
            1296929286,
            false,
            ""turn"",
            -1633706252.8239555,
            -416493760.95404387,
            1457462246.1026878,
            false,
            ""musical""
          ],
          ""both"": ""thought"",
          ""child"": false,
          ""throughout"": false,
          ""swim"": -1997143603.666954,
          ""good"": ""famous"",
          ""weigh"": false,
          ""sea"": ""pick"",
          ""arm"": 1436075835,
          ""produce"": false
        }},
        1592651478.535562,
        true,
        476463537,
        -644157959,
        ""tail"",
        false,
        ""later"",
        -296891852.3518505,
        ""curious""
      ],
      -2039490678.086616,
      89383969.79656267,
      ""create"",
      true,
      true,
      1043821456,
      -48936204,
      -59183255.402852535
    ],
    ""season"": 1680931183,
    ""mysterious"": 1974593212.2011132,
    ""am"": true,
    ""partly"": true,
    ""wire"": true,
    ""combine"": -1857004232
  }},
  ""said"": ""seat"",
  ""ran"": 1612198236,
  ""fallen"": true,
  ""cup"": false,
  ""chest"": ""having"",
  ""empty"": ""stairs"",
  ""probably"": ""honor"",
  ""above"": -38879534
}}

";

    }
}
