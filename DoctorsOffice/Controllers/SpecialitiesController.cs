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


      [HttpPost("/specialities")]
    public ActionResult Create(string specialityName)
      {
        Speciality newSpeciality = new Speciality(specialityName);
        newSpeciality.Save();
        List<Speciality> allSpecialities = Speciality.GetAll();
        return View("Index", allSpecialities);
      }


      [HttpGet("/specialities/{id}")]
    public ActionResult Show(int id)
      {
          Dictionary<string, object> model = new Dictionary<string, object>();
          Speciality selectedSpeciality = Speciality.Find(id);
          List<Doctor> specialityDoctors = selectedSpeciality.GetDoctors();
          List<Doctor> allDoctors = Doctor.GetAll();
          model.Add("speciality", selectedSpeciality);
          model.Add("specialityDoctors", specialityDoctors);  
          model.Add("allDoctors", allDoctors);
          return View(model);
      }


      [HttpPost("/specialities/{specialityId}/doctors/new")]
    public ActionResult AddDoctor(int specialityId, int doctorId)
      {
        Speciality speciality = Speciality.Find(specialityId);
        // Console.WriteLine(specialityId);
        Doctor doctor = Doctor.Find(doctorId);
        // Console.WriteLine(doctorId);
        speciality.AddDoctor(doctor);
        // Console.WriteLine("added doc");
        return RedirectToAction("Show",  new { id = specialityId });
      }


      [ActionName("Destroy"), HttpPost("/specialities/{id}/delete")]
        public ActionResult Destroy(int id)
        {
        Speciality deleteSpeciality = Speciality.Find(id);
        List<Doctor> deleteDoctors = deleteSpeciality.GetDoctors();
        foreach(Doctor doctor in deleteDoctors)
        {
            doctor.Delete();
        }
        deleteSpeciality.Delete();
        return RedirectToAction("Index");
        }

  }
}