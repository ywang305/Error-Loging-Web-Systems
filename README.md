# Error-Loging-Web-Systems
### The application shall consist of 3 parts – a REST webservice, a website and a library for logging.	ALL
  -	The web application shall implement strong authentication.	Web
  - The web application shall support two roles – admin and user.	Web
  - The web application shall collect the following data about each user:	Web
    *	 First Name	Web
    *	 Last Name	Web
    *	 Email	Web
    *	 Password	Web
    *	Active/Inactive	Web
    *	Last login date	Web
  -	The web application shall allow an admin to create a new application.	Web
  -	The web application shall allow an admin to assign a user to an application.	Web
  -	The web application shall allow an admin to disable an existing application.	Web
  -	Admin shall be able to access all applications and all data for them.	Web
  -	Upon login, user shall be presented with a list of their applications.	Web
  -	User’s access shall be restricted to only active applications to which they are allowed access.	Web
  -	A user shall be able to select, from this list a applications to view.	Web
  -	The web application shall present the user with error log data in two forms:	Web
    *	List of errors, displaying all of the data	Web
    *	A graphical representation of the errors (by category, over time, or a suitable combination).	Web
  -	The web application shall allow the user to filter, search and order the logs, on all categories/columns.	Web
    *	The selection of the filtered data shall impact the graphical display.	Web
  -	The webservice shall receive error logs and save them into the database.	REST
    *	Extra work on performance.	REST
    *	Extra work on stability.	REST
  -	The library shall collect all necessary data and log them into the database.	DLL
    *	Extra work on performance.	DLL
    *	Extra work on stability.  RESTful or db disconnection, try-catch(): at minimal;  or better save in the file and recovery connected, sending email to notify )	DLL
  -	The library shall be configured at application startup w/ the ID of the application for which we are saving.	DLL
  -	The web application shall use the logger for its own logging.	Web
  -	Your application shall have a look and feel of a polished final product.	Web
    * Extra work on UI.	Web
##
## ![architecerrlog](https://user-images.githubusercontent.com/24782000/36354732-bd5a0eda-14a6-11e8-82fe-617d1c15ebf5.PNG)
