// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.using System

namespace TodoService.Core.Models
{
	public class TodoItem : Entity
	{
        /// <summary>
        /// TodoItem item name
        /// </summary>
        /// <example>Grocery</example>
        public string Name { get; set; }

        /// <summary>
        /// TodoItem item Description
        /// </summary>
        /// <example>Pick Bread</example>
        public string Description { get; set; }
   
        /// <summary>
        /// TodoItem item category
        /// </summary>
        /// <example>Shopping</example>
        public string Category { get; set; }
	}
}
