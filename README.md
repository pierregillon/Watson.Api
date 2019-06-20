[![Build status](https://ci.appveyor.com/api/projects/status/m63e9e23175jiju4?svg=true)](https://ci.appveyor.com/project/pierregillon/watson-api)

# What is Watson ?
Watson is a collaborative web fact checker. We all are tired to see false assumptions, fake news and data manipulation when browsing the Internet. The idea behind Watson is to create a community of anonymous inspectors, simple citizens like you and me, who chases and reports suspicious facts. Cooperation through collective intelligence let us distinct true facts and bad information.

## This is an _API_. Do you have _client side apps_ ?
For now, the client applications are browser extensions :
* [For Chrome](https://github.com/pierregillon/Watson.Pluggins.Chrome)
* For Firefox (Coming soon)
* For IE (Coming soon)

# Features
No production version yet. Wait for v1.0.

## Implemented (v0.1)
- [x] Report a suspicious fact
- [x] List facts of a web page
- [x] Jwt token authentication

## In progress
As a member of the Watson fact checker community, I must be able to
- [x] List facts when same page but slightly different url (some parameters may differ)
- [ ] Mark a fact as interesting and further investigation needed
- [ ] Mark a fact as "not a fact" and must be removed
- [ ] Link 2 facts
- [ ] Qualify a fact relation (confirm => infirm)
- [ ] List all related fact of a fact
- [ ] Qualify document (scientific, politic, non professional blog) to estimate quality and pertinence
- [ ] Hide unimportant words in a fact, replaced by [...]

Technical
- [ ] Infrastructure requirements to automatically deploy from CI
- [ ] RabbitMQ for event publishing

# Development
Let's talk here about technical details. You might be interested of this section if you want to run the code on your machine.

## How the API is built ?
The api is built using .NET Core and following architecture patterns : 
- [Command Query Response Segregation (CQRS)](https://www.martinfowler.com/bliki/CQRS.html)
- [Domain Driven Design (DDD)](https://domainlanguage.com/ddd/)
- [Event Sourcing](https://martinfowler.com/eaaDev/EventSourcing.html)

## Main libraries
* [CQRSLite](https://github.com/gautema/CQRSlite) : light library for DDD and CQRS programming
* [Nancy](https://github.com/NancyFx/Nancy) : web hosting
* [jose-jwt](https://github.com/dvsekhvalnov/jose-jwt) : jwt token encryption
* [ElasticSearch.Net](https://github.com/elastic/elasticsearch-net) : client to ElasticSearch DB
* [EventStore.ClientAPI.NetCore](https://github.com/EventStore/EventStore/tree/master/src/EventStore.ClientAPI) : client to EventStore DB (the stream-oriented database optimised for event sourcing)

## Installing & Executing
0. Make sure .NET Core SDK is installed on your environment (dotnet command line tool)

1. Install dependencies
```
dotnet restore
```

2. Build
```
dotnet build
```

3. Run
```
dotnet run --project Watson/watson.csproj
```

4. Publish and run
Alternatively, you can publish the project and execute it.
```
dotnet publish Watson/Watson.csproj
cd Watson/bin/[Debug, Release]/netcoreapp2.2/publish
dotnet exec Watson.dll
```
## Running the tests
```
dotnet test
```
The tests run with xUnit.

# Versioning
The project use [SemVer](http://semver.org/) for versioning. For the versions available, see [the tags on this repository](https://github.com/pierregillon/Watson.Api/releases).

# License
This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

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
