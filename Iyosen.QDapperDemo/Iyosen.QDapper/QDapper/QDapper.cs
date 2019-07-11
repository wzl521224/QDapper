using Dapper;
using Microsoft.AspNetCore.Mvc;
using NReco.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Iyosen.QDapper
{
    public class QDapper
    {
        //[@:]\\w+|\\?\\w+\\?   =>  @xx和:xx用于匹配sqlserver、oracle、mysql、sqlite、pg|?xx?用于匹配access
        private static readonly Regex _regex = new Regex("[@:]\\w+|\\?\\w+\\?", RegexOptions.Compiled);
        private static readonly LambdaParser _lambdaParser = new LambdaParser();
        private static readonly ConcurrentDictionary<string, object> _variables = new ConcurrentDictionary<string, object>();

        public static List<object> QDapperData([FromQuery] Dictionary<string, string> param = null)
        {
            string sql="";
            var mappingId = param["mappingId"];
            var key = param["key"];

            param.Remove("mappingId");
            param.Remove("key");
            XmlDocument doc = new XmlDocument();
            XmlNode xn;
            var hasParameter = true;

            try
            {
                doc.Load(string.Format(@"./Config/Sql/{0}.xml", mappingId));
                xn = doc.SelectSingleNode("mappings");
            }
            catch (Exception e)
            {
                return new List<object> { e};
            }
            
            XmlNodeList xns = xn.ChildNodes;
            foreach (XmlNode x in xns)
            {
                XmlElement element = (XmlElement)x;
                
                if (element.GetAttribute("name") == key)
                {
                    XmlNodeList sql_if = element.SelectSingleNode("sql").SelectNodes("if");
                    foreach (XmlNode y in sql_if)
                    {
                        var matches = _regex.Matches(y.Attributes["test"].Value);
                        foreach (Match item in matches)
                        {
                            var matchName = item.Value.Trim("@".ToArray());
                            if ((param == null || (!param.ContainsKey(matchName))))
                            {
                                y.ParentNode?.RemoveChild(y);
                                hasParameter = false;
                                break;
                            }
                            if (!_variables.ContainsKey(matchName)) _variables.TryAdd(matchName, param[matchName]);

                            if (hasParameter)
                            {
                                var success = (bool)_lambdaParser.Eval(y.Attributes["test"].Value.Replace("@", ""), _variables);
                                if (!success) y.ParentNode?.RemoveChild(y);
                            }
                        }
                    }
                    sql = x.SelectSingleNode("sql").InnerText;
                }
            }
            var dbArgs = new DynamicParameters();
            foreach (var pair in param) dbArgs.Add(pair.Key, pair.Value);

            List<object> gs = DbContext.Query<object>(sql, dbArgs).ToList();
            return gs;
        }
    }
}
