
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace LearnApp.Models{


    public class CourseRepository:ICourseRepository{

        private string? connectionString;
        private SqlConnection sqlConnection;
        private readonly IConfiguration _configuration;

        public CourseRepository(IConfiguration configuration){
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
            sqlConnection = new SqlConnection();
        }

        //To Open Connection
        public void openConnection(){
            sqlConnection.Open();
        }

        //To Close Connection
        public void closeConnection(){
            sqlConnection.Close();
        }


        //Adding Course to database
        public void AddCourse(Course course){
            // Console.WriteLine("***********************"+course.PhotoPath);
            // Console.WriteLine("------------------------");
        
            using(sqlConnection = new SqlConnection(connectionString)){
                sqlConnection.Open();
                SqlCommand insert = new SqlCommand("insert into [Courses] values(@id,@name,@date,@batch,@video,@photo)",sqlConnection);
                insert.Parameters.AddWithValue("@id",course.CourseId);
                insert.Parameters.AddWithValue("@name",course.CourseName);
                insert.Parameters.AddWithValue("@date",course.StartDate);
                insert.Parameters.AddWithValue("@batch",course.BatchId);
                insert.Parameters.AddWithValue("@video",course.VideoPath);
                insert.Parameters.AddWithValue("@photo",course.PhotoPath);

                insert.ExecuteNonQuery();
            }
        }

        //Deleting course from database
        public void DeleteCourse(string id){
            
            using(sqlConnection = new SqlConnection(connectionString)){
                sqlConnection.Open();
                SqlCommand delete = new SqlCommand("delete from [Courses] where CourseId = @id",sqlConnection);
                delete.Parameters.AddWithValue("@id",id);
                delete.ExecuteNonQuery();
            }
        }


        public List<Course> GetCourses(){
            List<Course> courses = new List<Course>();
             
            // using(sqlConnection = new SqlConnection(connectionString)){
            //     sqlConnection.Open();
            //     SqlCommand display = new SqlCommand("Select * from [Courses]",sqlConnection);
            //     using(SqlDataReader reader = display.ExecuteReader()){

            //         if (reader.HasRows)
            //         {
            //             while (reader.Read())
            //             {
            //                 Course course = new Course();
            //                 course.CourseId = reader.GetString(0);
            //                 course.CourseName = reader.GetString(1);
            //                 course.StartDate = reader.GetDateTime(2);
            //                 course.BatchId = reader.GetString(3);
            //                 course.VideoPath = (byte[])reader.GetValue(4);
            //                 course.PhotoPath = (byte[])reader.GetValue(5);
            //                 courses.Add(course);
            //             }
            //         }
            //         else
            //         {
            //             Console.WriteLine("No rows found.");
            //         }
            //     }
                

            //     return courses;
            // }

            using(SqlConnection sqlConnection = new SqlConnection(connectionString)){
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandTimeout = 200;
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = "Select * from [Courses]";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand.CommandText,sqlConnection);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);

                if (dataSet.Tables.Count > 0)    
                {
                    DataTable dt = dataSet.Tables[0];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Course course = new Course();
                        course.CourseId = dt.Rows[i]["CourseId"].ToString();
                        course.CourseName = dt.Rows[i]["CourseName"].ToString();
                        course.StartDate = Convert.ToDateTime(dt.Rows[i]["StartDate"].ToString());
                        course.BatchId = dt.Rows[i]["BatchId"].ToString();
                        course.VideoPath = (byte[])dt.Rows[i]["VideoPath"];
                        course.PhotoPath = (byte[])dt.Rows[i]["PhotoPath"];
                        courses.Add(course);
                    }
                }
            }

            return courses;
            
        }
        


        public IEnumerable FetchCoursesForBatch(string batchid){
            //fecth all the courses depending on batchId

            List<Course> courses = new List<Course>();

            using(sqlConnection = new SqlConnection(connectionString)){
                sqlConnection.Open();
                 SqlCommand search = new SqlCommand("Select * from [Courses] where BatchId = @id",sqlConnection);
                 search.CommandTimeout = 200;
                 search.CommandType = CommandType.Text;
                search.Parameters.Add("@id",SqlDbType.VarChar,50,"BatchId").Value = batchid;
                
                SqlDataReader reader = search.ExecuteReader();
                if(reader.HasRows){
                    while(reader.Read()){
                        Course course = new Course();
                        course.CourseId = reader.GetString(0);
                        course.CourseName = reader.GetString(1);
                        course.StartDate = reader.GetDateTime(2);
                        course.VideoPath = (byte[])reader.GetValue(4);
                        course.PhotoPath = (byte[])reader.GetValue(5);
                        courses.Add(course);
                    }
                }
                else{
                    Console.WriteLine("No rows found");
                }
                
            }

            return courses;
        }

        public Course FetchDetailsForPlaylist(string courseid,string coursename){

            
            //fecth all the courses depending on batchId
            Course course = new Course();

            using(sqlConnection = new SqlConnection(connectionString)){
                sqlConnection.Open();
                SqlCommand search = new SqlCommand("Select StartDate,VideoPath from [Courses] where CourseId = @id",sqlConnection);
                search.Parameters.Add("@id",SqlDbType.VarChar,50,"CourseId").Value = courseid;

                SqlDataReader reader = search.ExecuteReader();
                if(reader.HasRows){
                    while(reader.Read()){
                        course.CourseName = coursename;
                        course.StartDate = reader.GetDateTime(0);
                        course.VideoPath = (byte[])reader.GetValue(1);
                    }
                }
                else{
                    Console.WriteLine("No rows found");
                }
                
            }

            return course;
        }




        //User Defined Exception Implementation
        //MySqlException
        public class NoBatchFound:Exception{
            public  NoBatchFound(string Message) : base(Message){
                
            }
        }


         //check course with help of bacthId
         public bool CheckCourseOnBatchId(string coursename,string batchid){
            
            int count=0;
            try{
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                SqlCommand search = new SqlCommand("select count(CourseID) from Courses where BatchId = @id and CourseName = @name",sqlConnection);
                search.Parameters.AddWithValue("@id",batchid);
                search.Parameters.AddWithValue("@name",coursename);
                count = (int)search.ExecuteScalar();
                if(count == 0){
                    throw (new NoBatchFound("NO Batches Available"));
                }
            }
            catch(NoBatchFound NoBatchFoundexception){
                Console.WriteLine("My SqlException :" + NoBatchFoundexception.Message);
                return false;
            }
            catch(FormatException formatException){
                Console.WriteLine(formatException);
            }
            catch(SqlException sqlexception){
                Console.WriteLine("SqlException:" + sqlexception.Message);
            }
            catch(Exception exception){
                Console.WriteLine("Exception: "+exception.Message);
            }
            finally{
                sqlConnection.Close();
            }
            

            return true;
         }



    }

}