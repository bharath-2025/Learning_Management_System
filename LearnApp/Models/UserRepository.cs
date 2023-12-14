
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
#nullable disable


namespace LearnApp.Models{
    public class UserRepository:IUserRepository{
        private string connectionString;
        private SqlConnection sqlConnection;
        private readonly IConfiguration _configuration;

        public UserRepository(){
            
        }
        public UserRepository(IConfiguration configuration){
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine("This is from User Controller --------------\n"+connectionString);
        }


        public bool ValidUser(User user){
            string userid = user.UserId;
            string password = user.Password;
            string pass="";
            //connectionString = "Data Source = Aspire1550\\SQLEXPRESS;Initial Catalog = MaxLearnDB;Integrated Security = SSPI";
            using(sqlConnection = new SqlConnection(connectionString)){
                sqlConnection.Open();
                SqlCommand search = new SqlCommand("select Password from [Users] where UserId = @id",sqlConnection);
                search.Parameters.Add("@id",SqlDbType.VarChar,50,"UserId").Value = userid;
                pass = (string)search.ExecuteScalar();
            }
            
            if(pass == password)
                return true;
            return false;
        }

        //To Fetch Role of a User
        public string FetchRole(User user){
            string userid = user.UserId;
            string role="";
            //connectionString = "Data Source = Aspire1550\\SQLEXPRESS;Initial Catalog = MaxLearnDB;Integrated Security = SSPI";
            using(sqlConnection = new SqlConnection(connectionString)){
                sqlConnection.Open();
                SqlCommand search = new SqlCommand("select role from [Users] where UserId = @id",sqlConnection);
                search.Parameters.Add("@id",SqlDbType.VarChar,50,"UserId").Value = userid;
                role = (string) search.ExecuteScalar();
            }
            
            return role;
        }

        //To Fetch UserName of a User
        public string FetchName(User user){
            string userid = user.UserId;
            string name="";
            //connectionString = "Data Source = Aspire1550\\SQLEXPRESS;Initial Catalog = MaxLearnDB;Integrated Security = SSPI";
            using(sqlConnection = new SqlConnection(connectionString)){
                sqlConnection.Open();
                SqlCommand search = new SqlCommand("select UserName from [Users] where UserId = @id",sqlConnection);
                search.Parameters.Add("@id",SqlDbType.VarChar,50,"UserId").Value = userid;
                name = (string) search.ExecuteScalar();
            }
            
            return name;
        }

        //To Fetch BatchId of a User
        public string FetchBatch(User user){
            string userid = user.UserId;
            string name="";
            //connectionString = "Data Source = Aspire1550\\SQLEXPRESS;Initial Catalog = MaxLearnDB;Integrated Security = SSPI";
            using(sqlConnection = new SqlConnection(connectionString)){
                sqlConnection.Open();
                SqlCommand search = new SqlCommand("select BatchId from [Users] where UserId = @id",sqlConnection);
                search.Parameters.Add("@id",SqlDbType.VarChar,50,"UserId").Value = userid;
                name = (string) search.ExecuteScalar();
            }
            
            return name;
        }

        //To Get All Users
        public IEnumerable GetUsers(){
            List<User> users = new List<User>();
            //connectionString = "Data Source = Aspire1550\\SQLEXPRESS;Initial Catalog = MaxLearnDB;Integrated Security = SSPI";
             
            using(sqlConnection = new SqlConnection(connectionString)){
                sqlConnection.Open();
                SqlCommand display = new SqlCommand("Select * from [Users]",sqlConnection);
                SqlDataReader reader = display.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        User user = new User();
                        user.UserId = reader.GetString(0);
                        user.UserName = reader.GetString(1);
                        user.Email = reader.GetString(2);
                        user.Password = reader.GetString(3);
                        user.Role = reader.GetString(4);
                        user.BatchId = reader.GetString(5);
                        users.Add(user);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                reader.Close();
                

                return users;
            }
            
        }


        //Adding User to database
        public void AddUser(User user){
            //connectionString = "Data Source = Aspire1550\\SQLEXPRESS;Initial Catalog = MaxLearnDB;Integrated Security = SSPI";
            using(sqlConnection = new SqlConnection(connectionString)){
                sqlConnection.Open();
                SqlCommand insert = new SqlCommand("insert into [Users] values(@id,@name,@email,@pass,@role,@batchid)",sqlConnection);
                insert.Parameters.AddWithValue("@id",user.UserId);
                insert.Parameters.AddWithValue("@name",user.UserName);
                insert.Parameters.AddWithValue("@email",user.Email);
                insert.Parameters.AddWithValue("@pass",user.Password);
                insert.Parameters.AddWithValue("@role",user.Role);
                insert.Parameters.AddWithValue("@batchid",user.BatchId);

                insert.ExecuteNonQuery();
            }
        }


        public void DeleteUser(string id){
            //connectionString = "Data Source = Aspire1550\\SQLEXPRESS;Initial Catalog = MaxLearnDB;Integrated Security = SSPI";
            using(sqlConnection = new SqlConnection(connectionString)){
                sqlConnection.Open();
                SqlCommand delete = new SqlCommand("delete from [Users] where UserId = @id",sqlConnection);
                delete.Parameters.Add("@id",SqlDbType.VarChar,50,"UserId").Value = id;
                delete.ExecuteNonQuery();
            }
        }

      
    }
}

