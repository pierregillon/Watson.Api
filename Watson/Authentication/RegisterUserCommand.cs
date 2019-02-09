using System;
using CQRSlite.Commands;

namespace Watson.Authentication
{
    public class LogUserInCommand : ICommand
    {
        public Guid UserId { get; set; }
    }
}