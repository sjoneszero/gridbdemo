# Market Price Data Analysis Demo

This is a  .NET 5 / Angular 12 based web application which allows the user to a upload market price related dataset (in the specified CSV file format), which then gets stored in a SQL Server database. Once the data has been successfully uploaded, the user can then view the data in graph format (powered by Chart.js) and also view specific analysis calculations - e.g. the min price, max price, average and the most expensive hour window.


In order to run the server, open the sln file in in Visual Studio. At this stage you can build the solution and then run the GridBeyondDemo.exe
The database layer uses the entity framework v5, and technically, once the DB connection string in server/GridBeyondDemo/appsetting.json is valid (assuming you've a local instance of SQL server), the app should run the db migration scripts and launch the URL **https://localhost:5001/api/Ping**

In order to run the client, navigate to the client folder and run the command: 

```sh
$ npm start
```

Once the packages are installed and the app is served, navigate to **http://localhost:4200**

![Screenshot](screenshots/datasets.png)

To upload a new dataset in CSV format, click on **Upload CSV**

![Screenshot](screenshots/datasets.png)

Add a description and select the CSV file to upload. When the file is uploaded, reload the datasets list to see the new import. Click on **Show Analysis** to view the analysis page 

![Screenshot](screenshots/analysis.png)