// InventoryData.cs

using System;
using System.Collections.Generic;
using UnityEngine;

public interface ICSVConvertible
{
    void FromCSV(string[] values, string[] headers);
    void DisplayData(); // Thêm phương thức này để hiển thị dữ liệu
}

public class InventoryData : ICSVConvertible
{
    public List<ItemData> Items { get; set; } = new List<ItemData>();

    public void FromCSV(string[] values, string[] headers)
    {
        ItemData item = new ItemData();
        item.FromCSV(values, headers);
        Items.Add(item);
    }

    public void DisplayData()
    {
        foreach (var item in Items)
        {
            item.DisplayData();
        }
    }
}

public class ItemData : ICSVConvertible
{
    public string Id { get; set; }
    public int Capacity { get; set; }
    public List<TypeData> Types { get; set; } = new List<TypeData>();

    public void FromCSV(string[] values, string[] headers)
    {
        Id = GetValue(values, headers, "Id");
        Capacity = int.TryParse(GetValue(values, headers, "Capacity"), out int capacity) ? capacity : 0;

        TypeData typeData = new TypeData();
        typeData.FromCSV(values, headers);
        Types.Add(typeData);
    }

    public void DisplayData()
    {
        Debug.Log($"Item Id: {Id}, Capacity: {Capacity}");
        foreach (var type in Types)
        {
            type.DisplayData();
        }
    }

    private string GetValue(string[] values, string[] headers, string columnName)
    {
        int index = Array.IndexOf(headers, columnName);
        return (index >= 0 && index < values.Length) ? values[index] : string.Empty;
    }
}

public class TypeData : ICSVConvertible
{
    public string Type { get; set; }
    public string ArrowId { get; set; }
    public Color Color { get; set; }

    public void FromCSV(string[] values, string[] headers)
    {
        Type = GetValue(values, headers, "Type");
        ArrowId = GetValue(values, headers, "ArrowId");

        string colorString = GetValue(values, headers, "Color");
        Color = ParseColor(colorString);
    }

    private string GetValue(string[] values, string[] headers, string columnName)
    {
        int index = Array.IndexOf(headers, columnName);
        return (index >= 0 && index < values.Length) ? values[index] : string.Empty;
    }

    private Color ParseColor(string colorString)
    {
        if (string.IsNullOrEmpty(colorString)) return new Color(0, 0, 0, 0);

        string[] colorComponents = colorString.Split('|');
        if (colorComponents.Length != 4) return new Color(0, 0, 0, 0);

        float r = float.TryParse(colorComponents[0], out r) ? r : 0;
        float g = float.TryParse(colorComponents[1], out g) ? g : 0;
        float b = float.TryParse(colorComponents[2], out b) ? b : 0;
        float a = float.TryParse(colorComponents[3], out a) ? a : 1;
        return new Color(r, g, b, a);
    }

    public void DisplayData()
    {
        Debug.Log($"Type: {Type}, ArrowId: {ArrowId}, Color: {Color}");
    }
}
