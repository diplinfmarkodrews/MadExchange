using Microsoft.Extensions.Configuration;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace MadXchange.Exchange.Helpers
{
    public static class StringObjectConfigToolkit
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static StringDictionary SelectKeysAndMarkedNames(this StringDictionary dict, IConfigurationSection section)
        //{
        //    if (dict is null)
        //        dict = new StringDictionary();

        //    foreach (var s in section.GetChildren())
        //    {
                
        //        var split = s.Value.Split('{', '}', options: StringSplitOptions.RemoveEmptyEntries).Where(sp => sp.All(Char.IsLetter));
                
                
        //    }

        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsMarked(this string input, char sign) => input.StartsWith(sign);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string RemoveMarkers(this string input, char sign)
        {
            char signB;
            if (sign == '[')
                signB = ']';

            signB = '}';
            input = input.Split(sign, signB, options: StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
            return input;

        }
        /// <summary>
        /// returns dict with only letter strings as keys
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="configString"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringDictionary MarkedTypeNameDictStringWSignValues(this StringDictionary dict, string configString)
        {

            if (dict is null)
                dict = new StringDictionary();

            var split = configString.Split('{', '}');
            var typenames = split.Where(s => s.All(Char.IsLetter));
            var strContext = split.Where(s => !s.All((Char.IsLetter)));
            for (int i = 0; i < typenames.Count(); i++)
            {
                dict.Add(typenames.ElementAt(i), strContext.ElementAtOrDefault(i));
            }
            return dict;
        }
        /// <summary>
        /// Selects lower letter attributes of keys and puts them with their value in a new dictionary
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="configString"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringDictionary SelectLowerletter(this StringDictionary dict)
        {
            
            var result = new StringDictionary();            
            for (int i = 0; i < dict.Count(); i++)
            {
                if (dict.Keys.ElementAt(i).StartsWith(dict.Keys.ElementAt(i).ToLower()))
                    result.Add(dict.Keys.ElementAt(i), dict.Values.ElementAt(i));
            }
            return result;
        }
        /// <summary>
        /// returns true if a given configsection value is an array which consist of more than 1 elems
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValueArray(IConfigurationSection section)
            => section.GetValue<string[]>(section.Key)?.Length > 1;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsMarkedTypeValue(this KeyValuePair<string, string> elem) => elem.Value.StartsWith('{');
           /// <summary>
           /// 
           /// </summary>
           /// <param name="config"></param>
           /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<string, object> GetAllUperCaseOf(IConfigurationSection config)
        {
            var result = new ObjectDictionary();
            var keyDict = config.AsEnumerable(true).ToObjectDictionary();
            return keyDict.MergeIntoObjectDictionary();
        }

    }
}
