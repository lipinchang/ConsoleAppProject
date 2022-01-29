using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystemModelsLibrary
{
    public class Appointment : IComparable
    {
        Appointment[] appointments;

        public Appointment()
        {

        }

        public Appointment this[int index]
        {
            get { return appointments[index]; }
            set { appointments[index] = value; }
        }

        public bool TimeSlotsAvailble(int docId, List<string> listOfToday)
        {
            int count = 0;
            bool isEmpty = false;
            for (int i = 0; i < listOfToday.Count; i++)
            {
                string a = listOfToday[i][listOfToday[i].Length - 1].ToString();
                string b = docId.ToString();

                if (a.Equals(b) && Convert.ToDateTime(listOfToday[i].Remove(listOfToday[i].Length - 1)) > DateTime.Now)
                {
                    Console.WriteLine(listOfToday[i].Remove(listOfToday[i].Length - 1));
                    count++;
                }
            }
            if (count == 0)
                isEmpty = true;

            return isEmpty;
        }

        public Boolean CheckTimeSlotCorrect(string time, int docId, List<string> listOfToday)
        {
            if (listOfToday.Contains(time + docId.ToString()))
                return true;
            else
                return false;
        }

        public Appointment(int id, string status, string remarks, string details, int doctorId, int patientId, DateTime appDate, string category, double paymentAmount, string paymentStatus)
        {
            Id = id;
            Status = status;
            Remarks = remarks;
            Details = details;
            DoctorId = doctorId;
            PatientId = patientId;
            DateCreated = Convert.ToDateTime("01-01-22 12:30 PM");
            AppDate = appDate;
            Category = category;
            PaymentAmount = paymentAmount;
            PaymentStatus = paymentStatus;      //new, paid, unpaid(time limit)
        }

        public override string ToString()
        {
            return "Appointment Id: " + Id
                + "\nStatus: " + Status
                + "\nRemarks: " + Remarks
                + "\nPatient Id: " + PatientId
                + "\nDate Created: " + DateCreated
                + "\nAppointment Date & Time: " + AppDate
                + "\nCategory: " + Category
                + "\nPayment Amount: $" + PaymentAmount
                + "\nPayment Status: " + PaymentStatus;
        }

        public int Id { get; set; }
        public string Status { get; set; }    //ongoing, new, old
        public string Remarks { get; set; }   //
        public string Details { get; set; }     //need to separate into smaller parts?
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime AppDate { get; set; }
        public string Category { get; set; }
        public double PaymentAmount { get; set; }
        public string PaymentStatus { get; set; }

        public void ViewListOfDoctors(User[] users, User u)
        {
            List<User> doctors = null;
           
            doctors = users.Where(d => d.Type == "Doctor").ToList();
          
            if(!doctors.Any())
                Console.WriteLine("No doctors");
            else
            {
                for (int i = 0; i < doctors.Count; i++)
                {
                    Console.WriteLine("-----------------------------------------");
                    Doctor a = new Doctor();
                    a = (Doctor)doctors[i];
                    Console.WriteLine("Doctor Name: " + a.Name + "\nId: "+a.Id +  "\nAge: " + a.Age + "\nExperience: " + a.Experience + "\nSpeciality: " + a.Speciality);
                    Console.WriteLine("-----------------------------------------");
                }
            }
        }

        //with appointment created
        public void ViewAppointmentDetailsWithNewAppointment(User[] users, List<Appointment> appointments,Appointment appointment, User u)
        {
            Appointment app = new Appointment();
            //for doctor and patient
            if (u.Type == "Doctor")
                app = FindAppoinmentByAppDateAndPatientIdForPatient(appointments, users, appointment.PatientId, appointment.AppDate);
            else
                app = FindAppoinmentByAppDateAndPatientIdForPatient(appointments, users, u.Id, appointment.AppDate);

            PrintAppointment(app);
        }

        //no appointment created
        public void ViewAppointmentDetails(User[] users, List<Appointment> appointments, User u)
        {
            //for doctor and patient
            Appointment app = new Appointment();
            app = FindAppoinmentByAppDateAndPatientId(appointments, users, u);
            PrintAppointment(app);
        }

        public void PrintAppointment(Appointment appointment)
        {
            Console.WriteLine("************************************");
            Console.WriteLine(appointment);
            Console.WriteLine("************************************");
        }

        public void PrintAppointments(List<Appointment> appointments)
        {
            appointments.Sort();
            foreach (var item in appointments)
            {
                if (item != null)
                    PrintAppointment(item);
            }
        }

        public void PrintPatientsInfo(User[] users)
        {
            List<User> patients = null;

            patients = users.Where(p => p.Type == "Patient").OrderBy(e => e.Id).ToList();
            foreach (var item in patients)
            {
                Console.WriteLine("************************************");
                Console.WriteLine(item);
                Console.WriteLine("************************************");
            }
        }

        public User FindPatientIDByName(User[] users)
        {
            string patientName="";
            Boolean userFound = false;
         
            User u = null;
            Console.WriteLine("Here is a list of patients information");
            PrintPatientsInfo(users);

            while (userFound == false)
            {
                Console.WriteLine("Please enter patient's full name");
                patientName = Console.ReadLine();    //try parse here
               
                u = users.SingleOrDefault(p => p.Name == patientName);
                if (u != null)
                    userFound = true;
                else
                    Console.WriteLine("Invalid patient's full name");
            }
            return u;
        }

        public Appointment FindAppoinmentByAppDateAndPatientId(List<Appointment> appointments, User[] users, User u)
        {
            DateTime appDate;
            int patientId = u.Id;
            Appointment app = null;
            List<Appointment> apps = null;
            Boolean appFound = false;

            while (appFound == false)
            {
                Console.WriteLine("Here is a list of appointments that you have");
                apps = appointments.Where(p => p.PatientId == patientId).ToList();
                PrintAppointments(apps);

                Console.WriteLine("Please enter your appointment & time date in format dd-mm-yyyy hh:mm tt (eg. 20-08-22 12:30 PM)");        
                while (!DateTime.TryParse(Console.ReadLine(), out appDate))
                {
                    Console.WriteLine("Invalid entry for appointment date & time. Please try again...");
                }

                app = apps.SingleOrDefault(p => p.PatientId == patientId && p.AppDate == appDate);
                if (app != null)
                    appFound = true;
            }
            return app;
        }

        public Appointment FindAppointmentById(List<Appointment> apps, int appId)
        {
            Appointment app = null;
            app = apps.SingleOrDefault(p => p.Id == appId);
            return app;
        }

        public Appointment FindAppoinmentByAppDateAndPatientIdForPatient(List<Appointment> apps, User[] users, int patientID, DateTime appDt)
        {
            Appointment app = null;
            Boolean appFound = false;

            while (appFound == false)
            {
                app = apps.SingleOrDefault(p => p.PatientId == patientID && p.AppDate == appDt);
                if(app != null)
                    appFound = true;
            }
            return app;
        }

        public int CompareTo(object obj)
        {
            Appointment a1, a2;
            a1 = this;
            a2 = (Appointment)obj;
            return a1.DoctorId.CompareTo(a2.DoctorId);
        }
    }
}
