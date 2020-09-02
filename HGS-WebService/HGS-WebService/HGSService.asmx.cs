using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace HGS_WebService
{
    /// <summary>
    /// Summary description for HGSService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class HGSService : System.Web.Services.WebService
    {


        SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-VK6705L\SQLEXPRESS;Initial Catalog=HgsDB;Integrated Security=True");



        [WebMethod]
        public bool SaleHGS(decimal balance, int customerId, string customerTCKN, string plateNumber, DateTime saleDate, string webOrMobil)
        {
            try
            {  
                connection.Open();
                string create = "INSERT INTO HGS(Balance,CustomerId,CustomerTCKN,PlateNumber,SaleDate,WebOrMobil) VALUES (@balance,@customerId,@customerTCKN,@plateNumber,@saleDate,@webOrMobil)";
                SqlCommand command = new SqlCommand(create,connection);
                command.Parameters.AddWithValue("@balance", balance);
                command.Parameters.AddWithValue("@CustomerId", customerId);
                command.Parameters.AddWithValue("@CustomerTCKN", customerTCKN);
                command.Parameters.AddWithValue("@PlateNumber",plateNumber);
                command.Parameters.AddWithValue("@SaleDate", saleDate);
                command.Parameters.AddWithValue("@WebOrMobil", webOrMobil);
                command.ExecuteNonQuery();
                connection.Close();

                return true;

            }
            catch (Exception)
            {
                throw new Exception("Hata Oluştu..");
            }
        }

        [WebMethod]
        public bool LoadBalance(string PlateNumber ,decimal balance)
        {
            try
            {
                connection.Open();

                string updateEntity = "Update HGS Set Balance = @Balance Where PlateNumber = @PlateNumber";

                SqlCommand command = new SqlCommand(updateEntity, connection);
                command.Parameters.AddWithValue("@Balance", balance);
                command.Parameters.AddWithValue("@PlateNumber", PlateNumber);
                command.ExecuteNonQuery();

                connection.Close();

                return true;
            }
            catch (Exception)
            {
                throw new Exception("Hata Oluştu..");
            }
        }

        [WebMethod]
        public bool UpdateBalance(string PlateNumber)
        {
            try
            {
                string _balance = 0.ToString();
                SqlCommand command = new SqlCommand();
                command.CommandText = "Select Balance from HGS Where PlateNumber = @PlateNumber";
                command.Connection = connection;
                connection.Open();
                command.Parameters.AddWithValue("@PlateNumber", PlateNumber);
                command.CommandType = CommandType.Text;

                SqlDataReader dataReader;
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    _balance = dataReader["Balance"].ToString();
                }

                
                UpdateBankDb(PlateNumber, _balance);
                connection.Close();
                return true;
            } 
            catch (Exception)
            {
                throw new Exception("Hata Oluştu..");
            }
        }


        public void UpdateBankDb(string PlateNumber , string _balance)
        {
            SqlConnection connection2 = new SqlConnection(@"Data Source=DESKTOP-VK6705L\SQLEXPRESS;Initial Catalog=BankDB;Integrated Security=True");

            try
            {
                connection2.Open();
                NumberStyles style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands;
                decimal Balance = Decimal.Parse(_balance, style);

                string updateEntity = "Update HGS Set Balance = @Balance Where PlateNumber = @PlateNumber";

                SqlCommand command = new SqlCommand(updateEntity, connection2);
                command.Parameters.AddWithValue("@Balance", Balance);
                command.Parameters.AddWithValue("@PlateNumber", PlateNumber);
                command.ExecuteNonQuery();

                connection2.Close();

             
            }
            catch (Exception)
            {
                throw new Exception("Hata Oluştu..");
            }


        }


    }
}
