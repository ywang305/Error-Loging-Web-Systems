# Error-Loging-Web-Systems

#	Text	Component
1	The application shall consist of 3 parts – a REST webservice, a website and a library for logging.	ALL
2	The web application shall implement strong authentication.	Web
3	The web application shall support two roles – admin and user.	Web
4	The web application shall collect the following data about each user:	Web
a	First Name	Web
b	Last Name	Web
c	Email	Web
d	Password	Web
e	Active/Inactive	Web
f	Last login date	Web
5	The web application shall allow an admin to create a new application.	Web
6	The web application shall allow an admin to assign a user to an application.	Web
7	The web application shall allow an admin to disable an existing application.	Web
8	Admin shall be able to access all applications and all data for them.	Web
9	Upon login, user shall be presented with a list of their applications.	Web
10	User’s access shall be restricted to only active applications to which they are allowed access.	Web
11	A user shall be able to select, from this list a applications to view.	Web
12	The web application shall present the user with error log data in two forms:	Web
a	List of errors, displaying all of the data	Web
b	A graphical representation of the errors (by category, over time, or a suitable combination).	Web
13	The web application shall allow the user to filter, search and order the logs, on all categories/columns.	Web
a	The selection of the filtered data shall impact the graphical display.	Web
14	The webservice shall receive error logs and save them into the database.	REST
a	Extra work on performance.	REST
b	Extra work on stability.	REST
15	The library shall collect all necessary data and log them into the database.	DLL
a	Extra work on performance.	DLL
b	Extra work on stability.  RESTful or db disconnection, try-catch(): at minimal;  or better save in the file and recovery connected, sending email to notify )	DLL
16	The library shall be configured at application startup w/ the ID of the application for which we are saving.	DLL
17	The web application shall use the logger for its own logging.	Web
18	Your application shall have a look and feel of a polished final product.	Web
a	Extra work on UI.	Web
