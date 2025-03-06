using System.Data;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace firstMVC.Models
{
	public class CustomerModel
	{
		private IConfiguration _configuration;
		private string _connectionString;

		public CustomerModel(IConfiguration configuration)
		{
			_configuration = configuration;
			_connectionString = "Server=localhost;Port=3308;Database=Violeta;User ID = dbkonstruktion; Password = Skata#23;Pooling=false;SslMode=none;convert zero datetime=True;";
        }

		//*************************************Show DesinfoSpridare table*****************************

		public DataTable GetDesinfoSpridare()
		{
			MySqlConnection dbcon = new MySqlConnection(_connectionString);
			dbcon.Open();
			MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM DesinfoSpridare;", dbcon);
			DataSet ds = new DataSet(); // vi gör ny data set
			adapter.Fill(ds, "result"); // vi lägger resultatet i dataset
			DataTable desinfoSpridareTable = ds.Tables["result"];
			dbcon.Close(); //stänger db konektion

			return desinfoSpridareTable;
        }

        //*************************************FRITEXTFÄLT för sökning efter namn*****************************


        public DataTable SearchCustomer(string name)
        {
            MySqlConnection dbcon = new MySqlConnection(_connectionString);
            dbcon.Open();
            MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM DesinfoSpridare WHERE namn=@agentsnamn;", dbcon);
            adapter.SelectCommand.Parameters.AddWithValue("@agentsnamn", name);
            DataSet ds2 = new DataSet();
            adapter.Fill(ds2, "result");
            DataTable AgentensTable = ds2.Tables["result"];
            dbcon.Close();

            return AgentensTable;
        }

        //**************************************** FORMULÅR för att lägga till data***************************************************

        public void AddAgent(string namn, string nr, string specialite, string namnInc, string nrInc)
        {
            MySqlConnection dbcon = new MySqlConnection(_connectionString);
            dbcon.Open();

            string addString = "INSERT INTO DesinfoSpridare (namn,nr,specialite,namnInc,nrInc) VALUES (@namn, @nr, @specialite, @namnInc, @nrInc);";
            MySqlCommand sqlCmd = new MySqlCommand(addString, dbcon);
            sqlCmd.Parameters.AddWithValue("@namn", namn);
            sqlCmd.Parameters.AddWithValue("@nr", nr);
            sqlCmd.Parameters.AddWithValue("@specialite", specialite);
            sqlCmd.Parameters.AddWithValue("@namnInc", namnInc);
            sqlCmd.Parameters.AddWithValue("@nrInc", nrInc);
            int rows = sqlCmd.ExecuteNonQuery();
            dbcon.Close();
        }

        //**************************************** Show Rapport table ******************

        public DataTable Repport()
        {
            MySqlConnection dbcon = new MySqlConnection(_connectionString);
            dbcon.Open();
            MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM Rapport;", dbcon);
            DataSet ds4 = new DataSet(); 
            adapter.Fill(ds4, "result"); 
            DataTable repportTable = ds4.Tables["result"];
            dbcon.Close(); 

            return repportTable;
        }
        //**************************************** DELETE Agent *********************************

        public void DeleteAgent(string namn, string nr)
        {
            MySqlConnection dbcon = new MySqlConnection(_connectionString);
            dbcon.Open();
            string deleteString = "DELETE FROM DesinfoSpridare WHERE namn=@namn AND nr=@nr;";
            MySqlCommand sqlCmd = new MySqlCommand(deleteString, dbcon);
            sqlCmd.Parameters.AddWithValue("@namn", namn);
            sqlCmd.Parameters.AddWithValue("@nr", nr);
            int rows = sqlCmd.ExecuteNonQuery();
            dbcon.Close();
        }

        //**************************************** DROPDOWN Rapport PROCEDURE*********************************

        public DataTable ChooseTitel(string titel)
        {
            MySqlConnection dbcon = new MySqlConnection(_connectionString);
            dbcon.Open();
            MySqlDataAdapter adapter = new MySqlDataAdapter("CALL getIncNamnByTitel2(@titel)", dbcon);
            adapter.SelectCommand.Parameters.AddWithValue("@titel", titel);
            DataSet ds4 = new DataSet(); 
            adapter.Fill(ds4, "result"); 
            DataTable repportTable = ds4.Tables["result"];
  
            dbcon.Close();
            return repportTable;
        }

        //**************************************** Show Fält Rapport table  *********************************

        public DataTable FaltRapport()
        {
            MySqlConnection dbcon = new MySqlConnection(_connectionString);
            dbcon.Open();
            MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM AvslRapFältrapport;", dbcon);
            DataSet ds = new DataSet(); 
            adapter.Fill(ds, "result"); 
            DataTable faltRapportTable = ds.Tables["result"];
            dbcon.Close(); //stänger db konektion

            return faltRapportTable;
        }

        //**************************************** PROCEDURE to move report to field report table *********************************

        public void MoveReport(string datumIn, string titelIn, string typNrIn)
        {
            MySqlConnection dbcon = new MySqlConnection(_connectionString);
            dbcon.Open();
            string deleteString = "CALL flyttaFältRap(@datumIn,@titelIn,@typNrIn)";
            MySqlCommand sqlCmd = new MySqlCommand(deleteString, dbcon);
            sqlCmd.Parameters.AddWithValue("@datumIn", datumIn);
            sqlCmd.Parameters.AddWithValue("@titelIn", titelIn);
            sqlCmd.Parameters.AddWithValue("@typNrIn", typNrIn);
            int rows = sqlCmd.ExecuteNonQuery();
            dbcon.Close();
        }
    }
}

