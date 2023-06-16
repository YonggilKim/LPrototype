using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEngine;
using System.Linq;
using Unity.Plastic.Newtonsoft.Json;
using Data;
using System.ComponentModel;
using static Define;

public class DataTransformer : EditorWindow
{
#if UNITY_EDITOR
    #region Functions
    [MenuItem("Tools/DeleteGameData ")]
    public static void DeleteGameData()
    {
        PlayerPrefs.DeleteAll();
        string path = Application.persistentDataPath + "/SaveData.json";
        if (File.Exists(path))
            File.Delete(path);
    }

    public static T ConvertValue<T>(string value)
    {
        if (string.IsNullOrEmpty(value))
            return default(T);

        TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
        return (T)converter.ConvertFromString(value);
    }

    public static List<T> ConvertList<T>(string value)
    {
        if (string.IsNullOrEmpty(value))
            return new List<T>();

        return value.Split('&').Select(x => ConvertValue<T>(x)).ToList();
    }
    #endregion

    [MenuItem("Tools/ParseExcel %#K")]
    public static void ParseExcel()
    {
        ParseCreatureData("Creature");
        Debug.Log("Complete DataTransformer");
    }

    static void ParseCreatureData(string filename)
    {
        CreatureDataLoader loader = new CreatureDataLoader();

        #region ExcelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');

            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;
            CreatureData cd = new CreatureData();
            cd.DataId = ConvertValue<int>(row[i++]);
            cd.DescriptionTextID = ConvertValue<string>(row[i++]);
            cd.PrefabLabel = ConvertValue<string>(row[i++]);
            cd.MaxHp = ConvertValue<float>(row[i++]);
            cd.MaxHpBonus = ConvertValue<float>(row[i++]);
            cd.Atk = ConvertValue<float>(row[i++]);
            cd.AtkBonus = ConvertValue<float>(row[i++]);
            cd.Def = ConvertValue<float>(row[i++]);
            cd.MoveSpeed = ConvertValue<float>(row[i++]);
            cd.TotalExp = ConvertValue<float>(row[i++]);
            cd.HpRate = ConvertValue<float>(row[i++]);
            cd.AtkRate = ConvertValue<float>(row[i++]);
            cd.DefRate = ConvertValue<float>(row[i++]);
            cd.AtkRange = ConvertValue<float>(row[i++]);
            cd.CloneCount = ConvertValue<int>(row[i++]);
            cd.SkelotonDataID = ConvertValue<string>(row[i++]);

            cd.SpineSkinName = ConvertValue<string>(row[i++]);
            cd.AnimIdle = ConvertValue<string>(row[i++]);
            cd.AnimMove = ConvertValue<string>(row[i++]);
            cd.AnimAttack = ConvertValue<string>(row[i++]);
            cd.AnimSkillA = ConvertValue<string>(row[i++]);
            cd.AnimSkillB = ConvertValue<string>(row[i++]);
            cd.AnimSkillC = ConvertValue<string>(row[i++]);
            cd.AnimUltimate = ConvertValue<string>(row[i++]);
            cd.AnimDamaged = ConvertValue<string>(row[i++]);
            cd.AnimDead = ConvertValue<string>(row[i++]);
            cd.IconLabel = ConvertValue<string>(row[i++]);
            cd.SkillTypeList = ConvertList<int>(row[i++]);
            loader.creatures.Add(cd);
        }

        #endregion

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
        AssetDatabase.Refresh();
    }

#endif

}