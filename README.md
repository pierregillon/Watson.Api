[![Build status](https://ci.appveyor.com/api/projects/status/uw4n1wgl996vdm4h?svg=true)](https://ci.appveyor.com/project/pierregillon/watson-api)

# What is Watson ?
Watson is a collaborative web media fact checker.

# This is an API. Do you have client side app ?
For now, the client applications are browser extensions :
* [For Chrome](https://github.com/pierregillon/Watson.Pluggins.Chrome)

# How the API is structured ?
The api is built using Command Query Response Segregation (CQRS), Domain Driven Design (DDD) and Event Sourcing.

# Libraries
* [CQRSLite](https://github.com/gautema/CQRSlite)

# Features
- Highlight text in a web page and report it

# In progress
As a member of the Watson fact checker community, I must be able to
- Link 2 facts
- Qualify a fact relation (confirm => infirm)
- List all related fact of a fact
- Vote a suspected fact is interesting and further investigation needed
- Vote a fact is not a fact and must be removed
- Qualify document (scientific, politic, non professional blog) to estimate quality and pertinence
- Hide unimportant words in a fact, replaced by [...]

# Technical
- Rework DetectSuspiciousFact command properties names

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