using Class.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Class.TimeTask
{
    /// <summary>
    /// 从wooyun读漏洞信息
    /// </summary>
    public class GetWooyunInfo
    {
        /// <summary>
        /// 最新提交的事件
        /// </summary>
        public void ZuiXinTiJiao(object sender, System.Timers.ElapsedEventArgs e)
        {

        }

        /// <summary>
        /// 最新提交
        /// </summary>
        /// <returns></returns>
        public List<string> GetZuiXinTiJiao()
        {
            var help = new HttpHelper();
            var html = help.getHtmlByHttpGET("http://wooyun.org/bugs/new_submit/");
            RegexWooyun reg = new RegexWooyun();
            return reg.MatchBugIDList(html);
        }



        /// <summary>
        /// 最新确认
        /// </summary>
        public void ZuiXinQueRen(object sender, System.Timers.ElapsedEventArgs e)
        {
            return;
        }

        /// <summary>
        /// 最新公开
        /// </summary>
        public void ZuiXinGongKai(object sender, System.Timers.ElapsedEventArgs e)
        {
            return;
        }

        /// <summary>
        /// 漏洞预警
        /// </summary>
        public void LouDongYuJing(object sender, System.Timers.ElapsedEventArgs e)
        {
            return;
        }

        /// <summary>
        /// 等待认领
        /// </summary>
        public void DengDaiRenLing(object sender, System.Timers.ElapsedEventArgs e)
        {

        }

        /// <summary>
        /// 根据wooyun-xxxx-xxxxxx获取漏洞细节
        /// </summary>
        /// <param name="bugid"></param>
        /// <returns>0bugid 1标题 2相关厂商 3简要描述</returns>
        public string[] GetBugInfo(string bugid)
        {
            var help = new HttpHelper();
            var html = help.getHtmlByHttpGET("http://wooyun.org/bugs/" + bugid);
            RegexWooyun reg = new RegexWooyun();
            string[] result = new string[4];
            result[0] = bugid;
            result[1] = reg.MatchBugTitle(html);
            result[2] = reg.MatchBugComp(html);
            result[3] = reg.MatchBugDes(html);
            return result;
        }
    }
}