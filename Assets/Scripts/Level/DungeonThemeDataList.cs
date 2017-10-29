using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonThemeDataList : ScriptableObject {
    public List<DungeonThemeData> DungeonThemeDatas;

    public List<DungeonThemeData> this[DungeonTheme boss] {
        get {
            List<DungeonThemeData> found = new List<DungeonThemeData>();
            foreach (DungeonThemeData data in DungeonThemeDatas)
                if (data.Theme == boss)
                    found.Add(data);
            return found;
        }
    }
}