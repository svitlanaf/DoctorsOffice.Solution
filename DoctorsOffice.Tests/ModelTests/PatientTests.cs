using Microsoft.VisualStudio.TestTools.UnitTesting;
using DoctorsOffice.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace DoctorsOffice.Tests
{
  [TestClass]
  public class PtientTest : IDisposable
  {

    public PtientTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=doctors_office_test;";
    }

    public void Dispose()
    {
      Doctor.ClearAll();
      Patient.ClearAll();
      Speciality.ClearAll();
    }


  [TestMethod]
    public void GetDoctors_ReturnsAllPatientDOctors_DoctorList()
    {
      Patient testPatient = new Patient("test", new DateTime(1/2/2019));
      testPatient.Save();
      Doctor testDoctor1 = new Doctor("test");
      testDoctor1.Save();
      Doctor testDoctor2 = new Doctor("test1");
      testDoctor2.Save();
      testPatient.AddDoctor(testDoctor1);
      List<Doctor> result = testPatient.GetDoctors();
      List<Doctor> testList = new List<Doctor> {testDoctor1};
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void AddDoctor_AddsDoctorToPatient_DoctorList()
    {
      Patient testPatient = new Patient("test", new DateTime(1/2/2019));
      testPatient.Save();
      Doctor testDoctor = new Doctor("test1");
      testDoctor.Save();
      testPatient.AddDoctor(testDoctor);
      List<Doctor> result = testPatient.GetDoctors();
      List<Doctor> testList = new List<Doctor>{testDoctor};
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void GetAll_ReturnsPatients_PatientList()
    {
      string name1 = "test";
      DateTime birthdate1 = DateTime.Parse("12/03/2019");
      string name2 = "test1";
      DateTime birthdate2 = DateTime.Parse("12/04/2019");
      Patient newPatient1 = new Patient(name1, birthdate1);
      newPatient1.Save();
      Patient newPatient2 = new Patient(name2, birthdate2);
      newPatient2.Save();
      List<Patient> newList = new List<Patient> { newPatient1, newPatient2 };
      List<Patient> result = Patient.GetAll();
      CollectionAssert.AreEqual(newList, result);
    }

  }
}