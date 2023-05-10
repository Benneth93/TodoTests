Feature: Todo
	Create a todo that can be updated, or deleted

@EndToEnd
Scenario: Create A Todo
	Given I have navigated to the todo page
		And I click on the New Todo button
	When I enter a title in the title box
		And I enter a description in the description box
	And I click the save button
	Then The new todo will have saved correctly to the database
		And The new todo exists on the webpage
	When I click on the edit button
		And I enter details on the update model
		And I click the save button
	Then The updated todo will be saved to the database
	When Click the delete button
	Then the todo should no longer exist on the web page
		And the todo should not exist in the database
