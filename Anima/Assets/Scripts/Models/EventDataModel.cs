using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDataModel : MonoBehaviour
{

    public static List<string> EventObjectModelList = new List<string> {
        EventObjectModel.Rain,
        EventObjectModel.Food,
        EventObjectModel.Population,
        EventObjectModel.Tree,
        EventObjectModel.Wildfire,
        EventObjectModel.flood,
        EventObjectModel.GroundCollapse,
        EventObjectModel.Desolation,
        EventObjectModel.Earthquake,
        EventObjectModel.Typhoon,
        EventObjectModel.Nothing
    };

    public static Dictionary<string, string> EventDescriptionEffectList = new Dictionary<string, string>()
    {
        { EventObjectModel.Rain, "Water เพิ่มขึ้น 1 lv" },
        { EventObjectModel.Food, "Food เพิ่มขึ้น 10 %" },
        { EventObjectModel.Population, "Population เพิ่มขึ้น 10 %"},
        { EventObjectModel.Tree, "Forest เพิ่มขึ้น 1 lv"},
        { EventObjectModel.Wildfire, "forest ลดลง 1 lv" },
        { EventObjectModel.flood, "food ลดลง 30%" },
        { EventObjectModel.GroundCollapse, "สุ่มลด Level สิ่งก่อสร้าง" },
        { EventObjectModel.Desolation, "Water ลดลง  2 lv" },
        { EventObjectModel.Earthquake, "สิ่งก่อสร้าง ลดลง 1/3 , population ลดลง 25%" },
        { EventObjectModel.Typhoon, "Wood Cutter, Mine, Farm, Town ลดลง 1 lv" },
        { EventObjectModel.Nothing, "ไม่มีเหตุการณ์อะไรเกิดขึ้น" }
    };

    public static Dictionary<string, string> EventDescriptionList = new Dictionary<string, string>()
    {
        { EventObjectModel.Rain, "ฝนตกตามฤดูกาล ได้รับน้ำจำนวนหนึ่ง" },
        { EventObjectModel.Food, "ผลผลิตงอกงาม สามารถเก็บเกี่ยวอาหารได้มาก" },
        { EventObjectModel.Population, "บ้านเมืองเจริญรุ่งเรือง ผู้คนอพยพเข้าเมือง"},
        { EventObjectModel.Tree, "ป่าไม้อุดมสมบูรณ์ สร้างร่มเงาให้แก่มนุษย์และสัตว์"},
        { EventObjectModel.Wildfire, "กลุ่มนายพรายทิ้งเสษบุหรี่ ไฟจึงลามไหม้ป่า" },
        { EventObjectModel.flood, "ปริมาณน้ำสะสมมากเกินไป จนไหลท่วมบ้านเรือน" },
        { EventObjectModel.GroundCollapse, "โพรงใต้ดินทรุดตัว บ้านเมืองเกิดความเสียหาย" },
        { EventObjectModel.Desolation, "ขาดแคลนน้ำจนไม่เพียงพอต่อการบริโภค" },
        { EventObjectModel.Earthquake, "เปลือกโลกเกิดการสั่นสะเทือนฉับพลัน ทำให้บ้านเมืองเสียหาย" },
        { EventObjectModel.Typhoon, "พายุเข้าโจมตี ทรัพยากรและบ้านเมืองเสียหาย" },
        { EventObjectModel.Nothing, "ไม่มีเหตุการณ์อะไรเกิดขึ้น" }
    };

    public static Dictionary<string, string> EventHeaderList = new Dictionary<string, string>()
    {
        { EventObjectModel.Rain, "ฝนตก" },
        { EventObjectModel.Food, "เก็บเกี่ยวอาหาร" },
        { EventObjectModel.Population, "ประชากร"},
        { EventObjectModel.Tree, "ป่าไม้เจริญเติบโต"},
        { EventObjectModel.Wildfire, "ไฟป่า" },
        { EventObjectModel.flood, "น้ำท่วม" },
        { EventObjectModel.GroundCollapse, "ดินทรุดตัว" },
        { EventObjectModel.Desolation, "ภาวะแห้งแล้ง" },
        { EventObjectModel.Earthquake, "แผ่นดินไหว" },
        { EventObjectModel.Typhoon, "พายุไต้ฝุ่น" },
        { EventObjectModel.Nothing, "ไม่มีเหตุการณ์อะไรเกิดขึ้น" }
    };
}

public static class EventObjectModel
{
    public const string
        Rain = "RAIN",
        Food = "FOOD",
        Population = "POPULATION",
        Tree = "TREE",
        Wildfire = "WILDFIRE",
        flood = "FLOOD",
        GroundCollapse = "GROUNDCOLLAPSE",
        Desolation = "DESOLATION",
        Earthquake = "EARTHQUAKE",
        Typhoon = "TYPHOON",
        Nothing = "NOTHING";
}