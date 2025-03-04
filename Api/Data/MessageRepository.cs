using Microsoft.EntityFrameworkCore;
using Share.Model;

namespace View.Data;

public class MessageRepository(ApplicationDbContext context) : IMessageRepository
{
   
   public async Task<List<Message>> GetMessages(int pageNumber, int pageSize)
   {
      return await context.Messages
         .OrderBy(m => m.Timestamp)
         .Skip((pageNumber - 1) * pageSize) 
         .Take(pageSize) 
         .ToListAsync();
   }

   public async Task<Message> AddMessage(Message message)
   {
      context.Messages.Add(message);
      await context.SaveChangesAsync();
      return message;
   }
   
}