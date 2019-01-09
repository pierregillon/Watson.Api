[![Build status](https://ci.appveyor.com/api/projects/status/uw4n1wgl996vdm4h?svg=true)](https://ci.appveyor.com/project/pierregillon/watson-api)

# What is Watson ?
Watson is a collaborative web media fact checker.

# This is an API. Do you have client side app ?
For now, the client applications are browser extensions :
* [For Chrome](https://github.com/pierregillon/Watson.Pluggins.Chrome/blob/master/README.md)

# How the API is structured ?
The api is built using Command Query Response Segregation (CQRS), Domain Driven Design (DDD) and Event Sourcing.

# Libraries
* [CQRSLite](https://github.com/gautema/CQRSlite)

# In progress
As a member of the Watson fact checker community, I must be able to
- Indicate investigating fact count in the present web page
- Link 2 facts
- Qualify a fact relation (confirm => infirm)
- List all related fact of a fact
- Indicate a suspected fact is interesting and further investigation needed
- Indicate a fact is not a fact. Remove needed.
- Qualify document (scientific, politic, non professional blog) to estimate quality and pertinence
- Hide unimportant words in a fact, replaced by [...]

# Technical
- Refactor SuspiciousFactDetected to SuspiciousFactReported ? (Quid associated command)
- Rework DetectSuspiciousFact command properties names

# Domain event brain storming
Domain events brain storming :
* SuspiciousFactDetected
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