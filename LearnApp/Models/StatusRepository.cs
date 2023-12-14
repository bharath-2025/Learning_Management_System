namespace LearnApp.Models;
using System.Data.SqlClient;

public class StatusRepository{

    private string connectionString;
    private SqlConnection sqlConnection;
    private bool statusValue;
    public StatusRepository(){
        //connectionString = "Data Source = Aspire1550\\SQLEXPRESS;Initial Catalog = MaxLearnDB;Integrated Security = SSPI";
        connectionString = "Data Source = LAPTOP-K0GUUCSK\\SQLEXPRESS;Initial Catalog = MaxLearnDB;Integrated Security = SSPI";
        sqlConnection = new SqlConnection(connectionString);
    }

    //To Open Connection
    public void openConnection(){
        sqlConnection.Open();
        //Console.WriteLine("\nConnection Established");
    }

    public void closeConnection(){
        sqlConnection.Close();
    }

    //Thread Implementation
    public void Setstatus(){
        this.statusValue = true;
    }

    //Add the status into DB
    public void UpdateTodayReport(StatusReport report){
        SqlCommand insert = new SqlCommand("insert into [Status] values(@id,@batch,@date,@message)",sqlConnection);
        insert.Parameters.AddWithValue("@id",report.UserId);
        insert.Parameters.AddWithValue("@batch",report.BatchId);
        insert.Parameters.AddWithValue("@date",report.UpdatedDate);
        insert.Parameters.AddWithValue("@message",report.Message);
        openConnection();
        insert.ExecuteNonQuery();
        closeConnection();
    }


}