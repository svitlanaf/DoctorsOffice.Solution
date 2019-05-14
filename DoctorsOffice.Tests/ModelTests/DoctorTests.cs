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
    }


  [TestMethod]
    public void Test_AddPatient_AddsPatientToDoctor()
    {
      Doctor testDoctor = new Doctor("test", "test");
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
      Doctor testDoctor = new Doctor("test", "test");
      testDoctor.Save();
      Patient testPatient = new Patient("test", new DateTime(1/2/2019));
      testPatient.Save();
      Patient testPatient2 = new Patient("test1", new DateTime(1/2/2019));
      testPatient2.Save();
      testDoctor.AddPatient(testPatient);
      List<Patient> savedItems = testDoctor.GetPatients();
      List<Patient> testList = new List<Patient> {testPatient};
      CollectionAssert.AreEqual(testList, savedItems);
    }

  }
}