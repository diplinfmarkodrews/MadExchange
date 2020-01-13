using System.Text;

namespace MadXchange.Common.Mex.Specification
{
    public static class ParameterStringSerializer
    {
        public static string Serialize<T>(T o)
        {
            var objType = typeof(T);
            var props = objType.GetProperties();
            StringBuilder sb = new StringBuilder();
            foreach (var item in props)
            {
                var it = item.GetValue(o, null);
                if (it != null)
                {
                    sb.Append($"&{item.Name}={it.ToString().ToLower()}");
                }
            }
            return sb.ToString();
        }
    }
}
