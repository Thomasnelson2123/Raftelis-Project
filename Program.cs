using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.FileIO;

namespace Raftelis_Project;
class Program
{ 
    // parses the text file, builds the HTML visualization, and opens it on your default browser
    static void Main(string[] args)
    {
        string htmlFile = "index.html";
        // if set to true, will overwrite existing file
        bool rewriteFile = true;
        // declare city and state here to make it easier to apply this program to different city information
        string city = "Mazama"; 
        string state = "WA";
        // build HTML file if it doesn't exist already
        if(!File.Exists(htmlFile) || rewriteFile) {
            string textFile = "Parcels.txt";
            List<List<string>> table = new List<List<string>>();
            table = ParseFile(textFile, "|", true, city, state);
            BuildHTMLFile(table, htmlFile);
        }

        // open on default browser
        var p = new Process();
        p.StartInfo = new ProcessStartInfo(htmlFile)
        { 
            UseShellExecute = true 
        };
        p.Start();
    }

    // this method takes in a text file and parses it to a 2D list, representing the table
    // can specifiy delimiter type, and whether or not to append a google maps column
    static List<List<String>> ParseFile(string fileName, string delimiter, bool useGoogleMaps, string city="", string state="") {
        if(!File.Exists(fileName)) {throw new Exception("file name not recognized");}
        bool firstLine = true; 
        TextFieldParser parser = new TextFieldParser(fileName);    
        List<List<string>> table = new List<List<string>>();    
        using (parser) {
            parser.SetDelimiters(delimiter);
            // read til end of file
            while (!parser.EndOfData) {
                string[]? row = parser.ReadFields();
                if (row != null)
                    if (useGoogleMaps) {
                        string[] mapsAppendedRow = row.Concat(new string[] {" "}).ToArray();
                        // if reading the first line, append the column header
                        if (firstLine) {
                            mapsAppendedRow[mapsAppendedRow.Length - 1] = "GOOGLE MAPS";
                            firstLine = false;
                        }
                        else {
                            // get the address index
                            int addressIndex = table[0].IndexOf("ADDRESS");
                            string address = row[addressIndex];
                            // format the address to query the google maps API
                            address += $", {city}, {state}";
                            string mapsAddress = "https:/www.google.com/maps/search/?api=1&query=" + Uri.EscapeDataString(address);
                            mapsAppendedRow[mapsAppendedRow.Length - 1] = mapsAddress;
                        }
                        row = mapsAppendedRow;
                    }
                    // add row to table (note: row should not be null, gives warning nonetheless)
                    table.Add(row.ToList<string>());       
            }
        }
        return table;
    }

    // test method to print the parsed text file
    static void PrintTextFile(List<List<String>> table) {
        int rowIndex = 0;
        foreach (List<string> row in table) {
            Console.Write(rowIndex + " ");
            foreach (string entry in row) {
                Console.Write(entry + " ");
            }
            Console.WriteLine();
            rowIndex++;
        }
    }

    // method to create the HTML file, given the parsed text file
    static void BuildHTMLFile(List<List<string>> table, string path) {
        string jqueryScript = "<script src=\"https://code.jquery.com/jquery-3.6.3.js\"></script>\n";
        string javascriptFile = "<script type=\"text/javascript\" src=\"main.js\"></script\n>";
        string pageName = "Raftelis Project";
        string htmlString = "";
        htmlString += $"<!DOCTYPE html>\n<html>\n<head>\n<title>{pageName}</title>\n<link rel=\"stylesheet\" type=\"text/css\" href=\"styles.css\"></head>\n<body>\n";
        // build the HTML table
        string tableString = BuildHTMLTable(table);
        htmlString+=tableString;
        htmlString+=jqueryScript;
        htmlString+= javascriptFile;
        htmlString+= "</body>\n</html>\n";

        // write to file
        File.WriteAllText(path, htmlString);
    }

    // this method builds and returns the HTML table for the front end app
    static string BuildHTMLTable(List<List<string>> table) {
        string tableString = $"<table id=\"Parcels\">\n<thead>\n<tr>\n";
        // following columns have hyperlinks
        int mapsCol = table[0].IndexOf("GOOGLE MAPS");
        int linkCol = table[0].IndexOf("LINK");
        // following columns are treated as currency values, are formatted differently
        int valueCol = table[0].IndexOf("MARKET_VALUE");
        int saleCol = table[0].IndexOf("SALE_PRICE");
        // builds table header
        for (int i = 0; i < table[0].Count; i++) {
            string id = $"{table[0][i]}";
            tableString+=$"<th id=\"{id}\" class=\"clickable\">{table[0][i]}</th>";
        }
        tableString+="\n</tr>\n</thead>\n<tbody>\n";
        // iterate over every row and append to HTML
        for (int row = 1; row < table.Count; row++) {
            tableString+="<tr>";
            for (int col = 0; col < table[0].Count; col++) {
                if (col == linkCol) {
                    tableString += $"<td><a href=\"{table[row][col]}\">Site Link</a></td>";
                }
                else if (col == mapsCol) {
                    tableString += $"<td><a href=\"{table[row][col]}\">Maps Link</a></td>";
                }
                else if (col == valueCol || col == saleCol) {
                    // format money values nicely
                    // to be safe, will ensure column values are formatted to be correctly parsed to decimal
                    string formated = Regex.Replace(table[row][col], @"[^\d.]", "");
                    decimal value = decimal.Parse(formated);
                    formated = string.Format("{0:C}", value);
                    tableString+=$"<td>{formated}</td>";
                }
                else {
                    tableString+=$"<td>{table[row][col]}</td>";
                } 
            }
            tableString+="</tr>\n";
        }
        tableString+="</tbody>\n</table>\n";

        return tableString;
    }
}

