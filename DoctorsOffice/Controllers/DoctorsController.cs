using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using DoctorsOffice.Models;
using System.Linq;
using MySql.Data.MySqlClient;

namespace DoctorsOffice.Controllers
{
  public class DoctorsController : Controller
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


      [HttpPost("/doctors")]
    public ActionResult Create(string doctorName)
      {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Doctor newDoctor = new Doctor(doctorName);
        newDoctor.Save();
        List<Doctor> allDoctors = Doctor.GetAll();
        List<Speciality> allSpecialities = Speciality.GetAll();

        model.Add("allDoctors", allDoctors);
        model.Add("allSpecialities", allSpecialities);
        return View("Index", model);
      }


    [HttpGet("/doctors/{id}")]
    public ActionResult Show(int id)
      {
          Dictionary<string, object> model = new Dictionary<string, object>();
          Doctor selectedDoctor = Doctor.Find(id);
          List<Patient> doctorPatients = selectedDoctor.GetPatients();
          List<Patient> allPatients = Patient.GetAll();
          List<Speciality> allDoctorSpecialty = selectedDoctor.GetSpecialities();

          model.Add("allDoctorSpecialty", allDoctorSpecialty);
          model.Add("doctor", selectedDoctor);
          model.Add("doctorPatients", doctorPatients);  
          model.Add("allPatients", allPatients);
          return View(model);
      }


    [HttpPost("/doctors/{doctorId}/patients/new")]
    public ActionResult AddPatient(int doctorId, int patientId)
      {
        Doctor doctor = Doctor.Find(doctorId);
        Patient patient = Patient.Find(patientId);
        doctor.AddPatient(patient);
        return RedirectToAction("Show",  new { id = doctorId });
      }


      [ActionName("Destroy"), HttpPost("/doctors/{id}/delete")]
        public ActionResult Destroy(int id)
        {
        Doctor deleteDoctor = Doctor.Find(id);

        deleteDoctor.Delete();
        return RedirectToAction("Index");
        }


  }
}