// DataManager.cs
using UnityEngine;
using System.Collections.Generic;
using AXitUnityTemplate.DataRender.CSV;

public class DataManagerr : MonoBehaviour
{
    private void Start()
    {
        LoadAndDisplayData<InventoryData>("example");
    }

    public void LoadAndDisplayData<T>(string fileName) where T : ICSVConvertible, new()
    {
        CsvReader csvReader = new CsvReader();
        List<T>   data      = csvReader.ReadData<T>(fileName);

        foreach (var item in data)
        {
            item.DisplayData();
        }
    }
}