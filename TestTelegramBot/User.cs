using System;

namespace TestTelegramBot
{
    public class User
    {
        public int Id { get; set; }
        public long UserIdentifier { get; set; }
        public long ChatId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegisterDate { get; set; }

        public User(long userIdentifier, long chatId, string userName, string firstName, string lastName, DateTime registerDate)
        {
            UserIdentifier = userIdentifier;
            ChatId = chatId;
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            RegisterDate = registerDate;
        }
    }
}
