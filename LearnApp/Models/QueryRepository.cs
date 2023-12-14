/***************************************************************************************
This Repository class is updated and corresponding module is implemented using WEB API
***************************************************************************************/


// using System.Data.SqlClient;
// using System.Collections;

// namespace LearnApp.Models{

//     public class QueryRepository{

//         private string connectionString;
//         private SqlConnection sqlConnection;
//         public QueryRepository(){
//             //connectionString = "Data Source = Aspire1550\\SQLEXPRESS;Initial Catalog = MaxLearnDB;Integrated Security = SSPI";
//             connectionString = "Data Source = LAPTOP-K0GUUCSK\\SQLEXPRESS;Initial Catalog = MaxLearnDB;Integrated Security = SSPI";
//             sqlConnection = new SqlConnection(connectionString);
//         }

//         //To Open Connection
//         public void openConnection(){
//             sqlConnection.Open();
//             //Console.WriteLine("\nConnection Established");
//         }

//         public void closeConnection(){
//             sqlConnection.Close();
//         }

//         //Add Query to database
//         public void AddQuery(Query query){
//             SqlCommand insert = new SqlCommand("INSERT INTO [QUERIES] (UserId,Email,Message) values(@id,@email,@message)",sqlConnection);
//             insert.Parameters.AddWithValue("@id",query.UserId);
//             insert.Parameters.AddWithValue("@email",query.Email);
//             insert.Parameters.AddWithValue("@message",query.Message);
//             openConnection();
//             insert.ExecuteNonQuery();
//             closeConnection();
//         }

//         //fecth Query from database
//         public IEnumerable FetchQuery(){

//             List<Query> queries = new List<Query>();

//             SqlCommand search = new SqlCommand("select * from [Queries]",sqlConnection);
//             openConnection();
//             SqlDataReader reader = search.ExecuteReader();
//             if (reader.HasRows)
//             {
//                 while (reader.Read())
//                 {
//                     Query query = new Query();
//                     query.QueryId = reader.GetGuid(0);
//                     query.UserId = reader.GetString(1);
//                     query.Email = reader.GetString(2);
//                     query.Message = reader.GetString(3);

//                     queries.Add(query);
//                 }
//             }
//             else
//             {
//                 Console.WriteLine("No rows found.");
//             }
//             reader.Close();
//             closeConnection();

//             return queries;

//         }

//         public void DeleteQuery(int id){
//             SqlCommand delete = new SqlCommand("delete from [Queries] where QueryId = @id",sqlConnection);
//             delete.Parameters.AddWithValue("@id",id);
//             openConnection();
//             delete.ExecuteNonQuery();
//             closeConnection();
//         }

//     }
// }