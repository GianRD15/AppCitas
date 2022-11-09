﻿using AppCitas.Service.DOTs;
using AppCitas.Service.DTOs;
using AppCitas.Service.Entities;
using AppCitas.Service.Extensions;
using AppCitas.Service.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AppCitas.Service.Controllers;

[Authorize]
public class MessagesController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly IMapper _mapper;

    public MessagesController(IUserRepository userRepository, IMessageRepository messageRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _messageRepository = messageRepository; 
    }

    [HttpPost]
    public async Task<ActionResult<MessageDto>> CreateMessage(MessageCreateDto messageCreateDto)
    {
        var username = User.GetUsername();

        if (username.ToLower().Equals(messageCreateDto.RecipientUsername.ToLower()))
            return BadRequest("You cannot send messages to yourself");

        var sender = await _userRepository.GetUserByUsernameAsync(username);
        var recipient = await _userRepository.GetUserByUsernameAsync(messageCreateDto.RecipientUsername);

        if (recipient == null) return NotFound();

        var message = new Message
        {
            Sender = sender,
            Recipient = recipient,
            SenderUserName = sender.UserName,
            RecipientUsername = recipient.UserName,
            Content = messageCreateDto.Content
        };

        _messageRepository.AddMessage(message);
        if (await _messageRepository.SaveAllAsync())
            return Ok(_mapper.Map<MessageDto>(message));

        return BadRequest("Failed to send the message");
    }
}
