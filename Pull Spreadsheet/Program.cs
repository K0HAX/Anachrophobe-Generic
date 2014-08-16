using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.GData.Spreadsheets;
using System.Data;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Timers;


namespace Pull_Spreadsheet
{
    class Program
    {
        

        static void Main(string[] args)
        {
            Console.Write("Google username: ");
            string username = Console.ReadLine();
            Console.Write("Google password: ");
            // Password Section
            ConsoleKeyInfo key;
            string password = "";

            do
            {
                key = Console.ReadKey(true);

                // Backspace Should Not Work
                if (key.Key != ConsoleKey.Backspace)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    Console.Write("\b");
                }
            }
            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();
            // END section

            Console.Write("Document Name: ");
            string docName = Console.ReadLine();

            formatSheet mySheet = new formatSheet(username, password, docName, "Sheet1");

            TimerContainerStore myContainer = new TimerContainerStore();

            foreach (DataRow row in mySheet.dt.Rows)
            {
                TimerDatastore myData = new TimerDatastore();
                myData.UpdateStart(MasterTime.GetOffset());
                int myColumn = 0;
                foreach (object column in row.ItemArray)
                {
                    if (column.ToString() != "")
                    {
                        switch (myColumn)
                        {
                            case 0:
                                myData.UpdateLength(column.ToString());
                                MasterTime.SetOffset(MasterTime.GetOffset(), column.ToString());
                                //Console.WriteLine(showDate + " " + column.ToString() + "am");
                                break;
                            case 1:
                                myData.Name = column.ToString();
                                break;
                            default:
                                break;
                        }
                        myColumn++;
                    }
                }
                myContainer.addTimer(myData);
            }

            foreach (TimerDatastore timer in myContainer.Timers)
            {
                Console.WriteLine("Start Offset: " + timer.StartOffset.ToString());
                Console.WriteLine("End Offset: " + timer.Length.ToString());
                Console.WriteLine("Label: " + timer.Name);
                Console.WriteLine("-----------");
            }

            SaveStore.SaveControls(myContainer);
            
        }
    }
}
