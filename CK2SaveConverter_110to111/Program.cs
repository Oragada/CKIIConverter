using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.IO;



namespace CK2SaveConverter_110to111
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Copyright Oragada");

            //Get a list of all .ck2 filename (path) in the current folder
            List<string> filenames = Directory.GetFiles(Directory.GetCurrentDirectory()).Where(e=>e.EndsWith(".ck2")).ToList();

            //For each of these filename
            foreach (var filename in filenames)
            {
                //A Console write to show progress
                Console.WriteLine(string.Format("Starting on {0}", filename));

                //Open the save game and load in all lines of data
                string[] lines = File.ReadAllLines(filename);

                //Create a list to hold all the line numbers that contains Traits
                List<int> traitLines = new List<int>();

                //Go through each of the data lines, i being the line number
                for (int i = 0; i < lines.Length; i++)
                {
                    //load in the line
                    string l = lines[i];


                    if (l.Contains("traits="))
                    {
                        //if the line contains the text "traits=", marking the beginning of a trait block 
                        //the line 2 place farther down (where the trait numbers actually are) is added to the TraitLines list created earlier
                        traitLines.Add(i+2);
                    }

                }

                //Go through each trait line number
                foreach (int traitLineNum in traitLines)
                {
                    //It is then split by ' ' (space), which will be each trait followed by the Tabs and the ending bracket
                    string[] traits = lines[traitLineNum].Split(' ');

                    //Go through each of these numbers, not including the final part
                    for (int i = 0; i < traits.Length-1; i++)
                    {
                        //The actual replacement, goes back to the lines list and replace the relevant trait number directly
                        //A lot of conversion is going back and forth as we have to work on the trait number as an actual number
                        if (Convert.ToInt32(traits[i]) >= 112)
                        {
                            lines[traitLineNum] = lines[traitLineNum].Replace(traits[i],
                                                        (Convert.ToInt32(traits[i]) + 1).ToString(
                                                            CultureInfo.InvariantCulture));
                        }
                    }
                }

                string[] parts = filename.Split('.');

                //The actual file creation with the modified trait lines
                File.WriteAllLines((parts[0] + "-111." + parts[1]), lines);

            }
        }
    }
}
