using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using DoctorsOffice.Models;
using System.Linq;
using MySql.Data.MySqlClient;

namespace DoctorsOffice.Controllers
{
  public class SpecialitiesController : Controller
  {

      [HttpGet("/specialities")]
      public ActionResult Index()
      {
          List<Speciality> allSpecialities = Speciality.GetAll();
          return View(allSpecialities);
      }


      [HttpGet("/specialities/new")]
      public ActionResult New()
      {
        return View();
      }

  }
}