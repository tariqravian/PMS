﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMS.Web.Controllers
{
    public class LandLordController : Controller
    {
        // GET: LandLord
        public ActionResult AddEdit()
        {
            return View();
        }
    }
}