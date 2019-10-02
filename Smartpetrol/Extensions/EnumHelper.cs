using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Smartpetrol.Extensions
{
    public static class EnumHelper
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            var attr = enumValue.GetType()
                .GetMember(enumValue.ToString())
                .FirstOrDefault();
            if (attr == null) return "";
            return attr.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
        }

        public static List<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).OfType<T>().ToList();
        }

        public static List<T> ToEnumList<T>(this ICollection<string> valuesList)
        {
            return GetValues<T>().Where(x => valuesList.Contains(x.ToString())).ToList();
        }
    }
}