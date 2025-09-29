using App;

//////// MY SPECIAL METHODS //////////

//this methods converts Readline and Writeline to Print and Input.
static void print(string input)
{
  Console.WriteLine(input);
}
static string input()
{
  return Console.ReadLine() ?? "";
}


//this method gives text in console color.
static void paint(ConsoleColor color, string input, string newline = "newline")
{
  if (newline == "newline")
  {
    Console.ForegroundColor = color;
    print(input);
    Console.ResetColor();
  }
  if (newline == "sameline")
  {
    Console.ForegroundColor = color;
    Console.Write(input);
    Console.ResetColor();
  }
}

//this is to help vs code with debugging.
// try { Console.Clear(); } catch { Console.WriteLine("\n------------------\n");}

/////////^^^^^^^^^ MY SPECIAL METHODS ^^^^^^^^^//////////

List<IUser> allUsers = new List<IUser> { new Person("amir@mail.com", "amir") };

allUsers.Add(new Person("max@mail.com", "max"));
allUsers.Add(new Person("jakob@mail.com", "jakob"));
allUsers.Add(new Person("pierino@mail.com", "pierino"));
allUsers.Add(new Person("muhammed@mail.com", "muhammed"));


IUser activeUser = null;
bool isRunning = true;
while (isRunning)
{
  try { Console.Clear(); } catch { print("\n---------------\n"); }
  if (activeUser == null)
  {
    paint(ConsoleColor.DarkYellow, "\nPlease login to start trading!");
    bool isLoggedin = false;
    do
    {
      paint(ConsoleColor.DarkYellow, "\nEmail: ", "sameline");
      string email = input().ToLower().Trim();
      while (true)
      {
        if (!email.Contains('@') || string.IsNullOrWhiteSpace(email))
        {
          print("Please write a valid Email...");
          paint(ConsoleColor.DarkGray, "Ex: name@mail.com\n");
          paint(ConsoleColor.DarkYellow, "\nEmail: ", "sameline");
          email = input().ToLower().Trim();
        }
        else
        {
          bool emailExsist = false;
          foreach (IUser person in allUsers)
          {
            if (person.getEmail() == email)
            {
              emailExsist = true;
              break;
            }
          }
          if (emailExsist == true)
          { break; }
          if (emailExsist == false)
          {
            paint(ConsoleColor.Red, "Sorry Email is not registerd! Try another Email...");
            paint(ConsoleColor.DarkYellow, "\nEmail: ", "sameline");
            email = input().ToLower().Trim();
          }
        }
      }
      paint(ConsoleColor.DarkYellow, "\nPassword: ", "sameline");
      string password = input().ToLower().Trim();
      while (true)
      {
        if (string.IsNullOrWhiteSpace(password))
        {
          paint(ConsoleColor.Red, "\nPlease write a valid Password...");
          paint(ConsoleColor.DarkYellow, "\nPassword: ", "sameline");
          password = input().ToLower().Trim();
        }
        else
        { break; }
      }

      try { Console.Clear(); } catch { print("\n---------------\n"); }

      foreach (IUser person in allUsers)
      {
        if (person.tryLogin(email, password))
        {
          activeUser = person;
          isLoggedin = true;
          break;
        }
      }
      if (isLoggedin == false)
      {
        paint(ConsoleColor.Red, "\nIncorrect Password or Email. Press enter to try again...\n");
        input();
      }
    } while (!isLoggedin);
  }

  else
  {
    paint(ConsoleColor.DarkYellow, "\n-----Welcome to Trade World-----\n");
    paint(ConsoleColor.DarkYellow, "\nItems avaliable for trading: \n");
    paint(ConsoleColor.DarkYellow, "\nbanana \nhat\njacket");


    print("\nWrite logout to exit.\n");
    while (true)
    {
      string userInput = input();
      if (userInput == "logout")
      {
        activeUser = null;
        break;
      }
      else
      {
        print("\nplease enter a valid input...\n");
      }
    }
  }
}