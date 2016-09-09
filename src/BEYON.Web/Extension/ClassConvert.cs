using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Formatting;
using System.Reflection;

namespace BEYON.Web
{
    public static class ClassConvert<T>
    {
        public static T[] Parse(FormDataCollection formData)
        {
            return null;
        }

        public static T[] Process(NameValueCollection data = null)
        {
            var list = new List<KeyValuePair<string, string>>();

            if (data != null)
            {
                foreach (var key in data.AllKeys)
                {
                    list.Add(new KeyValuePair<string, string>(key, data[key]));
                }
            }

            var datas = HttpData(list);
            if (!datas.ContainsKey("data"))
                return null;
            Dictionary<String, object> items = datas["data"] as Dictionary<String, object>;
            if (items == null)
                return null;
            List<T> results = new List<T>();
            foreach(var item in items)
            {
                T t = System.Activator.CreateInstance<T>();
                PropertyInfo[] pc = t.GetType().GetProperties();
                PropertyInfo key = pc.Where(pr=>pr.Name == "Id").FirstOrDefault();
                if(key != null)
                {
                    int lastIndex = item.Key.LastIndexOf('_');
                    if(lastIndex > -1)
                    {
                        key.SetValue(t, Int32.Parse(item.Key.Substring(lastIndex+1, item.Key.Length - lastIndex-1)));
                    }
                }

                Dictionary<String, object> values = item.Value as Dictionary<String, object>;
                foreach (PropertyInfo pi in pc)
                {
                    if (values.ContainsKey(pi.Name))
                    {
                        MethodInfo info = pi.GetSetMethod();
                        if (info == null || info.IsPrivate)
                            continue;
                        var value = values[pi.Name];
                        if (pi.PropertyType.Equals(typeof(string)))
                        {
                            pi.SetValue(t, value.ToString());
                        }
                        else if(pi.PropertyType.Equals(typeof(DateTime)))
                        {
                            try
                            {
                                pi.SetValue(t, DateTime.Parse(value.ToString()));
                            }
                            catch
                            {

                            }
                            
                        }
                        else
                        {
                            pi.SetValue(t, value);
                        }
                        
                    }
                }
                results.Add(t);
            }

            return results.ToArray();
        }

        /// <summary>
        /// Convert HTTP request data, in the standard HTTP parameter form
        /// submitted by jQuery into a generic dictionary of string / object
        /// pairs so the data can easily be accessed in .NET.
        ///
        /// This static method is generic and not specific to the DtRequest. It
        /// may be used for other data formats as well.
        /// 
        /// Note that currently this does not support nested arrays or objects in arrays
        /// </summary>
        /// <param name="dataIn">Collection of HTTP parameters sent by the client-side</param>
        /// <returns>Dictionary with the data and values contained. These may contain nested lists and dictionaries.</returns>
        private static Dictionary<string, object> HttpData(IEnumerable<KeyValuePair<string, string>> dataIn)
        {
            var dataOut = new Dictionary<string, object>();

            if (dataIn != null)
            {
                foreach (var pair in dataIn)
                {
                    var value = _HttpConv(pair.Value);

                    if (pair.Key.Contains('['))
                    {
                        var keys = pair.Key.Split('[');
                        var innerDic = dataOut;
                        string key;

                        for (int i = 0, ien = keys.Count() - 1; i < ien; i++)
                        {
                            key = keys[i].TrimEnd(']');
                            if (key == "")
                            {
                                // If the key is empty it is an array index value
                                key = innerDic.Count().ToString();
                            }

                            if (!innerDic.ContainsKey(key))
                            {
                                innerDic.Add(key, new Dictionary<string, object>());
                            }
                            innerDic = innerDic[key] as Dictionary<string, object>;
                        }

                        key = keys.Last().TrimEnd(']');
                        if (key == "")
                        {
                            key = innerDic.Count().ToString();
                        }

                        innerDic.Add(key, value);
                    }
                    else
                    {
                        dataOut.Add(pair.Key, value);
                    }
                }
            }

            return dataOut;
        }

        private static object _HttpConv(string dataIn)
        {
            // Boolean
            if (dataIn == "true")
            {
                return true;
            }
            if (dataIn == "false")
            {
                return false;
            }

            // Numeric looking data, but with leading zero
            if (dataIn.IndexOf('0') == 0 && dataIn.Length > 1)
            {
                return dataIn;
            }

            try
            {
                return Convert.ToInt32(dataIn);
            }
            catch (Exception) { }

            try
            {
                return Convert.ToDecimal(dataIn);
            }
            catch (Exception) { }

            return dataIn;
        }

    }
}
