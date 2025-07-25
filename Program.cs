//  Jonathan Bodrero
//  July 25, 25
//  Lab 10  ATM


using System.Diagnostics;

string[] readBankFile = LoadBankCustomers();
List<(string userName, int PIN, double balance)> customerData = new List<(string userName, int PIN, double balance)>();
(string userName, int PIN, double balance) testData = ("testRun", 98765, 1000);

string[] customerTemp;

Stack<double> transStack = new Stack<double>();

string nameTemp = "";
int PINTemp = 0;
double balanceTemp = 0;
int location; 
int option = -1;

foreach (string customer in readBankFile)       //  Load the bank data
{
    customerTemp = customer.Split(',');
    nameTemp = customerTemp[0];
    PINTemp = Convert.ToInt32(customerTemp[1]);
    balanceTemp = Convert.ToDouble(customerTemp[2]);
    customerData.Add((nameTemp, PINTemp, balanceTemp));
}

//Debug.Assert(ValidLogin(testData) == true);
//Debug.Assert(MakeDeposit(ref testData, 100));
//Debug.Assert(MakeDeposit(testData, 100) == true);

/*foreach (var customer in customerData)
{
    Console.WriteLine($"{customer.userName} has a PIN of {customer.PIN} with a balance of ${customer.balance}.");
}*/
(string userName, int PIN, double balance) currentCustomer = ("", -1, 0.0);  //  Create a tuple for current customer

Console.Clear();
Console.WriteLine("Welcome to BadgerBank.");
currentCustomer = ValidLogin(customerData);

/*if (currentCustomer.PIN != -1)
{
    Console.Clear();
    Console.WriteLine("Thank you for logging in. ");
}*/

do
{
    Console.Clear();
    Console.WriteLine(@"Please pick an option from the list below.
    1.  Check Balance
    2.  Withdraw
    3.  Deposit
    4.  Display last 5 transactions
    5.  Quick Withdraw $40
    6.  Quick Withdraw $100
    7.  End current session");
    Console.Write("Selection: ");

    while (!Int32.TryParse(Console.ReadLine(), out option)) ;  //  Check if input is an integer
    if (option < 1 || option > 7)
    {
        Console.WriteLine("Oops. That is not a valid option.  Try again.");
    }
    switch (option)
    {
        case 1:
            CheckBalance(ref currentCustomer);
            break;

        case 2:
            double withdrawl = -1;

            MakeWithdrawl(ref currentCustomer, withdrawl);
            break;

        case 3:
            double deposit = -1;
            MakeDeposit(ref currentCustomer, deposit);
            break;

        case 4:
            LastFive(ref currentCustomer, transStack);
            break;

        case 5:
            int amount40 = 40;
            QuickWithdrawl(ref currentCustomer, amount40);
            break;

        case 6:
            int amount100 = 100;
            QuickWithdrawl(ref currentCustomer, amount100);
            break;

        case 7:     //  Update current customer to bank file and write out to file.
            int place = customerData.FindIndex(x => x.userName == currentCustomer.userName);
            customerData.RemoveAt(place);
            customerData.Insert(place, currentCustomer);
            SaveBankCustomers(customerData);
            break;
    }
} while (option != 7);


string[] LoadBankCustomers()
{
    string[] Data = File.ReadAllLines("bank.txt");  // Read in bank database
    return Data;
}



void SaveBankCustomers(List<(string userName, int PIN, double balance)> customerData)
{
    List<string> writeBankFile = new List<string>();

    
    for (int i = 0; i < customerData.Count; i++)
    {
        writeBankFile.Add($"{customerData[i].userName}, {customerData[i].PIN}, {customerData[i].balance}");
    }
            //foreach(string bankFile in writeBankFile)
            //Console.WriteLine(bankFile);
            File.WriteAllLines("bank.txt", writeBankFile);
}

//  Method for validating login
(string userName, int PIN, double balance) ValidLogin(List<(string userName, int PIN, double balance)> customerData)
{

    string inputUser;
    int inputPIN;
    
    Console.Write("Please enter your userName: ");
    inputUser = Console.ReadLine();
    Console.Write("Please enter your PIN: ");
    while (!Int32.TryParse(Console.ReadLine(), out inputPIN))  //  Check if input is an integer
    {
        Console.Write("Oops. That is not a number. Try starting over.");
        return ("", -1, 0.0);
    }
    
    foreach (var user in customerData)
    {  if (user.userName == inputUser && user.PIN == inputPIN)
        {
            return user;  // Return the matched tuple
        }
    }

        Console.WriteLine("That is not a valid username / PIN number combination. Try starting over.");
        return ("", -1, 0.0);

}

//Method to check balance
void CheckBalance(ref (string userName, int PIN, double balance) currentCustomer)
{
    Console.WriteLine($"Your current balance is: ${currentCustomer.balance}.\nPress any key to continue.");
    Console.ReadKey(true);
}


//  Method for making deposits.
void MakeDeposit(ref (string userName, int PIN, double balance) currentCustomer, double deposit)
{
    bool isValidDeposit = false;

    while (!isValidDeposit)
    {
        Console.Write("How much are you depositing? $");
        string input = Console.ReadLine();
        input.Trim('$');
        input.Trim(' ');
        input.Trim(';');

        // Try to parse the input as a double > 0

        isValidDeposit = Double.TryParse(input, out deposit);

        if (!isValidDeposit)
        {
            Console.WriteLine("Oops. That is not a valid amount. Please enter a valid number.\nPress any key to continue.");
            Console.ReadKey(true);
            return;
        }
        if (deposit <= 0)
        {
            Console.WriteLine("Oops. That is not a valid amount. Please enter a number greater than 0.00.\nPress any key to continue.");
            Console.ReadKey(true);
            return;
        }
    }
    transStack.Push(deposit);
    currentCustomer.balance = currentCustomer.balance + deposit;
    Console.WriteLine($"Your current balance is: ${currentCustomer.balance}.\nPress any key to continue.");
    Console.ReadKey(true);
}

//  Methods for making withdrawls.
void MakeWithdrawl(ref (string userName, int PIN, double balance) currentCustomer, double withdrawl)
{
    bool isValidWithdrawl = false;

    while (!isValidWithdrawl)
    {
        Console.Write("How much are you withdrawing? $");
        string input = Console.ReadLine();
        input.Trim('$');
        input.Trim(' ');
        input.Trim(';');

        // Try to parse the input as a double > 0

        isValidWithdrawl = Double.TryParse(input, out withdrawl);

        if (!isValidWithdrawl)
        {
            Console.WriteLine("Oops. That is not a valid amount. Please enter a valid number.\nPress any key to continue.");
            Console.ReadKey(true);
            return;
        }
        if (withdrawl <= 0)
        {
            Console.WriteLine("Oops. That is not a valid amount. Please enter a number greater than 0.00.\nPress any key to continue.");
            Console.ReadKey(true);
            return;
        }
        if (withdrawl > currentCustomer.balance)
        {
            Console.WriteLine($"Oops. You don't have that much money in your account.  \nYou may withdraw up to ${currentCustomer.balance}.\nPress any key to continue.");
            Console.ReadKey(true);
            return;
        }
    }
    transStack.Push(-1 * withdrawl);
    currentCustomer.balance = currentCustomer.balance - withdrawl;
    Console.WriteLine($"Your current balance is: ${currentCustomer.balance}.\nPress any key to continue.");
    Console.ReadKey(true);

}

void QuickWithdrawl(ref (string userName, int PIN, double balance) currentCustomer, int amount)
{
    if (amount > currentCustomer.balance)
    {
        Console.WriteLine($"Oops. You don't have that much money in your account.  \nYou may withdraw up to ${currentCustomer.balance}.\nPress any key to continue.");
        Console.ReadKey(true);
        return;
    }
    currentCustomer.balance = currentCustomer.balance - amount;
    transStack.Push(-1 * amount);
    Console.WriteLine($"You are doing a quick withdrawl for ${amount}. \n Your new balance is ${currentCustomer.balance}.\nPress any key to continue.");
    Console.ReadKey(true);
}

void LastFive(ref (string userName, int PIN, double balance) currentCustomer, Stack<double>transStack)
{
    Console.WriteLine("Your last transactions during this login were: ");
    Stack<double> tempStack = new Stack<double>(transStack);

    int tempTrans = 5;
    if (transStack.Count < 5)
      { tempTrans = transStack.Count; }
    for (int i = 0;  i < tempTrans; i++)
    {
         double transaction = tempStack.Pop();

        // Display the transaction (positive for deposits, negative for withdrawals)
        if (transaction > 0)
        {
            Console.WriteLine($"Deposited: ${transaction}");
        }
        else
        {
            Console.WriteLine($"Withdrew:  ${-transaction}");
        }
    }
    Console.WriteLine($"Your current balance is: ${currentCustomer.balance}.\nPress any key to continue.");
    Console.ReadKey(true);
}