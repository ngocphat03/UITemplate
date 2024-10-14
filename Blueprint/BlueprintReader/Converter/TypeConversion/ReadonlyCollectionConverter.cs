namespace AXitUnityTemplate.Blueprint.BlueprintReader.Converter.TypeConversion
{
    using System;
    using System.Collections.Generic;
    using AXitUnityTemplate.Blueprint.BlueprintReader.Converter;

    public class ReadonlyCollectionConverter : ITypeConverter
    {
        public object ConvertFromString(string text, Type typeInfo)
        {
            return Activator.CreateInstance(typeInfo, CsvHelper.TypeConverterCache.GetConverter(typeof(List<>)).ConvertFromString(text, typeInfo));
        }

        public string ConvertToString(object value, Type typeInfo)
        {
            throw new NotImplementedException();
        }
    }
}