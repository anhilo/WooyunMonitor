using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Class.Common
{
    /// <summary>
    /// 关键词类
    /// </summary>
    public class KeywordHelper
    {
        private string[] _keywords_tele = { "电信", "移动", "联通", "运营商", "铁通", "中心", "天翼", "e家" };
        private string[] _keywords_bank = { "银行", "建行", "农行", "中行", "工行", "交行", "ccb", "cbc", "boc", "金融" };
        private string[] _keywords_gov = { "政府", "公安", "局", "厅", "烟草", "能源", "电力", "电网", "工业" };
        private string[] _keywords_geo = { "广西", "南宁", "桂林", "柳州", "梧州", "白色", "北海", "防城", "钦州", "玉林", "河池", };

        /// <summary>
        /// 是否包含指定关键字
        /// 暂时只有运营商测试
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool isContainsKeyword(string input)
        {
            foreach (string item in _keywords_tele)
            {
                if (input.IndexOf(item.Trim().ToLower()) > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断一个wooyuninfo是否存在keyword
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool isContainsKeyword(string[] input)
        {
            foreach (string item in input)
            {
                if (isContainsKeyword(item.Trim().ToLower()))
                {
                    return true;
                }
            }
            return false;
        }
    }
}