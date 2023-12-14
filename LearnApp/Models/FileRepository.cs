
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace LearnApp.Models;
public class FileRepository{
    
    public void AddFile(Files file){


        string connectionString = "Data Source = LAPTOP-K0GUUCSK\\SQLEXPRESS;Initial Catalog = MaxLearnDB;Integrated Security = SSPI";
        using(SqlConnection connection = new SqlConnection(connectionString)){

            connection.Open();

            SqlCommand insert = new SqlCommand("insert into [Files] values(@id,@material,@name)",connection);
            insert.Parameters.AddWithValue("@id",file.CourseId);
            insert.Parameters.AddWithValue("@material",file.Material);
            insert.Parameters.AddWithValue("@name",file.MaterialName);
            insert.ExecuteNonQuery();
        }
    }


    public DataTable FetchFiles(string courseid){
        
        string connectionString = "Data Source = LAPTOP-K0GUUCSK\\SQLEXPRESS;Initial Catalog = MaxLearnDB;Integrated Security = SSPI";
        SqlConnection connection = new SqlConnection(connectionString);

        SqlCommand search = new SqlCommand("select Material,MaterialName from [Files] where CourseId = @id",connection);
        search.Parameters.AddWithValue("@id",courseid);
        SqlDataAdapter dataAdapter = new SqlDataAdapter(search);
        DataTable dataTable = new DataTable();
        dataAdapter.Fill(dataTable);

        return dataTable;
    }


    public void UpdateFileData(Files file){
        string connectionString = "Data Source = LAPTOP-K0GUUCSK\\SQLEXPRESS;Initial Catalog = MaxLearnDB;Integrated Security = SSPI";
        using(SqlConnection connection = new SqlConnection(connectionString)){

            connection.Open();

            SqlCommand update = new SqlCommand("update [Files] set Material = @material where CourseId = @id and MaterialName = @name",connection);
            update.Parameters.AddWithValue("@id",file.CourseId);
            update.Parameters.AddWithValue("@material",file.Material);
            update.Parameters.AddWithValue("@name",file.MaterialName);
            update.ExecuteNonQuery();
        }
    }

    public DataTable FetchFiles()
    {
        string connectionString = "Data Source = LAPTOP-K0GUUCSK\\SQLEXPRESS;Initial Catalog = MaxLearnDB;Integrated Security = SSPI";
        SqlConnection connection = new SqlConnection(connectionString);
        SqlCommand select = new SqlCommand("Select * from [Files]",connection);

        DataTable dataTable = new DataTable();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(select);
        dataAdapter.Fill(dataTable);

        return dataTable;
    }
}