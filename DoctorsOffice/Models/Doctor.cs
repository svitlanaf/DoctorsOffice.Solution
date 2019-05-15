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

    public void Dispose()
    {
      Doctor.ClearAll();
      Patient.ClearAll();
    }


    public override bool Equals(System.Object otherDoctor)
    {
        if (!(otherDoctor is Doctor))
        {
            return false;
        }
        else
        {
            Doctor newDoctor = (Doctor) otherDoctor;
            bool idEquality = this.GetId().Equals(newDoctor.GetId());
            bool nameEquality = this.GetName().Equals(newDoctor.GetName());
            bool specialityEquality = this.GetSpeciality().Equals(newDoctor.GetSpeciality());
            return (idEquality && nameEquality);
        }
    }


    public static void ClearAll()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"DELETE FROM doctors;";
        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
    }

    public void Save()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();

        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO doctors (name, speciality) VALUES (@name, @speciality);";

        MySqlParameter name = new MySqlParameter();
        name.ParameterName = "@name";
        name.Value = this._name;
        cmd.Parameters.Add(name);

        MySqlParameter speciality = new MySqlParameter();
        speciality.ParameterName = "@speciality";
        speciality.Value = this._speciality;
        cmd.Parameters.Add(speciality);

        cmd.ExecuteNonQuery();
        _id = (int) cmd.LastInsertedId;
        conn.Close();
        if (conn != null)
            {
                conn.Dispose();
            }

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
      Console.WriteLine("test");
      string DoctorName = "";
      string DoctorSpeciality = "";
      while(rdr.Read())
      {
        DoctorId = rdr.GetInt32(0);
        Console.WriteLine(DoctorId);
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

    public void Delete()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        MySqlCommand cmd = new MySqlCommand( "DELETE FROM doctors WHERE id = @DoctorId;", conn);
        MySqlParameter doctorIdParameter = new MySqlParameter();
        doctorIdParameter.ParameterName = "@DoctorId";
        doctorIdParameter.Value = this.GetId();
        cmd.Parameters.Add(doctorIdParameter);
        cmd.ExecuteNonQuery();

        if (conn != null)
        {
            conn.Close();
        }
    }

    
    public List<Patient> GetPatients()
    {

        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT patients.* FROM 
            doctors JOIN doctors_patients ON (doctors.id = doctors_patients.doctor_id)
                    JOIN patients ON (doctors_patients.patient_id = patients.id)
                    WHERE doctors.id = @DoctorId;";
        MySqlParameter doctorIdParameter = new MySqlParameter();
        doctorIdParameter.ParameterName = "@DoctorId";
        doctorIdParameter.Value = _id;
        cmd.Parameters.Add(doctorIdParameter);
        MySqlDataReader patientQueryRdr = cmd.ExecuteReader() as MySqlDataReader;
        List<Patient> patients = new List<Patient> {
        };

        while(patientQueryRdr.Read())
        {
            int thisPatientId = patientQueryRdr.GetInt32(0);
            string patientName = patientQueryRdr.GetString(1);
            DateTime patientBirthdate = patientQueryRdr.GetDateTime(2);

            Patient newPatient = new Patient (patientName, patientBirthdate, thisPatientId);
            patients.Add (newPatient);
        }

        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        return patients;
    }


    public void AddPatient (Patient newPatient)
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO doctors_patients (doctor_id, patient_id) VALUES (@DoctorId, @PatientId);";
        MySqlParameter doctor_id = new MySqlParameter();
        doctor_id.ParameterName = "@DoctorId";
        doctor_id.Value = _id;
        cmd.Parameters.Add(doctor_id);
        MySqlParameter patien_id = new MySqlParameter();
        patien_id.ParameterName = "@PatientId";
        patien_id.Value = newPatient.GetId();
        cmd.Parameters.Add(patien_id);
        cmd.ExecuteNonQuery();
        conn.Close();
        if(conn != null)
        {
            conn.Dispose();
        }
    }

    public void Edit(string newName, string newSpeciality)
        {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"UPDATE doctors SET name = @newName, speciality = @newSpeciality WHERE id = (@searchId);";
        MySqlParameter searchId = new MySqlParameter();
        searchId.ParameterName = "@searchId";
        searchId.Value = _id;
        cmd.Parameters.Add(searchId);

        MySqlParameter name = new MySqlParameter();
        name.ParameterName = "@newName";
        name.Value = newName;
        cmd.Parameters.Add(name);

        MySqlParameter speciality = new MySqlParameter();
        speciality.ParameterName = "@newBSpeciality";
        speciality.Value = newSpeciality;
        cmd.Parameters.Add(speciality);
        cmd.ExecuteNonQuery();
        _name = newName;
        _speciality = newSpeciality;
        conn.Close();
        if (conn != null)
            {
                conn.Dispose();
            }
        }

  }
}