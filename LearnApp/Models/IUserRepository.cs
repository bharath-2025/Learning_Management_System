
using System.Collections;

namespace LearnApp.Models{
    public interface IUserRepository{
        
        public IEnumerable GetUsers();
        public void AddUser(User user);
        public void DeleteUser(string id);
        public bool ValidUser(User user);
        public string FetchRole(User user);
        public string FetchName(User user);

        public string FetchBatch(User user);

    }
}