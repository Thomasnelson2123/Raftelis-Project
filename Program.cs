using System.Diagnostics;
using Microsoft.VisualBasic.FileIO;

namespace Raftelis_Project;
class Program
{ 
    static void Main(string[] args)
    {
        string htmlFile = "index2.html";
        if(!File.Exists(htmlFile)) {
            string textFile = "Parcels.txt";
            List<List<string>> table = new List<List<string>>();
            table = ParseFile(textFile, "|");
            BuildHTMLFile(table, htmlFile);
        }

        var p = new Process();
        p.StartInfo = new ProcessStartInfo(htmlFile)
        { 
            UseShellExecute = true 
        };
        p.Start();
    }

    static List<List<String>> ParseFile(string fileName, string delimiter) {
        if(!File.Exists(fileName)) {throw new Exception("file name not recognized");}
        TextFieldParser parser = new TextFieldParser(fileName);    
        List<List<string>> table = new List<List<string>>();    
        using (parser) {
            parser.SetDelimiters(delimiter);
            while (!parser.EndOfData) {
                string[] row = parser.ReadFields();
                if (row != null)
                    table.Add(row.ToList<string>());       
            }
        }
        return table;
    }

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

    static void BuildHTMLFile(List<List<string>> table, string path) {
        string jqueryScript = "<script src=\"https://code.jquery.com/jquery-3.6.3.js\"></script>\n";
        string javascriptFile = "<script type=\"text/javascript\" src=\"main.js\"></script\n>";
        string pageName = "Raftelis Project";
        string htmlString = "";
        htmlString += $"<!DOCTYPE html>\n<html>\n<head>\n<title>{pageName}</title>\n</head>\n<body>\n";
        string tableString = BuildHTMLTable(table);
        htmlString+=tableString;
        htmlString+=jqueryScript;
        htmlString+= javascriptFile;
        htmlString+= "</body>\n</html>\n";
        File.WriteAllText(path, htmlString);
    }

    static string BuildHTMLTable(List<List<string>> table) {
        string tableString = $"<table id=\"Parcels\">\n<thead>\n<tr>\n";
        for (int i = 0; i < table[0].Capacity; i++) {
            string id = $"{table[0][i]}";
            tableString+=$"<th id=\"{id}\" class=\"clickable\">{table[0][i]}</th>";
        }
        tableString+="\n</tr>\n</thead>\n<tbody>\n";
        for (int row = 1; row < table.Count; row++) {
            tableString+="<tr>";
            for (int col = 0; col < table[0].Count; col++) {
                tableString+=$"<td>{table[row][col]}</td>";
            }
            tableString+="</tr>\n";
        }
        tableString+="</tbody>\n</table>\n";

        return tableString;
    }

}

