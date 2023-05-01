// Rakendus andmebaasiga mydb

using System.Data.SQLite;
using System.Globalization;

ReadData(CreateConnection());
//InsertCustomer(CreateConnection());
//RemoveCustomer(CreateConnection());
FindCustomer(CreateConnection());


static SQLiteConnection CreateConnection()
{
    SQLiteConnection connection = new SQLiteConnection("Data Source=mydb.db; Version = 3; New = True, Compress = True");
    try
    {
        connection.Open();
        //Console.WriteLine("DB found.");
    }
    catch
    {
        Console.WriteLine("DB not found.");
    }
    return connection;
}


// andmebaasist andmete maha lugemine ja kuvamine
static void ReadData(SQLiteConnection myConnection)
{
    Console.Clear();
    SQLiteDataReader reader;
    SQLiteCommand command;

    command = myConnection.CreateCommand();
    command.CommandText = "SELECT rowid, * FROM customer";
    //command.CommandText = "SELECT customer.firstname, customer.lastname, status.statusType FROM customerStatus " +
    //    "JOIN customer ON customer.rowid = customerStatus.customerid " +
    //    "JOIN status ON status.rowid = customerStatus.statusId " +
    //    "ORDER BY status.statustype";

    reader = command.ExecuteReader();

    while (reader.Read())
    {
        string readerRowId = reader["rowid"].ToString(); // teine meetod reader.GetStringi asemel
        string readerStringFirstName = reader.GetString(1);
        string readerStringLastName = reader.GetString(2);
        string readerStringDoB = reader.GetString(3);
        //string readerStringStatus = reader.GetString(2);

        Console.WriteLine($"{readerRowId}. Full name: {readerStringFirstName} {readerStringLastName}; DoB; {readerStringDoB}");
    }
    myConnection.Close();
}

// andmete sisestamine
static void InsertCustomer(SQLiteConnection myConnection)
{
    SQLiteCommand command;
    string fName, lName, dob;

    Console.WriteLine("Enter first name:");
    fName = Console.ReadLine();
    Console.WriteLine("Enter last name:");
    lName = Console.ReadLine();
    Console.WriteLine("Enter date of birth (yyyy-mm-dd)");
    dob = Console.ReadLine();

    command = myConnection.CreateCommand();
    command.CommandText = $"INSERT INTO customer(firstName, lastName, dateOfBirth) " +
        $"VALUES ('{fName}', '{lName}', '{dob}')";
    
    int rowInserted = command.ExecuteNonQuery();
    Console.WriteLine($"Row inserted: {rowInserted}");

    ReadData(myConnection);
}

// andmete kustutamine
static void RemoveCustomer(SQLiteConnection myConnection)
{
    SQLiteCommand command;

    string idToDelete;
    Console.WriteLine("Enter an ID to deleta a customer:");
    idToDelete = Console.ReadLine();

    command = myConnection.CreateCommand();
    command.CommandText = $"DELETE FROM customer WHERE rowid = {idToDelete}";
    int rowRemoved = command.ExecuteNonQuery();
    Console.WriteLine($"{rowRemoved} was removed from the table customer");
    
    ReadData(myConnection);
}

// Kasutaja otsing

static void FindCustomer(SQLiteConnection myConnection)
{
    SQLiteDataReader reader;
    SQLiteCommand command;
    
    string searchName;
    Console.WriteLine("Enter a first name to display customer data:");
    searchName = Console.ReadLine();

    command =myConnection.CreateCommand();
    command.CommandText = $"SELECT customer.rowid, customer.firstname, customer.lastname, status.statustype " +
        $"FROM customerStatus " +
        $"JOIN customer ON customer.rowid = customerStatus.customerid " +
        $"JOIN status ON status.rowid = customerStatus.statusId " +
        $"WHERE firstname LIKE '{searchName}'";

    reader = command.ExecuteReader();

    while (reader.Read())
    {
        string readerRowid = reader["rowid"].ToString(); // teine meetod reader.GetStringi asemel
        string readerStringName = reader.GetString(1);
        string readerStringLastName = reader.GetString(2);
        string readerStringStatus = reader.GetString(3);

        Console.WriteLine($"Search result: ID: {readerRowid}. {readerStringName} {readerStringLastName}. Status: {readerStringStatus}");

    }
    myConnection.Close();

}