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
        List<Speciality> allSpecialities = Speciality.GetAll();
        return View(allSpecialities);
      }


      [HttpPost("/doctors/create")]
    public ActionResult Create(string doctorName)
      {   
        Doctor newDoctor = new Doctor(doctorName);    
        List<Doctor> allDoctors = Doctor.GetAll();
        newDoctor.Save();
        return View("Index", allDoctors);
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


      [HttpPost("/doctors/{id}/delete")]
        public ActionResult Destroy(int id)
        {
        Doctor deleteDoctor = Doctor.Find(id);

        deleteDoctor.Delete();
        return RedirectToAction("Index");
        }


  }
}