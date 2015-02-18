using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Class.Common
{
    /// <summary>
    /// wooyun正则
    /// </summary>
    public class RegexWooyun
    {
        #region regexstring
        /// <summary>
        /// 获取bugid  得到"wooyun-xxxx-xxxxxx"格式的字符串，可能重复
        /// </summary>
        private string regex_GetBugIDList = "(?!href=\"/bugs/)wooyun-\\d{4}-\\d{6}";

        /// <summary>
        /// 从漏洞细节获取标题，得到形如"漏洞标题：	 工商银行旗下某基金分站存在命令执行   "
        /// </summary>
        private string regex_GetBugTitle = "(?=漏洞标题：).+(?=</h3>)";
        /// <summary>
        /// 获取漏洞相关厂商，得到"<a href="http://www.wooyun.org/corps/工商银行">"
        /// </summary>
        private string regex_GetBugCompany = "(?<=<a href=\"http://www.wooyun.org/corps/).+(?=\">)";
        /// <summary>
        /// 获取漏洞状态，得到"<h3>漏洞状态：已交由第三方厂商(cncert国家互联网应急中心)处理"
        /// </summary>
        private string regex_GetBugStatus = "<h3>漏洞状态：.+\n.+\n";
        /// <summary>
        /// 获取漏洞描述，<p class="detail">工商银行某分站存在命令执行</p>
        /// 可能会得到2行，取第一行
        /// </summary>
        private string regex_GetBugDescription = "简要描述：[\\s\\S]*详细说明：";
        #endregion

        /// <summary>
        /// 从目录页收集BUGID
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<string> MatchBugIDList(string input)
        {
            Regex reg = new Regex(regex_GetBugIDList);
            var matches = reg.Matches(input);
            if (matches.Count > 0)
            {
                List<string> result = new List<string>();
                foreach (Match item in matches)
                {
                    result.Add(item.Value);
                }
                return result.Distinct().ToList(); ;
            }
            return null;
        }

        /// <summary>
        /// 漏洞相关厂商
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string MatchBugComp(string input)
        {
            return Match(input, regex_GetBugCompany)
                .Replace("<a href=\"http://www.wooyun.org/corps/", string.Empty)
                .Replace("\">", string.Empty)
                .Trim();
        }

        /// <summary>
        /// 漏洞状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string MatchBugStatus(string input)
        {
            return Match(input, regex_GetBugStatus);
        }

        /// <summary>
        /// 漏洞描述
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string MatchBugDes(string input)
        {
            var r = Match(input, regex_GetBugDescription)
                .Replace("简要描述：", string.Empty)
                .Replace("<p class=\"detail\">", string.Empty)
                .Replace("</p>", string.Empty)
                .Replace("<br />", string.Empty)
                .Replace("</h3>", string.Empty)
                .Replace("<h3 class=\"detailTitle\">详细说明：", string.Empty)
                .Trim();
            return r.IndexOf("危害等级") > 0 ? "无" : r;

        }

        /// <summary>
        /// 漏洞标题
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string MatchBugTitle(string input)
        {
            return Match(input, regex_GetBugTitle).Replace("漏洞标题：", string.Empty).Trim();
        }


        /// <summary>
        /// bug细节页总方法
        /// </summary>
        /// <param name="input"></param>
        /// <param name="regstring"></param>
        /// <returns></returns>
        private string Match(string input, string regstring)
        {
            Regex reg = new Regex(regstring);
            var match = reg.Match(input);
            return match.Success ? match.Value : string.Empty;
        }
    }
}