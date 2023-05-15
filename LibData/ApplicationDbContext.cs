using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibData
{
    public class ApplicationDbContexts
    {
        NhaHangNVDEntities db = new NhaHangNVDEntities();
        public NhaHangNVDEntities ApplicationDbContext { get { return db; } }
    }
}