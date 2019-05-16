using Microsoft.VisualStudio.TestTools.UnitTesting;
using DoctorsOffice.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace DoctorsOffice.Tests
{
  [TestClass]
  public class DoctorTest : IDisposable
  {

    public DoctorTest()
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
    public void Test_AddPatient_AddsPatientToDoctor()
    {
      Doctor testDoctor = new Doctor("test");
      testDoctor.Save();
      Patient testPatient = new Patient("test", new DateTime(1/2/2019));
      testPatient.Save();
      Patient testPatient2 = new Patient("test1", new DateTime(1/2/2019));
      testPatient2.Save();
      testDoctor.AddPatient(testPatient);
      testDoctor.AddPatient(testPatient2);
      List<Patient> result = testDoctor.GetPatients();
      List<Patient> testList = new List<Patient>{testPatient, testPatient2};
      CollectionAssert.AreEqual(testList, result);
    }


    [TestMethod]
    public void GetPatients_ReturnsAllDoctorPatients_PatientList()
    {
      Doctor testDoctor = new Doctor("test");
      testDoctor.Save();
      Patient testPatient = new Patient("test", new DateTime(1/2/2019));
      testPatient.Save();
      Patient testPatient2 = new Patient("test1", new DateTime(1/2/2019));
      testPatient2.Save();
      testDoctor.AddPatient(testPatient);
      List<Patient> savedPatients = testDoctor.GetPatients();
      List<Patient> testList = new List<Patient> {testPatient};
      CollectionAssert.AreEqual(testList, savedPatients);
    }


    [TestMethod]
    public void Test_AddSpeciality_AddsSpecialityToDoctor()
    {
      Doctor testDoctor = new Doctor("test");
      testDoctor.Save();
      Speciality testSpeciality = new Speciality("Dermatologist");
      testSpeciality.Save();
      Speciality testSpeciality2 = new Speciality("Audiologist");
      testSpeciality2.Save();
      testDoctor.AddSpeciality(testSpeciality);
      testDoctor.AddSpeciality(testSpeciality2);
      List<Speciality> result = testDoctor.GetSpecialities();
      List<Speciality> testList = new List<Speciality>{testSpeciality, testSpeciality2};
      CollectionAssert.AreEqual(testList, result);
    }


    [TestMethod]
    public void GetSpecialities_ReturnsAllDoctorSpecialities_SpecialityList()
    {
      Doctor testDoctor = new Doctor("test");
      testDoctor.Save();
      Speciality testSpeciality1 = new Speciality("Dermatologist");
      testSpeciality1.Save();
      Speciality testSpeciality2 = new Speciality("Audiologist");
      testSpeciality2.Save();
      testDoctor.AddSpeciality(testSpeciality1);
      List<Speciality> savedSpecialities = testDoctor.GetSpecialities();
      List<Speciality> testList = new List<Speciality> {testSpeciality1};
      CollectionAssert.AreEqual(testList, savedSpecialities);
    }


  }
}