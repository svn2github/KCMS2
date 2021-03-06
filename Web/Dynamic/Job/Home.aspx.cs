﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Voodoo;
using Voodoo.Basement;

namespace Web.Dynamic.Job
{
    public partial class Home : BasePage
    {
        public string ResumeOpen = "";
        public string Image = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadInfo();
        }

        protected void LoadInfo()
        {
            User u = UserAction.opuser;
            if (u.ID < 0)
            {
                Js.AlertAndChangUrl("您还没有登录，请登录或注册后进入简历管理！", "/");
            }
            DataEntities ent = new DataEntities();
            JobResumeInfo r = new JobResumeInfo();
            try
            {
                r = (from l in ent.JobResumeInfo where l.UserID == u.ID select l).First();
            }
            catch
            {
                r.UserID = u.ID;
                r.IsResumeOpen = true;
                r.Image = "/u/ResumeFace/0.jpg";
                r.Title = u.UserName + "的简历";
                ent.AddToJobResumeInfo(r);
                ent.SaveChanges();
            }

            ResumeOpen = r.IsResumeOpen == true ? "简历完全开放" : "简历关闭";
            Image = r.Image;


            //var list = from l in ent.ViewHistory
            //           from com in ent.JobCompany
            //           from p in ent.JobPost
            //           where
            //           l.ItemID == p.ID
            //           && p.CompanyID == com.ID
            //           && l.ModelID == 5
            //           && l.UserID == u.ID
            //           orderby l.ViewTime descending
            //           select new
            //           {
            //               p.ID,
            //               CompanyID = com.ID,
            //               com.CompanyName,
            //               Pid = p.ID,
            //               p.Title,
            //               l.ViewTime
            //           };

            //var list = from l in ent.JobApplicationRecord
            //           from re in ent.JobResumeInfo
            //           from com in ent.JobCompany
            //           from p in ent.JobPost
            //           where
            //              l.UserID == u.ID
            //              && l.CompanyID == com.ID
            //              && p.CompanyID == com.ID
            //              && re.UserID==u.ID
            //              && re.WorkPlace==p.City
            //           select new
            //           {
            //               l.ID,
            //               l.CompanyID,
            //               com.CompanyName,
            //               Pid = p.ID,
            //               p.Title,
            //               l.ApplicationTime

            //           };
            var list = from l in ent.JobPost
                       from re in ent.JobResumeInfo
                       from com in ent.JobCompany
                       where
                          l.CompanyID == com.ID
                          && re.UserID == u.ID
                          && re.WorkPlace == l.City
                       select new
                       {
                           l.ID,
                           l.CompanyID,
                           l.Title,
                           l.Intro,
                           l.PostTime,
                           com.CompanyName
                       };

            var keywords = r.Keywords.ToS().Split(',', '，', ' ').ToList();
            foreach (var keyword in keywords)
            {
                list = from l in list where l.Title.Contains(keyword) || l.Intro.Contains(keyword) select l;
            }

            rp_lis.DataSource = list.OrderByDescending(p => p.ID).Take(10);
            rp_lis.DataBind();

            var list2 = from l in ent.ViewHistory
                        from com in ent.JobCompany
                        from p in ent.JobPost
                        where
                        l.ItemID == p.ID
                        && p.CompanyID == com.ID
                        && l.ModelID == 5
                        && l.UserID == u.ID
                        orderby l.ViewTime descending
                        select new
                        {
                            p.ID,
                            CompanyID = com.ID,
                            com.CompanyName,
                            Pid = p.ID,
                            p.Title,
                            l.ViewTime
                        };
            rp_lis2.DataSource = list2.OrderBy(p => p.Pid).Take(10);
            rp_lis2.DataBind();
        }
    }
}