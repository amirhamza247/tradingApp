using System.Buffers;
using System.Globalization;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using App;

//////// MY UTILITY METHODS //////////

//activeUser methods converts Readline and Writeline to Print and Input.
static void print(string input)
{
  Console.WriteLine(input);
}
static string input()
{
  return Console.ReadLine() ?? "";
}


//activeUser method gives text in console color.
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

//activeUser is to help vs code with debugging.
// try { Console.Clear(); } catch { Console.WriteLine("\n------------------\n");}

/////////^^^^^^^^^ MY UTILITY METHODS ^^^^^^^^^//////////

List<Person> allUsers = new List<Person>();
List<TradeRequest> allRequests = new List<TradeRequest>();


allUsers.Add(new Person("amir", "amir@mail.com", "amir"));
allUsers.Add(new Person("max", "max@mail.com", "max"));
allUsers.Add(new Person("jakob", "jakob@mail.com", "jakob"));
allUsers.Add(new Person("pierino", "pierino@mail.com", "pierino"));
allUsers.Add(new Person("muhammed", "muhammed@mail.com", "muhammed"));

LoadUsersFromCsv("users.csv");
LoadItemFromCsv("items.csv");
LoadTradesFromCsv("trades.csv");

void SaveTradesToCsv(string path)
{
  List<string> lines = new List<string>();
  lines.Add("RequesterName,RequesterItem,OwnerName,OwnerItem,TradeStatus");
  foreach (TradeRequest request in allRequests)
  {
    lines.Add($"{request.Requester.Name},{request.RequesterItem.Name},{request.Owner.Name},{request.OwnerItem.Name},{request.Status}");
  }
  File.WriteAllLines(path, lines);
}

Person? GetPersonByName(string name)
{
  foreach (Person person in allUsers)
  {
    if (person.Name == name)
    {
      return person;

    }
  }
  return null;
}



void LoadTradesFromCsv(string path)
{
  if (!File.Exists(path)) return;
  string[] lines = File.ReadAllLines(path);
  allRequests.Clear();
  for (int i = 1; i < lines.Length; i++)
  {
    string[] parts = lines[i].Split(',');
    string requesterName = parts[0];
    string requesterItem = parts[1];
    string ownerName = parts[2];
    string ownerItem = parts[3];
    string requestStatus = parts[4];

    Person? requester = GetPersonByName(requesterName);
    Person? owner = GetPersonByName(ownerName);

    if (requester is null || owner is null) continue;

    Items? reqItem = null;
    foreach (Items item in requester.myItemsList)
    {
      if (requesterItem == item.Name)
      {
        reqItem = item;
        break;
      }
    }


    Items? ownItem = null;
    foreach (Items item in owner.myItemsList)
    {
      if (ownerItem == item.Name)
      {
        ownItem = item;
        break;
      }
    }
    if (reqItem is null || ownItem is null) continue;

    TradeStatus status;
    if (!Enum.TryParse(requestStatus, true, out status))
    {
      status = TradeStatus.Pending;
    }

    TradeRequest csvTradeRequest = new TradeRequest(requester: requester, requesterItem: reqItem, owner: owner, ownerItem: ownItem);
    csvTradeRequest.Status = status;

    allRequests.Add(csvTradeRequest);
  }
}

void SaveUsersToCsv(string path)
{
  List<string> lines = new List<string>();
  lines.Add("Name,Email,Password");
  foreach (Person currentPerson in allUsers)
  { lines.Add($"{currentPerson.Name},{currentPerson.Email},{currentPerson._Password}"); }
  File.WriteAllLines(path, lines);
}

void LoadUsersFromCsv(string path)
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
void SaveItemsToCsv(string path)
{
  List<string> lines = new List<string>();
  lines.Add("OwnerEamil,Name,Description");
  foreach (Person user in allUsers)
  {
    if (user.myItemsList != null)
    {
      foreach (Items item in user.myItemsList)
      {

        lines.Add($"{user.Email},{item.Name},{item.Description}");

      }

    }

  }
  File.WriteAllLines(path, lines);
}

void LoadItemFromCsv(string path)
{
  if (!File.Exists(path)) return;
  string[] lines = File.ReadAllLines(path);

  for (int i = 1; i < lines.Length; i++)
  {
    string[] parts = lines[i].Split(',');
    string ownerEmail = parts[0];
    string itemName = parts[1];
    string itemDescription = parts[2];

    Person owner = null;
    foreach (Person person in allUsers)
    {
      if (person.getEmail() == ownerEmail)
      {
        owner = person;
        break;
      }
    }

    if (owner != null)
    {
      Items newItem = new Items(itemName, itemDescription);
      owner.myItemsList.Add(newItem);

    }

  }
}







Person activeUser = null;
bool isRunning = true;
while (isRunning)
{
  try { Console.Clear(); } catch { print("\n---------------\n"); }
  paint(ConsoleColor.DarkYellow, "\nTo be able to trade you must have an account.\n\n[1] Login\n[2] Creat an account\n[3] Exit");

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
                foreach (Person person in allUsers)
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

            foreach (Person person in allUsers)
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
              paint(ConsoleColor.Red, "\nIncorrect Password or Email. \nPress enter to try again...\n");

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
              {
                Menu activeMenu = new Menu();
                bool personLogedin = true;
                while (personLogedin)
                {
                  switch (activeMenu)
                  {
                    case Menu.Main:
                      try { Console.Clear(); } catch { print("---------------"); }
                      paint(ConsoleColor.DarkYellow, $"\n-----Welcome to Trade World-----");
                      paint(ConsoleColor.DarkYellow, "_____________________\nMain menu:\n[1] Show my Items\n[2] Upload Item\n[3] Trade\n[4] Logout\n_____________________\n");
                      paint(ConsoleColor.DarkYellow, "\n► ", "sameline");

                      switch (input())
                      {
                        case "1":
                          activeMenu = Menu.ShowMyItems;
                          break;

                        case "2":
                          activeMenu = Menu.UploadItem;

                          break;

                        case "3":
                          activeMenu = Menu.Trade;
                          break;

                        case "4":
                          personLogedin = false;
                          break;

                        default:
                          print("please enter a valid option...");
                          break;

                      }
                      break;

                    case Menu.ShowMyItems:
                      try { Console.Clear(); } catch { print("---------------"); }

                      paint(ConsoleColor.DarkYellow, "\nItems in your inventory:");
                      if (activeUser.myItemsList.Count == 0)
                      {
                        print("Inventory Empty!");
                      }
                      else
                      {
                        foreach (Items item in activeUser.myItemsList)
                        {
                          print($"\n       Name: {item.Name}\nDescription: {item.Description}\n\n");
                        }
                      }
                      print("\nTo go to main menu press enter...");
                      input();
                      activeMenu = Menu.Main;
                      break;

                    case Menu.UploadItem:
                      try { Console.Clear(); } catch { print("---------------"); }
                      Console.Write("\nWrite the name of your item: ");
                      string itemName = input().ToLower().Trim().Replace(",", "");
                      Console.Write("\nWrite the description of your item: ");
                      string itemDescription = input().ToLower().Trim().Replace(",", "");

                      if (string.IsNullOrEmpty(itemName) || string.IsNullOrEmpty(itemDescription))
                      {
                        paint(ConsoleColor.Red, "\nItem name and description cannot be empty. Item upload failed!");

                      }
                      else
                      {
                        Items newItem = new Items(itemName, itemDescription);
                        activeUser.myItemsList.Add(newItem);
                        Console.Write("\nItem ");
                        paint(ConsoleColor.Green, $"{newItem.Name}", "sameline");
                        Console.Write(" has been successfully uploaded!\n");

                      }


                      print("\nTo go to main menu press enter...");
                      input();
                      activeMenu = Menu.Main;
                      break;

                    case Menu.Trade:
                      try { Console.Clear(); } catch { print("---------------"); }
                      paint(ConsoleColor.DarkYellow, "\n--- Trade Menu ---\n\n[1] Show all items avalible for trade \n[2] My requests \n[3] Others requests \n[4] Trade history \n[5] Back to Main Menu\n  ");
                      paint(ConsoleColor.DarkYellow, "\n► ", "sameline");
                      switch (input())
                      {

                        case "1":
                          activeMenu = Menu.TradeMarket;
                          break;

                        case "2":
                          activeMenu = Menu.MyRequests;
                          break;

                        case "3":
                          activeMenu = Menu.OthersRequest;
                          break;

                        case "4":
                          activeMenu = Menu.TradeHistory;
                          break;

                        case "5":
                          activeMenu = Menu.Main;
                          break;

                        case "6":
                          activeMenu = Menu.Logout;
                          break;

                        default:
                          print("Please enter a valid option...");
                          break;
                      }
                      break;
                    case Menu.TradeMarket:
                      try { Console.Clear(); } catch { print("---------------"); }
                      paint(ConsoleColor.DarkYellow, "\nItems avaliable for trading in trade world:");

                      paint(ConsoleColor.DarkYellow, $"\n--- Your Inventory {activeUser.Name} ---");
                      int localItemNumberActiveUser = 1;
                      foreach (Items currentItem in activeUser.myItemsList)
                      {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        print($"[{localItemNumberActiveUser}]    Name: {currentItem.Name}\nDescription: {currentItem.Description}\n");
                        localItemNumberActiveUser++;
                        Console.ResetColor();
                      }


                      int userIndex;
                      List<Person> tradeableUsers = new List<Person>();

                      foreach (Person otherUser in allUsers)
                      {
                        if (otherUser.myItemsList?.Count > 0 && otherUser != activeUser)
                        {
                          paint(ConsoleColor.DarkYellow, $"\n--- [{tradeableUsers.Count + 1}] {otherUser.Name}'s Inventory ---");
                          tradeableUsers.Add(otherUser);
                          int localItemNumber = 1;
                          foreach (Items currentItem in otherUser.myItemsList)
                          {
                            print($"[{localItemNumber}]    Name: {currentItem.Name}\nDescription: {currentItem.Description}\n");
                            localItemNumber++;
                          }
                        }

                      }

                      userIndex = tradeableUsers.Count;
                      if (tradeableUsers.Count == 0 && activeUser.myItemsList.Count == 0)
                      { print("There are no items currently avalible for trade."); }


                      if (activeUser.myItemsList.Count == 0)
                      {
                        paint(ConsoleColor.Red, "\nYou must upload at least one item before sending a trade request");
                        print("\nPress enter to go to previous menu...");
                        activeMenu = Menu.Trade;
                        break;
                      }
                      if (tradeableUsers.Count == 0)
                      {
                        paint(ConsoleColor.Red, "\nNo other users have items to trade right now.");
                        print("\nPress enter to go to previous menu...");
                        input();
                        activeMenu = Menu.Trade;
                        break;
                      }


                      paint(ConsoleColor.DarkYellow, "\n--- Trade Selection --\n");
                      paint(ConsoleColor.DarkYellow, $"Write **number** for the person you want to trade with (1-{userIndex}): ", "sameline");
                      if (!int.TryParse(input(), out int targetUserNumber) || targetUserNumber > userIndex || targetUserNumber < 1)
                      {

                        paint(ConsoleColor.Red, "\nInvalid person number.");
                        print("\nPress enter to continue");
                        input();
                        activeMenu = Menu.Trade;
                        break;
                      }


                      Person targetOwner = tradeableUsers[targetUserNumber - 1];

                      int maxItemIndex = targetOwner.myItemsList.Count;
                      paint(ConsoleColor.DarkYellow, $"\nYou selected {targetOwner.Name}'s inventory.\nNow, which of their items (1-{maxItemIndex}) do you want to trade for:  ", "sameline");
                      if (!int.TryParse(input(), out int targetItemIndex) || targetItemIndex < 1 || targetItemIndex > maxItemIndex)
                      {
                        paint(ConsoleColor.Red, "\nInvalid item number.");
                        print("\nPress enter to continue");
                        input();
                        activeMenu = Menu.TradeMarket;
                        break;
                      }
                      Items targetItem = targetOwner.myItemsList[targetItemIndex - 1];

                      Console.Write("\nYou selected :");
                      paint(ConsoleColor.DarkYellow, $"{targetItem.Name}", "sameline");
                      paint(ConsoleColor.DarkYellow, $"\n\nNow, which of you items will you offer? Enter 1-{activeUser.myItemsList.Count}: ");

                      if (!int.TryParse(input(), out int offerIndex) || offerIndex < 1 || offerIndex > activeUser.myItemsList.Count)
                      {
                        paint(ConsoleColor.Red, "Invalid offer item number.");
                        print("\nPress enter to return to the Trade Market...");
                        input();
                        activeMenu = Menu.TradeMarket;
                        break;
                      }
                      Items offerItem = activeUser.myItemsList[offerIndex - 1];

                      TradeRequest request = new TradeRequest(requester: activeUser, requesterItem: offerItem, owner: targetOwner, ownerItem: targetItem);
                      allRequests.Add(request);

                      Console.Write("\nTrade request sent! Offering your:");
                      paint(ConsoleColor.DarkYellow, $" [{offerItem.Name}] ", "sameline");
                      Console.Write("for");
                      paint(ConsoleColor.DarkYellow, $" [{targetItem.Name}]");
                      Console.Write("from");
                      paint(ConsoleColor.DarkYellow, $" [{targetOwner.Name}]");





                      paint(ConsoleColor.DarkYellow, "\nTo go to Trade menu, press enter...");
                      input();
                      activeMenu = Menu.Trade;
                      break;

                    case Menu.MyRequests:
                      try { Console.Clear(); } catch { print("---------------"); }
                      List<TradeRequest> myPendingRequests = new List<TradeRequest>();
                      foreach (TradeRequest myRequests in allRequests)
                      {
                        if (myRequests.Requester == activeUser && myRequests.Status == TradeStatus.Pending)
                        {
                          myPendingRequests.Add(myRequests);
                        }

                      }
                      if (myPendingRequests.Count == 0)
                      {
                        print("\nYou have no pending requests sent by you.");
                      }
                      else
                      {
                        int outgoingReqIndex = 1;
                        paint(ConsoleColor.DarkYellow, $"\n--- My pending outgoing requests ({myPendingRequests.Count}) ---");
                        foreach (TradeRequest pendingRequest in myPendingRequests)
                        {
                          print($"\n[{outgoingReqIndex}] Request to {pendingRequest.Owner.Name}: Offering {pendingRequest.RequesterItem.Name} for their {pendingRequest.OwnerItem.Name}. \nStatus: {pendingRequest.Status}");
                          outgoingReqIndex++;
                        }
                      }


                      print("\nTo go to main menu press enter...");
                      input();
                      activeMenu = Menu.Trade;
                      break;

                    case Menu.OthersRequest:
                      try { Console.Clear(); } catch { print("---------------"); }

                      List<TradeRequest> pendingRequestsToMe = new List<TradeRequest>();
                      foreach (TradeRequest currentRequest in allRequests)
                      {
                        if (currentRequest.Owner == activeUser && currentRequest.Status == TradeStatus.Pending)
                        {
                          pendingRequestsToMe.Add(currentRequest);
                        }
                      }
                      if (pendingRequestsToMe.Count == 0)
                      {
                        print("You have no pending trade requests...");
                        print("\nPress enter to go to previous menu.");
                        input();
                        activeMenu = Menu.Trade;
                        break;
                      }

                      paint(ConsoleColor.DarkYellow, $"\n --- Incoming Trade Request ({pendingRequestsToMe.Count} pedning) ---");

                      int requestNumber = 1;
                      foreach (TradeRequest pendingRequest in pendingRequestsToMe)
                      {
                        print($"\n[{requestNumber}] From: {pendingRequest.Requester.Name}");
                        print($"    Wants: {pendingRequest.OwnerItem.Name} (your item)");
                        print($"    Offers: {pendingRequest.RequesterItem.Name} ");
                        requestNumber++;
                      }

                      paint(ConsoleColor.DarkYellow, "\nWrite the *number* for the request you want to ACCEPt or Deny, or press enter to go back: ", "sameline");

                      string inputChoice = input();

                      if (string.IsNullOrWhiteSpace(inputChoice))
                      {
                        paint(ConsoleColor.Red, "Invalid input, Press enter to continue...");
                        activeMenu = Menu.Trade;
                        break;
                      }

                      if (int.TryParse(inputChoice, out int choiceIndex) && choiceIndex >= 1 && choiceIndex <= pendingRequestsToMe.Count)
                      {
                        TradeRequest selectedRequest = pendingRequestsToMe[choiceIndex - 1];
                        try { Console.Clear(); } catch { print("---------------"); }

                        if (selectedRequest.Status != TradeStatus.Pending || selectedRequest.Owner != activeUser)
                        {
                          paint(ConsoleColor.Red, "Cannot process request (either status is not pending or you are not the owner).");
                          print("Press enter to go to previous menu...");
                          input();
                          activeMenu = Menu.Trade;
                          break; ;
                        }

                        paint(ConsoleColor.DarkYellow, "\nPlease select an option:");
                        paint(ConsoleColor.DarkYellow, "\n[1] Accept \n[2] Deny \n[3] Previous menu");
                        paint(ConsoleColor.DarkYellow, "\n► ", "sameline");
                        switch (input())
                        {
                          case "1":
                            selectedRequest.Status = TradeStatus.Accepted;
                            paint(ConsoleColor.Green, "Let's gooooo, Trade completeted succesfully!");

                            Person requester = selectedRequest.Requester;

                            activeUser.myItemsList.Remove(selectedRequest.OwnerItem);
                            requester.myItemsList.Add(selectedRequest.OwnerItem);


                            requester.myItemsList.Remove(selectedRequest.RequesterItem);
                            activeUser.myItemsList.Add(selectedRequest.RequesterItem);

                            Items TradeItem1 = selectedRequest.OwnerItem;
                            Items TradeItem2 = selectedRequest.RequesterItem;

                            foreach (TradeRequest pendingRequest in allRequests)
                            {

                              if (pendingRequest.Status == TradeStatus.Pending && pendingRequest != selectedRequest)
                              {
                                if (pendingRequest.OwnerItem == TradeItem1 || pendingRequest.OwnerItem == TradeItem2 || pendingRequest.RequesterItem == TradeItem1 || pendingRequest.RequesterItem == TradeItem2)
                                {
                                  pendingRequest.Status = TradeStatus.Denied;
                                  paint(ConsoleColor.Red, $"\nConflicting request denined:  {pendingRequest.Requester.Name} for {pendingRequest.OwnerItem.Name}");
                                }
                              }
                            }


                            print("Press enter to go to previous menu...");
                            input();
                            break;

                          case "2":
                            selectedRequest.Status = TradeStatus.Denied;
                            paint(ConsoleColor.Red, "Trade requst denied.");

                            print("Press enter to go to previous menu...");
                            input();
                            break;

                          case "3":
                            break;

                          default:
                            print("Invalid selection, Press enter to go to previous menu...");
                            input();
                            break;
                        }

                      }
                      else
                      {
                        paint(ConsoleColor.Red, "Invalid selection..");
                        input();
                        activeMenu = Menu.OthersRequest;
                        break;
                      }

                      break;

                    case Menu.TradeHistory:
                      try { Console.Clear(); } catch { print("---------------"); }
                      List<TradeRequest> approvedTrades = new List<TradeRequest>();
                      List<TradeRequest> deniedTrades = new List<TradeRequest>();

                      foreach (TradeRequest trades in allRequests)
                      {
                        if (trades.Status == TradeStatus.Accepted)
                        {
                          approvedTrades.Add(trades);
                        }
                        if (trades.Status == TradeStatus.Denied)
                        {
                          deniedTrades.Add(trades);
                        }
                      }
                      int acceptedIndex = 1;
                      paint(ConsoleColor.Green, $"\n--- [{approvedTrades.Count}] Accepted Trades ---");
                      if (approvedTrades.Count != 0)
                      {
                        foreach (TradeRequest trade in approvedTrades)
                        {
                          print($"[{acceptedIndex}] Your {trade.RequesterItem.Name} for {trade.Owner.Name}'s, {trade.OwnerItem.Name}");
                          acceptedIndex++;
                        }

                      }
                      else
                      {
                        print("You have no accepted trades.");
                      }


                      paint(ConsoleColor.Red, $"\n--- [{deniedTrades.Count}] Denied Trades ---");
                      int deniedIndex = 1;
                      if (deniedTrades.Count != 0)
                      {
                        foreach (TradeRequest trade in deniedTrades)
                        {
                          print($"[{deniedIndex}] Your {trade.OwnerItem.Name} for {trade.Requester.Name}'s, {trade.RequesterItem.Name}");
                          deniedIndex++;
                        }

                      }
                      else
                      {
                        print("You have no denied trades.");
                      }



                      print("\nTo go to main menu press enter...");
                      input();
                      activeMenu = Menu.Trade;
                      break;

                    case Menu.Logout:
                      activeMenu = Menu.Logout;
                      break;

                    default:
                      print("\nPlease enter a valid option...");
                      break;
                  }
                }

              }
              break;
          }
          activeUser = null;
          loginRunning = false;

        }

      }

      break;

    case "2":
      try { Console.Clear(); } catch { print("\n---------------\n"); }
      paint(ConsoleColor.DarkYellow, "\nEnter your Name: ", "sameline");
      string userName = input().Trim();
      while (string.IsNullOrEmpty(userName))
      {
        print("\nPlease enter a valid Name.");
        paint(ConsoleColor.DarkYellow, "\nEnter your Name: ", "sameline");
        userName = input().Trim();
      }

      paint(ConsoleColor.DarkYellow, "\nEnter your Email: ", "sameline");
      string userEmail = input().Trim();
      while (string.IsNullOrEmpty(userEmail) || !userEmail.Contains('@'))
      {
        print("\nPlease enter a valid Email.");
        paint(ConsoleColor.DarkGray, "Ex: name@mail.com");
        paint(ConsoleColor.DarkYellow, "\nEnter your Email: ", "sameline");
        userEmail = input().Trim();
      }

      foreach (Person user in allUsers)
      {
        if (user.Email == userEmail)
        {
          paint(ConsoleColor.Red, $"\nAccount already exists for {userEmail}. Please login instead.");
          print($"This is you default password: {user.Name}");
          print("\nPress enter to continue...");
          input();
          break;
        }
      }
      paint(ConsoleColor.DarkYellow, "\nEnter your Password: ", "sameline");
      string user_password = input().Trim();
      while (string.IsNullOrEmpty(userName))
      {
        print("\nPlease enter a valid Password.");
        paint(ConsoleColor.DarkYellow, "\nEnter your Password: ", "sameline");
        user_password = input().Trim();
      }

      Person newPerson = new Person(userName, userEmail, user_password);
      allUsers.Add(newPerson);
      paint(ConsoleColor.Green, $"\nCongurgulations! {userName} you have succesfully created a Trading account.");
      print("\nPress enter to go to login menu...");
      input();
      break;

    case "3":
      paint(ConsoleColor.DarkYellow, "\nSaving all data before closing...\n");
      isRunning = false;
      break;


    default:
      print("\nPlease enter a valid option...");
      break;
  }

}
SaveUsersToCsv("users.csv");
SaveItemsToCsv("items.csv");
SaveTradesToCsv("trades.csv");
paint(ConsoleColor.Green, "All data saved. Goodbye!");