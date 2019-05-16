using Microsoft.VisualStudio.TestTools.UnitTesting;
using DoctorsOffice.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace DoctorsOffice.Tests
{
  [TestClass]
  public class SpecialityTest : IDisposable
  {

    public SpecialityTest()
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
    public void GetAll_TakeAllSpecialties_List()
    {
        List<Speciality> newList = new List<Speciality> {};
        List<Speciality> result = Speciality.GetAll();
        CollectionAssert.AreEqual(newList, result);
    }

    [TestMethod]
    public void GetAll_ReturnsSpecialties_SpecialtiesList()
    {
        Speciality newSpecialty = new Speciality("Family");
        newSpecialty.Save();
        List<Speciality> newList = new List<Speciality> {newSpecialty};
        List<Speciality> result = Speciality.GetAll();
        CollectionAssert.AreEqual(newList, result);
    }
  }
}
