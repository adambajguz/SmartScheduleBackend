﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartSchedule.Application.Event.Commands.CreateEvent;
using SmartSchedule.Application.Event.Commands.DeleteEvent;
using SmartSchedule.Application.Event.Commands.UpdateEvent;
using SmartSchedule.Application.Event.Queries.GetEventList;

namespace SmartSchedule.Api.Controllers
{
    public class EventController : BaseController
    {
        [HttpPost("/api/CreateEvent")]
        public async Task<IActionResult> CreateEvent([FromBody]CreateEventCommand eventCommand)
        {
            return Ok(await Mediator.Send(eventCommand));
        }

        [HttpPost("/api/UpdateEvent")]
        public async Task<IActionResult> UpdateEvent([FromBody]UpdateEventCommand eventCommand)
        {
            return Ok(await Mediator.Send(eventCommand));
        }

        [HttpPost("/api/DeleteEvent")]
        public async Task<IActionResult> DeleteEvent([FromBody]DeleteEventCommand eventCommand)
        {
            return Ok(await Mediator.Send(eventCommand));
        }

        [HttpGet("/api/events")]
        public async Task<IActionResult> GetCalendarsList()
        {
            return Ok(await Mediator.Send(new GetEventListQuery()));
        }

    }
}