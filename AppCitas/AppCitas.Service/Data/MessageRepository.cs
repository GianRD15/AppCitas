using AppCitas.Service.DTOs;
using AppCitas.Service.Entities;
using AppCitas.Service.Helpers;
using AppCitas.Service.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace AppCitas.Service.Data;

public class MessageRepository : IMessageRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public MessageRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
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

    public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
    {
        var query = _context.Message.OrderByDescending(n => n.DateSent).AsQueryable();
        query = messageParams.Container.ToLower() switch
        {
            "inbox" => query.Where(u => u.Recipient.UserName.Equals(messageParams.Username)),
            "outbox" => query.Where(u => u.Sender.UserName.Equals(messageParams.Username)),
            _ => query.Where(u => u.Recipient.UserName.Equals(messageParams.Username) && u.DateRead == null)
        };

        var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);
            
            return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
    }

    public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername)
    {
        var messages = await _context.Message
            .Include(u => u.Sender). ThenInclude(p => p.Photos)
            .Include(u => u.Recipient).ThenInclude(p => p.Photos)
            .Where(m => m.Recipient.UserName.Equals(currentUsername)
                    && m.SenderUsername.Equals(recipientUsername)
                    || m.Recipient.UserName.Equals(recipientUsername)
                    && m.SenderUsername.Equals(currentUsername))
            .ToListAsync();
        
        var unreadMessgaes = messages
            .Where(m => m.DateRead == null
                    && m.Recipient.UserName.Equals(currentUsername)).ToList();

        if (unreadMessgaes.Any())
        {
            foreach(var message in unreadMessgaes)
            {
                message.DateRead = DateTime.Now;
            }
            await _context.SaveChangesAsync();
        }

        return _mapper.Map<IEnumerable<MessageDto>>(messages);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}