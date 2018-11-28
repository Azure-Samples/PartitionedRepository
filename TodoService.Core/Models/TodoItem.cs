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
