using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using DoctorsOffice.Models;
using System.Linq;
using MySql.Data.MySqlClient;

namespace DoctorsOffice.Controllers
{
  public class PatientsController : Controller
  {

      [HttpGet("/patients")]
      public ActionResult Index()
      {
          List<Patient> allPatients = Patient.GetAll();
          return View(allPatients);
      }

      [HttpGet("/patients/new")]
      public ActionResult New()
      {
        return View();
      }

  }
}