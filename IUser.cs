namespace App;

public interface IUser
{
  public bool tryLogin(string email, string password);
}