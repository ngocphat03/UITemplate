using System.Collections.Generic;

namespace AXitUnityTemplate.DataRender.CSV
{
    public static class DataManager
    {
        public static List<T> LoadAndDisplayData<T>(string fileName) where T : new()
        {
            var csvReader = new CsvReader();

            return csvReader.ReadData<T>(fileName);
        }
    }
}