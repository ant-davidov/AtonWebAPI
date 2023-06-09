﻿using AronWebAPI.DTOs;
using AronWebAPI.Entites;

namespace AtonWebAPI.Interfaces
{
    public interface IUserRepository
    {
        public Task<bool> Add(User user, string password);
        public Task<bool> AddRole(User user, string role);
        public bool Delete(User user, string adminLogin);
        public Task<bool> Update(User user, UserUpdateDTO updateUser, string adminLogin);
        public Task<bool> UpdateLogin(User user, string newLogin, string adminLogin);
        public Task<bool> UpdatePassword(User user, string password, string newPassword, string adminLogin);
        public Task<List<User>> GetAllActiveUsers();
        public Task<User> GetByLogin(string login);
        public Task<User> GetByLoginAndPassword(string login, string password);
        public Task<bool> LoginIsFree(string login);
        public Task<List<User>> GetUserByAge(int age);
        public Task<bool> Recovery(User user);

    }
}
