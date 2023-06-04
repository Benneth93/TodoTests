Feature: TodoAPI
	Creation and management of Todo Tasks

Scenario: Create a new Todo
	Given I have a new task to create
	When I send a request to create a new todo with the task details
	Then I should get a successful response
	And The response should contain an ID
	And The response should contain the correct details of the task