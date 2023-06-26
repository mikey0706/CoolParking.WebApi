// See https://aka.ms/new-console-template for more information

using CoolParking.VL;

namespace Program 
{
    class Program
    {
        static void Main(string[] args)
        {

            Menu();
            Console.ReadLine();
        }

        public static void Menu() 
        {
            Homepage homepage = new Homepage();
            int opt = 100;

            while (opt != 0)
            {
                Console.WriteLine("Select your action by printing specific number.");
                Console.WriteLine("1 - Enter to admin account;");
                Console.WriteLine("2 - Enter to user account;");
                Console.WriteLine("0 - Exit;");

                opt = Convert.ToInt32(Console.ReadLine());

                if (opt == 1)
                {
                    homepage.AdminAccount();
                }
                else
                if (opt == 2)
                {
                    homepage.UserAccount();
                }
                else 
                if (opt == 0) 
                {
                    Environment.Exit(0);
                }

            }
        }

    }
}
