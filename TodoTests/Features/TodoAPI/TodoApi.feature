Feature: TodoAPI
	Creation and management of Todo Tasks

@CleardownTodo
Scenario: Create a new Todo
	Given I have a new task to create
	When I send a request to create a new todo with the task details
	Then I should get a successful response
		And The response should contain an ID
		And The response should contain the correct details of the task
		
@CleardownTodo
Scenario: Edit an existing Todo
	Given I have an existing todo
		And Data ready to edit that todo
	When I send the request to edit the todo
	Then The response should be successful from the edit
		And The details of the todo should contain the details of the edit
		
Scenario: Delete an existing Todo
	Given I have an existing todo
	When I send a request to delete that todo
	Then I should get a successful response from the delete
		And the details of the todo deleted should be correct
		
Scenario Outline: Validation error messages for creation of a Todo
	Given I have a new task to create with <TitleLength> and <DescriptionLength>
	When I send a request to create a new todo with the task details
	Then I should get an unsuccessful response of <ResponseCode>
		And The response should contain an error message with '<TitleMessage>' and '<DescriptionMessage>'
	
	Examples:
	| TitleLength | DescriptionLength | ResponseCode | TitleMessage                                                                 | DescriptionMessage                                                   |
	| 2           | 10                | 400          | The field Title must be a string or array type with a minimum length of '3'. |                                                                      |
	| 10          | 505               | 400          |                                                                              | The field Description must be a string with a maximum length of 500. |
	| 1           | 505               | 400          | The field Title must be a string or array type with a minimum length of '3'. | The field Description must be a string with a maximum length of 500. |
	 