namespace App;

public class Person : IUser
{
  public string Email;
  public string _Password;


  public Person(string email, string _password)
  {
    Email = email;
    _Password = _password;
  }

  public bool tryLogin(string email, string password)
  {
    return email == Email && password == _Password;
  }
}