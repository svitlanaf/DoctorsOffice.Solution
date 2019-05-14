using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using DoctorsOffice.Models;
using System.Linq;
using MySql.Data.MySqlClient;

namespace DoctorsOffice.Controllers
{
  public class DOctorsController : Controller
  {

      [HttpGet("/doctors")]
      public ActionResult Index()
      {
          List<Doctor> allDoctors = Doctor.GetAll();
          return View(allDoctors);
      }

      [HttpGet("/doctors/new")]
      public ActionResult New()
      {
        return View();
      }

  }
}