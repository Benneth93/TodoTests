Feature: TodoAPI
	Creation and management of Todo Tasks

Scenario: Create a new Todo
	Given I have a new task to create
	When I send a request to create a new todo with the task details
	Then I should get a successful response
	And The response should contain an ID
	And The response should contain the correct details of the task
	
Scenario: Edit an existing Todo
	Given I have an existing todo
		And Data ready to edit that todo
	When I send the request to edit the todo
	Then The response should be successful
		And The details of the todo should contain the details of the edit