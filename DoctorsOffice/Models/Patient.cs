using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace DoctorsOffice.Models
{
  public class Patient
  {
    private int _id;
    private string _name;
    private DateTime _birthdate;
    

    public Patient(string patientName, DateTime patientBirthdate, int id = 0)
    {
      _name = patientName;
      _birthdate = patientBirthdate;
      _id = id;
    }

    public string GetName()
    {
      return _name;
    }

    public DateTime GetBirthdate()
    {
      return _birthdate;
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
    

    public override bool Equals(System.Object otherPatient)
        {
        if (!(otherPatient is Patient))
            {
                return false;
            }
        else
            {
                Patient newPatient = (Patient) otherPatient;
                bool idEquality = this.GetId() == newPatient.GetId();
                bool nameEquality = this.GetName() == newPatient.GetName();
                bool birthdateEquality = this.GetBirthdate() == newPatient.GetBirthdate();
                return (idEquality && nameEquality && birthdateEquality);
            }
        }

    public static List<Patient> GetAll()
    {
      List<Patient> allPatients = new List<Patient> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM patients;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int patientId = rdr.GetInt32(0);
        string patientName = rdr.GetString(1);
        DateTime patientBirthdate = rdr.GetDateTime(2);
        Patient newPatient = new Patient(patientName, patientBirthdate, patientId);
        allPatients.Add(newPatient);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allPatients;
    }

    public static void ClearAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM patients;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
       conn.Dispose();
      }
    }

    public static Patient Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM patients WHERE id = (@searchId);";
      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int patientId = 0;
      string patientName = "";
      DateTime patientBirthdate = new DateTime();
      while(rdr.Read())
      {
        patientId = rdr.GetInt32(0);
        patientName = rdr.GetString(1);
        patientBirthdate = rdr.GetDateTime(2);
      }
      Patient newPatient = new Patient(patientName, patientBirthdate, patientId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newPatient;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO patients (name, birthdate) VALUES (@name, @birthdate);";
      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = this._name;
      cmd.Parameters.Add(name);

      MySqlParameter birthdate = new MySqlParameter();
      birthdate.ParameterName = "@birthdate";
      birthdate.Value = this._birthdate;
      cmd.Parameters.Add(birthdate);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void Edit(string newName, DateTime newBirthdate)
        {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"UPDATE patients SET name = @newName, birthdate = @newBirthdate WHERE id = (@searchId);";
        MySqlParameter searchId = new MySqlParameter();
        searchId.ParameterName = "@searchId";
        searchId.Value = _id;
        cmd.Parameters.Add(searchId);

        MySqlParameter name = new MySqlParameter();
        name.ParameterName = "@newName";
        name.Value = newName;
        cmd.Parameters.Add(name);

        MySqlParameter birthdate = new MySqlParameter();
        birthdate.ParameterName = "@newBirthdate";
        birthdate.Value = newBirthdate;
        cmd.Parameters.Add(birthdate);
        cmd.ExecuteNonQuery();
        _name = newName;
        _birthdate = newBirthdate;
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
        MySqlCommand cmd = new MySqlCommand("DELETE FROM patients WHERE id = @PatientId;", conn);
        MySqlParameter patientParameter = new MySqlParameter();
        patientParameter.ParameterName = "@PatientId";
        patientParameter.Value = _id;
        cmd.Parameters.Add(patientParameter);
        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn != null)
            {
            conn.Dispose();
            }
      }


      public List<Doctor> GetDoctors()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT doctor_id FROM doctors_patients WHERE patient_id = @patientId;";
        MySqlParameter patientIdParameter = new MySqlParameter();
        patientIdParameter.ParameterName = "@patientId";
        patientIdParameter.Value = _id;
        cmd.Parameters.Add(patientIdParameter);
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        List<int> doctorIds = new List <int> {
        };
        while (rdr.Read())
        {
            int doctorId = rdr.GetInt32(0);
            doctorIds.Add(doctorId);
        }
        rdr.Dispose();
        List<Doctor> doctors = new List<Doctor> {
        };
        foreach (int doctorId in doctorIds)
        {
            var doctorQuery = conn.CreateCommand() as MySqlCommand;
            doctorQuery.CommandText = @"SELECT * FROM doctors WHERE id = @DoctorId;";
            MySqlParameter doctorIdParameter = new MySqlParameter();
            doctorIdParameter.ParameterName = "@DoctorId";
            doctorIdParameter.Value = doctorId;
            doctorQuery.Parameters.Add(doctorIdParameter);
            var doctorQueryRdr = doctorQuery.ExecuteReader() as MySqlDataReader;
            while(doctorQueryRdr.Read())
            {
                int thisDoctorId = doctorQueryRdr.GetInt32(0);
                string doctorName = doctorQueryRdr.GetString(1);
                string doctorSpeciality = doctorQueryRdr.GetString(2);
                Doctor foundDoctor = new Doctor(doctorName, doctorSpeciality, thisDoctorId);
                doctors.Add(foundDoctor);
            }
            doctorQueryRdr.Dispose();
        }
        conn.Close();
        if(conn != null)
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
        cmd.CommandText = @"INSERT INTO doctors_patients (doctor_id, patient_id) VALUES (@DoctorId, @PatientId);";
        MySqlParameter doctor_id = new MySqlParameter();
        doctor_id.ParameterName = "@DoctorId";
        doctor_id.Value = newDoctor.GetId();
        cmd.Parameters.Add(doctor_id);
        MySqlParameter patient_id = new MySqlParameter();
        patient_id.ParameterName = "@PatientId";
        patient_id.Value = _id;
        cmd.Parameters.Add(patient_id);
        cmd.ExecuteNonQuery();


        conn.Close();
        if(conn != null)
        {
        conn.Dispose();
        }
    }


  }
}