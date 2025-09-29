namespace App;

public class Items
{
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
  public string Name;
  public string Description;
  public string Id;

  public Items(string name, string description, string id)
  {
    Name = name;
    Description = description;
    Id = id;
  }


}