using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Class.TimeTask
{
    /// <summary>
    /// 设置每隔一段时间自动执行
    /// </summary>
    public class TimeTask
    {
        /// <summary>
        /// 到点执行的时间
        /// </summary>
        public event System.Timers.ElapsedEventHandler ExecuteTask;
        private static readonly TimeTask _task = null;
        private System.Timers.Timer _timer = null;
        private int _interval = 60;

        /// <summary>
        /// 间隔
        /// </summary>
        public int Interval
        {
            set
            {
                _interval = value;
                _timer.Interval = value;
            }
            get
            {
                return _interval;
            }
        }

        static TimeTask()
        {
            _task = new TimeTask();
        }

        /// <summary>
        /// 获取对象实体
        /// </summary>
        /// <returns></returns>
        public static TimeTask Instance()
        {
            return _task;
        }

        /// <summary>
        /// 开始计时
        /// </summary>
        public void Start()
        {
            _timer.Interval = this.Interval;
            _timer.Start();
        }

        protected void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var midnight = Convert.ToDateTime("00:00");
            var ts = DateTime.Now.Subtract(midnight).Duration();
            if ((ts.Minutes <= 5 && ts.Minutes > 0) && (null != ExecuteTask))
            {
                ExecuteTask(sender, e);
            }
        }

        /// <summary>
        /// 停止计时
        /// </summary>
        public void Stop()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
            }
        }

        public void Init()
        {
            if (_timer == null)
            {
                _timer = new System.Timers.Timer(_interval);
                _timer.Elapsed += new System.Timers.ElapsedEventHandler(_timer_Elapsed);
                _timer.Enabled = true;
            }
            var getinfo = new GetWooyunInfo();
            this._timer.Elapsed += getinfo.DengDaiRenLing;
            this._timer.Elapsed += getinfo.LouDongYuJing;
            this._timer.Elapsed += getinfo.ZuiXinGongKai;
            this._timer.Elapsed += getinfo.ZuiXinQueRen;
            this._timer.Elapsed += getinfo.ZuiXinTiJiao;
        }


    }
}