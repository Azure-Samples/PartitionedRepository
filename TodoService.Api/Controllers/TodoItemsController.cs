// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.using System;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoService.Core.Exceptions;
using TodoService.Core.Interfaces;
using TodoService.Core.Models;

namespace TodoService.Api.Controllers
{
    [ApiVersion("1")]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoItemRepository _repo;

        public TodoItemsController(ITodoItemRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Creating a new TodoItem Item
        /// </summary>
        /// <param name="newTodoItem"> JSON New TodoItem document</param>
        /// <returns>Returns the new TodoItem Id </returns>
        /// <returns>Returns 201 Created success</returns>
        /// <returns>Returns 400 Bad Request error</returns>
        /// <returns>Returns 500 Internal Server Error </returns>
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult> CreateItem([FromBody] TodoItem newTodoItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var toDo = await _repo.AddAsync(newTodoItem);

            return Ok(toDo);
        }

        /// <summary>
        /// Retrieving a TodoItem using its TodoItem Id
        /// </summary>
        /// <remarks>
        /// Retrieves a TodoItem using its TodoItem Id
        /// </remarks>
        /// <param name="toDoId">The Id of the TodoItem item to be retrieved</param>
        /// <returns>Returns the full TodoItem document </returns>
        /// <returns>Returns 200 OK success </returns>
        /// <returns>Returns 404 Not Found error</returns>
        /// <returns>Returns 500 Internal Server Error </returns>
        [HttpGet("{todoId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TodoItem))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetItem(string toDoId)
        {
            try
            {
                var toDo = await _repo.GetByIdAsync(toDoId);
                return Ok(toDo);
            }
            catch (EntityNotFoundException)
            {
                return NotFound(toDoId);
            }
        }


        /// <summary>
        /// Updating TodoItem item 
        /// </summary>
        /// <remarks>Updates an existing item </remarks>
        /// <param name="toDoId">Id of an existing TodoItem that needs to be updated</param>
        /// <param name="updatedItem">JSON TodoItem document to be updated in an existing TodoItem</param>
        /// <returns>Returns 200 OK success</returns>
        /// <returns>Returns 400 Bad Request error</returns>
        /// <returns>Returns 404 Not Found error</returns>
        /// <returns>Returns 500 Internal Server Error </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{todoId}")]
        public async Task<ActionResult> UpdateItem(string toDoId,  [FromBody]TodoItem updatedItem)
        {
            if (updatedItem.Id != toDoId)
            {
                return BadRequest(updatedItem.Id);
            }

            try
            {
                if (toDoId == null)
                {
                    return NotFound(toDoId);
                }
                
                await _repo.UpdateAsync(updatedItem);
                return Ok();
            }
            catch (EntityNotFoundException)
            {
                return NotFound(toDoId);
            }
        }

        /// <summary>
        /// Deleting an existing TodoItem
        /// </summary>
        /// <remarks>Deletes an existing TodoItem item list</remarks>
        /// <param name="toDoId"> Id of an existing TodoItem that needs to be deleting</param>
        /// <returns>Returns 204 No Content success</returns>
        /// <returns>Returns 404 Not Found error</returns>
        /// <returns>Returns 500 Internal Server Error </returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{toDoId}")]
        public async Task<ActionResult> RemoveItem(string toDoId)
        {
            try
            {
                var toDoToUpdate = await _repo.GetByIdAsync(toDoId);

                if (toDoToUpdate == null)
                {
                    return NotFound(toDoId);
                }


                await _repo.DeleteAsync(toDoToUpdate);

                return NoContent();
            }
            catch (EntityNotFoundException)
            {
                return NotFound(toDoId);
            }
        }

        
    }
}
