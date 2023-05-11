using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using System.Transactions;
namespace ExpensiveTracker
{
    public class TrackerApp
    {             
        public void AddTrans()
        {
            SqlConnection con = new SqlConnection("Server=US-CJB79S3; database=TestDB; Integrated Security=true");
            con.Open();
            SqlCommand cmd = new SqlCommand($"insert into Transactions values(@title,@description,@amount,@date)", con);
            Console.Write("Enter Title :");
            string title = Console.ReadLine();
            Console.Write("Enter Description: ");
            string description = Console.ReadLine();
            Console.Write("Enter Amount: ");
            decimal amount = decimal.Parse(Console.ReadLine());
            Console.Write("Enter Date(MM/DD/YYYY): ");
            DateTime date = DateTime.Parse(Console.ReadLine());
            cmd.Parameters.AddWithValue("@title", title);
            cmd.Parameters.AddWithValue("@description", description);
            cmd.Parameters.AddWithValue("@amount", amount);
            cmd.Parameters.AddWithValue("@date", date);
            cmd.ExecuteNonQuery();
            Console.WriteLine("Transaction Added Successfully");
            con.Close();
        }

        public void ShowExpenses()
        {
            Console.WriteLine("ID\tTitle\tDescription\tAmount\tDate");
            SqlConnection con = new SqlConnection("Server=US-CJB79S3; database=TestDB; Integrated Security=true");
            con.Open();
            SqlCommand cmd1 = new SqlCommand($"Select * from Transactions Where Amount < 0", con);
            SqlDataReader dr1 = cmd1.ExecuteReader();

            while (dr1.Read())
            {
                for (int i = 0; i < dr1.FieldCount; i++)
                {
                    object value = dr1.GetValue(i);
                    Console.Write($"{value}\t");
                }
                Console.WriteLine();
            }
            dr1.Close();
            con.Close();
        }

        public void ShowIncome()
        {
            Console.WriteLine("ID\tTitle\tDescription\tAmount\tDate");
            SqlConnection con = new SqlConnection("Server=US-CJB79S3; database=TestDB; Integrated Security=true");
            con.Open();
            SqlCommand cmd2 = new SqlCommand($"Select * from Transactions Where Amount > 0", con);
            SqlDataReader dr2 = cmd2.ExecuteReader();
            while (dr2.Read())
            {
                int n = dr2.FieldCount;
                for (int i = 0; i < n; i++)
                {
                    Console.Write($"{dr2[i]}\t");
                }
                Console.WriteLine();
            }
            dr2.Close();
            con.Close();
        }

        public void ShowBalance()
        {
            SqlConnection con = new SqlConnection("Server=US-CJB79S3; database=TestDB; Integrated Security=true");
            con.Open();
            SqlCommand cmd3 = new SqlCommand($"Select Sum(Amount) as TotalBalance from Transactions", con);
            SqlDataReader dr3 = cmd3.ExecuteReader();
            if (dr3.Read())
            {
                decimal totalBalance = dr3.GetDecimal(0);
                Console.WriteLine($"Rs {totalBalance}");
            }
            dr3.Close();
            con.Close();
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            TrackerApp t = new TrackerApp();
            string ret = "y";
            do
            {
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine("Welcome to Expensive Tracker");
                Console.WriteLine("1. Add Transaction");
                Console.WriteLine("2. View Expenses");
                Console.WriteLine("3. View Income");
                Console.WriteLine("4. View Balance");
                int choice = 0;
                try
                {
                    Console.WriteLine("Enter Your choice: ");
                    choice = Convert.ToInt16(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Enter only Numbers");
                }
                switch (choice)
                {
                    case 1:
                        {
                            Console.WriteLine("Enter the data of the Transaction");
                            t.AddTrans();
                            break;
                        }
                    case 2:
                        {
                            Console.WriteLine("The total Expenses recoreded so far are given below :");
                            t.ShowExpenses();
                            break;
                        }
                    case 3:
                        {
                            Console.WriteLine("The total Income recoreded so far is given below :");
                            t.ShowIncome();
                            break;
                        }
                    case 4:
                        {
                            Console.WriteLine("The total Balance remaining so far is given below :");
                            t.ShowBalance();
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Wrong Choice Entered");
                            break;
                        }
                }
                Console.WriteLine("Do you wish to continue? [y/n] ");
                ret = Console.ReadLine();
            } while (ret.ToLower() == "y");
        }
    }
}
 
   