using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHolder : MonoBehaviour
{
    [SerializeField] private List<HiddenObjectData> hiddenObjectList;   //list of all the hiddenObjects available in the scene

    public List<HiddenObjectData> HiddenObjectList { get => hiddenObjectList;}

    public void ArrangeList()
    {
        hiddenObjectList = new List<HiddenObjectData>();

        for (int i = 0; i < transform.childCount; i++)
        {
            HiddenObjectData hiddenObjectData = new HiddenObjectData();
            hiddenObjectData.hiddenObj = transform.GetChild(i).gameObject;
            hiddenObjectData.name = transform.GetChild(i).name;
            hiddenObjectData.makeHidden = false;

            hiddenObjectList.Add(hiddenObjectData);
        }
    }

}
