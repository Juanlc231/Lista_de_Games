using Lista_de_Games.Models;

namespace Lista_de_Games.ViewModel
{
    public class UsersViewModel
    {
        public List<Users> UsersList { get; set; }
        public List<Games> Games { get; set; }
        public Users User { get; set; }

        public UsersViewModel(List<Games> games,List<Users> userList,Users user)
        {
            this.Games = games;
            this.UsersList = userList;
            this.User = user;
        }
    }
}
