[![Build status](https://ci.appveyor.com/api/projects/status/uw4n1wgl996vdm4h?svg=true)](https://ci.appveyor.com/project/pierregillon/watson-api)

# What is Watson ?
Watson is a collaborative web fact checker.

# This is an API. Do you have client side app ?
For now, the client applications are browser extensions :
* [For Chrome](https://github.com/pierregillon/Watson.Pluggins.Chrome)

# How the API is structured ?
The api is built using Command Query Response Segregation (CQRS), Domain Driven Design (DDD) and Event Sourcing.

# Libraries
* [CQRSLite](https://github.com/gautema/CQRSlite)

# Features v1.0
- [DONE] Report a suspicious fact
- [DONE] List facts of a web page
- [DONE] Jwt token authentication
- [IN PROGRESS] TS : Infrastructure requirements

# Next features
As a member of the Watson fact checker community, I must be able to
- Mark a fact as interesting and further investigation needed
- Mark a fact as "not a fact" and must be removed
- Link 2 facts
- Qualify a fact relation (confirm => infirm)
- List all related fact of a fact
- Qualify document (scientific, politic, non professional blog) to estimate quality and pertinence
- Hide unimportant words in a fact, replaced by [...]

# Todo list
- Move to https
- Prepare linux server to host api
- Configure automatic deployment when new release pushed on git (appveyor)
- Error translation ? client side vs server side ?
- Nancy Validate model ? (missing fields)
- Assert that published events are processed in the same order (multithreading issue)
- Implement a custom IEventPublisher and publish events in EventStore implementation not in Repo (careful ioc).
- check port listening in command line execution. (5000 + 5001)

# Domain event brain storming
Domain events brain storming :
* ReportSuspiciousFact
    * FactId
    * Wording
    * Location
        * WebPageUrl
        * Xpath
    * UserId
* RelatedFactAdded
    * FactId
    * RelatedFactId
    * UserId
* FactRelationQualified
    * FactId
    * RelatedFactId
    * UserId
    * Fact qualification
        * Confirm completely
        * Confirm partially
        * Infirm partially
        * Infirm completely
        * Not a fact
* FactUpVoted
    * FactId
    * UserId
* FactDownVoted
    * FactId
    * UserId