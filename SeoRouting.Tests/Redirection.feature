Feature: Redirection
	When matching but incorrectly formatted requests are made, they should be 301'd to the correct url.

Scenario Outline: Redirect
	Given a route url of <routeUrl>
	And ForceTrailingSlash is set to <fts>
	And ForceLowerCase set to <flc>
	When I request <requestUrl>
	Then I should be redirected to <redirectUrl>

Scenarios:
| routeUrl      | requestUrl   | redirectUrl   | fts   | flc   |
| test1/{value} | Test1/Test2  | /test1/Test2  | false | false |
| test1         | Test1        | /test1        | false | false |
| test1/test2   | Test1/Test2  | /test1/test2  | false | false |
| test1/{value} | Test1/Test2/ | /test1/Test2  | false | false |
| test1         | Test1/       | /test1        | false | false |
| test1/test2   | Test1/Test2/ | /test1/test2  | false | false |
| test1/{value} | Test1/Test2  | /test1/Test2/ | true  | false |
| test1         | Test1        | /test1/       | true  | false |
| test1/test2   | Test1/Test2  | /test1/test2/ | true  | false |
| Test1/{value} | Test1/Test2  | /test1/test2  | false | true  |
| Test1         | Test1        | /test1        | false | true  |
| Test1/Test2   | Test1/Test2  | /test1/test2  | false | true  |

Scenario Outline: No Redirect
	Given a route url of <routeUrl>
	And ForceTrailingSlash is set to <fts>
	And ForceLowerCase set to <flc>
	When I request <requestUrl>
	Then I should not be redirected

Scenarios:
| routeUrl      | requestUrl   | fts   | flc   |
| test1 | test1 | false | false |
| {value} | Test1 | false | false |
| test1 | test1/ | false | false |
| {value} | Test1/ | false | false |
| test1 | test1 | false | true |
| test1 | test1/ | false | true |
| {value} | test1 | false | true |
| {value} | test1/ | false | true |
| test1 | test1/ | true | false |
| {value} | Test1/ | true | false |
