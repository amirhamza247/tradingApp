using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using App;

//////// MY UTILITY METHODS //////////

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

/////////^^^^^^^^^ MY UTILITY METHODS ^^^^^^^^^//////////

List<IUser> allUsers = new List<IUser> { new Person("amir", "amir@mail.com", "amir") };

allUsers.Add(new Person("max", "max@mail.com", "max"));
allUsers.Add(new Person("jakob", "jakob@mail.com", "jakob"));
allUsers.Add(new Person("pierino", "pierino@mail.com", "pierino"));
allUsers.Add(new Person("muhammed", "muhammed@mail.com", "muhammed"));
LoadFromCsv("Users.csv");

void SaveToCsv(string path)
{
  List<string> lines = new List<string>();
  lines.Add("Name,Email,Password");
  foreach (Person currentPerson in allUsers)
  { lines.Add($"{currentPerson.Name},{currentPerson.Email},{currentPerson._Password}"); }
  File.WriteAllLines(path, lines);
  print("User saved to CSV!");
}

void LoadFromCsv(string path)
{
  if (!File.Exists(path)) return;
  string[] lines = File.ReadAllLines(path);
  allUsers.Clear();
  for (int i = 1; i < lines.Length; i++)
  {
    string[] parts = lines[i].Split(',');
    string name = parts[0];
    string mail = parts[1];
    string _password = parts[2];
    Person myPerson = new Person(name, mail, _password);
    allUsers.Add(myPerson);
  }
}



IUser activeUser = null;
bool isRunning = true;
while (isRunning)
{
  try { Console.Clear(); } catch { print("\n---------------\n"); }
  paint(ConsoleColor.DarkYellow, "\nTo be able to trade you must have an account.\n\n[1] Login\n[2] Creat an account\n");

  switch (input())
  {
    case "1":
      bool loginRunning = true;
      while (loginRunning)
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

          switch (activeUser)
          {
            case Person p:
              print($"\n{p.Name} you have succecfully loged in your account!\n");
              p.ShowMenu();
              break;
          }
          activeUser = null;
          loginRunning = false;

        }

      }

      break;

    case "2":
      paint(ConsoleColor.DarkYellow, "Enter your Name: ", "sameline");
      string userName = input();
      paint(ConsoleColor.DarkYellow, "Enter your Email: ", "sameline");
      string userEmail = input();
      paint(ConsoleColor.DarkYellow, "Enter your Password: ", "sameline");
      string user_password = input();

      Person newPerson = new Person(userName, userEmail, user_password);
      allUsers.Add(newPerson);
      print($"Congurgulations! **{userName}** you have succesfully created a Trading account.");
      print("\nPress enter to go to login menu...");
      input();
      SaveToCsv("Users.csv");
      break;


    default:
      print("Please enter a valit option...");
      break;
  }

}