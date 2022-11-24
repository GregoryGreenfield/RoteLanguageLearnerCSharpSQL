/*
Written by Gregory Greenfield. 18/11/22
gregorysgreenfield@gmail.com

This program tests the user with predefined German words multiple times. The program revolves around the RoteNumber: 
    RoteNumber = 0 for newly added words
    RoteNumber++ if the user is correct
    RoteNumber-- if the user is not correct
    if 5<RoteNumber<16 then RoteNumber++
    if RoteNumber==16 then the user is tested again. If they are correct then RoteNumber = 17 and the word is considered memorised. If they are incorrect, then RoteNumber = 0, as though it were a new word. 
    RoteNumber can not go below 0.
The data is stored on a SQL database using Docker. This is because the programmer wrote this on a Mac.
*/
using Microsoft.Data.SqlClient;
namespace CSharpSQLRoteWordLearner
{
    class Program
    {
        // Tests connection and return the entire database
        static List<List<string>> GetWholeDatabase(SqlConnection ConnectionString)
        {
            List<List<string>> rowEntries = new List<List<string>>(); // List of col
            List<string> columnEntries = new List<string>();

            using (SqlConnection conn = ConnectionString)
            {
                try
                {
                    using (conn)
                    {
                        // tests to see if the connection is successful
                        conn.Open();
                        Console.WriteLine("Connection to SQL successful");

                        // SQL statement selecting the entire table
                        String stringWholeDB = "SELECT * FROM GermanWords;";
                        using (SqlCommand comm = new SqlCommand(stringWholeDB, conn))
                        {
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    columnEntries = new List<string>();
                                    columnEntries.Add(Convert.ToString(reader.GetSqlInt32(0)));
                                    columnEntries.Add((string)reader.GetSqlString(1));
                                    columnEntries.Add((string)reader.GetSqlString(2));
                                    columnEntries.Add((string)reader.GetSqlString(3));
                                    columnEntries.Add((string)reader.GetSqlString(4));
                                    columnEntries.Add((string)reader.GetSqlString(5));
                                    columnEntries.Add(Convert.ToString(reader.GetSqlInt32(6)));
                                    rowEntries.Add(columnEntries);
                                }
                            }
                        }
                        conn.Close();
                    }
                }
                catch (SqlException e)
                {
                    Console.WriteLine("Connection to SQL not successful");
                    Console.WriteLine(e.ToString());
                }
            }
            return rowEntries;
        }

        // Updates the SQL database connection and return the entire database - need to write the code
        static void updateSQLDatabase(SqlConnection ConnectionString, string updatedDB)
        {
            using (SqlConnection conn = ConnectionString)
            {
                try
                {
                    using (conn)
                    {
                        // tests to see if the connection is successful
                        conn.Open();

                        // SQL statement selecting the entire table
                        String stringWholeDB = updatedDB;
                        using (SqlCommand comm = new SqlCommand(stringWholeDB, conn))
                        {
                            SqlDataReader reader = comm.ExecuteReader();
                        }
                    }
                    conn.Close();
                }
                catch (SqlException e)
                {
                    Console.WriteLine("Connection to SQL not successful");
                    Console.WriteLine(e.ToString());
                }
            }
        }

        // Main shenanigans
        static void Main(string[] args)
        {
            #region Connection details and string builder
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "localhost,1433";
            builder.UserID = "sa";
            builder.Password = "5uper5trongPW!";
            builder.InitialCatalog = "master";
            builder.TrustServerCertificate = true;
            builder.Pooling = false;
            SqlConnection connectionGetDB = new SqlConnection(builder.ConnectionString);
            SqlConnection connectionUpdateDB = new SqlConnection(builder.ConnectionString);
            #endregion

            // variables 
            bool exitRequested;
            string userInput;
            string roteValueStr;
            int roteValueInt;

            // Test connection and returns the entire database in list form
            List<List<string>> originalDB;
            originalDB = GetWholeDatabase(connectionGetDB);

            // Give instructions, check for exit
            Console.WriteLine("\nPlease press \ne for exit\ny when you’re correct\nn when you’re incorrect \nPress enter for next word.");

            // check if want to quit
            userInput = Console.ReadLine();
            if (userInput == "e")
            {
                exitRequested = true;
                goto programEnd;
            }
            else
            {
                exitRequested = false;
            }
            // Create SQDL db command beginning
            string returnDBSQLCommand = "DELETE FROM GermanWords; INSERT INTO GermanWords (WordID, EnglishWord, GermanWord, Gender, WordType, NudgingNotes, RoteNumber) VALUES ";

            // see whether a word needs testing
            for (int column = 0; column < originalDB.Count(); column++)
            {
                if (exitRequested == false)
                {
                    switch (Convert.ToInt16(originalDB[column][6])) // get the RoteValue for the respective rows
                    {
                        case <= 5:
                            // test user for correct definition
                            Console.WriteLine(originalDB[column][1]);
                            Console.ReadLine(); // wait for return key
                            Console.WriteLine(originalDB[column][2] + "\t" + originalDB[column][3] + "\t" + originalDB[column][5]);
                            Console.WriteLine("Did you get it right? y/n");
                            var CorrectAnswer = Console.ReadLine(); // wait for response

                            switch (CorrectAnswer)
                            {
                                case "e":
                                    exitRequested = true;
                                    break;

                                case "y":
                                    roteValueInt = Convert.ToInt16(originalDB[column][6]);
                                    roteValueInt++;
                                    roteValueStr = Convert.ToString(roteValueInt);
                                    originalDB[column][6] = roteValueStr;
                                    break;

                                case "n":
                                    if (originalDB[column][6] == "0")
                                    {
                                        ;
                                    }
                                    else
                                    {
                                        roteValueInt = Convert.ToInt16(originalDB[column][6]);
                                        roteValueInt--;
                                        roteValueStr = Convert.ToString(roteValueInt);
                                        originalDB[column][6] = roteValueStr;
                                    }
                                    break;

                                default:
                                    Console.WriteLine("You did not enter an appropriate character");
                                    break;
                            }
                            break;
                        // update without questioning the user
                        case > 5 and <= 15:
                            roteValueInt = Convert.ToInt16(originalDB[column][6]);
                            roteValueInt++;
                            roteValueStr = Convert.ToString(roteValueInt);
                            originalDB[column][6] = roteValueStr;
                            break;

                        // revisit 5-time passed word
                        case 16:
                            Console.WriteLine(originalDB[column][1]);
                            Console.ReadLine(); // wait for return key
                            Console.WriteLine(originalDB[column][2] + originalDB[column][3] + originalDB[column][5]);
                            Console.WriteLine("Did you get it right? y/n");
                            CorrectAnswer = Console.ReadLine();
                            switch (CorrectAnswer)
                            {
                                case "e":
                                    exitRequested = true;
                                    break;

                                case "y":
                                    roteValueInt = Convert.ToInt16(originalDB[column][6]);
                                    roteValueInt = 17;
                                    roteValueStr = Convert.ToString(roteValueInt);
                                    originalDB[column][6] = roteValueStr;
                                    break;

                                case "n":
                                    roteValueInt = Convert.ToInt16(originalDB[column][6]);
                                    roteValueInt = 0;
                                    roteValueStr = Convert.ToString(roteValueInt);
                                    originalDB[column][6] = roteValueStr;
                                    break;

                                default:
                                    Console.WriteLine("You did not enter an appropriate character");
                                    break;
                            }
                            break;

                        // correct when revisited
                        case 17:
                            ; // do nothing
                            break;

                        default:
                            ;
                            break;
                    }
                }
                else
                {
                    ;
                }
                returnDBSQLCommand = returnDBSQLCommand + "('" + originalDB[column][0] + "', '" + originalDB[column][1] + "', '" + originalDB[column][2] + "', '" + originalDB[column][3] + "', '" + originalDB[column][4] + "', '" + originalDB[column][5] + "', '" + originalDB[column][6] + "'), ";
            }
            returnDBSQLCommand = returnDBSQLCommand.Remove(returnDBSQLCommand.Length - 2); // remove from index length minus 2
            returnDBSQLCommand = returnDBSQLCommand + ";"; // add the final end line to the instruction
            // call the update sql db function passing in the updated List<List>>
            updateSQLDatabase(connectionUpdateDB, returnDBSQLCommand);
            
            // Close / end program
        programEnd:
            Console.WriteLine("Words finished!\nProgram closed.");
        }
    }
}