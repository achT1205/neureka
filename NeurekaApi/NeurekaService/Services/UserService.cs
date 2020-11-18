using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NeurekaDAL.Models;
using NeurekaDAL.Repositories;

namespace NeurekaService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> Get() => await _userRepository.Get();
        public async Task<User> Get(string id) => await _userRepository.Get(id);
        public async Task<User> Create(User user) => await _userRepository.Create(user);
        public async Task Update(string id, User user) => await _userRepository.Update(id, user);
        public async Task Remove(User user) => await _userRepository.Remove(user);
        public async Task Remove(string id) => await _userRepository.Remove(id);
        public async Task<bool> Authenticate(string email, string password) => await _userRepository.Authenticate(email, password);
        public async Task<User> GetUserByEmail(string email) => await _userRepository.GetUserByEmail(email);
        public async Task<bool> ChangePassword(string email, string oldPassword, string password) => await _userRepository.ChangePassword(email, oldPassword, password);
        public async Task<IEnumerable<User>> GetUsersByRole(string role) => await _userRepository.GetUsersByRole(role);
        public async Task<string> ResetPassword(string email) => await _userRepository.ResetPassword(email);
    }
}
