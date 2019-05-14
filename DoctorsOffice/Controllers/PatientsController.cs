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
      public ActionResult New(int id)
      {
          return View(id);
      }


      [HttpPost("/patients")]
      public ActionResult Create(string name, DateTime birthdate)
      {
        Patient newPatient = new Patient(name, birthdate);
        newPatient.Save();
        List<Patient> allPatients = Patient.GetAll();
        return View("Index", allPatients);
      }


      [HttpGet("/patients/{id}")]
      public ActionResult Show(int id)
      {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Patient selectedPatient = Patient.Find(id);
        List<Doctor> patientDoctors = selectedPatient.GetDoctors();
        List<Doctor> allDoctors = Doctor.GetAll();
        model.Add("selectedPatient", selectedPatient);
        model.Add("patientDoctors", patientDoctors);
        model.Add("allDoctors", allDoctors);
        return View(model);
      }


      [HttpPost("/patients/{patientId}/doctors/new")]
      public ActionResult AddDoctor(int patientId, int doctorId)
      {
        Patient patient = Patient.Find(patientId);
        Doctor doctor = Doctor.Find(doctorId);
        patient.AddDoctor(doctor);
        return RedirectToAction("Show",  new { id = patientId });
      }


  }
}