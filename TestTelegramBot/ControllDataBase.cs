using System;
using System.Collections.Generic;
using System.Linq;

namespace TestTelegramBot
{
    public class ControllDataBase : IControllDataBase
    {
        public ControllDataBase() { }
        public void Add(User user)
        {
            using(UserContext context = new UserContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        public bool CheckUser(long identify)
        {
            using(UserContext context = new UserContext())
            {
                User user = context.Users.FirstOrDefault(x => x.UserIdentifier == identify);

                if(user != null)
                    return true;
                else 
                    return false;
            }
        }

        public DateTime GetDate(long identify)
        {
            using (UserContext context = new UserContext())
            {
                return context.Users.FirstOrDefault(x => x.UserIdentifier == identify).RegisterDate;
            }
        }
        
        public List<long> GetChatId()
        {
            using (UserContext context = new UserContext())
            {
                List<long> chatId = new List<long>();
                foreach(var user in context.Users)
                {
                    chatId.Add(user.ChatId);
                }
                return chatId;
            }
        }
    }
}
