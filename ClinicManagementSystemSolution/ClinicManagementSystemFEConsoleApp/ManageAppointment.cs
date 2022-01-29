using ClinicManagementSystemModelsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystemFEConsoleApp
{
    internal class ManageAppointment
    {
        List<string> listOfToday = new List<string>();
        Appointment a1 = new Appointment();

        public ManageAppointment()
        {
            
        }
        public void CreateTodaysTimeslot()
        {
            //all avail one
            listOfToday.Add("09:00 AM5");
            listOfToday.Add("09:00 AM4");
            listOfToday.Add("09:30 AM5");
            listOfToday.Add("09:30 AM4");
            listOfToday.Add("10:00 AM5");
            listOfToday.Add("10:00 AM4");
            listOfToday.Add("10:30 AM5");
            listOfToday.Add("10:30 AM4");
            listOfToday.Add("11:00 AM5");
            listOfToday.Add("11:00 AM4");
            listOfToday.Add("01:00 PM5");
            listOfToday.Add("01:00 PM4");
            listOfToday.Add("01:30 PM5");
            listOfToday.Add("01:30 PM4");
            listOfToday.Add("02:00 PM5");
            listOfToday.Add("02:00 PM4");
        }

        public void BookAppointment(User u, User[] users, List<Appointment> appointments)
        {
            //for doctor and patient
            Appointment appointment = new Appointment();
            int docId = 0;

            Console.WriteLine("You have chosen book appointment.");

            if (u.Type == "Doctor")
            {
                docId = u.Id;
                appointment.DoctorId = u.Id;
                appointment.PatientId = a1.FindPatientIDByName(users).Id;

            }
            else
            {
                appointment.PatientId = u.Id;
              
                Console.WriteLine("This is the list of doctors available");
                a1.ViewListOfDoctors(users, u);
                Console.WriteLine("Please enter a doctor id you would like to consult with");
                bool isDocIdCorrect = false;
                while (isDocIdCorrect == false)
                {
                    while (!Int32.TryParse(Console.ReadLine(), out docId))
                    {
                        Console.WriteLine("Invalid entry for doctor id. Please try again...");
                    }
                    User user = null;
                    user = users.SingleOrDefault(a => a.Id == docId && a.Type == "Doctor");
                    if (user != null)
                    {
                        appointment.DoctorId = docId;
                        isDocIdCorrect = true;
                    }
                    else
                        Console.WriteLine("Invalid entry for doctor id. Please try again...");
                }
            }
            Console.WriteLine("These are the timeslots available today");
            bool isEmpty = a1.TimeSlotsAvailble(docId, listOfToday);
            if (isEmpty)
            {
                Console.WriteLine("Sorry, the timeslots today are all taken up. Please come again tomorrow");
                return;
            }
                
            Console.WriteLine("Please enter a time in format hh:mm tt (eg. 12:30 PM)");

            string time = "";
            bool isCorrectTime = false;

            while (isCorrectTime == false)
            {
                time = Console.ReadLine();

                isCorrectTime = a1.CheckTimeSlotCorrect(time, docId, listOfToday);
                if (isCorrectTime == true)
                    isCorrectTime = true;
                else
                    Console.WriteLine("Incorrect time. Please choose from slots available. Try again");
            }

            DateTime AppDate = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + time);

            Console.WriteLine("Please enter a reason for the appointment.");
            string Category = Console.ReadLine();
            DateTime DateCreated = DateTime.Now;
            string Status = "New";
            appointment.Id = appointments.Count + 101;
            appointment.AppDate = AppDate;
            appointment.Category = Category;
            appointment.DateCreated = DateCreated;
            appointment.Status = Status;
            appointment.Remarks = "NIL";
            appointment.PaymentStatus = "New";
            appointments.Add(appointment);

            //remove the time from the avail list
            listOfToday.Remove(time + docId.ToString());

            Console.WriteLine("Your appointment has been booked.");
            a1.PrintAppointment(appointment);
        }

        public void ViewCurrentAppointmentsById(User user, User[] users, List<Appointment> appointments)
        {
            int id = user.Id;
            DateTime current = DateTime.Now;
            List<Appointment> apps = null;

            if (user.Type == "Doctor")
                apps = appointments.Where(p => p.DoctorId == id && p.AppDate >= current).ToList();
            else
                apps = appointments.Where(p => p.PatientId == id && p.AppDate >= current).ToList();

            if (!apps.Any())
                Console.WriteLine("You do not have any current appointments");
            else
                a1.PrintAppointments(apps);
        }

        public void ViewPastAppointmentsById(User user, List<Appointment> appointments)
        {
            int id = user.Id;
            DateTime current = DateTime.Now;
            List<Appointment> apps = null;

            if (user.Type == "Doctor")
                apps = appointments.Where(p => p.DoctorId == id && p.AppDate <= current).ToList();
            else
                apps = appointments.Where(p => p.PatientId == id && p.AppDate <= current).ToList();

            if (!apps.Any())
                Console.WriteLine("You do not have any past appointments");
            else
                a1.PrintAppointments(apps);
        }

        public void CreatePayment(User u, List<Appointment> appointments)
        {
            //for doctor
            Appointment app = null;
            List<Appointment> apps = null;
            int appId;
            bool isIdCorrect = false;
            Console.WriteLine("Please select an appointment for raising payment request");

            apps = appointments.Where(p => p.PaymentStatus == "New").ToList();
            a1.PrintAppointments(apps);

            while (isIdCorrect == false)
            {
                Console.WriteLine("Please enter the appointment ID");
                while (!Int32.TryParse(Console.ReadLine(), out appId))
                {
                    Console.WriteLine("Invalid entry for appointment id, please pick from above. Please try again...");
                }

                app = a1.FindAppointmentById(appointments, appId);

                if (app == null)
                    Console.WriteLine("No such appointment exist. Try again");
                else
                    isIdCorrect = true;
            }

            Console.WriteLine("Please enter any message to be saved");
            string remarks = Console.ReadLine();
            Console.WriteLine("Please enter the amount to be collected");
            double paymentAmount;
            while (!double.TryParse(Console.ReadLine(), out paymentAmount))
            {
                Console.WriteLine("Invalid entry for payment amount. Please try again...");
            }

            app.Remarks = remarks;
            app.PaymentAmount = paymentAmount;
            app.PaymentStatus = "Pending payment";
            app.Status = "Processing";

            Console.WriteLine("Payment is updated");
            a1.PrintAppointment(app);
        }

        public void UpdateAppointment(User u, List<Appointment> appointments, User[] users)
        {
            //for doctor
            Appointment app = null;

            Console.WriteLine("You have chosen to update appointment.");

            a1.PrintAppointments(appointments);

            Console.WriteLine("Please enter the appointment ID");
            int appId;
            while (!Int32.TryParse(Console.ReadLine(), out appId))
            {
                Console.WriteLine("Invalid entry for appointment ID. Please try again...");
            }

            app = a1.FindAppointmentById(appointments, appId);
            if (app == null)
            {
                Console.WriteLine("Invalid Id, cannot edit");
                return;
            }

            Console.WriteLine("Please enter which field number do you want to edit.");
            Console.WriteLine("(1) Remarks");
            Console.WriteLine("(2) Status");
            Console.WriteLine("(3) Appointment Time");
            Console.WriteLine("(4) Category");
            Console.WriteLine("(5) Payment amount");
            Console.WriteLine("(6) Payment Status");

            int choice;
            while (!Int32.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid entry for field number. Please try again...");
            }

            string editTo = "";
            DateTime appDate = new DateTime();

            if (choice == 3)
            {
                if (app.AppDate < DateTime.Now)
                {
                    Console.WriteLine("This appointment date is past today's date, you cannot update the appointment date");
                    return;
                }

                Console.WriteLine("These are the timeslots available today that you can choose from");
                
                bool isEmpty = a1.TimeSlotsAvailble(u.Id, listOfToday);
                if (isEmpty)
                {
                    Console.WriteLine("Sorry, the timeslots today are all taken up. Please come again tomorrow");
                    return;
                }
                Console.WriteLine("Please enter, the time format is hh:mm tt (eg. 12:30 PM)");

                //string time = "";
                bool isCorrectTime = false;

                while (isCorrectTime == false)
                {
                    editTo = Console.ReadLine();

                    isCorrectTime = a1.CheckTimeSlotCorrect(editTo, u.Id, listOfToday);
                    if (isCorrectTime == true)
                        isCorrectTime = true;
                    else
                        Console.WriteLine("Incorrect time. Please choose from slots available. Try again");
                }

                appDate = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + editTo);
            }
            else
            {
                Console.WriteLine("Please enter what values you want to change to:");
                editTo = Console.ReadLine();
            }           

            switch (choice)
            {
                case 1:
                    app.Remarks = editTo;
                    break;
                case 2:
                    app.Status = editTo;
                    break;
                case 3:                                       
                    if (app.AppDate.ToShortDateString() == DateTime.Now.ToShortDateString())
                    {
                        listOfToday.Remove(app.AppDate.ToShortDateString()+app.DoctorId);
                        app.AppDate = appDate;
                    }
                    break;
                case 4:
                    app.Category = editTo;
                    break;
                case 5:
                    double paymentAmt;
                    while (!double.TryParse(Console.ReadLine(), out paymentAmt))
                    {
                        Console.WriteLine("Invalid entry for price. Please try again...");
                    }
                    app.PaymentAmount = paymentAmt;
                    break;
                case 6:
                    app.PaymentStatus = editTo;
                    break;
                default:
                    Console.WriteLine("Invalid Entry.");
                    break;
            }
            Console.WriteLine("The appointment has been changed.");
            a1.PrintAppointment(app);
        }

        public void MakePayment(User u, List<Appointment> apps)
        {
            //for patient
            Appointment app = null;
            app = apps.SingleOrDefault(p => p.PaymentStatus == "Pending payment" && p.PatientId == u.Id);
            if (app == null)
                Console.WriteLine("You do not have outstanding payment.");
            else
            {
                Console.WriteLine("This is your outstanding appointment:");
                a1.PrintAppointment(app);
                Console.WriteLine("You have outstanding bills of : $" + app.PaymentAmount);

                bool isFieldCorrect = false;
                while (isFieldCorrect == false)
                {
                    Console.WriteLine("Do you want to pay now? (y/n)");
                    string choice = Console.ReadLine();
                    if (choice == "y")
                    {
                        app.PaymentStatus = "Paid";
                        app.Status = "Completed";
                        Console.WriteLine("Payment is successful");
                        a1.PrintAppointment(app);
                        isFieldCorrect = true;
                    }
                    else if (choice == "n")
                        return;
                    else
                        Console.WriteLine("Incorrect entry. Try again");
                }
            }
        }
    }
}
