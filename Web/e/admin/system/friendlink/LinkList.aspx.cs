﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Voodoo;
using Voodoo.Data;
using Voodoo.Basement;
using Voodoo.Setting;


namespace Web.e.admin.system.friendlink
{
    public partial class LinkList : AdminBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (WS.RequestString("action") == "del")
            {
                btn_Del_Click(sender, e);
            }
            if (!IsPostBack)
            {
                BindList();
            }
        }

        protected void BindList()
        {
            using (DataEntities ent = new DataEntities())
            {
                rp_list.DataSource = from l in ent.Link select l;
                rp_list.DataBind();
            }
        }

        protected void btn_Del_Click(object sender, EventArgs e)
        {
            DataEntities ent = new DataEntities();

            var ids = WS.RequestString("id").Split(',').ToList();
            var qs = from l in ent.Link where ids.IndexOf(l.ID.ToString()) > 0 select l;
            foreach (var q in qs)
            {
                ent.DeleteObject(q);
            }
            ent.SaveChanges();
            ent.Dispose();

            BindList();

        }
    }
}
