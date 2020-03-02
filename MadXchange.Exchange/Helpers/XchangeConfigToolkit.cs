using MadXchange.Exchange.Contracts;
using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Types;
using Microsoft.Extensions.Configuration;
using Serilog;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

namespace MadXchange.Exchange.Helpers
{
    public static class XchangeConfigToolkit
    {

      
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
            else
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
        /// Selects lower letter attributes of key named properties and adds it with its value in a new dictionary
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
        /// checks whether the property is writing with brackets, assigning a type
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsMarkedTypeValue(this KeyValuePair<string, string> elem) => elem.Value.StartsWith('{');
        /// <summary>
        /// Finds all properties with capital letters
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
        /// <summary>
        /// Reads all endpoints from a given ConfigurationSection
        /// </summary>
        /// <param name="endPointDic"></param>
        /// <param name="routes"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<string, EndPoint> ReadExchangeEndPoints(this Dictionary<string, EndPoint> endPointDic, IConfigurationSection routes)
        {
            
            if(endPointDic == default)
                endPointDic = new Dictionary<string, EndPoint>();
            var preFix = routes.GetChildren();
            foreach (var route in preFix)            
                foreach (var r in route.GetChildren())                                                    
                    endPointDic.Add($"{route.Key}{r.Key}", ReadEndPoint(r, route.Key));                
                        
            return endPointDic;
        }
        /// <summary>
        /// Reads a single Endpoint from the given ConfigurationSection
        /// </summary>
        /// <param name="cSection"></param>
        /// <returns></returns>
        public static EndPoint ReadEndPoint(IConfigurationSection cSection, string method)
        {
            var endP = new EndPoint();
            endP.Url = cSection.GetSection("Url")?.Value;
            endP.Name = cSection.Key;
            endP.Method = method;
            var paramSection = cSection.GetSection("Parameter").GetChildren();
            var parameterCount = paramSection.Count();
            if (parameterCount > 0)
            {
                endP.Parameter = new Dictionary<string, Parameter>();
                for (int i = 0; i < parameterCount; i++)
                {
                    var paramAt = paramSection.ElementAt(i);
                    string domain = paramAt.GetSection("Domain")?.Value ?? paramAt.Key;
                    bool? isRequired = paramAt.GetValue<bool>("Required");
                    string type = paramAt.GetSection("Type")?.Value ?? string.Empty;
                    var parameter = new Parameter()
                    {
                        ExtName = paramAt.Key,
                        Required = isRequired.GetValueOrDefault(false),
                        Type = type
                    };
                    
                    endP.Parameter.Add(domain, parameter);
                }
            }
            endP.Result = cSection.GetSection("Result")?.Value ?? cSection.Key;
            return endP;
        }
       
        /// <summary>
        /// Read a type from a preselected ConfigurationSection
        /// </summary>
        /// <param name="typeSection"></param>
        /// <returns></returns>
        
        private static StringDictionary ReadType(IConfigurationSection typeSection)
        {
            StringDictionary result = new StringDictionary();
            foreach (var member in typeSection.GetChildren())            
                if (string.IsNullOrEmpty(member.Value))
                    result.Add(member.Key.ToCamelCase(), member.Key);
                else
                    result.Add(member.Value, member.Key);
            
            return result;
        }
        
        public static void SetEndPointReturnTypes(this XchangeDescriptor descriptor) 
        {
            foreach (var endP in descriptor.EndPoints)
            {
                if (endP is null) continue;
                var typename = endP.Result;  
                if(descriptor.DomainTypes.ContainsKey(typename))
                    endP.ReturnType = descriptor.DomainTypes[typename];                
                if(descriptor.DomainTypes.ContainsKey(typename+"Dto"))
                    endP.ReturnType = descriptor.DomainTypes[typename + "Dto"];
            }
        }

        /// <summary>
        /// Generates DomainTypes of an exchange configuration section, uses contract classes, which are read and added automatically 
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        
        public static Dictionary<string, Type> GenerateTypesDictionary(Dictionary<string, Type> typeDic, IConfigurationSection section)
        {
            var typeDicRes = new Dictionary<string, Type>();
            var domainDic = ReadTypeDictionary(section);
            foreach (var type in domainDic)
            {
                if (typeDic.ContainsKey(type.Key))
                {
                    var dt = typeDic[type.Key];
                   // var dtGeneric = dt.GetType();
                    //fetch all serializable attributes of a type, properties defined in the contract poco's
                    var serializables = dt.GetSerializableProperties();
                    //create generic type definition to save in XchangeDescriptor
                    //var genericTypeDefinition = dt.GetTypeInfo().GetGenericTypeDefinition();
                    //genericTypeDefinition.
                    foreach (var s in serializables)
                    {
                        try
                        {
                            if (domainDic[type.Key].ContainsKey(s.Name))
                                s.AddAttributes(new DataMemberAttribute() { Name = domainDic[type.Key][s.Name].ToString() });
                        }
                        catch
                        {
                            //Todo: mark properties which aren't covered by the type description
                            //=> additive fields depending on exchange. our type descriptors only cover the type properties needed for our trade engine
                        }
                    }
                    typeDicRes.Add(dt.Name, dt);
                }

                ///Todo create gneric DataContractType
                ///add members with DataMember attribute

            }
            return typeDicRes;
        }

        /// <summary>
        /// Reads all DomainTypes from a ExchangeConfigurationSection
        /// </summary>
        /// <param name="configurationSection"></param>
        /// <returns></returns>

        public static Dictionary<string, StringDictionary> ReadTypeDictionary(IConfiguration configurationSection)
        {
            var result = new Dictionary<string, StringDictionary>();
            var configTypes = configurationSection.GetChildren();
            foreach (var type in configTypes)
                result.Add(type.Key, ReadType(type));

            return result;
        }

        private static IEnumerable<Type> ScanAssemblyForDataContracts()
        {
            var result = new List<Type>();
            Assembly assembly = typeof(InstrumentDto).Assembly;
            foreach (var type in assembly.GetTypes())
                if (type.HasAttribute<DataContractAttribute>())
                    result.Add(type);

            return result;
        }

        public static Dictionary<string, Type> GenerateTypeDictionary()
        {
            var result = new Dictionary<string, Type>();
            var contractAssemblies = ScanAssemblyForDataContracts();
            foreach (var c in contractAssemblies)
            {
                var name = c.Name;
                if (name.EndsWith("Dto"))
                    name = name.Remove(name.Length - 3);

                result.Add(name, c);
            }
            return result;
        }

    }
}
