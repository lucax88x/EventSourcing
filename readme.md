# EventSourcing in F\#

## WHAT

Sample implementation of the EventSourcing "SimplestPossibleThing" example of Greg Young [link](https://github.com/gregoryyoung/m-r)

## HOW

- C# for the Infrastructure part
- F# for the Domain level
- Cassandra as db

## WHY

I wanted to study F# and to give a try to Cassandra

## REMARKS

- I attempted to also have the Application level done with F#, but this [missing feature](https://github.com/fsharp/fslang-suggestions/issues/545) prevented me to do so
- there is no application, just a couple of really simple tests
- If you need to know how to run a Cassandra, you can just do 

```bash
 docker run -p 9042:9042 --rm --name cassandra -d cassandra
```