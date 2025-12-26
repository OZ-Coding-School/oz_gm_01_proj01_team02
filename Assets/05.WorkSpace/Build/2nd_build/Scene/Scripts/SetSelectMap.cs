using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSelectMap : MonoBehaviour
{
    public static SetSelectMap Instance;

    public MapData SelectedMap;

    public void SetSelectedMap(MapData data)
    {
        SelectedMap = data;

    }
}
