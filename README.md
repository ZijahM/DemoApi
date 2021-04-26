# DemoApi
The API is built using DotNet Core along with the help of Entity framework and Npgsql packages which I used for creating a database context to manipulate the data in my Postgres database.
I used a code-first approach, coding the models first (since there were no strict guidlines for the models I went ahead and added the ID property) and using "dotnet ef add migration" created my database and tables.
The connection string is still the connection string I used on my database with my credentials and address of the database.
A script will be provided above for the identical database.

Since the return types in the assignment didn't match my model, I made Data transfer objects for certain controllers to return the identical objects. The main model also uses strings to represent dates, so I used a ToString method on my DateTimes with an emphasis on returning Zulu Time formats.
I also did some preventative checks to see if the Datetime is null because I didn.t use nullable DateTime since it doesn't work with ToString method overrides.

The database must contain the dates for CreatedAt and UpdatedAt, and I made sure that happens because when the POST request is made, the object gets a datetime.now value in both of these properties, and when a PUT request is called, the updatedAt property gets changed.
This is because the dates are returned in a such a manner in the assignement that they must not be nulls, although I first thought of making them nullable (DateTime?).


Another way of making sure the DateTime is always Zulu is also implementing a DateTimeKind attribude, however this time this approach was not necessary. 
