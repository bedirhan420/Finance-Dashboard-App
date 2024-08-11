using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace FinanceAPPServer.Firebase
{
    internal class FireBaseAuth
    {
        public static async Task<bool> LogIn(string email,string password) 
        {
			try
			{
				var data = await FireStoreDB.GetAdminData();

                var decodedPassword = Decrypt(data.Password);
                
                if (email == data.Email && password == decodedPassword)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
			catch (Exception e)
			{
                Console.WriteLine($" Error in Login : {e}");
                return false;
			}
        }

        public static string Decrypt(string str)
        {
            if (str == null) throw new ArgumentNullException("str");
            byte[] decodedBytes = Convert.FromBase64String(str);
            string decodedString = Encoding.UTF8.GetString(decodedBytes);

            return decodedString;
        }
    }
}
