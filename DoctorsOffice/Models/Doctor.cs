using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace DoctorsOffice.Models
{
  public class Doctor
  {
    private int _id;
    private string _name;
    private string _speciality;
    

    public Doctor(string doctorName, string doctorSpeciality, int id = 0)
    {
      _name = doctorName;
      _speciality = doctorSpeciality;
      _id = id;
    }

    public string GetName()
    {
      return _name;
    }

    public string GetSpeciality()
    {
      return _speciality;
    }

    public int GetId()
    {
      return _id;
    }

    public override int GetHashCode()
    {
    return this.GetId().GetHashCode();
    }

    public static List<Doctor> GetAll()
    {
      List<Doctor> allDoctors = new List<Doctor> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM doctors;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int DoctorId = rdr.GetInt32(0);
        string DoctorName = rdr.GetString(1);
        string DoctorSpeciality = rdr.GetString(2);
        Doctor newDoctor = new Doctor(DoctorName, DoctorSpeciality, DoctorId);
        allDoctors.Add(newDoctor);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allDoctors;
    }

    public static Doctor Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM doctors WHERE id = (@searchId);";
      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int DoctorId = 0;
      string DoctorName = "";
      string DoctorSpeciality = "";
      while(rdr.Read())
      {
        DoctorId = rdr.GetInt32(0);
        DoctorName = rdr.GetString(1);
        DoctorSpeciality = rdr.GetString(2);

      }
      Doctor newDoctor = new Doctor(DoctorName, DoctorSpeciality, DoctorId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newDoctor;
    }

    // public List<Patient> GetPatients()
    // {
    //   MySqlConnection conn = DB.Connection();
    //   conn.Open();
    //   var cmd = conn.CreateCommand() as MySqlCommand;
    //   cmd.CommandText = @"SELECT item_id FROM categories_items WHERE category_id = @CategoryId;";
    //   MySqlParameter categoryIdParameter = new MySqlParameter();
    //   categoryIdParameter.ParameterName = "@CategoryId";
    //   categoryIdParameter.Value = _id;
    //   cmd.Parameters.Add(categoryIdParameter);
    //   var rdr = cmd.ExecuteReader() as MySqlDataReader;
    //   List<int> itemIds = new List<int> {};
    //   while(rdr.Read())
    //   {
    //     int itemId = rdr.GetInt32(0);
    //     itemIds.Add(itemId);
    //   }
    //   rdr.Dispose();
    //   List<Item> items = new List<Item> {};
    //   foreach (int itemId in itemIds)
    //   {
    //     var itemQuery = conn.CreateCommand() as MySqlCommand;
    //     itemQuery.CommandText = @"SELECT * FROM items WHERE id = @ItemId;";
    //     MySqlParameter itemIdParameter = new MySqlParameter();
    //     itemIdParameter.ParameterName = "@ItemId";
    //     itemIdParameter.Value = itemId;
    //     itemQuery.Parameters.Add(itemIdParameter);
    //     var itemQueryRdr = itemQuery.ExecuteReader() as MySqlDataReader;
    //     while(itemQueryRdr.Read())
    //     {
    //       int thisItemId = itemQueryRdr.GetInt32(0);
    //       string itemDescription = itemQueryRdr.GetString(1);
    //       Item foundItem = new Item(itemDescription, thisItemId);
    //       items.Add(foundItem);
    //     }
    //     itemQueryRdr.Dispose();
    //   }
    //   conn.Close();
    //   if (conn != null)
    //   {
    //     conn.Dispose();
    //   }
    //   return items;
    // }


  }
}