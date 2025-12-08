using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;


namespace DataAccessLayer
{
    public interface IStudentRepository
    {
        List<StudentDetail> Getstudent();
        StudentDetail Getstudentbyid(int id);
        void Addstudent(StudentDetail s);
        void Updatestudent(StudentDetail s);
        void DeleteStudents(int id);
        List<State> GetState();
        bool isduplicateEmailorMobile(string Email, string Mobilenumber);
        bool isduplicateEmailorMobileforedit(int id, string Email, string Mobilenumber);


    }
    public class StudentRepository : IStudentRepository
    {
        public IConfiguration _Configuration;
        string connectionstring = string.Empty;
        public StudentRepository(IConfiguration appsettings)
        {
            _Configuration = appsettings;
            connectionstring = appsettings.GetSection("DBConnection").Value;
            var connect = appsettings.GetConnectionString("SqlServerConnection");

        }


        public void Addstudent(StudentDetail obj)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionstring))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Addstudent", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Name", obj.Name);
                        cmd.Parameters.AddWithValue("@Email", obj.Email);
                        cmd.Parameters.AddWithValue("@Mobilenumber", obj.Mobilenumber);
                        cmd.Parameters.AddWithValue("@stateId", obj.StateId);
                        cmd.Parameters.AddWithValue("@Gender", obj.Gender);
                        cmd.Parameters.AddWithValue("@DOB", obj.DOB);
                        con.Open();
                        cmd.ExecuteNonQuery();


                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }


        }

        public void Updatestudent(StudentDetail s)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionstring))
                {
                    SqlCommand cmd = new SqlCommand("sp_Updatestudent", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", s.Id);
                    cmd.Parameters.AddWithValue("@Name", s.Name);
                    cmd.Parameters.AddWithValue("@Email", s.Email);
                    cmd.Parameters.AddWithValue("@Mobilenumber", s.Mobilenumber);
                    cmd.Parameters.AddWithValue("@stateId", s.StateId);
                    cmd.Parameters.AddWithValue("@Gender", s.Gender);
                    cmd.Parameters.AddWithValue("@DOB", s.DOB);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public StudentDetail Getstudentbyid(int id)
        {

            StudentDetail student = null;
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                SqlCommand cmd = new SqlCommand("sp_Getstudentbyid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Id", id);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {

                    student = new StudentDetail
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        Email = reader["Email"].ToString(),
                        Mobilenumber = reader["Mobilenumber"].ToString(),
                        StateId = Convert.ToInt32(reader["StateId"]),
                        Gender = reader["Gender"].ToString(),
                        DOB = Convert.ToDateTime(reader["DOB"]).Date,
                        StateName = reader["StateName"] != DBNull.Value
                                ? reader["StateName"].ToString()
                                : "",
                    };
                }
                con.Close();
            }
            return student;
        }
        public void DeleteStudents(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionstring))
                {
                    SqlCommand cmd = new SqlCommand("sp_Deletestudent", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<StudentDetail> Getstudent()
        {
            try
            {
                List<StudentDetail> students = new List<StudentDetail>();
                using (SqlConnection con = new SqlConnection(connectionstring))
                {
                    SqlCommand cmd = new SqlCommand("sp_Getstudent", con);
                    cmd.CommandType = CommandType.StoredProcedure;


                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        students.Add(new StudentDetail
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            Email = reader["Email"].ToString(),
                            Mobilenumber = reader["Mobilenumber"].ToString(),
                            StateId = Convert.ToInt32(reader["StateId"]),
                            StateName = reader["StateName"] != DBNull.Value
                                ? reader["StateName"].ToString()
                                : "",
                            Gender = reader["Gender"].ToString(),
                            DOB = Convert.ToDateTime(reader["DOB"]).Date
                        });

                    }


                    return students;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        public List<State> GetState()
        {
            List<State> states = new List<State>();
            using (SqlConnection con = new SqlConnection(connectionstring))
            {

                SqlCommand cmd = new SqlCommand("sp_Getstate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        states.Add(new State
                        {

                            StateId = Convert.ToInt32(dr["StateId"]),
                            StateName = dr["StateName"].ToString()
                        });

                    }
                }

            }


            return states;
        }

        public bool isduplicateEmailorMobile(string Email,string Mobilenumber)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {

                SqlCommand cmd = new SqlCommand("sp_checkduplicateEmailorMobile", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@Mobilenumber",Mobilenumber);
                con.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }


        }

        public bool isduplicateEmailorMobileforedit(int id,string Email, string Mobilenumber)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {

                SqlCommand cmd = new SqlCommand("sp_checkduplicateEmailorMobileforedit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@Mobilenumber", Mobilenumber);
                
                con.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }


        }




    }
}

        



                        
                            
                        
                    

                
            



