using System;
using CQRSlite.Commands;

namespace Watson.Domain.RegisterUser
{
    public class RegisterUserCommand : ICommand
    {
        public Guid UserId { get; set; }
    }
}