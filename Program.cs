//  Jonathan Bodrero
//  July 25, 25
//  Lab 10  ATM


string[] readBankFile = File.ReadAllLines("bank.txt");  // Read in bank database
List<(string userName, int PIN, double balance)> customerData = new List<(string userName, int PIN, double balance)>();
// string userName;
// int PIN;
// double balance;

string[] customerTemp;
//(string)

string nameTemp = "";
int PINTemp = 0;
double balanceTemp = 0;

foreach (string customer in readBankFile)
{
    customerTemp = customer.Split(',');
    nameTemp = customerTemp[0];
    PINTemp = Convert.ToInt32(customerTemp[1]);
    balanceTemp = Convert.ToDouble(customerTemp[2]);

    customerData.Add((nameTemp, PINTemp, balanceTemp));

}
// customerData.ForEach(thing => Console.Write($"{thing}, ")); // Cycles through list, printing each item.
// foreach (string customer in readBankFile)



foreach (var customer in customerData)
{
    Console.WriteLine($"{customer.userName} has a PIN of {customer.PIN} with a balance of ${customer.balance}.");
}

Console.Clear();
Console.WriteLine("Welcome to BadgerBank.");
if (ValidLogin(customerData) !=-1)
{
    Console.WriteLine("Thank you for logging in. ");
}



int ValidLogin(List<(string userName, int PIN, double balance)> customerData)
{

    string inputUser;
    int inputPIN;
    int location; 
    Console.Write("Please enter your userName: ");
    inputUser = Console.ReadLine();
    Console.Write("Please enter your PIN: ");
    while (!Int32.TryParse(Console.ReadLine(), out inputPIN))  //  Check if input is an integer
    {
        Console.Write("Oops. That is not a number. Try starting over.");
        return -1;
    }
    List<string> userTemp = new List<string>();
    foreach (var user in customerData)
    {   userTemp.Add(user.userName); }

    if (userTemp.Contains(inputUser))
    {
        location = userTemp.IndexOf(inputUser);
        return location;
    }
    else
    {
        Console.WriteLine("That is not a valid username / PIN number combination. Try starting over.");
        return -1;
    }

}

