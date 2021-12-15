Feature: Gmail Account
	In order to create a gmail account
	I want to creat new mail account

@test
Scenario: Create a Gmail Account
	Given I in gmail site
	And I click at create acccount
	And I input the data
	And Confirm the data
	When I press continue
	Then the new gmail account must be create
