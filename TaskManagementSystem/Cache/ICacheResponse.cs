using TaskManagementSystem.Models;

namespace TaskManagementSystem.Cache
{
    public interface ICacheResponse
    {
        public void Add(string userId, LoginResponse loginResponse);
        public void Remove(string userId);
        public LoginResponse Get(string userId);
    }
}
