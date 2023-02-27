# Raftelis-Project
## Overview of Project
This project was designed to be scalable, such that if new data were appended to the text file, or new columns of data were added, little to no changes would need to be made to the code to visualize them.
Below is a listing of the main files of the program, and briefly what each does.
### Program.cs
This file is the driver of the program. It parses the `Parcels.txt` file and creates an HTML file to visualize the data cleanly.
### main.js
This file gives functionality to the HTML, allowing the user to click on any of the column headers in order to sort by that column. 
Clicking a second time will then sort it in descending order.
### style.css
Styling code to make the visualization look nice.
### index.html
This file is not included on the repository. It is generated when you run `Program.cs`.   
## How to run application:
This application uses C# to build an HTML file to visualize the given delimited data file.
You can either run this code in an IDE like Visual Studio Code, or from the command line by running
`dotnet run Program.cs`.
This will create the HTML file, and then automatically open it in your default browser. If for some reason it does not open automatically
(perhaps a permissions error), the file will be stored in the main directory  
  
