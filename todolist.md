# Todo list
- Move to https
- Prepare linux server to host api
- Configure automatic deployment when new release pushed on git (appveyor)
- Error translation ? client side vs server side ?
- Nancy Validate model ? (missing fields)
- Assert that published events are processed in the same order (multithreading issue)
- Implement a custom IEventPublisher and publish events in EventStore implementation not in Repo (careful ioc).
- check port listening in command line execution. (5000 + 5001)