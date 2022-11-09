using AppCitas.Service.DTOs;
using AppCitas.Service.Entities;
using AppCitas.Service.Helpers;
using AppCitas.Service.Interfaces;

namespace AppCitas.Service.Data;

public class MessageRepository : IMessageRepository
{
    private readonly DataContext _context;

    public MessageRepository(DataContext context)
    {
        _context = context;
    }

    public void AddMessage(Message message)
    {
        _context.Message.Add(message);
    }

    public void DeleteMessage(Message message)
    {
        _context.Message.Remove(message);
    }

    public async Task<Message> GetMessage(int id)
    {
        return await _context.Message.FindAsync(id);
    }

    public Task<PagedList<MessageDto>> GetMessagesForUser()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<MessageDto>> GetMessageThread(int currentUserId, int recipientId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}