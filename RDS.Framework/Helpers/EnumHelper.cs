using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace RDS.Framework.Helpers
{
    public class EnumJson
    {
        public int Id { get; set; }
        public string DisplayText { get; set; }
    }

    public static class EnumHelper
    {
        private static IEnumerable<TEnum> GetEnums<TEnum>() where TEnum : Enum
        {
            return (Enum.GetValues(typeof(TEnum)) as TEnum[]);
        }

        public static IEnumerable<TEnum> GetEnumsFromStr<TEnum>(string str) where TEnum : Enum
        {
            if (string.IsNullOrWhiteSpace(str))
                return new HashSet<TEnum>();

            return str.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Where(value => int.TryParse(value, out int enumInt) && Enum.IsDefined(typeof(TEnum), enumInt))
                .Select(value => (TEnum)Enum.Parse(typeof(TEnum), value, true));
        }

        public static string GetDisplayText<TEnum>(this TEnum @enum) where TEnum : Enum
        {
            var fi = @enum.GetType().GetField(@enum.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            var name = (attributes.Length > 0) ? attributes[0].Description : @enum.ToString();

            return name;
        }

        public static int GetId<TEnum>(this TEnum @enum) where TEnum : Enum
        {
            return (int)((object)@enum);
        }

        public static IEnumerable<string> GetDisplayTexts<TEnum>() where TEnum : Enum
        {
            return GetJsons<TEnum>().Select(x => x.DisplayText);
        }

        public static IEnumerable<int> GetIds<TEnum>() where TEnum : Enum
        {
            return GetJsons<TEnum>().Select(x => x.Id);
        }

        public static IEnumerable<EnumJson> GetJsons<TEnum>(IEnumerable<TEnum> enums = null) where TEnum : Enum
        {
            enums = enums ?? GetEnums<TEnum>();
            return enums.Select(x => new EnumJson
            {
                Id = x.GetId(),
                DisplayText = x.GetDisplayText()
            });
        }

        public static IEnumerable<EnumJson> GetJsonsFromStr<TEnum>(string str) where TEnum : Enum
        {
            var lst = GetEnumsFromStr<TEnum>(str);
            return GetJsons(lst);
        }

        public static bool IsDefine<TEnum>(int intValue) where TEnum : Enum
        {
            return GetIds<TEnum>().Contains(intValue);
        }
    }
}
