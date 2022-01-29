using ClinicManagementSystemModelsLibrary;  //import statement
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystemFEConsoleApp
{
    internal class Program
    {
        void manageMenu()
        {
            ManageMenu mu = new ManageMenu();
            mu.AddHardCodeUsers();
            mu.AddHardCodeAppointments();
            mu.UserLogin();
        }

        static void Main(string[] args)
        {
            Program program = new Program();
            program.manageMenu();

            Console.ReadKey();
        }
    }
}
