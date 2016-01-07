# <a href="http://www.focusbi.co.uk/">Focus BI</a> Dashboard
Based on the work by Yorek(https://github.com/yorek/ssis-dashboard) this is a dashboard to monitor SSIS packages based in the database catalog. The dashboard will provide real time data on the status of task and is ideal for monitoring data migrations and similar jobs. 
Using the UI you can drill down to view the state of tasks, see messages or errors. The drilldown data is loaded dynamicall so it will be uptodate as can be. The screen can be set to auto refresh at a selected interval or can updated manually. All updates to the UI use json and are fairly lightweight so even 100+ errors will load in a second or over the net. Obviously when run locally updates are near instantaneous.
The dashboard uses a Visual Studio 2012/15 MVC web site which can be run locally or on a server. Data access is via EF 6.0.

A functioning demo can be found at: http://focusbidashboard.azurewebsites.net/

<p>Collapsed</p>
![dashboard1](https://cloud.githubusercontent.com/assets/15170287/12120517/11dcd2e4-b3c9-11e5-9e8f-4921aef46229.png) 

<p>Drill down for more</p>
![dashboard2](https://cloud.githubusercontent.com/assets/15170287/12120691/babf5602-b3c9-11e5-9706-003ba1a9d83a.png)

