using ClinicManagementSystemModelsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystemFEConsoleApp
{
    internal class ManageMenu
    {
        User[] users = new User[5];
    
        List<Appointment> appointments= new List<Appointment>();

        //patient status: patient, non-patient, inactive, archived

        User user = null;
        ManageAppointment ma = new ManageAppointment();

        public Appointment this[int index]
        {
            get { return appointments[index]; }
            set { appointments[index] = value; }
        }

        public void AddHardCodeAppointments()
        {
            Appointment a1 = new Appointment(101, "Completed", "NIL", "NIL", 4, 1, Convert.ToDateTime("20-07-21 12:30 PM"), "General", 0, "Paid");
            Appointment a2 = new Appointment(102, "Completed", "NIL", "NIL", 5, 2, Convert.ToDateTime("21-08-21 12:30 PM"), "OBGYN", 123.5, "Paid");
            Appointment a3 = new Appointment(103, "Completed", "NIL", "NIL", 4, 2, Convert.ToDateTime("13-01-22 12:30 PM"), "General", 60, "Paid");
            Appointment a4 = new Appointment(104, "Processing", "NIL", "NIL", 5, 1, Convert.ToDateTime("25-01-22 12:30 PM"), "Brain", 90.50, "Pending payment");
            Appointment a5 = new Appointment(105, "Completed", "NIL", "NIL", 5, 2, Convert.ToDateTime("01-03-21 12:30 PM"), "Stomach", 0, "Paid");

            appointments.Add(a1);
            appointments.Add(a2);
            appointments.Add(a3);
            appointments.Add(a4);
            appointments.Add(a5);

            ma.CreateTodaysTimeslot();
        }
        public void AddHardCodeUsers()
        {
            User p1 = new Patient(1, "Poppy123","Poppy", 23, "qwertyu", "Patient", "NIL");
            User p2 = new Patient(2, "Adele321", "Adele", 50, "qwertyu", "Patient", "Went to NUH before");
            User p3 = new Patient(3, "Buddy123", "Buddy", 30, "qwertyu", "Patient", "NIL");
            User d1 = new Doctor(4, "Tom567", "Tom", 30, "qwertyu", 10, "Oncology");
            User d2 = new Doctor(5, "Timmy732", "Timmy", 50, "qwertyu", 20, "Cancer");

            users[0] = p1;
            users[1] = p2;
            users[2] = p3;
            users[3] = d1;
            users[4] = d2;
        }

        public void UserLogin()
        {
            int type = -1;
            Console.WriteLine("Welcome to the clinic");

            try
            {
                while (type != 0)
                {
                    Console.WriteLine("Key in 1 if you are a doctor");
                    Console.WriteLine("Key in 2 if you are a patient");

                    while (!int.TryParse(Console.ReadLine(), out type))
                    {
                        Console.WriteLine("Invalid entry for user type. Please key again...");
                    }

                    if (type == 1)
                    {
                        user = new Doctor();
                        Console.WriteLine("Hello Doctor");
                    }
                    else if (type == 2)
                    {
                        user = new Patient();
                        Console.WriteLine("Hello Patient");
                    }
                    else
                    {
                        Console.WriteLine("Invalid Entry.");
                        continue;
                    }
                    user = user.TakeLoginDetailsFromUserAndAuthenticate(user, users);
                    ChooseFunction(user);
                    type = 0;
                }
            }
            catch (NullReferenceException nre)
            {
                Console.WriteLine("Null mistake");
                Console.WriteLine(nre.Message);
            }
            catch (ArgumentOutOfRangeException aore)
            {
                Console.WriteLine("Appointment could not be found");
                Console.WriteLine(aore.Message);
            }
            catch (FormatException fe)
            {
                Console.WriteLine("Expecting a number");
                Console.WriteLine(fe.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Oops something went wrong");
                Console.WriteLine(e.Message);
            }
        }
     
        public void ChooseFunction(User u)
        {
            int choice = 0;

            do
            {
                if (u.Type == "Doctor")
                {
                    Console.WriteLine("Choose from the options");
                    Console.WriteLine("(1) Book appointment");
                    Console.WriteLine("(2) View your current appointments");
                    Console.WriteLine("(3) View your past appointments");
                    Console.WriteLine("(4) Raise payment request for an appointment");
                    Console.WriteLine("(5) Update appointment");
                    Console.WriteLine("(0) Exit");
                    Console.WriteLine("The current date time is: " + DateTime.Now);

                    while (!int.TryParse(Console.ReadLine(), out choice))
                    {
                        Console.WriteLine("Invalid entry. Please enter a number");
                    }
   
                    switch (choice)
                    {
                        case 1:
                            ma.BookAppointment(u, users, appointments);
                            break;
                        case 2:
                            ma.ViewCurrentAppointmentsById(u, users, appointments);
                            break;
                        case 3:
                            ma.ViewPastAppointmentsById(u, appointments);
                            break;
                        case 4:
                            ma.CreatePayment(u, appointments);
                            break;
                        case 5:
                            ma.UpdateAppointment(u, appointments, users);
                            break;
                        case 0:
                            Console.WriteLine("Bye " + u.Type);
                            break;
                        default:
                            Console.WriteLine("Invalid entry. ");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Choose from the options");
                    Console.WriteLine("(1) Book appointment");
                    Console.WriteLine("(2) View your current appointments");
                    Console.WriteLine("(3) View your past appointments");
                    Console.WriteLine("(4) Pay for appointment");
                    Console.WriteLine("(0) Exit");
                    Console.WriteLine("The current date time is: "+DateTime.Now);

                    while (!int.TryParse(Console.ReadLine(), out choice))
                    {
                        Console.WriteLine("Invalid entry. Please enter a number");
                    }

                    switch (choice)
                    {
                        case 1:
                            ma.BookAppointment(u, users, appointments);
                            break;
                        case 2:
                            ma.ViewCurrentAppointmentsById(u, users, appointments); 
                            break;
                        case 3:
                            ma.ViewPastAppointmentsById(u, appointments); 
                            break;
                        case 4:
                            ma.MakePayment(u, appointments);
                            break;
                        case 0:
                            Console.WriteLine("Bye " + u.Type);
                            break;
                        default:
                            Console.WriteLine("Invalid entry. ");
                            break;
                    }

                }
            } while (choice != 0);
  
        }
    }
}
