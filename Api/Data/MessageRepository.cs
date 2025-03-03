using Microsoft.EntityFrameworkCore;
using Share.Model;

namespace View.Data;

public class MessageRepository(ApplicationDbContext context) : IMessageRepository
{
   
   public async Task<List<Message>> GetMessages()
   {
      return await context.Messages
         .OrderByDescending(m => m.Timestamp)
         .ToListAsync();
   }

   public async Task<Message> AddMessage(Message message)
   {
      context.Messages.Add(message);
      await context.SaveChangesAsync();
      return message;
   }
   
}