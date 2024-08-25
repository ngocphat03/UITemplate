namespace AXitUnityTemplate.DataRender.CSV
{
    using System;
    using System.IO;
    using UnityEngine;
    using System.Reflection;
    using System.Collections.Generic;

    public class CsvReader
    {
        public List<T> ReadData<T>(string fileName) where T : new()
        {
            List<T> dataList = new List<T>();

            // Load the CSV file from Resources
            TextAsset csvFile = Resources.Load<TextAsset>("Data/" + fileName);
            if (csvFile == null)
            {
                Debug.LogError("CSV file not found");

                return dataList;
            }

            StringReader reader   = new StringReader(csvFile.text);
            bool         isHeader = true;
            string[]     headers  = null;

            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();
                if (isHeader)
                {
                    headers  = line.Split(',');
                    isHeader = false; // Skip the header line

                    continue;
                }

                string[] values = line.Split(',');

                T instance = new T();
                for (int i = 0; i < headers.Length; i++)
                {
                    string header = headers[i];
                    string value  = values[i];

                    PropertyInfo property = typeof(T).GetProperty(header, BindingFlags.Public | BindingFlags.Instance);
                    if (property != null && property.CanWrite)
                    {
                        object convertedValue = Convert.ChangeType(value, property.PropertyType);
                        property.SetValue(instance, convertedValue, null);
                    }
                }

                dataList.Add(instance);
            }

            return dataList;
        }
    }
}