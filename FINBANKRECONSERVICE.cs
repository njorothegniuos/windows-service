using FINBANKRECONSERVICE;
using FINBANKRECONSERVICE.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FINBANKRECONSERVICE
{
    public partial class FINBANKRECONSERVICE : ServiceBase
    {
        private Timer timer;
        private int INTERVAL = 10;
        private Db db;
        public FINBANKRECONSERVICE()
        {
            InitializeComponent();
            db = new Db(Util.GetDbConnString());
           // StartTheTimer();
        }

        protected override void OnStart(string[] args)
        {
            //----- Start timer
            StartTheTimer();
        }


        protected override void OnStop()
        {
            this.timer.Dispose();
        }

        private void StartTheTimer()
        {
            try
            {
                timer = new Timer(new TimerCallback(TimerCallback));
                DateTime runTime = DateTime.Now.AddSeconds(INTERVAL);
                if (DateTime.Now > runTime)
                {
                    //If Scheduled Time is passed set Schedule for the next Interval.
                    runTime = runTime.AddSeconds(INTERVAL);
                }

                TimeSpan timeSpan = runTime.Subtract(DateTime.Now);
                //Get the difference in Minutes between the Scheduled and Current Time.
                int dueTime = Convert.ToInt32(timeSpan.TotalMilliseconds);

                //Change the Timer's Due Time.
                timer.Change(dueTime, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                Util.LogError("StartTheTimer", ex);
            }
        }

        private void TimerCallback(object e)
        {

            try
            {
                //1.read file and save to db
                string responseXml = "";
                var queryResp = new DeclarationQueryResponse();
                ////===================================
                var data = db.GetSettings(100);
                responseXml = File.ReadAllText(data.Data1);
                ////=====================================
                XDocument xml = XDocument.Parse(responseXml);
                var myData = xml.Descendants().Where(x => x.Name.LocalName == "getDeclarationDetailsResult").FirstOrDefault();
                if (myData != null)
                {
                            queryResp.OfficeCode = (string)myData.Element("officeCode");
                            queryResp.OfficeName = (string)myData.Element("officeName");

                }
                //save to db
                db.insertfiledata(queryResp);
            }
            catch (Exception ex)
            {
                Util.LogError("TimerCallback", ex);
            }

            this.StartTheTimer();
        }
    }
}
