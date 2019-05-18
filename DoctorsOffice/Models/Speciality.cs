using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace DoctorsOffice.Models
{
  public class Speciality
  {
    private int _id;
    private string _name;


    public Speciality(string specialityName, int id = 0)
    {
      _name = specialityName;
      _id = id;
    }

    public string GetName()
    {
      return _name;
    }


    public int GetId()
    {
      return _id;
    }

    public override int GetHashCode()
    {
    return this.GetId().GetHashCode();
    }

    public override bool Equals(System.Object otherSpeciality)
        {
        if (!(otherSpeciality is Speciality))
            {
                return false;
            }
        else
            {
                Speciality newSpeciality = (Speciality) otherSpeciality;
                bool idEquality = this.GetId() == newSpeciality.GetId();
                bool nameEquality = this.GetName() == newSpeciality.GetName();
                return (idEquality && nameEquality);
            }
        }

    public void Dispose()
    {
      Speciality.ClearAll();
      Doctor.ClearAll();
      Patient.ClearAll();
    }

     public void Save()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();

        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO specialities (name) VALUES (@name);";

        MySqlParameter name = new MySqlParameter();
        name.ParameterName = "@name";
        name.Value = this._name;
        cmd.Parameters.Add(name);

        cmd.ExecuteNonQuery();
        _id = (int) cmd.LastInsertedId;
        conn.Close();
        if (conn != null)
            {
                conn.Dispose();
            }
    }


    public static List<Speciality> GetAll()
    {
      List<Speciality> allSpecialities = new List<Speciality> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM specialities;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int SpecialityId = rdr.GetInt32(0);
        string SpecialityName = rdr.GetString(1);
        Speciality newSpeciality = new Speciality(SpecialityName, SpecialityId);
        allSpecialities.Add(newSpeciality);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allSpecialities;
    }

    public static Speciality Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM specialities WHERE id = (@searchId);";
      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int SpecialityId = 0;
      string SpecialityName = "";
      while(rdr.Read())
      {
        SpecialityId = rdr.GetInt32(0);

        Console.WriteLine(SpecialityId);

        SpecialityName = rdr.GetString(1);

        Console.WriteLine(SpecialityName);
      }
      Speciality newSpeciality = new Speciality(SpecialityName, SpecialityId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newSpeciality;
    }

    public static void ClearAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM specialities;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void Delete()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        MySqlCommand cmd = new MySqlCommand( "DELETE FROM specialities WHERE id = @SpecialityId;", conn);
        MySqlParameter specialityIdParameter = new MySqlParameter();
        specialityIdParameter.ParameterName = "@SpecialityId";
        specialityIdParameter.Value = this.GetId();
        cmd.Parameters.Add(specialityIdParameter);
        cmd.ExecuteNonQuery();

        if (conn != null)
        {
            conn.Close();
        }
    }


    public List<Doctor> GetDoctors()
    {

        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT doctors.* FROM
            specialities JOIN specialities_doctors ON (specialities.id = specialities_doctors.speciality_id)
                    JOIN doctors ON (specialities_doctors.doctor_id = doctors.id)
                    WHERE specialities.id = @SpecialityId;";
        MySqlParameter specialityIdParameter = new MySqlParameter();
        specialityIdParameter.ParameterName = "@SpecialityId";
        specialityIdParameter.Value = _id;
        cmd.Parameters.Add(specialityIdParameter);
        MySqlDataReader doctorQueryRdr = cmd.ExecuteReader() as MySqlDataReader;
        List<Doctor> doctors = new List<Doctor> {
        };

        while(doctorQueryRdr.Read())
        {
            int thisDoctorId = doctorQueryRdr.GetInt32(0);
            string doctorName = doctorQueryRdr.GetString(1);

            Doctor newDoctor = new Doctor (doctorName, thisDoctorId);
            doctors.Add (newDoctor);
        }

        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        return doctors;
    }


    public void AddDoctor (Doctor newDoctor)
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO specialities_doctors (speciality_id, doctor_id) VALUES (@SpecialityId, @DoctorId);";
        MySqlParameter speciality_id = new MySqlParameter();
        speciality_id.ParameterName = "@SpecialityId";
        speciality_id.Value = _id;
        cmd.Parameters.Add(speciality_id);
        MySqlParameter doctor_id = new MySqlParameter();
        doctor_id.ParameterName = "@DoctorId";
        doctor_id.Value = newDoctor.GetId();
        cmd.Parameters.Add(doctor_id);
        cmd.ExecuteNonQuery();
        conn.Close();
        if(conn != null)
        {
            conn.Dispose();
        }
    }


  }
}
