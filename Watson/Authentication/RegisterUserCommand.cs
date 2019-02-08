using System;
using CQRSlite.Commands;

namespace Watson.Authentication
{
    public class RegisterUserCommand : ICommand
    {
        public Guid UserId { get; set; }
    }
}