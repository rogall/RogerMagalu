﻿using Entities;
using Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Concrete
{
    public class AdminService : IAdminService
    {
        private MagaluDbContext _context;

        public AdminService(MagaluDbContext context)
        {
            _context = context;
        }

        public User Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = _context.Users.SingleOrDefault(x => x.UserName == username);

            // check if username exists
            if (user == null)
                return null;

            //// check if password is correct
            //if (!VerifyPasswordHash(password, user.PasswordHash))
            //    return null;

            // authentication successful
            return user;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users;
        }        

        public User CreateUser(User user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("Password is required");

            if (_context.Users.Any(x => x.UserName == user.UserName))
                throw new Exception("Username \"" + user.UserName + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = Convert.ToBase64String(passwordHash);           

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public void UpdateUser(User userParam, string password = null)
        {
            var user = _context.Users.Find(userParam.Id);

            if (user == null)
                throw new Exception("User não encontrado");

            if (userParam.Email != user.Email)
            {
                // username has changed so check if the new username is already taken
                if (_context.Users.Any(x => x.UserName == userParam.UserName))
                    throw new Exception("Email " + userParam.UserName + " já utilizado");
            }
            
            //user.LastName = userParam.LastName;
            //user.Username = userParam.Username;

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash.ToString();               
            }

            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public User GetUserById(string id)
        {
            return _context.Users.Find(id);
        }

        #region password methods
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Este campo não pode ser vazio.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Senha não pode ser nula ou vazia.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Tamanho inválido (64 bytes).", "passwordHash");
            
            
            return true;
        }
        #endregion
    }
}

