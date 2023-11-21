//namespace EmergencyRoomPriorityQueue_WilkersonV1
//{
//    internal class Program
//    {
//        static void Main(string[] args)
//        {
//            Console.WriteLine("Hello, World!");
//        }
//    }
//}


































//// Jonathan Wilkerson
//// IT113
//// NOTES: Please review and provide feedback.

//// BEHAVIORS NOT IMPLEMENTED AND WHY: None.

using System;
using System.Collections.Generic;
using System.IO;

namespace EmergencyRoomPriorityQueue_WilkersonV1
{
    // Patient class representing individual patients
    public class Patient
    {
        public string FirstName { get; }
        public string LastName { get; }
        public DateOnly Birthdate { get; }
        public int Priority { get; }

        public Patient(string firstName, string lastName, DateOnly birthdate, int priority)
        {
            FirstName = firstName;
            LastName = lastName;
            Birthdate = birthdate;
            Priority = priority;
        }

        public override string ToString()
        {
            return $"{LastName}, {FirstName} {Birthdate.ToString()}, {Priority}";
        }
    }

    // Medical Priority Queue using PriorityQueue<TElement,TPriority>
    public class ERQueue
    {
        private readonly PriorityQueue<Patient, int> priorityQueue = new PriorityQueue<Patient, int>();

        public int Enqueue(Patient patient)
        {
            priorityQueue.Enqueue(patient, patient.Priority);
            return priorityQueue.Count;
        }

        public Patient Dequeue()
        {
            if (!priorityQueue.TryDequeue(out var patient))
            {
                return null;
            }

            return patient;
        }

        public List<string> ListQueue()
        {
            var patientInfoList = new List<string>();
            foreach (var patient in priorityQueue)
            {
                patientInfoList.Add(patient.ToString());
            }

            return patientInfoList;
        }
    }

    class Program
    {
        static void Main()
        {
            // Load initial patients from the CSV file
            var erQueue = LoadPatients();

            while (true)
            {
                Console.WriteLine("Menu");
                Console.WriteLine("(A)dd Patient  (P)rocess Current Patient  (L)ist All in Queue  (Q)uit");
                var choice = Console.ReadLine()?.ToUpper();

                switch (choice)
                {
                    case "A":
                        Console.WriteLine("Enter patient information:");
                        Console.Write("First Name: ");
                        var firstName = Console.ReadLine();
                        Console.Write("Last Name: ");
                        var lastName = Console.ReadLine();
                        Console.Write("Birthdate (MM/DD/YYYY): ");
                        var birthdate = DateOnly.Parse(Console.ReadLine());
                        Console.Write("Priority (1-5): ");
                        var priority = int.Parse(Console.ReadLine());

                        // Adjust priority based on age
                        if ((DateTime.Now - birthdate.ToDateTimeUnspecified()).TotalDays / 365.25 < 21 ||
                            (DateTime.Now - birthdate.ToDateTimeUnspecified()).TotalDays / 365.25 > 65)
                        {
                            priority = 1;
                        }

                        var newPatient = new Patient(firstName, lastName, birthdate, priority);
                        erQueue.Enqueue(newPatient);
                        Console.WriteLine($"Patient added. Queue size: {erQueue.ListQueue().Count}");
                        break;

                    case "P":
                        var processedPatient = erQueue.Dequeue();
                        if (processedPatient != null)
                        {
                            Console.WriteLine($"Processed Patient: {processedPatient}");
                        }
                        else
                        {
                            Console.WriteLine("Queue is empty.");
                        }
                        break;

                    case "L":
                        var patientList = erQueue.ListQueue();
                        Console.WriteLine("Patients in Queue:");
                        foreach (var patientInfo in patientList)
                        {
                            Console.WriteLine(patientInfo);
                        }
                        break;

                    case "Q":
                        Console.WriteLine("Exiting program.");
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        static ERQueue LoadPatients()
        {
            var erQueue = new ERQueue();

            try
            {
                var lines = File.ReadAllLines("Patients.csv");

                foreach (var line in lines)
                {
                    var data = line.Split(',');
                    var firstName = data[1].Trim();
                    var lastName = data[0].Trim();
                    var birthdate = DateOnly.Parse(data[2].Trim());
                    var priority = int.Parse(data[3].Trim());

                    erQueue.Enqueue(new Patient(firstName, lastName, birthdate, priority));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error reading Patients.csv: {e.Message}");
            }

            return erQueue;
        }
    }
}